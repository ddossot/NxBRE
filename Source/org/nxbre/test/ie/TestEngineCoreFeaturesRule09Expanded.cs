namespace org.nxbre.test.ie {
	
	using org.nxbre.ie.adapters;
	
	using NUnit.Framework;
	
	public class TestEngineCoreFeaturesRule09Expanded:TestEngineCoreFeaturesRule09Compact {
		[TestFixtureSetUp]
		public override void GenerateRuleFiles() {
			testFile = GenerateRuleFile("test-0_9.ruleml", SaveFormatAttributes.Expanded);
			gedcomFile = GenerateRuleFile("gedcom-relations-0_9.ruleml", SaveFormatAttributes.Expanded);
		}
	}
	
}
