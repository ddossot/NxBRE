namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;

	/// <summary> This rule will always throw an exception.
	/// *
	/// </summary>
	/// <P>
	/// <PRE>
	/// CHANGELOG:
	/// v1.0	- Created
	/// v1.01	- Removed setRuleParams and instead added a Map param to
	/// executeRule as required in the new BRERuleFactory
	/// Removed BRERuleFatalException throw from executeRule
	/// v1.5	- Renamed and moved to different package
	/// </PRE>
	/// </P>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  1.5
	/// </version>
	public class Exception : IBRERuleFactory
	{
		
		public const string MESSAGE = "Message";

		/// <summary> Throws a BRERuleException with the message "Test Exception"
		/// unless the parameter Message provides a specific message.
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
		public virtual object ExecuteRule(IBRERuleContext aBrc, Hashtable aMap, object aStep)
		{
			string message = (string) aMap[MESSAGE];
			if (message == null) throw new BRERuleException("Test Exception");
			else throw new BRERuleException(message);
		}
	}
}
