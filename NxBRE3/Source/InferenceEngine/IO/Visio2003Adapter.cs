namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Xml;
	using System.Xml.Xsl;
	using System.Xml.XPath;

	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	///<summary>Adapter supporting Visio 2003 DatadiagramML Format (VDX file).</summary>
	/// <remarks>Only READ is supported!</remarks>
	/// <author>David Dossot</author>
	public class Visio2003Adapter:IRuleBaseAdapter {
		//TODO: target RuleML09 adapter
		private RuleML086NafDatalogAdapter rml086da;
		
		/// <summary>
		/// Instantiates a non-strict Visio 2003 DatadiagramML (VDX file) adapter for reading from a stream.
		/// It is possible to load only a selection of pages from the Visio file.
		/// </summary>
		/// <remarks>
		/// Only READ mode is supported!
		/// </remarks>
		/// <param name="streamVDX">The stream to read from.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="pageNames">An optional list of page names.</param>
		public Visio2003Adapter(Stream streamVDX, FileAccess mode, params string[] pageNames) {
			Init(new XPathDocument(streamVDX), mode, false, pageNames);
		}
		
		/// <summary>
		/// Instantiates a non-strict Visio 2003 DatadiagramML (VDX file) adapter for reading from an URI.
		/// It is possible to load only a selection of pages from the Visio file.
		/// </summary>
		/// <remarks>
		/// Only READ mode is supported!
		/// </remarks>
		/// <param name="uriVDX">The URI to read from.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="pageNames">An optional list of page names.</param>
		public Visio2003Adapter(string uriVDX, FileAccess mode, params string[] pageNames) {
			Init(new XPathDocument(uriVDX), mode, false, pageNames);
		}
		
		/// <summary>
		/// Instantiates a Visio 2003  (VDX file) adapter for reading from a stream.
		/// It is possible to load only a selection of pages from the Visio file.
		/// </summary>
		/// <remarks>
		/// Only READ mode is supported!
		/// </remarks>
		/// <param name="streamVDX">The stream to read from.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="strict">If true, the adapter expects the predicates to be strictly separated by commas.</param>
		/// <param name="pageNames">An optional list of page names.</param>
		public Visio2003Adapter(Stream streamVDX, FileAccess mode, bool strict, params string[] pageNames) {
			Init(new XPathDocument(streamVDX), mode, strict, pageNames);
		}
		
		/// <summary>
		/// Instantiates a Visio 2003 DatadiagramML (VDX file) adapter for reading from an URI.
		/// It is possible to load only a selection of pages from the Visio file.
		/// </summary>
		/// <remarks>
		/// Only READ mode is supported!
		/// </remarks>
		/// <param name="uriVDX">The URI to read from.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="strict">If true, the adapter expects the predicates to be strictly separated by commas.</param>
		/// <param name="pageNames">An optional list of page names.</param>
		public Visio2003Adapter(string uriVDX, FileAccess mode, bool strict, params string[] pageNames) {
			Init(new XPathDocument(uriVDX), mode, strict, pageNames);
		}

		/// <summary>
		/// Optional direction of the rulebase: forward, backward or bidirectional.
		/// </summary>
		public string Direction {
			get {
				return rml086da.Direction;
			}
			set {
				rml086da.Direction = value;
			}
		}
		
		/// <summary>
		/// Optional label of the rulebase.
		/// </summary>
		public string Label {
			get {
				return rml086da.Label;
			}
			set {
				rml086da.Label = value;
			}
		}
		
		/// <summary>
		/// Collection containing all the queries in the rulebase.
		/// </summary>
		public IList<Query> Queries {
			get {
				return rml086da.Queries;
			}
			set {
				rml086da.Queries = value;
			}
		}
		
		/// <summary>
		/// Collection containing all the implications in the rulebase.
		/// </summary>
		public IList<Implication> Implications {
			get {
				return rml086da.Implications;
			}
			set {
				rml086da.Implications = value;
			}
		}
		
		/// <summary>
		/// Collection containing all the facts in the rulebase.
		/// </summary>
		public IList<Fact> Facts {
			get {
				return rml086da.Facts;
			}
			set {
				rml086da.Facts = value;
			}
		}
		
		/// <summary>
		/// Returns an instance of the associated FlowEngineBinder or null.
		/// </summary>
		public IBinder Binder {
			set {
				rml086da.Binder = value;
			}
		}
		
		/// <summary>
		/// Called when the adapter is no longer used.
		/// </summary>
		public void Dispose() {
			rml086da.Dispose();
		}
		
		/// <summary>
		/// Returns a list of page names available in the specified Visio 2003 XML file.
		/// </summary>
		/// <param name="uriVDX">The URI to read from.</param>
		/// <returns>An array containing the Visio document page names.</returns>
		public static IList<string> GetPageNames(string uriVDX) {
			return GetPageNames(new XPathDocument(uriVDX));
		}
		
		/// <summary>
		/// Returns a list of page names available in the specified Visio 2003 XML file.
		/// </summary>
		/// <param name="streamVDX">The stream to read from.</param>
		/// <returns>An array containing the Visio document page names.</returns>
		public static IList<string> GetPageNames(Stream streamVDX) {
			return GetPageNames(new XPathDocument(streamVDX));
		}
		
		// Private methods --------------------------------------------------------
		
		private static IList<string> GetPageNames(XPathDocument xpDoc) {
			IList<string> result = new List<string>();
			
			XPathNavigator navigator = xpDoc.CreateNavigator();
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(navigator.NameTable);
			nsmgr.AddNamespace("vdx", "http://schemas.microsoft.com/visio/2003/core");
			XPathExpression xpe = navigator.Compile("vdx:VisioDocument/vdx:Pages/vdx:Page/@Name");
			xpe.SetContext(nsmgr);
			XPathNodeIterator pageNames = navigator.Select(xpe);
			while(pageNames.MoveNext()) result.Add(pageNames.Current.Value);
				
			return result;
		}
		
		private void Init(XPathDocument xpDoc, FileAccess mode, bool strict, params string[] pageNames) {
			if (mode != FileAccess.Read) throw new IOException(mode.ToString() + " not supported");

			XsltArgumentList xslArgs = new XsltArgumentList();
			if (pageNames.Length > 0) {
				string pageNameList = "|";
				foreach(string pageName in pageNames) pageNameList += (pageName + "|");
				xslArgs.AddParam("selected-pages", String.Empty, pageNameList);
			}
			    
			
			// load the appropriate XSL
			//TODO: try to refactor into one parametrable XSL
			XslCompiledTransform xslt = Xml.GetCachedCompiledTransform(strict?"strict-vdx2nxbre-ie.xsl":"vdx2nxbre-ie.xsl");
			
			MemoryStream stream = new MemoryStream();
			xslt.Transform(xpDoc, xslArgs, stream);
			stream.Seek(0, SeekOrigin.Begin);
			rml086da = new RuleML086NafDatalogAdapter(stream, FileAccess.Read);
		}
		
	}
}
