namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	
	using NxBRE.FlowEngine;
	using NxBRE.Util;

	/// <summary> This rule will always throw a fatal exception
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public sealed class FatalException : IBRERuleFactory
	{
		
		public const string MESSAGE = "Message";
		
		/// <summary> Throws a BRERuleFatalException with the message "Test Fatal Exception"
		/// *
		/// </summary>
		/// <param name="aBrc">- The BRERuleContext object
		/// </param>
		/// <param name="aMap">- The IDictionary of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns>The exception</returns>
		public object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			string message = (string) aMap[MESSAGE];
			
			BRERuleFatalException breRuleFatalException;
			if (message == null) breRuleFatalException = new BRERuleFatalException();
			else breRuleFatalException = new BRERuleFatalException(message);			
			
			if (Logger.IsFlowEngineRuleBaseCritical) Logger.FlowEngineRuleBaseSource.TraceData(TraceEventType.Critical, 0, breRuleFatalException);
			
			return breRuleFatalException;
		}
	}
}
