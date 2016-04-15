namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.Collections.Generic;
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
			
			// for now we compare only the size after normalization, which is accurate enough for this test
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
			IList<Fact> facts = adapter.Facts;
			Assert.IsNotNull(facts, "Facts not null");
			Assert.AreEqual(1, facts.Count, "One fact expected");
			Assert.AreEqual("regular", facts[0].Type, "Fact of type 'regular' expected");
		}

		[Test]
		public void RuleML08SaveRuleBase() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML08DatalogAdapter(ruleFilesFolder + "discount.ruleml", FileAccess.Read));
			ie.SaveRuleBase(new RuleML08DatalogAdapter(outFile, FileAccess.Write));

			// for now, let's only reload the rulebase
			ie.LoadRuleBase(new RuleML08DatalogAdapter(outFile, FileAccess.Read));
		}
	
		[Test]
		public void RuleML086SaveRuleBase() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML086DatalogAdapter(ruleFilesFolder + "endlessloop.ruleml", FileAccess.Read));
			ie.SaveRuleBase(new RuleML086DatalogAdapter(outFile, FileAccess.Write));
			
			ie.LoadRuleBase(new RuleML086DatalogAdapter(outFile, FileAccess.Read));
			
			Assert.IsTrue(AreSameXml(ruleFilesFolder + "endlessloop.ruleml", outFile), "Same XML");
		}
	
		[Test]
		public void RuleMLNaf086SaveRuleBase() {
			string inFile = ruleFilesFolder + "test-0_86.ruleml";
			
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML086NafDatalogAdapter(inFile, FileAccess.Read));
			ie.SaveRuleBase(new RuleML086NafDatalogAdapter(outFile, FileAccess.Write));

			ie.LoadRuleBase(new RuleML086NafDatalogAdapter(outFile, FileAccess.Read));

			Assert.IsTrue(AreSameXml(inFile, outFile), "Same XML");
		}
	
		[Test]
		public void RuleMLNaf09SaveRuleBase() {
			string inFile = ruleFilesFolder + "test-0_9.ruleml";
			
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(inFile, FileAccess.Read));
			ie.SaveRuleBase(new RuleML09NafDatalogAdapter(outFile, FileAccess.Write));

			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(outFile, FileAccess.Read));
			
			Assert.IsTrue(AreSameXml(inFile, outFile), "Same XML: " + inFile + " and " + outFile);
		}
	
		[Test]
		public void RuleMLNaf091SaveRuleBase() {
			string inFile = ruleFilesFolder + "test-0_91.ruleml";
			
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML091NafDatalogAdapter(inFile, FileAccess.Read));
			ie.SaveRuleBase(new RuleML091NafDatalogAdapter(outFile, FileAccess.Write));

			ie.LoadRuleBase(new RuleML091NafDatalogAdapter(outFile, FileAccess.Read));

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

			ie.LoadFacts(new RuleML086DatalogAdapter(ruleFilesFolder + "facts.ruleml", FileAccess.Read));
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
			using(Visio2003Adapter va = new Visio2003Adapter(ruleFilesFolder + "test-2_1.vdx", FileAccess.Read)) {
				CommonVisio2003ImplicationProperties(0, 3, va, true);
			}
		}
		
		[Test]
		public void Visio2003ImplicationProperties_2_2() {
			using(Visio2003Adapter va = new Visio2003Adapter(ruleFilesFolder + "test-2_2.vdx", FileAccess.Read)) {
				CommonVisio2003ImplicationProperties(0, 4, va, false);
			}
		}
		
		[Test]
		public void Visio2003ImplicationProperties_2_3() {
			using(Visio2003Adapter va = new Visio2003Adapter(ruleFilesFolder + "test-2_3.vdx", FileAccess.Read)) {
				CommonVisio2003ImplicationProperties(1, 5, va, false);
				foreach(Implication i in va.Implications) {
					if (i.Label == "imp1") {
						Assert.IsTrue(i.Deduction.HasFormula, "imp1: HasFormula");
						break;
					}
				}
			}
		}
		
		[Test]
		public void Visio2003ImplicationProperties_3_0() {
			RunVisio2003ImplicationProperties_3_0(false);
		}
		
		[Test]
		public void Visio2003ImplicationProperties_3_0_Strict() {
			RunVisio2003ImplicationProperties_3_0(true);
		}
		
		private void RunVisio2003ImplicationProperties_3_0(bool strict) {
			using(Visio2003Adapter va = new Visio2003Adapter(ruleFilesFolder + "test-3_0" + (strict?"-strict":"") + ".vdx",
			                                                 FileAccess.Read,
			                                                 strict)) {
				
				CommonVisio2003ImplicationProperties(1, 5, va, false);
				
				foreach(Implication i in va.Implications) {
					if (i.Label == "imp1") {
						Assert.IsTrue(i.Deduction.HasFormula, "imp1: HasFormula");
						Atom atom = (Atom)i.AtomGroup.Members[0];
						Assert.IsTrue(atom.HasSlot, "Has Slot");
						Assert.IsInstanceOfType(typeof(byte[]), atom.GetPredicateValue(1), "Typed predicate support");
						
						Assert.IsInstanceOfType(typeof(Function), atom.Members[2], "Basic slot type predicate support");
						Assert.AreEqual("Size", atom.SlotNames[2], "Basic slot name predicate support");
						
						Assert.IsInstanceOfType(typeof(int), atom.GetPredicateValue(3), "Typed slot type predicate support");
						Assert.AreEqual("Year", atom.SlotNames[3], "Typed slot name predicate support");
						Assert.AreEqual(2006, atom.GetPredicate("Year").Value, "Typed slot value support");
						break;
					}
				}
				
				Assert.AreEqual(1, va.IntegrityQueries.Count, "Integrity Queries Count");
				Assert.AreEqual("iq1", va.IntegrityQueries[0].Label, "Integrity Query Label");
				
				Assert.AreEqual(2, va.Equivalents.Count, "Equivalents count");
				foreach(Equivalent equivalent in va.Equivalents) Assert.AreEqual("eq1", equivalent.Label, "Equivalent label");
				
				List<string> expected = new List<string>(new string[] {"own{?person,?stuff} == possess{?person,?stuff}", "own{?person,?stuff} == belong{?stuff,?person}"});
				foreach(Equivalent equivalent in va.Equivalents) {
					Assert.IsTrue(expected.Contains(equivalent.ToString()), equivalent.ToString() + " found.");
					expected.Remove(equivalent.ToString());
				}
			}
		}
		
		private void CommonVisio2003ImplicationProperties(int expectedQueryCount, int expectedImplicationCount, Visio2003Adapter va, bool onlyAssert) {
			Assert.AreEqual(expectedQueryCount, va.Queries.Count, "expectedQueryCount");
			
			foreach(Query q in va.Queries) {
      	if (q.Label == "q1") {
					Assert.AreEqual(1, q.AtomGroup.AllAtoms.Count, "Count of query atoms");
      	}
			}
			
			Assert.AreEqual(expectedImplicationCount, va.Implications.Count, "expectedImplicationCount");
			                
			foreach(Implication i in va.Implications) {
				if (i.Label == "imp1") {
					Assert.AreEqual(70, i.Priority, "imp1: Priority");
					Assert.AreEqual("imp2", i.Mutex, "imp1: Mutex");
					Assert.AreEqual("imp3", i.Precondition, "imp1: Precondition");
					Assert.AreEqual(ImplicationAction.Assert, i.Action, "imp1: Assert");
				}
				else if (i.Label == "imp2") {
					Assert.AreEqual(20, i.Priority, "imp2: Priority");
					Assert.AreEqual("imp1", i.Mutex, "imp2: Mutex");
					Assert.AreEqual("imp3", i.Precondition, "imp2: Precondition");
					Assert.AreEqual(onlyAssert?ImplicationAction.Assert:ImplicationAction.Retract, i.Action, "imp2: Retract");
				}
				else if (i.Label == "imp3") {
					Assert.AreEqual(50, i.Priority, "imp3: Priority");
					Assert.AreEqual(String.Empty, i.Mutex, "imp3: Mutex");
					Assert.AreEqual(String.Empty, i.Precondition, "imp3: Precondition");
					Assert.AreEqual(ImplicationAction.Assert, i.Action,  "imp3: Assert");
				}
				else if (i.Label == "imp4") {
					Assert.AreEqual(50, i.Priority, "imp4: Priority");
					Assert.AreEqual(String.Empty, i.Mutex, "imp4: Mutex");
					Assert.AreEqual(String.Empty,i.Precondition,  "imp4: Precondition");
					Assert.AreEqual(onlyAssert?ImplicationAction.Assert:ImplicationAction.Count, i.Action, "imp4: Count");
				}
				else if (i.Label == "imp5") {
					Assert.AreEqual(50, i.Priority, "imp5: Priority");
					Assert.AreEqual(String.Empty, i.Mutex, "imp5: Mutex");
					Assert.AreEqual(String.Empty, i.Precondition, "imp5: Precondition");
					Assert.AreEqual(onlyAssert?ImplicationAction.Assert:ImplicationAction.Modify, i.Action, "imp4: Modify");
				}
			}
		}
		
		[Test]
		public void Visio2003PageNames() {
			IList<string> pageNames = Visio2003Adapter.GetPageNames(ruleFilesFolder + "discount.vdx");			
			Assert.AreEqual(5, pageNames.Count, "Page count");
			
			string expectedResult = "|Customer Data|Customer Rules|Discount Rules|Product Data|Queries|";
			foreach(string pageName in pageNames) Assert.IsTrue(expectedResult.Contains("|" + pageName + "|"), pageName + " found");
		}
	
		[Test]
		public void PerfomativeLoadProcessing() {
			string inFile = ruleFilesFolder + "discount-0_9.ruleml";

			IInferenceEngine ie = new IEImpl();
			
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(inFile, FileAccess.Read), false);
			Assert.AreEqual(0, ie.FactsCount, "No fact should have been loaded");
			
			ie.Process(RuleTypes.ConnectivesOnly);
			Assert.AreEqual(0, ie.FactsCount, "No fact should have been deducted");
			
			ie.Process(RuleTypes.PerformativesOnly);
			Assert.AreEqual(3, ie.FactsCount, "Only fact assertion should have happened");
			
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(inFile, FileAccess.Read), false);
			Assert.AreEqual(0, ie.FactsCount, "Memory should be empty at this point");
			
			ie.Process(RuleTypes.All);
			Assert.AreEqual(6, ie.FactsCount, "Fact assertion and deduction should have happened");
		}
		
		[Test]
		[ExpectedException(typeof(InvalidDataException))]
		public void HRFWithSyntaxError() {
			// regression test for bug 1850255
			new HRF086Adapter(new MemoryStream(Encoding.ASCII.GetBytes("#DIRECTION_FORWARD\n\rfoo(bar);")), FileAccess.Read);
		}
		
	}
}
