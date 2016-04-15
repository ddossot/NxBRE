namespace NxBRE.InferenceEngine.IO {
	using System.Collections;

	using NxBRE.InferenceEngine.Core;
	using NxBRE.InferenceEngine.Rules;

	/// <summary>
	/// The types of business object binding that determines the inversion of control
	/// calls, either:
	/// - BeforeProcess() and AfterProcess,
	/// or
	/// - ControlProcess().
	/// </summary>
	public enum BindingTypes {
		/// <summary>
		/// A binder that receives control before and after the inference process.
		/// </summary>
		BeforeAfter=1,
		/// <summary>
		/// A binder that receives full control and must start the inference process.
		/// </summary>
		Control=2
	};
		
	/// <summary>
	/// Defines a generalized binder object for connecting the rulebase and the business objects.
	/// It uses the inversion of control principle: the engine calls it when it is the right time.
	/// </summary>
	public interface IBinder {
		
		/// <summary>
		/// Gets or sets the Inference Engine Façade to use in the binder.
		/// </summary>
		IEFacade IEF {
			get;
			set ;
		}
		
		/// <summary>
		/// Gets or sets the binding type of the binder.
		/// </summary>
		BindingTypes BindingType {
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the IDictionary of Business Objects, where the user is free to store whatever
		/// fits his requirements. The key represents the type of object and the value a business object
		/// or a collection of business objects of this type.
		/// </summary>
		IDictionary BusinessObjects {
			get;
			set;
		}
		
		/// <summary>
		/// If BindingType is Control, called by the Inference Engine instead of starting the inference
		/// process. It then belongs to the implementer to assert facts and start the processing.
		/// </summary>
		void ControlProcess();

		/// <summary>
		/// If BindingType is BeforeAfter, 
		/// called by the Inference Engine before starting the inference process. This is where
		/// the implementer should assert initial facts based on the business objects.
		/// </summary>
		void BeforeProcess();

		/// <summary>
		/// If BindingType is BeforeAfter, 
		/// called by the Inference Engine after finishing the inference process. This is where
		/// the implementer can perform results analysis or updates of business objects.
		/// </summary>
		void AfterProcess();
		
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
	  NewFactEvent OnNewFact {
	  	get;
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
	  NewFactEvent OnDeleteFact {
	  	get;
	  }
		
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
	  NewFactEvent OnModifyFact {
	  	get;
	  }

		/// <summary>
		/// Called by the Inference Engine whenever a Function predicate must be evaluated.
		/// </summary>
		/// <param name="predicate">The predicate value to check the function against.</param>
		/// <param name="function">The function name defined in the rule base.</param>
		/// <param name="arguments">The function arguments defined in the rule base.</param>
		/// <returns>True if the predicate value matches the function.</returns>
		bool Evaluate(object predicate, string function, string[] arguments);
		
		/// <summary>
		/// Called by the Inference Engine whenever a Function atom relation must be evaluated.
		/// </summary>
		/// <param name="function">The function name defined in the rule base.</param>
		/// <param name="predicates">The predicates' values acting as function parameters.</param>
		/// <returns>True if the function relation is positive.</returns>
		bool Relate(string function, object[] predicates);
		
		/// <summary>
		/// Called by the Inference Engine whenever a formula individual must be evaluated.
		/// </summary>
		/// <param name="operationName">The operation name defined in the rule base.</param>
		/// <param name="arguments">The arguments (name/value) used when evaluating the formula.</param>
		/// <returns>An object representing the value of the computed formula.</returns>
		object Compute(string operationName, IDictionary arguments);
		
		/// <summary>
		/// Called by the Inference Engine whenever an Individual predicate is found in the
		/// rule base and must be evaluated to determine if it is a function.
		/// </summary>
		/// <param name="individual">The Individual found in the rule base.</param>
		/// <returns>The unchanged Individual if it is not a function, else a Function predicate.</returns>
		IPredicate AnalyzeIndividualPredicate(Individual individual);
	}
}
