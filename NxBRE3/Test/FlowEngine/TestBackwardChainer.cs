namespace NxBRE.Test.FlowEngine {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.IO;
	using NxBRE.FlowEngine.Core;
	
	using NxBRE.Util;

	using NUnit.Framework;
	
	[TestFixture]
	public class TestBackwardChainer {
		private IFlowEngine flowEngine;
		private BackwardChainer backwardChainer;
		
		[TestFixtureSetUp]
		public void InitializeChainer() {
			string ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder");
			
			flowEngine = new BREImpl();
			flowEngine.Init(new XBusinessRulesFileDriver(ruleFilesFolder + "/car-loan-rules.xbre"));
			
			backwardChainer = new BackwardChainer(flowEngine);
		}
		
		[Test]
		public void SetsForKnownOutcome() {
			IList<string> sets = backwardChainer.GetSetIdsFromTargetObjectId("IncomeCriteriaOk");
			Assert.AreEqual(2, sets.Count);
			Assert.IsTrue(sets.Contains("rule9"));
			Assert.IsTrue(sets.Contains("rule10"));
		}
				
		[Test]
		public void SetsForUnknownOutcome() {
			Assert.AreEqual(0, backwardChainer.GetSetIdsFromTargetObjectId("foo").Count);
		}
				
		[Test]
		public void SourceObjectsForKnownSet() {
			IList<string> objects = backwardChainer.GetSourceObjectIdsFromSetId("rule9");
			Assert.AreEqual(4, objects.Count);
			Assert.IsTrue(objects.Contains("IsCebEmployee"));
			Assert.IsTrue(objects.Contains("TotalIncome"));
		}
		
		[Test]
		public void SourceObjectsForUnknownSet() {
			Assert.AreEqual(0, backwardChainer.GetSourceObjectIdsFromSetId("foo").Count);
		}
		
	}
}
