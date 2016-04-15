namespace NxBRE.FlowEngine.IO {
	using System.Diagnostics;
	using System.Xml;
	using Util;
	
	/// <summary>
	/// Driver for loading rules files valid against businessRules.xsd (native NxBRE grammar)
	/// </summary>
	/// <author>David Dossot</author>
	/// <remarks>
	///  businessRules.xsd must be included in the assembly.
	/// </remarks>
	public sealed class BusinessRulesFileDriver:AbstractRulesDriver {
		public BusinessRulesFileDriver(string xmlFileUri):base(xmlFileUri) {}
		
		protected override XmlReader GetReader() {
			if (Logger.IsFlowEngineInformation) Logger.FlowEngineSource.TraceEvent(TraceEventType.Information, 0, "BusinessRulesFileDriver loading " + xmlSource);
			
			return GetXmlInputReader(xmlSource, "businessRules.xsd");
		}
	}
}
