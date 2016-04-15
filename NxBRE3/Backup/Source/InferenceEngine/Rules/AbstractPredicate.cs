namespace NxBRE.InferenceEngine.Rules {
	using System;
	
	using NxBRE.Util;
	
	/// <summary>
	/// AbstractPredicate is the prototype for predicates. It is immutable.
	/// </summary>
	/// <author>David Dossot</author>
	public abstract class AbstractPredicate:IPredicate {
		private readonly object predicate;
		private readonly int hashCode;
		
		/// <summary>
		/// The actual value of the Predicate.
		/// </summary>
		public object Value {
			get {
				return predicate;
			}
		}
		
		protected AbstractPredicate(object predicate) {
			if (predicate == null) throw new ArgumentNullException("Null is not a valid predicate.");

			this.predicate = predicate;
			this.hashCode = new HashCodeBuilder().Append(predicate.GetType()).Append(predicate).Value;
		}
		
		public override string ToString() {
			return Value.ToString();
		}
		
		public abstract object Clone();

		public override bool Equals(object o) {
			if (o.GetType() != this.GetType()) return false;
			
			IPredicate other = (IPredicate)o;
			
			return Value.Equals(other.Value);
		}

		public override int GetHashCode() {
			return hashCode;
		}
		
	}
	
}

