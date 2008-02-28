namespace NxBRE.FlowEngine.Core {
	using System;
	using System.Collections.Generic;
	using System.Xml.XPath;

	/// <summary>
	/// TODO Description of BackwardChainer.
	/// Schedules processing of sets based on a desired outcome (i.e. the id of context object that must be asserted).
	/// </summary>
	internal sealed class BackwardChainer {
		private readonly IFlowEngine flowEngine;
		
		public BackwardChainer(IFlowEngine flowEngine) {
			this.flowEngine = flowEngine;
			
			// TODO ensure the loaded rules are backwards chainable (no rules outside of sets for ex., no delegates?)
		}
		
		public object Resolve(string id) {
			// FIXME implement process all the scheduled sets until the sought id is found in context
			return null;
		}
		
		internal IList<string> GetSetIdsFromTargetObjectId(string id) {
			IList<string> ids = new List<string>();
			
			XPathNodeIterator setIdAttributes = flowEngine.XmlDocumentRules.CreateNavigator().Select("//Set[.//Logic//Do//Rule[@id='"
			                                                                                         + id
			                                                                                         + "']/@id | .//Logic//Else//Rule[@id='"
			                                                                                         + id
			                                                                                         + "']]/@id");
			while(setIdAttributes.MoveNext()) {
				ids.Add(setIdAttributes.Current.Value);
			}
			
			return ids;
		}
		
		internal IList<string> GetSourceObjectIdsFromSetId(string id) {
			IList<string> ids = new List<string>();
			
			XPathNodeIterator setIdAttributes = flowEngine.XmlDocumentRules.CreateNavigator().Select("//Set[@id='"
			                                                                                         + id
			                                                                                         + "']//Condition/Compare/@leftId | //Set[@id='"
			                                                                                         + id
			                                                                                         + "']//Condition/Compare/@rightId");
			while(setIdAttributes.MoveNext()) {
				ids.Add(setIdAttributes.Current.Value);
			}
			
			return ids;
		}
		
	}
}
