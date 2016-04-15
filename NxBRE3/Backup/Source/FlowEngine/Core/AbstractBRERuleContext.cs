namespace NxBRE.FlowEngine.Core
{
	using System;
	using System.Collections;
	using System.Reflection;
	
	/// <summary> Abstract definition of BRERuleContext.
	/// There is a good chance that RuleContexts may change per
	/// client, so an abstract object is provided to assist this.
	/// Use this if you wish to do inheritance.
	/// </summary>
	/// <seealso cref="NxBRE.FlowEngine.Core.BRERuleContextImpl"/>
	/// <author>  Sloan Seaman </author>
	/// <author>  David Dossot </author>
	internal abstract class AbstractBRERuleContext : IBRERuleContext
	{
		protected Stack internalCallStack;
		protected IDictionary factories = null;
		protected IDictionary operators = null;
		protected IDictionary results = null;
		
	
		/// <summary> Returns a Stack of BRERuleFactory UID's that represents
		/// the call stack up to the point of this method invocation
		/// *
		/// </summary>
		/// <returns> An Stack of BRERuleFactory UID's
		/// 
		/// </returns>
		virtual public Stack CallStack
		{
			get
			{
				return internalCallStack;
			}
			
		}
		/// <summary> Returns a Map of the RuleFactories
		/// *
		/// </summary>
		/// <returns> Map of RuleFactories
		/// 
		/// </returns>
		virtual public IDictionary FactoryMap
		{
			get
			{
				return factories;
			}
			
		}
		/// <summary> Returns the internal parameters map
		/// *
		/// </summary>
		/// <returns> The Map object containing all parameters
		/// 
		/// </returns>
		virtual public IDictionary OperatorMap
		{
			get
			{
				return operators;
			}
			
		}
		/// <summary> Returns a Map of the Results
		/// *
		/// </summary>
		/// <returns> Map of Results Map
		/// 
		/// </returns>
		virtual public IDictionary ResultsMap
		{
			get
			{
				return results;
			}
			
		}
		/// <summary> Protected constructor to allow for data structure population
		/// *
		/// </summary>
		/// <param name="aStack">The Stack for the call stack
		/// </param>
		/// <param name="aResults">The IDictionary for the RuleResults
		/// </param>
		/// <param name="aFactories">The IDictionary for the RuleFactory's
		/// </param>
		/// <param name="aOperators">The IDictionary for the Operators
		/// 
		/// </param>
		protected internal AbstractBRERuleContext(Stack aStack, IDictionary aFactories, IDictionary aOperators, IDictionary aResults)
		{
			internalCallStack = aStack;
			factories = aFactories;
			operators = aOperators;
			results = aResults;
		}
				
		/// <summary> Performs a shallow copy of the Rule Context, i.e. returns a new RuleContext
		/// containing shallow copies of its internal hashtables and stack
		/// </summary>
		public abstract object Clone();


		/// <summary> Returns a BRERuleFactory specified by the specific UID
		/// *
		/// </summary>
		/// <param name="aId">The UID of the RuleFactory
		/// </param>
		/// <returns> The requested RuleFactory
		/// @throws ClassCastException If object retrieved is not of type BRERuleFactory
		/// (This is only possible if the developer passes in a Map object that
		/// is already populated with other objects)
		/// 
		/// </returns>
		public virtual IBRERuleFactory GetFactory(object aId)
		{
			if ((factories != null) && (factories.Contains(aId)))
				return (IBRERuleFactory) factories[aId];
			return null;
		}
		
		
		/// <summary> Returns a parameter that was set for use by the RuleFactory
		/// *
		/// </summary>
		/// <param name="aId">The UID of the parameter
		/// </param>
		/// <returns> The corresponding value
		/// 
		/// </returns>
		public virtual IBREOperator GetOperator(object aId)
		{
			if ((operators != null) && (operators.Contains(aId)))
				return (IBREOperator) operators[aId];
			return null;
		}
		
		
		/// <summary> Returns a BusinessRuleResult that was generated from a BRF
		/// *
		/// </summary>
		/// <param name="aId">The UID of the ResultSet
		/// </param>
		/// <returns> The requested RuleResult
		/// @throws ClassCastException If object retrieved is not of type BRERuleResult
		/// (This is only possible if the developer passes in a Map object that
		/// is already populated with other objects)
		/// 
		/// </returns>
		public virtual IBRERuleResult GetResult(object aId)
		{
			if ((results != null) && (results.Contains(aId)))
			{
				return (IBRERuleResult) results[aId];
			}
			return null;
		}
		
		
		/// <summary> Sets a RuleFactory
		/// *
		/// </summary>
		/// <param name="aId">The UID of the RuleFactory
		/// </param>
		/// <param name="aFactory">The Factory
		/// 
		/// </param>
		public virtual void  SetFactory(object aId, IBRERuleFactory aFactory)
		{
			if (factories != null) {
				if (factories.Contains(aId)) factories.Remove(aId);
				factories.Add(aId, aFactory);
			}
		}
		
		/// <summary> Sets a parameter for use by RuleFactories
		/// *
		/// </summary>
		/// <param name="aId">The UID of the parameter
		/// </param>
		/// <param name="aValue">The value to set
		/// 
		/// </param>
		public virtual void  SetOperator(object aId, IBREOperator aValue)
		{
			if (operators != null) {
				if (operators.Contains(aId)) operators.Remove(aId);
				operators.Add(aId, aValue);
			}
		}
		
		/// <summary> Sets a RuleResult
		/// *
		/// </summary>
		/// <param name="aId">The UID of the RuleResult
		/// </param>
		/// <param name="aResult">The RuleResult
		/// 
		/// </param>
		public virtual void  SetResult(object aId, IBRERuleResult aResult)
		{
			if (results != null) {
				if (results.Contains(aId)) results.Remove(aId);
				results.Add(aId, aResult);
			}
		}
		
		/// <summary> Sets a business object
		/// *
		/// </summary>
		/// <param name="aId">The UID of the business object
		/// </param>
		/// <param name="aObject">The business object
		/// 
		/// </param>
		public abstract void SetObject(object aId, object aObject);
		/// <summary> Returns a business object
		/// *
		/// </summary>
		/// <param name="aId">The UID of the business object
		/// </param>
		/// <returns> The requested business object
		/// 
		/// </returns>
		public abstract object GetObject(object aId);
		
		/// <summary> Display Method
		/// *
		/// </summary>
		/// <returns> String containing info
		/// 
		/// </returns>
		public override string ToString()
		{
			return "*** Stack ***\n"
							+ AddEnumeration(internalCallStack.GetEnumerator())
							+ "\n*** Factories ***\n"
							+ AddEnumeration(factories.GetEnumerator()) 
							+ "\n*** Operators ***\n"
							+ AddEnumeration(operators.GetEnumerator()) 
							+ "\n*** Results ***\n"
							+ AddEnumeration(results.GetEnumerator()); 
		}
		
		private string AddEnumeration(IEnumerator enumerator) {
			string result = "";
			object entry;
			while (enumerator.MoveNext()) {
				entry = enumerator.Current;
				if (entry is DictionaryEntry)
					result += ((DictionaryEntry)entry).Key + " -> " + ((DictionaryEntry)entry).Value + "\n";
				else
					result += "-> " + entry.ToString() + "\n";
			}
			return result;
		}
	}
}
