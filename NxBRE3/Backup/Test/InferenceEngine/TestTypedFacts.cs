namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.IO;
	
	using NUnit.Framework;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	[TestFixture]
	public class TestTypedFacts:AbstractTestEngine {

		public void DoPersistenceOfNonConvertibleTypedFacts(IRuleBaseAdapter adapter) {
			string outFile = Parameter.GetString("unittest.outputfolder") + "/outfacts.ruleml";
			ie.LoadRuleBase(adapter);
			
			TestBinder.Character theDuke = new TestBinder.Character("The Duke", "hello world");
			Assert.IsTrue(ie.Assert(new Fact("Character Name",
			                                 new Individual(theDuke),
			                                 new Individual(theDuke.Name))), "Asserted Typed Fact");

			ie.SaveFacts(new RuleML086NafDatalogAdapter(outFile, FileAccess.Write, true));
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void PersistenceOfNonConvertibleTypedFacts086() {
			DoPersistenceOfNonConvertibleTypedFacts(new RuleML086NafDatalogAdapter(ruleFilesFolder + "typed-facts-0_86.ruleml", FileAccess.Read));
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void PersistenceOfNonConvertibleTypedFacts09() {
			DoPersistenceOfNonConvertibleTypedFacts(new RuleML09NafDatalogAdapter(ruleFilesFolder + "typed-facts-0_9.ruleml", FileAccess.Read));
		}
		
		private void EnsureFactCorrectlyTyped(IRuleBaseAdapter adapter, int expectedFactCount) {
			ie.LoadRuleBase(adapter);
			
			Assert.AreEqual(expectedFactCount, ie.FactsCount, "Number of facts");
			
			for(IEnumerator e = ie.Facts ; e.MoveNext(); ) {
				Fact fact = (Fact) e.Current;
				Assert.AreEqual(fact.Type.Substring(1 + fact.Type.LastIndexOf(' ')), fact.GetPredicateValue(0).GetType().FullName, "Ensure type support");
			}
		}
		
		[Test]
		public void LoadTypedFactsRuleML086() {
			EnsureFactCorrectlyTyped(new RuleML086NafDatalogAdapter(ruleFilesFolder + "typed-facts-0_86.ruleml", FileAccess.Read), 9);
		}
		
		[Test]
		public void LoadTypedFactsRuleML09() {
			EnsureFactCorrectlyTyped(new RuleML09NafDatalogAdapter(ruleFilesFolder + "typed-facts-0_9.ruleml", FileAccess.Read), 43);
		}
		
		[Test]
		public void SaveTypedFactsRuleML086() {
			string outFile = Parameter.GetString("unittest.outputfolder") + "/outfacts.ruleml";
			ie.LoadRuleBase(new RuleML086NafDatalogAdapter(ruleFilesFolder + "typed-facts-0_86.ruleml", FileAccess.Read, true));
			ie.SaveFacts(new RuleML086NafDatalogAdapter(outFile, FileAccess.Write, true));
			EnsureFactCorrectlyTyped(new RuleML086NafDatalogAdapter(outFile, FileAccess.Read, true), ie.FactsCount);
		}
		
		[Test]
		public void SaveTypedFactsRuleML09() {
			string outFile = Parameter.GetString("unittest.outputfolder") + "/outfacts.ruleml";
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "typed-facts-0_9.ruleml", FileAccess.Read));
			ie.SaveFacts(new RuleML09NafDatalogAdapter(outFile, FileAccess.Write, SaveFormatAttributes.Compact));
			EnsureFactCorrectlyTyped(new RuleML09NafDatalogAdapter(outFile, FileAccess.Read), ie.FactsCount);
		}
	}
}
