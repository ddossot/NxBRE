namespace org.nxbre.ie.adapters {
	using System;
	using System.Collections;
	using System.IO;
	using System.Xml.XPath;
	
	using net.ideaity.util.events;
	
	using org.nxbre.rule;
	using org.nxbre.util;

	using org.nxbre.ie.core;
	using org.nxbre.ie.predicates;

	using org.nxbre.ri;
	using org.nxbre.ri.rule;
	using org.nxbre.ri.drivers;
	using org.nxbre.ri.factories;
	
	/// <summary>
	/// Provides an implementation of IBinder that is based on AbstractBinder
	/// and uses the Flow Engine for asserting facts from the business objects
	/// and evaluating functions.
	/// 
	/// The business rules used for the Flow Engine Binder must be valid with
	/// xBusinessRules.xsd.
	/// </summary>
	/// <remarks>
	/// The business object keys should not contain the reserved keys.
	/// </remarks>
	/// <see cref="org.nxbre.ie.adapters.AbstractBinder"/>
	/// <seealso cref="org.nxbre.IBRE"/>
	public sealed class FlowEngineBinder:AbstractBinder {
		private const string EVALUATE_PREFIX = "Evaluate_";
		private const string RELATE_PREFIX = "Relate_";
		private const string COMPUTE_PREFIX = "Compute_";
		
		private const string ON_NEW_FACT = "OnNewFact";
		private const string ON_DELETE_FACT = "OnDeleteFact";
		private const string ON_MODIFY_FACT = "OnModifyFact";
		private const string CONTROL_PROCESS = "ControlProcess";
		private const string AFTER_PROCESS = "AfterProcess";
		private const string BEFORE_PROCESS = "BeforeProcess";

		public const string INFERENCE_ENGINE_ID = "NxBRE-IE";
		public const string GLOBALWM_ID = "IE-GLOBALWM";
		public const string ISOLATEDWM_ID = "IE-ISOLATEDWM";
		public const string PREDICATE_ID = "IE-PREDICATE";
		public const string NEWFACT_ID = "IE-NEWFACT";
		public const string DELETEDFACT_ID = "IE-DELETEDFACT";
		public const string MODIFIEDFACT_ID = "IE-MODIFIEDFACT";
		public const string MODIFIEDOTHERFACT_ID = "IE-MODIFIEDOTHERFACT";
		public const string ARGUMENTS_ID = "IE-ARGUMENTS";
		public const string RESULT_ID = "IE-RESULT";
		public const string TYPEOF_PARAMETER_ID = "TYPEOF_PARAMETER";
		
		private string[] reservedIds = new string[] {INFERENCE_ENGINE_ID,
																									GLOBALWM_ID,
																									ISOLATEDWM_ID,
																									PREDICATE_ID,
																									NEWFACT_ID,
																									DELETEDFACT_ID,
																									RESULT_ID};
		private IFlowEngine bre;
		
		[ThreadStatic]
		private static IFlowEngine prepared_bre;
		
		[ThreadStatic]
		private static IFlowEngine working_bre;
		
		/// <summary>
		/// Gets or sets the hashtable of Business Objects, where the user is free to store whatever
		/// fits his requirements. The key represents the type of object and the value a business object
		/// or a collection of business objects of this type.
		/// </summary>
		public override Hashtable BusinessObjects {
			get {
				return businessObjectsMap;
			}
			set {
				businessObjectsMap = value;
				
				if (businessObjectsMap != null)
					foreach(string reservedId in reservedIds)
						if (businessObjectsMap.Contains(reservedId))
							throw new BREException("Business Object key '" +
							                       reservedId +
							                       "' conflicts with a reserved key of the Flow Engine Binder.");
				
				PrepareBRE();
			}
		}
		
		/// <summary>
		/// Instantiates a new FlowEngineBinder, using a stream for reading the business rules.
		/// </summary>
		/// <remarks>
		/// The business rules used for the binding must be valid with xBusinessRules.xsd.
		/// </remarks>
		/// <param name="stream">The Stream used to read the business rules from.</param>
		public FlowEngineBinder(Stream stream, BindingTypes bindingType):base(bindingType) {
			Init(new XBusinessRulesStreamDriver(stream));
		}
		
		/// <summary>
		/// Instantiates a new FlowEngineBinder, reading the business rules from a certain URI.
		/// </summary>
		/// <remarks>
		/// The business rules used for the binding must be valid with xBusinessRules.xsd.
		/// </remarks>
		/// <param name="uri">The URI from which the business rules must be read.</param>
		public FlowEngineBinder(string uri, BindingTypes bindingType):base(bindingType) {
			Init(new XBusinessRulesFileDriver(uri));
		}
		
		/// <summary>
		/// If BindingType is Control, called by the Inference Engine instead of starting the inference
		/// process. It then belongs to the implementer to assert facts and start the processing.
		/// </summary>
		/// <remarks>
		/// Object available in the Flow Engine context (ID = Description):
		/// NxBRE-IE = The inference engine façade.
		/// IE-GLOBALWM = WorkingMemoryTypes.Global,
		/// IE-ISOLATEDWM = WorkingMemoryTypes.Isolated.
		/// </remarks>
		public override void ControlProcess() {
			Process(CONTROL_PROCESS);
		}

		/// <summary>
		/// Called by the Inference Engine before starting the inference process. This is where
		/// the implementer should assert initial facts based on the business objects.
		/// </summary>
		/// <remarks>
		/// Object available in the Flow Engine context (ID = Description):
		/// NxBRE-IE = The inference engine façade.
		/// IE-GLOBALWM = WorkingMemoryTypes.Global,
		/// IE-ISOLATEDWM = WorkingMemoryTypes.Isolated.
		/// </remarks>
		public override void BeforeProcess() {
			Process(BEFORE_PROCESS);
		}
		
		/// <summary>
		/// Called by the Inference Engine after finishing the inference process. This is where
		/// the implementer can perform results analysis or updates of business objects.
		/// </summary>
		/// <remarks>
		/// Object available in the Flow Engine context (ID = Description):
		/// NxBRE-IE = The inference engine façade,
		/// IE-GLOBALWM = WorkingMemoryTypes.Global,
		/// IE-ISOLATEDWM = WorkingMemoryTypes.Isolated.
		/// </remarks>
		public override void AfterProcess() {
			Process(AFTER_PROCESS);
		}
		
		/// <summary>
		/// Called by the Inference Engine whenever a Function predicate must be evaluated.
		/// </summary>
		/// <remarks>
		/// Objects available in the Flow Engine context (ID = Description):
		/// NxBRE-IE     = The inference engine façade,
		/// Arg0..n      = The arguments passed to the function,
		/// IE-PREDICATE = The predicate to evaluate.
		/// 
		/// Objects that must be placed in the context (ID = Description):
		/// IE-RESULT    = A boolean, True if the evaluation is positive.
		/// </remarks>
		/// <param name="predicate">The predicate value to check the function against.</param>
		/// <param name="function">The function name defined in the rule base.</param>
		/// <param name="arguments">The function arguments defined in the rule base.</param>
		/// <returns>True if the predicate value matches the function.</returns>
		public override bool Evaluate(object predicate, string function, string[] arguments) {
			NewWorkingBRE();
			working_bre.RuleContext.SetObject(PREDICATE_ID, predicate);
			for(int i=0; i<arguments.Length; i++) working_bre.RuleContext.SetObject("Arg" + i, arguments[i]);
			working_bre.RuleContext.SetObject(INFERENCE_ENGINE_ID, IEF);
			working_bre.Process(EVALUATE_PREFIX + function);
			object result = working_bre.RuleContext.GetObject(RESULT_ID);
			if (result != null) return (bool)result;
			else return false;
		}

		/// <summary>
		/// Called by the Inference Engine whenever a Function atom relation must be evaluated.
		/// </summary>
		/// <remarks>
		/// Objects available in the Flow Engine context (ID = Description):
		/// NxBRE-IE     = The inference engine façade,
		/// Arg0..n      = The arguments passed to the function.
		/// 
		/// Objects that must be placed in the context (ID = Description):
		/// IE-RESULT    = A boolean, True if the evaluation is positive.
		/// </remarks>
		/// <param name="function">The function name defined in the rule base.</param>
		/// <param name="predicates">The predicates' values acting as function parameters.</param>
		/// <returns>True if the function relation is positive.</returns>
		public override bool Relate(string function, object[] predicates) {
			NewWorkingBRE();
			for(int i=0; i<predicates.Length; i++) working_bre.RuleContext.SetObject("Arg" + i, predicates[i]);
			working_bre.RuleContext.SetObject(INFERENCE_ENGINE_ID, IEF);
			working_bre.Process(RELATE_PREFIX + function);
			object result = working_bre.RuleContext.GetObject(RESULT_ID);
			if (result != null) return (bool)result;
			else return false;
		}
		
		/// <summary>
		/// Called by the Inference Engine whenever a formula individual must be evaluated.
		/// </summary>
		/// <remarks>
		/// Objects available in the Flow Engine context (ID = Description):
		/// NxBRE-IE     = The inference engine façade,
		/// IE-ARGUMENTS = The IDictionary containing the arguments passed to the function.
		/// TYPEOF_PARAMETER_ID = The key used to extract the (possibily empty) IList of extra arguments.
		/// 
		/// Objects that must be placed in the context (ID = Description):
		/// IE-RESULT    = An object, resulting of the computation.
		/// </remarks>
		/// <param name="operationName">The operation name defined in the rule base.</param>
		/// <param name="arguments">The arguments (name/value) used when evaluating the formula.</param>
		/// <returns>An object representing the value of the computed formula.</returns>
		public override object Compute(string operationName, IDictionary arguments) {
			NewWorkingBRE();
			working_bre.RuleContext.SetObject(ARGUMENTS_ID, arguments);
			working_bre.RuleContext.SetObject(TYPEOF_PARAMETER_ID, typeof(Parameter));
			working_bre.RuleContext.SetObject(INFERENCE_ENGINE_ID, IEF);
			working_bre.Process(COMPUTE_PREFIX + operationName);
			return working_bre.RuleContext.GetObject(RESULT_ID);
		}
		
		/// <summary>
		/// NewFactEvent delegate called by the Inference Engine whenever a fact is deducted
		/// while infering. This is where the implementer can perform updates of business objects.
		/// </summary>
		/// <remarks>
		/// Return Null if no particular handling of the event is done.
		/// The implementer should refrain from performing any operation on the IEFacade object,
		/// like asserting new facts.
		/// 
		/// Object available in the Flow Engine context (ID = Description):
		/// IE-NEWFACT = The new fact, source of the event.
		/// </remarks>
		/// <see cref="org.nxbre.ie.core.NewFactEventArgs">Definition of NewFactEventArgs.</see>
		public override NewFactEvent OnNewFact {
			get {
				if (HasFactEventHandler(ON_NEW_FACT)) return new NewFactEvent(NewFactHandler);
				else return null;
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
		/// 
		/// Object available in the Flow Engine context (ID = Description):
		/// IE-DELETEDFACT = The deleted fact, source of the event.
		/// </remarks>
		/// <see cref="org.nxbre.ie.core.NewFactEventArgs">Definition of NewFactEventArgs.</see>
		public override NewFactEvent OnDeleteFact {
			get {
				if (HasFactEventHandler(ON_DELETE_FACT)) return new NewFactEvent(DeleteFactHandler);
				else return null;
			}
	  }
	  
		/// <summary>
		/// NewFactEvent delegate called by the Inference Engine whenever a new fact is deducted
		/// while infering. This is where the implementer can perform updates of business objects.
		/// </summary>
		/// <remarks>
		/// Return Null if no particular handling of the event is done.
		/// The implementer should refrain from performing any operation on the IEFacade object,
		/// like asserting new facts.
		/// 
		/// Object available in the Flow Engine context (ID = Description):
		/// IE-MODIFIEDFACT = The modified fact.
		/// IE-MODIFIEDOTHERFACT = The new fact after modification.
		/// </remarks>
		/// <see cref="org.nxbre.ie.core.NewFactEventArgs">Definition of NewFactEventArgs.</see>
		public override NewFactEvent OnModifyFact {
			get {
				if (HasFactEventHandler(ON_MODIFY_FACT)) return new NewFactEvent(ModifyFactHandler);
				else return null;
			}
	  }

		public override string ToString() {
	  	return "FlowEngineBinder w/BindingType:" + this.BindingType;
	  }
	  
		// private methods ------------------------------------------------------
		
		private void PrepareBRE() {
			prepared_bre = (IFlowEngine)bre.Clone();
						
			if (businessObjectsMap != null)
				foreach(object key in BusinessObjects.Keys)
					prepared_bre.RuleContext.SetObject(key, BusinessObjects[key]);			
		}
		
		private void NewWorkingBRE() {
			if (prepared_bre == null) PrepareBRE();
			working_bre = (IFlowEngine)prepared_bre.Clone();
		}
		
		private void Init(IRulesDriver driver) {
			bre = new BREFactory(new DispatchException(HandleExceptionEvent)).NewBRE(driver);
			prepared_bre = null;
			
			if (bre == null)
				throw new BREException("The initialization of the Flow Engine Binder failed.");
		}
		
		private void Process(string setId) {
			NewWorkingBRE();
			working_bre.RuleContext.SetObject(INFERENCE_ENGINE_ID, IEF);
			working_bre.RuleContext.SetObject(GLOBALWM_ID, WorkingMemoryTypes.Global);
			working_bre.RuleContext.SetObject(ISOLATEDWM_ID, WorkingMemoryTypes.Isolated);
			working_bre.Process(setId);
		}
	
		private void HandleExceptionEvent(object obj, IExceptionEvent aException) {
			// whatever is the exception hit, the process is stoped and an exception is thrown
			// because the inference engine does not tolerate exceptions.
			working_bre.Stop();
			throw aException.Exception;
		}
		
		private void NewFactHandler(NewFactEventArgs nfea) {
			NewWorkingBRE();
			working_bre.RuleContext.SetObject(NEWFACT_ID, nfea.Fact);
			working_bre.Process(ON_NEW_FACT);			
		}
		
		private void DeleteFactHandler(NewFactEventArgs nfea) {
			NewWorkingBRE();
			working_bre.RuleContext.SetObject(DELETEDFACT_ID, nfea.Fact);
			working_bre.Process(ON_DELETE_FACT);			
		}
		
		private void ModifyFactHandler(NewFactEventArgs nfea) {
			NewWorkingBRE();
			working_bre.RuleContext.SetObject(MODIFIEDFACT_ID, nfea.Fact);
			working_bre.RuleContext.SetObject(MODIFIEDOTHERFACT_ID, nfea.OtherFact);
			working_bre.Process(ON_MODIFY_FACT);			
		}
		
		/// <summary>
		/// Checks if the binder must be called for each fact deducted or deleted while infering.
		/// </summary>
		/// <param name="eventType">The type of fact event.</param>
		/// <returns>True if the FlowEngineBinder should handle this type of fact event.</returns>
		private bool HasFactEventHandler(string eventType) {
			return bre.XmlDocumentRules
								.CreateNavigator()
								.Select("//Set[@id='" + eventType + "']")
								.MoveNext();
		}
		
		private static Hashtable nxbreOperatorCache = Hashtable.Synchronized(new Hashtable());
		
		/// <summary>
		/// Façade for calling an NxBRE Flow Engine RI helper operator.
		/// </summary>
		/// <param name="functionName">The prefixed name of the helper operator to evaluate.</param>
		/// <param name="values">The arguments to pass to the operator.</param>
		/// <returns>True if the value matches the operator.</returns>
		/// <see cref="org.nxbre.ri.helpers.operators.*"/>
		public static bool EvaluateFERIOperator(string functionName, params object[] values) {
			if (values.Length != 2)
				throw new BREException(values.Length +
				                       " is not a valid number of arguments for: " +
				                       functionName +
				                       ", only 2 is supported.");
			
			string operatorType = "org.nxbre.ri.helpers.operators.";
			
			if (functionName.ToLower().StartsWith("nxbre:")) operatorType += functionName.Substring(6).Split(Parameter.PARENTHESIS)[0];
			else operatorType += functionName.Split(Parameter.PARENTHESIS)[0];
			
			IBREOperator operatorObject;
			
			if (!nxbreOperatorCache.ContainsKey(operatorType)) {
				operatorObject = (IBREOperator)Reflection.ClassNew(operatorType, null);
				nxbreOperatorCache.Add(operatorType, operatorObject);
			}
			else
				operatorObject = (IBREOperator)nxbreOperatorCache[operatorType];
			
			ObjectPair pair = new ObjectPair(values[0], values[1]);
			Reflection.CastToStrongType(pair);
			
			return operatorObject.ExecuteComparison(null,
			                                        null,
			                                        pair.First,
			                                        pair.Second);
		}
		
	}
	
}
