namespace NxBRE.Test.FlowEngine
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;
	
	public class StringContainsOperator : IBREOperator
	{
		public bool AcceptsNulls {
			get {
				return true;
			}
		}
		
		public bool ExecuteComparison(IBRERuleContext aBRC, IDictionary aMap, object aObj, object aCompareTo) {
			if ((aObj == null) || (aCompareTo == null)) {
				return false;
			}
			else {
				return ((string)aObj).Contains((string)aCompareTo);
			}
			
		}
	}
}
