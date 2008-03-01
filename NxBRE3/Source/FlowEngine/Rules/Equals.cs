namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;

	/// <summary> Compares two objects to see if they are equal.
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public sealed class Equals : IBREOperator
	{
		//TODO refactor to use common comparison abstract class
		
		/// <summary>Defines whether an operator is able to deal with empty operands</summary>
		/// <returns>False for the current operator</returns>
		public bool AcceptsNulls {
			get {
				return false;
			}
		}

		/// <summary> Checks the two objects to see if they are equal
		/// *
		/// </summary>
		/// <param name="aBRC">The BRERuleContext object containing all the state
		/// information for use by this method.
		/// </param>
		/// <param name="aMap">Not used yet. An IDictionary object containing the parameters from the XML
		/// </param>
		/// <param name="aObj">The object (value) to compare against.
		/// </param>
		/// <param name="aCompareTo">The object to compare it to.
		/// </param>
		/// <returns> True if they meet the condition, False otherwise.
		/// </returns>
		public bool ExecuteComparison(IBRERuleContext aBRC, IDictionary aMap, object aObj, object aCompareTo)
		{
			if ((aObj is IComparable) && (aCompareTo is IComparable))
			{
				if (((IComparable) aObj).CompareTo(aCompareTo) == 0)
				{
					return true;
				}
				return false;
			}
			else
			{
				return aCompareTo.Equals(aObj);
			}
		}
	}
}
