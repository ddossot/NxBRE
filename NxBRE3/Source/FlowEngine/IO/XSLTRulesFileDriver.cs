namespace NxBRE.FlowEngine.IO {
	using System;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.Xsl;
	using System.Xml.XPath;
	
	using net.ideaity.util.events;
	
	using NxBRE.FlowEngine;
	using NxBRE.Util;
	
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
		protected XslCompiledTransform xslt = null;
		protected string inputXMLSchema = null;
		
		protected XSLTRulesFileDriver(string xmlFileURI):base(xmlFileURI) {}
		
		public XSLTRulesFileDriver(string xmlFileURI, XslCompiledTransform xslt):base(xmlFileURI) {
			if (xslt == null)
				throw new BRERuleFatalException("Null is not a valid XslTransform");

			this.xslt = xslt;
		}	
		
		public XSLTRulesFileDriver(string xmlFileURI, string xslFileURI):base(xmlFileURI) {
			if (xslFileURI == null)
				throw new BRERuleFatalException("Null is not a valid XSL File URI");

			this.xslFileURI = xslFileURI;
		}
		
		private XslCompiledTransform GetXSLT() {
			if (xslt == null) {
				if (LogDispatcher != null)
					LogDispatcher.DispatchLog("XSLTRulesFileDriver loading "+xslFileURI, LogEventImpl.INFO);
				
				xslt = new XslCompiledTransform();
				xslt.Load(xslFileURI);
			}
			
			return xslt;
		}
		
		protected override XmlReader GetReader() {
			if (LogDispatcher != null) LogDispatcher.DispatchLog("XSLTRulesFileDriver loading " + xmlSource, LogEventImpl.INFO);
			
			XmlReader fileReader = GetXmlInputReader(xmlSource, inputXMLSchema);

	  	MemoryStream stream = new MemoryStream();
	  	GetXSLT().Transform(new XPathDocument(fileReader), null, stream);
			fileReader.Close();
	  	stream.Seek(0, SeekOrigin.Begin);
			
			return GetXmlInputReader(stream, "businessrules.xsd");
		}
	}
}
