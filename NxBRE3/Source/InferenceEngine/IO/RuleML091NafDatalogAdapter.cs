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
	public class RuleML091NafDatalogAdapter:RuleML09NafDatalogAdapter {
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

	}
	
}
