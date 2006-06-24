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
			if (!IsFact)
				throw new BREException("Can not create Facts on Queries: " + ToString());

			if ((null != label) && (String.Empty == label)) this.label = null;
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
			this.label = label;
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
		
		// -------------------- Static Members --------------------
		
		/// <summary>
		/// Populates the Variable predicates of a target Atom, using another Atom as a template
		/// and a Fact as the source of data, i.e. Individual predicates.
		/// </summary>
		/// <param name="data">The data for populating the Atom.</param>
		/// <param name="template">The template of Atom being populated.</param>
		/// <param name="target">The members to populate.</param>
		public static void Populate(Fact data, Atom template, IPredicate[] members) {
			for(int i=0;i<members.Length;i++)
				if (members[i] is Variable) {
					// try to locate a Variable with the same name in the template
					int j = Array.IndexOf(template.Members, members[i]);
					if (j >= 0) {
						members[i] = data.Members[j];
					}
					else {
						// try to locate a Slot with the same name in the template
						j = Array.IndexOf(template.SlotNames, members[i].Value);
						if (j >= 0) members[i] = data.Members[j];	
					}
				}
		}
	
		/// <summary>
		/// Prepare the atom to be pattern matched by replacing in a fact:
		/// -  all the predicates that match function predicates in the passed atom with 
		///   the string representation of these function predicates,
		/// -  all the predicates that match individual predicates in the passed atom with 
		///   the string representation of these individual predicates.
		/// </summary>
		/// <remarks>
		/// This operation must be done *if and only if* the fact matches the atom.
		/// </remarks>
		/// <param name="factToResolve">The fact that must be resolved.</param>
		/// <param name="atom">The atom with which the current fact matches.</param>
		/// <returns>A new fact with only String individuals.</returns>
		public static Fact Resolve(Fact factToResolve, Atom atom) {
			return Resolve(true, factToResolve, atom);
		}
	
		/// <summary>
		/// Prepare the atom to be pattern matched by replacing in a fact:
		/// -  all the predicates that match function predicates in the passed atom with 
		///   the string representation of these function predicates,
		/// -  in fully mode, all the predicates that match individual predicates in the passed atom with 
		///   the string representation of these individual predicates.
		/// </summary>
		/// <remarks>
		/// This operation must be done *if and only if* the fact matches the atom.
		/// </remarks>
		/// <param name="fully">Forces resolution of non-string individual to String.</param>
		/// <param name="factToResolve">The fact that must be resolved.</param>
		/// <param name="atom">The atom with which the current fact matches.</param>
		/// <returns>A new fact with only String individuals.</returns>
		public static Fact Resolve(bool fully, Fact factToResolve, Atom atom) {
			IPredicate[] predicates = new IPredicate[factToResolve.Members.Length];
			
			for(int i=0; i<factToResolve.Members.Length; i++) {
				if ((atom.Members[i] is Function)
						|| ((fully) && (atom.Members[i] is Individual) && (!(factToResolve.Members[i].Value is System.String)))) {
					predicates[i] = new Individual(atom.Members[i].ToString());
				}
				else {
					predicates[i] = factToResolve.Members[i];
				}
			}
			
			return (Fact)factToResolve.CloneWithNewMembers(predicates);
		}
		
	}

}

