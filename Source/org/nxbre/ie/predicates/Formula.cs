namespace org.nxbre.ie.predicates {
	using System;
	using System.Collections;
	
	using org.nxbre.ie.adapters;
	using org.nxbre.util;
	
	/// <summary>
	/// A Formula is a special predicate that represents an evaluation of a C# expression that produces a
	/// new individual predicate value (unlike Function that simply compare individuals).
	/// It can be used in a deduction atom only. It is immutable.
	/// </summary>
	/// <author>David Dossot</author>
	public sealed class Formula:AbstractPredicate {
		public const string DEFAULT_EXPRESSION_PLACEHOLDER = @"\{(var:|ind|predicate:)(?<1>[^}]*)\}";
		public const string DEFAULT_NUMERIC_ARGUMENT_PATTERN = @"\{predicate:([^}]*)\}";
		
		private readonly string expression;
		private IDictionaryEvaluator evaluator;
		private string functionSignature;
		private readonly FormulaResolutionType resolutionType;
		private readonly IBinder bob;

		/// <summary>
		/// The different types of resolution methods for the function.
		/// </summary>
		public enum FormulaResolutionType {Binder, NxBRE};
		
		/// <summary>
		/// The resolution type used by this formula
		/// </summary>
		public FormulaResolutionType ResolutionType {
			get {
				return resolutionType;
			}
		}
		
		/// <summary>
		/// Instantiates a new Formula predicate.
		/// </summary>
		/// <param name="resolutionType">The type of resolution for the formula.</param>
		/// <param name="bob">The business object binder to use when evaluating the formula, or null.</param>
		/// <param name="expression">The expression value, i.e. the C# code source that should be computable.</param>
		public Formula(FormulaResolutionType resolutionType, IBinder bob, string expression):this(resolutionType, bob, expression, null) {}
		
		/// <summary>
		/// Instantiates a new Formula predicate.
		/// </summary>
		/// <param name="resolutionType">The type of resolution for the formula.</param>
		/// <param name="bob">The business object binder to use when evaluating the formula, or null.</param>
		/// <param name="expression">The expression value, i.e. the C# code source that should be computable.</param>
		/// <param name="evaluator">A precompiled evaluator, or null.</param>
		private Formula(FormulaResolutionType resolutionType, IBinder bob, string expression, IDictionaryEvaluator evaluator):base(expression) {
			this.resolutionType = resolutionType;
			this.bob = bob;
			this.expression = expression;
			this.evaluator = evaluator;
			this.functionSignature = null;
		}

		/// <summary>
		/// Returns a clone of the Formula.
		/// </summary>
		/// <returns>A clone of the current Formula.</returns>
		public override object Clone() {
			return new Formula(resolutionType, bob, expression, evaluator);
		}
		
		/// <summary>
		/// The long hashcode of an Formula should never be used, as an Formula can not be involved in
		/// any kind of comparison.
		/// </summary>
		/// <returns>The long hashcode of the predicate.</returns>
		public override long GetLongHashCode() {
			return 0;
		}
		
		/// <summary>
		/// Evaluate the current Formula with a passed list of arguments.
		/// </summary>
		/// <param name="arguments">The name/value pairs of arguments.</param>
		/// <returns>An object representing the value of the Formula.</returns>
		public object Evaluate(IDictionary arguments) {
			if (resolutionType == FormulaResolutionType.NxBRE) {
				if (evaluator == null) evaluator = Compilation.NewEvaluator(expression,
				                                                            DEFAULT_EXPRESSION_PLACEHOLDER,
				                                                            DEFAULT_NUMERIC_ARGUMENT_PATTERN,
				                                                            arguments);
				return evaluator.Run(arguments);
			}
			else if (resolutionType == FormulaResolutionType.Binder) {
				if (functionSignature == null) functionSignature = Parameter.BuildFunctionSignature(expression, new object[0]);
				return bob.Compute(functionSignature, arguments);
			}
			else
				throw new BREException("Formula evaluation mode not supported: " + resolutionType);

		}
	}
}
