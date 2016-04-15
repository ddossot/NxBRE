/// <summary>
/// CURRENTLY INTERNAL BECAUSE NOT READY FOR PRIME TIME!
/// </summary>
namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.XPath;
	
	using NxBRE.InferenceEngine.Core;
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;

	///<summary>Adapter supporting RuleML 0.91 NafDatalog Sublanguage.</summary>
	///<remarks>UTF-8 is the default encoding.</remarks>
	/// <author>David Dossot</author>
	internal class RuleML091NafDatalogAdapter:AccumulatingExtendedRuleBaseAdapter {
		private static readonly Fact IGNORED_DEDUCTION = new Fact("_ignored_");

		/// <summary>
		/// Instantiates a RuleML 0.91 NafDatalog adapter for reading from or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <remarks>
		/// By default, the standard syntax is used and non-typed individuals are not typified.
		/// When reading, all formats are automatically supported.
		/// </remarks>
		public RuleML091NafDatalogAdapter(Stream streamRuleML, FileAccess mode):base(streamRuleML, mode) {
			// NOOP
		}

		/// <summary>
		/// Instantiates a RuleML 0.91 NafDatalog adapter for reading from or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="attributes">The RuleML format attributes, only used if writing.</param>
		/// <remarks>When reading, all formats are automatically supported.</remarks>
		public RuleML091NafDatalogAdapter(Stream streamRuleML, FileAccess mode, SaveFormatAttributes attributes):base(streamRuleML, mode) {
			// NOOP
		}

		/// <summary>
		/// Instantiates a RuleML 0.91 NafDatalog adapter for reading from or writing to an URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <remarks>
		/// By default, the standard syntax is used and non-typed individuals are not typified.
		/// When reading, all formats are automatically supported.
		/// </remarks>
		public RuleML091NafDatalogAdapter(string uriRuleML, FileAccess mode):base(uriRuleML, mode) {
			// NOOP
		}
		
		/// <summary>
		/// Instantiates a RuleML 0.91 Datalog adapter for reading from or writing to an URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="attributes">The RuleML format attributes, only used if writing.</param>
		/// <remarks>When reading, all formats are automatically supported.</remarks>
		public RuleML091NafDatalogAdapter(string uriRuleML, FileAccess mode, SaveFormatAttributes attributes):base(uriRuleML, mode) {
			// NOOP
		}
				
		// Protected/Private methods --------------------------------------------------------
		
		protected override XPathExpression FactElementXPath {
			get{ return BuildXPathExpression(base.FactElementXPath.Expression + " | //dl:Rulebase/dl:Atom | //dl:Rulebase/dl:formula/dl:Atom"); }
		}
		
		protected override XPathExpression ImplicationElementXPath {
			get { return BuildXPathExpression(base.ImplicationElementXPath.Expression + " | dl:RuleML/dl:Assert/dl:Rulebase/dl:Implies | dl:RuleML/dl:Assert/dl:Rulebase/dl:formula/dl:Implies | dl:RuleML/dl:Assert/dl:Entails/dl:Rulebase[1]/dl:Implies | dl:RuleML/dl:Assert/dl:Entails/dl:Rulebase[1]/dl:formula/dl:Implies"); }
		}

		protected override XPathExpression EquivalentElementXPath {
			get { return BuildXPathExpression(base.EquivalentElementXPath +  " | //dl:Rulebase/dl:Equivalent"); }
		}
		
		protected override XPathExpression IntegrityQueriesElementXPath {
			get { return BuildXPathExpression(base.IntegrityQueriesElementXPath + " | dl:RuleML/dl:Assert/dl:Entails/dl:Rulebase[2]/dl:Implies | dl:RuleML/dl:Assert/dl:Entails/dl:Rulebase[2]/dl:formula/dl:Implies"); }
		}
		
		protected override string DatalogSchema {
			get {
				return "ruleml-0_91-nafdatalog.xsd";
			}
		}
		
		protected override string DatalogNamespaceURL {
			get {
				return "http://www.ruleml.org/0.91/xsd";
			}
		}
		
		protected override void ValidateRulebase() {
			base.ValidateRulebase();
			
			//TODO FR-1546485: enable the currently unsupported features: retraction, multiple rulebases, "named" rulebases
			string[] notSupportedXPaths = new string[] {"//dl:Retract", "//dl:Assert/dl:Rulebase", "//dl:Rulebase/dl:oid"};
			
			foreach(string notSupportedXPath in notSupportedXPaths)
				if (Navigator.Select(BuildXPathExpression(notSupportedXPath)).MoveNext())
					throw new BREException("RuleML syntax '" + notSupportedXPath + "' is currently not supported by this adapter.");
		}
		
		protected override void WriteIntegrityQuery(XmlElement target, Query query) {
			//TODO FR-1546485: check how integrity queries are actually supposed to be written in Naf Datalog!
			WriteImplication(target, new Implication(query.Label, ImplicationPriority.Medium, String.Empty, String.Empty, IGNORED_DEDUCTION, query.AtomGroup));
		}

		protected override void BuildDomRulebase(IList<Fact> facts, IList<Query> queries, IList<Implication> implications, IList<Equivalent> equivalents, IList<Query> integrityQueries) {
			WriteQueries(queries);
			
			XmlElement assert = WriteMapElement("Assert");
			
			XmlElement mainRuleBaseParent = assert;
			
			if (integrityQueries.Count > 0) {
				XmlElement entails = Document.CreateElement("Entails", DatalogNamespaceURL);
				assert.AppendChild(entails);
				mainRuleBaseParent = entails;
			}
			
			XmlElement mainRuleBase = Document.CreateElement("Rulebase", DatalogNamespaceURL);
			mainRuleBaseParent.AppendChild(mainRuleBase);
			
			if (implications.Count > 0) {
				mainRuleBase.AppendChild(Document.CreateComment("Implications"));
				foreach(Implication implication in implications) WriteImplication(mainRuleBase, implication);
			}
			
			if (equivalents.Count > 0) {
				mainRuleBase.AppendChild(Document.CreateComment("Equivalents"));
				foreach(Equivalent equivalent in equivalents) WriteEquivalent(mainRuleBase, equivalent);
			}
			
			if (facts.Count > 0) {
				mainRuleBase.AppendChild(Document.CreateComment("Facts"));
				foreach(Fact fact in facts)	WriteFact(mainRuleBase, fact);
			}
			
			if (integrityQueries.Count > 0) {
				XmlElement integrityRuleBase = Document.CreateElement("Rulebase", DatalogNamespaceURL);
				mainRuleBaseParent.AppendChild(integrityRuleBase);
				
				if (integrityQueries.Count > 0) {
					integrityRuleBase.AppendChild(Document.CreateComment("Integrity Queries"));
					foreach(Query integrityQuery in integrityQueries) WriteIntegrityQuery(integrityRuleBase, integrityQuery);
				}
			}
			
		}
		
	}
	
}
