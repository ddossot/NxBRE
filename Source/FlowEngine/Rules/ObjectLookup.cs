namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;

	using NxBRE.FlowEngine;
	using NxBRE.Util;

	/// <summary> This class is designed to be used anytime you wish to access
	/// a member of an object stored in the Rule Context and referenced by an Id
	/// or a static method on a class.
	/// <P>
	/// This class takes the following parameters:</P><P>
	/// Member - The member to access: field, property or method name.</P><P>
	/// ArgumentX - The argument(s) to be passed to the constructor</P><P>
	/// ObjectId - The Id of the object to access in the result context</P><P>
	/// - or -</P><P>
	/// Class - The full class name, optionally suffixed with the assembly name</P>
	/// <P>
	/// Example:
	/// <PRE>
	/// <Rule id="VALUE1" factory="NxBRE.FlowEngine.rule.helpers.ObjectLookup">
	/// <Parameter name="ObjectId" value="myObject"/>
	/// <Parameter name="Member" value="myMethod"/>
	/// </Rule>
	/// <Rule id="VALUE2" factory="NxBRE.FlowEngine.rule.helpers.ObjectLookup">
	/// <Parameter name="Type" value="test.MyClass,MyAssembly"/>
	/// <Parameter name="Member" value="myStaticMethod"/>
	/// <Parameter name="Argument0" value="a_string"/>
	/// <Parameter name="Argument1" ruleValue="an_object"/>
	/// <Parameter name="Argument2" value="5.25" type="System.Double"/>
	/// </Rule>
	/// </PRE>
	/// *
	/// </P>
	/// </summary>
	/// <author>David Dossot</author>
	public sealed class ObjectLookup : IBRERuleFactory
	{
		public const string OBJECTID = "ObjectId";
		public const string TYPE = "Type";
		public const string MEMBER = "Member";
		public const string ARGUMENT = "Argument";

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
			bool staticCall = false;
			if (!aMap.Contains(OBJECTID))
			{
				if (!aMap.Contains(TYPE))
					throw new BRERuleException("Parameter 'Type' or 'ObjectId' not found");
				else staticCall = true;
			}
			if (!aMap.Contains(MEMBER))
			{
				throw new BRERuleException("Parameter 'Member' not found");
			}
			else
			{
				if (staticCall)
					return Reflection.ClassCall((string)aMap[TYPE],
						                           (string)aMap[MEMBER],
						                           GetArguments(aMap));
				else
					return Reflection.ObjectCall(aBrc.GetResult(aMap[OBJECTID]).Result,
						                           (string)aMap[MEMBER],
						                           GetArguments(aMap));
			}
		}
			
		public object[] GetArguments(IDictionary aMap) {
			ArrayList arguments = new ArrayList();
			int i=0;
			while (aMap.Contains(ARGUMENT + i)) {
				arguments.Add(aMap[ARGUMENT + i]);
				i++;
			}
			return arguments.ToArray();
		}
			
	}
}
