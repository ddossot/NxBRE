namespace NxBRE.InferenceEngine.Rules {
	using System;
	
	/// <summary>
	/// A Variable is a predicate that represents a named value placeholder.
	/// </summary>
	/// <author>David Dossot</author>
	public sealed class Variable:AbstractPredicate {
		public Variable(object predicate):base(predicate) {}
	
		/// <summary>
		/// Converts the value of this Variable to its equivalent string representation,
		/// for display only. 
		/// </summary>
		/// <returns>The string representation of the value of this Variable.</returns>
		public override string ToString() {
			return "?" + Value;
		}
		
		/// <summary>
		/// Returns a shallow clone of the Variable, the base value is not cloned.
		/// </summary>
		/// <returns>A clone of the current Variable</returns>
		public override object Clone() {
			return new Variable(Value);
		}
	}
}
