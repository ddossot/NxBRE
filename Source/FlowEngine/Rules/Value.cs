namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;

	using NxBRE.FlowEngine;
	using NxBRE.Util;

	/// <summary> This class is designed to be used anytime you wish to set
	/// a value in the XML itself. 
	/// <P>
	/// The value can be of any type and of any value.
	/// </P>
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public sealed class Value : IBRERuleFactory
	{
		
		public const string VALUE = "Value";
		
		public const string TYPE = "Type";
		
		/// <summary> Returns a value cast to a specific type
		/// *
		/// </summary>
		/// <param name="aBrc">- The BRERuleContext object
		/// </param>
		/// <param name="aMap">- The IDictionary of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns> The value cast to the specified type
		/// 
		/// </returns>
		public object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			if (!aMap.Contains(TYPE))
			{
				throw new BRERuleException("Parameter 'Type' not found");
			}
			else
			{
				if (!aMap.Contains(VALUE))
				{
					ObjectLookup ol = new ObjectLookup();
					object[] arguments = ol.GetArguments(aMap);
					return Reflection.ClassNew((string)aMap[TYPE], arguments);
				}
				return Reflection.CastValue(aMap[VALUE], (string)aMap[TYPE]);
			}
		}
		
	}
}
