namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;

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
		/// <returns> Nothing. It throws an exception each time
		/// 
		/// </returns>
		public object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			string message = (string) aMap[MESSAGE];
			if (message == null) throw new BRERuleFatalException("Test Fatal Exception");
			else throw new BRERuleFatalException(message);
		}
	}
}
