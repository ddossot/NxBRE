namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Xml.XPath;
	using System.Xml.Schema;
	
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	/// <summary>
	/// An abstract RuleML adapter that contains common methods for loading and saving rule bases.
	/// </summary>
	public abstract class AbstractRuleMLAdapter:IRuleBaseAdapter {
		/// <summary>
		/// Structure containing the Implication meta-informations stored in the label.
		/// This is an extension to RuleML, which allows to stay valid with the Schema and is conform
		/// to this remark from ruleml.org about label:
		/// "rule label is a handle for the imp: for various uses, including prioritization".
		/// </summary>
		/// <remarks>
		/// The struct was made public only for unit test purpose.
		/// </remarks>
		internal struct ImplicationProperties 
		{
			public string label;
			public int priority;		
			public string mutex;
			public string precondition;
			public string action;
		
			public ImplicationProperties(string label):
				this(Parameter.GetTaggedInfo(label, "label"),
             Parameter.GetTaggedInfo(label, "priority"),
             Parameter.GetTaggedInfo(label, "mutex"),
             Parameter.GetTaggedInfo(label, "precondition"),
             Parameter.GetTaggedInfo(label, "action"))  {
	
				// if we don't have a label tag
				if (this.label == String.Empty) {
					// then if we have any tagged info, it is an error!!!
					if ((this.priority != -1) ||
					    (this.mutex != String.Empty) ||
					    (this.precondition != String.Empty) ||
					    (this.action != String.Empty))
						throw new BREException("The implication label uses the tagged form but does not contain a label: "+label);
					// else consider the whole label to be...a label (standard way of using RuleML label)
					this.label = label;
				}
	
				// assign medium priority as default
				if (this.priority == -1)
					this.priority = (int)ImplicationPriority.Medium;
			}
			
			public ImplicationProperties(string label,
			                             string priority,
			                             string mutex,
			                             string precondition,
			                             string action):
				this(label, -1, mutex, precondition, action)
			{
				if (priority == String.Empty) this.priority = -1; // this means: someone has to assign me the default value for priority
			  else this.priority = Int32.Parse(priority);
			}
			
			public ImplicationProperties(string label,
			                             int priority,
			                             string mutex,
			                             string precondition,
			                             string action)
			{
			  this.label = label;
				this.priority = priority;
			  this.mutex = mutex;
			  this.precondition = precondition;
			  this.action = action;
			}
			
			public override string ToString() {
				if ((priority != (int)ImplicationPriority.Medium) ||
				    (mutex != String.Empty) ||
				    (precondition != String.Empty) ||
				    (action != String.Empty))
					return "label:" + label + ";" +
								 ((priority != (int)ImplicationPriority.Medium)?"priority:" + priority + ";" : "") +
								 ((mutex != String.Empty)?"mutex:" + mutex + ";" : "") +
								 ((precondition != String.Empty)?"precondition:" + precondition + ";" : "") +
								 ((action != String.Empty)?"action:" + action + ";" : "");
				else
					return label;
			}
		}

		protected enum State {NonInitialized, Read, Write, Disposed};
		
		protected State AdapterState {
			get {
				return adapterState;
			}
			set {
				adapterState = value;
			}
		}
		
		private State adapterState = State.NonInitialized;
		
		private IBinder bob = null;
		
		/// <summary>
		/// Sets the optionally associated IBinder or null.
		/// </summary>
		public IBinder Binder {
			get {
				return bob;
			}
			set {
				bob = value;
			}
		}
		
		/// <summary>
		/// Instantiates a RuleML adapter for reading or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public AbstractRuleMLAdapter(Stream streamRuleML, FileAccess mode) {
			if (streamRuleML == null) throw new ArgumentNullException("A null stream is invalid");
			if ((mode != FileAccess.Read) && (mode != FileAccess.Write)) throw new IOException(mode.ToString() + " not supported");
		}

		/// <summary>
		/// Instantiates a RuleML adapter for reading or writing to an URI.
		/// In Read mode, the adapter tries to locate a flow engine binder definition file 
		/// by appending .xbre to the URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read rules from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public AbstractRuleMLAdapter(string uriRuleML, FileAccess mode) {
			if (uriRuleML == null) throw new ArgumentNullException("A null URI is invalid");
			if ((mode != FileAccess.Read) && (mode != FileAccess.Write)) throw new IOException(mode.ToString() + " not supported");
		}
		
				
		/// <summary>
		/// Called when the adapter is no longer used.
		/// </summary>
		public virtual void Dispose() {
			try {
				if (Reader != null) {
					reader.Close();
					reader = null;
				}
			}
			finally {
				try {
					if (Writer != null) {
						document.WriteTo(Writer);
						writer.Flush();
						writer.Close();
						writer = null;
					}
				}
				finally {
					adapterState = State.Disposed;
				}
			}
		}
		
		
		///<remarks>in case of any failure, this will throw enough exceptions for the user to
		/// understand what is going wrong.</remarks>
		protected virtual void Init(Stream streamRuleML, string uriRuleML, FileAccess mode) {
			if (AdapterState == State.Disposed)
				throw new BREException("A disposed adapter can not accept new operations");
			else if (AdapterState == State.NonInitialized) {
				if (mode == FileAccess.Read) {
					if (streamRuleML != null) {
						reader = new XmlTextReader(streamRuleML);
						navigator = new XPathDocument(GetXmlValidatingReaderForStream(DatalogSchema)).CreateNavigator();
					}
					else {
						reader = new XmlTextReader(uriRuleML);
						navigator = new XPathDocument(Xml.NewValidatingReader(reader)).CreateNavigator();
					}
					
					nsmgr = new XmlNamespaceManager(navigator.NameTable);
					nsmgr.AddNamespace("dl", DatalogNamespaceURL);
			
					ValidateRulebase();
					
					adapterState = State.Read;
				}
				else {
					if (streamRuleML != null)
						writer = new XmlTextWriter(streamRuleML, Encoding.UTF8);
					else
						writer = new XmlTextWriter(uriRuleML, Encoding.UTF8);
					
					writer.Formatting = Formatting.Indented;
					document = new XmlDocument();
					CreateDocumentElement();
					Document.InsertBefore(Document.CreateXmlDeclaration("1.0", "utf-8", "no"), Document.DocumentElement);
					Document.InsertBefore(Document.CreateComment(" Generated by " + this.GetType() + " "), Document.DocumentElement);
					AdapterState = State.Write;
				}
			}
			else
				throw new BREException("A RuleML adapter can not be initialized several times.");
		}
		
		
		// -------------- Interface methods, some being left for implementation to subclasses ---------------
		
		public abstract string Direction {	get; set; }
		
		public abstract string Label {	get; set; }
		
				
		/// <summary>
		/// Collection containing all the queries in the rulebase.
		/// </summary>		
		public ArrayList Queries {
			get {
				return ArrayList.ReadOnly(ExtractQueries());
			}
			set {
				if (value.Count > 0) {
					Document.DocumentElement.AppendChild(Document.CreateComment("Queries"));
					WriteQueries(value);
				}
			}
		}
		
		public ArrayList Implications {
			get {
				return ArrayList.ReadOnly(ExtractImplications());
		 	}
			set {
				if (value.Count > 0) {
					Document.DocumentElement.AppendChild(Document.CreateComment("Implications"));
					WriteImplications(value);
				}
			}
		}
		
		public ArrayList Facts {
			get {
				return ArrayList.ReadOnly(ExtracFacts());
		 	}
			set {
				if (value.Count > 0) {
					Document.DocumentElement.AppendChild(Document.CreateComment("Facts"));
					WriteFacts(value);
				}
			}
		}
		
		
		// -------------- Technical Methods to be implemented by sub-classes ---------------
		
		protected abstract void ValidateRulebase();
		
		protected abstract string DatalogNamespaceURL {get;}

		protected abstract string DatalogSchema {get;}
		
		protected abstract void CreateDocumentElement();
		
		protected abstract IPredicate BuildPredicate(XPathNavigator predicateElement, bool inHead, bool resolveImmediatly);
		
		protected struct RelationResolution {
			public AtomFunction.RelationResolutionType type;
			public string atomRelation;
		}
		
		protected abstract RelationResolution AnalyzeRelationResolution(XPathNavigator relationElement);
		
		
		// ---------------- Attributes, properties and methods related to XML parsing ------------------
		
		private XmlReader reader = null;
		private XmlTextWriter writer = null;
		private XmlNamespaceManager nsmgr;
		private XPathNavigator navigator;
		private XmlDocument document;
		
		protected XmlReader Reader {
			get {
				return reader;
			}
		}
		
		protected XmlTextWriter Writer {
			get {
				return writer;
			}
		}
		
		protected XmlDocument Document {
			get {
				return document;
			}
		}
		
		protected XPathNavigator Navigator {
			get {
				return navigator;
			}
		}
		
		public XmlNamespaceManager Nsmgr {
			get {
				return nsmgr;
			}
		}

		protected virtual XmlReader GetXmlValidatingReaderForStream(string schemaName) {
			return Xml.NewValidatingReader(Reader, ValidationType.Schema, schemaName);
		}

		protected XPathExpression BuildXPathExpression(string xPath) {
			XPathExpression xpe = Navigator.Compile(xPath);
			xpe.SetContext(Nsmgr);
			return xpe;
		}
		
		protected string GetString(XPathNavigator nav, string xpath) {
			return GetString(nav, BuildXPathExpression(xpath));
		}
		
		protected string GetString(XPathNavigator nav, XPathExpression xpe) {
			string result = null;
			XPathNodeIterator rbaselab = nav.Select(xpe);
			if (rbaselab.MoveNext()) {
				result = rbaselab.Current.Value;
				if ((result != null) && (result == String.Empty))
					result = null;
			}
			return result;
		}
		
		// --------------------- RuleML Parsing Methods --------------
		
		protected abstract XPathExpression RelativeLabelXPath {get;}
		protected abstract XPathExpression FactElementXPath {get;}
		protected abstract XPathExpression RelativeFactContentXPath {get;}
		protected abstract XPathExpression RelativeRelationXPath {get;}
		protected abstract XPathExpression RelativePredicatesXPath {get;}
		protected abstract XPathExpression ImplicationElementXPath {get;}
		protected abstract XPathExpression RelativeHeadXPath {get;}
		protected abstract XPathExpression RelativeBodyXPath {get;}
		protected abstract XPathExpression RelativeBodyContentXPath {get;}
		protected abstract XPathExpression QueryElementXPath {get;}
		
		protected ArrayList ExtracFacts() {
			ArrayList result = new ArrayList();
			XPathNodeIterator facts = Navigator.Select(FactElementXPath);

			while(facts.MoveNext()) {
				XPathNavigator fact = facts.Current;
				XPathNodeIterator head_atom = fact.Select(RelativeFactContentXPath);
				head_atom.MoveNext();
				result.Add(new Fact(GetString(fact, RelativeLabelXPath),
				                    GetAtom(head_atom.Current, false, true, true)));
			}

			return result;
		}
		
		protected ArrayList ExtractQueries() {
			ArrayList result = new ArrayList();
			
			XPathNodeIterator queries = Navigator.Select(QueryElementXPath);
			while(queries.MoveNext())	result.Add(GetQuery(queries.Current));
			
			return result;
		}
		
		protected ArrayList ExtractImplications() {
			ArrayList result = new ArrayList();
				
			XPathNodeIterator imps = Navigator.Select(ImplicationElementXPath);
			
			while(imps.MoveNext()) {
				XPathNavigator imp = imps.Current;
				Query query = GetQuery(imp);
				
				XPathNodeIterator head_atom = imp.Select(RelativeHeadXPath);
				if (head_atom.Count != 1) throw new BREException("Found " + head_atom.Count + " head atoms in implication '" + query.Label + "'");
				head_atom.MoveNext();

				ImplicationProperties ip = new ImplicationProperties(query.Label);
				
				// action matching
				ImplicationAction action;
				switch(ip.action.ToLower()) {
					case "":
					case "assert":
						action = ImplicationAction.Assert;
						break;
					case "retract":
						action = ImplicationAction.Retract;
						break;
					case "count":
						action = ImplicationAction.Count;
						break;
					case "modify":
						action = ImplicationAction.Modify;
						break;
					default:
						throw new BREException("An Implication can not perform the action: " + ip.action);
				}

				// instantiate new implication
				result.Add(new Implication(ip.label,
								                    ip.priority,
								                    ip.mutex,
								                    ip.precondition,
								                    GetAtom(head_atom.Current, false, true, false),
								                    query.AtomGroup,
								                    action));
			}
			
			return result;
		}
				
		protected Atom GetAtom(XPathNavigator atom, bool negative, bool inHead, bool resolveImmediatly) {
			// build the array of predicates
			ArrayList relationPredicates = new ArrayList();
			XPathNodeIterator predicates = atom.Select(RelativePredicatesXPath);
			while(predicates.MoveNext()) relationPredicates.Add(BuildPredicate(predicates.Current, inHead, resolveImmediatly));
			IPredicate[] predicatesArray = (IPredicate[])relationPredicates.ToArray(typeof(IPredicate));
			
			// identify function based atom relations
			XPathNodeIterator rel = atom.Select(RelativeRelationXPath);
			rel.MoveNext();
			RelationResolution relationResolution = AnalyzeRelationResolution(rel.Current);
			
			if ((relationResolution.type == AtomFunction.RelationResolutionType.NxBRE) ||
			    (relationResolution.type == AtomFunction.RelationResolutionType.Binder))
				return new AtomFunction(relationResolution.type,
				                        negative,
				                        Binder,
				                        relationResolution.atomRelation,
					          	  				predicatesArray);
			
			else if (relationResolution.type == AtomFunction.RelationResolutionType.Expression)
				return new AtomFunction(relationResolution.type,
				                        negative,
				                        new ExpressionRelater(relationResolution.atomRelation, predicatesArray),
				                        relationResolution.atomRelation,
					          	  				predicatesArray);
			else
				return new Atom(negative,
	              				relationResolution.atomRelation,
	          	  				predicatesArray);
		}
		
				
		private object[] GetAtomGroupContent(XPathNodeIterator body_atom) {
			ArrayList result = new ArrayList();
			
			while (body_atom.MoveNext()) {
				XPathNavigator currentBodyAtom = body_atom.Current;
				string name = currentBodyAtom.Name.ToLower();
					
				if (name == "atom")
					result.Add(GetAtom(currentBodyAtom, false, false, false));
				else if (name == "naf") {
					currentBodyAtom.MoveToFirstChild();

					if (currentBodyAtom.Name.ToLower() == "weak") {
						currentBodyAtom.MoveToFirstChild();
						result.Add(GetAtom(currentBodyAtom, true, false, false));
						currentBodyAtom.MoveToParent();
					}
					else {
						result.Add(GetAtom(currentBodyAtom, true, false, false));
					}
						
					currentBodyAtom.MoveToParent();
				}
				else if (name == "and")
					result.Add(NewAtomGroup(AtomGroup.LogicalOperator.And, GetAtomGroupContent(currentBodyAtom.SelectChildren(XPathNodeType.Element))));
				else if (name == "or")
					result.Add(NewAtomGroup(AtomGroup.LogicalOperator.Or, GetAtomGroupContent(currentBodyAtom.SelectChildren(XPathNodeType.Element))));
				else
					throw new BREException("Found an unrecognized element in body: " + currentBodyAtom.Name);
			}
			
			return result.ToArray();
		}

		protected Query GetQuery(XPathNavigator query) {
			string label = GetString(query, RelativeLabelXPath);
			XPathNodeIterator body = query.Select(RelativeBodyXPath);
			
			if (!body.MoveNext())	throw new BREException("Query '" + label + "' is bodyless");

			object[] content = GetAtomGroupContent(body.Current.Select(RelativeBodyContentXPath));
			
			if (content.Length == 0) throw new BREException("Can not locate any atom or atom group in: " + body.Current.Name);
			if (content.Length != 1) throw new BREException("Found unexpected query '" + label + "' body of size " + content.Length);
			
			if (content[0] is Atom)
				return new Query(label, NewAtomGroup(AtomGroup.LogicalOperator.And, content));
			else if (content[0] is AtomGroup)
				return new Query(label, (AtomGroup)content[0]);
			else
				throw new BREException("Found unexpected query '" + label +
			                         "' content of type " + content[0].GetType().FullName);
		}
		
		protected virtual AtomGroup NewAtomGroup(AtomGroup.LogicalOperator logicalOperator, object[] content) {
			return new AtomGroup(logicalOperator, content);
		}
		
		// --------------------- Evaluation related nested classes --------------
		
		/// <summary>
		/// Returns true if the binder is an internal expression evaluation binder.
		/// </summary>
		/// <param name="binder">The binder to evaluate</param>
		/// <returns>True if internal binder</returns>
		protected bool IsExpressionBinder(IBinder binder) {
			return ((binder is ExpressionEvaluator) || (binder is ExpressionRelater));
		}
		
		/// <summary>
		/// The ExpressionEvaluator is a private class that uses expression for performing binder "Evaluate".
		/// </summary>
		protected class ExpressionEvaluator:AbstractBinder {
			private readonly string expression;
			private readonly IList variableNames;
			private IListEvaluator evaluator;
			
			public ExpressionEvaluator(string individual):base(BindingTypes.Control){
				expression = individual;
				variableNames = new ArrayList();
				variableNames.Add(String.Empty); // an individual has no name
				evaluator = null;
			}
			
			public override bool Evaluate(object predicate, string function, string[] arguments) {
				if (evaluator == null) evaluator = Compilation.NewEvaluator(expression,
				                                							              Formula.DEFAULT_EXPRESSION_PLACEHOLDER,
											                                              variableNames,
											                                              new object[]{predicate});
				
				return Convert.ToBoolean(evaluator.Run(new object[]{predicate}));
			}
		}
		
		/// <summary>
		/// The ExpressionRelater is a private class that uses expression for performing binder "Relate".
		/// </summary>
		protected class ExpressionRelater:AbstractBinder {
			private readonly IList variableNames;
			private readonly string expression;
			private IListEvaluator evaluator;
			
			public ExpressionRelater(string atomRelation, IPredicate[] variables):base(BindingTypes.Control){
				expression = atomRelation;
				variableNames = new ArrayList();
				foreach(IPredicate variable in variables) variableNames.Add(variable.Value);
				evaluator = null;
			}
			
			public override bool Relate(string function, object[] predicates) {
				if (evaluator == null) evaluator = Compilation.NewEvaluator(expression,
											                                              Formula.DEFAULT_EXPRESSION_PLACEHOLDER,
											                                              variableNames,
											                                              predicates);
				
				return Convert.ToBoolean(evaluator.Run(predicates));;
			}
		}
		
		// ------------------- RuleML writing methods ------------------
		
		protected void WriteLabel(XmlElement target, string labelElement, string labelContentElement, string labelContent) {
			if ((labelContent != null) && (labelContent != String.Empty)) {
				XmlElement rbaselab = Document.CreateElement(labelElement, DatalogNamespaceURL);
				XmlElement ind = Document.CreateElement(labelContentElement, DatalogNamespaceURL);
				ind.InnerText = labelContent;
				rbaselab.AppendChild(ind);
				target.AppendChild(rbaselab);
			}
		}
		
		protected abstract void WriteLabel(XmlElement target, string labelContent);
		
		protected abstract void WriteQuery(XmlElement target, Query query);

		protected abstract void WriteQueries(ArrayList queries);
		
		protected abstract void WriteImplications(ArrayList implications);

		protected abstract void WriteFacts(ArrayList facts);
		
		protected void WriteAtomGroup(XmlElement target, AtomGroup atomGroup, string andElement, string orElement) {
			if (atomGroup.Members.Length != 1) {
				XmlElement op = Document.CreateElement((atomGroup.Operator == AtomGroup.LogicalOperator.And)?andElement:orElement, DatalogNamespaceURL);
				target.AppendChild(op);
				for(int i=0; i<atomGroup.Members.Length; i++) {
					if (atomGroup.Members[i] is Atom) WriteAtom(op, (Atom)atomGroup.Members[i], false);
					else if (atomGroup.Members[i] is AtomGroup) WriteAtomGroup(op, (AtomGroup)atomGroup.Members[i]);
				}
			}
			else
				WriteAtom(target, (Atom)atomGroup.Members[0], false);
		}
		
		protected abstract void WriteAtomGroup(XmlElement target, AtomGroup atomGroup);
		
		protected abstract void WriteAtom(XmlElement target, Atom atom, bool inFact);
		
		protected abstract void WriteFact(XmlElement target, Fact fact);
	}
}
