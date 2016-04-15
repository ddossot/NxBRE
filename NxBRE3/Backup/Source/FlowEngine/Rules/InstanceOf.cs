namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	using System.Diagnostics;

	using NxBRE.FlowEngine;
	using NxBRE.Util;

	/// <summary> Checks if one object is the instance of another
	/// </summary>
	/// <author>  Sloan Seaman </author>
	/// <author>  David Dossot </author>
	public sealed class InstanceOf : IBREOperator
	{
		/// <summary>Defines whether an operator is ablt to deal with empty operands</summary>
		/// <returns>True for the current operator</returns>
		public bool AcceptsNulls {
			get {
				return true;
			}
		}
		
		/// <summary> Checks if one object is the instance of another
		/// </summary>
		/// <param name="aBRC">The BRERuleContext object containing all the state
		/// information for use by this method.
		/// </param>
		/// <param name="aMap">An IDictionary Map object containing the parameters from the XML
		/// </param>
		/// <param name="aObj">The object (value) to compare against.
		/// </param>
		/// <param name="aCompareTo">The object to compare it to.
		/// </param>
		/// <returns>True if they meet the condition, False otherwise.
		/// 
		/// </returns>
		public bool ExecuteComparison(IBRERuleContext aBRC, IDictionary aMap, object aObj, object aCompareTo)
		{
			try {
				if ((aObj == null) || (aCompareTo == null)) {
					return false;
				}
				else if ((aObj.GetType().IsInstanceOfType(aCompareTo)) 
				    || (aObj.GetType().IsSubclassOf(aCompareTo.GetType()))) {
						return true;
				}
				else {
					return false;
				}
			}
			catch(System.Exception e) {
				if (Logger.IsFlowEngineWarning)
					Logger.FlowEngineSource.TraceEvent(TraceEventType.Warning,
																							0,
																							"InstanceOf can not execute comparison with context: " + aBRC +
                                              "\n parameters: " + Misc.IDictionaryToString(aMap) +
                                              "\n first object: " + aObj +
                                              "\n second object: " + aCompareTo +
                                              "\n stacktrace: " + e.StackTrace);
			}
			
			return false;
		}
	}
}
