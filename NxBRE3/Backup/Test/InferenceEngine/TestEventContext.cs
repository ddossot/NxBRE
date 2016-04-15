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
		private string ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
		
		private IEImpl ie;
		
		private int newFactEventCount;
		
		private int deleteFactEventCount;
		
		private int modifyFactEventCount;
		
		private int factEventCount;
		
		[SetUp]
		public void Setup() {
			ie = new IEImpl();
			
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "events-test.ruleml",
			                                           FileAccess.Read));
			
			newFactEventCount = 0;
			deleteFactEventCount = 0;
			modifyFactEventCount = 0;
			factEventCount = 0;
		}
		
		[Test]
		public void TestAllActionsWithContext() {
			ie.exposeEventContext = true;
			
			ie.NewFactHandler +=  new NewFactEvent(AssertFactHandlerWithContext);
			ie.DeleteFactHandler += new NewFactEvent(RetractFactHandlerWithContext);
			ie.ModifyFactHandler += new NewFactEvent(ModifyFactHandlerWithContext);
			
			ie.Process();
			
			Assert.AreEqual(2, newFactEventCount);
			Assert.AreEqual(1, deleteFactEventCount);
			Assert.AreEqual(1, modifyFactEventCount);
		}
		
		private void AssertFactHandlerWithContext(NewFactEventArgs nfea) {
			newFactEventCount++;
			
			string label = nfea.Context.Implication.Label;
			
			if (label == "Asserting Implication") {
				Assert.AreEqual("((trigger{whatever}))", Misc.IListToString(nfea.Context.Facts));
			} else if (label == "Counting Implication") {
				Assert.AreEqual("((toCount{a}),(toCount{b}),(toCount{c}))", Misc.IListToString(nfea.Context.Facts));
			} else {
				Assert.Fail("Unexpected implication label: " + label);
			}
		}
		
		private void RetractFactHandlerWithContext(NewFactEventArgs nfea) {
			deleteFactEventCount++;
			
			Assert.AreEqual("Retracting Implication", nfea.Context.Implication.Label);
			Assert.AreEqual("((trigger{whatever}))", Misc.IListToString(nfea.Context.Facts));
		}
		
		private void ModifyFactHandlerWithContext(NewFactEventArgs nfea) {
			modifyFactEventCount++;
			
			Assert.AreEqual("Modifying Implication", nfea.Context.Implication.Label);
			Assert.AreEqual("((toModify{whatever}))", Misc.IListToString(nfea.Context.Facts));
		}
		
		[Test]
		public void TestAllActionsWithoutContext() {
			ie.exposeEventContext = false;
			
			ie.NewFactHandler += new NewFactEvent(EventHandlerWithoutContext);
			ie.DeleteFactHandler += new NewFactEvent(EventHandlerWithoutContext);
			ie.ModifyFactHandler += new NewFactEvent(EventHandlerWithoutContext);
			
			ie.Process();
			
			Assert.AreEqual(4, factEventCount);
		}
		
		private void EventHandlerWithoutContext(NewFactEventArgs nfea) {
			factEventCount++;
			
			Assert.IsNull(nfea.Context);
		}
		
	}
}
