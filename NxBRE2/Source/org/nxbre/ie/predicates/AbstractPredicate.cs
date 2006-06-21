namespace org.nxbre.ie.predicates {
	using System;
	
	/// <summary>
	/// AbstractPredicate is the prototype for predicates. It is immutable.
	/// </summary>
	/// <author>David Dossot</author>
	public abstract class AbstractPredicate:IPredicate {
		private readonly object predicate;
		
		// the hashcode is expensive to compute, so do it lazily
		private int hashCode;
		private bool hashCodeInitialized;
		
		/// <summary>
		/// The actual value of the Predicate.
		/// </summary>
		public object Value {
			get {
				return predicate;
			}
		}
		
		protected AbstractPredicate(object predicate) {
			this.predicate = predicate;
			hashCodeInitialized = false;
		}
		
		public override string ToString() {
			return Value.ToString();
		}
		
		public abstract object Clone();
		
		public override int GetHashCode() {
			if (!hashCodeInitialized) {
				long lhc = GetLongHashCode();
				hashCode = (int)(lhc & 0xFFFFFFFF) ^ (int)((lhc >> 32) & 0xFFFFFFFF);
				hashCodeInitialized = true;
			}
			return hashCode;
		}
		
		public override bool Equals(object o) {
			if (!(o is IPredicate)) throw new ArgumentException();
			 
			return ((o.GetType().IsInstanceOfType(this))
			        && (((IPredicate)o).GetLongHashCode() == this.GetLongHashCode()));
		}
		
		public abstract long GetLongHashCode();
		
	}
	
}

