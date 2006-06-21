namespace org.nxbre.ri.drivers {
	using System.Xml;
	using net.ideaity.util.events;
	
	/// <summary>
	/// Interface that any rules driver must implement.
	/// A rule driver is responsible for fetching rules data from a certain source
	/// and returning an XmlValidatingReader on it.
	/// </summary>
	/// <author>David Dossot</author>
	public interface IRulesDriver {
		///<summary>Member to retrieve a XMLValidatingReader</summary>
		XmlValidatingReader GetXmlReader();

		///<summary>Read/Write Property to handle the LogDispatcher</summary>
		ILogDispatcher LogDispatcher {
			get;
			set;
		}

	}
}
