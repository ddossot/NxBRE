namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// An abstract enumerator designed for enumerating facts, while potentially skipping some of them based on a list of excluded
	/// hashcodes.
	/// </summary>
	internal abstract class AbstractFactEnumerator : IEnumerator {
		private const int RESET = -1;
		
		protected readonly IList excludedHashCodes;
		
		protected int position;
		
		protected Fact currentFact;
		
		public AbstractFactEnumerator():this(null) {}
		
		public AbstractFactEnumerator(IList excludedHashCodes) {
			this.excludedHashCodes = excludedHashCodes;
			Reset();
		}
		
		public void Reset() {
			position = RESET;
		}
		
		public object Current {
			get {
				if (position == RESET) throw new InvalidOperationException("The atom enumerator is not positioned for reading.");
				return currentFact;
			}
		}
		
		public bool MoveNext() {
			position++;
			
			if (!DoMoveNext()) return false;
			
			if (currentFact == null) return false;
			
			// if the current fact is in the excluded list, automatically move to the next record
			if ((excludedHashCodes != null) && (excludedHashCodes.Contains(currentFact.GetLongHashCode()))) return MoveNext();
			else return true;
		}
		
		protected abstract bool DoMoveNext();
	}
	
}
