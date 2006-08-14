namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;
	
	using NxBRE.InferenceEngine.Core;
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;
	
	/// <summary>
	/// An Atom represents a typed association of predicates. It is immutable (because predicates are immutable so Members will not vary).
	/// </summary>
	/// <remarks>The Atom supports the method for data pattern matching that are the core of
	/// the forward-chaining (data driven) inference engine.</remarks>
	/// <author>David Dossot</author>
	public class Atom:ICloneable {
		private readonly bool negative;
		private readonly string type;
		private readonly IPredicate[] predicates;
		private readonly string signature;
		private readonly string[] slotNames;
		
		private readonly int hashCode;
		private readonly bool isFact;
		private readonly bool hasSlot;
		private readonly bool hasFormula;
		private readonly bool hasFunction;
		private readonly bool hasIndividual;
		private readonly bool onlyVariables;
		
		/// <summary>
		/// Negative fact.
		/// </summary>
		public bool Negative {
			get {
				return negative;
			}
		}
		
		/// <summary>
		/// The type of Atom.
		/// </summary>
		public string Type {
			get {
				return type;
			}
		}
		
		/// <summary>
		/// The array of predicates associated in the Atom.
		/// </summary>
		public IPredicate[] Members {
			get {
				return predicates;
			}
		}
		
		/// <summary>
		/// The array of slot names in the Atom. Non-named members have a String.Empty slot name.
		/// </summary>
		public string[] SlotNames {
			get {
				return slotNames;
			}
		}
		
		/// <summary>
		/// Returns True if there is at least one Slot in the members.
		/// </summary>
		public bool HasSlot {
			get {
				return hasSlot;
			}
		}
		
		/// <summary>
		/// Checks if the current Atom is a Fact by analyzing the predicates.
		/// </summary>
		/// <description>A Fact is an association of predicates of type Individual.</description>
		/// <returns>True if the Atom is a Fact.</returns>
		public bool IsFact {
			get {
				return isFact;
			}
		}
		
		/// <summary>
		/// Returns True if there is at least one Function predicate in the members.
		/// </summary>
		public bool HasFunction {
			get {
				return hasFunction;
			}
		}
		
		/// <summary>
		/// Returns True if there is at least one Formula predicate in the members.
		/// </summary>
		public bool HasFormula {
			get {
				return hasFormula;
			}
		}
		
		/// <summary>
		/// Returns True if there is in the members at least one Individual predicate.
		/// </summary>
		public bool HasIndividual {
			get {
				return hasIndividual;
			}
		}
		
		/// <summary>
		/// Returns True if there is in the members at least one Variable predicate.
		/// </summary>
		public bool HasVariable {
			get {
				return !isFact;
			}
		}
		
		/// <summary>
		/// A String signature used for internal purposes
		/// </summary>
		internal string Signature {
			get {
				return signature;
			}
		}
		
		/// <summary>
		/// Returns True is the members are only Variable predicates.
		/// </summary>
		internal bool OnlyVariables {
			get {
				return onlyVariables;
			}
		}
		
		/// <summary>
		/// Instantiates a new Positive (non-NAF) non-function Relation Atom.
		/// </summary>
		/// <param name="type">Type of the Atom.</param>
		/// <param name="members">Array of predicates associated in the Atom.</param>
		public Atom(string type, params IPredicate[] members):this(false, type, members) {}
		
		/// <summary>
		/// Instantiates a new Atom.
		/// </summary>
		/// <param name="negative">Negative Atom.</param>
		/// <param name="type">The relation type of the Atom.</param>
		/// <param name="members">Array of predicates associated in the Atom.</param>
		/// <remarks>This is the principal constructor for Atom and descendant objects.</remarks>
		public Atom(bool negative, string type, params IPredicate[] members) {
			this.negative = negative;
			this.type = type;
			
			// load the predicates, extracting the slot names if any
			predicates = new IPredicate[members.Length];
			slotNames = new string[members.Length];
			hasSlot = false;
			
			for(int i=0; i<members.Length; i++) {
				if (members[i] is Slot) {
					hasSlot = true;
					Slot slot = (Slot)members[i];
					predicates[i] = slot.Predicate;
					slotNames[i] = slot.Name;
				}
				else {
					predicates[i] = members[i];
					slotNames[i] = String.Empty;
				}
			}
			
			// initialize long hashcode & other characteristics
			HashCodeBuilder hcb = new HashCodeBuilder().Append(type);
			isFact = true;
			hasFunction = false;
			hasFormula = false;
			hasIndividual = false;
			onlyVariables = true;
			
			foreach(IPredicate member in predicates) {
				hcb.Append(member);
				
				if (member is Individual) hasIndividual = true;
				else isFact = false;
				
				if (!(member is Variable)) onlyVariables = false;
				
				if (member is Function) hasFunction = true;
				else if (member is Formula) hasFormula = true;
			}
			
			hashCode = hcb.Value;
			
			// initialize signature
			signature = type + predicates.Length;
		}
		
		/// <summary>
		/// Protected constructor used for cloning purpose.
		/// </summary>
		/// <param name="source">The atom to use as a template.</param>
		/// <param name="members">The members to use instead of the ones in the source, or null if the ones of the source must be used.</param>
		protected Atom(Atom source, IPredicate[] members):this(source.negative, source.type, members)	{
			this.slotNames = source.slotNames;
		}
		
		/// <summary>
		/// Protected constructor used for cloning purpose.
		/// </summary>
		/// <param name="source"></param>
		protected Atom(Atom source):this(source, (IPredicate[])source.predicates.Clone()) {}

		/// <summary>
		/// Returns a cloned Atom, of same type and containing a clone of the array of predicates.
		/// </summary>
		/// <returns>A new Atom, based on the existing one.</returns>
		/// <remarks>The predicates are not cloned.</remarks>
		public virtual object Clone() {
			return new Atom(this);
		}
		
		/// <summary>
		/// Performs a clone of the current Atom but substitute members with the provided ones.
		/// </summary>
		/// <param name="members">New members to use.</param>
		/// <returns>A clone with new members.</returns>
		public virtual Atom CloneWithNewMembers(params IPredicate[] members) {
			return new Atom(this, members);
		}
		
		/// <summary>
		/// Checks if the signature of the current Atom matches with the signature of another one.
		/// </summary>
		/// <param name="atom">The other atom to determine the signature matching.</param>
		/// <returns>True if the two atoms have the same signature, False otherwise.</returns>
		public bool BasicMatches(Atom atom) {
			return (atom.Signature == Signature);
		}
		
		/// <summary>
		/// Checks if the predicates of the current Atom match with the predicates of another one, i.e. are equal or functions resolve to True.
		/// </summary>
		/// <param name="atom">The other atom to determine the predicates matching.</param>
		/// <param name="strictTyping">True if String individual predicate are not considered as potential representations of other types.</param>
		/// <param name="ignoredPredicates">A list of predicate positions to exclude from comparison, or null if all predicates must be matched</param>
		/// <returns>True if the two atoms have matching predicates, False otherwise.</returns>
		internal bool PredicatesMatch(Atom atom, bool strictTyping, IList<int> ignoredPredicates) {
			for(int position=0; position<predicates.Length; position++) {
				if ((ignoredPredicates == null) || ((ignoredPredicates != null) && (!ignoredPredicates.Contains(position)))) {
					if ((predicates[position] is Individual) &&
					    (atom.predicates[position] is Function) &&
					    (!((Function)atom.predicates[position]).Evaluate((Individual)predicates[position]))) {
						return false;
					}
					else if ((predicates[position] is Function) &&
					         (atom.predicates[position] is Individual) &&
					         (!((Function)predicates[position]).Evaluate((Individual)atom.predicates[position]))) {
						return false;
					}
					else if ((predicates[position] is Function) &&
					         (atom.predicates[position] is Function) &&
					         (!(predicates[position].Equals(atom.predicates[position])))) {
						return false;
					}
					else if ((predicates[position] is Individual) && (atom.predicates[position] is Individual)) {
						// we have two individuals
						if (predicates[position].Value.GetType() == atom.predicates[position].Value.GetType()) {
						  if (!predicates[position].Equals(atom.predicates[position])) 
								// the two individuals are of same types: if equals fail, no match
								return false;
						}
						else {
							if (!strictTyping) {
								// the two individuals are of different types and we are not in strict typing, so
								// we try to cast to stronger type and compare
								ObjectPair pair = new ObjectPair(predicates[position].Value, atom.predicates[position].Value);
								Reflection.CastToStrongType(pair);
								if (!pair.First.Equals(pair.Second)) return false;
							}
							else {
								return false;
							}
						}
					}
				}
			}
			
			// we went through all the comparisons without a scratch, it means the atoms do match
			return true;			
		}

		/// <summary>
		/// Checks if the current Atom matches with another one, i.e. if they are of same type,
		/// contain the same number of predicates, and if their Individual predicates are equal.
		/// </summary>
		/// <description>
		/// This functions takes care of casting as it always tries to cast to the strongest type
		/// of two compared individuals. Since predicates can come from weakly-typed rule files
		/// (Strings) and other predicates can be generated by the user, this function tries to
		/// convert from String to the type of the other predicate (as String is considered not
		/// strongly typed).
		/// </description>
		/// <param name="atom">The other atom to determine the matching.</param>
		/// <returns>True if the two atoms match.</returns>
		public bool Matches(Atom atom) {
			if (!BasicMatches(atom)) return false;
			
			return PredicatesMatch(atom, false, null);
		}
		
		/// <summary>
		/// Check if the current intersects with another one, which means that:
		///  - they Match() together,
		///  - their predicate types are similar,
		///  - if there are variables, at least one should be equal to the corresponding one.
		/// </summary>
		/// <param name="atom">The other atom to determine the intersection.</param>
		/// <returns>True if the two atoms intersect.</returns>
		/// <remarks>IsIntersecting calls Matches first.</remarks>
		public bool IsIntersecting(Atom atom) {
			if (!Matches(atom)) return false;

			for(int i=0; i<predicates.Length; i++)
				if (predicates[i].GetType() != atom.predicates[i].GetType())
					return false;
			
			if (!HasVariable) return true;
			
			int nonMatchingVariables = 0;
			int variableCount = 0;
			
			for(int i=0; i<predicates.Length; i++) {
				variableCount++;
				if (predicates[i] is Variable) {
					if (!(predicates[i].Equals(atom.predicates[i])))	nonMatchingVariables++;
				}
			}
	
			if (variableCount < predicates.Length) return true;
			else return (nonMatchingVariables < variableCount);
		}		
		
		/// <summary>
		/// Returns the String representation of the Atom for display purpose only.
		/// </summary>
		/// <remarks>
		/// If the Inference Engine trace switch is Verbose, the type of non String predicates will be shown.
		/// </remarks>
		/// <returns>The String representation of the Atom.</returns>
		/// <see cref="ToString(bool outputType)"/>
		public override string ToString() {
			return ToString(Logger.IsInferenceEngineVerbose);
		}
		
		/// <summary>
		/// Returns the String representation of the Atom for display purpose only.
		/// </summary>
		/// <param name="outputType">If True, the type of non String predicates will be displayed</param>
		/// <returns>The String representation of the Atom.</returns>
		public string ToString(bool outputType) {
			StringBuilder result = new StringBuilder(negative?"!":"");
			result.Append(type).Append("{");
			bool first = true;
			
			for(int i=0; i<predicates.Length; i++) {
				IPredicate member = predicates[i];
				if (!first) result.Append(",");
				if (slotNames[i] != String.Empty) result.Append(slotNames[i]).Append("=");
				result.Append(member.ToString());
				
				// Type is displayed for non-string predicates, as suggested by Chuck Cross
				if ((outputType) && (member.Value.GetType() != typeof(System.String)))
					result.Append(" [").Append(member.Value.GetType().ToString()).Append("]");
				
				if (first) first = false;
			}
			
			result.Append("}");
			return result.ToString();
		}
		
		/// <summary>
		/// Checks if the current Atom is equal to another one, based on their type and predicates.
		/// </summary>
		/// <param name="o">The other Atom to test the equality.</param>
		/// <returns>True if the two atoms are equal.</returns>
		public override bool Equals(object o) {
			if (o.GetType() != this.GetType()) return false;
			
			Atom other = (Atom)o;
			
			if (Signature != other.Signature) return false;
			
			for(int i=0; i<Members.Length; i++)
				if (!Members[i].Equals(other.Members[i]))
					return false;
			
			return true;
		}
		
		/// <summary>
		/// Calculates the hashcode of the current Atom.
		/// </summary>
		/// <returns>The hashcode of the current Atom.</returns>
		public override int GetHashCode() {
			return hashCode;
		}

		/// <summary>
		/// A helper method for easily reaching a member predicate value from its index.
		/// </summary>
		/// <param name="predicateIndex">The index of the predicate in the array of Members.</param>
		/// <returns>The actual value of the predicate, or throws an exception if the index is out of range.</returns>
		public object GetPredicateValue(int predicateIndex) {
			return predicates[predicateIndex].Value;
		}
		
		/// <summary>
		/// A helper method for easily reaching a member predicate value from its slot name.
		/// </summary>
		/// <param name="slotName">The name of the slot in which the predicate is stored</param>
		/// <returns>The actual value of the predicate, or throws an exception if no slot matches the name.</returns>
		public object GetPredicateValue(string slotName) {
			IPredicate predicate = GetPredicate(slotName);
			if (predicate == null) throw new ArgumentException("There is no slot named: " + slotName);
			else return predicate.Value;
		}
		
		/// <summary>
		/// A helper method for easily reaching a member predicate from its slot name.
		/// </summary>
		/// <param name="slotName">The name of the slot in which the predicate is stored</param>
		/// <returns>The predicate or null if no slot matches the name.</returns>
		public IPredicate GetPredicate(string slotName) {
			if ((slotName == null) || (slotName == String.Empty)) throw new ArgumentException("The name of a slot can not be null or empty");
			
			int slotIndex = Array.IndexOf(slotNames, slotName);
			
			if (slotIndex < 0) return null;
			else return predicates[slotIndex];
		}
		
		/// <summary>
		/// A helper accessor for easily getting the member predicate values.
		/// </summary>
		/// <returns>An array of objects containing the member predicate values.</returns>
		/// <remarks>
		/// This method is inefficient performance-wise and should be used wisely.
		/// </remarks>
		public object[] PredicateValues {
			get {
				ArrayList values = new ArrayList();
				foreach(IPredicate member in predicates)values.Add(member.Value);
				return values.ToArray();
			}
		}
		
	}

}
