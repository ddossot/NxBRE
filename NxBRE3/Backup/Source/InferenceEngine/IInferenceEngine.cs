namespace NxBRE.InferenceEngine
{	
	using System.Collections;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Core;
	using NxBRE.InferenceEngine.Rules;

	/// <summary>
	/// The available types of rules to process.
	/// </summary>
	public enum RuleTypes {
		/// <summary>
		/// Only process performatives, i.e. only perform assertions and retraction as defined in the loaded rule base.
		/// </summary>
		PerformativesOnly,
		/// <summary>
		/// Only process connectives, i.e. only run implications defined in the loaded rule base.
		/// </summary>
		ConnectivesOnly,
		/// <summary>
		/// Process performatives and connectives.
		/// </summary>
		/// <remarks>
		/// This is the default mode of the Inference Engine.
		/// </remarks>
		All
	}
	
	/// <summary>
	/// This interface defines the Inference Engine (IE) of NxBRE.
	/// </summary>
	/// <author>David Dossot</author>
	public interface IInferenceEngine {
		/// <summary>
		/// The event to subscribe in order to be notified of assertion of facts.
		/// </summary>
		event NewFactEvent NewFactHandler;
		
		/// <summary>
		/// The event to subscribe in order to be notified of retraction of facts.
		/// </summary>
		event NewFactEvent DeleteFactHandler;
		
		/// <summary>
		/// The event to subscribe in order to be notified of modification of facts.
		/// </summary>
		event NewFactEvent ModifyFactHandler;
		
		/// <summary>
		/// The optional business object binder type (null if none).
		/// </summary>
		string BinderType { get; }

		/// <summary>
		/// The direction of the loaded rule base.
		/// </summary>
		string Direction { get; }
		
		/// <summary>
		/// The label of the loaded rule base.
		/// </summary>
		string Label { get; }
		
		/// <summary>
		/// The current type of working memory.
		/// </summary>
		WorkingMemoryTypes WorkingMemoryType { get; }

		/// <summary>
		/// Returns true if the engine is initialized with a valid rulebase.
		/// </summary>
		bool Initialized { get; }
		
		/// <summary>
		/// Loads a rule base and process the performatives. The working memory is reset (all facts are lost).
		/// </summary>
		/// <param name="adapter">The Adapter used to read the rule base.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// This is equivalent to calling: <code>LoadRuleBase(adapter, true)</code>
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.IO.IRuleBaseAdapter"/>
		void LoadRuleBase(IRuleBaseAdapter adapter);
		
		/// <summary>
		/// Loads a rule base and process the performatives. The working memory is reset (all facts are lost).
		/// </summary>
		/// <param name="adapter">The Adapter used to read the rule base.</param>
		/// <param name="businessObjectsBinder">The business object binder that the engine must use.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// This is equivalent to calling: <code>LoadRuleBase(adapter, businessObjectsBinder, true)</code>
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.IO.IRuleBaseAdapter"/>
		void LoadRuleBase(IRuleBaseAdapter adapter, IBinder businessObjectsBinder);
		
		/// <summary>
		/// Loads a rule base. The working memory is reset (all facts are lost).
		/// </summary>
		/// <param name="adapter">The Adapter used to read the rule base.</param>
		/// <param name="processPerformatives">Immediatly process the performative actions (assert, retract) found in the rule base.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.IO.IRuleBaseAdapter"/>
		void LoadRuleBase(IRuleBaseAdapter adapter, bool processPerformatives);
		
		/// <summary>
		/// Loads a rule base. The working memory is reset (all facts are lost).
		/// </summary>
		/// <param name="adapter">The Adapter used to read the rule base.</param>
		/// <param name="businessObjectsBinder">The business object binder that the engine must use.</param>
		/// <param name="processPerformatives">Immediatly process the performative actions (assert, retract) found in the rule base.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.IO.IRuleBaseAdapter"/>
		void LoadRuleBase(IRuleBaseAdapter adapter, IBinder businessObjectsBinder, bool processPerformatives);
		
		/// <summary>
		/// Saves the WorkingMemory in a rule base.
		/// </summary>
		/// <param name="adapter">The Adapter used to save the rule base.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.IO.IRuleBaseAdapter"/>
		void SaveRuleBase(IRuleBaseAdapter adapter);
		
		/// <summary>
		/// Load facts in the current working memory. Current implications, facts and queries
		/// remain unchanged.
		/// </summary>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <param name="adapter">The Adapter used to read the fact base.</param>
		void LoadFacts(IFactBaseAdapter adapter);

		/// <summary>
		/// Save facts of the current working memory. Current implications, facts and queries
		/// remain unchanged.
		/// </summary>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <param name="adapter">The Adapter used to save the fact base.</param>
		void SaveFacts(IFactBaseAdapter adapter);
		
		/// <summary>
		/// Sets the WorkingMemory of the engine, either by forking the existing Global memory
		/// to a new Isolated one, or by simply using the Global one.
		/// </summary>
		/// <param name="memoryType">The new type of working memory.</param>
		void NewWorkingMemory(WorkingMemoryTypes memoryType);

		/// <summary>
		/// Makes the current isolated memory the new global memory and sets the working memory
		/// type to global. Throws an exception in the current memory type is not isolated.
		/// </summary>
		void CommitIsolatedMemory();
		
		/// <summary>
		/// Dispose the current isolated memory sets the working memory type to global.
		/// Throws an exception in the current memory type is not isolated.
		/// </summary>
		void DisposeIsolatedMemory();
		
		/// <summary>
		/// Process all the performative and connective rules on the current working memory and stops
		/// infering when no new Fact is deducted or retracted.
		/// </summary>
		/// <remarks>
		/// This is equivalent to calling: <code>Process(ProcessModes.ConnectivesOnly)</code>
		/// </remarks>
		void Process();
		
		/// <summary>
		/// Process the selected rules on the current working memory and stops
		/// infering when no new Fact is deducted or retracted.
		/// </summary>
		/// <param name="ruleType">The particular rule type to process.</param>
		void Process(RuleTypes ruleType);
		
		/// <summary>
		/// Process all the performative and connective rules on the current working memory and stops
		/// infering when no new Fact is deducted or retracted.
		/// </summary>
		/// <remarks>
		/// This is equivalent to calling: <code>Process(businessObjects, ProcessModes.ConnectivesOnly)</code>
		/// If businessObjects is Null, this method performs the same operation as <code>Process()</code>
		///  ; else it uses the binder provided in the constructor to perform fact operations and
		/// orchestrate the process.
		/// If businessObjects is not Null and no binder has been provided in the constructor, it throws
		/// a BREException.
		/// </remarks>
		/// <param name="businessObjects">An IDictionary of business objects, or Null.</param>
		void Process(IDictionary businessObjects);
		
		/// <summary>
		/// Process the selected rules on the current working memory and stops
		/// infering when no new Fact is deducted or retracted.
		/// </summary>
		/// <remarks>
		/// If businessObjects is Null, this method performs the same operation as <code>Process()</code>
		///  ; else it uses the binder provided in the constructor to perform fact operations and
		/// orchestrate the process.
		/// If businessObjects is not Null and no binder has been provided in the constructor, it throws
		/// a BREException.
		/// </remarks>
		/// <param name="businessObjects">An IDictionary of business objects, or Null.</param>
		/// <param name="ruleType">The particular rule type to process.</param>
		void Process(IDictionary businessObjects, RuleTypes ruleType);
		
		/// <summary>
		/// Gets the number of facts in the current working memory.
		/// </summary>
		/// <returns>The number of facts in the current working memory.</returns>
		int FactsCount { get; }
		
		/// <summary>
		/// Gets an enumeration of the facts contained in the working memory.
		/// </summary>
		/// <returns>An IEnumerator on the facts contained in the working memory.</returns>
		/// <remarks>Do not alter the facts from this enumemration: use retract and modify instead.</remarks>
		IEnumerator<Fact> Facts { get; }
		
		/// <summary>
		/// Returns true if a Fact exists in the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to check existence.</param>
		/// <returns>True if the Fact exists.</returns>
		bool FactExists(Fact fact);
		
		/// <summary>
		/// Returns true if a Fact exists in the current working memory.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to check existence.</param>
		/// <returns>True if the Fact exists.</returns>
		bool FactExists(string factLabel);
		
		/// <summary>
		/// Returns a Fact from its label if it exists, else returns null.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to retrieve.</param>
		/// <returns>The Fact that matches the label if it exists, otherwise null.</returns>
		Fact GetFact(string factLabel);
		
		/// <summary>
		/// Asserts (adds) a Fact in the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to assert.</param>
		/// <returns>True if the Fact was added to the Fact Base, i.e. if it was really new!</returns>
		bool Assert(Fact fact);
				
		/// <summary>
		/// Retracts (removes) a Fact from the current working memory.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to retract.</param>
		/// <returns>True if the Fact has been retracted from the FactBase, otherwise False.</returns>
		bool Retract(string factLabel);
		
		/// <summary>
		/// Retracts (removes) a Fact from the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to retract.</param>
		/// <returns>True if the Fact has been retracted from the FactBase, otherwise False.</returns>
		bool Retract(Fact fact);
		
		/// <summary>
		/// Modify a Fact by Retracting it and Asserting the replacement one.
		/// If the new Fact has no label (null or Empty), then the Label of the existing fact is kept.
		/// </summary>
		/// <param name="currentFact">The Fact to modify.</param>
		/// <param name="newFact">The Fact to modify to.</param>
		/// <returns>True if <term>currentFact</term> has been retracted from the FactBase, otherwise False ; this whether <term>newFact</term> already exists in the factbase, or not.</returns>
		bool Modify(Fact currentFact, Fact newFact);
		
		/// <summary>
		/// Modify a Fact by Retracting it and Asserting the replacement one.
		/// If the new Fact has no label (null or Empty), then the Label of the existing fact is kept.
		/// </summary>
		/// <param name="currentFactLabel">The label of the Fact to modify.</param>
		/// <param name="newFact">The Fact to modify to.</param>
		/// <returns>True if <term>currentFact</term> has been retracted from the FactBase, otherwise False ; this whether <term>newFact</term> already exists in the factbase, or not.</returns>
		bool Modify(string currentFactLabel, Fact newFact);

		/// <summary>
		/// Gets the number of queries in the current rulebase.
		/// </summary>
		/// <returns>The number of queries in the current rulebase.</returns>
		int QueriesCount { get; }
		
		/// <summary>
		/// Gets an enumeration of the queries in the current rulebase.
		/// </summary>
		/// <returns>An IEnumerator on the queries in the current rulebase.</returns>
		/// <remarks>Do not try to alter the queries from this enumemration: unexpected results might occur if you do so.</remarks>
		IEnumerator<Query> Queries { get; }
		
		/// <summary>
		/// Gets the labels of the queries in the current rulebase.
		/// </summary>
		/// <returns>An <code>IList&lt;string></code> containing the labels of all the queries in the current rulebase.</returns>
		IList<string> QueryLabels { get; }
		
		/// <summary>
		/// Runs a new Query in the current working memory.
		/// </summary>
		/// <remarks>
		/// For performance reasons, it is recommended to declare all queries in the rule base
		/// and to use RunQuery(queryLabel)
		/// </remarks>
		/// <param name="query">The new Query to run.</param>
		/// <returns>An <code>IList&lt;IList&lt;Fact>></code> containing the results found.</returns>
		IList<IList<Fact>> RunQuery(Query query);
		
		/// <summary>
		/// Runs a Query in the current working memory.
		/// </summary>
		/// <param name="queryIndex">The query base index of the Query to run.</param>
		/// <returns>An <code>IList&lt;IList&lt;Fact>></code> containing the results found.</returns>
		/// <remarks>It is recommanded to use labelled queries.</remarks>
		IList<IList<Fact>> RunQuery(int queryIndex);
		
		/// <summary>
		/// Runs a Query in the current working memory.
		/// </summary>
		/// <param name="queryLabel">The label of the Query to run.</param>
		/// <returns>An <code>IList&lt;IList&lt;Fact>></code> containing the results found.</returns>
		IList<IList<Fact>> RunQuery(string queryLabel);
		
		/// <summary>
		/// Gets the number of implications in the current rulebase.
		/// </summary>
		/// <returns>The number of implications in the current rulebase.</returns>
		int ImplicationsCount { get; }
		
		/// <summary>
		/// Gets an enumeration of the implications in the current rulebase.
		/// </summary>
		/// <returns>An IEnumerator on the implications in the current rulebase.</returns>
		/// <remarks>Do not try to alter the implications from this enumemration: unexpected results might occur if you do so.</remarks>
		IEnumerator<Implication> Implications { get; }
	}
}
