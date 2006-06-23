namespace org.nxbre.ri.helpers.rule
{
	using System;
	using System.Collections;
	
	using net.ideaity.util;
	using org.nxbre.rule;

	/// <summary> This class is designed to be used to decrement everytime the executeRule is called
	/// <P>
	/// This class takes the following parameter:</P><P>
	/// StartIdx - The initial starting index (defaults to 0).
	/// </P>
	/// <P>
	/// Example:
	/// </P>
	/// <PRE>
	/// <Rule id="VALUE1" factory="org.nxbre.ri.rule.helpers.Decrement">
	/// <Parameter name="StartIdx" value="1"/>
	/// </Rule>
	/// </PRE>
	/// <P>
	/// <B>NOTE:</B>
	/// This will only decrement if you pass a Decrement parameter.
	/// Otherwise it returns the current value
	/// </P>
	/// *
	/// </summary>
	/// <PRE>
	/// CHANGELOG:
	/// v1.5	- Created
	/// </PRE>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  1.5
	/// </version>
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
		/// <param name="aMap">- The Map of parameters from the XML
		/// </param>
		/// <param name="aStep">- The step that it is on
		/// </param>
		/// <returns> The current value of the index
		/// 
		/// </returns>
		public virtual object ExecuteRule(IBRERuleContext aBrc, Hashtable aMap, object aStep)
		{
			if (aMap.ContainsKey(DECREMENT))
			{
				sIdx = sIdx - System.Int32.Parse((System.String) aMap[DECREMENT]);
			}
			return sIdx;
		}
	}
}
