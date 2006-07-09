namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;
	
	using NxBRE.Util;
	
	/// <summary>
	/// Represents a container that can hold ordered atoms and other groups of atoms related together
	/// by a logical operator. This defines the hierarchy of atoms in a query or an implication.
	/// It is immutable.
	/// </summary>
	public class AtomGroup:ICloneable {
		/// <summary>
		/// Available AtomGroup Logical Operators
		/// </summary>
		public enum LogicalOperator {And, Or};
		
		private readonly LogicalOperator logicalOperator;
		private readonly object[] members;
		private readonly object[] orderedMembers;
		private readonly object[] resolvedMembers;
		
		/// <summary>
		/// The logical operator used to link the members together
		/// </summary>
		public LogicalOperator Operator {
			get {
				return logicalOperator;
			}
		}
		
		/// <summary>
		/// The members of the current group, in the original order.
		/// </summary>
		public object[] Members {
			get {
				return members;
			}
		}

		/// <summary>
		/// The atoms in the current group, and all the subgroups.
		/// </summary>
		internal Atom[] AllAtoms {
			get {
				ArrayList result = new ArrayList();
				
				foreach(object member in orderedMembers) {
					if (member is Atom) result.Add(member);
					else if (member is AtomGroup) result.AddRange(((AtomGroup)member).AllAtoms);
				}
				
				return (Atom[])result.ToArray(typeof(Atom));
			}
		}
		
		/// <summary>
		/// The atom groups and atoms in the current group with their functions resolved, in the processing order.
		/// </summary>
		internal object[] OrderedMembers {
			get {
				return orderedMembers;
			}
		}
		
		/// <summary>
		/// The atom groups and atoms in the current group with their functions resolved, in the processing order.
		/// </summary>
		internal object[] ResolvedMembers {
			get {
				return resolvedMembers;
			}
		}
		
		/// <summary>
		/// A comparer that sorts in this order: FunctionRel-Atom/Naf-Atom/AtomGroups/Atom
		/// </summary>
		private class AtomComparer:IComparer {
			private int ObjectScore(object x) {
				if (x is AtomFunction) return 3;
				if ((x is Atom) && ((Atom)x).Negative) return 2;
				if (x is AtomGroup) return 1;
				return 0;
			}
			
			public int Compare(object x, object y) {
				return ObjectScore(x)- ObjectScore(y);
			}
		}

		/// <summary>
		/// Instantiates a new atom group.
		/// </summary>
		/// <param name="logicalOperator">The operator that characterizes the relationship between the atoms and atoms group.</param>
		/// <param name="members">An array containing atoms and atom groups.</param>
		public AtomGroup(LogicalOperator logicalOperator, params object[] members):this(logicalOperator, members, members) {}
		
		
		/// <summary>
		/// Instantiates a new atom group.
		/// </summary>
		/// <param name="logicalOperator">The operator that characterizes the relationship between the atoms and atoms group.</param>
		/// <param name="members">An array containing atoms and atom groups.</param>
		internal AtomGroup(LogicalOperator logicalOperator, object[] members, object[] runningMembers) {
			// order atoms & groups so naf-atoms are at the end
			foreach(object o in members) {
				if (o == null)
					throw new BREException("An atom group can not contain a null member");
				else if (o is AtomGroup) {
					if (((AtomGroup)o).logicalOperator == logicalOperator)
						throw new BREException("An atom group can not contain another group with the same logical operator");
				}
				else if (o is Atom) {
					if (((Atom)o).HasFormula)
						throw new BREException("An atom group can not contain an atom that contains a formula");
				}
				else
					throw new BREException("An atom group can not hold objects of type: " + o.GetType());
			}
			
			this.logicalOperator = logicalOperator;
			this.members = members;
			
			object[] membersToOrder = (object[])runningMembers.Clone();
			Array.Sort(membersToOrder, new AtomComparer());
			this.orderedMembers = membersToOrder;
			
			resolvedMembers = new object[orderedMembers.Length];
			
			for(int i=0; i<orderedMembers.Length; i++) {
				if (orderedMembers[i] is Atom) resolvedMembers[i] = Atom.ResolveFunctions((Atom)orderedMembers[i]);
				else resolvedMembers[i] = orderedMembers[i];
			}
		}
		
		/// <summary>
		/// Shallow cloning of the current Atom group.
		/// </summary>
		/// <returns>A shallow clone of the current group.</returns>
		public object Clone() {
			return new AtomGroup(this.logicalOperator, this.members.Clone());
		}
		
		/// <summary>
		/// Determines whether two AtomGroup objects have the same value.
		/// </summary>
		/// <param name="o">An Object.</param>
		/// <returns>true if o is a AtomGroup and its value is the same as this instance;
		/// otherwise, false.</returns>
		public override bool Equals(object o) {
			if (!(o is AtomGroup)) return false;
			return (o.GetHashCode() == this.GetHashCode());
		}
	
		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode() {
		  int hashCode = logicalOperator.GetHashCode();
	
			foreach(object o in members) 
				hashCode ^= o.GetHashCode();
	
			return hashCode;
		}
		
		/// <summary>
		/// Returns the String representation of the AtomGroup for display purpose only.
		/// </summary>
		/// <returns>The String representation of the AtomGroup.</returns>
		public override string ToString() {
			return Operator +
						 "(\n" +
						 Misc.ArrayListToString(new ArrayList(members), "  ") +
						 ")";
		}
		
		/// <summary>
		/// Returns the String representation of the AtomGroup running members for internal purpose only.
		/// </summary>
		/// <returns>The String representation of the AtomGroup.</returns>
		internal string ToStringWithRunningMembers() {
			return Operator +
						 "(\n" +
						 Misc.ArrayListToString(new ArrayList(orderedMembers), "  ") +
						 ")";
		}
		
	}
}
