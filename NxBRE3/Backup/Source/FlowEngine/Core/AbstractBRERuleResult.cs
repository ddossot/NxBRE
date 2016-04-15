namespace NxBRE.FlowEngine.Core
{
	using System;
	using System.Text;
	
	/// <summary> Abstract definition of BRERuleResult.
	/// Use this if you wish to do inheritance.
	/// *
	/// </summary>
	/// <seealso cref="NxBRE.FlowEngine.IBRERuleResult"/>
	/// <author>Sloan Seaman</author>
	/// <author>David Dossot</author>
	internal abstract class AbstractBRERuleResult : IBRERuleResult
	{
		/// <summary> Returns the MetaData specific to this BusinessRuleResult
		/// *
		/// </summary>
		/// <returns> The relative metadata
		/// 
		/// </returns>
		virtual public IBRERuleMetaData MetaData
		{
			get
			{
				return metaData;
			}
			
		}
		/// <summary> Returns the result of the Business Rule
		/// *
		/// </summary>
		/// <returns> The object resulting from the execution of the Business Rule
		/// 
		/// </returns>
		virtual public object Result
		{
			get
			{
				return result;
			}
			
		}
		private IBRERuleMetaData metaData = null;
		
		private object result = null;
		
		/// <summary> Protected constructor to allow for data structure population
		/// *
		/// </summary>
		/// <param name="aMetaData">The RuleMetaData of the RuleResult
		/// </param>
		/// <param name="aResult">The actual result
		/// 
		/// </param>
		protected internal AbstractBRERuleResult(IBRERuleMetaData aMetaData, object aResult)
		{
			metaData = aMetaData;
			result = aResult;
		}
		
		
		
		/// <summary> Display Method </summary>
		/// <returns> String containing info</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			if (metaData != null) sb.Append("MetaData   :\n").Append(metaData.ToString()).Append("\n");
			
			if (result != null) sb.Append("Result Type: ").Append(result.GetType().FullName).Append("\nResult Str : ").Append(result.ToString()).Append("\n");
			else sb.Append("Result: Null");
				
			return sb.ToString();
		}
	}
}
