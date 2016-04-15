namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;

	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Core;
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;
	
	/// <summary>
	/// An AtomFunction represents a function-driven association of predicates. It is immutable.
	/// </summary>
	/// <remarks>This kind of atom does not produce any fact. It validates (or not) a branch
	/// in the pattern-matching result graph.</remarks>
	/// <author>David Dossot</author>
	public sealed class AtomFunction:Atom {
		private readonly IBinder bob;
		private readonly string functionSignature;
		private readonly RelationResolutionType resolutionType;

		/// <summary>
		/// The different types of resolution methods for the atom relation.
		/// </summary>
		public enum RelationResolutionType {None, Binder, NxBRE, Expression};

		/// <summary>
		/// The business object binder to use when evaluating the function.
		/// </summary>
		public IBinder BOB {
			get {
				return bob;
			}
		}
		
		/// <summary>
		/// The resolution type for this function.
		/// </summary>
		public RelationResolutionType ResolutionType {
			get {
				return resolutionType;
			}
		}
		
		/// <summary>
		/// Instantiates a new AtomFunction.
		/// </summary>
		/// <param name="resolutionType">The type of resolution for the atom relation function.</param>
		/// <param name="negative">Negative Atom.</param>
		/// <param name="bob">The business object binder to use when evaluating the function.</param>
		/// <param name="type">The relation type of the Atom.</param>
		/// <param name="members">Array of predicates associated in the Atom.</param>
		public AtomFunction(RelationResolutionType resolutionType,
		                    bool negative,
		                    IBinder bob,
		                    string type,
		                    params IPredicate[] members):this(resolutionType, negative, bob, type, members, null){}
			
		private AtomFunction(RelationResolutionType resolutionType,
		                    bool negative,
		                    IBinder bob,
		                    string type,
		                    IPredicate[] members,
		                    string functionSignature):base(negative, type, members) {
			
			if ((HasFunction) || (HasFormula))
				throw new BREException("Atom with function relation can not have a function or formula has a member: " + ToString());

			this.resolutionType = resolutionType;
			this.bob = bob;
			
			if (resolutionType == RelationResolutionType.Binder) {
				// precalculate the function signature to use in the binder to evaluate the function
				if (functionSignature == null)
					this.functionSignature = Parameter.BuildFunctionSignature(type, members);
				else
					this.functionSignature = functionSignature;
			}
		}
		
		/// <summary>
		/// Private constructor used for cloning.
		/// </summary>
		/// <param name="source">The source AtomFunction to use for building the new one.</param>
		private AtomFunction(AtomFunction source):base(source) {
			this.bob = source.bob;
			this.functionSignature = source.functionSignature;
			this.resolutionType = source.resolutionType;
		}
		
		/// <summary>
		/// Private constructor used for cloning.
		/// </summary>
		/// <param name="source">The source AtomFunction to use for building the new one.</param>
		/// <param name="members">The members to use in the new AtomFunction.</param>
		private AtomFunction(AtomFunction source, IPredicate[] members):base(source, members) {
			this.bob = source.bob;
			this.functionSignature = source.functionSignature;
			this.resolutionType = source.resolutionType;
		}
		
		/// <summary>
		/// Returns a cloned AtomFunction, of same type and containing a clone of the array of predicates.
		/// </summary>
		/// <returns>A new AtomFunction, based on the existing one.</returns>
		/// <remarks>The predicates are not cloned.</remarks>
		public override object Clone() {
			return new AtomFunction(this);
		}
		
		/// <summary>
		/// Performs a clone of the current AtomFunction but substitute members with the provided ones.
		/// </summary>
		/// <param name="members">New members to use.</param>
		/// <returns>A clone with new members.</returns>
		public override Atom CloneWithNewMembers(params IPredicate[] members) {
			return new AtomFunction(this, members);
		}
		
		/// <summary>
		/// Returns true if the atom represents a positive relation, i.e. if the evaluation of the
		/// function, to which the atom's predicates have been passed, has been positive.
		/// </summary>
		public bool PositiveRelation {
			get {
				if (!IsFact)
					throw new BREException("A function relation can not be estimated if any member is a variable: " + ToString());
				
				if (resolutionType == RelationResolutionType.NxBRE) {
					return FlowEngineBinder.EvaluateFERIOperator(Type, PredicateValues);
				}
				else if ((resolutionType == RelationResolutionType.Binder)
				      || (resolutionType == RelationResolutionType.Expression)) {
					try {
						return bob.Relate(functionSignature, PredicateValues);
					} catch(Exception e) {
						throw new BREException("Error when evaluating '"+ functionSignature + "' with predicates: " + Misc.ArrayToString(PredicateValues) ,e);
					}
				}
				else
					throw new BREException("Relation evaluation mode not supported: " + resolutionType);
			}
		}
		
		/// <summary>
		/// Returns the String representation of the Atom for display purpose only.
		/// </summary>
		/// <returns>The String representation of the Atom.</returns>
		public override string ToString() {
			return "function::" + base.ToString();
		}
		
	}

}
