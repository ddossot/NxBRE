namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;

	using NxBRE.FlowEngine;

	/// <summary> Compares two objects to see if they are not equal.
	/// </summary>
	public sealed class NotEquals : IBREOperator
	{
		/// <summary>Defines whether an operator is ablt to deal with empty operands</summary>
		public bool AcceptsNulls {
			get {
				return true;
			}
		}
		
		public bool ExecuteComparison(IBRERuleContext aBRC, IDictionary aMap, object aObj, object aCompareTo)
		{
		    if ((aObj == null) || (aCompareTo == null)) {
				return true;
			}
		    var obj = aObj as IComparable;
		    if ((obj != null) && (aCompareTo is IComparable)) {
		        return obj.CompareTo(aCompareTo) != 0;
		    }
		    return !aCompareTo.Equals(aObj);
		}
	}
}
