namespace org.nxbre.ie.rule {
	using System;
	using System.Collections;
	
	/// <summary>
	/// An Equivalent is a pair of Atoms that are equivalent together, meaning that they can indiferently be used in Queries
	/// search expressions.
	/// </summary>
	/// <version>2.5</version>
	public sealed class Equivalent {
		private readonly string label;
		private readonly Atom firstAtom;
		private readonly Atom secondAtom;
		
		/// <summary>
		/// The optional label of the Equivalent.
		/// </summary>
		public string Label {
			get {
				return label;
			}
		}
		
		/// <summary>
		/// One of the atoms in the Equivalent pair.
		/// </summary>
		public Atom FirstAtom {
			get {
				return firstAtom;
			}
		}
		
		/// <summary>
		/// The other atom of the Equivalent pair.
		/// </summary>
		public Atom SecondAtom {
			get {
				return secondAtom;
			}
		}
		
		/// <summary>
		/// Instantiates a new Equivalent pair of atoms.
		/// </summary>
		/// <param name="firstAtom">One of the atoms in the Equivalent pair.</param>
		/// <param name="secondAtom">The other atom of the Equivalent pair.</param>
		/// <remarks>This object is immutable</remarks>
		public Equivalent(Atom firstAtom, Atom secondAtom):this(null, firstAtom, secondAtom) {}
		
		/// <summary>
		/// Instantiates a new Equivalent pair of atoms.
		/// </summary>
		/// <param name="label">The optional label, or null if not needed.</param>
		/// <param name="firstAtom">One of the atoms in the Equivalent pair.</param>
		/// <param name="secondAtom">The other atom of the Equivalent pair.</param>
		/// <remarks>This object is immutable</remarks>
		public Equivalent(string label, Atom firstAtom, Atom secondAtom)
		{
			this.label = label;
			this.firstAtom = firstAtom;
			this.secondAtom = secondAtom;
		}
		
		/// <summary>
		/// If the passed atom matches one of the atoms in the Equivalent pair, returns the other atom, with its variable
		/// names modified to match the ones of the passed atom.
		/// </summary>
		/// <param name="atom">The atom that is potentially equivalent to one of the atoms of the Equivalent pair.</param>
		/// <returns>The atom equivalent to the passed one, or null if none of the atom pair is matching it.</returns>
		public Atom Get(Atom atom) {
			if (firstAtom.Matches(atom)) return Atom.TranslateVariables(atom, firstAtom, secondAtom);
			else if (secondAtom.Matches(atom)) return Atom.TranslateVariables(atom, secondAtom, firstAtom);
			else return null;
		}
		
		/// <summary>
		/// Internal method used for exploring the potential hierarchy of equivalence and building the complete
		/// list of atoms equivalent to one atom.
		/// </summary>
		internal static ArrayList GetAll(ArrayList equivalentPairs, Atom atom, ArrayList equivalentAtoms) {
			if ((atom != null) && (!equivalentAtoms.Contains(atom))) {
				equivalentAtoms.Add(atom);
				foreach(Equivalent equivalentPair in equivalentPairs) GetAll(equivalentPairs, equivalentPair.Get(atom), equivalentAtoms);
			}
			
			// returning the list is not necessary at all but just convenient for the calling methods
			return equivalentAtoms;
		}
		
	}
	
}
