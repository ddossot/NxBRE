namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;
	using System.Xml.XPath;
	using System.Xml.Schema;
	
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;

	///<summary>Adapter supporting RuleML 0.86 NafDatalog Sublanguage.</summary>
	///<remarks>UTF-8 is the default encoding.</remarks>
	/// <author>David Dossot</author>
	public class RuleML086NafDatalogAdapter:AbstractRuleMLAdapter {

		private readonly bool supportTypedFacts;
		/// <summary>
		/// Instantiates a RuleML 0.86 NafDatalog adapter for reading or writing to a stream.
		/// </summary>
		/// <remarks>
		/// By default, typed facts are not supported.
		/// </remarks>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public RuleML086NafDatalogAdapter(Stream streamRuleML, FileAccess mode):this(streamRuleML, mode, false) {}

		/// <summary>
		/// Instantiates a RuleML 0.86 Datalog adapter for reading or writing to an URI.
		/// </summary>
		/// <remarks>
		/// By default, typed facts are not supported.
		/// </remarks>
		/// <param name="uriRuleML">The URI to read rules from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public RuleML086NafDatalogAdapter(string uriRuleML, FileAccess mode):this(uriRuleML, mode, false) {}
		
		/// <summary>
		/// Instantiates a RuleML 0.86 NafDatalog adapter for reading or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="supportTypedFacts">If true, the adapter supports typed facts.</param>
		public RuleML086NafDatalogAdapter(Stream streamRuleML, FileAccess mode, bool supportTypedFacts):base(streamRuleML, mode) {
			this.supportTypedFacts = supportTypedFacts;
			Init(streamRuleML, null, mode);
		}

		/// <summary>
		/// Instantiates a RuleML 0.86 Datalog adapter for reading or writing to an URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read rules from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="supportTypedFacts">If true, the adapter supports typed facts.</param>
		public RuleML086NafDatalogAdapter(string uriRuleML, FileAccess mode, bool supportTypedFacts):base(uriRuleML, mode) {
			this.supportTypedFacts = supportTypedFacts;
			Init(null, uriRuleML, mode);
		}
		
		/// <summary>
		/// Optional direction of the rulebase: forward, backward or bidirectional.
		/// </summary>
		public override string Direction {
			get {
				return String.Empty + GetString(Navigator, "dl:rulebase/@direction");
			}
			set {
				Document.DocumentElement.SetAttribute("direction", value);
			}
		}

		/// <summary>
		/// Optional label of the rulebase.
		/// </summary>
		public override string Label {
			get {
				return String.Empty + GetString(Navigator, "dl:rulebase/dl:_rbaselab/dl:ind");
			}
			set {
				WriteLabel(Document.DocumentElement, "_rbaselab", "ind", value);
			}
		}
				
		// Protected/Private methods --------------------------------------------------------
		
		protected override XPathExpression RelativeLabelXPath {
			get{ return BuildXPathExpression("dl:_rlab/dl:ind"); }
		}
		
		protected override XPathExpression FactElementXPath {
			get{ return BuildXPathExpression("dl:rulebase/dl:fact"); }
		}
		
		protected override XPathExpression RelativeFactContentXPath {
			get{ return BuildXPathExpression("dl:_head/dl:atom"); }
		}
		
		protected override XPathExpression RelativeRelationXPath {
			get { return BuildXPathExpression("dl:_opr/dl:rel"); }
		}
		
		protected override XPathExpression RelativePredicatesXPath {
			get {	return BuildXPathExpression("dl:ind | dl:var");	}
		}

		protected override XPathExpression ImplicationElementXPath {
			get { return BuildXPathExpression("dl:rulebase/dl:imp"); }
		}
		
		protected override XPathExpression RelativeHeadXPath {
			get { return BuildXPathExpression("dl:_head/dl:atom"); }
		}
		
		protected override XPathExpression RelativeBodyXPath {
			get { return BuildXPathExpression("dl:_body"); }
		}
		
		protected override XPathExpression RelativeBodyContentXPath {
			get { return BuildXPathExpression("dl:atom | dl:naf | dl:and | dl:or"); }
		}
		
		protected override void ValidateRulebase() {
			if (!Navigator.Select(BuildXPathExpression("dl:rulebase")).MoveNext())
				throw new BREException("Can not locate the rulebase root.");
			
			if (Navigator.Select(BuildXPathExpression("dl:rulebase//dl:_slot")).MoveNext())
				throw new BREException("RuleML Datalog Slots are not supported by this adapter.");
		}
		
		protected override XPathExpression QueryElementXPath {
			get { return BuildXPathExpression("dl:rulebase/dl:query"); }
		}

		protected override string DatalogSchema {
			get {
				return "ruleml-0_86-nafdatalog.xsd";
			}
		}
		
		protected override string DatalogNamespaceURL {
			get {
				return "http://www.ruleml.org/0.86/xsd";
			}
		}
		
		protected override void CreateDocumentElement() {
			// this is darn ugly but i can not figure out how to add xsi:schemaLocation to the DOM - the other attributes were ok
			Document.LoadXml("<rulebase xmlns='" +
			                 DatalogNamespaceURL +
			                 "' xsi:schemaLocation='" +
			                 DatalogNamespaceURL +
			                 " " +
			                 DatalogSchema +
			                 "' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'/>");
		}
		
		protected override RelationResolution AnalyzeRelationResolution(XPathNavigator relationElement) {
			RelationResolution result = new RelationResolution();

			string lowerAtomRelation = relationElement.Value.ToLower();
				
			if (lowerAtomRelation.StartsWith("nxbre:")) {
				result.atomRelation = relationElement.Value;
				result.type = AtomFunction.RelationResolutionType.NxBRE;
			}
			else if (lowerAtomRelation.StartsWith("binder:")) {
				result.atomRelation = relationElement.Value;
				result.type = AtomFunction.RelationResolutionType.Binder;
			}
			else if (lowerAtomRelation.EndsWith("()")) {
				result.atomRelation = relationElement.Value;
				result.type = AtomFunction.RelationResolutionType.Binder;
			}
			else if (lowerAtomRelation.StartsWith("expr:")) {
				result.atomRelation = relationElement.Value;
				result.type = AtomFunction.RelationResolutionType.Expression;
			}
			else {
				result.atomRelation = relationElement.Value;
				result.type = AtomFunction.RelationResolutionType.None;
			}
			return result;
		}
		
		protected override IPredicate BuildPredicate(XPathNavigator predicateElement, bool inHead, bool resolveImmediatly) {
			IPredicate predicate;
			string predicateName = predicateElement.Name;
			string predicateValue = predicateElement.Value;
			
			switch(predicateName) {
				// --------- IND predicates --------
				case "ind":
					if (predicateValue.ToLower().StartsWith("expr:")) {
						if (inHead) {
							if (resolveImmediatly) predicate = new Individual(Compilation.Evaluate(predicateValue));
							else predicate = new Formula(Formula.FormulaResolutionType.NxBRE,
							                             Binder,
							                             predicateValue);
						}
						else {
							predicate = new Function(Function.FunctionResolutionType.Binder,
							                         predicateValue,
							                         new ExpressionEvaluator(predicateValue),
							                       	 String.Empty,
							                      	 String.Empty);
						}
					}
					else if (predicateValue.ToLower().StartsWith("nxbre:")) {
						// NxBRE functions must follow this pattern: NxBRE:Function(uniqueargument)
						ObjectPair operatorCall = Parameter.ParseOperatorCall(predicateValue);
						predicate = new Function(Function.FunctionResolutionType.NxBRE,
						                         predicateValue,
						                         null,
																		 (string)operatorCall.First,
																		 (string)operatorCall.Second);
					}
					else if (Binder == null) {
						predicate = new Individual(predicateValue);
					}
					else if ((inHead) && (predicateValue.ToLower().StartsWith("binder:"))) {
						predicate = new Formula(Formula.FormulaResolutionType.Binder,
						                        Binder,
						                        predicateValue);
					}
					else if ((inHead) && (predicateValue.EndsWith("()"))) {
						predicate = new Formula(Formula.FormulaResolutionType.Binder,
						                        Binder,
						                        predicateValue);
					}
					else {
						predicate = Binder.AnalyzeIndividualPredicate(new Individual(predicateValue));
					}
			
				break;
			
			// --------- VAR predicates --------
			case "var":
				predicate = new Variable(predicateValue);
				break;
			
			// --------- UNKNOWN predicates --------
			default:
				throw new BREException("Unsupported predicate type: " + predicateName);
			}
			
			return predicate;
		}
		
		// ------------------- RuleML writing methods ------------------
		
		protected override void WriteLabel(XmlElement target, string labelContent) {
			WriteLabel(target, "_rlab", "ind", labelContent);
		}
		
		protected override void WriteAtomGroup(XmlElement target, AtomGroup atomGroup) {
			WriteAtomGroup(target, atomGroup, "and", "or");
		}
		
		///<summary>Predicate persistence relies on ToString()
		/// Do not expect business object persistence with this adapter!</summary>
		protected override void WriteAtom(XmlElement target, Atom atom, bool inFact) {
			XmlElement eAtom = eAtom = Document.CreateElement("atom", DatalogNamespaceURL);
			XmlElement rel = Document.CreateElement("rel", DatalogNamespaceURL);
			rel.InnerText = atom.Type;
			XmlElement opr = Document.CreateElement("_opr", DatalogNamespaceURL);
			opr.AppendChild(rel);
			eAtom.AppendChild(opr);
			
			foreach(IPredicate pre in	atom.Members) {
				XmlElement predicate = Document.CreateElement((pre is Variable)?"var":"ind", DatalogNamespaceURL);
				object predicateValue = pre.Value;
				
				if ((inFact) && (supportTypedFacts) && (!(predicateValue is string))) {
					if (predicateValue is IConvertible) predicate.InnerText = "expr:System.Convert.To" + predicateValue.GetType().Name + "(\"" + predicateValue.ToString() + "\")";
					else throw new BREException(predicateValue.GetType().FullName + " is not IConvertible hence can not be persisted as a typed fact: " + atom);
				}
				else {
					predicate.InnerText = predicateValue.ToString();
				}
				
				eAtom.AppendChild(predicate);
			}
			if (atom.Negative) {
				XmlElement naf = Document.CreateElement("naf", DatalogNamespaceURL);
				naf.AppendChild(eAtom);
				target.AppendChild(naf);
			}
			else {
				target.AppendChild(eAtom);
			}
		}
		
		protected override void WriteFact(XmlElement target, Fact fact) {
			XmlElement eFact = Document.CreateElement("fact", DatalogNamespaceURL);
			WriteLabel(eFact, fact.Label);
			XmlElement head = Document.CreateElement("_head", DatalogNamespaceURL);
			WriteAtom(head, fact, true);
			eFact.AppendChild(head);
			target.AppendChild(eFact);
		}
		
		protected override void WriteQueries(IList<Query> queries) {
			foreach(Query query in queries) WriteQuery(Document.DocumentElement, query);
		}

		protected override void WriteImplications(IList<Implication> implications) {
			foreach(Implication implication in implications) WriteImplication(Document.DocumentElement, implication);
		}

		protected override void WriteFacts(IList<Fact> facts) {
			foreach(Fact fact in facts)	WriteFact(Document.DocumentElement, fact);
		}
		
		protected override void WriteQuery(XmlElement target, Query query) {
			XmlElement eQuery = Document.CreateElement("query", DatalogNamespaceURL);
			WriteLabel(eQuery, query.Label);
			XmlElement body = Document.CreateElement("_body", DatalogNamespaceURL);
			WriteAtomGroup(body, query.AtomGroup);
			eQuery.AppendChild(body);
			target.AppendChild(eQuery);
		}
		
		private void WriteImplication(XmlElement target, Implication implication) {
			XmlElement eImplication = Document.CreateElement("imp", DatalogNamespaceURL);
			
			// action mapping
			String action = String.Empty;
			if (implication.Action != ImplicationAction.Assert)
				action = implication.Action.ToString().ToLower();
			
			ImplicationProperties ip = new ImplicationProperties(implication.Label,
			                                                     implication.Priority,
			                                                     implication.Mutex,
			                                                     implication.Precondition,
			                                                     action);
			WriteLabel(eImplication, ip.ToString());
			XmlElement head = Document.CreateElement("_head", DatalogNamespaceURL);
			WriteAtom(head, implication.Deduction, false);
			eImplication.AppendChild(head);
			XmlElement body = Document.CreateElement("_body", DatalogNamespaceURL);
			WriteAtomGroup(body, implication.AtomGroup);
			eImplication.AppendChild(body);
			target.AppendChild(eImplication);
		}
		
	}
}
