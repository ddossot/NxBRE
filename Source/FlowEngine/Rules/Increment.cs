namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using net.ideaity.util;
	using NxBRE.FlowEngine;

	/// <summary> This class is designed to be used to increment everytime the executeRule is called.
	/// <P>
	/// This class takes the following parameter:</P><P>
	/// StartIdx - The initial starting index (defaults to 0)</P><P>
	/// </P>
	/// Example:<P>
	/// <PRE>
	/// <Rule id="VALUE1" factory="NxBRE.FlowEngine.rule.helpers.Increment">
	/// <Parameter name="Increment" value="1"/>
	/// </Rule>
	/// </PRE>
	/// </P>
	/// <P>
	/// <B>NOTE:</B>
	/// This will only increment if you pass an Increment parameter.
	/// Otherwise it returns the current value
	/// </P>
	/// *
	/// </summary>
	/// <PRE>
	/// CHANGELOG:
	/// v1.5	- Created
	/// V1.6	- Made FINAL
	/// </PRE>
	/// <author>  Sloan Seaman
	/// </author>
	public sealed class Increment : IBRERuleFactory, IInitializable
	{
//		public const string INIT = "Init";

		public const string INCREMENT = "Increment";
		
		private int sIdx = 0;
		
		/// <summary> Takes an Integer with a value equal to the starting index point
		/// *
		/// </summary>
		/// <param name="aObj">An Integer representing the starting index point
		/// 
		/// </param>
		public bool Init(object aObj)
		{
			if (aObj is Int32)
			{
				sIdx = ((Int32) aObj);
				return true;
			}
			else if (aObj is string)
			{
				sIdx = Int32.Parse((string) aObj);
				return true;
			}
			return false;
		}
		
		/// <summary> If passed the parameter Increment it will increment the index.
		/// Otherwise it will just return the current value of the index
		/// *
		/// </summary>
		/// <param name="aBrc">- The BRERuleContext object
		/// </param>
		/// <param name="aMap">- The IDictionary of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns> The current value of the increment
		/// 
		/// </returns>
		public object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			if (aMap.Contains(INCREMENT))
				sIdx = sIdx + Int32.Parse((string) aMap[INCREMENT]);
			
			return sIdx;
		}
	}
}
