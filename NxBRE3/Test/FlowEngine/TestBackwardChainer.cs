using Microsoft.VisualStudio.TestTools.UnitTesting;

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
		private string ruleFilesFolder;
		
		[TestFixtureSetUp]
		public void InitializeFixture() {
			ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder");
		}
		
		[SetUp]
		public void InitializeChainer() {
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
		public void CircularityDetection() {
			flowEngine = new BREImpl();
			flowEngine.Init(new XBusinessRulesFileDriver(ruleFilesFolder + "/circularity.xbre"));
			backwardChainer = new BackwardChainer(flowEngine);
			
			Stack<string> resolutionPath = new Stack<string>();
			Assert.IsNull(backwardChainer.Resolve("A", resolutionPath));
			Assert.IsTrue(resolutionPath.Contains("{Circularity}"));
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
		}
		
		[Test]
		public void TwoLevelProcessingWithOutcome() {
			Stack<string> resolutionPath = new Stack<string>();
			flowEngine.RuleContext.SetObject("TotalIncome", 100.0);
			flowEngine.RuleContext.SetObject("MonthlyInstallments", 10.0);
			Assert.IsTrue((bool)backwardChainer.Resolve("ApproveCarLoan", resolutionPath));
			Assert.IsTrue(CheckFirstSetIs(resolutionPath, "rule11"), "Deducting Set should have been: rule11");
		}
		
		[Test]
		public void NLevelProcessingWithOutcome() {
			Stack<string> resolutionPath = new Stack<string>();
			flowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			flowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			flowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			flowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			flowEngine.RuleContext.SetObject("TimeAtCurrentJob", 13);
			flowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "Dutch");
			
			Assert.IsTrue((bool) backwardChainer.Resolve("ApproveCarLoan", resolutionPath));
			Assert.IsTrue(CheckFirstSetIs(resolutionPath, "rule6"), "Deducting Set should have been: rule6");
		}
		
		[Test]
		public void PositiveResolution() {
			flowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			flowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			flowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			flowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			flowEngine.RuleContext.SetObject("TimeAtCurrentJob", 13);
			flowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "Dutch");
			Assert.IsTrue((bool) backwardChainer.Resolve("ApproveCarLoan"));
		}
		
		[Test]
		public void NegativeResolutions() {
			IFlowEngine transientFlowEngine = (IFlowEngine) flowEngine.Clone();
			transientFlowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			transientFlowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			transientFlowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			transientFlowEngine.RuleContext.SetObject("IsCebEmployee", true);
			transientFlowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			transientFlowEngine.RuleContext.SetObject("TimeAtCurrentJob", 13);
			transientFlowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "French");
			Assert.IsNull(backwardChainer.Resolve("ApproveCarLoan"));
			
			transientFlowEngine = (IFlowEngine) flowEngine.Clone();
			transientFlowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			transientFlowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			transientFlowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			transientFlowEngine.RuleContext.SetObject("IsCebEmployee", true);
			transientFlowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			transientFlowEngine.RuleContext.SetObject("TimeAtCurrentJob", 10);
			transientFlowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "Dutch");
			Assert.IsNull(backwardChainer.Resolve("ApproveCarLoan"));
			
			transientFlowEngine = (IFlowEngine) flowEngine.Clone();
			transientFlowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			transientFlowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			transientFlowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			transientFlowEngine.RuleContext.SetObject("IsCebEmployee", false);
			transientFlowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			transientFlowEngine.RuleContext.SetObject("TimeAtCurrentJob", 15);
			transientFlowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "Dutch");
			Assert.IsNull(backwardChainer.Resolve("ApproveCarLoan"));
		}
		
		[Test]
		public void FlowEnginePositiveResolutionNoDefault() {
			flowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			flowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			flowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			flowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			flowEngine.RuleContext.SetObject("TimeAtCurrentJob", 13);
			flowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "Dutch");
			Assert.IsTrue((bool) flowEngine.Resolve("ApproveCarLoan"));
		}
		
		[Test]
		public void FlowEnginePositiveResolutionDefault() {
			flowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			flowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			flowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			flowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			flowEngine.RuleContext.SetObject("TimeAtCurrentJob", 13);
			flowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "Dutch");
			Assert.IsTrue((bool) flowEngine.Resolve("ApproveCarLoan", true));
		}
		
		[Test]
		public void FlowEngineNegativeResolutionNoDefault() {
			flowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			flowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			flowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			flowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			flowEngine.RuleContext.SetObject("TimeAtCurrentJob", 13);
			flowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "French");
			Assert.IsNull(flowEngine.Resolve("ApproveCarLoan"));
		}
		
		[Test]
		public void FlowEngineNegativeResolutionDefault() {
			flowEngine.RuleContext.SetObject("TotalIncome", 11000.0);
			flowEngine.RuleContext.SetObject("MonthlyInstallments", 5500.0);
			flowEngine.RuleContext.SetObject("AgeOfApplicant", 27);
			flowEngine.RuleContext.SetObject("IsCebEmployee", true);
			flowEngine.RuleContext.SetObject("ApplicantIsWorking", true);
			flowEngine.RuleContext.SetObject("TimeAtCurrentJob", 13);
			flowEngine.RuleContext.SetObject("CitizenshipOfApplicant", "French");
			Assert.IsFalse((bool) flowEngine.Resolve("ApproveCarLoan", false));
		}
		
		private bool CheckFirstSetIs(Stack<string> resolutionPath, string setId) {
			foreach(string s in resolutionPath) {
				if (s.StartsWith("{Set:")) {
					return s.Equals("{Set:" + setId + "}");
				}
			}
			
			return false;
		}
		
	}
}
