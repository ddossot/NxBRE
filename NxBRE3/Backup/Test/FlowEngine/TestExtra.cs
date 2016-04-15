namespace NxBRE.Test.FlowEngine
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.IO;
	
	using NxBRE.Util;

	using NUnit.Framework;
	
	/// <summary>
	/// Extra unit tests for the Flow Engine
	/// </summary>
	[TestFixture]
	public class TestExtra {
		
		protected readonly string ruleFilesFolder;
		
		public TestExtra() {
			ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
		}
		
		private IFlowEngine NewEngine() {
			IFlowEngine flowEngine = new BREImpl();
			if (flowEngine.Init(new XBusinessRulesFileDriver(ruleFilesFolder + "test-extra.xbre"))) {
				return flowEngine;
			}
			else {
				throw new Exception("Can not load: " + ruleFilesFolder + "test-extra.xbre");
			}
		}
		
		[Test]
		public void TestCancelWhile() {
			IFlowEngine flowEngine = NewEngine();
			flowEngine.RuleContext.SetObject("FlowEngine", flowEngine);
			
			flowEngine.Process("CANCEL-WHILE");
			
			// random test an object in the context
			Assert.AreEqual(10, flowEngine.RuleContext.GetObject("10i"));
		}
		
	}
}
