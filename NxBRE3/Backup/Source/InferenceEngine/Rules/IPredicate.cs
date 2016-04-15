namespace NxBRE.InferenceEngine.Rules {
	using System;

	///<summary>
	/// A Predicate is a cloneable unmutable object that returns a fixed value.
	///</summary>
	/// <author>David Dossot</author>
	public interface IPredicate:ICloneable {
		/// <summary>
		/// The actual value of the Predicate.
		/// </summary>
		object Value { get; }
	}
}
