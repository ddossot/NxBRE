namespace NxBRE.FlowEngine
{
	using System;
	using System.IO;
	using System.Collections;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.XPath;
	using System.Xml.Xsl;
	
	using net.ideaity.util;
	using net.ideaity.util.events;
	
	using NxBRE.Util;
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.Core;
	using NxBRE.FlowEngine.IO;
	
	/// <summary>The Rule Interpretor implementation of IBRE, the Flow Engine of NxBRE.</summary>
	/// <remarks> Take a deep breath.... Inhale... Exhale... Lets begin:
	/// <P>
	/// BREImpl is a reference implementation of a Business Rule Engine(BRE).
	/// </P>
	/// <P>
	/// <B>10000' view</B><BR/>
	/// This accomplishes its task by traversing (DOM) an XML document that should be passed
	/// in through the init() method.  The XML document MUST adhere to the
	/// businessRules.xsd Schema.  If it does not you may get some odd behaviors from this
	/// class. It then call all the executeRule() methods in the defined factories.
	/// The factories get passed a RuleContext, a Step, and any Parameters that have 
	/// been set.  The RuleContext contains all
	/// the generated results so far as well as other BRE related information.  The Step
	/// is a nullable object that can be used to have one executeRule() execute differently
	/// depending on the "step" that it is on.  The results of the call to executeRule()
	/// are wrapped in a BRERuleResult and added to the RuleContext.
	/// </P>
	/// <P>
	/// It is key to note that this class will not throw ANY exceptions unless they are so
	/// severe that the whole class crashes (99.9% impossible). Instead it listeners.dispatches all
	/// errors to registered listeners who implement ExceptionListener.  This allows multiple
	/// object the ability to handle exceptions.
	/// </P>
	/// <P>
	/// Logging occurs via listeners.dispatching to any registered listener who implements
	/// BRELogListener.  DEBUG as well as WARN, ERROR, and FATAL are created by this class.
	/// </P>
	/// <P>
	/// <B>100' view</B><BR/>
	/// </P>
	/// Lets get to the meat of it shall we?
	/// <P>
	/// The first method executed should be the init() method.
	/// The init() method preloads ALL defined Rule factories 
	/// even if they may never be used. This is to catch any classload 
	/// exceptions as early on as possible to allows the calling class
	/// the ability to fix any issues realtime.  If a rule is defined 
	/// without a factory BREImpl checks to see if it already exists in 
	/// the RuleContext.  This is because RuleContext may be passed in 
	/// and may already be populated.  If it does not find a factory anywhere it
	/// will listeners.dispatch a warning to alert the registered listeners of the issue.
	/// </P>
	/// <P>
	/// The error is only a warning at this point because the process() method
	/// should not have been called yet thus allowing the calling class the 
	/// ability to still set things in the RuleContext.
	/// </P>
	/// <P>
	/// The process method should be called next to actually execute the 
	/// the business rules.  The process method traverses the XML document
	/// making calls to the defined Factories that implement BRERuleFactory.
	/// We still have to make sure that the classes implements BRERuleFactory
	/// because the RuleContext may have changed.  This is a FATAL if this
	/// occurs.
	/// </P>
	/// <P>
	/// Next it takes the results from the executeRule() and wraps it into a
	/// RuleResult type.  
	/// </P>
	/// <P>
	/// If an exception occurs within executeRule() BREImpl will listeners.dispatch an ERROR
	/// to any listening classes and then will take the exception and wrap it with
	/// BRERuleResult.  This allows the developer the ability to define exception
	/// cases within the XML document.  In English, you can throw exceptions on 
	/// purpose and handle them accordingly in the XML doc.
	/// </P>
	/// <P>
	/// The last steps are to add the final RuleResult to the call stack, inform
	/// any listeners implementing BRERuleResultListener that we have a RuleResult,
	/// and add the RuleResult to the RuleContext so that other RuleFactories may
	/// utilize and share the data.
	/// </P>
	/// <P>
	/// <B>Key Notes</B><BR/>
	/// <OL>
	/// <LI>This object does not intentionally throw Exceptions.  If a Error listener is
	/// not registered, you will not know when something blows up!
	/// </LI>
	/// <LI>Errors will only be thrown if the object hits a fatal runtime
	/// error within itself and cannot continue
	/// </LI>
	/// </OL>
	/// </P>
	/// </remarks>
	/// <author>  Sloan Seaman </author>
	/// <author>  David Dossot </author>
	/// <P>
	/// <PRE>
	/// CHANGELOG:
	/// Moved to README
	/// *
	/// </PRE>
	/// </P>
	public sealed class BREImpl : AbstractLogExceptionDispatcher, IFlowEngine
	{
		// XML Related info
		private const string BUSINESS_RULES = "BusinessRules";
		private const string COMPARE = "Compare";
		private const string CONDITION = "Condition";
		private const string DO = "Do";
		private const string ELSEIF = "ElseIf";
		private const string ELSE = "Else";
		private const string FOREACH = "ForEach";
		private const string IF = "If";
		private const string INVOKESET = "InvokeSet";
		private const string LOG = "Log";
		private const string LOGIC = "Logic";
		private const string PARAMETER = "Parameter";
		private const string RETRACT = "Retract";
		private const string RULE = "Rule";
		private const string SET = "Set";
		private const string WHILE = "While";
		
		private sealed class COMPARE_ATTRS
		{
			public const string LEFTID = "leftId";
			public const string RIGHTID = "rightId";
			public const string OPERATOR = "operator";
		}
		
		private sealed class CONDITION_ATTRS
		{
			public const string TYPE = "type";
			public const string AND = "AND";
			public const string OR = "OR";
			public const string NOT = "NOT";
		}
		
		private sealed class FOREACH_ATTRS
		{
			public const string ID = "id";
			public const string RULE_VALUE = "ruleValue";
		}
		
		private sealed class INVOKESET_ATTRS
		{
			public const string ID = "id";
			public const string RULE_VALUE = "ruleValue";
		}		
		
		private sealed class LOG_ATTRS
		{
			public const string MESSAGE = "msg";
			public const string MESSAGE_ID = "msgId";
			public const string LEVEL = "level";
		}
		
		private sealed class PARAMETER_ATTRS
		{
			public const string NAME = "name";
			public const string VALUE = "value";
			public const string TYPE = "type";
			public const string RULE_VALUE = "ruleValue";
		}
		private sealed class RULE_ATTRS
		{
			public const string FACTORY = "factory";
			public const string ID = "id";
			public const string STEP = "step";
		}
		
		private sealed class SET_ATTRS
		{
			public const string ID = "id";
		}
		
		private sealed class RETRACT_ATTRS
		{
			public const string ID = "id";
		}
		
		private bool running = false;
		private bool initialized = false;
		private IBRERuleContext ruleContext = null;
		private XPathDocument xmlDocument = null;
		private IRulesDriver rulesDriver = null;
		
		public event DispatchRuleResult ResultHandlers;
		
		/// <summary> Returns or Sets the RuleContext in it's current state.
		/// <P>
		/// If the developer wishes to have a private copy, make sure
		/// to use Clone().
		/// </P>
		/// <P>
		/// This method allows developers to provide an already populated BRERuleContext.
		/// </P>
		/// <P>
		/// This is provides to allow for RuleFactories that have already created, thus 
		/// allowing a more stateful RuleFactory
		/// </P>
		/// </summary>
		/// <returns> The RuleContext in its current state</returns>
		public IBRERuleContext RuleContext
		{
			get
			{
				return ruleContext;
			}
			
			set
			{
				DispatchLog("RuleContext provided by external entity", LogEventImpl.DEBUG);
				ruleContext = value;
			}
			
		}
		
		/// <summary> Returns the loaded XML Rules in the native NxBRE syntax
		/// </summary>
		/// <returns> The loaded Business Rules</returns>
		public XPathDocument XmlDocumentRules
		{
			get
			{
				return xmlDocument;
			}
		}
			
		/// <summary> Running state of the engine, i.e. when processing.
		/// </summary>
		/// <returns> True if the engine is processing. </returns>
		public bool Running
		{
			get
			{
				return !running;
			}
		}
		
		//FIXME: improve this construct
		private void DispatchRuleResult(IBRERuleResult ruleResult) {
			if (ResultHandlers != null) ResultHandlers(this, ruleResult);
		}
		
		//FIXME: improve this construct
		private DispatchRuleResult GetResultHandlers() {
			return ResultHandlers;
		}
			
		/// <summary> Performs a shallow copy of a pre-initialized BRE, i.e. returns a new BRE
		/// containing a shallow copied rule context, ready to fire!
		/// </summary>
		public object Clone() {
			if (!initialized)
				throw new BREException("Clone in not available if BRE is not initialized.");

			BREImpl newBRE = new BREImpl();
			
			// pass the handlers
			newBRE.LogHandlers += GetLogHandlers();
			newBRE.ExceptionHandlers += GetExceptionHandlers();
			newBRE.ResultHandlers += GetResultHandlers();
			
			// pass a cloned context
			newBRE.RuleContext = (IBRERuleContext)ruleContext.Clone();

			// pass the loaded rule
			newBRE.Init(xmlDocument);
			
			return newBRE;
		}

		/// <summary> Initializes the object.<P>
		/// The default constructor exists so that objects may register
		/// as listeners before any action takes place.
		/// </P>
		/// </summary>
		/// <param name="aObj">The XML configuration document
		/// 									(either System.Xml.XPath.XPathDocument or string poiting to a XML file
		/// </param>
		/// <returns> True if successful, False otherwise
		/// 
		/// </returns>
		public bool Init(object aObj)
		{
			if (running) {
				DispatchException("BRE already running: a violent Stop will be tried!", LogEventImpl.ERROR);
				Stop();
			}
			else DispatchLog("BRE Starting...", LogEventImpl.INFO);
			
			if (aObj == null)
			{
				DispatchException("Business Rules provided by external entity\nObject passed to init() must not be Null", LogEventImpl.FATAL);
				return false;
			}

			if (aObj is IRulesDriver)	{
				rulesDriver = (IRulesDriver) aObj;
				rulesDriver.LogDispatcher = this;
				xmlDocument = null;
			}
			else if (aObj is XPathDocument) xmlDocument = (XPathDocument) aObj;
			else
			{
				DispatchException("Business Rules provided by external entity\nObject passed to init() must be of type System.Xml.XPath.XPathDocument or NxBRE.FlowEngine.IO.IRulesDriver and not "+aObj.GetType(), LogEventImpl.FATAL);
				return false;
			}
			DispatchLog("BRE Initializing...", LogEventImpl.INFO);
			
			if (ruleContext == null)
				ruleContext = new BRERuleContextImpl(new Stack(),
				                                     new Hashtable(),
				                                     new Hashtable(),
				                                     new Hashtable());

			// pre-load all operators
			foreach(Type type in Assembly.GetExecutingAssembly().GetTypes())
				if (null != type.GetInterface(typeof(IBREOperator).FullName, false)) 
						GetOperator(type.FullName);
			
			// pre-load factories
			initialized = LoadFactories(GetXmlDocumentRules().Select("//"+RULE+"[@"+RULE_ATTRS.FACTORY+"]"));
			
			return initialized;
		}
		
		/// <summary>Reset the context's call stack and results
		/// </summary>
		public void Reset() {
			if (running) {
				DispatchException("BRE already running: a violent Stop will be tried!", LogEventImpl.ERROR);
				Stop();
			}
			
			if (ruleContext != null) {
				ruleContext.ResultsMap.Clear();
				ruleContext.CallStack.Clear();
			}
			
			DispatchLog("BRE has been reset.", LogEventImpl.INFO);
		}
		
		/// <summary> Returns either an XPathNavigator containing the rules that was passed to Init(),
		/// or the XmlDocument that the IRuleDriver passed to Init() is set to provide.
		/// </summary>
		private XPathNavigator GetXmlDocumentRules() {
			if ((rulesDriver != null) && (xmlDocument == null)) {
				XmlValidatingReader reader = rulesDriver.GetXmlReader();
				xmlDocument = new XPathDocument(reader);
				
				// this close is very important for freeing the underlying resource, which can be a file
				reader.Close();
			}
			
			if (xmlDocument != null) {
				XPathNodeIterator navDoc = xmlDocument.CreateNavigator().Select(BUSINESS_RULES);
				navDoc.MoveNext();
				return navDoc.Current;
			}
			else throw new BREException("No RulesDriver nor Business Rules available.");
		}

		/// <summary> Execute the BRE.
		/// *
		/// </summary>
		/// <returns> True if successful, False otherwise
		/// 
		/// </returns>
		public bool Process()
		{
			return Process(null);
		}
		
		/// <summary> Execute the BRE but only do a certain set.
		/// *
		/// </summary>
		/// <returns> True if successful, False otherwise
		/// 
		/// </returns>
		public bool Process(object aId)
		{
			if (!initialized)
				throw new BREException("Process in not available if BRE is not initialized.");

			bool wasRunning = running;
			
			// an empty string is of no interest
			if ((aId != null) && ((string)aId == String.Empty)) aId = null;
			
			DispatchLog("BRE Processing"
			            + ((aId == null)?String.Empty:" Set: " + aId.ToString())
			            + ((wasRunning)?" (Re-entrant)":String.Empty),
			            LogEventImpl.INFO);
			
			if (ruleContext == null) {
				DispatchException("RuleContext is null", ExceptionEventImpl.FATAL);
				return false;
			}
			
			if (!running) running = true;

			ProcessXML(GetXmlDocumentRules(), aId, null);
			DispatchLog("BRE Finished"
			            + ((aId == null)?String.Empty:" Set: " + aId.ToString())
			            + ((wasRunning)?" (Re-entrant)":String.Empty),
			            LogEventImpl.INFO);
			
			if (!wasRunning) running=false;
			return true;
		}

		/// <summary> Violently stop the BRE </summary>
		public void Stop() {
			running = false;
		}

		/// <summary> This method preloads all defined factories with the XML document.
		/// This is to catch any errors relating to class loading up front before we
		/// get to the real document parsing and business logic.
		/// <P>
		/// This does not support graceful degradation on purpose.
		/// If I can't get to a business rule, it should be understood
		/// that technically, in the end, the rules fail.
		/// </P>
		/// </summary>
		/// <param name="aNodeList">The List of Nodes to process
		/// </param>
		/// <returns> True if successful, False otherwise
		/// 
		/// </returns>
		private bool LoadFactories(XPathNodeIterator aNodeList)
		{
			DispatchLog("BRE Loading RuleFactories...", LogEventImpl.INFO);
			
			if (aNodeList != null)
			{
				try
				{
					while(aNodeList.MoveNext())
					{
						string factory = aNodeList.Current.GetAttribute(RULE_ATTRS.FACTORY, String.Empty);
						
						if (factory != String.Empty)
						{
							string id = aNodeList.Current.GetAttribute(RULE_ATTRS.ID, String.Empty);
							
							DispatchLog("Found Factory: " + factory + " Id: " + id, LogEventImpl.DEBUG);
							DispatchLog("Loading Factory: " + id, LogEventImpl.DEBUG);
							
							object tmpClass = Assembly.GetExecutingAssembly().CreateInstance(factory);
							
							if (tmpClass is IBRERuleFactory)
							{
								IBRERuleFactory brf = (IBRERuleFactory) tmpClass;
								ruleContext.SetFactory(id, brf);
								DispatchLog("BRE RuleFactory " + id + " loaded and added to RuleContext.", LogEventImpl.DEBUG);
							}
							else
							{
								throw new System.InvalidCastException("Specified Rule Factory " + factory + " with id " + id + " not of type BRERuleFactory");
							}
						}
					}
					return true;
				}
				catch (System.Exception e)
				{
					DispatchException(e, ExceptionEventImpl.FATAL);
				}
			}
			return false;
		}
		
		/// <summary> This method processes the XML Document.
		/// <P>
		/// This is a recursive alg. So be careful if editing it!!!!!!!
		/// </P>
		/// </summary>
		/// <param name="aNode">The Node to process
		/// </param>
		/// <param name="aSetId">The set to process</param>
		/// <param name="aObj">A bland object for anything
		/// </param>
		/// <returns> True if successful, False otherwise
		/// 
		/// </returns>
		private object ProcessXML(XPathNavigator aNode, object aSetId, object aObj)
		{
			if ((aNode == null) || (!running)) return null;
			
			string nodeName = aNode.LocalName;
			DispatchLog("Element Node: " + nodeName, LogEventImpl.DEBUG);

			/*
			A lot of this code is the same but it is broken up for
			scalability reasons...
			*/
			if (nodeName == BUSINESS_RULES) {
				// Instead of parsing all sub nodes perform an xPath pre-selection of nodes
				// depending if a set is selected or not.
				XPathNodeIterator selectedNodes;
				if (aSetId == null)
					selectedNodes = aNode.Select("*[count(ancestor-or-self::"+SET+")=0]");
				else 
					selectedNodes = aNode.Select("*[count(ancestor-or-self::"+SET+")=0] | "+SET+"[@"+SET_ATTRS.ID+"='"+aSetId+"']/*");
				
				while(selectedNodes.MoveNext())
					ProcessXML(selectedNodes.Current, aSetId, aObj);
			}
			else if (nodeName == COMPARE)
			{
				Hashtable map = new Hashtable();
				DoRecursion(aNode, aSetId, map);
				aObj = ProcessCompareNode(aNode, map);
				map = null;
			}
			else if (nodeName == CONDITION)
			{
				aObj = ProcessConditionNode(aNode, aSetId, aObj);
			}
			else if (nodeName == DO)
			{
				DoRecursion(aNode, aSetId, aObj);
			}
			else if (nodeName == ELSE)
			{
				DoRecursion(aNode, aSetId, aObj);
			}
			else if (nodeName == FOREACH)
			{
				string resultToAssert = aNode.GetAttribute(FOREACH_ATTRS.ID, String.Empty);
				string objectToEnumerate = aNode.GetAttribute(FOREACH_ATTRS.RULE_VALUE, String.Empty);
				IEnumerable enumerable = (IEnumerable)ruleContext.GetObject(objectToEnumerate);
				if (enumerable != null)
					foreach(object parser in enumerable)
					{
						ruleContext.SetObject(resultToAssert, parser);
						DoRecursion(aNode, aSetId, aObj);
					}
			}
			else if ((nodeName == IF) || (nodeName == ELSEIF) || (nodeName == WHILE))
			{
				bool exitWhile = false;
				do {
					bool firstChild = true;
					XPathNodeIterator children = aNode.SelectChildren(XPathNodeType.Element);
					while(children.MoveNext())
					{
						if (firstChild) aObj = null;
						// Thanks Sihong & Bernhard
						aObj = ProcessXML(children.Current, aSetId, aObj);
						
						// Only the first child node is considered as test
						// the rest are executed blindely
						if ((firstChild) && (aObj is System.Boolean))
						{
							bool passed = ((System.Boolean) aObj);
							if ((!passed) || (!running)) {
								exitWhile = true;
								break;
							}
						}
						firstChild=false;
					}
				} while ((nodeName == WHILE) && (!exitWhile));
			}
			else if (nodeName== INVOKESET)
			{
				ProcessInvokeSetNode(aNode);
			}
			else if (nodeName == LOG)
			{
				ProcessLogNode(aNode);
			}
			else if (nodeName == LOGIC)
			{
				XPathNodeIterator children = aNode.SelectChildren(XPathNodeType.Element);
				while(children.MoveNext())
				{
					aObj = ProcessXML(children.Current, aSetId, aObj);
					if (aObj is System.Boolean)
					{
						bool passed = ((System.Boolean) aObj);
						if ((passed) || (!running))
							break;
					}
				}
			}
			else if (nodeName == PARAMETER)
			{
				if (aObj is Hashtable)
					ProcessParameterNode(aNode, (Hashtable) aObj);
			}
			else if (nodeName == RETRACT)
			{
				string idToRetract = aNode.GetAttribute(RETRACT_ATTRS.ID, String.Empty);
				if (ruleContext.ResultsMap.Contains(idToRetract))
					ruleContext.ResultsMap.Remove(idToRetract);
			}
			else if (nodeName == RULE)
			{
				Hashtable map = new Hashtable();
				DoRecursion(aNode, aSetId, map);
				ProcessRuleNode(aNode, map);
				map = null;
			}
			else if (nodeName == SET)
			{
				// If I reach a SET node, it has been pre-filtered at document level
				// therefore I process it blindely
				DoRecursion(aNode, aSetId, aObj);
			}

			return aObj;
		}
		
		/// <summary> doRecursive actually does the recursion in terms of callback 
		/// into the algorythm.  It is in it's own method for code reuse
		/// reasons.
		/// <P>
		/// This may be a bit confusing to some because the recursive alg.
		/// (processXML) actually calls out to an external method to do the
		/// calls to go back in. (if that makes sense...)
		/// </P>
		/// </summary>
		/// <param name="aNode">The node to use
		/// </param>
		/// <param name="aSetId">The set to process</param>
		/// <param name="aObj">The generic object to use
		/// </param>
		/// <returns>s The object returned by processXML
		/// 
		/// </returns>
		private object DoRecursion(XPathNavigator aNode, object aSetId, object aObj)
		{
			object o = null;

			if (running) {
				XPathNodeIterator children = aNode.SelectChildren(XPathNodeType.Element);
				while(children.MoveNext())
					o = ProcessXML(children.Current, aSetId, aObj);
			}
			
			return o;
		}
		
		/// <summary> Processes the Condition Node.
		/// <P>
		/// Internally this method is slightly different than the others because it
		/// handles the children internally.  This is to support the OR/AND 
		/// functionality that is required.
		/// </P>
		/// <P>
		/// Verbose:</P><P>
		/// We go through the list of children within a defined BLOCK.
		/// The BLOCK is defined so we can exit out quickly when we hit our
		/// criteria. 
		/// </P>
		/// <P>
		/// We only wish to deal with ELEMENT_NODEs. This is key to note because
		/// children actually contains other types of nodes.
		/// </P>
		/// <P>
		/// If the processState is null (i.e. they did not specifiy AND or OR in
		/// the XML) we default to AND.
		/// </P>
		/// <P>
		/// If it is not null we check for AND or OR. If AND and the call to
		/// processXML() (which would end up calling processCompareNode())
		/// is false, we know the AND fails so we can break out of the loop. 
		/// Otherwise we just increment our TRUE count.  Incrementing the
		/// TRUE count isn't really necessary here, but it is for the OR
		/// condition, and this is a work around.
		/// </P>
		/// <P>
		/// If we get an OR we are only looking for 1 TRUE so we can break
		/// as soon as we hit it.  We increment the TRUE count here as well.
		/// This is so that later, if the TRUE count == 0, we know that the
		/// OR never hit a true statement and should then fail.
		/// REMEBER: we only break if we hit TRUE, the loop will exit nomally
		/// if it doesn't.  And since we are optimistic with returnBool being 
		/// set to TRUE (for the AND stmt) we must set it to false afer the OR
		/// loop finishes if the trueCount = 0.
		/// </P>
		/// </summary>
		/// <param name="aNode">The Node to process
		/// </param>
		/// <param name="aSetId">The set to rule process</param>
		/// <param name="aObj">The object to evaluate</param>
		/// <returns> True if the If stmt passes, False otherwise
		/// 
		/// </returns>
		private System.Boolean ProcessConditionNode(XPathNavigator aNode, object aSetId, object aObj)
		{
			bool returnBool = true;
			bool childrenBool = true;
			string processType = aNode.GetAttribute(CONDITION_ATTRS.TYPE, String.Empty);
			int trueCount = 0;
			
			XPathNodeIterator children = aNode.SelectChildren(XPathNodeType.Element);
			while(children.MoveNext())
			{
				object tmpObj = ProcessXML(children.Current, aSetId, aObj);
				childrenBool = ((Boolean) tmpObj);
				if (tmpObj is Boolean)
				{
					/* If we are doing a NOT, break at first child element */
					if (processType == CONDITION_ATTRS.NOT)
					{
						childrenBool = !childrenBool;
						trueCount++;
						break;
					}
					/* If we are doing an OR, count the number of TRUE, we only need one. */
					else if (processType == CONDITION_ATTRS.OR)
					{
						if (childrenBool)
						{
							trueCount++;
							break;
						}
					}
					/* If we are doing an AND (or anything else that we default to AND)
					 * break out any time we hit a FALSE */
					else
					{
						if (!childrenBool) {
							trueCount = 0;
							break;
						}
						else trueCount++;
					}
				}
			}
			
			if (trueCount == 0) returnBool = false;
			else returnBool = childrenBool;

			return returnBool;
		}
		
		///<summary>Lazy loading of operator.</summary>
		/// <param name="operatorId">Full qualified name of an operator</param>
		/// <returns>An operator object implementing IBREOperator</returns>
		private IBREOperator GetOperator(string operatorId) {
			IBREOperator ruleOperator = null;
			
			if (!ruleContext.OperatorMap.Contains(operatorId)) {
				DispatchLog("Loading Operator: " + operatorId, LogEventImpl.DEBUG);
				ruleOperator = (IBREOperator) Assembly.GetExecutingAssembly().CreateInstance(operatorId);
				ruleContext.SetOperator(operatorId, ruleOperator);
			}
			else
				ruleOperator = ruleContext.GetOperator(operatorId);
			
			return ruleOperator;
		}
		
		/// <summary> This methods processes the Compare nodes that may exist in the XML.
		/// <P>
		/// The best comparison works when the object implements Comparable.
		/// </P>
		/// <P>
		/// If the object does not do so, it eliminates the &lt;,&gt;,&lt;=,&gt;= 
		/// functionality as we are left with .equals(), !.equals(), and
		/// exception
		/// </P>
		/// </summary>
		/// <param name="aNode">The Node to process
		/// </param>
		/// <param name="aMap"/>
		/// <returns> True if the If stmt passes, False otherwise
		/// 
		/// </returns>
		private Boolean ProcessCompareNode(XPathNavigator aNode, Hashtable aMap)
		{
			bool resultBool = false;
				
			// This is required in the XML, so we shouldn't have to worry about nulls....
			string leftId = aNode.GetAttribute(COMPARE_ATTRS.LEFTID, String.Empty);
			string rightId = aNode.GetAttribute(COMPARE_ATTRS.RIGHTID, String.Empty);
			string operatorId = aNode.GetAttribute(COMPARE_ATTRS.OPERATOR, String.Empty);
			
			IBREOperator ruleOperator = GetOperator(operatorId);
			
			if (ruleOperator != null)
			{
				// Get the results
				IBRERuleResult leftResult = (IBRERuleResult) ruleContext.GetResult(leftId);
				IBRERuleResult rightResult = (IBRERuleResult) ruleContext.GetResult(rightId);
	
				// If it does not, consider a null in left or right members as exceptions!
				if ((!ruleOperator.AcceptsNulls) && (leftResult == null))
					DispatchException(new BREException("RuleResult " + leftId + " not found in RuleContext"), ExceptionEventImpl.ERROR);
				else if ((!ruleOperator.AcceptsNulls) && (rightResult == null))
					DispatchException(new BREException("RuleResult " + rightId + " not found in RuleContext"), ExceptionEventImpl.ERROR);
				else
				{
					DispatchLog("Retrieved results for comparison", LogEventImpl.DEBUG);
					
					object left = (leftResult==null)?null:leftResult.Result;
					object right = (rightResult==null)?null:rightResult.Result;
					
					try
					{
						DispatchLog("BREOperator " + operatorId + " executing.", LogEventImpl.DEBUG);
						resultBool = ruleOperator.ExecuteComparison(ruleContext, aMap, left, right);
					}
					catch (System.InvalidCastException)
					{
						DispatchException(new BREException("Specified BREOperator "
						                                   + operatorId
						                                   + " not of type BREOperator or objects being compared are not"
						                                   + " of the same type.\n"
						                                   + "Left Object Name:" + leftId
						                                   + "\nLeft Object Type:" + left.GetType().FullName
						                                   + "\nRight Object Name:" + rightId
						                                   + "\nRight Object Type:" + right.GetType().FullName
						                                   + "\n"),
						                  ExceptionEventImpl.FATAL);
					}
					catch (System.Exception e)
					{
						DispatchException(new BREException(e.ToString()), ExceptionEventImpl.FATAL);
					}
				}
			}
			else
			{
				DispatchException(new BREException("Operator could not be loaded from BRERuleContext"), ExceptionEventImpl.FATAL);
			}
			
			DispatchLog("Compare result: " + resultBool, LogEventImpl.DEBUG);
			return resultBool;
		}
		
		/// <summary> Gets a String Id from either the id attribute or the ruleValue attribute
		/// </summary>
		/// <param name="aNode">The node to process</param>
		/// <param name="idAttribute">The Id of the attribute to process</param>
		/// <param name="valueAttribute">The value used in this node.</param>
		/// <returns> The Id found in the node attributes, null if nothing found</returns>
		private string ProcessIdValueAttributes(XPathNavigator aNode, string idAttribute, string valueAttribute)
		{
			string idNode = aNode.GetAttribute(idAttribute, String.Empty);
			string ruleValueNode = aNode.GetAttribute(valueAttribute, String.Empty);

			if ((idNode == String.Empty) && (ruleValueNode != String.Empty))
			{
				IBRERuleResult result = ruleContext.GetResult(ruleValueNode);
				if (result != null)	return result.Result.ToString();
			}
			else if (idNode != String.Empty) return idNode;
			
			return null;
		}

		/// <summary> Handles the InvokeSet Node
		/// *
		/// </summary>
		/// <param name="aNode">The InvokeSet node to process</param>
		private void ProcessInvokeSetNode(XPathNavigator aNode)
		{
			string id = ProcessIdValueAttributes(aNode, INVOKESET_ATTRS.ID, INVOKESET_ATTRS.RULE_VALUE);
			
			if (id == null)
				DispatchException(new BREException("Can not invoke a set with no Id"),
				                  ExceptionEventImpl.FATAL);
			else
				if (!Process(id))
					DispatchException(new BREException("Error when invoking set "+id),
				 		                ExceptionEventImpl.FATAL);
		}
		
		/// <summary> Handles the Log Node
		/// *
		/// </summary>
		/// <param name="aNode">The Node to process
		/// 
		/// </param>
		private void ProcessLogNode(XPathNavigator aNode)
		{
			string msg = ProcessIdValueAttributes(aNode, LOG_ATTRS.MESSAGE, LOG_ATTRS.MESSAGE_ID);
			int level = Int32.Parse(aNode.GetAttribute(LOG_ATTRS.LEVEL, String.Empty));
			DispatchLog(msg, level);
		}
		
		/// <summary> Handles the Parameter node
		/// *
		/// </summary>
		/// <param name="aNode">The Node to process
		/// </param>
		/// <param name="aMap">The Parameters map
		/// 
		/// </param>
		private void  ProcessParameterNode(XPathNavigator aNode, Hashtable aMap)
		{
			string valueNode = aNode.GetAttribute(PARAMETER_ATTRS.VALUE, String.Empty);
			string typeNode = aNode.GetAttribute(PARAMETER_ATTRS.TYPE, String.Empty);
			string ruleValueNode = aNode.GetAttribute(PARAMETER_ATTRS.RULE_VALUE, String.Empty);
			
			// Used to be initialized with null: changed to String.Empty to solve bug #1190485
			object finalValue = String.Empty;
			
			if (valueNode != String.Empty) {
				if (typeNode != String.Empty) finalValue = Reflection.CastValue(valueNode, typeNode);
				else finalValue = valueNode;
			}
			else if (ruleValueNode != String.Empty)
			{
				IBRERuleResult result = ruleContext.GetResult(ruleValueNode);
				if (result != null)	finalValue = result.Result;
			}
			aMap.Add(aNode.GetAttribute(PARAMETER_ATTRS.NAME, String.Empty), finalValue);
		}
		
		
		/// <summary> Handles the Rule Node and calls doRule()
		/// *
		/// </summary>
		/// <param name="aNode">The Node to process
		/// </param>
		/// <param name="aMap">The Parameters map
		/// 
		/// </param>
		private void  ProcessRuleNode(XPathNavigator aNode, Hashtable aMap)
		{
			DoRule(aNode.GetAttribute(RULE_ATTRS.ID, String.Empty),
			       aNode.GetAttribute(RULE_ATTRS.ID, String.Empty),
			       aMap);
		}
		
		/// <summary> This methods processes the Rule nodes that may exist in the XML.
		/// <P>
		/// It executes as follows (this may not look so straightforward in the code...):
		/// <OL>
		/// <LI>Executes Factories executeRule()</LI>
		/// <LI>takes the result and puts it into a RuleResult object</LI>
		/// <LI>listeners.dispatches an error if it could not find the factory 
		/// (See docs in code)</LI>
		/// <LI>Catches any exceptions from the executeRule() and makes it a 
		/// RuleResult so it can be handled gracefully</LI>
		/// <LI>Adds the RuleResult to the CallStack</LI>
		/// <LI>listeners.dispatched the RuleResult to any listeners</LI>
		/// <LI>Adds the RuleResult to the RuleContext</LI>
		/// </OL>
		/// </P>
		/// *
		/// </summary>
		/// <param name="id">The ID of the Rule
		/// </param>
		/// <param name="step">The current Step
		/// </param>
		/// <param name="aMap">The Parameters map
		/// </param>
		private void DoRule(object id, object step, Hashtable aMap)
		{
			int nextStackLoc = ruleContext.CallStack.Count;
			
			IBRERuleResult ruleResult = null;
			
			try
			{
				IBRERuleFactory factory = ruleContext.GetFactory(id);
				
				/* 
				I have to check for null because if the RuleContext
				was passed in, an external reference exists and can be
				modified at any time
				*/
				
				if (factory != null)
				{
					//setup metadata
					IBRERuleMetaData metaData = new BRERuleMetaDataImpl(id, factory, aMap, nextStackLoc, step);
					
					object result = factory.ExecuteRule(ruleContext, aMap, step);
					
					ruleResult = new BRERuleResultImpl(metaData, result);
				}
				else
				{
					/* 
					A WARN version of this error can occur when the 
					Factories are loaded.  But if the developer passed in
					a RuleContext, they can place the Factory into the 
					RuleContext still.  If we get to this point it is now a full
					blown error because if it is not in the RuleContext at this point and
					it to late and can cause issues 
					*/
					
					DispatchException(new BREException("Factory Id " + id + " defined, but not found in RuleContext"), ExceptionEventImpl.ERROR);
				}
			}
			// This can occur internally in the RuleContext
			catch (System.InvalidCastException cce)
			{
				DispatchException(new BREException("Object in RuleContext not of correct type. " + cce.ToString()), ExceptionEventImpl.FATAL);
			}
			// Catch unknown exceptions in the factory itself
			catch (System.Exception e)
			{
				if (e is BRERuleFatalException)
					DispatchException((BRERuleFatalException) e, ExceptionEventImpl.FATAL);
				else if (e is BRERuleException)
					DispatchException((BRERuleException) e, ExceptionEventImpl.ERROR);
				else
					DispatchException(new BREException("RuleFactory encountered an error. " + e.ToString()), ExceptionEventImpl.ERROR);
				
				/*
				Set the RuleResult to an exception so I can test for it in the If
				Hey, technically it IS what it returned ;)
				The factory can return null here, but that could have caused the 
				exception anyway.....
				*/
				
				IBRERuleMetaData metaData = new BRERuleMetaDataImpl(id, ruleContext.GetFactory(id), aMap, nextStackLoc, step);
				ruleResult = new BRERuleResultImpl(metaData, e);
			}
			
			ruleContext.CallStack.Push(ruleResult);
			
			// call listeners
			DispatchRuleResult(ruleResult);
			
			ruleContext.SetResult(id, ruleResult);
		}
		
	}
}
