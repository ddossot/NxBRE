namespace org.nxbre.ie {
	using System;
	using System.Collections;

	using net.ideaity.util.events;

	using org.nxbre.ie.core;
	using org.nxbre.ie.adapters;
	using org.nxbre.ie.predicates;
	using org.nxbre.ie.rule;
	
	using org.nxbre.util;

	/// <summary>
	/// The available types of working memory.
	/// </summary>
	public enum WorkingMemoryTypes {
		/// <summary>
		/// The global working memory that contains common facts.
		/// </summary>
		Global=1,
		/// <summary>
		/// An isolated working memory that contains a copy of the global facts plus facts locally asserted or deducted.
		/// </summary>
		Isolated=2,
		/// <summary>
		/// An isolated working memory that contains only facts locally asserted or deducted.
		/// </summary>
		IsolatedEmpty=3
	};

	/// <summary>
	/// The available types of working memory threading model.
	/// </summary>
	public enum ThreadingModelTypes {
		/// <summary>
		/// Threading model that is suitable for <b>single threaded applications only</b>.
		/// </summary>
		Single=1,
		/// <summary>
		/// Threading model that is suitable for multi-threaded applications, but does not support rulebase reloading.
		/// </summary>
		Multi=2,
		/// <summary>
		/// Threading model that is suitable for multi-threaded applications and supports rulebase reloading (hot-swapping).
		/// </summary>
		MultiHotSwap=3
	};
	
	/// <summary>
	/// The available types for fact base storage.
	/// </summary>
	public enum FactBaseStorageTypes {
		/// <summary>
		/// Hashtable is the faster and have been proven collision-safe to 300,000 facts.
		/// </summary>
		Hashtable,
		/// <summary>
		/// DataTable is the slowest but is fully collision-safe.
		/// </summary>
		DataTable
	};

	/// <summary>
	/// The Inference Engine of NxBRE.
	/// </summary>
	/// <remarks>
	/// Except for asserting/retracting facts and running queries, this implementation does
	/// not offer an API for the Rule Base. The rule base source is the Adapter, this is where
	/// customization should take place for feeding the engine with rules from other sources than
	/// RuleML files.
	/// </remarks>
	/// <version>2.4</version>
	public sealed class IEImpl:AbstractLogDispatcher, IInferenceEngine {
		
		/// <summary>
		/// The event to subscribe in order to be notified of assertion of facts.
		/// </summary>
		public event NewFactEvent NewFactHandler;
		
		/// <summary>
		/// The event to subscribe in order to be notified of retraction of facts.
		/// </summary>
		public event NewFactEvent DeleteFactHandler;
		
		/// <summary>
		/// The event to subscribe in order to be notified of modification of facts.
		/// </summary>
		public event NewFactEvent ModifyFactHandler;
		
		private ArrayList equivalents;
		private ArrayList integrityQueries;
		private QueryBase qb;
		private ImplicationBase ib;
		private IWorkingMemory wm;
		private MutexManager mm;
		private PreconditionManager pm;
		private IBinder bob;
		private bool initialized;
		
		private	int iteration;
		private string direction;
		private string label;

		private static int iterationLimit = 1000;
		private static bool strictImplication = false;
		private static int lockTimeOut = 15000;
		private static FactBaseStorageTypes factBaseStorageType = FactBaseStorageTypes.Hashtable;
		
		private ArrayList Equivalents {
			get {
				return equivalents;
			}
		}
		
		private ArrayList IntegrityQueries {
			get {
				return integrityQueries;
			}
		}
		
		private QueryBase QB {
			get {
				return qb;
			}
		}
		
		private ImplicationBase IB {
			get {
				return ib;
			}
		}
		
		private IWorkingMemory WM {
			get {
				return wm;
			}
		}
		
		/// <summary>
		/// The optional business object binder (null if none).
		/// </summary>
		private IBinder Binder {
			get {
				return bob;
			}
			set {
				if ((value != null) && (value != bob)) {
					bob = value;			
					
					bob.IEF = new IEFacade(this);
					NewFactHandler += bob.OnNewFact;
					DeleteFactHandler += bob.OnDeleteFact;
					ModifyFactHandler += bob.OnModifyFact;
					
					if (HasLogListener) ForceDispatchLog("NxBRE Flow Engine Binder Initialized", LogEventImpl.INFO);
				}
				else if (value == null)
					bob = null;
			}
		}
		
		/// <summary>
		/// The optional business object binder type (null if none).
		/// </summary>
		public string BinderType {
			get {
				if (Binder != null) return Enum.GetName(typeof(BindingTypes), Binder.BindingType);
				else return null;
			}
		}

		/// <summary>
		/// The direction of the loaded rule base.
		/// </summary>
		public string Direction {
			get {
				return direction;
			}
		}
		
		/// <summary>
		/// The label of the loaded rule base.
		/// </summary>
		public string Label {
			get {
				return label;
			}
		}
		
		/// <summary>
		/// The maximum number of iteration to perform in one process cycle.
		/// If this limit is reached, the engine will throw an exception.
		/// </summary>
		/// <remarks>Default value is 1000.</remarks>
		public static int IterationLimit {
			get {
				return iterationLimit;
			}
			set {
				iterationLimit = value;
			}
		}
		
		/// <summary>
		/// The time-out in millisecond for acquiring a lock when hot swapping a rule base
		/// in multi-threaded environments.
		/// </summary>
		/// <remarks>Default value is 15000.</remarks>
		public static int LockTimeOut {
			get {
				return lockTimeOut;
			}
			set {
				lockTimeOut = value;
			}
		}
		
		/// <summary>
		/// Defines whether the engine should throw an exception in case an implication
		/// tries to assert a fact whose variable predicates have not all be resolved
		/// by the data returned by the atoms of the body.
		/// </summary>
		/// <remarks>Default value is false.</remarks>
		public static bool StrictImplication {
			get {
				return strictImplication;
			}
			set {
				strictImplication = value;
			}
		}
		
		/// <summary>
		/// Defines the fact base storage type used by the engine.
		/// </summary>
		/// <remarks>Default value is Hashtable</remarks>
		public static FactBaseStorageTypes FactBaseStorageType {
			get {
				return factBaseStorageType;
			}
			set {
				factBaseStorageType = value;
			}
		}
		
		
		/// <summary>
		/// The current type of working memory.
		/// </summary>
		public WorkingMemoryTypes WorkingMemoryType {
			get {
				return WM.Type;
			}
		}
		
		/// <summary>
		/// Returns true if the engine is initialized with a valid rulebase.
		/// </summary>
		public bool Initialized {
			get {
				return initialized;
			}
		}
		
		/// <summary>
		/// Instantiates a new Inference Engine with single-thread support.
		/// </summary>
		public IEImpl():this(null) {}
		
		/// <summary>
		/// Instantiates a new Inference Engine with the specified threading model.
		/// </summary>
		/// <param name="threadingModelType">The threading model type that the engine must support.</param>
		public IEImpl(ThreadingModelTypes threadingModelType):this(null, threadingModelType) {}

		/// <summary>
		/// Instantiates a new Inference Engine with single-thread support, using a business objects binder for asserting facts and evaluating functions.
		/// </summary>
		/// <param name="businessObjectsBinder">The business object binder that the engine must use.</param>
		public IEImpl(IBinder businessObjectsBinder):this(businessObjectsBinder, ThreadingModelTypes.Single) {}
		
		/// <summary>
		/// Instantiates a new Inference Engine with the specified threading model, using a business objects binder for asserting facts and evaluating functions.
		/// </summary>
		/// <param name="businessObjectsBinder">The business object binder that the engine must use.</param>
		/// <param name="threadingModelType">The threading model type that the engine must support.</param>
		public IEImpl(IBinder businessObjectsBinder, ThreadingModelTypes threadingModelType) {
			initialized = false;
			
			// instantiate a new working memory
			wm = WorkingMemoryFactory.NewWorkingMemory(threadingModelType);
			
			// set the binder
			Binder = businessObjectsBinder;
		}
		
		/// <summary>
		/// Loads a rule base. The working memory is reset (all facts are lost).
		/// </summary>
		/// <param name="adapter">The Adapter used to read the rule base.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <see cref="org.nxbre.ie.adapters.IRuleBaseAdapter"/>
		public void LoadRuleBase(IRuleBaseAdapter adapter) {
			LoadRuleBase(adapter, Binder);
		}
		
		/// <summary>
		/// Loads a rule base. The working memory is reset (all facts are lost).
		/// </summary>
		/// <param name="adapter">The Adapter used to read the rule base.</param>
		/// <param name="businessObjectsBinder">The business object binder that the engine must use.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <see cref="org.nxbre.ie.adapters.IRuleBaseAdapter"/>
		public void LoadRuleBase(IRuleBaseAdapter adapter, IBinder businessObjectsBinder) {
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Rule Base Loading Started, using adapter " + adapter.GetType().FullName,
			            													LogEventImpl.INFO);
			
			using(adapter) {
				// reset the WM
				WM.PrepareInitialization();
				
				// sets the Binder
				Binder = businessObjectsBinder;
				
				// and pass it to the adapter if needed
				if (Binder != null) adapter.Binder = Binder;

				// currently only forward chaining is supported
				direction = adapter.Direction;
				if (direction == "backward")
					throw new BREException("NxBRE does not support backward chaining");
				else if (direction == String.Empty)
					if (HasLogListener) ForceDispatchLog("NxBRE interprets no-direction directive as forward chaining.", LogEventImpl.WARN);
				else if (direction == "bidirectional")
					if (HasLogListener) ForceDispatchLog("NxBRE interprets bidirectional as forward chaining.", LogEventImpl.WARN);
				else if (direction != "forward")
					throw new BREException("NxBRE does not support direction: "+direction);
				
				// sets the label
				label = adapter.Label;
				
				// load the Equivalents and IntegrityQueries if the adapter supports it
				if (adapter is IExtendedRuleBaseAdapter) {
					equivalents = ((IExtendedRuleBaseAdapter)adapter).Equivalents;
					if (HasLogListener) ForceDispatchLog("Loaded " + equivalents.Count + " Equivalents", LogEventImpl.DEBUG);
					
					integrityQueries = ((IExtendedRuleBaseAdapter)adapter).IntegrityQueries;
					foreach(Query integrityQuery in integrityQueries) WM.FB.RegisterAtoms(integrityQuery.AtomGroup.AllAtoms);
				
					if (HasLogListener) ForceDispatchLog("Loaded " + integrityQueries.Count + " IntegrityQueries", LogEventImpl.DEBUG);
				}
				else {
					equivalents = new ArrayList();
					integrityQueries = equivalents;
				}

				// instantiate the implication base and the query base				
				ib = new ImplicationBase();
				qb = new QueryBase();

				// instantiate the related managers
				mm = new MutexManager(IB);
				pm = new PreconditionManager(IB);
				initialized = true;
				
				// load queries
				foreach(Query query in adapter.Queries) {
					QB.Add(query);
					WM.FB.RegisterAtoms(query.AtomGroup.AllAtoms);
				}
				if (HasLogListener) ForceDispatchLog("Loaded " + QB.Count + " Queries", LogEventImpl.DEBUG);
				
				// load implications
				foreach(Implication implication in adapter.Implications) {
					IB.Add(implication);
					int nbRA = WM.FB.RegisterAtoms(implication.AtomGroup.AllAtoms);
					if (HasLogListener) ForceDispatchLog("Registered: " + nbRA + " body atoms", LogEventImpl.DEBUG);
					
					// modifying implication must run searches based on their deduction, so must register the atom
					if (implication.Action == ImplicationAction.Modify) {
						nbRA = WM.FB.RegisterAtoms(implication.Deduction);
						if (HasLogListener) ForceDispatchLog("Registered: " + nbRA + " head atoms", LogEventImpl.DEBUG);
					}
				}
				if (HasLogListener) ForceDispatchLog("Loaded " + IB.Count + " Implications\n", LogEventImpl.DEBUG);
				
				// load mutexes
				mm.AnalyzeImplications();
				if (HasLogListener) ForceDispatchLog("Loaded Mutexes\n" + mm.ToString(), LogEventImpl.DEBUG);
				
				// load preconditions
				pm.AnalyzeImplications();
				if (HasLogListener) ForceDispatchLog("Loaded Preconditions\n" + pm.ToString(), LogEventImpl.DEBUG);
				
				// load facts
				foreach(Fact fact in adapter.Facts) Assert(fact);
				if (HasLogListener) ForceDispatchLog("Loaded " + WM.FB.Count + " Facts", LogEventImpl.DEBUG);
				
				// finish the WM init
				WM.FinishInitialization();
				
			} //end: using adapter
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Rule Base Loading Finished", LogEventImpl.INFO);
		}
		
		/// <summary>
		/// Saves the WorkingMemory in a rule base.
		/// </summary>
		/// <param name="adapter">The Adapter used to save the rule base.</param>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <see cref="org.nxbre.ie.adapters.IRuleBaseAdapter"/>
		public void SaveRuleBase(IRuleBaseAdapter adapter) {
			CheckInitialized();
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Rule Base Saving Started, using adapter " + adapter.GetType().FullName,
			            													LogEventImpl.INFO);
			
			using(adapter) {
				// header
				adapter.Direction = Direction;
				adapter.Label = Label;
		
				//queries
				ArrayList queries = new ArrayList();
				foreach(Query query in QB)	queries.Add(query);
				adapter.Queries = queries;

				// implications
				ArrayList implications = new ArrayList();
				foreach(Implication implication in IB) implications.Add(implication);
				adapter.Implications = implications;
				
				// equivalents & integrity queries if supported
				if (adapter is IExtendedRuleBaseAdapter) {
					((IExtendedRuleBaseAdapter)adapter).Equivalents = equivalents;
					((IExtendedRuleBaseAdapter)adapter).IntegrityQueries = integrityQueries;
				}
				
				// facts
				ArrayList facts = new ArrayList();
				foreach(Fact fact in WM.FB)	facts.Add(fact);
				adapter.Facts = facts;
			}
			
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Rule Base Saving Finished", LogEventImpl.INFO);
		}
		
		/// <summary>
		/// Load facts in the current working memory. Current implications, facts and queries
		/// remain unchanged.
		/// </summary>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <param name="adapter">The Adapter used to read the fact base.</param>
		public void LoadFacts(IFactBaseAdapter adapter) {
			CheckInitialized();
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Facts Loading Started, using adapter " + adapter.GetType().FullName,
			            													LogEventImpl.INFO);
			
			using(adapter) {
				// sets the eventual Binder
				if (Binder != null) adapter.Binder = Binder;

				// load facts
				int initialFactsCount = WM.FB.Count;
				foreach(Fact fact in adapter.Facts) Assert(fact);
				if (HasLogListener) ForceDispatchLog("Added " + (WM.FB.Count - initialFactsCount) + " new Facts", LogEventImpl.DEBUG);
				
			} //end: using adapter
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Facts Loading Finished", LogEventImpl.INFO);
		}

		/// <summary>
		/// Save facts of the current working memory. Current implications, facts and queries
		/// remain unchanged.
		/// </summary>
		/// <remarks>
		/// The adapter will be disposed at the end of the method's execution.
		/// </remarks>
		/// <param name="adapter">The Adapter used to save the fact base.</param>
		public void SaveFacts(IFactBaseAdapter adapter) {
			CheckInitialized();
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Facts Saving Started, using adapter " + adapter.GetType().FullName,
			            													LogEventImpl.INFO);
			using(adapter) {
				// header
				adapter.Direction = Direction;
				adapter.Label = Label;
		
				// facts
				ArrayList facts = new ArrayList();
				foreach(Fact fact in WM.FB)	facts.Add(fact);
				adapter.Facts = facts;
			} //end: using adapter
			
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Facts Saving Finished", LogEventImpl.INFO);
		}

		/// <summary>
		/// Sets the WorkingMemory of the engine, either by forking the existing Global memory
		/// to a new Isolated one, or by simply using the Global one.
		/// </summary>
		/// <param name="memoryType">The new type of working memory.</param>
		public void NewWorkingMemory(WorkingMemoryTypes memoryType) {
			CheckInitialized();
			WM.Type = memoryType;
		}
		
		/// <summary>
		/// Makes the current isolated memory the new global memory and sets the working memory
		/// type to global. Throws an exception in the current memory type is not isolated.
		/// </summary>
		public void CommitIsolatedMemory() {
			CheckInitialized();
			WM.CommitIsolated();
		}
		
		/// <summary>
		/// Dispose the current isolated memory sets the working memory type to global.
		/// Throws an exception in the current memory type is not isolated.
		/// </summary>
		public void DisposeIsolatedMemory() {
			CheckInitialized();
			WM.DisposeIsolated();
		}
		
		/// <summary>
		/// Performs all the possible deductions on the current working memory and stops
		/// infering when no new Fact is deducted.
		/// </summary>
		public void Process() {
			Process(null);
		}
		
		/// <summary>
		/// If businessObjects is Null, this method performs the same operation as the parameterless
		/// method ; else uses the binder provided in the constructor to perform fact assertions and
		/// orchestrate the process.
		/// If businessObjects is not Null and no binder has been provided in the constructor, throws
		/// a BREException.
		/// </summary>
		/// <param name="businessObjects">An Hashtable of business objects, or Null.</param>
		public void Process(Hashtable businessObjects) {
			CheckInitialized();
			
			iteration = 0;

			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Processing Started", LogEventImpl.INFO);
			
			if (businessObjects == null)
				InferUntilNoNewFact(new ArrayList());
			
			else if (Binder == null)
				throw new BREException("NxBRE Inference Engine needs a Binder to process business objects");
			
			else if (Binder.BindingType == BindingTypes.BeforeAfter) {
				long iniTime = DateTime.Now.Ticks;
				Binder.BusinessObjects = businessObjects;
				Binder.BeforeProcess();
				if (HasLogListener) ForceDispatchLog("NxBRE Binder 'BeforeProcess' Done in " +
																            (long)(DateTime.Now.Ticks - iniTime)/10000 +
															            	" milliseconds", LogEventImpl.INFO);
				
				bool binderIterate = true;
				ArrayList positiveImplications = new ArrayList();
				
				while(binderIterate) {
					binderIterate = false;
					
					InferUntilNoNewFact(positiveImplications);

					WM.FB.ModifiedFlag = false;
					iniTime = DateTime.Now.Ticks;
					Binder.AfterProcess();
					binderIterate = WM.FB.ModifiedFlag;			

					if (HasLogListener) ForceDispatchLog("NxBRE Binder 'AfterProcess' Done in " +
																	            (long)(DateTime.Now.Ticks - iniTime)/10000 +
																            	" milliseconds with " +
																            	(binderIterate?"":"no ") +
																            	"new fact(s) detected",
																            	LogEventImpl.INFO);
				
				} // while binderIterate			
			}
			
			else if (Binder.BindingType == BindingTypes.Control) {
				long iniTime = DateTime.Now.Ticks;
				Binder.BusinessObjects = businessObjects;
				Binder.ControlProcess();
				if (HasLogListener) ForceDispatchLog("NxBRE Binder 'ControlProcess' Done in " +
																            (long)(DateTime.Now.Ticks - iniTime)/10000 +
															            	" milliseconds", LogEventImpl.INFO);
			}
			
			else
				throw new BREException("Unexpected behaviour: BOs=" + 
				                       businessObjects +
				                       " ; Binder=" +
				                       Binder);

			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Processing Finished", LogEventImpl.INFO);			
		}
		
		/// <summary>
		/// Gets the number of implications in the current rulebase.
		/// </summary>
		public int ImplicationsCount {
			get {
				CheckInitialized();				
				return IB.Count;
			}
		}
		
		/// <summary>
		/// Gets the number of facts in the current working memory.
		/// </summary>
		public int FactsCount {
			get {
				CheckInitialized();
				return WM.FB.Count;
			}
		}
		
		/// <summary>
		/// Gets an enumeration of the facts contained in the working memory.
		/// </summary>
		/// <returns>An IEnumerator on the facts contained in the working memory.</returns>
		/// <remarks>Do not alter the facts from this enumemration: use retract and modify instead.</remarks>
		public IEnumerator Facts {
			get {
				CheckInitialized();
				return WM.FB.GetEnumerator();
			}
		}
		
		/// <summary>
		/// Returns true if a Fact exists in the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to check existence.</param>
		/// <returns>True if the Fact exists.</returns>
		public bool FactExists(Fact fact) {
			CheckInitialized();
			return WM.FB.Exists(fact);
		}
		
		/// <summary>
		/// Returns true if a Fact exists in the current working memory.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to check existence.</param>
		/// <returns>True if the Fact exists.</returns>
		public bool FactExists(string factLabel) {
			return (GetFact(factLabel) != null);
		}
		
		/// <summary>
		/// Returns a Fact from its label if it exists, else returns null.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to retrieve.</param>
		/// <returns>The Fact that matches the label if it exists, otherwise null.</returns>
		public Fact GetFact(string factLabel) {
			CheckInitialized();
			return WM.FB.GetFact(factLabel);
		}
		
		/// <summary>
		/// Asserts (adds) a Fact in the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to assert.</param>
		/// <returns>True if the Fact was added to the Fact Base, i.e. if it was really new!</returns>
		public bool Assert(Fact fact) {
			CheckInitialized();
			return WM.FB.Assert(fact);
		}
				
		/// <summary>
		/// Retracts (removes) a Fact from the current working memory.
		/// </summary>
		/// <param name="factLabel">The label of the Fact to retract.</param>
		/// <returns>True if the Fact has been retracted from the FactBase, otherwise False.</returns>
		public bool Retract(string factLabel) {
			Fact fact = GetFact(factLabel);
			if (fact != null) return WM.FB.Retract(fact);
			else return false;
		}
		
		/// <summary>
		/// Retracts (removes) a Fact from the current working memory.
		/// </summary>
		/// <param name="fact">The Fact to retract.</param>
		/// <returns>True if the Fact has been retracted from the FactBase, otherwise False.</returns>
		public bool Retract(Fact fact) {
			CheckInitialized();
			return WM.FB.Retract(fact);
		}
		
		/// <summary>
		/// Modify a Fact by Retracting it and Asserting the replacement one.
		/// If the new Fact has no label (null or Empty), then the Label of the existing fact is kept.
		/// </summary>
		/// <param name="currentFact">The Fact to modify.</param>
		/// <param name="newFact">The Fact to modify to.</param>
		/// <returns>True if <term>currentFact</term> has been retracted from the FactBase, otherwise False ; this whether <term>newFact</term> already exists in the factbase, or not.</returns>
		public bool Modify(Fact currentFact, Fact newFact) {
			CheckInitialized();
			return WM.FB.Modify(currentFact, newFact);
		}
		
		/// <summary>
		/// Modify a Fact by Retracting it and Asserting the replacement one.
		/// If the new Fact has no label (null or Empty), then the Label of the existing fact is kept.
		/// </summary>
		/// <param name="currentFactLabel">The label of the Fact to modify.</param>
		/// <param name="newFact">The Fact to modify to.</param>
		/// <returns>True if <term>currentFact</term> has been retracted from the FactBase, otherwise False ; this whether <term>newFact</term> already exists in the factbase, or not.</returns>
		public bool Modify(string currentFactLabel, Fact newFact) {
			Fact currentFact = GetFact(currentFactLabel);
			if (currentFact != null) return WM.FB.Modify(currentFact, newFact);
			else return false;
		}

		/// <summary>
		/// Gets the number of queries in the current rulebase.
		/// </summary>
		public int QueriesCount {
			get {
				CheckInitialized();				
				return QB.Count;
			}
		}
		
		/// <summary>
		/// Gets the labels of the queries in the current rulebase.
		/// </summary>
		public string[] QueryLabels {
			get {
				CheckInitialized();				
				string[] result = new string[QueriesCount];
				int i = 0;
				foreach(Query query in QB) result[i++] = query.Label;
				return result;
			}
		}
		
		/// <summary>
		/// Runs a new Query in the current working memory.
		/// </summary>
		/// <remarks>
		/// For performance reasons, it is recommended to declare all queries in the rule base
		/// and to use RunQuery(queryLabel)
		/// </remarks>
		/// <param name="query">The new Query to run.</param>
		/// <returns>A QueryResultSet containing the results found.</returns>
		/// <see cref="org.nxbre.ie.rule.QueryResultSet"/>
		public QueryResultSet RunQuery(Query query) {
			return RunQuery(query, true);
		}
		
		/// <summary>
		/// Runs a Query in the current working memory.
		/// </summary>
		/// <param name="queryIndex">The query base index of the Query to run.</param>
		/// <returns>A QueryResultSet containing the results found.</returns>
		/// <see cref="org.nxbre.ie.rule.QueryResultSet"/>
		/// <remarks>It is recommanded to use labelled queries.</remarks>
		public QueryResultSet RunQuery(int queryIndex) {
			return RunQuery(QB.Get(queryIndex), false);
		}
		
		/// <summary>
		/// Runs a Query in the current working memory.
		/// </summary>
		/// <param name="queryLabel">The label of the Query to run.</param>
		/// <returns>A QueryResultSet containing the results found.</returns>
		/// <see cref="org.nxbre.ie.rule.QueryResultSet"/>
		public QueryResultSet RunQuery(string queryLabel) {
			return RunQuery(QB.Get(queryLabel), false);
		}

		// Private methods ---------------------------------------------------------

		private void CheckInitialized() {	
			if (!Initialized)
				throw new BREException("The inference engine is not yet initialized and can not perform this operation.");
		}
		
		private void InferUntilNoNewFact(ArrayList positiveImplications) {
			long iniTime;
			
			if (HasLogListener) ForceDispatchLog("(Starting) " +
															            ((WM.Type == WorkingMemoryTypes.Global)?"Global":"Isolated") +
															            "Working Memory contains: " +
															            WM.FB.Count + " facts, " +
															            IB.Count + " implications, " +
															            QB.Count + " queries.",
															            LogEventImpl.DEBUG);
			
			iniTime = DateTime.Now.Ticks;
			ArrayList iterationPositiveImplications = null;
			bool iterate = true;
			Agenda agenda = new Agenda();

			// Loop as long as there are new deduction made
			while(iterate) {
				if (iteration >= IterationLimit)
					throw new BREException("Maximum limit of iterations reached: " + IterationLimit);
				
				iterate = false;
				iteration++;

				// Schedule all implications matching the existing facts
				agenda.Schedule(iterationPositiveImplications, IB);
				agenda.PrepareExecution();
				
				if (HasLogListener) ForceDispatchLog("Iteration #" +
																	            iteration +
																	            ": " +
																	            agenda.Count + " implications in agenda, with " +
																	            positiveImplications.Count + " positive.",
																	            LogEventImpl.DEBUG);
				
				iterationPositiveImplications = new ArrayList();
					
				while (agenda.HasMoreToExecute) {
					Implication firedImplication = agenda.NextToExecute;
					
					// check if this implication is worth processing:
					// first: see if it is not mutexed by a previously positive implication
					if ((firedImplication.MutexChain != null) &&
					    (!positiveImplications.Contains(firedImplication)) &&
							(Misc.AreIntersecting(firedImplication.MutexChain, positiveImplications))) {
						if (HasLogListener) ForceDispatchLog("Mutexed: "+firedImplication.Label, LogEventImpl.DEBUG);
						firedImplication = null;
					}
					
					// second: see if it is not pre-condition disabled by a previously negative implication
					if ((firedImplication != null) &&
					    (firedImplication.PreconditionImplication != null) &&
					    (!positiveImplications.Contains(firedImplication.PreconditionImplication))) {
						if (HasLogListener) ForceDispatchLog("Negative Precondition: "+firedImplication.Label, LogEventImpl.DEBUG);
						firedImplication = null;
					}
					
					if (firedImplication != null) {
						WM.FB.ModifiedFlag = false;

						int resultsCount = RunImplication(firedImplication);
						
						if (HasLogListener) ForceDispatchLog("Fired Implication: " +
																			            firedImplication.ToString() +
																			            " returned: " + 
																			            resultsCount,
																			            LogEventImpl.DEBUG);
						
						// if processor has been positive, i.e an implication deducted at least one fact
						if (resultsCount > 0) positiveImplications.Add(firedImplication);
						
						// if the fact base has been anyhow modified
						if (WM.FB.ModifiedFlag) iterationPositiveImplications.Add(firedImplication);
					}
					
				} // while agenda has more
				
				// if at least one is accepted ask for another processing iteration
				iterate = (iterationPositiveImplications.Count > 0);
				

			} // while iterate
			
			// perform integrity checks
			foreach(Query integrityQuery in IntegrityQueries) {
				QueryResultSet qrs = RunQuery(integrityQuery);
				if (qrs.Count == 0) throw new IntegrityException("Rulebase integrity violated: " + integrityQuery.Label);
			}
			
			if (HasLogListener) ForceDispatchLog("NxBRE Inference Engine Execution Time: " +
															            (long)(DateTime.Now.Ticks - iniTime)/10000 +
															            " milliseconds", LogEventImpl.INFO);
			
			if (HasLogListener) ForceDispatchLog("(Finishing) " +
																            ((WM.Type == WorkingMemoryTypes.Global)?"Global":"Isolated") +
																            "Working Memory contains: " +
																            WM.FB.Count + " facts, " +
																            IB.Count + " implications, " +
																            QB.Count + " queries.",
																            LogEventImpl.DEBUG);
		}
		
		private QueryResultSet RunQuery(Query query, bool newQuery) {
			CheckInitialized();
			if (query == null) throw new BREException("Query is null or not found.");
			if (newQuery) WM.FB.RegisterAtoms(query.AtomGroup.AllAtoms);
			return new QueryResultSet(WM.FB.RunQuery(query));
		}
		
		private int RunImplication(Implication implication) {
			int implicationResultsCount = 0;
			
			FactBase.ProcessResultSet processResults = WM.FB.ProcessAtomGroup(implication.AtomGroup);

			if (implication.Action == ImplicationAction.Count)
			{
				if (HasLogListener) ForceDispatchLog("Counting Implication '" + implication.Label + "' counted: " + processResults.Count, LogEventImpl.DEBUG);

				bool variableFound = false;
				IPredicate[] members = (IPredicate[])implication.Deduction.Members.Clone();
				for(int i=0; !variableFound && i<members.Length; i++) {
					if (members[i] is Variable) {
						members[i] = new Individual(processResults.Count);
						variableFound = true;
						break;
					}
				}
				
				if ((IEImpl.StrictImplication) && (!variableFound))
					throw new BREException("Strict counting implication rejected the assertion due to lack of variable predicate: " + implication.Deduction);
					
				Fact deductedFact = new Fact(implication.Deduction.Type, members);
				implicationResultsCount++;
				
				// counting implication factbase action
				bool result = WM.FB.Assert(deductedFact);
				if ((result) && (NewFactHandler != null)) NewFactHandler(new NewFactEventArgs(deductedFact));
				if (HasLogListener) ForceDispatchLog((result?"Asserted":"Ignored Assertion of ") + " Fact: " + deductedFact.ToString(), LogEventImpl.DEBUG);
			}
			else if ((implication.Action == ImplicationAction.Assert)
	      		|| (implication.Action == ImplicationAction.Retract))
			{
				// loop on each result and try to build a new fact out of the predicates coming for each result
				foreach(ArrayList processResult in processResults) {
					Fact deductedFact = BuildFact(implication.Deduction, processResult);
						
					if (deductedFact != null) {
						implicationResultsCount++;

						if (implication.Action == ImplicationAction.Retract) {
							// retracting implication factbase action
							bool result = WM.FB.Retract(deductedFact);
							if ((result) && (DeleteFactHandler != null)) DeleteFactHandler(new NewFactEventArgs(deductedFact));
							if (HasLogListener) ForceDispatchLog((result?"Retracted":"Ignored Retraction of ") + " Fact: " + deductedFact.ToString(), LogEventImpl.DEBUG);
						}
						else {
							// asserting implication factbase action
							bool result = WM.FB.Assert(deductedFact);
							if ((result) && (NewFactHandler != null)) NewFactHandler(new NewFactEventArgs(deductedFact));
							if (HasLogListener) ForceDispatchLog((result?"Asserted":"Ignored Assertion of ") + " Fact: " + deductedFact.ToString(), LogEventImpl.DEBUG);
						}
					}
				}
			}
			else if (implication.Action == ImplicationAction.Modify)
			{
			  foreach(ArrayList processResult in processResults) {
				  // look for facts to modify by:
				  //  - resolving variable predicates of the deduction
				  //  - replacing formulas with variables
				  // and performing a search in the fact base
				  Atom modificationTargetLookup = FactBase.BuildQueryFromDeduction(implication.Deduction, processResult);

				  if (HasLogListener) ForceDispatchLog("Modifying Implication '" + implication.Label + "' will target matches of: " + modificationTargetLookup, LogEventImpl.DEBUG);
				  	
				 	foreach(Fact factToModify in FactBase.ExtractFacts(WM.FB.ProcessAtomGroup(new AtomGroup(AtomGroup.LogicalOperator.And, modificationTargetLookup)))) {
					  if (HasLogListener) ForceDispatchLog("-> found target: " + factToModify, LogEventImpl.DEBUG);

					  // for each fact, perform the modification
				  	Fact deductedFact = BuildFact(implication.Deduction,
				  	                              FactBase.EnrichResults(processResult, modificationTargetLookup, factToModify));

					  if (HasLogListener) ForceDispatchLog("-> modified target: " + deductedFact, LogEventImpl.DEBUG);

					  if ((deductedFact != null) && (!factToModify.Equals(deductedFact))) {
							implicationResultsCount++;
							bool result = WM.FB.Modify(factToModify, deductedFact);
							if ((result) && (ModifyFactHandler != null))ModifyFactHandler(new NewFactEventArgs(factToModify, deductedFact));
							if (HasLogListener) ForceDispatchLog((result?"Modified":"Ignored Modification of ") + " Fact: " + factToModify.ToString(), LogEventImpl.DEBUG);
						}
				  }
				}
			}
			else
				throw new BREException("Implication action not supported: " + implication.Action);
			
			return implicationResultsCount;
		}

		private Fact BuildFact(Atom targetAtom, ArrayList processResult) {
			Atom factBuilder = FactBase.Populate(targetAtom, processResult, true);
			
			if (!factBuilder.IsFact) {
				if (IEImpl.StrictImplication)
					throw new BREException("Strict implication rejected the assertion of incompletely resolved fact: " + factBuilder);
			}
			else {
				return new Fact(factBuilder);
			}
			
			return null;
		}
	}
}
