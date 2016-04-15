namespace NxBRE.FlowEngine
{
	using System;
	using System.Collections;
	/// <summary> This interface defines an object that will be passed between
	/// the various objects that implement BRERuleFactory (BRF).  
	/// The intent of this object is to provide a free-form manner of
	/// passing information between the various BRF's to allow them
	/// access to specific information they may require for their business
	/// decisions.
	/// </summary>
	/// <author>  Sloan Seaman </author>
	/// <author>  David Dossot </author>
	public interface IBRERuleContext : ICloneable
		{
			/// <summary> Returns an Stack of BRERuleFactory UID's that represents
			/// the call stack up to the point of this method invocation
			/// *
			/// </summary>
			/// <returns> An Stack of BRERuleFactory UID's
			/// 
			/// </returns>
			Stack CallStack
			{
				get;
				
			}
			/// <summary> Returns an IDictionary of the RuleFactories
			/// *
			/// </summary>
			/// <returns> IDictionary of RuleFactories
			/// 
			/// </returns>
			IDictionary FactoryMap
			{
				get;
				
			}
			/// <summary> Returns an IDictionary of the Operator
			/// *
			/// </summary>
			/// <returns> IDictionary of the Operators
			/// 
			/// </returns>
			IDictionary OperatorMap
			{
				get;
				
			}
			/// <summary> Returns an IDictionary of the Results
			/// *
			/// </summary>
			/// <returns> IDictionary of Results Map
			/// 
			/// </returns>
			IDictionary ResultsMap
			{
				get;
				
			}
			/// <summary> Returns a BRERuleFactory specified by the specific UID
			/// *
			/// </summary>
			/// <param name="aId">The UID of the RuleFactory
			/// </param>
			/// <returns> The requested RuleFactory
			/// 
			/// </returns>
			IBRERuleFactory GetFactory(object aId);
			/// <summary> Returns the requested Operator
			/// *
			/// </summary>
			/// <returns> The requested Operator
			/// 
			/// </returns>
			IBREOperator GetOperator(object aId);
			/// <summary> Returns a BusinessRuleResult that was generated from a BRF
			/// *
			/// </summary>
			/// <param name="aId">The UID of the ResultSet
			/// </param>
			/// <returns> The requested RuleResult
			/// 
			/// </returns>
			IBRERuleResult GetResult(object aId);
			/// <summary> Sets a RuleFactory
			/// *
			/// </summary>
			/// <param name="aId">The UID of the RuleFactory
			/// </param>
			/// <param name="aFactory">The Factory
			/// 
			/// </param>
			void  SetFactory(object aId, IBRERuleFactory aFactory);
			/// <summary> Sets an Operator
			/// *
			/// </summary>
			/// <param name="aId">The UID of the Operator
			/// </param>
			/// <param name="aOperator">The operator
			/// 
			/// </param>
			void  SetOperator(object aId, IBREOperator aOperator);
			/// <summary> Sets a RuleResult
			/// *
			/// </summary>
			/// <param name="aId">The UID of the RuleResult
			/// </param>
			/// <param name="aResult">The RuleResult
			/// 
			/// </param>
			void  SetResult(object aId, IBRERuleResult aResult);
			/// <summary> Sets a business object
			/// *
			/// </summary>
			/// <param name="aId">The UID of the business object
			/// </param>
			/// <param name="aObject">The business object</param>
			void  SetObject(object aId, object aObject);
			/// <summary> Returns a business object
			/// *
			/// </summary>
			/// <param name="aId">The UID of the business object
			/// </param>
			/// <returns> The requested business object
			/// 
			/// </returns>
			object GetObject(object aId);

		}
}
