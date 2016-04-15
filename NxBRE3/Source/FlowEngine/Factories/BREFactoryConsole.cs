namespace NxBRE.FlowEngine.Factories
{
    using System.Diagnostics;
	
	using IO;
	using Util;
	
	/// <summary>This factory allows an easy creation of a BRE object,
	/// that redirects its log and error messages on Console Out and Error</summary>
	/// <author>David Dossot</author>
	public sealed class BREFactoryConsole {
		private BREFactory bref = null;
		
		public BREFactoryConsole(SourceLevels engineTraceLevel, SourceLevels ruleBaseTraceLevel) {
			Logger.FlowEngineSource.Switch.Level = engineTraceLevel;
			Logger.FlowEngineRuleBaseSource.Switch.Level = ruleBaseTraceLevel;
			Logger.RefreshBooleanSwitches();
			
			var ctl = new ConsoleTraceListener();
			Logger.FlowEngineSource.Listeners.Add(ctl);
			Logger.FlowEngineRuleBaseSource.Listeners.Add(ctl);
		}
				
		public IFlowEngine NewBRE(IRulesDriver rulesDriver) {
			if (bref == null)	bref = new BREFactory();
			
			return bref.NewBRE(rulesDriver);
		}

	}
	
}
