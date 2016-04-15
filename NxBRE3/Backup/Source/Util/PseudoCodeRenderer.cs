namespace NxBRE.Util
{
	using System;
	using System.IO;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.XPath;
	using System.Xml.Xsl;
	
	using NxBRE.FlowEngine.IO;
	using NxBRE.Util;
	
	/// <summary>An helper class for rendering XBusinessRules in pseudo-code HTML.
	/// The class can render the rules body, an index on these rules and a frameset
	/// to encapsulate them both.
	/// </summary>
	/// <author>David Dossot</author>
	public class PseudoCodeRenderer {
		
		private string ruleFileURI;
		
		///<summary>Instantiate a new renderer for a specific rule file.</summary>
		/// <param name="ruleFileURI">The URI of a rule file that validates on xBusinessRules.xsd</param>
		public PseudoCodeRenderer(string ruleFileURI)
		{
			this.ruleFileURI = ruleFileURI;
		}
		
		private void Render(Stream stream, string xslResourceName, XsltArgumentList args) {
			XslCompiledTransform xslt = Xml.GetCachedCompiledTransform(xslResourceName);
			XmlReader reader = Xml.NewValidatingReader(new XmlTextReader(ruleFileURI), ValidationType.Schema, "xBusinessRules.xsd");
			xslt.Transform(reader, args, stream);
			reader.Close();
		}
		
		/// <summary>Renders the body of the rule file in pseudo-code HTML.</summary>
		/// <param name="fileName">Output file name to generate.</param>
		/// <param name="title">Title for the HTML page.
		/// If null, the default value defined in pseudocode_body.xsl will be used.</param>
		public void RenderBody(string fileName, string title) {
			FileStream fs = new FileStream(fileName, FileMode.Create);
			RenderBody(fs, title);
			fs.Flush();
			fs.Close();
		}
		
		/// <summary>Renders the body of the rule file in pseudo-code HTML.</summary>
		/// <param name="stream">Stream into which the generated page will be writen to.</param>
		/// <param name="title">Title for the HTML page.
		/// If null, the default value defined in pseudocode_body.xsl will be used.</param>
		public void RenderBody(Stream stream, string title) {
			XsltArgumentList args = new XsltArgumentList();
			if (title != null) args.AddParam("title", "", title);
			Render(stream, "pseudocode_body.xsl", args);
		}
		
		/// <summary>Renders the body of the rule file in pseudo-code HTML.</summary>
		/// <param name="fileName">Output file name to generate.</param>
		/// <description>Default values are used in this shorter method call.</description>
		public void RenderBody(string fileName) {
			RenderBody(fileName, null);
		}
		
		/// <summary>Renders the body of the rule file in pseudo-code HTML.</summary>
		/// <param name="stream">Stream into which the generated page will be writen to.</param>
		/// <description>Default values are used in this shorter method call.</description>
		public void RenderBody(Stream stream) {
			RenderBody(stream, null);
		}
		
		/// <summary>Renders the index of the rule file in pseudo-code HTML.</summary>
		/// <param name="fileName">Output file name to generate.</param>
		/// <param name="title">Title for the HTML page.</param>
		/// <param name="bodyURI">URI where the pseudo-code rules body is stored.
		/// It will be used for generating a correct "a href" HTML tag.
		/// </param>
		/// <description>If title or bodyURI are null, the default value defined in pseudocode_index.xsl will be used.</description>
		public void RenderIndex(string fileName, string title, string bodyURI) {
			FileStream fs = new FileStream(fileName, FileMode.Create);
			RenderIndex(fs, title, bodyURI);
			fs.Flush();
			fs.Close();
		}
		
		/// <summary>Renders the index of the rule file in pseudo-code HTML.</summary>
		/// <param name="stream">Stream into which the generated page will be writen to.</param>
		/// <param name="title">Title for the HTML page.</param>
		/// <param name="bodyURI">URI where the pseudo-code rules body is stored.
		/// It will be used for generating a correct "a href" HTML tag.
		/// </param>
		/// <description>If title or bodyURI are null, the default value defined in pseudocode_index.xsl will be used.</description>
		public void RenderIndex(Stream stream, string title, string bodyURI) {
			XsltArgumentList args = new XsltArgumentList();
			if (title != null) args.AddParam("title", null, title);
			if (bodyURI != null) args.AddParam("bodyfile", "", bodyURI);
			Render(stream, "pseudocode_index.xsl", args);
		}
		
		/// <summary>Renders the index of the rule file in pseudo-code HTML.</summary>
		/// <param name="fileName">Output file name to generate.</param>
		/// <description>Default values are used in this shorter method call.</description>
		public void RenderIndex(string fileName) {
			RenderIndex(fileName, null, null);
		}
		
		/// <summary>Renders the index of the rule file in pseudo-code HTML.</summary>
		/// <param name="stream">Stream into which the generated page will be writen to.</param>
		/// <description>Default values are used in this shorter method call.</description>
		public void RenderIndex(Stream stream) {
			RenderIndex(stream, null, null);
		}
		
		/// <summary>Renders a frameset for encapsulating pseudo-code HTML rule body and index.</summary>
		/// <param name="fileName">Output file name to generate.</param>
		/// <param name="title">Title for the HTML page.</param>
		/// <param name="indexURI">URI where the pseudo-code rules index is stored.</param>
		/// <param name="bodyURI">URI where the pseudo-code rules body is stored.</param>
		/// <description>If title, indexURI or bodyURI are null, the default value defined in pseudocode_frames.xsl will be used.</description>
		public void RenderFrameset(string fileName, string title, string indexURI, string bodyURI) {
			FileStream fs = new FileStream(fileName, FileMode.Create);
			RenderFrameset(fs, title, indexURI, bodyURI);
			fs.Flush();
			fs.Close();
		}
		
		/// <summary>Renders a frameset for encapsulating pseudo-code HTML rule body and index.</summary>
		/// <param name="stream">Stream into which the generated page will be writen to.</param>
		/// <param name="title">Title for the HTML page.</param>
		/// <param name="indexURI">URI where the pseudo-code rules index is stored.</param>
		/// <param name="bodyURI">URI where the pseudo-code rules body is stored.</param>
		/// <description>If title, indexURI or bodyURI are null, the default value defined in pseudocode_frames.xsl will be used.</description>
		public void RenderFrameset(Stream stream, string title, string indexURI, string bodyURI) {
			XsltArgumentList args = new XsltArgumentList();
			if (title != null) args.AddParam("title", "", title);
			if (indexURI != null) args.AddParam("indexfile", "", indexURI);
			if (bodyURI != null) args.AddParam("bodyfile", "", bodyURI);
			Render(stream, "pseudocode_frames.xsl", args);
		}
		
		/// <summary>Renders a frameset for encapsulating pseudo-code HTML rule body and index.</summary>
		/// <param name="fileName">Output file name to generate.</param>
		/// <description>Default values are used in this shorter method call.</description>
		public void RenderFrameset(string fileName) {
			RenderFrameset(fileName, null, null, null);
		}
		
		/// <summary>Renders a frameset for encapsulating pseudo-code HTML rule body and index.</summary>
		/// <param name="stream">Stream into which the generated page will be writen to.</param>
		/// <description>Default values are used in this shorter method call.</description>
		public void RenderFrameset(Stream stream) {
			RenderFrameset(stream, null, null, null);
		}
		
	}
}
