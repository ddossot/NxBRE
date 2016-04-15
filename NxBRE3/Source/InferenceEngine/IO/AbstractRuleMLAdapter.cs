using static System.String;

namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Xml;
	using System.Xml.XPath;
	using Rules;
	
	using Util;
	
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
				if (this.label == Empty) {
					// then if we have any tagged info, it is an error!!!
					if ((this.priority != -1) ||
					    (this.mutex != Empty) ||
					    (this.precondition != Empty) ||
					    (this.action != Empty))
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
				if (priority == Empty) this.priority = -1; // this means: someone has to assign me the default value for priority
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
			
			public override string ToString()
			{
			    if ((priority != (int)ImplicationPriority.Medium) ||
				    (mutex != Empty) ||
				    (precondition != Empty) ||
				    (action != Empty))
					return "label:" + label + ";" +
								 ((priority != (int)ImplicationPriority.Medium)?"priority:" + priority + ";" : "") +
								 ((mutex != Empty)?"mutex:" + mutex + ";" : "") +
								 ((precondition != Empty)?"precondition:" + precondition + ";" : "") +
								 ((action != Empty)?"action:" + action + ";" : "");
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
				if ((AdapterState == State.Read) && (reader != null)) {
					reader.Close();
					reader = null;
				}
				else if ((AdapterState == State.Write) && (writer != null)) {
					document.WriteTo(writer);
					writer.Flush();
					writer.Close();
					writer = null;
				} 
			}
			finally {
				AdapterState = State.Disposed;
			}
		}
		
		
		///<remarks>in case of any failure, this will throw enough exceptions for the user to
		/// understand what is going wrong.</remarks>
		protected virtual void Init(Stream streamRuleML, string uriRuleML, FileAccess mode)
		{
		    switch (AdapterState)
		    {
		        case State.Disposed:
		            throw new BREException("A disposed adapter can not accept new operations");
		        case State.NonInitialized:
		            if (mode == FileAccess.Read) {
		                var settings = new XmlReaderSettings {ProhibitDtd = false};
		                settings.CloseInput = true;
					
		                reader = streamRuleML != null ? XmlReader.Create(streamRuleML, settings) : XmlReader.Create(uriRuleML, settings);
					
		                try {
		                    navigator = new XPathDocument(GetXmlValidatingReader(DatalogSchema)).CreateNavigator();
		                    nsmgr = new XmlNamespaceManager(navigator.NameTable);
		                    nsmgr.AddNamespace("dl", DatalogNamespaceURL);
		                    ValidateRulebase();
		                } catch(Exception e) {
		                    // Fix for bug 1850290: release file on error
		                    reader?.Close();
		                    throw;
		                }
					
		                AdapterState = State.Read;
		            }
		            else {
		                var settings = new XmlWriterSettings();
		                settings.Encoding = Encoding.UTF8;
		                settings.Indent = true;
		                settings.CloseOutput = true;
						
		                writer = streamRuleML != null ? XmlWriter.Create(streamRuleML, settings) : XmlWriter.Create(uriRuleML, settings);
					
		                document = new XmlDocument();
		                CreateDocumentElement();
		                Document.InsertBefore(Document.CreateXmlDeclaration("1.0", "utf-8", "no"), Document.DocumentElement);
		                Document.InsertBefore(Document.CreateComment(" Generated by " + this.GetType() + " "), Document.DocumentElement);
		                AdapterState = State.Write;
		            }
		            break;
		        default:
		            throw new BREException("A RuleML adapter can not be initialized several times.");
		    }
		}


	    // -------------- Interface methods, some being left for implementation to subclasses ---------------
		
		public abstract string Direction {	get; set; }
		
		public abstract string Label {	get; set; }
				
		public virtual IList<Query> Queries {
			get {
				return ExtractQueries().AsReadOnly();
			}
			set {
				WriteQueries(value);
			}
		}
		
		public virtual IList<Implication> Implications {
			get {
				return ExtractImplications().AsReadOnly();
		 	}
			set {
				WriteImplications(value);
			}
		}
		
		public virtual IList<Fact> Facts {
			get {
				return ExtracFacts().AsReadOnly();
		 	}
			set {
				WriteFacts(value);
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
		
		private XmlReader reader;
		private XmlWriter writer;
		private XmlNamespaceManager nsmgr;
		private XPathNavigator navigator;
		private XmlDocument document;
		
		protected XmlReader Reader {
			get {
				return reader;
			}
		}
		
		protected XmlWriter Writer {
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

		protected virtual XmlReader GetXmlValidatingReader(string schemaName) {
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
			var rbaselab = nav.Select(xpe);
		    if (!rbaselab.MoveNext()) return result;
		    result = rbaselab.Current.Value;
		    if ((result != null) && (result == Empty))
		        result = null;
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
		
		protected List<Fact> ExtracFacts() {
			List<Fact> result = new List<Fact>();
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
		
		protected List<Query> ExtractQueries() {
			List<Query> result = new List<Query>();
			
			XPathNodeIterator queries = Navigator.Select(QueryElementXPath);
			
			while(queries.MoveNext()) result.Add(GetQuery(queries.Current));
			
			return result;
		}
		
		protected List<Implication> ExtractImplications() {
			var result = new List<Implication>();
				
			var imps = Navigator.Select(ImplicationElementXPath);
			
			while(imps.MoveNext()) {
				var imp = imps.Current;
				var query = GetQuery(imp);
				
				var head_atom = imp.Select(RelativeHeadXPath);
				if (head_atom.Count != 1) throw new BREException("Found " + head_atom.Count + " head atoms in implication '" + query.Label + "'");
				head_atom.MoveNext();

				var ip = new ImplicationProperties(query.Label);
				
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
					case "none":
						action = ImplicationAction.None;
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
			var relationPredicates = new List<IPredicate>();
			var predicates = atom.Select(RelativePredicatesXPath);
			while(predicates.MoveNext()) relationPredicates.Add(BuildPredicate(predicates.Current, inHead, resolveImmediatly));
			var predicatesArray = relationPredicates.ToArray();
			
			// identify function based atom relations
			var rel = atom.Select(RelativeRelationXPath);
			rel.MoveNext();
			var relationResolution = AnalyzeRelationResolution(rel.Current);
			
			switch (relationResolution.type)
			{
			    case AtomFunction.RelationResolutionType.NxBRE:
			    case AtomFunction.RelationResolutionType.Binder:

			        return new AtomFunction(relationResolution.type,
			            negative,
			            Binder,
			            relationResolution.atomRelation,
			            predicatesArray);
			    case AtomFunction.RelationResolutionType.Expression:

			        return new AtomFunction(relationResolution.type,
			            negative,
			            new ExpressionRelater(relationResolution.atomRelation, predicatesArray),
			            relationResolution.atomRelation,
			            predicatesArray);
			    default:
			        return new Atom(negative,
			            GetString(atom, RelativeLabelXPath),
			            relationResolution.atomRelation,
			            predicatesArray);
			}
		}
		
				
		private object[] GetAtomGroupContent(XPathNodeIterator body_atom) {
			var result = new ArrayList();
			
			while (body_atom.MoveNext()) {
				var currentBodyAtom = body_atom.Current;
				var name = currentBodyAtom.Name.ToLower();
					
				switch (name)
				{
				    case "atom":
				        result.Add(GetAtom(currentBodyAtom, false, false, false));
				        break;
				    case "naf":
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
				        break;
				    case "and":
				        result.Add(NewAtomGroup(AtomGroup.LogicalOperator.And, GetAtomGroupContent(currentBodyAtom.SelectChildren(XPathNodeType.Element))));
				        break;
				    case "or":
				        result.Add(NewAtomGroup(AtomGroup.LogicalOperator.Or, GetAtomGroupContent(currentBodyAtom.SelectChildren(XPathNodeType.Element))));
				        break;
				    default:
				        throw new BREException("Found an unrecognized element in body: " + currentBodyAtom.Name);
				}
			}
			
			return result.ToArray();
		}

		protected Query GetQuery(XPathNavigator query) {
			var label = GetString(query, RelativeLabelXPath);
			var body = query.Select(RelativeBodyXPath);

		    if (!body.MoveNext()) throw new BREException("Query '" + label + "' is bodyless");
		    var content = GetAtomGroupContent(body.Current.Select(RelativeBodyContentXPath));

		    if (content.Length == 0)
		        throw new BREException("Can not locate any atom or atom group in: " + body.Current.Name);
		    if (content.Length != 1)
		        throw new BREException("Found unexpected query '" + label + "' body of size " + content.Length);

		    if (content[0] is Atom)
		        return new Query(label, NewAtomGroup(AtomGroup.LogicalOperator.And, content));
		    var @group = content[0] as AtomGroup;
		    if (@group != null)
		        return new Query(label, @group);
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
				variableNames.Add(Empty); // an individual has no name
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
				foreach(var variable in variables) variableNames.Add(variable.Value);
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
			if ((labelContent != null) && (labelContent != Empty)) {
				XmlElement rbaselab = Document.CreateElement(labelElement, DatalogNamespaceURL);
				XmlElement ind = Document.CreateElement(labelContentElement, DatalogNamespaceURL);
				ind.InnerText = labelContent;
				rbaselab.AppendChild(ind);
				target.AppendChild(rbaselab);
			}
		}
		
		protected abstract void WriteLabel(XmlElement target, string labelContent);
		
		protected abstract void WriteQuery(XmlElement target, Query query);

		protected abstract void WriteQueries(IList<Query> queries);
		
		protected abstract void WriteImplications(IList<Implication> implications);

		protected abstract void WriteFacts(IList<Fact> facts);
		
		protected void WriteAtomGroup(XmlElement target, AtomGroup atomGroup, string andElement, string orElement) {
			if (atomGroup.Members.Length != 1) {
				var op = Document.CreateElement((atomGroup.Operator == AtomGroup.LogicalOperator.And)?andElement:orElement, DatalogNamespaceURL);
				target.AppendChild(op);
                foreach (var t in atomGroup.Members)
                {
                    var atom = t as Atom;
                    if (atom != null) WriteAtom(op, atom, false);
				    else if (t is AtomGroup) WriteAtomGroup(op, (AtomGroup)t);
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
