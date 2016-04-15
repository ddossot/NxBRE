namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.IO;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.XPath;
	using System.Xml.Xsl;
	
	using NxBRE.InferenceEngine.IO.Hrf086;
	using NxBRE.Util;

	///<summary>Adapter supporting NxBRE-IE Human Readable Format (HRF) version 0.86.
	/// HRF is parsed and transformed to RuleML NafDatalog 0.86
	/// </summary>
	///<remarks>US-ASCII is the only supported encoding.
	/// Var-less queries and implications are not supported.
	/// To be used in lieu of the previous (experimental) version HRF 0.8</remarks>
	/// <author>David Dossot</author>
	/// <author>Ron Evans</author>
	public class HRF086Adapter:RuleML086NafDatalogAdapter {
		string tempfileName;
		Stream resultStream;
		
		public HRF086Adapter(Stream streamHRF, FileAccess mode):base(streamHRF, mode) {}

		public HRF086Adapter(string uriHRF, FileAccess mode):base(uriHRF, mode) {}

		/// <summary>
		/// Called when the adapter is no longer used.
		/// </summary>
		public override void Dispose() {
			base.Dispose();
			
			if (tempfileName != null) {
				XslCompiledTransform xslt = Xml.GetCachedCompiledTransform("ruleml-nafdatalog-0_86-2hrf.xsl");
				
				XmlTextReader reader = new XmlTextReader(tempfileName);
				xslt.Transform(reader, new XsltArgumentList(), resultStream);
				reader.Close();
				
				resultStream.Flush();
				resultStream.Close();
			}
			
		}

		// Protected/Private methods --------------------------------------------------------
		
		protected override void Init(Stream streamHRF, string uriHRF, FileAccess mode) {
			tempfileName = null;
			
			if (mode == FileAccess.Read) {
				MemoryStream ms = new MemoryStream();
				Scanner s = new Scanner();
				
				if (streamHRF != null) s.Init(streamHRF);
				else s.Init(uriHRF);
				
				Parser p = new Parser(s);
				p.Parse(ms);
				
				if (p.ParserErrors.List.Count >0)
					throw new BREException(p.ParserErrors.List.Count +
					             		       " HRF Parser error(s), top one is: " +
							                   p.ParserErrors.List[0]);
				
				ms.Seek(0, SeekOrigin.Begin);
				base.Init(ms, null, FileAccess.Read);
			}
			else {
				if (streamHRF != null) resultStream = streamHRF;
				else resultStream = new FileStream(uriHRF, FileMode.Create);
					
				tempfileName = Path.GetTempFileName();
				base.Init(null, tempfileName, FileAccess.Write);
			}
			
		}
		
	}
	
}
