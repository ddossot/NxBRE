namespace NxBRE.InferenceEngine.Core
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;

	internal abstract class RulesUtil
	{
		private RulesUtil() {}
		
		/* Equivalent Utils */
		
		/// <summary>
		/// Internal method used for exploring the potential hierarchy of equivalence and building the complete
		/// list of atoms equivalent to one atom.
		/// </summary>
		internal static ArrayList GetAll(IList<Equivalent> equivalentPairs, Atom atom, ArrayList equivalentAtoms) {
			if ((atom != null) && (!equivalentAtoms.Contains(atom))) {
				equivalentAtoms.Add(atom);
				foreach(Equivalent equivalentPair in equivalentPairs) GetAll(equivalentPairs, equivalentPair.Get(atom), equivalentAtoms);
			}
			
			// returning the list is not necessary at all but just convenient for the calling methods
			return equivalentAtoms;
		}
		
		/* Atom Utils */
		
		/// <summary>
		/// Resolves all Function predicates by replacing them by their String representations.
		/// </summary>
		/// <param name="atom">The Atom to resolve.</param>
		/// <returns>A new Atom where all Function predicates have been resolved. If no
		/// Function predicate exists, it returns a clone of the current Atom.</returns>
		internal static Atom ResolveFunctions(Atom atom) {
			if (atom.HasFunction) {
				IPredicate[] predicates = new IPredicate[atom.Members.Length];
				
				for(int i=0; i<atom.Members.Length; i++)
					if (atom.Members[i] is Function) predicates[i] = new Individual(atom.Members[i].ToString());
					else predicates[i] = atom.Members[i];
				return new Atom(atom.Negative, atom.Type, predicates);
			}
			else
				return (Atom)atom.Clone();
		}
		
		/// <summary>
		/// Translates variable names of a target atom with names from a template atom matching the position of a
		/// source atom.
		/// </summary>
		/// <remarks>Template and source atoms must match together else unpredictible result may occur.</remarks>
		/// <param name="template"></param>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		internal static Atom TranslateVariables(Atom template, Atom source, Atom target) {
			IPredicate[] resultMembers = new IPredicate[target.Members.Length];
			
			for(int i=0; i<target.Members.Length; i++) {
				IPredicate targetMember = target.Members[i];
				
				if (targetMember is Variable) {
					int indexOfSourceMember = Array.IndexOf(source.Members, targetMember);
					if (indexOfSourceMember >= 0) resultMembers[i] = template.Members[indexOfSourceMember];
					else resultMembers[i] = targetMember;
				}
				else
					resultMembers[i] = targetMember;
			}
			
			return new Atom(template.Negative, target.Type, resultMembers);
		}
		
		/* Fact Utils */
		
		/// <summary>
		/// Populates the Variable predicates of a target Atom, using another Atom as a template
		/// and a Fact as the source of data, i.e. Individual predicates.
		/// </summary>
		/// <param name="data">The data for populating the Atom.</param>
		/// <param name="template">The template of Atom being populated.</param>
		/// <param name="members">The members to populate.</param>
		internal static void Populate(Fact data, Atom template, IPredicate[] members) {
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
		internal static Fact Resolve(Fact factToResolve, Atom atom) {
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
		internal static Fact Resolve(bool fully, Fact factToResolve, Atom atom) {
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
