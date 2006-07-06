namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.IO;
	using System.Text;
	using System.Xml;
		
	using NUnit.Framework;

	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;
	
	[TestFixture]
	public class TestAdapter {
		private string ruleFilesFolder;
		private string outFile;
		private string outFileHRF;

		public TestAdapter() {
			ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
		}
		
		private static void CleanAndNormalize(XmlDocument document) {
			XmlNodeList comments = document.SelectNodes("//comment()");
			foreach(XmlNode comment in comments) comment.ParentNode.RemoveChild(comment);
			document.Normalize();
		}
		
		public static bool AreSameXml(string fileName1, string fileName2) {
			XmlDocument doc1 = new XmlDocument();
			doc1.Load(fileName1);
			CleanAndNormalize(doc1);
			
			XmlDocument doc2 = new XmlDocument();
			doc2.Load(fileName2);
			CleanAndNormalize(doc2);
			
			// for now we compare only the size, which is pretty accurate
			return (doc1.OuterXml.Length == doc2.OuterXml.Length);
		}
		
		[SetUp]
		public void CleanOutputFiles() {
			outFile = Parameter.GetString("unittest.outputfolder") + "/outtest.ruleml";
			FileInfo fi = new FileInfo(outFile);
			if (fi.Exists) fi.Delete();
			
			outFileHRF = Parameter.GetString("unittest.outputfolder") + "/outtest.hrf";
			fi = new FileInfo(outFileHRF);
			if (fi.Exists) fi.Delete();
		}
	
		[Test][ExpectedException(typeof(BREException))]
		public void RuleMLImplicationLabelTagParserMissingLabel() {
			new RuleML086NafDatalogAdapter.ImplicationProperties("priority:25");
			Assert.Fail("Should never reach me!");
		}
	
		[Test]
		public void RuleMLImplicationLabelTagParserSimpleLabel() {
			string simpleLabel = "Simple Label";
		
			RuleML086NafDatalogAdapter.ImplicationProperties labelInfo = new RuleML086NafDatalogAdapter.ImplicationProperties(simpleLabel);
			Assert.AreEqual(simpleLabel, labelInfo.label, "Label");
		}
	
		[Test]
		public void RuleMLImplicationLabelTagParserComplexLabel() {
			string complexLabel = "label:Mutex C;priority:75;mutex:Mutex A;precondition:Precond A";
		
			RuleML086NafDatalogAdapter.ImplicationProperties labelInfo = new RuleML086NafDatalogAdapter.ImplicationProperties(complexLabel);
			Assert.AreEqual("Mutex C", labelInfo.label, "Label");
			Assert.AreEqual(75, labelInfo.priority, "Prority");
			Assert.AreEqual("Mutex A", labelInfo.mutex, "Mutex");
			Assert.AreEqual("Precond A", labelInfo.precondition, "Precondition");
		}
		
		[Test]
		public void RuleMLStreamSource() {
			string ruleBase = "<rulebase><fact><_head><atom><_opr><rel>regular</rel></_opr><ind>Honda</ind></atom></_head></fact></rulebase>";
			RuleML08DatalogAdapter adapter = new RuleML08DatalogAdapter(new MemoryStream(new UTF8Encoding().GetBytes(ruleBase)),
			                                                            FileAccess.Read);
			ArrayList facts = adapter.Facts;
			Assert.IsNotNull(facts, "Facts not null");
			Assert.AreEqual(1, facts.Count, "One fact expected");
			Assert.AreEqual("regular", ((Fact)facts[0]).Type, "Fact of type 'regular' expected");
		}

		[Test]
		public void RuleML08SaveRuleBase() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML08DatalogAdapter(ruleFilesFolder + "discount.ruleml", FileAccess.Read));
			ie.SaveRuleBase(new RuleML08DatalogAdapter(outFile, FileAccess.Write));

			// for now, let's reload the rulebase
			ie.LoadRuleBase(new RuleML08DatalogAdapter(outFile, FileAccess.Read));
		}
	
		[Test]
		public void RuleML086SaveRuleBase() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML086DatalogAdapter(ruleFilesFolder + "endlessloop.ruleml", FileAccess.Read));
			ie.SaveRuleBase(new RuleML086DatalogAdapter(outFile, FileAccess.Write));

			// for now, compare only the size
			Assert.IsTrue(AreSameXml(ruleFilesFolder + "endlessloop.ruleml", outFile), "Same XML");
		}
	
		[Test]
		public void RuleMLNaf086SaveRuleBase() {
			string inFile = ruleFilesFolder + "test-0_86.ruleml";
			
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML086NafDatalogAdapter(inFile, FileAccess.Read));
			ie.SaveRuleBase(new RuleML086NafDatalogAdapter(outFile, FileAccess.Write));

			// for now, compare only the size
			Assert.IsTrue(AreSameXml(inFile, outFile), "Same XML");
		}
	
		[Test]
		public void RuleMLNaf09SaveRuleBase() {
			string inFile = ruleFilesFolder + "test-0_9.ruleml";
			
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(inFile, FileAccess.Read));
			ie.SaveRuleBase(new RuleML09NafDatalogAdapter(outFile, FileAccess.Write));

			// for now, compare only the size
			Assert.IsTrue(AreSameXml(inFile, outFile), "Same XML: " + inFile + " and " + outFile);
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void RuleML086LoadFactsWithNoRuleBase() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadFacts(new RuleML086DatalogAdapter(ruleFilesFolder + "facts.ruleml", FileAccess.Read));
			Assert.Fail("Should never reach me!");
		}
	
		[Test]
		public void RuleML08LoadFacts() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML08DatalogAdapter(ruleFilesFolder + "discount.ruleml", FileAccess.Read));

			ie.Process();
			Assert.AreEqual(6, ie.FactsCount, "Initial process");

			ie.LoadFacts(new RuleML08DatalogAdapter(ruleFilesFolder + "facts.ruleml", FileAccess.Read));
			Assert.AreEqual(8, ie.FactsCount, "Loaded facts");

			ie.Process();
			Assert.AreEqual(10, ie.FactsCount, "Subsequent process");
		}
	
		[Test][ExpectedException(typeof(BREException))]
		public void RuleML086SaveFactsWithNoRuleBase() {
			IInferenceEngine ie = new IEImpl();
			ie.SaveFacts(new RuleML086DatalogAdapter(Parameter.GetString("unittest.outputfolder") + "/_outfacts.ruleml",
			                                         FileAccess.Write));
			Assert.Fail("Should never reach me!");
		}
		
		[Test]
		public void RuleML086SaveFacts() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML086NafDatalogAdapter(ruleFilesFolder + "test-0_86.ruleml", FileAccess.Read));
			
			ie.SaveFacts(new RuleML086NafDatalogAdapter(outFile, FileAccess.Write));

			IInferenceEngine ie2 = new IEImpl();
			ie2.LoadRuleBase(new RuleML086NafDatalogAdapter(outFile, FileAccess.Read));
			Assert.AreEqual(ie.Label, ie2.Label, "Label");
			Assert.AreEqual(ie.FactsCount, ie2.FactsCount, "FactsCount");
		}

		[Test]
		public void HRFLoadRuleBase() {
			IInferenceEngine ie = new IEImpl();
			
			ie.LoadRuleBase(new HRF086Adapter(ruleFilesFolder + "gedcom-relations.hrf", FileAccess.Read));
			Assert.AreEqual("forward", ie.Direction, "Gedcom Direction");
			Assert.AreEqual("gedcom-relations", ie.Label, "Gedcom Label");
			Assert.AreEqual(0, ie.FactsCount, "Gedcom Facts");
			
			ie.LoadRuleBase(new HRF086Adapter(ruleFilesFolder + "fraudcontrol.hrf", FileAccess.Read));
			Assert.AreEqual(3, ie.FactsCount, "FraudControl Facts");
			
			ie.LoadRuleBase(new HRF086Adapter(ruleFilesFolder + "telco-rating.hrf", FileAccess.Read));
			Assert.AreEqual(3, ie.FactsCount, "Telco-Rating Facts");
		}

		[Test]
		public void HRFSaveRuleBase() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new HRF086Adapter(ruleFilesFolder + "gedcom-relations.hrf", FileAccess.Read));
			ie.SaveRuleBase(new HRF086Adapter(outFileHRF, FileAccess.Write));
			
			IInferenceEngine ie2 = new IEImpl();
			ie2.LoadRuleBase(new HRF086Adapter(outFileHRF, FileAccess.Read));

			Assert.AreEqual(ie.Direction, ie2.Direction, "Same Direction");
			Assert.AreEqual(ie.Label, ie2.Label, "Same Label");
		}
		
		[Test]
		public void HRFDirectionless() {
			// test added after detecting a bug in the adapter when the direction is empty string
			using(HRF086Adapter hrfa = new HRF086Adapter(new MemoryStream(Encoding.ASCII.GetBytes("+Luxury{Rolex};")),
				                                           FileAccess.Read)) {
				
				Assert.AreEqual(1, hrfa.Facts.Count);
			}
		}
		
		[Test]
		public void Visio2003ImplicationProperties_2_1() {
			int totalTest = 0;
			
			using(Visio2003Adapter va = new Visio2003Adapter(ruleFilesFolder + "test-2_1.vdx", FileAccess.Read)) {
				foreach(Implication i in va.Implications) {
					if (i.Label == "imp1") {
						Assert.AreEqual(70, i.Priority, "imp1: Priority");
						Assert.AreEqual("imp2", i.Mutex, "imp1: Mutex");
						Assert.AreEqual("imp3", i.Precondition, "imp1: Precondition");
						Assert.AreEqual(ImplicationAction.Assert, i.Action, "imp1: Asserting");
						totalTest++;
					}
					else if (i.Label == "imp2") {
						Assert.AreEqual(20, i.Priority, "imp2: Priority");
						Assert.AreEqual("imp1", i.Mutex, "imp2: Mutex");
						Assert.AreEqual("imp3", i.Precondition, "imp2: Precondition");
						Assert.AreEqual(ImplicationAction.Assert, i.Action, "imp2: Asserting");
						totalTest++;
					}
				}
			}
			
			Assert.AreEqual(2, totalTest, "totalTest");
		}
		
		[Test]
		public void Visio2003ImplicationProperties_2_2() {
			int totalTest = 0;
			
			using(Visio2003Adapter va = new Visio2003Adapter(ruleFilesFolder + "test-2_2.vdx", FileAccess.Read)) {
				foreach(Implication i in va.Implications) {
					if (i.Label == "imp1") {
						Assert.AreEqual(70, i.Priority, "imp1: Priority");
						Assert.AreEqual("imp2", i.Mutex, "imp1: Mutex");
						Assert.AreEqual("imp3", i.Precondition, "imp1: Precondition");
						Assert.AreEqual(ImplicationAction.Assert, i.Action, "imp1: Asserting");
					}
					else if (i.Label == "imp2") {
						Assert.AreEqual(20, i.Priority, "imp2: Priority");
						Assert.AreEqual("imp1", i.Mutex, "imp2: Mutex");
						Assert.AreEqual("imp3", i.Precondition, "imp2: Precondition");
						Assert.AreEqual(ImplicationAction.Retract, i.Action, "imp2: Retract");
					}
					else if (i.Label == "imp3") {
						Assert.AreEqual(50, i.Priority, "imp3: Priority");
						Assert.AreEqual(String.Empty, i.Mutex, "imp3: Mutex");
						Assert.AreEqual(String.Empty, i.Precondition, "imp3: Precondition");
						Assert.AreEqual(ImplicationAction.Assert, i.Action, "imp3: Assert");
					}
					else if (i.Label == "imp4") {
						Assert.AreEqual(50, i.Priority, "imp4: Priority");
						Assert.AreEqual(String.Empty, i.Mutex, "imp4: Mutex");
						Assert.AreEqual(String.Empty, i.Precondition, "imp4: Precondition");
						Assert.AreEqual(ImplicationAction.Count, i.Action, "imp4: Count");
					}
					totalTest++;
				}
			}
			
			Assert.AreEqual(4, totalTest, "totalTest");
		}
		
		[Test]
		public void Visio2003ImplicationProperties_2_3() {
			int totalTest = 0;
			
			using(Visio2003Adapter va = new Visio2003Adapter(ruleFilesFolder + "test-2_3.vdx", FileAccess.Read)) {
				foreach(Implication i in va.Implications) {
					if (i.Label == "imp1") {
						Assert.AreEqual(70, i.Priority, "imp1: Priority");
						Assert.AreEqual("imp2", i.Mutex, "imp1: Mutex");
						Assert.AreEqual("imp3", i.Precondition, "imp1: Precondition");
						Assert.AreEqual(ImplicationAction.Assert, i.Action, "imp1: Asserting");
						Assert.IsTrue(i.Deduction.HasFormula, "imp1: HasFormula");
					}
					else if (i.Label == "imp2") {
						Assert.AreEqual(20, i.Priority, "imp2: Priority");
						Assert.AreEqual("imp1", i.Mutex, "imp2: Mutex");
						Assert.AreEqual("imp3", i.Precondition, "imp2: Precondition");
						Assert.AreEqual(ImplicationAction.Retract, i.Action, "imp2: Retract");
					}
					else if (i.Label == "imp3") {
						Assert.AreEqual(50, i.Priority, "imp3: Priority");
						Assert.AreEqual(String.Empty, i.Mutex, "imp3: Mutex");
						Assert.AreEqual(String.Empty, i.Precondition, "imp3: Precondition");
						Assert.AreEqual(ImplicationAction.Assert,i.Action,  "imp3: Assert");
					}
					else if (i.Label == "imp4") {
						Assert.AreEqual(50, i.Priority, "imp4: Priority");
						Assert.AreEqual(String.Empty, i.Mutex, "imp4: Mutex");
						Assert.AreEqual(String.Empty,i.Precondition,  "imp4: Precondition");
						Assert.AreEqual(ImplicationAction.Count, i.Action, "imp4: Count");
					}
					else if (i.Label == "imp5") {
						Assert.AreEqual(50, i.Priority, "imp5: Priority");
						Assert.AreEqual(String.Empty, i.Mutex, "imp5: Mutex");
						Assert.AreEqual(String.Empty, i.Precondition, "imp5: Precondition");
						Assert.AreEqual(ImplicationAction.Modify, i.Action, "imp4: Modify");
					}
					totalTest++;
				}
			}
			
			Assert.AreEqual(5, totalTest, "totalTest");
		}
		
		[Test]
		public void Visio2003PageNames() {
			string[] pageNames = Visio2003Adapter.GetPageNames(ruleFilesFolder + "discount.vdx");			
			Assert.AreEqual(5, pageNames.Length, "Page count");
			
			Array.Sort(pageNames);
			string expectedResult = "|Customer Data|Customer Rules|Discount Rules|Product Data|Queries|";
			string actualResult = "|";
			foreach(string pageName in pageNames) actualResult += (pageName + "|");
			Assert.AreEqual(expectedResult, actualResult, "Page names");
		}
	}
}
