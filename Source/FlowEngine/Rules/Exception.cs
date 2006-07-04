namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;

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
		/// <returns> Nothing. It throws an exception each time
		/// 
		/// </returns>
		public virtual object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			string message = (string) aMap[MESSAGE];
			if (message == null) throw new BRERuleException("Test Exception");
			else throw new BRERuleException(message);
		}
	}
}
