using System.Linq;

namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Xml;
	using System.Xml.XPath;
	
	using Core;
	using Rules;
	using Util;

	///<summary>Adapter supporting RuleML 0.9 NafDatalog Sublanguage.</summary>
	///<remarks>UTF-8 is the default encoding.</remarks>
	/// <author>David Dossot</author>
	public class RuleML09NafDatalogAdapter:AbstractRuleMLAdapter,IExtendedRuleBaseAdapter {
		private SaveFormatAttributes syntax;
		private bool forceDataTyping;
		private List<Equivalent> equivalents;
		private string globalDirection;
		
		private readonly static IList<Fact> NO_FACT =  new ReadOnlyCollection<Fact>(new List<Fact>());
		
		/// <summary>
		/// Instantiates a RuleML 0.9 NafDatalog adapter for reading from or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <remarks>
		/// By default, the standard syntax is used and non-typed individuals are not typified.
		/// When reading, all formats are automatically supported.
		/// </remarks>
		public RuleML09NafDatalogAdapter(Stream streamRuleML, FileAccess mode):base(streamRuleML, mode) {
			SetAttributes(SaveFormatAttributes.Standard);
			Init(streamRuleML, null, mode);
		}

		/// <summary>
		/// Instantiates a RuleML 0.9 NafDatalog adapter for reading from or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="attributes">The RuleML format attributes, only used if writing.</param>
		/// <remarks>When reading, all formats are automatically supported.</remarks>
		public RuleML09NafDatalogAdapter(Stream streamRuleML, FileAccess mode, SaveFormatAttributes attributes):base(streamRuleML, mode) {
			SetAttributes(attributes);
			Init(streamRuleML, null, mode);
		}

		/// <summary>
		/// Instantiates a RuleML 0.9 NafDatalog adapter for reading from or writing to an URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <remarks>
		/// By default, the standard syntax is used and non-typed individuals are not typified.
		/// When reading, all formats are automatically supported.
		/// </remarks>
		public RuleML09NafDatalogAdapter(string uriRuleML, FileAccess mode):base(uriRuleML, mode) {
			SetAttributes(SaveFormatAttributes.Standard);
			Init(null, uriRuleML, mode);
		}
		
		/// <summary>
		/// Instantiates a RuleML 0.9 Datalog adapter for reading from or writing to an URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="attributes">The RuleML format attributes, only used if writing.</param>
		/// <remarks>When reading, all formats are automatically supported.</remarks>
		public RuleML09NafDatalogAdapter(string uriRuleML, FileAccess mode, SaveFormatAttributes attributes):base(uriRuleML, mode) {
			SetAttributes(attributes);
			Init(null, uriRuleML, mode);
		}
		
		// --------------- Properties -----------------
		
		public override string Direction {
			get {
				return globalDirection;
			}
			set {
				globalDirection = value;
			}
		}
		
		public override string Label {
			get {
				return String.Empty + GetString(Navigator, "dl:RuleML/dl:oid/dl:Ind");
		 	}
			
			set {
				WriteLabel(Document.DocumentElement, value);
			}
		}
		
		/// <summary>
		/// Collection containing all the facts to assert.
		/// </summary>
		public virtual IList<Fact> Assertions {
			get {
				return base.Facts;
			}
			
			set {
				base.Facts = value;
			}
		}

		/// <summary>
		/// Collection containing all the facts to retract.
		/// </summary>
		public virtual IList<Fact> Retractions {
			get {
				// not supported in RuleML 0.9
				return NO_FACT;
			}
			
			set {
				// not supported in RuleML 0.9
			}
		}
		
		public virtual IList<Query> IntegrityQueries {
			get {
				var result = new List<Query>();
				
				var integrityQueries = Navigator.Select(IntegrityQueriesElementXPath);
				while(integrityQueries.MoveNext()) result.Add(GetQuery(integrityQueries.Current));
				
				return result.AsReadOnly();
		 	}
			
			set {
				WriteIntegrityQueries(value);
			}
		}
		
		public virtual IList<Equivalent> Equivalents {
			get {
				return equivalents.AsReadOnly();
		 	}
			
			set {
				WriteEquivalents(value);
			}
		}
		
		// Protected/Private methods --------------------------------------------------------
		
		protected override XPathExpression RelativeLabelXPath {
			get{ return BuildXPathExpression("dl:oid/dl:Ind"); }
		}
		
		protected override XPathExpression FactElementXPath {
			get{ return BuildXPathExpression("dl:RuleML/dl:Assert/dl:Atom | dl:RuleML/dl:Assert/dl:formula/dl:Atom"); }
		}
		
		protected override XPathExpression RelativeFactContentXPath {
			get{ return BuildXPathExpression("."); }
		}
		
		protected override XPathExpression RelativeRelationXPath {
			get { return BuildXPathExpression("dl:op/dl:Rel | dl:Rel"); }
		}
		
		protected override XPathExpression RelativePredicatesXPath {
			get {
				XPathExpression xpe = BuildXPathExpression("dl:slot | dl:Ind | dl:Var | dl:Data | dl:arg/dl:Ind | dl:arg/dl:Var | dl:arg/dl:Data");
				xpe.AddSort("../@index", XmlSortOrder.Ascending, XmlCaseOrder.None, String.Empty, XmlDataType.Number);
				return xpe;
			}
		}		

		protected override XPathExpression ImplicationElementXPath {
			get { return BuildXPathExpression("dl:RuleML/dl:Assert/dl:Implies | dl:RuleML/dl:Assert/dl:formula/dl:Implies"); }
		}
		
		protected override XPathExpression RelativeHeadXPath {
			get { return BuildXPathExpression("dl:head/dl:Atom | *[position()=last() and local-name(.)='Atom']"); }
		}
		
		protected override XPathExpression RelativeBodyXPath {
			get { return BuildXPathExpression("."); }
		}
		
		protected override XPathExpression RelativeBodyContentXPath {
			// who could have guessed that writing an xPath for selecting the body of a query/implication would become so complex
			// with the new compact/expanded modes?
			get { return BuildXPathExpression("dl:body/dl:Atom | dl:body/dl:Naf | dl:body/dl:And | dl:body/dl:Or | *[((local-name(..)='Implies' and position()=last()-1) or (local-name(..)='Query') or (local-name(..)='Integrity')) and (local-name(.)='Atom' or local-name(.)='Naf' or local-name(.)='Or' or local-name(.)='And')]"); }
		}
		
		protected override XPathExpression QueryElementXPath {
			get { return BuildXPathExpression("dl:RuleML/dl:Query"); }
		}
		
		protected virtual XPathExpression EquivalentElementXPath {
			get { return BuildXPathExpression("dl:RuleML/dl:Assert/dl:Equivalent"); }
		}
		
		protected virtual XPathExpression IntegrityQueriesElementXPath {
			get { return BuildXPathExpression("dl:RuleML/dl:Protect//dl:Integrity"); }
		}
		
		protected override string DatalogSchema {
			get {
				return "ruleml-0_9-nafdatalog.xsd";
			}
		}
		
		protected override string DatalogNamespaceURL {
			get {
				return "http://www.ruleml.org/0.9/xsd";
			}
		}
		
		protected override void CreateDocumentElement() {
			// this is darn ugly but i can not figure out how to add xsi:schemaLocation to the DOM - the other attributes were ok
			Document.LoadXml("<RuleML xmlns='" +
			                 DatalogNamespaceURL +
			                 "' xsi:schemaLocation='" +
			                 DatalogNamespaceURL +
			                 " " +
			                 DatalogSchema +
			                 "' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema'/>");
		}

		protected override void ValidateRulebase() {
			if (!Navigator.Select(BuildXPathExpression("dl:RuleML")).MoveNext())
						throw new BREException("Can not locate the rulebase root.");
			
			// quietly ignored elements : @mapClosure @closure
			// check not supported elements for which we will throw an exception, as the engine might not react as expected
			var notSupportedElements = new string[] {"declare", "slot[local-name(*[1]) != 'Ind']", "Exists", "Forall", "Reify", "Skolem", "Protect[not(dl:Integrity | dl:warden)]", "*[@type]"};
			
			foreach (var notSupportedElement in notSupportedElements.Where(notSupportedElement => Navigator.Select(BuildXPathExpression("//dl:" + notSupportedElement)).MoveNext()))
			    throw new BREException("RuleML element '" + notSupportedElement + "' is not supported by this adapter.");
		}
		
		private void SetAttributes(SaveFormatAttributes attributes) {
			// can not be compact & standard & expanded
			syntax = attributes & (SaveFormatAttributes.Compact|SaveFormatAttributes.Standard|SaveFormatAttributes.Expanded);

			if ((syntax != SaveFormatAttributes.Compact) && (syntax != SaveFormatAttributes.Standard) && (syntax != SaveFormatAttributes.Expanded))
				throw new ArgumentException("Can only use one save format attribute out of: compact, standard and expanded");
			
			forceDataTyping = ((attributes & SaveFormatAttributes.ForceDataTyping) == SaveFormatAttributes.ForceDataTyping);
		}
		
		protected override void Init(Stream streamRuleML, string uriRuleML, FileAccess mode) {
			base.Init(streamRuleML, uriRuleML, mode);

		    if (AdapterState != State.Read) return;
		    // estimate the global direction: ie consistent direction attributes (empty are ignored)
		    globalDirection = String.Empty;
		    var directionAttributes = Navigator.Select(BuildXPathExpression("//dl:*/@direction|//dl:*/@mapDirection"));
				
		    while(directionAttributes.MoveNext()) {
		        var direction = directionAttributes.Current.Value;
		        if ((direction == String.Empty) || (direction == "bidirectional")) continue;
		        if (globalDirection == String.Empty) {
		            globalDirection = direction;
		        }
		        else if (direction != globalDirection) {
		            globalDirection = "inconsistent";
		            break;
		        }
		    }
				
		    // load equivalent atom definitions
		    equivalents = new List<Equivalent>();
		    var equivalentElements = Navigator.Select(EquivalentElementXPath);
				
		    while (equivalentElements.MoveNext()) {
		        // extract label, if any
		        var labelIterator = equivalentElements.Current.Select(BuildXPathExpression("dl:oid/dl:Ind"));
		        var label = (labelIterator.MoveNext())?labelIterator.Current.Value:String.Empty;
					
		        var equivalentAtoms = equivalentElements.Current.Select(BuildXPathExpression(".//dl:Atom"));
		        if (equivalentAtoms.Count != 2) throw new BREException("An Equivalent group should contain exactly 2 atoms and not " + equivalentAtoms.Count);
					
		        equivalentAtoms.MoveNext();
		        var firstAtom = GetAtom(equivalentAtoms.Current, false, false, false);
					
		        equivalentAtoms.MoveNext();
		        var secondAtom = GetAtom(equivalentAtoms.Current, false, false, false);
					
		        equivalents.Add(new Equivalent(label, firstAtom, secondAtom));
		    }
		}
		
		protected override RelationResolution AnalyzeRelationResolution(XPathNavigator relationElement) {
		    var result = new RelationResolution {atomRelation = relationElement.Value};

		    var atomRelationURI = relationElement.GetAttribute("uri", string.Empty).ToLower();
			
			switch(atomRelationURI) {
				case "nxbre://operator":
					result.type = AtomFunction.RelationResolutionType.NxBRE;
					break;

				case "nxbre://binder":
					if (Binder == null) throw new BREException("No binder available for relation: " + result.atomRelation);
					result.type = AtomFunction.RelationResolutionType.Binder;
					break;

				case "nxbre://expression":
					result.type = AtomFunction.RelationResolutionType.Expression;
					break;
					
				default:
					result.type = AtomFunction.RelationResolutionType.None;
					break;
			}
			
			return result;
		}
		
		protected override IPredicate BuildPredicate(XPathNavigator predicateElement, bool inHead, bool resolveImmediatly) {
			IPredicate predicate;
			var predicateName = predicateElement.Name;
			var predicateValue = predicateElement.Value;
			
			switch(predicateName) {
				// --------- IND predicates --------
				case "Ind":
					var predicateURI = predicateElement.GetAttribute("uri", String.Empty).ToLower();
					
					switch(predicateURI) {
						case "nxbre://expression":
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
							break;
					
						case "nxbre://operator":
							// NxBRE operators must follow this pattern: operator(uniqueargument)
							var operatorCall = Parameter.ParseOperatorCall(predicateValue);
							predicate = new Function(Function.FunctionResolutionType.NxBRE,
							                         predicateValue,
							                         null,
							                         (string)operatorCall.First,
																			 (string)operatorCall.Second);
							break;
							
						case "nxbre://binder":
							if (Binder == null) throw new BREException("No binder available for Individual: " + predicateValue);
							
							if (inHead) predicate = new Formula(Formula.FormulaResolutionType.Binder,
							                        						Binder,
							                       							predicateValue);
							else predicate = Binder.AnalyzeIndividualPredicate(new Individual(predicateValue));
							
							break;
							
						case "":
							predicate = new Individual(predicateValue);
							break;
					
						default:
							// there is a predicateURI but it is not recognized by the engine so we assimilate it as
							// a web reference
							predicate = new Individual(new HyperLink(predicateValue, predicateURI));
							break;
					}
					break;
					
			// --------- VAR predicates --------
			case "Var":
				predicate = new Variable(predicateValue);
				break;
				
			// --------- DATA predicates --------
			case "Data":
				var schemaType = predicateElement.GetAttribute("type", "http://www.w3.org/2001/XMLSchema-instance");
				if (schemaType != String.Empty) {
					// remove any preceding namespace, like in "xs:string"
					if (schemaType.IndexOf(':')>=0) schemaType = schemaType.Split(':')[1];
					
					// this is a strongly typed individual
					predicate = new Individual(Xml.ToClr(predicateValue, schemaType), schemaType);
				}
				else {
					// this is just a string based predicate, using Data was not so wise...
					predicate = new Individual(predicateValue);
				}

				break;
				
			// --------- SLOT predicates --------
			case "slot":
				// the first child must be an Ind, we do not support other slot name holders
				if (predicateElement.MoveToFirstChild()) {
					if (predicateElement.Name != "Ind") throw new BREException("Only Ind is accepted as a slot name holder");
					var slotName = predicateElement.Value;
					if (!predicateElement.MoveToNext()) throw new BREException("A slot should contain two children");
					predicate = new Slot(slotName, BuildPredicate(predicateElement, inHead, resolveImmediatly));
				}
				else {
					throw new BREException("A slot can not be empty");
				}
				
				break;
			
			// --------- UNKNOWN predicates --------
			default:
				throw new BREException("Unsupported predicate type: " + predicateName);
			}
			
			return predicate;
		}
		
		protected override AtomGroup NewAtomGroup(AtomGroup.LogicalOperator logicalOperator, object[] content) {
			if (equivalents.Count == 0) return new AtomGroup(logicalOperator, content);
			
			// if we have equivalent atoms, try to translate content into equivalent sub-groups
			var enrichedContent = new ArrayList();
				
			foreach(var atomOrAtomGroup in content) {
				if (atomOrAtomGroup is Atom) {
					var atom = (Atom)atomOrAtomGroup;
					var atomEquivalents = RulesUtil.GetAll(equivalents, atom, new ArrayList());
						
					if (atomEquivalents.Count > 1) {
						if (logicalOperator == AtomGroup.LogicalOperator.Or) {
							// in an OR block, negative atoms are surrounded by AND
							if (atom.Negative) enrichedContent.Add(new AtomGroup(AtomGroup.LogicalOperator.And, atomEquivalents.ToArray()));
							else enrichedContent.AddRange(atomEquivalents);
					  }
				    else {
							// in an AND block, positive atoms are surrounded by OR
							if (atom.Negative) enrichedContent.AddRange(atomEquivalents);
				    	else enrichedContent.Add(new AtomGroup(AtomGroup.LogicalOperator.Or, atomEquivalents.ToArray()));
				    }
					}
					else {
						// add atoms that have found no equivalents
						enrichedContent.AddRange(atomEquivalents);
					}
				}
				else {
					// directly add atom groups
					enrichedContent.Add(atomOrAtomGroup);
				}
			}
			
			return new AtomGroup(logicalOperator, content, enrichedContent.ToArray());
		}
		
		// ----------------- RuleML writing methods ---------------
		
		protected override void WriteLabel(XmlElement target, string labelContent) {
			WriteLabel(target, "oid", "Ind", labelContent);
		}
		
		protected override void WriteAtom(XmlElement target, Atom atom, bool inFact) {
			XmlElement eAtom = eAtom = Document.CreateElement("Atom", DatalogNamespaceURL);
			
			if (atom.Label != null) WriteLabel(eAtom, atom.Label);
			
			var rel = Document.CreateElement("Rel", DatalogNamespaceURL);
			rel.InnerText = atom.Type;

		    var atomFunction = atom as AtomFunction;
		    if (atomFunction != null) {
				switch (atomFunction.ResolutionType) {
					case AtomFunction.RelationResolutionType.Binder:
						rel.SetAttribute("uri", "nxbre://binder");
						break;
					
					case AtomFunction.RelationResolutionType.NxBRE:
						rel.SetAttribute("uri", "nxbre://operator");
						break;
					
					case AtomFunction.RelationResolutionType.Expression:
						rel.SetAttribute("uri", "nxbre://expression");
						break;
				    case AtomFunction.RelationResolutionType.None:
				        break;
				    default:
				        throw new ArgumentOutOfRangeException();
				}
			}

		    if (syntax != SaveFormatAttributes.Compact)
		    {
		        var opr = Document.CreateElement("op", DatalogNamespaceURL);
		        opr.AppendChild(rel);
		        eAtom.AppendChild(opr);
		    }
		    else
		    {
		        eAtom.AppendChild(rel);
		    }


		    for (var i = 0; i < atom.Members.Length; i++)
		    {
		        var pre = atom.Members[i];

		        // build the predicate element depending on its type
		        XmlElement predicate;

		        if (pre is Variable)
		        {
		            predicate = Document.CreateElement("Var", DatalogNamespaceURL);
		            predicate.InnerText = pre.Value.ToString();
		        }
		        else if (pre is Individual)
		        {
		            var link = pre.Value as HyperLink;
		            if (link != null)
		            {
		                // we deal with the special case of hyperlinks
		                var hl = link;
		                predicate = Document.CreateElement("Ind", DatalogNamespaceURL);
		                predicate.SetAttribute("uri", hl.Uri);
		                predicate.InnerText = hl.Text;
		            }
		            else
		            {
		                var sourceType = ((Individual) pre).SourceType;
		                if ((forceDataTyping) && (!(pre.Value is string)) && ((sourceType == null) || (sourceType == String.Empty))) sourceType = Xml.GetSchemaTypeFromClr(pre.Value);

		                if (!string.IsNullOrEmpty(sourceType))
		                {
		                    // we persist as a typed data
		                    predicate = Document.CreateElement("Data", DatalogNamespaceURL);
		                    predicate.SetAttribute("type", Xml.NS_URI, "xs:" + sourceType);
		                    predicate.InnerText = Xml.FromClr(pre.Value, sourceType);
		                }
		                else
		                {
		                    // we persist as a String based individual
		                    predicate = Document.CreateElement("Ind", DatalogNamespaceURL);
		                    predicate.InnerText = pre.Value.ToString();
		                }
		            }
		        }
		        else if (pre is Formula)
		        {
		            predicate = Document.CreateElement("Ind", DatalogNamespaceURL);
		            predicate.SetAttribute("uri", (((Formula) pre).ResolutionType == Formula.FormulaResolutionType.Binder) ? "nxbre://binder" : "nxbre://expression");

		            predicate.InnerText = pre.Value.ToString();
		        }
		        else if (pre is Function)
		        {
		            var function = (Function) pre;
		            predicate = Document.CreateElement("Ind", DatalogNamespaceURL);
		            predicate.SetAttribute("uri", (function.ResolutionType == Function.FunctionResolutionType.NxBRE) ? "nxbre://operator" : (IsExpressionBinder(function.Binder) ? "nxbre://expression" : "nxbre://binder"));

		            predicate.InnerText = pre.Value.ToString();
		        }
		        else
		        {
		            // should never happen
		            throw new BREException("Can not persist a rulebase containing a predicate of type: " + pre.GetType().FullName);
		        }

		        // add wrapper elements if necessary: if there is one or more slot, args can not be used
		        var slotName = atom.SlotNames[i];
		        if (slotName != String.Empty)
		        {
		            var slot = Document.CreateElement("slot", DatalogNamespaceURL);
		            var slotInd = Document.CreateElement("Ind", DatalogNamespaceURL);
		            slotInd.InnerText = slotName;
		            slot.AppendChild(slotInd);
		            slot.AppendChild(predicate);
		            eAtom.AppendChild(slot);
		        }
		        else if ((!atom.HasSlot) && (syntax == SaveFormatAttributes.Expanded))
		        {
		            var argument = Document.CreateElement("arg", DatalogNamespaceURL);
		            argument.SetAttribute("index", (i + 1).ToString());
		            argument.AppendChild(predicate);
		            eAtom.AppendChild(argument);
		        }
		        else
		        {
		            eAtom.AppendChild(predicate);
		        }
		    }

		    if (atom.Negative)
		    {
		        var naf = Document.CreateElement("Naf", DatalogNamespaceURL);

		        if (syntax == SaveFormatAttributes.Expanded)
		        {
		            var weak = Document.CreateElement("weak", DatalogNamespaceURL);
		            weak.AppendChild(eAtom);
		            naf.AppendChild(weak);
		        }
		        else
		        {
		            naf.AppendChild(eAtom);
		        }

		        target.AppendChild(naf);
		    }
		    else
		    {
		        target.AppendChild(eAtom);
		    }
		}

	    protected override void WriteAtomGroup(XmlElement target, AtomGroup atomGroup)
	    {
	        WriteAtomGroup(target, atomGroup, "And", "Or");
	    }

	    protected override void WriteQuery(XmlElement target, Query query)
	    {
	        WriteQueryBody(target, Document.CreateElement("Query", DatalogNamespaceURL), query);
	    }

	    protected void WriteQueryBody(XmlElement target, XmlElement queryElement, Query query)
	    {
	        WriteLabel(queryElement, query.Label);
	        WriteAtomGroup(queryElement, query.AtomGroup);
	        target.AppendChild(queryElement);
	    }

	    protected virtual void WriteIntegrityQuery(XmlElement target, Query query)
	    {
	        if (syntax == SaveFormatAttributes.Expanded)
	        {
	            var warden = Document.CreateElement("warden", DatalogNamespaceURL);
	            target.AppendChild(warden);
	            target = warden;
	        }

	        WriteQueryBody(target, Document.CreateElement("Integrity", DatalogNamespaceURL), query);
	    }

	    protected virtual void WriteEquivalent(XmlElement target, Equivalent equivalent)
	    {
	        var equivalentElement = Document.CreateElement("Equivalent", DatalogNamespaceURL);
	        WriteLabel(equivalentElement, equivalent.Label);
	        WriteEquivalentAtom(equivalentElement, equivalent.FirstAtom);
	        WriteEquivalentAtom(equivalentElement, equivalent.SecondAtom);
	        target.AppendChild(equivalentElement);
	    }

	    private void WriteEquivalentAtom(XmlElement target, Atom atom)
	    {
	        if (syntax == SaveFormatAttributes.Expanded)
	        {
	            var torso = Document.CreateElement("torso", DatalogNamespaceURL);
	            WriteAtom(torso, atom, false);
	            target.AppendChild(torso);
	        }
	        else
	        {
	            WriteAtom(target, atom, false);
	        }
	    }

	    protected void WriteImplication(XmlElement target, Implication implication)
	    {
	        var implicationElement = Document.CreateElement("Implies", DatalogNamespaceURL);

	        // action mapping
	        var action = String.Empty;
	        if (implication.Action != ImplicationAction.Assert) action = implication.Action.ToString().ToLower();

	        var ip = new ImplicationProperties(implication.Label, implication.Priority, implication.Mutex, implication.Precondition, action);

	        WriteLabel(implicationElement, ip.ToString());

	        if (syntax == SaveFormatAttributes.Compact)
	        {
	            // in compact mode, the order is forced to body,head (equivalent to if,then)
	            WriteAtomGroup(implicationElement, implication.AtomGroup);
	            WriteAtom(implicationElement, implication.Deduction, false);
	        }
	        else
	        {
	            var body = Document.CreateElement("body", DatalogNamespaceURL);
	            WriteAtomGroup(body, implication.AtomGroup);
	            implicationElement.AppendChild(body);

	            var head = Document.CreateElement("head", DatalogNamespaceURL);
	            WriteAtom(head, implication.Deduction, false);
	            implicationElement.AppendChild(head);
	        }

	        if (syntax == SaveFormatAttributes.Expanded)
	        {
	            var formula = Document.CreateElement("formula", DatalogNamespaceURL);
	            formula.AppendChild(implicationElement);
	            target.AppendChild(formula);
	        }
	        else
	        {
	            target.AppendChild(implicationElement);
	        }
	    }

	    protected override void WriteFact(XmlElement target, Fact fact)
	    {
	        if (syntax == SaveFormatAttributes.Expanded)
	        {
	            var formula = Document.CreateElement("formula", DatalogNamespaceURL);
	            WriteAtom(formula, fact, true);
	            target.AppendChild(formula);
	        }
	        else
	        {
	            WriteAtom(target, fact, true);
	        }
	    }

	    protected XmlElement WriteMapElement(string name)
	    {
	        var element = Document.CreateElement(name, DatalogNamespaceURL);
	        // we ignore "bidirectional" which is the default
	        if ((!string.IsNullOrEmpty(Direction)) && (Direction != "bidirectional")) element.SetAttribute("mapDirection", Direction);
	        Document.DocumentElement.AppendChild(element);
	        return element;
	    }

	    protected override void WriteQueries(IList<Query> queries)
	    {
	        if (queries.Count <= 0) return;
	        Document.DocumentElement.AppendChild(Document.CreateComment("Queries"));
	        foreach (var query in queries) WriteQuery(Document.DocumentElement, query);
	    }

	    protected override void WriteImplications(IList<Implication> implications)
	    {
	        if (implications.Count <= 0) return;
	        Document.DocumentElement.AppendChild(Document.CreateComment("Implications"));
	        var target = WriteMapElement("Assert");
	        foreach (var implication in implications) WriteImplication(target, implication);
	    }

	    protected virtual void WriteEquivalents(IList<Equivalent> equivalents)
	    {
	        if (equivalents.Count <= 0) return;
	        Document.DocumentElement.AppendChild(Document.CreateComment("Equivalents"));
	        var target = WriteMapElement("Assert");
	        foreach (var equivalent in equivalents) WriteEquivalent(target, equivalent);
	    }

	    protected override void WriteFacts(IList<Fact> facts)
	    {
	        if (facts.Count <= 0) return;
	        Document.DocumentElement.AppendChild(Document.CreateComment("Facts"));
	        var target = WriteMapElement("Assert");
	        foreach (var fact in facts) WriteFact(target, fact);
	    }

	    protected virtual void WriteIntegrityQueries(IList<Query> integrityQueries)
	    {
	        if (integrityQueries.Count <= 0) return;
	        Document.DocumentElement.AppendChild(Document.CreateComment("Integrity Queries"));
	        var target = WriteMapElement("Protect");
	        foreach (var integrityQuery in integrityQueries) WriteIntegrityQuery(target, integrityQuery);
	    }
	}
}
