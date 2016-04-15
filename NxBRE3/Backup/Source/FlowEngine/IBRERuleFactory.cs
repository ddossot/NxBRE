namespace NxBRE.FlowEngine
{
	using System;
	using System.Collections;
	/// <summary> The BRERuleFactory defines the default interface to use with the 
	/// Business Rule Engine (BRE).  
	/// A class implementing BRERuleFactory is callable by the BRE to execute 
	/// specific business rule code based on the information provided by the
	/// BRERuleContext object.
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public interface IBRERuleFactory
		{
			/// <summary> The executeRule() method is invoked by the Business Rule Engine (BRE)
			/// to execute the specific rule for this class.
			/// *
			/// </summary>
			/// <param name="aBRC">The BRERuleContext object containing all the state
			/// information for use by this method.
			/// </param>
			/// <param name="aStep">The specific rule to execute.  This allows the same class
			/// and method to be used for multiple Business Rules.
			/// </param>
			/// <param name="aMap">An IDictionary object containing the parameters from the XML
			/// </param>
			/// <returns> The object containing the results of the
			/// rule.
			/// 
			/// </returns>
			object ExecuteRule(IBRERuleContext aBRC, IDictionary aMap, object aStep);
		}
}
