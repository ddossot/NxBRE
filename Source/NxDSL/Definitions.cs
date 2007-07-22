namespace NxDSL {
	
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Xml;
	using System.Xml.XPath;
	
	//FIXME make internal
	public class Definitions {
		
		private readonly IDictionary<Regex, string> definitions = new Dictionary<Regex, string>();
		
		public Definitions(string definitionFile) {
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
		
		public bool Contains(string statement) {
			return (GetMatchingRegex(statement) != null);
		}
		
		public string GetResolvedContent(string statement) {
			Regex regex = GetMatchingRegex(statement);
			
			Match match = regex.Match(statement);
		
			if (match != null) {
				GroupCollection groups = match.Groups;
				
				Object[] capturedValues = new Object[groups.Count-1];
				
				for(int groupIndex = 0; groupIndex<capturedValues.Length; groupIndex++) {
					capturedValues[groupIndex] = groups[groupIndex+1];
			    }
				
				return String.Format(definitions[regex], capturedValues);
			}
		
			throw new DslException("Impossible to find a definition for: " + statement);
		}
		
		private Regex GetMatchingRegex(string statement) {
			foreach(Regex regex in definitions.Keys) {
				if (regex.IsMatch(statement)) {
					return regex;
				}
			}
			
			return null;	
		}
		
	}
}
