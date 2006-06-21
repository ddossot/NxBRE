namespace org.nxbre
{
	using System;
	using System.Xml.XPath;
	
	using net.ideaity.util;
	using org.nxbre.rule;
	
	/// <summary>
	/// This interface defines the main method within a Business Rule Engine (BRE).
	/// </summary>
	/// <remarks>Obsolete, will soon be removed.</remarks>
	/// <see cref="org.nxbre.IFlowEngine"/>
	/// <author>Sloan Seaman</author>
	/// <author>David Dossot</author>
	/// <version>2.1</version>
	[Obsolete("org.nxbre.IBRE has been replaced by org.nxbre.IFlowEngine")]
	public interface IBRE : IFlowEngine {
		// see: IFlowEngine
	}
	
}
