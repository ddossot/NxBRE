namespace NxBRE.FlowEngine.Factories
{
	using System;
	
	using net.ideaity.util.events;

	using NxBRE.FlowEngine.IO;
	
	/// <summary>This factory allows an easy creation of a BRE object,
	/// that redirects its log and error messages on Console Out and Error</summary>
	/// <author>David Dossot</author>
	/// <version>1.8.2</version>
	public sealed class BREFactoryConsole:BREFactory {
		private int errorLevel;
		private int logLevel;
		private BREFactory bref = null;
		
		public BREFactoryConsole(int errorLevel, int logLevel)
		{
			this.errorLevel = errorLevel;
			this.logLevel = logLevel;
		}
				
		private void HandleExceptionEvent(object obj, IExceptionEvent aException)
		{
			if (aException.Priority >= errorLevel)
				Console.Error.WriteLine("NxBRE ERROR " + aException.Priority + ": " + aException.Exception.ToString());
		}
		
		private void HandleLogEvent(object obj, ILogEvent aLog)
		{
			if (aLog.Priority >= logLevel)
				Console.Out.WriteLine("NxBRE LOG " + aLog.Priority + " MSG  : " + aLog.Message);
		}

		public override IFlowEngine NewBRE(IRulesDriver rulesDriver) {
			if (bref == null)
				bref = new BREFactory(new DispatchException(HandleExceptionEvent),
			               					new DispatchLog(HandleLogEvent));

			return bref.NewBRE(rulesDriver);
		}

	}
	
}
