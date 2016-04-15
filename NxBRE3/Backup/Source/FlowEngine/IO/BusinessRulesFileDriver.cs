namespace NxBRE.FlowEngine.IO {
	using System;
	using System.Diagnostics;
	using System.Xml;
	
	using NxBRE.FlowEngine;
	using NxBRE.Util;
	
	/// <summary>
	/// Driver for loading rules files valid against businessRules.xsd (native NxBRE grammar)
	/// </summary>
	/// <author>David Dossot</author>
	/// <remarks>
	///  businessRules.xsd must be included in the assembly.
	/// </remarks>
	public sealed class BusinessRulesFileDriver:AbstractRulesDriver {
		public BusinessRulesFileDriver(string xmlFileURI):base(xmlFileURI) {}
		
		protected override XmlReader GetReader() {
			if (Logger.IsFlowEngineInformation) Logger.FlowEngineSource.TraceEvent(TraceEventType.Information, 0, "BusinessRulesFileDriver loading " + xmlSource);
			
			return GetXmlInputReader(xmlSource, "businessRules.xsd");
		}
	}
}
