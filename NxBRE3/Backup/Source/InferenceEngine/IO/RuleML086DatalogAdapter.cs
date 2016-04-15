namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.XPath;
	using System.Xml.Schema;
	
	using NxBRE.Util;
	
	///<summary>Adapter supporting RuleML 0.86 Datalog Sublanguage.</summary>
	///<remarks>UTF-8 is the default encoding.</remarks>
	/// <author>David Dossot</author>
	public class RuleML086DatalogAdapter:RuleML086NafDatalogAdapter {
		
		/// <summary>
		/// Instantiates a RuleML 0.86 Datalog adapter for reading or writing to a stream.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public RuleML086DatalogAdapter(Stream streamRuleML, FileAccess mode):base(streamRuleML, mode) {}

		/// <summary>
		/// Instantiates a RuleML 0.86 Datalog adapter for reading or writing to an URI.
		/// </summary>
		/// <param name="uriRuleML">The URI to read rules from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public RuleML086DatalogAdapter(string uriRuleML, FileAccess mode):base(uriRuleML, mode) {}

		// Protected/Private methods --------------------------------------------------------
		
		protected override string DatalogSchema {
			get {
				return "ruleml-0_86-datalog.xsd";
			}
		}
		
	}
	
}
