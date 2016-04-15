namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	
	using NxBRE.FlowEngine;
	using NxBRE.Util;

	/// <summary> This rule will always throw an exception.
	/// </summary>
	public class Exception : IBRERuleFactory
	{
		
		public const string MESSAGE = "Message";

		/// <summary> Throws a BRERuleException with the message "Test Exception"
		/// unless the parameter Message provides a specific message.
		/// *
		/// </summary>
		/// <param name="aBrc">- The BRERuleContext object
		/// </param>
		/// <param name="aMap">- The IDictionary of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns>The Exception</returns>
		public virtual object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			string message = (string)aMap[MESSAGE];

			BRERuleException breRuleException;
			if (message == null) breRuleException = new BRERuleException();
			else breRuleException = new BRERuleException(message);			
			
			if (Logger.IsFlowEngineRuleBaseError) Logger.FlowEngineRuleBaseSource.TraceData(TraceEventType.Error, 0, breRuleException);
			
			return breRuleException;
		}
	}
}
