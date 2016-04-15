namespace NxBRE.FlowEngine
{
	using System;
	/// <summary> This object defines the interface for all objects that wish to contain
	/// a result created by a BusinessRuleFactory.
	/// *
	/// The result can be of any type.  The BRERuleMetaData object contains 
	/// information specific to this result.
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public interface IBRERuleResult
		{
			/// <summary> Returns the MetaData specific to this BusinessRuleResult
			/// *
			/// </summary>
			/// <returns> The relative metadata
			/// 
			/// </returns>
			IBRERuleMetaData MetaData
			{
				get;
				
			}
			/// <summary> Returns the result of the Business Rule
			/// *
			/// </summary>
			/// <returns> The object resulting from the execution of the Business Rule
			/// 
			/// </returns>
			object Result
			{
				get;
				
			}
		}
}
