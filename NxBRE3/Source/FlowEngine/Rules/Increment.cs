namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using FlowEngine;

	/// <summary> This class is designed to be used to increment everytime the executeRule is called.
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public sealed class Increment : IBRERuleFactory, IInitializable
	{
		public const string INCREMENT = "Increment";
		
		private int sIdx = 0;
		
		/// <summary> Takes an Integer with a value equal to the starting index point
		/// </summary>
		/// <param name="aObj">An Integer representing the starting index point
		/// 
		/// </param>
		public bool Init(object aObj)
		{
			if (aObj is int)
			{
				sIdx = ((int) aObj);
				return true;
			}
		    if (!(aObj is string)) return false;
		    sIdx = int.Parse((string) aObj);
		    return true;
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
				sIdx = sIdx + int.Parse((string) aMap[INCREMENT]);
			
			return sIdx;
		}
	}
}
