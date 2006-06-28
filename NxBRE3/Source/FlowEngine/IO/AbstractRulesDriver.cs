namespace NxBRE.FlowEngine.IO {
	using System;
	using System.IO;
	using System.Net;
	using System.Xml;
	using System.Xml.Schema;
	
	using net.ideaity.util.events;

	using NxBRE.FlowEngine;
	using NxBRE.Util;
	
	/// <summary>
	/// Driver for loading NxBRE rules from different sources.
	/// <see cref="NxBRE.FlowEngine.IO.IRulesDriver"/>
	/// </summary>
	/// <author>David Dossot</author>
	public abstract class AbstractRulesDriver:IRulesDriver {
		protected string xmlSource = null;
		private ILogDispatcher logDispatcher = null;

		///<summary>Builder pattern where the actual implementation is delegated to a descendant concrete class</summary>
		protected abstract XmlValidatingReader GetReader();
		
		
		public XmlValidatingReader GetXmlReader() {
			return GetReader();			
		}
		
		public ILogDispatcher LogDispatcher {
			get {
				return logDispatcher;
			}
			set {
				logDispatcher = value;
			}
		}
		
		///<summary>The XML rule source (either an URI or a string containing an XML fragment)</summary>
		/// <param name="xmlFileURI">The URI of the rule file</param>
		protected AbstractRulesDriver(string xmlSource) {
			if (xmlSource == null)
				throw new BRERuleFatalException("Null is not a valid XML source");

			this.xmlSource = xmlSource;
		}
		
		protected AbstractRulesDriver() {}
		
		protected XmlReader GetXmlInputReader(XmlTextReader xmlReader, string xsdResourceName) {
			XmlReader sourceReader;
			
			if (xsdResourceName != null) {
				// we validate against a well defined schema
				sourceReader = new XmlValidatingReader(xmlReader);
				((XmlValidatingReader) sourceReader).ValidationType = ValidationType.Schema;
				((XmlValidatingReader) sourceReader).Schemas.Add(XmlSchema.Read(Parameter.GetEmbeddedResourceStream(xsdResourceName),	null));
			}
			else {
				// it is easier to by default be lax if no internal XSD resource has been given
				sourceReader = xmlReader;
			}
			
			return sourceReader;
		}
		
		protected XmlReader GetXmlInputReader(string sourceURI, string xsdResourceName) {
			return GetXmlInputReader(new XmlTextReader(sourceURI), xsdResourceName);
		}
		
		protected XmlReader GetXmlInputReader(Stream sourceStream, string xsdResourceName) {
			return GetXmlInputReader(new XmlTextReader(sourceStream), xsdResourceName);
		}
	}
}

