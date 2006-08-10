namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	/// <summary>
	/// A Fact is a specialization of Atom, which can be labelled and
	/// contains only Individual predicates. It is immutable.
	/// </summary>
	/// <author>David Dossot</author>
	public class Fact:Atom {
		private readonly string label;
		
		/// <summary>
		/// The Label of the Fact.
		/// </summary>
		public string Label {
			get {
				return label;
			}
		}	

		/// <summary>
		/// Instantiates a new anonymous (not labelled) Fact based on a Type and an array of predicates.
		/// </summary>
		/// <param name="type">The Type of the new Fact.</param>
		/// <param name="members">The Array of predicates that the Fact will contain.</param>
		public Fact(string type, params IPredicate[] members):this(null, type, members) {}
		
		/// <summary>
		/// Instantiates a new labelled Fact based on a Label, a Type and an array of predicates.
		/// </summary>
		/// <param name="label">The Label of the new Fact.</param>
		/// <param name="type">The Type of the new Fact.</param>
		/// <param name="members">The Array of predicates that the Fact will contain.</param>
		public Fact(string label, string type, params IPredicate[] members):base(false, type, members) {
			if (!IsFact) throw new BREException("Can not create Facts on Queries: " + ToString());

			if ((label != null) && (label == String.Empty)) this.label = null;
			else this.label = label;
		}
		
		/// <summary>
		/// Instantiates a new anonymous (not labelled) Fact based on an existing Atom.
		/// </summary>
		/// <param name="atom">The Atom that the fact will be built on.</param>
		public Fact(Atom atom):this(null, atom) {}

		/// <summary>
		/// Instantiates a new labelled Fact based on an existing Atom and a provided Label.
		/// </summary>
		/// <param name="label">The Label of the new Fact.</param>
		/// <param name="atom">The Atom that the Fact will be built on.</param>
		public Fact(string label, Atom atom):base(atom) {
			if ((label != null) && (label == String.Empty)) this.label = null;
			else this.label = label;
		}
		
		private Fact(Fact source, IPredicate[] members):base(source, members) {
			this.label = source.label;
		}

		/// <summary>
		/// Returns a cloned Fact, of same label, type and containing a clone of the array of predicates.
		/// </summary>
		/// <returns>A new Fact, based on the existing one.</returns>
		/// <remarks>The predicates are not cloned.</remarks>
		public override object Clone() {
			return new Fact(Label, this);
		}
				
		/// <summary>
		/// Changes the label of a fact by building a new one with the new label (because it is immutable).
		/// </summary>
		/// <param name="newLabel">The new label to use.</param>
		/// <returns>A new Fact based on the current one and the provided new label.</returns>
		public Fact ChangeLabel(String newLabel) {
			return new Fact(newLabel, this);
		}
		
		/// <summary>
		/// Performs a clone of the current Atom but substitute members with the provided ones.
		/// </summary>
		/// <param name="members">New members to use.</param>
		/// <returns>A clone with new members.</returns>
		public override Atom CloneWithNewMembers(params IPredicate[] members) {
			return new Fact(this, members);
		}
		
	}

}

