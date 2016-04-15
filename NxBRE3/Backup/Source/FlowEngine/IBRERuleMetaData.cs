namespace NxBRE.FlowEngine
{
	using System;
	using System.Collections;
	/// <summary> This interface defines information about a specific Business Rule. 
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public interface IBRERuleMetaData
		{
			/// <summary> Returns the BRERuleFactory that created the Business Rule
			/// *
			/// </summary>
			/// <returns>s The parent BRERuleFactory
			/// 
			/// </returns>
			IBRERuleFactory Factory
			{
				get;
				
			}
			/// <summary> Returns a UID for this result set
			/// *
			/// </summary>
			/// <returns> The UID of this result
			/// 
			/// </returns>
			object Id
			{
				get;
				
			}
			/// <summary> Returns any parameters that were passed in to the specific instance of this
			/// Rule
			/// *
			/// </summary>
			/// <returns> An IDictionary of the parameters
			/// 
			/// </returns>
			IDictionary Parameters
			{
				get;
				
			}
			/// <summary> Returns the location that the RuleResult was returned from in 
			/// the call stack
			/// *
			/// </summary>
			/// <returns> The location of the RuleResult on the stack
			/// 
			/// </returns>
			int StackLocation
			{
				get;
				
			}
			/// <summary> Returns the step that was called in the executeRule() within
			/// the parent BRERuleFactory.
			/// Note: Can be NULL!
			/// *
			/// </summary>
			/// <returns>The step that was called.
			/// 
			/// </returns>
			object Step
			{
				get;
				
			}
		}
}
