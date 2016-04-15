namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.IO;
	using System.Xml;
	using System.Xml.XPath;
	
	using NxBRE.Util;
	
	///<summary>Adapter supporting RuleML 0.8 Datalog Sublanguage.</summary>
	///<remarks>UTF-8 is the default encoding.</remarks>
	/// <author>David Dossot</author>
	public class RuleML08DatalogAdapter:RuleML086DatalogAdapter {
		
		/// <summary>
		/// Instantiates a RuleML 0.8 Datalog adapter for reading or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public RuleML08DatalogAdapter(Stream streamRuleML, FileAccess mode):base(streamRuleML, mode) {}

		/// <summary>
		/// Instantiates a RuleML 0.8 Datalog adapter for reading or writing to an URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read rules from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public RuleML08DatalogAdapter(string uriRuleML, FileAccess mode):base(uriRuleML, mode) {}

		// Protected/Private methods --------------------------------------------------------
		
		protected override string DatalogNamespaceURL {
			get {
				return String.Empty;
			}
		}
		
		protected override void ValidateRulebase() {
			if (!Navigator.Select(BuildXPathExpression("dl:rulebase")).MoveNext())
				throw new BREException("Can not locate the rulebase root.");
		}

		
		protected override void CreateDocumentElement() {
			Document.LoadXml("<rulebase/>");
		}
				
		protected override XmlReader GetXmlValidatingReader(string ignored) {
			return Xml.NewValidatingReader(Reader, ValidationType.DTD);
		}

	}
	
}
