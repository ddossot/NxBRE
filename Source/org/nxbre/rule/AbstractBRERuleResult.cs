namespace org.nxbre.rule
{
	using System;
	/// <summary> Abstract definition of BRERuleResult.
	/// Use this if you wish to do inheritance.
	/// *
	/// </summary>
	/// <seealso cref="org.nxbre.rule.IBRERuleResult"/>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  1.0
	/// </version>
	public abstract class AbstractBRERuleResult : IBRERuleResult
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
		
		
		
		/// <summary> Display Method
		/// *
		/// </summary>
		/// <returns> String containing info
		/// 
		/// </returns>
		public override string ToString()
		{
			return "MetaData   :\n"
						 + metaData.ToString()
						 + "\nResult Type: "
						 + result.GetType().FullName
						 + "\nResult Str : "
						 + result.ToString()
						 + "\n";
		}
	}
}
