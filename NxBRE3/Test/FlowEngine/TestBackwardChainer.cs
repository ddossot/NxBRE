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
		
		[SetUp]
		public void InitializeChainer() {
			string ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder");
			
			flowEngine = new BREImpl();
			flowEngine.Init(new XBusinessRulesFileDriver(ruleFilesFolder + "/car-loan-rules.xbre"));
			
			backwardChainer = new BackwardChainer(flowEngine);
		}
		
		[Test]
		[ExpectedException(typeof(BREException))]
		public void InvalidRuleBase() {
			flowEngine = new BREImpl();
			flowEngine.Init(new XBusinessRulesFileDriver(Parameter.GetString("unittest.inputfile")));
			backwardChainer = new BackwardChainer(flowEngine);
		}
		
		[Test]
		public void SetsForKnownOutcome() {
			Assert.AreEqual(0, backwardChainer.setIdsFromTargetObjectId.Count);

			IList<string> sets = backwardChainer.GetSetIdsFromTargetObjectId("IncomeCriteriaOk");
			Assert.AreEqual(2, sets.Count);
			Assert.IsTrue(sets.Contains("rule9"));
			Assert.IsTrue(sets.Contains("rule10"));
			
			Assert.AreEqual(1, backwardChainer.setIdsFromTargetObjectId.Count);
			
			// should hit the cache this time
			sets = backwardChainer.GetSetIdsFromTargetObjectId("IncomeCriteriaOk");
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
			Assert.AreEqual(0, backwardChainer.sourceObjectIdsFromSetId.Count);
			
			IList<string> objects = backwardChainer.GetSourceObjectIdsFromSetId("rule9");
			Assert.AreEqual(4, objects.Count);
			Assert.IsTrue(objects.Contains("IsCebEmployee"));
			Assert.IsTrue(objects.Contains("TotalIncome"));

			Assert.AreEqual(1, backwardChainer.sourceObjectIdsFromSetId.Count);
			
			// should hit the cache this time
			objects = backwardChainer.GetSourceObjectIdsFromSetId("rule9");
			Assert.AreEqual(4, objects.Count);
			Assert.IsTrue(objects.Contains("IsCebEmployee"));
			Assert.IsTrue(objects.Contains("TotalIncome"));
		}
		
		[Test]
		public void SourceObjectsForUnknownSet() {
			Assert.AreEqual(0, backwardChainer.GetSourceObjectIdsFromSetId("foo").Count);
		}
		
		[Test]
		public void ResolveAlreadyAssertedObject() {
			flowEngine.RuleContext.SetObject("flowEngine", flowEngine);
			Assert.AreSame(flowEngine, backwardChainer.Resolve("flowEngine"));
		}
		
		[Test]
		public void UnresolvableObject() {
			Assert.IsNull(backwardChainer.Resolve("foo"));
		}
		
		[Test]
		public void OneLevelProcessingNoOutcome() {
			Assert.IsNull(backwardChainer.Resolve("IncomeCriteriaOk"), "zero base facts");
			
			flowEngine.RuleContext.SetObject("TotalIncome", 16000);
			Assert.IsNull(backwardChainer.Resolve("IncomeCriteriaOk"), "not enough base facts for rule 10");
			
			flowEngine.RuleContext.ResultsMap.Clear();
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			Assert.IsNull(backwardChainer.Resolve("IncomeCriteriaOk"), "not enough base facts for rule 9");
		}
		
		[Test]
		public void OneLevelProcessingWithOutcome() {
			Stack<string> resolutionPath = new Stack<string>();
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			flowEngine.RuleContext.SetObject("TotalIncome", 12000);
			Assert.IsTrue((bool)backwardChainer.Resolve("IncomeCriteriaOk", resolutionPath));
			Assert.AreEqual("({Set:rule9},?IncomeCriteriaOk)", Misc.IListToString<string>(new List<string>(resolutionPath)));

			resolutionPath = new Stack<string>();
			Assert.IsTrue((bool)backwardChainer.Resolve("IncomeCriteriaOk", resolutionPath));
			Assert.AreEqual("({RuleContext},?IncomeCriteriaOk)", Misc.IListToString<string>(new List<string>(resolutionPath)));

			resolutionPath = new Stack<string>();
			flowEngine.RuleContext.ResultsMap.Remove("IncomeCriteriaOk");
			Assert.IsTrue((bool)backwardChainer.Resolve("IncomeCriteriaOk", resolutionPath));
			Assert.AreEqual("({Set:rule9},?IncomeCriteriaOk)", Misc.IListToString<string>(new List<string>(resolutionPath)));
			
			resolutionPath = new Stack<string>();
			flowEngine.RuleContext.ResultsMap.Clear();
			flowEngine.RuleContext.SetObject("IsCebEmployee", false);
			flowEngine.RuleContext.SetObject("TotalIncome", 16000);
			Assert.IsTrue((bool)backwardChainer.Resolve("IncomeCriteriaOk", resolutionPath));
			Assert.AreEqual("({Set:rule10},?IncomeCriteriaOk)", Misc.IListToString<string>(new List<string>(resolutionPath)));
		}
		
		[Test]
		public void TwoLevelProcessingNoOutcome() {
			Stack<string> resolutionPath = new Stack<string>();
			flowEngine.RuleContext.SetObject("TotalIncome", -1);
			Assert.IsNull(backwardChainer.Resolve("ApproveCarLoan", resolutionPath), "not enough base facts for rule 11");
			Console.WriteLine(Misc.IListToString<string>(new List<string>(resolutionPath)));
		}
		
	}
}
