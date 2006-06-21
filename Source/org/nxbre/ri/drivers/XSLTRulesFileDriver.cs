namespace org.nxbre.ri.drivers {
	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.Xsl;
	using System.Xml.XPath;
	
	using net.ideaity.util.events;
	
	using org.nxbre.rule;
	using org.nxbre.util;
	
	/// <summary>
	/// Driver for loading rules, which first executes an XSLT.
	/// The resulting XML document must be valid against businessRules.xsd
	/// </summary>
	/// <author>David Dossot</author>
	/// <remarks>
	///  businessRules.xsd must be included in the assembly.
	/// </remarks>
	public class XSLTRulesFileDriver:AbstractRulesDriver {

		private string xslFileURI = null;
		protected XslTransform xslt = null;
		protected string inputXMLSchema = null;
		
		protected XSLTRulesFileDriver(string xmlFileURI):base(xmlFileURI) {}
		
		public XSLTRulesFileDriver(string xmlFileURI, XslTransform xslt):base(xmlFileURI) {
			if (xslt == null)
				throw new BRERuleFatalException("Null is not a valid XslTransform");

			this.xslt = xslt;
		}	
		
		public XSLTRulesFileDriver(string xmlFileURI, string xslFileURI):base(xmlFileURI) {
			if (xslFileURI == null)
				throw new BRERuleFatalException("Null is not a valid XSL File URI");

			this.xslFileURI = xslFileURI;
		}
		
		private XslTransform GetXSLT() {
			if (xslt == null) {
				if (LogDispatcher != null)
					LogDispatcher.DispatchLog("XSLTRulesFileDriver loading "+xslFileURI, LogEventImpl.INFO);
				
				xslt = new XslTransform();
				xslt.Load(xslFileURI);
			}
			
			return xslt;
		}
		
		protected override XmlValidatingReader GetReader() {
			if (LogDispatcher != null)
				LogDispatcher.DispatchLog("XSLTRulesFileDriver loading "+xmlSource, LogEventImpl.INFO);
			
			XmlReader fileReader = GetXmlInputReader(xmlSource, inputXMLSchema);

	  	MemoryStream stream = new MemoryStream();
	  	GetXSLT().Transform(new XPathDocument(fileReader), null, stream, null);
			fileReader.Close();
	  	stream.Seek(0, SeekOrigin.Begin);
			XmlSchemaCollection schemas = new XmlSchemaCollection();
			
	    XmlValidatingReader streamReader = (XmlValidatingReader) GetXmlInputReader(stream,
	                                                                               Parameter.GetString("businessrules.xsd", "businessRules.xsd"));
			return streamReader;
		}
	}
}
