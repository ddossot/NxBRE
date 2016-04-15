namespace NxBRE.Test.InferenceEngine {
	using System.IO;
	
	using NxBRE.InferenceEngine.IO;
	
	using NUnit.Framework;
	
	public class TestEngineCoreFeaturesRuleML091:TestEngineCoreFeaturesRuleML09 {
		protected override IRuleBaseAdapter NewTestAdapter() {
			return new RuleML091NafDatalogAdapter(ruleFilesFolder + "test-0_91.ruleml", FileAccess.Read);
		}	
		
		protected override IRuleBaseAdapter NewGedcomAdapter() {
			return new RuleML091NafDatalogAdapter(ruleFilesFolder + "gedcom-relations-0_91.ruleml", FileAccess.Read);
		}

	}
	
}
