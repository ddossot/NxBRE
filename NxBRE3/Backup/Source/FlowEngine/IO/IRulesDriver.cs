namespace NxBRE.FlowEngine.IO {
	using System.Xml;
	
	/// <summary>
	/// Interface that any rules driver must implement.
	/// A rule driver is responsible for fetching rules data from a certain source
	/// and returning an XmlReader on it.
	/// </summary>
	/// <author>David Dossot</author>
	public interface IRulesDriver {
		///<summary>Member to retrieve a XmlReader</summary>
		XmlReader GetXmlReader();
	}
}
