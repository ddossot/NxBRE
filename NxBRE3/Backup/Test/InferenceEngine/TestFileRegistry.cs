namespace NxBRE.Test.InferenceEngine {
	using NUnit.Framework;

	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Registry;
	
	using NxBRE.Util;
	

	[TestFixture]
	public class TestFileRegistry {
		private IRegistry registry;
		
		[TestFixtureSetUp]
		public void Initialize() {
			string ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
			
			registry = new FileRegistry(ruleFilesFolder + "test-file-registry.xml");
		}
		
		[Test]
		public void Count() {
			Assert.AreEqual(5, registry.Count);
		}
		
		[Test]
		public void BasicEngineChecks() {
			foreach(string engineID in registry.EngineIDs) {
				Assert.IsNotEmpty(registry.GetEngine(engineID).Label);
			}
		}

	}
	
}
