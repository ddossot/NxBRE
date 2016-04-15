namespace NxBRE.Test.InferenceEngine {
	
	using NxBRE.InferenceEngine.IO;
	
	using NUnit.Framework;
	
	public class TestEngineCoreFeaturesRuleML09Expanded:TestEngineCoreFeaturesRuleML09Compact {
		[TestFixtureSetUp]
		public override void GenerateRuleFiles() {
			testFile = GenerateRuleFile("test-0_9.ruleml", SaveFormatAttributes.Expanded);
			gedcomFile = GenerateRuleFile("gedcom-relations-0_9.ruleml", SaveFormatAttributes.Expanded);
		}
	}
	
}
