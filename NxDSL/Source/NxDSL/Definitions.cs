namespace NxDSL {
	
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Xml;
	using System.Xml.XPath;
	
	public class Definitions {
		
		private readonly IDictionary<Regex, String> definitions;
		
		public Definitions(String definitionFile) {
			definitions = new Dictionary<Regex, String>();
			
			using (XmlReader reader = XmlReader.Create(definitionFile)) {
				XPathNavigator navigator = new XPathDocument(reader).CreateNavigator();
				
				XPathNodeIterator atomPatterns = navigator.SelectDescendants("AtomPattern", "", false);
				
				while (atomPatterns.MoveNext()) {
					XPathNavigator atomPattern = atomPatterns.Current;
					Regex regex = new Regex("^\\s*" + atomPattern.GetAttribute("regex", "") + "$", RegexOptions.Compiled);
					
					definitions.Add(regex, atomPattern.InnerXml);
				}
			}

		}
		
		public bool Contains(String statement) {
			foreach(Regex regex in definitions.Keys) {
				if (regex.IsMatch(statement)) {
					return true;
				}
			}
			
			return false;
		}
		
	}
}
