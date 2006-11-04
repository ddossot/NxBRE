namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections.Generic;

	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;
	
	/// <summary>
	/// The possibile implication actions.
	/// </summary>
	public enum ImplicationAction {Assert, Retract, Count, Modify};
	
	/// <summary>
	/// The remarkable implication priorities. Any value between in the [0-100] is valid.
	/// </summary>
	public enum ImplicationPriority {Minimum=0, Medium=50, Maximum=100};

	/// <summary>
	/// An Implication is a specialization of a Query than is able to make deductions.
	/// </summary>
	/// <description>
	/// The Deduction is an Atom that is used as a template that will be filled with values
	/// provided by facts matching the pattern defined by the atoms of the underlying Query.
	/// </description>
	public sealed class Implication:Query {
		// Immutable members
		private readonly Atom deduction;
		private readonly int priority;
		private readonly ImplicationAction action;
		private readonly string mutex = String.Empty;
		private readonly string precondition = String.Empty;
		private readonly bool hasNaf;

		// Members whose values are estimated by external objects.
		private int salience = 0;
		private IList<Implication> mutexChain = null;
		private Implication preconditionImplication = null;
		
		internal bool HasNaf {
			get {
				return hasNaf;
			}
		}
		
		/// <summary>
		/// The Deduction that this Implication tries to make.
		/// </summary>
		public Atom Deduction {
			get {
				return deduction;
			}
		}	

		/// <summary>
		/// Returns the implication action.
		/// </summary>
		public ImplicationAction Action {
			get {
				return action;
			}
		}
		
		/// <summary>
		/// The Weight of the Implication is a combination of its Priority and Salience
		/// and represents its global priority, used in the Agenda.
		/// </summary>		
		/// <see cref="NxBRE.InferenceEngine.Core.Agenda"/>
		public int Weight {
			get {
				return (1+ Priority) * 100 + Salience;
			}
		}	
		
		/// <summary>
		/// The Priority of the Implication, as defined in the rule base.
		/// </summary>
		public int Priority {
			get {
				return priority;
			}
		}	
		
		/// <summary>
		/// The salience is a computed value the represents the fact that, at equal priority levels,
		/// the implication that is a pre-condition for another one, must be executed before.
		/// This behaviour is enabled by assigning a higher salience to the implications that are higher
		/// in the pre-condition chains.
		/// </summary>
		/// <see cref="NxBRE.InferenceEngine.Core.PreconditionManager"/>
		public int Salience {
			get {
				return salience;
			}
			set {
				if ((value < 0) || (value > 99))
					throw new ArgumentOutOfRangeException("Salience must be in the [0-99] range.");
		
				salience = value;
			}
		}	
		
		/// <summary>
		/// The optional Label of an Implication that mutexes the current one.
		/// </summary>
		public string Mutex {
			get {
				return mutex;
			}
		}

		/// <summary>
		/// The optional MutexChain the current Implication is member of.
		/// </summary>		
		public IList<Implication> MutexChain {
			get {
				return mutexChain;
			}
			set {
				mutexChain = value;
			}
		}
		
		/// <summary>
		/// The optional Label of an Implication whose success preconditions the execution of the current one.
		/// </summary>
		public string Precondition {
			get {
				return precondition;
			}
		}
		
		/// <summary>
		/// The optional reference to the Implication whose success preconditions the execution of the current one.
		/// </summary>		
		public Implication PreconditionImplication {
			get {
				return preconditionImplication;
			}
			set {
				preconditionImplication = value;
			}
		}

		/// <summary>
		/// Returns the String representation of the Implication for display purpose only.
		/// </summary>
		/// <returns>The String representation of the Implication.</returns>
		public override string ToString() {
			string result = "Implication[Action:" + Action +
											";Label:" + Label + 
											";Priority:" + Priority + 
											";Salience:" + Salience + 
											";Mutex:" + Mutex + 
											";Precondition:" + Precondition + "; \n" +
											AtomGroup +
											" -> " +
											Deduction +
											"]";
			
			return result;
		}
	
		/// <summary>
		/// Calculates the hashcode of the current Implication by combining the hashcodes of
		/// its atoms, both in query and deduction.
		/// If the label is present, it becomes the main identifier of the Implication.
		/// </summary>
		/// <returns>The hashcode of the current Query.</returns>
		public override int GetHashCode() {
			if (Label != null) return Label.GetHashCode();
			else return (base.GetHashCode() ^ Deduction.GetHashCode());
		}

		/// <summary>
		/// Instantiates a new Implication.
		/// </summary>
		/// <param name="label">The label of the new implication.</param>
		/// <param name="priority">The priority  of the new implication.</param>
		/// <param name="mutex">String.Empty or the label of an implication mutexed by the new one.</param>
		/// <param name="precondition">String.Empty or the label of an implication that preconditions the new one.</param>
		/// <param name="deduction">The Atom used as a prototype for what this Implication tries to proove.</param>
		/// <param name="atomGroup">The top level group of atoms used in the query part (pattern matching) of the new Implication.</param>
		/// <see cref="NxBRE.InferenceEngine.Rules.ImplicationPriority"/>
		public Implication(string label,
		                   ImplicationPriority priority,
		                   string mutex,
		                   string precondition,
		                   Atom deduction,
		                   AtomGroup atomGroup):this(label,
		                                             (int)priority,
		                                             mutex,
		                                             precondition,
		                                             deduction,
		                                             atomGroup) {}
		

		/// <summary>
		/// Instantiates a new Implication.
		/// </summary>
		/// <param name="label">The label of the new implication.</param>
		/// <param name="priority">The priority  of the new implication.</param>
		/// <param name="mutex">String.Empty or the label of an implication mutexed by the new one.</param>
		/// <param name="precondition">String.Empty or the label of an implication that preconditions the new one.</param>
		/// <param name="deduction">The Atom used as a prototype for what this Implication tries to proove.</param>
		/// <param name="atomGroup">The top level group of atoms used in the query part (pattern matching) of the new Implication.</param>
		/// <param name="action">The implication action.</param>
		/// <see cref="NxBRE.InferenceEngine.Rules.ImplicationAction"/>
		/// <see cref="NxBRE.InferenceEngine.Rules.ImplicationPriority"/>
		public Implication(string label,
		                   ImplicationPriority priority,
		                   string mutex,
		                   string precondition,
		                   Atom deduction,
		                   AtomGroup atomGroup,
		                   ImplicationAction action):this(label,
																											(int)priority,
																											mutex,
																											precondition,
																											deduction,
																											atomGroup,
																											action) {}
		
		/// <summary>
		/// Instantiates a new Implication.
		/// </summary>
		/// <param name="label">The label of the new implication.</param>
		/// <param name="priority">The priority  of the new implication.</param>
		/// <param name="mutex">String.Empty or the label of an implication mutexed by the new one.</param>
		/// <param name="precondition">String.Empty or the label of an implication that preconditions the new one.</param>
		/// <param name="deduction">The Atom used as a prototype for what this Implication tries to proove.</param>
		/// <param name="atomGroup">The top level group of atoms used in the query part (pattern matching) of the new Implication.</param>
		/// <see cref="NxBRE.InferenceEngine.Rules.ImplicationPriority"/>
		public Implication(string label,
		                   int priority,
		                   string mutex,
		                   string precondition,
		                   Atom deduction,
		                   AtomGroup atomGroup):this(label,
		                                             priority,
		                                             mutex,
		                                             precondition,
		                                             deduction,
		                                             atomGroup,
		                                             ImplicationAction.Assert) {}
		
		/// <summary>
		/// Instantiates a new Implication.
		/// </summary>
		/// <param name="label">The label of the new implication.</param>
		/// <param name="priority">The priority  of the new implication.</param>
		/// <param name="mutex">String.Empty or the label of an implication mutexed by the new one.</param>
		/// <param name="precondition">String.Empty or the label of an implication that preconditions the new one.</param>
		/// <param name="deduction">The Atom used as a prototype for what this Implication tries to proove.</param>
		/// <param name="atomGroup">The top level group of atoms used in the query part (pattern matching) of the new Implication.</param>
		/// <param name="action">The implication action.</param>
		/// <see cref="NxBRE.InferenceEngine.Rules.ImplicationAction"/>
		/// <see cref="NxBRE.InferenceEngine.Rules.ImplicationPriority"/>
		public Implication(string label,
		                   int priority,
		                   string mutex,
		                   string precondition,
		                   Atom deduction,
		                   AtomGroup atomGroup,
		                   ImplicationAction action):base(label, atomGroup)
		{
			if (deduction.HasFunction)
				throw new BREException("Can not create Implication with functions in the Deduction: " + deduction.ToString());
		
			if ((mutex != null) && (mutex == label))
				throw new BREException("An Implication can not Mutex itself: " + mutex);
		
			if ((precondition != null) && (precondition == label))
				throw new BREException("An Implication can not Pre-Condition itself: " + precondition);
		
			/* Commented out to solve bug #1469851 : this test was not necessary anymore as the engine can now handle typed data in implications
			foreach(object member in AtomGroup.Members)
				if ((member is Atom) && (((Atom)member).HasNotStringIndividual))
					throw new BREException("Can not create Implication with non-String individuals in the atoms: " + member.ToString());
			*/
			
			if ((priority < (int)ImplicationPriority.Minimum) || (priority > (int)ImplicationPriority.Maximum))
				throw new ArgumentOutOfRangeException("Priority must be in the [" +
				                                      ImplicationPriority.Minimum +
				                                      "-" +
				                                      ImplicationPriority.Maximum +
				                                      "] range.");
			if (action == ImplicationAction.Count) {
				if (deduction.IsFact) throw new BREException("A counting Implication must have one Variable predicate in its deduction atom.");
				int varCount = 0;
				foreach(IPredicate member in deduction.Members) {
					if (member is Variable) varCount++;
					if (varCount > 1) throw new BREException("A counting Implication must have only one Variable predicate in its deduction atom.");
				}
			}

			this.priority = priority;
			this.deduction = deduction;
			this.mutex = mutex;
			this.precondition = precondition;
			this.action = action;
			
			hasNaf = false;
			foreach(Atom atom in AtomGroup.AllAtoms) {
				if (atom.Negative) {
					hasNaf = true;
					break;
				}
			}
			
		}
		
	}
}
