namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;

	/// <summary> This rule will always throw a fatal exception
	/// *
	/// </summary>
	/// <P>
	/// <PRE>
	/// CHANGELOG:
	/// v1.0	- Created
	/// v1.01	- Removed setRuleParams and instead added a Map param to
	/// executeRule as required in the new BRERuleFactory
	/// Removed BRERuleException throw from executeRule
	/// v1.5	- Renamed and moved to different package
	/// v1.6	- Made final
	/// </PRE>
	/// </P>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  1.6
	/// </version>
	public sealed class FatalException : IBRERuleFactory
	{
		
		public const string MESSAGE = "Message";
		
		/// <summary> Throws a BRERuleFatalException with the message "Test Fatal Exception"
		/// *
		/// </summary>
		/// <param name="aBrc">- The BRERuleContext object
		/// </param>
		/// <param name="aMap">- The Map of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns> Nothing. It throws an exception each time
		/// 
		/// </returns>
		public object ExecuteRule(IBRERuleContext aBrc, Hashtable aMap, object aStep)
		{
			string message = (string) aMap[MESSAGE];
			if (message == null) throw new BRERuleFatalException("Test Fatal Exception");
			else throw new BRERuleFatalException(message);
		}
	}
}
