namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;

	using net.ideaity.util;
	using NxBRE.FlowEngine;

	/// <summary> Compares two objects to see if they are less than or equal to each other.
	/// *
	/// </summary>
	/// <P>
	/// <PRE>
	/// CHANGELOG:
	/// v1.5	- Created
	/// </PRE>
	/// </P>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  1.5
	/// </version>
	public sealed class LessThanEqualTo : IBREOperator, IInitializable
	{
		/// <summary>Defines whether an operator is ablt to deal with empty operands</summary>
		/// <returns>False for the current operator</returns>
		public bool AcceptsNulls {
			get {
				return false;
			}
		}

		/// <summary> Vairable that holds the optimism
		/// </summary>
		private bool optimistic = false;
		
		/// <summary> Takes a Boolean as its object.  The Boolean represents
		/// if the object should be pessistic or optimistic when 
		/// doing a comparison. I.E. if it cannot do a comparison
		/// does it return true or false.  Default is false.
		/// *
		/// </summary>
		/// <param name="aObj">A Boolean containing the optimism
		/// 
		/// </param>
		public bool Init(object aObj)
		{
			if (aObj is System.Boolean)
			{
				optimistic = ((System.Boolean) aObj);
				return true;
			}
			return false;
		}
		
		/// <summary> Compares two objects to see if they are less than or equal to each other
		/// <P>
		/// If the objects are not <code>Comparable</code> then we default
		/// to the optimism that is set.  The default is false.
		/// *
		/// </P>
		/// </summary>
		/// <param name="aBRC">The BRERuleContext object containing all the state
		/// information for use by this method.
		/// </param>
		/// <param name="aMap">Not used yet. A Map object containing the parameters from the XML
		/// </param>
		/// <param name="aObj">The object (value) to compare against.
		/// </param>
		/// <param name="aCompareTo">The object to compare it to.
		/// </param>
		/// <returns>s True if they meet the condition, False otherwise.
		/// 
		/// </returns>
		public bool ExecuteComparison(IBRERuleContext aBRC, Hashtable aMap, object aObj, object aCompareTo)
		{
			Init(aMap);
			
			if ((aObj is IComparable) && (aCompareTo is IComparable))
			{
				if (((IComparable) aObj).CompareTo(aCompareTo) <= 0)
				{
					return true;
				}
				return false;
			}
			else
			{
				return optimistic;
			}
		}
	}
}
