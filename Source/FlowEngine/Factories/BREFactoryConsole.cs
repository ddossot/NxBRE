namespace NxBRE.FlowEngine.Factories
{
	using System;
	
	using NxBRE.FlowEngine.IO;
	
	/// <summary>This factory allows an easy creation of a BRE object,
	/// that redirects its log and error messages on Console Out and Error</summary>
	/// <author>David Dossot</author>
	public sealed class BREFactoryConsole:BREFactory {
		private int errorLevel;
		private int logLevel;
		private BREFactory bref = null;
		
		public BREFactoryConsole(int errorLevel, int logLevel)
		{
			//FIXME: make FlowEngine output to console out and error if possible to separate
			this.errorLevel = errorLevel;
			this.logLevel = logLevel;
		}
				
		public override IFlowEngine NewBRE(IRulesDriver rulesDriver) {
			if (bref == null)	bref = new BREFactory();
			
			return bref.NewBRE(rulesDriver);
		}

	}
	
}
