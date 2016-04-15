namespace NxBRE.FlowEngine.Factories
{
	using System;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.IO;
	
	/// <summary>This factory allows an easy creation of a BRE objects from a unique Clone,<br/>
	/// which is very convenient for a multi-threaded environment where each thread will
	/// use a different clone.</summary>
	/// <author>David Dossot</author>
	public sealed class BRECloneFactory {
		private IRulesDriver rulesDriver = null;
		private BREFactory bref = null;
		private IFlowEngine bre = null;
		
		public BRECloneFactory(IRulesDriver rulesDriver):this(rulesDriver, null) {}
		
		public BRECloneFactory(IRulesDriver rulesDriver, DispatchRuleResult resultHandler) {
			if (rulesDriver == null)
				throw new BREException("A non-null IRulesDriver must be passed to BRECloneFactory");

			this.rulesDriver = rulesDriver;
			
			if (bref == null)	bref = new BREFactory(resultHandler);
		}
				
		public IFlowEngine NewBRE() {
			if (bref == null)
				throw new BREException("BRECloneFactory is not correctly initialized.");

			if (bre == null)
				bre = bref.NewBRE(rulesDriver);
			
			if (bre == null)
				throw new BREException("BRECloneFactory could not instantiate an valid IBRE implementation.");
			
			return (IFlowEngine)bre.Clone();
		}

	}
	
}
