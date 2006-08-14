namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data;
	using System.Diagnostics;
	using System.Text;
	
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	/// <summary>
	/// The FactBase is the repository of facts for the inference engine.
	/// This implementation does not allow duplicated facts.
	/// </summary>
	/// <remarks>This class is not thread safe.</remarks>
	/// <author>David Dossot</author>
	internal sealed class FactBase:ICloneable, IEnumerable<Fact> {
		private static readonly NegativeFact NAF = new NegativeFact();
		
		/// <summary>
		/// Defines whether the fact storage should consider typed objects as equivalent to their String representation.
		/// If StrictTyping is set to true, they are will not be considered equivalent. The default is false.
		/// </summary>
		/// <remarks>
		/// It is internal to allow changing it for unit testing purposes.
		/// </remarks>
		internal bool strictTyping = Parameter.Get<bool>("factBase.strictTyping", false);
		
		/// <summary>
		/// The main storage of facts.
		/// </summary>
		private readonly IDictionary<long, HashSet<Fact>> factStore;
		
		/// <summary>
		/// A prepared empty fact collection.
		/// </summary>
		private static readonly ICollection<Fact> EMPTY_FACT_COLLECTION = new List<Fact>().AsReadOnly();
		
		/// <summary>
		/// A flag that external class can use to detect fact assertions/retractions.
		/// </summary>
		private bool modifiedFlag;
		
		/// <summary>
		/// The total number of facts in the fact base.
		/// </summary>
		public int Count {
			get {
				return GetFactsForIndex(GetGlobalIndex()).Count;
			}
		}
		
		/// <summary>
		/// A flag that external class can use to detect new fact assertions. The fact base sets
		/// this fact to True if it is false and a new fact has been asserted and accepted.
		/// </summary>
		public bool ModifiedFlag {
			get {
				return modifiedFlag;
			}
			set {
				modifiedFlag = value;
			}
		}
		
		/// <summary>
		/// A technical subclass of Fact for representing a positive negative-fact!
		/// </summary>
		private class NegativeFact:Fact {
			public NegativeFact():base("###Dummy NAF Result###", new IPredicate[0]{}){}
		}
		
		/// <summary>
		/// A class representing one selected match between an atom and a fact
		/// </summary>
		public class PositiveMatchResult {
			private readonly Atom source;
			private readonly Fact fact;
			
			public PositiveMatchResult(Atom source, Fact fact) {
				this.source = source;
				this.fact = fact;
			}
			
			public Atom Source {
				get {
					return source;
				}
			}
			
			public Fact Fact {
				get {
					return fact;
				}
			}
			
			public override string ToString() {
				return "~" + source + "->" + fact;
			}
		}

		
		/// <summary>
		/// Instantiates a new fact base.
		/// </summary>
		public FactBase():this(null) {}
		
		/// <summary>
		/// Instantiates a new fact base, using an existing factStore as a source.
		/// </summary>
		/// <param name="factStore">The factStore to use as a source or null.</param>
		/// <remarks>
		/// Usually used for cloning.
		/// </remarks>
		private FactBase(IDictionary<long, HashSet<Fact>> initialFactStore) {
			factStore = new Dictionary<long, HashSet<Fact>>();
			
			if (initialFactStore != null) {
				for(IEnumerator<KeyValuePair<long, HashSet<Fact>>> e = initialFactStore.GetEnumerator(); e.MoveNext() ;) {
					KeyValuePair<long, HashSet<Fact>> kvp = e.Current;
					factStore.Add(kvp.Key, new HashSet<Fact>(kvp.Value));
				}
			}
		}
		
		///<summary>Clones the fact base.</summary>
		public object Clone() {
			return new FactBase(this.factStore);
		}
		
		/// <summary>
		/// Gets the enumeration of all facts in the fact base.
		/// </summary>
		/// <returns>An IEnumerator of all facts.</returns>
		public IEnumerator<Fact> GetEnumerator() {
			return GetFactsForIndex(GetGlobalIndex()).GetEnumerator();
		}

		/// <summary>
		/// Gets the enumeration of all facts in the fact base.
		/// </summary>
		/// <returns>An IEnumerator of all facts.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetFactsForIndex(GetGlobalIndex()).GetEnumerator();
		}
		
		///<remarks>As Facts labels are basically ignored (no retrieval nor any operation based
		/// on it, the FactBase does not bother check if we have different facts with same labels.</remarks>
		public bool Assert(Fact fact) {
			if (fact == null) throw new ArgumentNullException("Null is not a valid fact to assert");
			
			if (!fact.IsFact)
				throw new BREException("Can not add non-facts to the fact base: "+fact.ToString());
			
			if (!Exists(fact)) {
				AddFactToStore(fact);
				
				// the fact was new and added to the factbase, return true
				if (!ModifiedFlag) ModifiedFlag = true;
				
				return true;
			}
			else
				// else return false
				return false;
		}
		
		/// <summary>
		/// Retracts (removes) a Fact from the FactBase.
		/// </summary>
		/// <param name="fact">The Fact to remove.</param>
		/// <returns>True if the Fact has been retracted from the FactBase, otherwise False.</returns>
		public bool Retract(Fact fact) {
			if (fact == null) throw new ArgumentNullException("Null is not a valid fact to retract");
			
			for(IEnumerator<Fact> e = Select(fact, null); e.MoveNext() ; ) {
				RemoveFactFromStore(e.Current);
				
				// the fact existing and removed from the factbase, return true
				if (!ModifiedFlag) ModifiedFlag = true;

				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// Modify a Fact by Retracting it and Asserting the replacement one.
		/// If the new Fact has no label (null or Empty), then the Label of the existing fact is kept.
		/// </summary>
		/// <param name="currentFact">The Fact to modify.</param>
		/// <param name="newFact">The Fact to modify to.</param>
		/// <returns>True if <term>currentFact</term> has been retracted from the FactBase, otherwise False ; this whether <term>newFact</term> already exists in the factbase, or not.</returns>
		public bool Modify(Fact currentFact, Fact newFact) {
			if (currentFact == null) throw new ArgumentNullException("Null is not a valid fact to modify");
			if (newFact == null) throw new ArgumentNullException("Null is not a valid fact to use as a new value");
			
			if (Retract(currentFact)) {
				// Retraction is done, let's Assert
				try {
					// If the new fact already exists, retract it first
					if (Exists(newFact)) Retract(newFact);
					
					// Preserve the label if no new one provided
					if ((currentFact.Label != null) && (currentFact.Label != String.Empty)
					    && ((newFact.Label == null) || (newFact.Label == String.Empty)))
						Assert(newFact.ChangeLabel(currentFact.Label));
					else
						Assert(newFact);
					return true;
				} catch(Exception e) {
					// Assert mysteriously failed: compensate the retraction
					Assert(currentFact);
					// and throw a wrapped exception
					throw new BREException("Modify failed because Assert failed.", e);
				}
			}
			
			return false;
		}
		
		/// <summary>
		/// Checks if the FactBase contains a certain Fact.
		/// </summary>
		/// <param name="fact">The Fact to check.</param>
		/// <returns>True if the Fact is already present in the FactBase, otherwise False.</returns>
		public bool Exists(Fact fact) {
			return Select(fact, null).MoveNext();
		}
		
		/// <summary>
		/// Gets a certain Fact out of the FactBase from its Label.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to get.</param>
		/// <returns>The Fact matching the label if present in the FactBase, otherwise null.</returns>
		/// <remarks>A label is not discriminent for NxBRE so the engine returns the first fact that matches the searched label.</remarks>
		public Fact GetFact(string factLabel) {
			if (factLabel == null) throw new ArgumentNullException("Null is not a valid fact label to search for.");
			ICollection<Fact> foundFacts = GetFactsForIndex(GetLabelBasedIndex(factLabel));
			IEnumerator<Fact> e = foundFacts.GetEnumerator();
			if (e.MoveNext()) return e.Current;
			else return null;
		}
		
		/// <summary>
		/// A String representation of the FactBase for display purpose only.
		/// </summary>
		/// <returns>The String representation of the FactBase.</returns>
		public override string ToString() {
			return "FactBase[Count:" + Count + "]";
		}
		
		/// <summary>
		/// Runs a Query against a the FactBase.
		/// </summary>
		/// <param name="query">The Query to run.</param>
		/// <returns>An ICollection<IList<Fact>> containing references to facts matching the pattern of the Query.</returns>
		public IList<IList<Fact>> RunQuery(Query query) {
			return FilterDistinct(ProcessAtomGroup(query.AtomGroup));
		}

		/// <summary>
		/// Extract all the facts found in process results.
		/// </summary>
		/// <param name="processResults">The process results from where facts must be extracted.</param>
		/// <returns>The array of extracted facts.</returns>
		public static IList<Fact> ExtractFacts(IList<IList<PositiveMatchResult>> processResults) {
			IList<Fact> result = new List<Fact>();
			
			foreach(IList<PositiveMatchResult> processResult in processResults)
				foreach(PositiveMatchResult pmr in processResult)
					// naf atom dummy results are skipped
					if (!(pmr.Fact is FactBase.NegativeFact)) 
						result.Add(pmr.Fact);
			
			return result;
		}

		/// <summary>
		/// Runs an AtomGroup against the FactBase.
		/// </summary>
		/// <remarks>
		/// Each Atom in the group and sub-groups must have been registered.
		/// </remarks>
		/// <param name="AG">The AtomGroup to execute</param>
		/// <returns>An IList<IList<PositiveMatchResult>> object containing the results.</returns>
		public IList<IList<PositiveMatchResult>> ProcessAtomGroup(AtomGroup AG) {
			List<IList<PositiveMatchResult>> runResult = new List<IList<PositiveMatchResult>>();
			
			if (AG.Operator == AtomGroup.LogicalOperator.And)
				ProcessAnd(AG, runResult, 0, new List<PositiveMatchResult>());
			else if (AG.Operator == AtomGroup.LogicalOperator.Or)
				ProcessOr(AG, runResult, new List<PositiveMatchResult>());
			else
				throw new BREException("Processor encountered unsupported logical operator: " + AG.Operator);
			
			return runResult.AsReadOnly();
		}

		public static Atom Populate(Atom targetAtom, IList<PositiveMatchResult> resultStack, bool evaluateFormulas) {
	  	IPredicate[] members = (IPredicate[])targetAtom.Members.Clone();
	  	
	  	// populate the variable elements with predicate values coming
	  	// from the query part of the implication
	  	foreach(PositiveMatchResult pmr in resultStack)
	  		if (!(pmr.Fact is FactBase.NegativeFact))
	  			RulesUtil.Populate(pmr.Fact, pmr.Source, members);

	  	// if there are formulas in the atom, resolve these expressions, passing
	  	// the variable values as arguments
	  	if ((evaluateFormulas) && (targetAtom.HasFormula)) {
	  			// formulas must be evaluated and the results placed in individual predicates
		  		IDictionary arguments = new Hashtable();
		  		
		  		foreach(PositiveMatchResult pmr in resultStack) {
		  			if (!(pmr.Fact is FactBase.NegativeFact)) {
			  			for(int i=0; i<pmr.Source.Members.Length; i++) {
			  				object sourcePredicateKey = null;
		  					
		  					if (pmr.Source.Members[i] is Variable) {
			  					sourcePredicateKey = pmr.Source.Members[i].Value;
	    					}
			  				else if (pmr.Source.SlotNames[i] != String.Empty) {
		  						sourcePredicateKey = pmr.Source.SlotNames[i];
		  					}
			  				
			  				if ((sourcePredicateKey != null) && (!arguments.Contains(sourcePredicateKey)))
			  					arguments.Add(sourcePredicateKey, pmr.Fact.Members[i].Value);
			  				
		  				}
	  				}
		  		}
		  	
		  		for(int i=0; i<members.Length; i++) {
		  			if (members[i] is Formula) {
     					try {
					      members[i] = new Individual(((Formula)members[i]).Evaluate(arguments));
     					}
     					catch (Exception ex) {
		  					// Chuck Cross added try/catch block with addtional info in new thrown exception
		  					StringBuilder sb = new StringBuilder("Error evaluating formula ")
		  																				.Append(members[i])
		  																				.Append(" in atom: ")
		  																				.Append(targetAtom.Type)
		  																				.Append(".\r\n  Arguments:");
      					
		  					foreach(DictionaryEntry entry in arguments) {
      						sb.Append("   ")
      							.Append(entry.Key.ToString());
      						
      						if (entry.Value != null) {
      							sb.Append("[")
      								.Append(entry.Value.GetType().ToString())
      								.Append("] = ")
      								.Append(entry.Value.ToString());
      						}
      						else sb.Append("[Null]");
      						
      						sb.Append("\r\n");
		  					}
      					
      					throw new BREException(sb.ToString(), ex);
							}
		  			}
		  		}
	  	}
	  	
	  	// clone the target with new members, because atom is immutable
	  	return targetAtom.CloneWithNewMembers(members);
		}

		/// <summary>
		/// Add a result (and its producing atom) in a result stack. 
		/// </summary>
		/// <param name="resultStack">An ArrayList containing the results to enrich.</param>
		/// <param name="source">The atom that has produced the fact to add</param>
		/// <param name="result">The fact to add to the result stack.</param>
		/// <returns>An arraylist containing the original result stack enriched by the new fact.</returns>
		/// <remarks>The new fact is added at the top of the result stack, to give it maximum priority in further processing.</remarks>
		public static IList<PositiveMatchResult> EnrichResults(IList<PositiveMatchResult> resultStack, Atom source, Fact result) {
			List<PositiveMatchResult> enrichedProcessResult = new List<PositiveMatchResult>();
			enrichedProcessResult.Add(new PositiveMatchResult(source, result));
			enrichedProcessResult.AddRange(resultStack);
			return enrichedProcessResult;
		}
		
		/// <summary>
		/// Build an atom for querying facts on the basis of an implication deduction, whose individual
		/// predicates will be replaced by variables and whose variables will be resolved from values
		/// coming from an implication result stack.
		/// </summary>
		/// <param name="targetAtom">The implication deduction atom.</param>
		/// <param name="resultStack">The implication result stack.</param>
		/// <returns>The atom ready for querying facts.</returns>
		public static Atom BuildQueryFromDeduction(Atom targetAtom, IList<PositiveMatchResult> resultStack) {
	  	IPredicate[] members = (IPredicate[])targetAtom.Members.Clone();
	  	
	  	// populate the variable elements with predicate values coming
	  	// from the query part of the implication
	  	foreach(PositiveMatchResult pmr in resultStack) RulesUtil.Populate(pmr.Fact, pmr.Source, members);
	  	
	  	if ((targetAtom.HasFormula) || (targetAtom.HasIndividual)) {
  			// formulas and individuals must be replaced by variables
  			// named after the position of the predicate
  			for(int i=0; i<members.Length; i++) {
	  			if (members[i] is Formula) members[i] = new Variable(i);
	  			// Individuals present in the deduction are static values (compared to values coming from
	  			// variables) and must be transformed into variables in order to perform a selection of
	  			// potentially matching facts.
	  			// To the contrary, Variables, resolved with values coming from the body part of the implication
	  			// must be used to search matching facts.
	  			else if (targetAtom.Members[i] is Individual) members[i] = new Variable(i.ToString());
  			}
	  	}
	  	
	  	// clone the target with new members, because atom is immutable
	  	return targetAtom.CloneWithNewMembers(members);
		}
		
		//----------------------------- INTERNAL & PRIVATE MEMBERS ------------------------------
		
		/// <summary>
		/// The index under which all the facts will be stored.
		/// </summary>
		private static long GetGlobalIndex() {
			return 0;
		}
		
		/// <summary>
		/// The index under which all the facts with the same label will be stored.
		/// </summary>
		private static long GetLabelBasedIndex(string label) {
			return (long)label.GetHashCode();
		}
		
		/// <summary>
		/// The index under which all the facts with the same signature will be stored.
		/// </summary>
		private static long GetSignatureBasedIndex(string signature) {
			return (long)signature.GetHashCode() << 32;
		}
		
		/// <summary>
		/// The index under which all the facts with the same signature, predicate position and predicate will be stored.
		/// </summary>
		private static long GetSignatureAndPredicateBasedIndex(string signature, int predicatePosition, IPredicate predicate) {
			return GetSignatureBasedIndex(signature) ^ (long)predicate.GetHashCode() ^ Math.BigMul(37, predicatePosition);
		}
		
		/// <summary>
		/// Adds a fact to the store at a particular index position.
		/// </summary>
		/// <remarks>
		/// It assumes that the fact is not already present in the store: in case the fact is alreay there, runtime error
		/// will occur.
		/// </remarks>
		private void AddFactToStore(long index, Fact fact) {
			if (Logger.IsInferenceEngineVerbose)
				Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0 , "FactBase.AddFactToStore @ " + index + " : " + fact);
			
			HashSet<Fact> factCollection;
			
			if (!factStore.TryGetValue(index, out factCollection)) {
				factCollection = new HashSet<Fact>();
				factStore.Add(index, factCollection);
			}
			
			factCollection.Add(fact);
		}
		
		/// <summary>
		/// Adds a fact to the store in all the relevant index positions.
		/// </summary>
		/// <remarks>
		/// It assumes that the fact is not already present in the store: in case the fact is alreay there, runtime error
		/// will occur.
		/// </remarks>
		private void AddFactToStore(Fact fact) {
			// store in global storage
			AddFactToStore(GetGlobalIndex(), fact);
			
			// store under fact signature
			AddFactToStore(GetSignatureBasedIndex(fact.Signature), fact);
			
			// store under each predicate
			for(int predicatePosition=0; predicatePosition < fact.Members.Length; predicatePosition++) {
				AddFactToStore(GetSignatureAndPredicateBasedIndex(fact.Signature, predicatePosition, fact.Members[predicatePosition]), fact);
				
				// if we do not want strict typing, we also store the non-string individuals under their string representation
				if (!strictTyping) {
					object predicateValue = fact.Members[predicatePosition].Value;
					if (!(predicateValue is string)){
						AddFactToStore(GetSignatureAndPredicateBasedIndex(fact.Signature, predicatePosition, new Individual(predicateValue.ToString())), fact);
					}
				}
			}
			
			// store under label, if available
			if (fact.Label != null) AddFactToStore(GetLabelBasedIndex(fact.Label), fact);
		}
		
		/// <summary>
		/// Removes a fact from the store.
		/// </summary>
		/// <remarks>
		/// It assumes that the fact is actually present in the store: in case the fact is not there, runtime error
		/// will occur.
		/// </remarks>
		private void RemoveFactFromStore(Fact fact) {
			// remove the fact from the lists of reference where it is referenced
			foreach(ICollection<Fact> factList in factStore.Values)	factList.Remove(fact);
		}
		
		/// <summary>
		/// Gets a collection of facts for a particular index.
		/// </summary>
		/// <returns>A collection of facts, empty if no collection exists at the index in the fact store.</returns>
		private ICollection<Fact> GetFactsForIndex(long index) {
			HashSet<Fact> result;
			if(factStore.TryGetValue(index, out result)) return result;
			else return EMPTY_FACT_COLLECTION;
		}

		/// <summary>
		/// Gets a list of facts matching a particular atom.
		/// </summary>
		/// <param name="atom">The atom to match</param>
		/// <param name="excludedFacts">A list of facts not to return, or null</param>
		/// <returns>An IList containing the matching facts (empty if no match, but never null).</returns>
		public IEnumerator<Fact> Select(Atom filter, IList<Fact> excludedFacts) {
			if (Logger.IsInferenceEngineVerbose)
				Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
				                                        0,
				                                        "FactBase.Select: " + filter + " - Excluding: " + Misc.IListToString((IList)excludedFacts));

			// if the predicate map does not contain an entry for the filter signature or if this entry is empty, return empty result
			ICollection<Fact> factsMatchingFilterSignature = GetFactsForIndex(GetSignatureBasedIndex(filter.Signature));
			if (factsMatchingFilterSignature.Count == 0) {
				if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "No fact matching signature: " + filter.Signature);
				return EMPTY_FACT_COLLECTION.GetEnumerator();
			}
			
			// if the filter contains only variable predicates everything matching the signature should be returned
			if (filter.OnlyVariables) {
				if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "Filter with no Ind or Fun -> Return all facts matching signature: " + filter.Signature);
				return FactEnumeratorFactory.NewFactListExcludingEnumerator(factsMatchingFilterSignature, excludedFacts);
			}
			
			// we build result lists and will reduce the biggest ones from the smallest ones
			ICollection<Fact> resultList = null;
			int smallestList = Int32.MaxValue;
			int positionOfSmallestList = -1;
			
			for (int position=0; position < filter.Members.Length; position++) {
				if (filter.Members[position] is Individual) {
					ICollection<Fact> factsMatchingPositionedIndividual = GetFactsForIndex(GetSignatureAndPredicateBasedIndex(filter.Signature, position, filter.Members[position]));
					int countOfFactsMatchingPositionedIndividual = factsMatchingPositionedIndividual.Count;
					
					if (countOfFactsMatchingPositionedIndividual > 0) {
							Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
							                                        0,
							                                        "Matched predicateValue: "+ filter.Members[position].Value + " [" + filter.Members[position].Value.GetType() + "]");
	
						if (countOfFactsMatchingPositionedIndividual < smallestList) {
							resultList = factsMatchingPositionedIndividual;
							smallestList = countOfFactsMatchingPositionedIndividual;
							positionOfSmallestList = position;
							
							if (Logger.IsInferenceEngineVerbose)
								Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
								                                        0,
								                                        "Smallest list of size: "+ smallestList + " at position: " + positionOfSmallestList);
						}
								
					}
					else  {
						// no match found on a particular individual predicate? early return an empty result!
						if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "No match -> Return no fact");

						return EMPTY_FACT_COLLECTION.GetEnumerator();
					}
				}
			}

			// only one predicate in the filter and we matched it, no need for post matching, return filtered results directly
			if ((filter.Members.Length == 1) && (resultList != null)) {
				if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "One member filter and got resultList -> Return facts immediatly");
				
				return FactEnumeratorFactory.NewFactListExcludingEnumerator(resultList, excludedFacts);
			}

			// we have not been able to match anything (the filter might contain only variables or functions),
			// let's load all the facts matching the signature of the filter
			if (resultList == null) {
				resultList = factsMatchingFilterSignature;
				
				if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "No resultList -> Used the list matching the signature, which contains: " + resultList.Count);
			}
			
			// we append a fact as a result only if it matches the filter predicates
			// if a predicate has been previously matched, we do not compare it again in the matching process: we add its position
			// to the ignore list
			IList<int> ignoredPredicates = null;
			if (positionOfSmallestList != -1) {
				ignoredPredicates = new List<int>();
				ignoredPredicates.Add(positionOfSmallestList);
			}
			
			return FactEnumeratorFactory.NewFactListPredicateMatchingEnumerator(resultList,
			                                                                    filter,
			                                                                    strictTyping,
			                                                                    ignoredPredicates,
			                                                                    excludedFacts);
		}
		
		private static IList<IList<Fact>> FilterDistinct(IList<IList<PositiveMatchResult>> processResults) {
			IDictionary<long, IList<Fact>> resultSet = new Dictionary<long, IList<Fact>>();
			
			foreach(IList<PositiveMatchResult> processResult in processResults) {
				IList<Fact> row = new List<Fact>();
				long rowLongHashCode = 0;
	  		
				foreach(PositiveMatchResult pmr in processResult) {
					// naf atom dummy results are skipped
					if (!(pmr.Fact is FactBase.NegativeFact)) {
						row.Add(pmr.Fact);
						rowLongHashCode ^= pmr.Fact.GetHashCode();
					}
					//TODO: with more than 16 entries in processResult, we will start losing bits: a rotation would be better
					rowLongHashCode <<= 1;
				}
				
				// add only new rows to perform a "select distinct"
				if (!resultSet.ContainsKey(rowLongHashCode)) resultSet.Add(rowLongHashCode, new ReadOnlyCollection<Fact>(row));
			}
			
			return new List<IList<Fact>>(resultSet.Values).AsReadOnly();
		}
			
		private void ProcessAnd(AtomGroup AG, IList<IList<PositiveMatchResult>> processResult, int depth, IList<PositiveMatchResult> resultStack) {
			if (AG.OrderedMembers[depth] is AtomGroup) {
				if (((AtomGroup)AG.OrderedMembers[depth]).Operator == AtomGroup.LogicalOperator.And)
					throw new BREException("Nested And unexpectedly found in atom group:" + AG.OrderedMembers[depth]);
				
				IList<IList<PositiveMatchResult>> subProcessResult = new List<IList<PositiveMatchResult>>();
			  
				ProcessOr((AtomGroup)AG.OrderedMembers[depth], subProcessResult, resultStack);
			  
				foreach(IList<PositiveMatchResult> resultRow in subProcessResult) {
					foreach(PositiveMatchResult rpRow in resultRow) {
				  	if (resultStack.Count == 0) {
							IList<PositiveMatchResult> tempResultStack = new List<PositiveMatchResult>(resultStack);
							tempResultStack.Add(rpRow);
							
							if (depth < (AG.OrderedMembers.Length-1)) ProcessAnd(AG, processResult, depth+1, tempResultStack);
					  	else processResult.Add(tempResultStack);
				  	}
				  	else {
					  	// exclude similar results for similar atoms (the engine must produce combinations
					  	// of facts in this case)
					  	bool ignore = false;
					  	
					  	foreach(PositiveMatchResult pmr in resultStack) {
					  		if (rpRow.Source.IsIntersecting(pmr.Source)) {
					  			ignore = true;
					  			break;
					  		}
					  		if (!ignore) {
							  	IList<PositiveMatchResult> tempResultStack = new List<PositiveMatchResult>(resultStack);
									tempResultStack.Add(rpRow);
									
									if (depth < (AG.OrderedMembers.Length-1)) ProcessAnd(AG, processResult, depth+1, tempResultStack);
							  	else processResult.Add(tempResultStack);
								}
					  	}
					  }
			  	}
				}
			}
			else {
		  	// resolve the functions and var parts of the atom 
		  	// from all the previous facts in the result stack
				Atom atomToRun = Populate((Atom)AG.OrderedMembers[depth], resultStack, false);
				IList<Fact> excludedFacts = new List<Fact>();
				
				foreach(PositiveMatchResult pmr in resultStack)
					if (((Atom)AG.OrderedMembers[depth]).IsIntersecting(pmr.Source))
						excludedFacts.Add(pmr.Fact);
		  	
		  	// then get the matching facts
		  	IEnumerator results = ProcessAtom(atomToRun, excludedFacts);
			
				if (results != null) {
		  		while(results.MoveNext()) {
			  		Fact result = (Fact)results.Current;
			  		IList<PositiveMatchResult> tempResultStack = new List<PositiveMatchResult>(resultStack);
						tempResultStack.Add(new PositiveMatchResult((Atom)AG.OrderedMembers[depth], result));

						if (depth < (AG.OrderedMembers.Length-1))
							ProcessAnd(AG, processResult, depth+1, tempResultStack);
				  	else
							processResult.Add(tempResultStack);
				  }
				}
			}
		}
		
		private void ProcessOr(AtomGroup AG, IList<IList<PositiveMatchResult>> processResult, IList<PositiveMatchResult> resultStack) {
			foreach(object member in AG.OrderedMembers) {
				if (member is AtomGroup) {
					if (((AtomGroup)member).Operator == AtomGroup.LogicalOperator.Or)
						throw new BREException("Nested Or unexpectedly found in atom group:" + member);

					IList<IList<PositiveMatchResult>> subProcessResult = new List<IList<PositiveMatchResult>>();
					ProcessAnd((AtomGroup)member, subProcessResult, 0, resultStack);
					
					foreach(IList<PositiveMatchResult> resultRow in subProcessResult) processResult.Add(resultRow);
				}
				else if (member is Atom) {
			  	// resolve the functions and var parts of the atom 
			  	// from all the previous facts in the result stack
					Atom atomToRun = Populate((Atom)member, resultStack, false);

					// return results found for the first atom that produces data
					IEnumerator results = ProcessAtom(atomToRun, null);
					
					if (results != null) {
						while(results.MoveNext()) {
				  		Fact result = (Fact)results.Current;
					  	IList<PositiveMatchResult> tempResultStack = new List<PositiveMatchResult>();
						  tempResultStack.Add(new PositiveMatchResult((Atom)member, result));
							processResult.Add(tempResultStack);
					  }
					}
				}
				
			}
			
		} //ProcessOr

		private IEnumerator<Fact> ProcessAtom(Atom atomToRun, IList<Fact> excludedFacts) {
			if (atomToRun is AtomFunction) {
				// an atom function is either positive or negative, it does not return any fact
				// if this atom function is wrapped in naf, then the base result is negated,
				// leading to a positive result if the underlying function is negative!
				if (((AtomFunction)atomToRun).PositiveRelation != atomToRun.Negative) return FactEnumeratorFactory.NewSingleFactEnumerator(NAF);
			}
			else {
				if (atomToRun.Negative) {
					// a negative atom, fails if any matching fact is found
					// and succeed if no data collection (ie returns one dummy fact as a token)
					// if no facts are actually found
					// Bug #1332214 pinpointed that excludedFacts should not be applied on Negative atoms
					IEnumerator matchResult = Select(atomToRun, null);
					if ((matchResult == null) || (!matchResult.MoveNext())) return FactEnumeratorFactory.NewSingleFactEnumerator(NAF);
				}
				else {
					return Select(atomToRun, excludedFacts);
				}
			}
			
			return null;
		}
		
	}
}
