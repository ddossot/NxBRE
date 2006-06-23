namespace org.nxbre.ie.core {
	using System;
	using System.Collections;
	
	using org.nxbre.ie.predicates;
	using org.nxbre.ie.rule;
	using org.nxbre.util;
	
	/// <summary>
	/// A fact storage class that uses Hashtables for storing facts.
	/// </summary>
	internal class HashtableMatchedFactStorage : IMatchedFactStorage {
		private static readonly IComparer LIST_COMPARER = new ListSizeComparer();
		private static readonly IList EMPTY_RESULT = ArrayList.ReadOnly(new ArrayList());
		
		private static bool strictTyping = false;
		private static bool typingLocked = false;
		
		private readonly Atom template;
		private readonly Hashtable factTable;
		private readonly TypeAwareHashtable[] predicateTables;
		
		/// <summary>
		/// Defines whether the fact storage should consider typed objects as equivalent to their String representation.
		/// If StrictTyping is set to true, they are will not be considered equivalent. The default is false.
		/// </summary>
		/// <remarks>
		/// This can not be changed if the storage already contains data. It must be set before any usage of the engine else
		/// unpredictible results may occur.
		/// </remarks>
		public static bool StrictTyping {
			get {
				return strictTyping;
			}
			set {
				if (typingLocked) throw new InvalidOperationException("Can not change storage typing if it contains data.");
				strictTyping = value;
			}
		}
		
		public HashtableMatchedFactStorage(Atom template) {
			if (!typingLocked) typingLocked=true;

			this.template = template;
			
			factTable = new Hashtable();
			predicateTables = new TypeAwareHashtable[template.Members.Length];
			
			InitializePredicateStorage();
		}
		
		private void InitializePredicateStorage() {
			for (int i=0; i<template.Members.Length; i++) predicateTables[i] = new TypeAwareHashtable();
		}
		
		private HashtableMatchedFactStorage(Atom template, Hashtable factTable, TypeAwareHashtable[] predicateTables) {
			this.template = template;
			this.factTable = factTable;
			this.predicateTables = predicateTables;
		}
		
		public object Clone() {
			TypeAwareHashtable[] newPredicateTables = new TypeAwareHashtable[predicateTables.Length];
			for(int i=0; i<predicateTables.Length; i++) newPredicateTables[i] = predicateTables[i].DeepClone();
			
			return new HashtableMatchedFactStorage(template, (Hashtable)factTable.Clone(), newPredicateTables);
		}
		
		public void Add(Fact fact, Atom matchingAtom) {
			if (!factTable.ContainsKey(fact.GetLongHashCode())) factTable.Add(fact.GetLongHashCode(), fact);
			
			Fact resolved = Fact.Resolve(false, fact, matchingAtom);

			for (int i=0; i<resolved.Members.Length; i++) {
				object predicateValue = resolved.GetPredicateValue(i);
				AddFactForPredicateValue(fact, i, predicateValue);
				
				// if we are not strictly enforcing typing and if the predicate value is not a string, then also store it as a string
				if ((!StrictTyping) && (!(predicateValue is string))) AddFactForPredicateValue(fact, i, predicateValue.ToString());
			}
		}
		
		private void AddFactForPredicateValue(Fact fact, int position, object predicateValue) {
			ArrayList matchingHashCodes = (ArrayList)predicateTables[position].Get(predicateValue);

			if (matchingHashCodes == null) {
				matchingHashCodes = new ArrayList();
				predicateTables[position].Add(predicateValue, matchingHashCodes);
			}
			
			// we do not check for duplicates before adding as post processing filters duplicates anyway
			matchingHashCodes.Add(fact.GetLongHashCode());
		}
		
		public void Remove(Fact fact) {
			factTable.Remove(fact.GetLongHashCode());
			
			// if facttable is empty, then we re-initialize the predicate storage
			if (factTable.Count == 0) InitializePredicateStorage();
		}
		
		public IEnumerator Select(Atom filter, ArrayList excludedHashcodes) {
			// if the filter does not contain any individual, then it is fully variable so everything should be returned
			if (!filter.HasIndividual) return factTable.Values.GetEnumerator();
			
			// we build result lists and will reduce the biggest ones from the smallest ones
			ArrayList listOfResults = new ArrayList();
			
			for (int i=0; i<filter.Members.Length; i++) {
				if (filter.Members[i] is Individual) {
					ArrayList matchingHashCodes = (ArrayList)predicateTables[i].Get(filter.GetPredicateValue(i));
														
					if (matchingHashCodes != null) listOfResults.Add(matchingHashCodes);
					else return EMPTY_RESULT.GetEnumerator();
				}
			}

			// nothing was found, return an empty enumerator
			if (listOfResults.Count == 0) return EMPTY_RESULT.GetEnumerator();
			
			// only one list of result was found, no post filtering is needed, return directly
			if (listOfResults.Count == 1) return BuildFactList((IList)listOfResults[0], listOfResults, excludedHashcodes);
			
			// we order the results from the smallest list to the longest one
			if (listOfResults.Count > 1) listOfResults.Sort(LIST_COMPARER);
			
			// we append fact as result only if its long hash code is in all the lists of result
			IList selectResults = new ArrayList();
			ArrayList rootResults = (ArrayList)listOfResults[0];
			
			for(int rootResultIndex=0; rootResultIndex<rootResults.Count; rootResultIndex++) {
				object rootResult = rootResults[rootResultIndex];
				int filteringResultsParser=1;
	
				while(true) {
					ArrayList filteringResults = (ArrayList)listOfResults[filteringResultsParser];
					
					if (filteringResults.IndexOf(rootResult) < 0) break;
					
					filteringResultsParser++;
					
					if (filteringResultsParser == listOfResults.Count) {
						selectResults.Add(rootResult);
						break;
					}
				}
				
			}
			
			return BuildFactList(selectResults, listOfResults, excludedHashcodes);
		}
		
		// ------------------------- Private Members -------------------------
		
		private class TypeAwareHashtable {
			private readonly Hashtable typesTable;
			
			public TypeAwareHashtable() {
				typesTable = new Hashtable();
			}
			
			public void Add(object key, ICloneable value) {
				InternalAdd(key, value);
			}
			
			private void InternalAdd(object key, object value) {
				Hashtable storageTable = (Hashtable)typesTable[key.GetType()];
				
				if (storageTable == null) {
					storageTable = new Hashtable();
					typesTable.Add(key.GetType(), storageTable);
				}
				
				if (!storageTable.ContainsKey(key)) storageTable.Add(key, value);
			}
			
			public object Get(object key) {
				Hashtable storageTable = (Hashtable)typesTable[key.GetType()];
				
				if (storageTable == null) return null;
				else return storageTable[key];
			}
			
			public TypeAwareHashtable DeepClone() {
				TypeAwareHashtable clone = new TypeAwareHashtable();
				
				foreach(Hashtable storageTable in typesTable.Values) {
					foreach(object key in storageTable.Keys) {
						clone.InternalAdd(key, ((ICloneable)storageTable[key]).Clone());
					}
				}
				
				return clone;				
			}
			
			public override string ToString() {
				return Misc.IDictionaryToString(typesTable);
			}
			
		}
		
		private IEnumerator BuildFactList(IList factHashCodes, IList listOfResults, ArrayList excludedHashcodes) {
			ArrayList returnFactList = new ArrayList();
			ArrayList deleteFactList = new ArrayList();
			
			for(int i=0; i<factHashCodes.Count; i++) {
				long factHashCode = (long)factHashCodes[i];
				
				if ((excludedHashcodes == null) ||
				    ((excludedHashcodes != null) && (excludedHashcodes.IndexOf(factHashCode) < 0))) {
					
					Fact fact = (Fact)factTable[factHashCode];
					
					if (fact != null) {
						returnFactList.Add(fact);
						if (excludedHashcodes == null) excludedHashcodes = new ArrayList();
						excludedHashcodes.Add(factHashCode);
					}
					else {
						// the fact was not found in the table, so we have to lazy remove its hashcode from the lists
						deleteFactList.Add(factHashCode);
					}
				}
			}
			
			// perform the deletion
			foreach(long factHashCode in deleteFactList)
				foreach(ArrayList results in listOfResults)
					results.Remove(factHashCode);
			
			return returnFactList.GetEnumerator();
		}
		
		private class ListSizeComparer : IComparer {
			public int Compare(object x, object y) {
				return ((IList)x).Count - ((IList)y).Count;
			}
		}
		
	}
	
}
