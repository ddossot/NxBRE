namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	/// <summary>
	/// A facade of the Inference Engine of NxBRE for usage from the object binder.
	/// </summary>
	/// <remarks>
	/// The facade provide some helper methods and hides forbidden methods, like Process(bo).
	/// </remarks>
	public sealed class IEFacade {
		private IInferenceEngine ie;
		
		private IInferenceEngine IE {
			get {
				return ie;
			}
		}
		
		/// <summary>
		/// Instantiates a new IEFacade that wraps an instance of the Inference Engine.
		/// </summary>
		/// <param name="ie">The Inference Engine the new facade must encapsulate.</param>
		public IEFacade(IInferenceEngine ie) {
			this.ie = ie;
		}
		
		/// <summary>
		/// The label of the loaded rule base.
		/// </summary>
		public string Label {
			get {
				return IE.Label;
			}
		}

		/// <summary>
		/// Sets the WorkingMemory of the engine, either by forking the existing Global memory
		/// to a new Isolated one, or by simply using the Global one.
		/// </summary>
		/// <param name="memoryType">The new type of working memory.</param>
		public void NewWorkingMemory(WorkingMemoryTypes memoryType) {
			IE.NewWorkingMemory(memoryType);
		}
		
		/// <summary>
		/// Makes the current isolated memory the new global memory and sets the working memory
		/// type to global. Throws an exception in the current memory type is not isolated.
		/// </summary>
		public void CommitIsolatedMemory() {
			IE.CommitIsolatedMemory();
		}
		
		/// <summary>
		/// Dispose the current isolated memory sets the working memory type to global.
		/// Throws an exception in the current memory type is not isolated.
		/// </summary>
		public void DisposeIsolatedMemory() {
			IE.DisposeIsolatedMemory();
		}
		
		/// <summary>
		/// Performs all the possible deductions on the current working memory and stops
		/// infering when no new Fact is deducted.
		/// </summary>
		public void Process() {
			IE.Process(null);
		}
		
		/// <summary>
		/// Gets the number of facts in the current working memory.
		/// </summary>
		public int FactsCount {
			get {
				return IE.FactsCount;
			}
		}
		
		/// <summary>
		/// Gets an enumeration of the facts contained in the working memory.
		/// </summary>
		/// <returns>An IEnumerator on the facts contained in the working memory.</returns>
		/// <remarks>Do not alter the facts from this enumemration: use retract and modify instead.</remarks>
		public IEnumerator Facts {
			get {
				return IE.Facts;
			}
		}
		
		/// <summary>
		/// Asserts (adds) a new Fact in the current working memory.
		/// </summary>
		/// <remarks>
		/// This method is an helper of the regular Assert method that creates the Fact object directly.
		/// </remarks>
		/// <param name="type">The Type of the new Fact to assert.</param>
		/// <param name="individuals">The Individual predicatesthat the Fact will contain.</param>
		/// <returns>True if the Fact was added to the Fact Base, i.e. if it was really new!</returns>
		public bool AssertNewFact(string type, params object[] individuals) {
			return Assert(NewFact(type, individuals));
		}
		
		/// <summary>
		/// Asserts (adds) a new Fact in the current working memory, or throw a BREException if 
		/// the assertion failed, i.e. if the fact was already present.
		/// </summary>
		/// <remarks>
		/// This method is an helper of the regular Assert method that creates the Fact object directly.
		/// </remarks>
		/// <param name="type">The Type of the new Fact to assert.</param>
		/// <param name="individuals">The Individual predicatesthat the Fact will contain.</param>
		public void AssertNewFactOrFail(string type, params object[] individuals) {
			Fact newFact = NewFact(type, individuals);
			
			if (!Assert(newFact))
				throw new BREException("New fact assertion failure: " + newFact.ToString());
		}
		
		/// <summary>
		/// Creates a new Fact.
		/// </summary>
		/// <remarks>
		/// This method is an helper of the regular Fact instantiation.
		/// </remarks>
		/// <param name="type">The Type of the new Fact to assert.</param>
		/// <param name="individuals">The Individual predicatesthat the Fact will contain.</param>
		/// <returns>A new Fact of desired Type and individuals.</returns>
		public Fact NewFact(string type, params object[] individuals) {
			Fact newFact;
			
			if (individuals.Length == 0) {
				newFact = new Fact(type);
			}
			else if (individuals.Length == 1) {
				newFact = new Fact(type, new Individual(individuals[0]));
			}
			else {
				Individual[] members = new Individual[individuals.Length];
				for (int i=0; i<individuals.Length; i++) members[i] = new Individual(individuals[i]);
				newFact = new Fact(type, members);
			}
			
			return newFact;
		}
		
		/// <summary>
		/// Returns true if a Fact exists in the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to check existence.</param>
		/// <returns>True if the Fact exists.</returns>
		public bool FactExists(Fact fact) {
			return IE.FactExists(fact);
		}
		
		/// <summary>
		/// Returns true if a Fact exists in the current working memory.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to check existence.</param>
		/// <returns>True if the Fact exists.</returns>
		public bool FactExists(string factLabel) {
			return IE.FactExists(factLabel);
		}
		
		/// <summary>
		/// Returns a Fact from its label if it exists, else returns null.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to retrieve.</param>
		/// <returns>The Fact that matches the label if it exists, otherwise null.</returns>
		public Fact GetFact(string factLabel) {
			return IE.GetFact(factLabel);
		}
		
		/// <summary>
		/// Asserts (adds) a Fact in the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to assert.</param>
		/// <returns>True if the Fact was added to the Fact Base, i.e. if it was really new!</returns>
		public bool Assert(Fact fact) {
			return IE.Assert(fact);
		}
				
		/// <summary>
		/// Retracts (removes) a Fact from the current working memory.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to retract.</param>
		public void Retract(string factLabel) {
			IE.Retract(factLabel);
		}
		
		/// <summary>
		/// Retracts (removes) a Fact from the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to retract.</param>
		public void Retract(Fact fact) {
			IE.Retract(fact);
		}
		
		/// <summary>
		/// Modify a Fact by Retracting it and Asserting the replacement one.
		/// If the new Fact has no label (null or Empty), then the Label of the existing fact is kept.
		/// </summary>
		/// <param name="currentFact">The Fact to modify.</param>
		/// <param name="newFact">The Fact to modify to.</param>
		/// <returns>True if <term>currentFact</term> has been retracted from the FactBase, otherwise False ; this whether <term>newFact</term> already exists in the factbase, or not.</returns>
		public bool Modify(Fact currentFact, Fact newFact) {
			return IE.Modify(currentFact, newFact);
		}
		
		/// <summary>
		/// Modify a Fact by Retracting it and Asserting the replacement one.
		/// If the new Fact has no label (null or Empty), then the Label of the existing fact is kept.
		/// </summary>
		/// <param name="currentFactLabel">The label of the Fact to modify.</param>
		/// <param name="newFact">The Fact to modify to.</param>
		/// <returns>True if <term>currentFact</term> has been retracted from the FactBase, otherwise False ; this whether <term>newFact</term> already exists in the factbase, or not.</returns>
		public bool Modify(string currentFactLabel, Fact newFact) {
			return IE.Modify(currentFactLabel, newFact);
		}
		
		/// <summary>
		/// Runs a new Query in the current working memory.
		/// </summary>
		/// <remarks>
		/// For performance reasons, it is recommended to declare all queries in the rule base
		/// and to use RunQuery(queryLabel)
		/// </remarks>
		/// <param name="query">The new Query to run.</param>
		/// <returns>An <code>IList&lt;IList&lt;Fact>></code> containing the results found.</returns>
		public IList<IList<Fact>> RunQuery(Query query) {
			return IE.RunQuery(query);
		}
		
		/// <summary>
		/// Runs a Query in the current working memory.
		/// </summary>
		/// <param name="queryIndex">The query base index of the Query to run.</param>
		/// <returns>An <code>IList&lt;IList&lt;Fact>></code> containing the results found.</returns>
		/// <remarks>It is recommanded to use labelled queries.</remarks>
		public IList<IList<Fact>> RunQuery(int queryIndex) {
			return IE.RunQuery(queryIndex);
		}
		
		/// <summary>
		/// Runs a Query in the current working memory.
		/// </summary>
		/// <param name="queryLabel">The label of the Query to run.</param>
		/// <returns>An <code>IList&lt;IList&lt;Fact>></code> containing the results found.</returns>
		public IList<IList<Fact>> RunQuery(string queryLabel) {
			return IE.RunQuery(queryLabel);
		}
		
		///<summary>Method for logging messages</summary>
		/// <param name="message">The message to log</param>
		/// <param name="traceLevel">Trace level of the message</param>
		/// <see cref="System.Diagnostics.TraceLevel"/>
		/// <remarks></remarks>
		[Obsolete("Binders should better use standard Trace methods, this method has been kept for compatibility purpose only")]
		public void DispatchLog(string message, int traceLevel) {
			Logger.InferenceEngineSource.TraceEvent(Logger.ConvertFromObsoleteIntLevel(traceLevel), 0, message);
		}
		
	}
}
