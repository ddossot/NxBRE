namespace NxBRE.Test.InferenceEngine {
	using System.IO;
	
	using NxBRE.InferenceEngine.IO;
	
	using NUnit.Framework;
	
	public class TestEngineCoreFeaturesRuleML091:TestEngineCoreFeaturesRuleML09 {
		protected override IRuleBaseAdapter NewTestAdapter() {
			return new RuleML091NafDatalogAdapter(ruleFilesFolder + "test-0_91.ruleml", FileAccess.Read);
		}	
		
		protected override IRuleBaseAdapter NewGedcomAdapter() {
			//FIXME migrate to 0.91
			return new RuleML09NafDatalogAdapter(ruleFilesFolder + "gedcom-relations-0_9.ruleml", FileAccess.Read);
		}

	}
	
}
