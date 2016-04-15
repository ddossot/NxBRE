namespace NxBRE.FlowEngine.Factories
{
	using System;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.IO;
	
	/// <summary>This factory allows an easy creation of a BRE object</summary>
	/// <author>David Dossot</author>
	public class BREFactory {
		private DispatchRuleResult resultHandler;
		                         
		public BREFactory():this(null) {}
		
		public BREFactory(DispatchRuleResult resultHandler) {
		  this.resultHandler = resultHandler;
		}
		
		public IFlowEngine NewBRE(IRulesDriver rulesDriver) {
			BREImpl bre = new BREImpl();
			
			// Lets register the result handler
			if (resultHandler != null) bre.ResultHandlers += resultHandler;

			if (!bre.Init(rulesDriver)) return null;
			else return bre;
		}
	}
	
}
