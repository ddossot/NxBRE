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
	/// <P>
	/// This class takes the following parameters:</P><P>
	/// Type - The type of the set (i.e java.lang.* or your own, optionally suffixed with the assembly name)
	/// Value - The value to be set for basic primitives</P><P>
	/// - or -
	/// ArgumentX - The argument(s) to be passed to the constructor
	/// </P>
	/// <P>
	/// Example:
	/// <PRE>
	/// <Rule id="VALUE1" factory="NxBRE.FlowEngine.rule.helpers.Value">
	/// <Parameter name="Value" value="5"/>
	/// <Parameter name="Type" value="System.Int32"/>
	/// </Rule>
	/// <Rule id="VALUE2" factory="NxBRE.FlowEngine.rule.helpers.Value">
	/// <Parameter name="Type" value="test.MyClass,MyAssembly"/>
	/// <Parameter name="Argument0" value="a_string"/>
	/// <Parameter name="Argument1" ruleValue="an_object"/>
	/// <Parameter name="Argument2" value="5.25" type="System.Double"/>
	/// </Rule>
	/// </PRE>
	/// </P>
	/// *
	/// </summary>
	/// <P>
	/// <PRE>
	/// CHANGELOG:
	/// v1.5	- Created
	/// v1.6	- Made final
	/// v1.7	- Changed aMap.get(VALUE) to aMap.get(VALUE).toString()
	/// v2.2  - Corrected bug 1044404 (Assert doesnt support empty constructor)
	/// </PRE>
	/// </P>
	/// *
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  1.7
	/// </version>
	public sealed class Value : IBRERuleFactory
	{
		
		public const string VALUE = "Value";
		
		public const string TYPE = "Type";
		
		/// <summary> Returns a value cast to a specific type
		/// *
		/// </summary>
		/// <param name="aBrc">- The BRERuleContext object
		/// </param>
		/// <param name="aMap">- The Map of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns> The value cast to the specified type
		/// 
		/// </returns>
		public object ExecuteRule(IBRERuleContext aBrc, Hashtable aMap, object aStep)
		{
			if (!aMap.ContainsKey(TYPE))
			{
				throw new BRERuleException("Parameter 'Type' not found");
			}
			else
			{
				if (!aMap.ContainsKey(VALUE))
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
