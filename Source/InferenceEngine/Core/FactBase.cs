namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// The FactBase is the repository of facts for the inference engine.
	/// This implementation does not allow duplicated facts.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class FactBase:ICloneable, IEnumerable {
		private static readonly NegativeFact NAF = new NegativeFact();
		
		/// <summary>
		/// The central repository of facts
		/// </summary>
		private Hashtable factList;

		/// <summary>
		/// The repository of matched resolved facts for search at inference time.
		/// </summary>
		private Hashtable matchedFactStorageTable;
		
		/// <summary>
		/// The list of all atoms found in implications and queries - including transient queries
		/// run against the fact base.
		/// </summary>
		private Hashtable atomList;
		
		/// <summary>
		/// A flag that external class can use to detect fact assertions/retractions.
		/// </summary>
		private bool modifiedFlag;
		
		/// <summary>
		/// The total number of facts in the fact base.
		/// </summary>
		public int Count {
			get {
				return factList.Count;
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
		/// Instantiates a new fact base.
		/// </summary>
		public FactBase() {
			factList = new Hashtable();
			atomList = new Hashtable();
			matchedFactStorageTable = new Hashtable();
		}
		
		/// <summary>
		/// Private constructor used for cloning.
		/// </summary>
		/// <param name="factList"></param>
		/// <param name="atomList"></param>
		/// <param name="matchedFactStorageTable"></param>
		private FactBase(Hashtable factList, Hashtable atomList, Hashtable matchedFactStorageTable) {
			this.factList = factList;
			this.atomList = atomList;
			this.matchedFactStorageTable = matchedFactStorageTable;
		}
		
		/// <summary>
		/// Gets the enumeration of all facts in the fact base.
		/// </summary>
		/// <returns>An IEnumerator of all facts.</returns>
		public IEnumerator GetEnumerator() {
			return factList.Values.GetEnumerator();
		}
		
		///<summary>Clones the fact base.</summary>
		public object Clone() {
			Hashtable newMatchedFactStorageTable = new Hashtable();
			
			foreach(string atomSignature in matchedFactStorageTable.Keys)
				newMatchedFactStorageTable.Add(atomSignature, ((IMatchedFactStorage)matchedFactStorageTable[atomSignature]).Clone());
				
			return new FactBase((Hashtable)factList.Clone(),
			                    (Hashtable)atomList.Clone(),
			                    newMatchedFactStorageTable);
		}
		
		/// <summary>
		/// Register the provided atoms in the atomMap, in order to allow referencing all facts
		/// that match each of the atom.
		/// </summary>
		/// <remarks>
		/// This operation facilitates the scheduling of implications to fire in the Agenda,
		/// by pre-determining all the potentially matching facts.
		/// </remarks>
		/// <param name="atoms">An array of Atoms</param>
		/// <returns>The number of atoms registered.</returns>
		public int RegisterAtoms(params Atom[] atoms) {
			int result = 0;
			
			foreach(Atom atom in atoms) {
				if (!atomList.ContainsKey(atom.GetLongHashCode())) {
					atomList.Add(atom.GetLongHashCode(), atom);
					result++;
					
					foreach(Fact fact in this)
						if (atom.Matches(fact))
							GetMatchingFactStorageTable(atom).Add(fact, atom);
				}
			}
			
			return result;
		}
		
		///<remarks>As Facts labels are basically ignored (no retrieval nor any operation based
		/// on it, the FactBase does not bother check if we have different facts with same labels.</remarks>
		public bool Assert(Fact fact) {
			if (!fact.IsFact)
				throw new BREException("Can not add non-facts to the fact base: "+fact.ToString());
			
			if (!Exists(fact)) {
				factList.Add(fact.GetLongHashCode(), fact);
				
				// check if the new fact matches any of the registered atoms
				// and if yes, then reference this fact in the list of matching ones
				foreach(Atom atom in atomList.Values)
					if (atom.Matches(fact))
						GetMatchingFactStorageTable(atom).Add(fact, atom);
				
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
			if (Exists(fact)) {
				factList.Remove(fact.GetLongHashCode());
				GetMatchingFactStorageTable(fact).Remove(fact);
				
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
			return factList.ContainsKey(fact.GetLongHashCode());
		}
		
		/// <summary>
		/// Gets a certain Fact out of the FactBase from its Label.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to get.</param>
		/// <returns>The Fact matching the label if present in the FactBase, otherwise null.</returns>
		public Fact GetFact(string factLabel) {
			foreach(Fact fact in factList.Values)
				if (fact.Label == factLabel)
					return fact;
			
			return null;
		}
		
		/// <summary>
		/// A String representation of the FactBase for display purpose only.
		/// </summary>
		/// <returns>The String representation of the FactBase.</returns>
		public override string ToString() {
			return "FactBase[factList:" + factList.Count + "]";
		}
		
		/// <summary>
		/// Runs a Query against a the FactBase.
		/// </summary>
		/// <param name="query">The Query to run.</param>
		/// <returns>An ICollection<IList<Fact>> containing references to facts matching the pattern of the Query.</returns>
		public IList<IList<Fact>> RunQuery(Query query) {
			//TODO: performance tests show that the engine is 60% slower: is this because of using generics inside of it, like here?
			// if yes: abandon generics and fall back to QueryResultSet object in IInferenceEngine
			return FilterDistinct(ProcessAtomGroup(query.AtomGroup));
		}

		/// <summary>
		/// Extract all the facts found in process results.
		/// </summary>
		/// <param name="processResults">The process results from where facts must be extracted.</param>
		/// <returns>The array of extracted facts.</returns>
		public static Fact[] ExtractFacts(ProcessResultSet processResults) {
			ArrayList facts = new ArrayList();
			
			foreach(ArrayList rps in processResults)
				foreach(ResultPocket rp in rps)
					// naf atom dummy results are skipped
					if (!(rp.fact is FactBase.NegativeFact)) 
						facts.Add(rp.fact);
			
			return (Fact[])facts.ToArray(typeof(Fact));
		}

		/// <summary>
		/// A read-only collection designed for holding process results.
		/// </summary>
		public class ProcessResultSet:ReadOnlyCollectionBase {
			public ProcessResultSet(ArrayList results) {
				InnerList.AddRange(results);
			}
			public ArrayList this[int index] {
			  get  {
					return((ArrayList)InnerList[index]);
			  }
			}
		}
		
		/// <summary>
		/// Runs an AtomGroup against the FactBase.
		/// </summary>
		/// <remarks>
		/// Each Atom in the group and sub-groups must have been registered.
		/// </remarks>
		/// <param name="AG">The AtomGroup to execute</param>
		/// <returns>A ProcessResults object containing the results.</returns>
		public ProcessResultSet ProcessAtomGroup(AtomGroup AG) {
			ArrayList runResult = new ArrayList();
			
			if (AG.Operator == AtomGroup.LogicalOperator.And)
				ProcessAnd(AG, runResult, 0, new ArrayList());
			else if (AG.Operator == AtomGroup.LogicalOperator.Or)
				ProcessOr(AG, runResult, new ArrayList());
			else
				throw new BREException("Processor encountered unsupported logical operator: " + AG.Operator);
			
			return new ProcessResultSet(runResult);
		}

		public static Atom Populate(Atom targetAtom, ArrayList resultStack, bool evaluateFormulas) {
	  	IPredicate[] members = (IPredicate[])targetAtom.Members.Clone();
	  	
	  	// populate the variable elements with predicate values coming
	  	// from the query part of the implication
	  	foreach(ResultPocket rp in resultStack)
	  		if (!(rp.fact is FactBase.NegativeFact))
	  			Fact.Populate(rp.fact, rp.source, members);
	  	
	  	// if there are formulas in the atom, resolve these expressions, passing
	  	// the variable values as arguments
	  	if (targetAtom.HasFormula) {
	  		if (evaluateFormulas) {
	  			// formulas must be evaluated and the results placed in individual predicates
		  		IDictionary arguments = new Hashtable();
		  		
		  		foreach(ResultPocket rp in resultStack) {
		  			if (!(rp.fact is FactBase.NegativeFact)) {
			  			for(int i=0; i<rp.source.Members.Length; i++) {
			  				object sourcePredicateKey = null;
		  					
		  					if (rp.source.Members[i] is Variable) {
			  					sourcePredicateKey = rp.source.Members[i].Value;
	    					}
			  				else if (rp.source.SlotNames[i] != String.Empty) {
		  						sourcePredicateKey = rp.source.SlotNames[i];
		  					}
			  				
			  				if ((sourcePredicateKey != null) && (!arguments.Contains(sourcePredicateKey)))
			  					arguments.Add(sourcePredicateKey, rp.fact.Members[i].Value);
			  				
		  				}
	  				}
		  		}
		  	
		  		for(int i=0; i<members.Length; i++)
		  			if (members[i] is Formula)
		  				members[i] = new Individual(((Formula) members[i]).Evaluate(arguments));
	  		}
	  		else {
	  			// formulas must be replaced by variables named after the position of the predicate
		  		for(int i=0; i<members.Length; i++)
		  			if (members[i] is Formula)
		  				members[i] = new Variable(i.ToString());
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
		public static ArrayList EnrichResults(ArrayList resultStack, Atom source, Fact result) {
			ArrayList enrichedProcessResult = new ArrayList();
			enrichedProcessResult.Add(new ResultPocket(source, result));
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
		public static Atom BuildQueryFromDeduction(Atom targetAtom, ArrayList resultStack) {
	  	IPredicate[] members = (IPredicate[])targetAtom.Members.Clone();
	  	
	  	// populate the variable elements with predicate values coming
	  	// from the query part of the implication
	  	foreach(ResultPocket rp in resultStack) Fact.Populate(rp.fact, rp.source, members);
	  	
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
		
		public static Fact[] GetBasicMatches(Atom targetAtom, ArrayList resultStack) {
			ArrayList basicMatches = new ArrayList();
			
			foreach(ResultPocket rp in resultStack)
				if (targetAtom.BasicMatches(rp.fact))
					basicMatches.Add(rp.fact);
			
			return (Fact[]) basicMatches.ToArray(typeof(Fact));
		}
		
		//----------------------------- PRIVATE MEMBERS ------------------------------
		
		private static IList<IList<Fact>> FilterDistinct(ProcessResultSet processResults) {
			IDictionary<long, IList<Fact>> resultSet = new Dictionary<long, IList<Fact>>();
			
			foreach(ArrayList rps in processResults) {
				IList<Fact> row = new List<Fact>();
				long rowLongHashCode = 0;
	  		
				foreach(ResultPocket rp in rps) {
					// naf atom dummy results are skipped
					if (!(rp.fact is FactBase.NegativeFact)) {
						row.Add(rp.fact);
						rowLongHashCode ^= rp.fact.GetLongHashCode();
					}
					rowLongHashCode <<= 1;
				}
				
				// add only new rows to perform a "select distinct"
				if (!resultSet.ContainsKey(rowLongHashCode)) resultSet.Add(rowLongHashCode, new ReadOnlyCollection<Fact>(row));
			}
			
			return new ReadOnlyCollection<IList<Fact>>(new List<IList<Fact>>(resultSet.Values));
		}
		
		private IMatchedFactStorage GetMatchingFactStorageTable(Atom atom) {
			IMatchedFactStorage mfs = (IMatchedFactStorage)matchedFactStorageTable[atom.Signature];
			
			if (mfs == null) {
				if (IEImpl.FactBaseStorageType == FactBaseStorageTypes.DataTable) mfs = new DataTableMatchedFactStorage(atom);
				else mfs = new HashtableMatchedFactStorage(atom);
				
				matchedFactStorageTable.Add(atom.Signature, mfs);
			}
			
			return mfs;
		}
		
		private IEnumerator GetMatchingFacts(Atom atom, ArrayList excludedHashCodes) {
			return GetMatchingFactStorageTable(atom).Select(atom, excludedHashCodes);
		}

		
		// Private members ---------------------------------------------------------		

		private struct ResultPocket 
		{
			public readonly Atom source;
			public readonly Fact fact;
			
			public ResultPocket(Atom source, Fact fact) {
				this.source = source;
				this.fact = fact;
			}
			
			public override string ToString() {
				return "~" + source + "->" + fact;
			}
		}
		
		private class SingleFactEnumerator : AbstractFactEnumerator {
			private readonly Fact singleFactResult;
			
			public SingleFactEnumerator(Fact singleFactResult, IList excludedHashCodes):base() {
				this.singleFactResult = singleFactResult;
			}
			
			protected override bool DoMoveNext() {
				if (position > 0) return false;
				currentFact = singleFactResult;
				return true;
			}
		}
		
		private void ProcessAnd(AtomGroup AG, ArrayList processResult, int parser, ArrayList resultStack)
		{
			if (AG.ResolvedMembers[parser] is AtomGroup) {
				if (((AtomGroup)AG.ResolvedMembers[parser]).Operator == AtomGroup.LogicalOperator.And)
					throw new BREException("Nested And unexpectedly found in atom group:" + AG.ResolvedMembers[parser]);
				
				ArrayList subProcessResult = new ArrayList();
			  
				ProcessOr((AtomGroup)AG.ResolvedMembers[parser], subProcessResult, resultStack);
			  
				foreach(ArrayList resultRow in subProcessResult) {
					foreach(ResultPocket rpRow in resultRow) {
				  	if (resultStack.Count == 0) {
							ArrayList tempResultStack = (ArrayList)resultStack.Clone();
							tempResultStack.Add(rpRow);
							
							if (parser < (AG.OrderedMembers.Length-1)) ProcessAnd(AG, processResult, parser+1, tempResultStack);
					  	else processResult.Add(tempResultStack);
				  	}
				  	else {
					  	// exclude similar results for similar atoms (the engine must produce combinations
					  	// of facts in this case)
					  	bool ignore = false;
					  	
					  	foreach(ResultPocket rp in resultStack) {
					  		if (rpRow.source.IsIntersecting(rp.source)) {
					  			ignore = true;
					  			break;
					  		}
					  		if (!ignore) {
							  	ArrayList tempResultStack = (ArrayList)resultStack.Clone();
									tempResultStack.Add(rpRow);
									
									if (parser < (AG.OrderedMembers.Length-1)) ProcessAnd(AG, processResult, parser+1, tempResultStack);
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
				Atom atomToRun = Populate((Atom)AG.ResolvedMembers[parser], resultStack, false);
				ArrayList excludedHashCodes = new ArrayList();
				
				foreach(ResultPocket rp in resultStack)
					if (((Atom)AG.OrderedMembers[parser]).IsIntersecting(rp.source))
						excludedHashCodes.Add(rp.fact.GetLongHashCode());
		  	
		  	// then get the matching facts
		  	IEnumerator results = ProcessAtom(atomToRun, excludedHashCodes);
			
				if (results != null) {
		  		while(results.MoveNext()) {
			  		Fact result = (Fact)results.Current;
			  		ArrayList tempResultStack = (ArrayList)resultStack.Clone();
						tempResultStack.Add(new ResultPocket((Atom)AG.OrderedMembers[parser], result));

						if (parser < (AG.OrderedMembers.Length-1))
							ProcessAnd(AG, processResult, parser+1, tempResultStack);
				  	else
							processResult.Add(tempResultStack);
				  }
				}
			}
		}
		
		private void ProcessOr(AtomGroup AG, ArrayList processResult, ArrayList resultStack)	{
			foreach(object member in AG.ResolvedMembers) {
				if (member is AtomGroup) {
					if (((AtomGroup)member).Operator == AtomGroup.LogicalOperator.Or)
						throw new BREException("Nested Or unexpectedly found in atom group:" + member);

					ArrayList subProcessResult = new ArrayList();
					ProcessAnd((AtomGroup)member, subProcessResult, 0, resultStack);
					
					foreach(ArrayList resultRow in subProcessResult) processResult.Add(resultRow);
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
					  	ArrayList tempResultStack = new ArrayList();
						  tempResultStack.Add(new ResultPocket((Atom)member, result));
							processResult.Add(tempResultStack);
					  }
					}
				}
				
			}
			
		} //ProcessOr

		private IEnumerator ProcessAtom(Atom atomToRun, ArrayList excludedHashCodes) {
			if (atomToRun is AtomFunction) {
				// an atom function is either positive or negative, it does not return any fact
				// if this atom function is wrapped in naf, then the base result is negated,
				// leading to a positive result if the underlying function is negative!
				if (((AtomFunction)atomToRun).PositiveRelation != atomToRun.Negative)
					return new SingleFactEnumerator(NAF, null);
			}
			else {
				if (atomToRun.Negative) {
					// a negative atom, fails if any matching fact is found
					// and succeed if no data collection (ie returns one dummy fact as a token)
					// if no facts are actually found
					// Bug #1332214 pinpointed that excludedHashCodes should not be applied on Negative atoms
					IEnumerator matchResult = GetMatchingFacts(atomToRun, null);
					if ((matchResult == null) || (!matchResult.MoveNext()))
						return new SingleFactEnumerator(NAF, null);
				}
				else
					return GetMatchingFacts(atomToRun, excludedHashCodes);
			}
			
			return null;
		}
		
	}
}
