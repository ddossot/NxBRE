namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections.Generic;
	using System.IO;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.Util;
	
	using NUnit.Framework;
	
	[TestFixture]
	public class TestEventContext {
		//TODO test counting implications event context
		
		private string ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
		
		private IEImpl ie;
		
		[TestFixtureSetUp]
		public void Setup() {
			ie = new IEImpl();
			
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "events-test.ruleml",
			                                           FileAccess.Read));
		}
		
		[Test]
		public void TestAllActionsWithContext() {
			ie.ExposeEventContext = true;
			
			ie.NewFactHandler +=  new NewFactEvent(AssertFactHandlerWithContext);
			ie.DeleteFactHandler += new NewFactEvent(RetractFactHandlerWithContext);
			ie.ModifyFactHandler += new NewFactEvent(ModifyFactHandlerWithContext);
			
			ie.Process();
		}
		
		private void AssertFactHandlerWithContext(NewFactEventArgs nfea) {
			Assert.AreEqual("Asserting Implication", nfea.Context.Implication.Label);
			Assert.AreEqual("((trigger{whatever}))", Misc.IListToString(nfea.Context.Facts));
		}
		
		private void RetractFactHandlerWithContext(NewFactEventArgs nfea) {
			Assert.AreEqual("Retracting Implication", nfea.Context.Implication.Label);
			Assert.AreEqual("((trigger{whatever}))", Misc.IListToString(nfea.Context.Facts));
		}
		
		private void ModifyFactHandlerWithContext(NewFactEventArgs nfea) {
			Assert.AreEqual("Modifying Implication", nfea.Context.Implication.Label);
			Assert.AreEqual("((toModify{whatever}))", Misc.IListToString(nfea.Context.Facts));
		}
		
		[Test]
		public void TestAllActionsWithoutContext() {
			ie.ExposeEventContext = false;
			
			ie.NewFactHandler +=  new NewFactEvent(EventHandlerWithoutContext);
			ie.DeleteFactHandler += new NewFactEvent(EventHandlerWithoutContext);
			ie.ModifyFactHandler += new NewFactEvent(EventHandlerWithoutContext);
			
			ie.Process();
		}
		
		private void EventHandlerWithoutContext(NewFactEventArgs nfea) {
			Assert.IsNull(nfea.Context);
		}
		
	}
}
