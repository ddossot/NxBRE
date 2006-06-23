namespace org.nxbre.test.ie {
	using System;
	using System.Collections;
	using System.IO;
	
	using NUnit.Framework;
	
	using net.ideaity.util.events;
	
	using org.nxbre.ie;
	using org.nxbre.ie.adapters;
	using org.nxbre.ie.predicates;
	using org.nxbre.ie.core;
	using org.nxbre.ie.rule;
	
	using org.nxbre.util;
	
	[TestFixture]
	public class TestEngineRuleML09MultiSyntax:AbstractTestEngine {
		private void PerformLoadTest(string ruleFile) {
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + ruleFile, FileAccess.Read));
			
			Assert.AreEqual(2, ie.FactsCount, "Initial facts count");
			
			string[] factsToCheck = new string[] {};
			for(IEnumerator e = ie.Facts ; e.MoveNext();) {
				Fact fact = (Fact) e.Current;
				Assert.IsTrue((fact.ToString() == "sell{John,Mary,XMLBible}") | ((fact.ToString() == "keep{Mary,XMLBible}")), "Initial facts value");
			}
			
			deductionsToCheck = new string[] {"buy{Mary,John,XMLBible}", "own{Mary,XMLBible}"};
			
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
			Process();
			Assert.AreEqual(2, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Deductions OK");
	  	ie.NewFactHandler -= henf;
	  	deductionsToCheck = null;
		}
	
		[Test]
		public void CompactSyntaxLoad() {
			PerformLoadTest("own_compact.ruleml");
		}
	
		[Test]
		public void StandardSyntaxLoad() {
			PerformLoadTest("own.ruleml");
		}
	
		[Test]
		public void ExpandedSyntaxLoad() {
			PerformLoadTest("own_expanded.ruleml");
		}
		
		private void PerformSaveTest(string ruleFile, SaveFormatAttributes sfa) {
			string inFile = ruleFilesFolder + ruleFile;
			
			string outFile = outFilesFolder + "outtest.ruleml";
			FileInfo fi = new FileInfo(outFile);
			if (fi.Exists) fi.Delete();
			
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(inFile, FileAccess.Read));
			ie.SaveRuleBase(new RuleML09NafDatalogAdapter(outFile, FileAccess.Write, sfa));

			Assert.IsTrue(TestAdapter.AreXmlOfSameLength(inFile, outFile), "Same XML file lengths");
		}
	
		[Test]
		public void CompactSyntaxSave() {
			PerformSaveTest("own_compact.ruleml", SaveFormatAttributes.Compact);
		}
	
		[Test]
		public void StandardSyntaxSave() {
			PerformSaveTest("own.ruleml", SaveFormatAttributes.Standard);
		}
	
		[Test]
		public void ExpandedSyntaxSave() {
			PerformSaveTest("own_expanded.ruleml", SaveFormatAttributes.Expanded);
		}
	}

}
