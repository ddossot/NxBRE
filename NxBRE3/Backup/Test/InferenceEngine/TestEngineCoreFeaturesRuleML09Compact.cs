namespace NxBRE.Test.InferenceEngine {

	using System;
	using System.IO;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	
	using NUnit.Framework;
	
	public class TestEngineCoreFeaturesRuleML09Compact:TestEngineCoreFeaturesRuleML09 {
		protected string testFile;
		protected string gedcomFile;
		
		[TestFixtureSetUp]
		public virtual void GenerateRuleFiles() {
			testFile = GenerateRuleFile("test-0_9.ruleml", SaveFormatAttributes.Compact);
			gedcomFile = GenerateRuleFile("gedcom-relations-0_9.ruleml", SaveFormatAttributes.Compact);
		}
		
		protected string GenerateRuleFile(string inFile, SaveFormatAttributes sfa) {
			string outFile = outFilesFolder + sfa.ToString().ToLower() + "-" + inFile;
			
			FileInfo fi = new FileInfo(outFile);
			if (fi.Exists) fi.Delete();
			
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + inFile, FileAccess.Read));
			ie.SaveRuleBase(new RuleML09NafDatalogAdapter(outFile, FileAccess.Write, sfa));
			
			Console.Out.WriteLine("Generated: {0}", outFile);
			
			return outFile;
		}
		
		protected override IRuleBaseAdapter NewTestAdapter() {
			return new RuleML09NafDatalogAdapter(testFile, FileAccess.Read);
		}	
		
		protected override IRuleBaseAdapter NewGedcomAdapter() {
			return new RuleML09NafDatalogAdapter(gedcomFile, FileAccess.Read);
		}
		

	
		
	}
	
}
