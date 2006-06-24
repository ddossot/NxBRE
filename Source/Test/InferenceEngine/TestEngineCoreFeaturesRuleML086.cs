namespace NxBRE.Test.InferenceEngine {

	using System.IO;
	
	using NxBRE.InferenceEngine.IO;
	
	public class TestEngineCoreFeaturesRuleML086:TestEngineCoreFeaturesRuleML09 {

		protected override IRuleBaseAdapter NewTestAdapter() {
			return new RuleML086NafDatalogAdapter(ruleFilesFolder + "test-0_86.ruleml", FileAccess.Read);
		}
		
		protected override IRuleBaseAdapter NewGedcomAdapter() {
			return new RuleML08DatalogAdapter(ruleFilesFolder + "gedcom-relations-0_86.ruleml", FileAccess.Read);
		}
		
		// the following tests are disabled because RuleML 0.86 Adapter does not support them. 
		public override void HyperLinkIndividual() {}
		public override void EquivalentDirect() {}
		public override void EquivalentTwoLevels() {}
		public override void EquivalentNaf() {}
		public override void ProtectIntegrity() {}
		public override void SlotSupport() {}
		public override void DataInBodySupport() {}
		public override void SlotContributeNamedValues() {}
		
	}
	
}
