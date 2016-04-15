namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;

	/// <summary> This class is designed to be used to decrement everytime the executeRule is called
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public class Decrement : IBRERuleFactory, IInitializable
	{
		public const string DECREMENT = "Decrement";
		
		private int sIdx = 0;
		
		/// <summary> Takes an Integer with a value equal to the starting index point
		/// *
		/// </summary>
		/// <param name="aObj">An Integer representing the starting index point
		/// 
		/// </param>
		public virtual bool Init(object aObj)
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
		
		/// <summary> If passed the parameter Decrement it will decrement the index.
		/// Otherwise it will just return the current value of the index
		/// *
		/// </summary>
		/// <param name="aBrc">- The BRERuleContext object
		/// </param>
		/// <param name="aMap">- The IDictionary of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns> The current value of the index
		/// 
		/// </returns>
		public virtual object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			if (aMap.Contains(DECREMENT))
			{
				sIdx = sIdx - System.Int32.Parse((System.String) aMap[DECREMENT]);
			}
			return sIdx;
		}
	}
}
