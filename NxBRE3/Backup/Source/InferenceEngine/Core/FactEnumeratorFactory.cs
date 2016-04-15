namespace NxBRE.InferenceEngine.Core
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	internal abstract class FactEnumeratorFactory
	{
		private FactEnumeratorFactory() {}
		
		public static IEnumerator<Fact> NewSingleFactEnumerator(Fact fact) {
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "NewSingleFactEnumerator: " + fact);
			return new SingleFactEnumerator(fact);
		}
		
		public static IEnumerator<Fact> NewFactListExcludingEnumerator(ICollection<Fact> factList, IList<Fact> excludedFacts) {
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "NewFactListExcludingEnumerator: factList.Count=" + factList.Count + " - excludedFacts.Count=" + (excludedFacts != null?excludedFacts.Count:0));
			return new FactListEnumerator(factList, excludedFacts);
		}
		
		public static IEnumerator<Fact> NewFactListPredicateMatchingEnumerator(ICollection<Fact> factList, Atom filter, bool strictTyping, IList<int> ignoredPredicates, IList<Fact> excludedFacts) {
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "NewFactListPredicateMatchingEnumerator: factList.Count=" + factList.Count + " - filter=" + filter + " - strictTyping=" + strictTyping + " - ignoredPredicates=" + Misc.IListToString((System.Collections.IList)ignoredPredicates) + " - excludedFacts.Count=" + (excludedFacts != null?excludedFacts.Count:0));
			return new FactListPredicateMatchingEnumerator(factList, filter, strictTyping, ignoredPredicates, excludedFacts);
		}
			
		// ---- Private Classes -----
		
		private abstract class AbstractExcludingFactEnumerator<Fact> : IEnumerator<Fact> {
			protected IList<Fact> excludedFacts;
			
			protected Fact currentFact;
			
			protected bool disposed = false;
			
			public AbstractExcludingFactEnumerator():this(null) {}
			
			public AbstractExcludingFactEnumerator(IList<Fact> excludedFacts) {
				this.excludedFacts = excludedFacts;
			}
			
			public virtual void Dispose()
			{
				disposed = true;
				excludedFacts = null;
				currentFact = default(Fact);
			}
			
			protected void CheckDisposed() {
				if (disposed) throw new InvalidOperationException("Impossible operation: the fact enumerator has been disposed.");
			}
			
			public abstract void Reset();
			
			private Fact GetCurrent() {
				CheckDisposed();
				return currentFact;
			}
			
			object System.Collections.IEnumerator.Current {
				get {
					return GetCurrent();
				}
			}
			
			Fact IEnumerator<Fact>.Current {
				get {
					return GetCurrent();
				}
			}
			
			public virtual bool MoveNext() {
				CheckDisposed();
				
				if (!DoMoveNext()) return false;
				
				if (currentFact == null) return false;
				
				// if the current fact is in the excluded list, automatically move to the next record
				if ((excludedFacts != null) && (excludedFacts.Contains(currentFact))) return MoveNext();
				else return true;
			}
			
			protected abstract bool DoMoveNext();
		}
	
		private class SingleFactEnumerator : AbstractExcludingFactEnumerator<Fact> {
			private readonly Fact singleFactResult;
			
			private bool readyToRead = true;
			
			public SingleFactEnumerator(Fact singleFactResult):base() {
				this.singleFactResult = singleFactResult;
			}
			
			public override void Reset() {
				CheckDisposed();
				readyToRead = true;
			}
			
			protected override bool DoMoveNext() {
				if (!readyToRead) {
					currentFact = null;
					return false;
				}
				else {
					currentFact = singleFactResult;
					readyToRead = false;
					return true;
				}
			}
	}
	
		private class FactListEnumerator : AbstractExcludingFactEnumerator<Fact> {
			private IEnumerator<Fact> factEnumerator;
			
			public FactListEnumerator(ICollection<Fact> factList):this(factList, null) {}
			
			public FactListEnumerator(ICollection<Fact> factList, IList<Fact> excludedFacts):base(excludedFacts) {
				this.factEnumerator = factList.GetEnumerator();
			}
			
			public override void Reset() {
				CheckDisposed();
				factEnumerator.Reset();
			}
			
			public override void Dispose() {
				base.Dispose();
				factEnumerator = null;
			}
			
			protected override bool DoMoveNext() {
				if (!factEnumerator.MoveNext()) return false;
				currentFact = factEnumerator.Current;
				return true;
			}
		}
	
		private class FactListPredicateMatchingEnumerator : FactListEnumerator {
			private Atom filter;
			private bool strictTyping;
			private IList<int> ignoredPredicates;
			
			public FactListPredicateMatchingEnumerator(ICollection<Fact> factList, Atom filter, bool strictTyping, IList<int> ignoredPredicates):this(factList, filter, strictTyping, ignoredPredicates, null) {}
			
			public FactListPredicateMatchingEnumerator(ICollection<Fact> factList, Atom filter, bool strictTyping, IList<int> ignoredPredicates, IList<Fact> excludedFacts):base(factList, excludedFacts) {
				this.filter = filter;
				this.strictTyping = strictTyping;
				this.ignoredPredicates = ignoredPredicates;
			}
			
			public override void Dispose() {
				base.Dispose();
				filter = null;
				ignoredPredicates = null;
			}
			
			public override bool MoveNext() {
				if (base.MoveNext()) {
					if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "FactListPredicateMatchingEnumerator.MoveNext: currentFact=" + currentFact + " -> " + currentFact.PredicatesMatch(filter, strictTyping, ignoredPredicates));
					
					if (!currentFact.PredicatesMatch(filter, strictTyping, ignoredPredicates)) return MoveNext();
					else return true;
				}
				else {
					return false;
				}
			}
			
		}

	}
}
