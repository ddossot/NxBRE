namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;
	using System.Text.RegularExpressions;
	using System.Security.Cryptography;
	using System.Threading;

	/// <summary>
	/// An Individual is a predicate that represents a fixed value.
	/// </summary>
	/// <author>David Dossot</author>
	public sealed class Individual:AbstractPredicate {
		private readonly string sourceType;
		
		public string SourceType {
			get {
				return sourceType;
			}
		}

		public Individual(object predicate):this(predicate, null) {}
	
		public Individual(object predicate, string sourceType):base(predicate) {
			this.sourceType = sourceType;
		}
	
		/// <summary>
		/// Returns a shallow clone of the Individual, the base value is not cloned.
		/// </summary>
		/// <returns>A clone of the current Individual.</returns>
		public override object Clone() {
			return new Individual(Value, SourceType);
		}
		
		/// <summary>
		/// A helper method for easily creating an array of Individual predicates.
		/// </summary>
		/// <param name="predicates">The array of predicate values.</param>
		/// <returns>The array of Individual built on the predicate values.</returns>
		public static Individual[] NewArray(params object[] predicates) {
			Individual[] individuals = new Individual[predicates.Length];
			
			for (int i=0; i<predicates.Length; i++)
				individuals[i] = new Individual(predicates[i]);
				
			return individuals;
		}
		
	}
}
