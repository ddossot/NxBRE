namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;

	/// <summary> Compares two objects to see if they are equal.
	/// </summary>
	public sealed class Equals : IBREOperator
	{
		/// <summary>Defines whether an operator is able to deal with empty operands</summary>
		public bool AcceptsNulls => true;

	    /// <summary> Checks the two objects to see if they are equal
		/// </summary>
		public bool ExecuteComparison(IBRERuleContext aBRC, IDictionary aMap, object aObj, object aCompareTo)
	    {
	        if ((aObj == null) || (aCompareTo == null)) {
				return false;
			}
	        var obj = aObj as IComparable;
	        if ((obj != null) && (aCompareTo is IComparable)) {
	            return obj.CompareTo(aCompareTo) == 0;
	        }
	        return aCompareTo.Equals(aObj);
	    }
	}
}
