namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;
	using System.Text;
	
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Core;
	using NxBRE.Util;
	
	/// <summary>
	/// An Function is a special predicate that represents an evaluation of a function
	/// with the tested predicate passed as first argument then the fixed arguments passed
	/// to the constructor. It is immutable.
	/// </summary>
	/// <author>David Dossot</author>
	public sealed class Function:AbstractPredicate {
		private readonly string[] arguments;
		private readonly IBinder bob;
		private readonly string name;
		private readonly string functionSignature;
		private readonly FunctionResolutionType resolutionType;
		private readonly int hashCode;
	
		/// <summary>
		/// The different types of resolution methods for the function.
		/// </summary>
		public enum FunctionResolutionType {Binder, NxBRE};
		
		/// <summary>
		/// The resolution type used by this function
		/// </summary>
		public FunctionResolutionType ResolutionType {
			get {
				return resolutionType;
			}
		}
		
		/// <summary>
		/// The binder or null if none is used.
		/// </summary>
		internal IBinder Binder {
			get {
				return bob;
			}
		}
		
		/// <summary>
		/// Instantiates a new function predicate.
		/// </summary>
		/// <param name="resolutionType">The type of resolution for the function.</param>
		/// <param name="predicate">The predicate value, i.e. the string representation of the function predicate.</param>
		/// <param name="bob">The business object binder to use when evaluating the function, or null.</param>
		/// <param name="name">The name of the function, as it was analyzed by the binder.</param>
		/// <param name="arguments">The array of arguments of the function, as it was analyzed by the binder.</param>
		public Function(FunctionResolutionType resolutionType, string predicate, IBinder bob, string name, params string[] arguments):base(predicate) {
			this.resolutionType = resolutionType;
			this.bob = bob;
			this.name = name;
			this.arguments = arguments;
			
			HashCodeBuilder hcb = new HashCodeBuilder(base.GetHashCode()).Append(resolutionType).Append(bob).Append(name);
			foreach(string argument in arguments) hcb.Append(argument);
			hashCode = hcb.Value;
			
			// precalculate the function signature to use in the binder to evaluate the function
			functionSignature = Parameter.BuildFunctionSignature(name, arguments);
		}
		
		public override int GetHashCode() {
			return hashCode;
		}
	
		/// <summary>
		/// Returns a clone of the Function.
		/// </summary>
		/// <returns>A clone of the current Individual.</returns>
		public override object Clone() {
			return new Function(resolutionType, (string)Value, bob, name, arguments);
		}
		
		/// <summary>
		/// Checks if an Individual matches the current Function by either calling a NxBRE helper method
		/// or using a binder.
		/// </summary>
		/// <param name="individual">The Individual to check.</param>
		/// <returns>True if the Individual matches the Function.</returns>
		public bool Evaluate(Individual individual) {
			try {
				if (resolutionType == FunctionResolutionType.NxBRE)
					return EvaluateNxBREOperator(individual.Value, name, arguments);
				else if (resolutionType == FunctionResolutionType.Binder)
					return bob.Evaluate(individual.Value, functionSignature, arguments);
				else
					throw new BREException("Function evaluation mode not supported: " + resolutionType);
			}
			catch (Exception ex) {
				// Chuck Cross added try/catch block with addtional info in new thrown exception
				StringBuilder sb = new StringBuilder("Error evaluating formula ")
																			.Append(this)
																			.Append(".\r\n  Arguments:");
				
				foreach(string argument in arguments) 
					sb.Append("   ").Append(argument==null?"Null":argument).Append("\r\n");
				
				throw new BREException(sb.ToString(), ex);
			}
		}
		
		private bool EvaluateNxBREOperator(object individualValue, string operatorName, string[] arguments) {
			if (arguments.Length != 1)
				throw new BREException("Only one argument must be passed to " +
				                       operatorName +
				                       " because the evaluated individual provides the other one.");
			
			return FlowEngineBinder.EvaluateFERIOperator(name, individualValue, arguments[0]);
		}
	}
}
