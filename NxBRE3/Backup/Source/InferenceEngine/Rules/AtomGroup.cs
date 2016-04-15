namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Core;
	
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
		private readonly IList<object> orderedMembers;
		private readonly int hashCode;
		private readonly List<Atom> allAtoms;
		
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
		/// A list of all the atoms in the current group and all the subgroups, explored recursively.
		/// </summary>
		public IList<Atom> AllAtoms {
			get {
				return allAtoms;
			}
		}
		
		/// <summary>
		/// The atom groups and atoms in the current group with their functions resolved, in the processing order.
		/// </summary>
		internal IList<object> OrderedMembers {
			get {
				return orderedMembers;
			}
		}
		
		/// <summary>
		/// Compute an index that allows maitaining original order of members of same kind for sorting them while preserving
		/// order.
		/// </summary>
		/// <param name="runningMembers"></param>
		/// <param name="originalIndex"></param>
		/// <returns></returns>
		private int GetMemberSortedIndex(object[] runningMembers, int originalIndex) {
			if (runningMembers[originalIndex] is AtomFunction) return 3 * runningMembers.Length + originalIndex;
			else if ((runningMembers[originalIndex] is Atom) && ((Atom)runningMembers[originalIndex]).Negative) return 2 * runningMembers.Length + originalIndex;
			else if (runningMembers[originalIndex] is AtomGroup) return runningMembers.Length + originalIndex;
			else return originalIndex;
		}
		
		/// <summary>
		/// Instantiates a new atom group.
		/// </summary>
		/// <param name="logicalOperator">The operator that characterizes the relationship between the atoms and atoms group.</param>
		/// <param name="members">An array containing atoms and atom groups.</param>
		public AtomGroup(LogicalOperator logicalOperator, params object[] members):this(logicalOperator, members, null) {}
		
		/// <summary>
		/// Instantiates a new atom group.
		/// </summary>
		/// <param name="logicalOperator">The operator that characterizes the relationship between the atoms and atoms group.</param>
		/// <param name="members">An array containing atoms and atom groups.</param>
		/// <param name="runningMembers">An array containing atoms and atom groups that will actually be run (they can be different
		/// from the members because of atom equivalence).</param>
		internal AtomGroup(LogicalOperator logicalOperator, object[] members, object[] runningMembers) {
			this.logicalOperator = logicalOperator;
			this.members = members;
			
			HashCodeBuilder hcb = new HashCodeBuilder();
			hcb.Append(logicalOperator);
			SortedList<int, object> sortedMembers = new SortedList<int, object>(Comparer<int>.Default);
			
			// check the members, compute hashcode and build sorted members list
			for(int i=0; i < members.Length; i++) {
				object member =members[i];
				
				if (member == null) {
					throw new BREException("An atom group can not contain a null member");
				}
				else if (member is AtomGroup) {
					if (((AtomGroup)member).logicalOperator == logicalOperator)
						throw new BREException("An atom group can not contain another group with the same logical operator");
				}
				else if (member is Atom) {
					if (((Atom)member).HasFormula)
						throw new BREException("An atom group can not contain an atom that contains a formula");
				}
				else {
					throw new BREException("An atom group can not hold objects of type: " + member.GetType());
				}
				
				hcb.Append(member);
				
				if (runningMembers == null) sortedMembers.Add(GetMemberSortedIndex(members, i), member);
			}
			
			hashCode = hcb.Value;
			
			// the members actually used when processing the atom group are not the ones defined in the rule file (usually because of equivalent atoms definitions)
			if (runningMembers != null) {
				for(int i=0; i < runningMembers.Length; i++) sortedMembers.Add(GetMemberSortedIndex(runningMembers, i), runningMembers[i]);
			}
			
			orderedMembers = sortedMembers.Values;
			
			allAtoms = new List<Atom>();
			foreach(object member in orderedMembers) {
				if (member is Atom) allAtoms.Add((Atom)member);
				else if (member is AtomGroup) allAtoms.AddRange(((AtomGroup)member).AllAtoms);
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
			return hashCode;
		}
		
		/// <summary>
		/// Returns the String representation of the AtomGroup for display purpose only.
		/// </summary>
		/// <returns>The String representation of the AtomGroup.</returns>
		public override string ToString() {
			return Operator +
						 "(\n" +
						 Misc.IListToString(new ArrayList(members), "  ") +
						 ")";
		}
	}
}
