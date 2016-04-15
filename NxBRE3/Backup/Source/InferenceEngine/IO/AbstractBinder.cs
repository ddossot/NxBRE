namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections;
	using System.Text.RegularExpressions;

	using NxBRE.InferenceEngine.Core;
	using NxBRE.InferenceEngine.Rules;

	using NxBRE.Util;
	
	/// <summary>
	/// Provides an abstract implementation of IBinder that implements the parameters
	/// and AnalyzeIndividualPredicate using a regular expression for detecting
	/// functions in Individual predicates.
	/// </summary>
	/// <see cref="NxBRE.InferenceEngine.IO.IBinder">IBinder definition.</see>
	public abstract class AbstractBinder:IBinder {
		[ThreadStatic]
		protected static IDictionary businessObjectsMap;
		
		private BindingTypes bindingType;
		private Regex regexFunction = null;
		private IEFacade ief;
	
		/// <summary>
		/// The Regex used to define if the String content of an Individual represents a Function.
		/// </summary>
		protected Regex RegexFunction {
			get {
				if (regexFunction == null)
					regexFunction = new Regex(Parameter.Get<string>("abstractbinder.function.regex", @"^(?<1>\w+)\x28((?<2>[^,\x28\x29]+),?)*\x29$"));
				return regexFunction;
			}
		}
		
		/// <summary>
		/// Gets or sets the Inference Engine Façade to use in the binder.
		/// </summary>
		public IEFacade IEF {
			get {
				return ief;
			}
			set {
				ief = value;
			}
		}
	
		/// <summary>
		/// Gets or sets the binding type of the binder.
		/// </summary>
		public BindingTypes BindingType {
			get {
				return bindingType;	
			}
			set {
				bindingType = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the IDictionary of Business Objects, where the user is free to store whatever
		/// fits his requirements. The key represents the type of object and the value a business object
		/// or a collection of business objects of this type.
		/// </summary>
		public virtual IDictionary BusinessObjects {
			get {
				return businessObjectsMap;
			}
			set {
				businessObjectsMap = value;
			}
		}
		
		public AbstractBinder(BindingTypes bindingType) {
			BindingType = bindingType;
		}
		
		/// <summary>
		/// Called by the Inference Engine whenever an Individual predicate is found in the
		/// rule base and must be evaluated to determine if it is a function.
		/// </summary>
		/// <param name="individual">The Individual found in the rule base.</param>
		/// <returns>The unchanged Individual if it is not a function, else a Function predicate.</returns>
		public IPredicate AnalyzeIndividualPredicate(Individual individual) {
			if (individual.Value is string) {
				// Match the regular expression pattern against a text string.
				Match m = RegexFunction.Match((string)individual.Value);
				if (m.Success) {
					// Create a function predicate with 
					ArrayList arguments = new ArrayList();
					foreach (Capture c2 in m.Groups[2].Captures)
						arguments.Add(c2.ToString().Trim());

					return new Function(Function.FunctionResolutionType.Binder,
					                    (string)individual.Value,
					                    this,
					                    m.Groups[1].Captures[0].ToString(),
					                    (string[])arguments.ToArray(typeof(string)));
				}		
			}
			
			return individual;
		}
	
		/// <summary>
		/// If BindingType is Control, called by the Inference Engine instead of starting the inference
		/// process. It then belongs to the implementer to assert facts and start the processing.
		/// </summary>
		/// <remarks>
		/// This method is implemented in the Abstract class but does nothing, it is just for convenience
		/// if it is not needed.
		/// </remarks>
		public virtual void ControlProcess() {}

		/// <summary>
		/// Called by the Inference Engine before starting the inference process. This is where
		/// the implementer should assert initial facts based on the business objects.
		/// </summary>
		/// <remarks>
		/// This method is implemented in the Abstract class but does nothing, it is just for convenience
		/// if it is not needed.
		/// </remarks>
		public virtual void BeforeProcess() {}
		
		/// <summary>
		/// Called by the Inference Engine after finishing the inference process. This is where
		/// the implementer can perform results analysis or updates of business objects.
		/// </summary>
		/// <remarks>
		/// This method is implemented in the Abstract class but does nothing, it is just for convenience
		/// if it is not needed.
		/// </remarks>
		public virtual void AfterProcess() {}
		
		/// <summary>
		/// NewFactEvent delegate called by the Inference Engine whenever a new fact is deducted
		/// while infering. This is where the implementer can perform updates of business objects.
		/// </summary>
		/// <remarks>
		/// Return Null if no particular handling of the event is done.
		/// The implementer should refrain from performing any operation on the IEFacade object,
		/// like asserting new facts.
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.NewFactEventArgs">Definition of NewFactEventArgs.</see>
		public virtual NewFactEvent OnNewFact {
			get {
				// default: no handler
				return null;
			}
		}
		
		/// <summary>
		/// NewFactEvent delegate called by the Inference Engine whenever a fact is deleted
		/// while infering. This is where the implementer can perform updates of business objects.
		/// </summary>
		/// <remarks>
		/// Return Null if no particular handling of the event is done.
		/// The implementer should refrain from performing any operation on the IEFacade object,
		/// like asserting new facts.
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.NewFactEventArgs">Definition of NewFactEventArgs.</see>
	  public virtual NewFactEvent OnDeleteFact {
			get {
				// default: no handler
				return null;
			}
	  }

		/// <summary>
		/// NewFactEvent delegate called by the Inference Engine whenever a fact is modifed
		/// while infering. This is where the implementer can perform updates of business objects.
		/// </summary>
		/// <remarks>
		/// Return Null if no particular handling of the event is done.
		/// The implementer should refrain from performing any operation on the IEFacade object,
		/// like asserting new facts.
		/// </remarks>
		/// <see cref="NxBRE.InferenceEngine.NewFactEventArgs">Definition of NewFactEventArgs.</see>
	  public virtual NewFactEvent OnModifyFact {
			get {
				// default: no handler
				return null;
			}
	  }

		/// <summary>
		/// Called by the Inference Engine whenever a Function predicate must be evaluated.
		/// </summary>
		/// <param name="predicate">The predicate value to check the function against.</param>
		/// <param name="function">The function name defined in the rule base.</param>
		/// <param name="arguments">The function arguments defined in the rule base.</param>
		/// <returns>True if the predicate value matches the function.</returns>
		public virtual bool Evaluate(object predicate, string function, string[] arguments) {
			// default: no function to evaluate
			return false;
		}

		/// <summary>
		/// Called by the Inference Engine whenever a formula individual must be evaluated.
		/// </summary>
		/// <param name="operationName">The operation name defined in the rule base.</param>
		/// <param name="arguments">The arguments (name/value) used when evaluating the formula.</param>
		/// <returns>An object representing the value of the computed formula.</returns>
		public virtual object Compute(string operationName, IDictionary arguments) {
			// default: returns null
			return null;
		}
		
		/// <summary>
		/// Called by the Inference Engine whenever a Function atom relation must be evaluated.
		/// </summary>
		/// <param name="function">The function name defined in the rule base.</param>
		/// <param name="predicates">The predicates' values acting as function parameters.</param>
		/// <returns>True if the function relation is positive.</returns>
		public virtual bool Relate(string function, object[] predicates) {
			// default: no relation to evaluate
			return false;
		}
		
	}

}
