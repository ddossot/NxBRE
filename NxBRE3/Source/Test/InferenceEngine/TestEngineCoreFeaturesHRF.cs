namespace NxBRE.Test.InferenceEngine {

	using System.IO;
	
	using NxBRE.InferenceEngine.IO;
	
	public class TestEngineCoreFeaturesHRF:TestEngineCoreFeaturesRuleML086 {

		protected override IRuleBaseAdapter NewTestAdapter() {
			return new HRF086Adapter(ruleFilesFolder + "test-0_86.hrf", FileAccess.Read);
		}	
		
		protected override IRuleBaseAdapter NewGedcomAdapter() {
			return new HRF086Adapter(ruleFilesFolder + "gedcom-relations.hrf", FileAccess.Read);
		}
		
		// the following tests are disabled because HRF can not support embedded C# expressions. 
		public override void QueryWithExpressionInAtomRelation() {}
		public override void QueryWithExpressionInIndividualEvaluation() {}
		public override void ExpressionAssertion() {}
		public override void ExpressionModification() {}
		public override void ExpressionIndividualEvaluation(){}
		public override void WrongMultipleFactInDataTables_Bug_1252700() {}
		public override void AndBlockContainingOnlyOrBlocks() {}
	}
	
}
