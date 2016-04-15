namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.IO;
	
	using NUnit.Framework;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	
	[TestFixture]
	public class TestQueryResultsExporter:AbstractTestEngine {
		private string outFile;
		
		[SetUp]
		public void CleanOutputFiles() {
			InitIE();
			outFile = outFilesFolder + "queryresult.ruleml";
			FileInfo fi = new FileInfo(outFile);
			if (fi.Exists) fi.Delete();
		}
		
		[Test]
		public void ExportQueryResult() {
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "discount-0_9.ruleml", FileAccess.Read));
			Process();
			Assert.AreEqual(3, deducted, "(1) Deducted");
			qrs = ie.RunQuery(0);
			deductionsToCheck = new string[] {"discount{Peter Miller,Honda,5.0 percent}",
											  "discount{Peter Miller,Porsche,7.5 percent}"};
			Assert.AreEqual(2, qrs.Count, "(1) Query Result Size");
			
			QueryResultsExporter.ExportResults(qrs,
			                                   new RuleML09NafDatalogAdapter(outFile, FileAccess.Write),
			                                   "results of query index 0");
			
			ie.NewWorkingMemory(WorkingMemoryTypes.IsolatedEmpty);
			Assert.AreEqual(0, ie.FactsCount, "(2) Isolated memory is empty");
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(outFile, FileAccess.Read));
			Assert.AreEqual(2, ie.FactsCount, "(2) Exported query results can be reloaded");
		}
	}
}
