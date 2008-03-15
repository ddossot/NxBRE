namespace NxBRE.FlowEngine.Core {
	using System;
	using System.Collections.Generic;
	using System.Xml.XPath;
	
	using NxBRE;

	/// <summary>
	/// A scheduler that processes Sets based on a desired outcome object ID in the RuleContext.
	/// </summary>
	internal sealed class BackwardChainer {
		private readonly IFlowEngine flowEngine;
		
		internal readonly IDictionary<string, IList<string>> setIdsFromTargetObjectId;
		
		internal readonly IDictionary<string, IList<string>> sourceObjectIdsFromSetId;
		
		public BackwardChainer(IFlowEngine flowEngine) {
			this.flowEngine = flowEngine;
			this.setIdsFromTargetObjectId = new Dictionary<string, IList<string>>();
			this.sourceObjectIdsFromSetId = new Dictionary<string, IList<string>>();
			
			ValidateXmlDocumentRules();
		}
		
		/// <summary>
		/// Executes set processing of sets based on a desired outcome (i.e. the id of context object that must be asserted).
		/// </summary>
		/// <param name="id"></param>
		/// <returns>the resolved object or null if it is not found</returns>
		public object Resolve(string targetObjectId) {
			return Resolve(targetObjectId, new Stack<string>());
		}
		
		internal object Resolve(string targetObjectId, Stack<string> resolutionPath) {
			resolutionPath.Push("?" + targetObjectId);
			
			// return immediately if already in context
			if (flowEngine.RuleContext.ResultsMap.Contains(targetObjectId)) {
				resolutionPath.Push("{RuleContext}");
				return flowEngine.RuleContext.GetObject(targetObjectId);
			}
			
			object result = ExploreBreadth(targetObjectId, resolutionPath);
			
			if (result == null) {
				result = ExplorePrecursors(targetObjectId, resolutionPath);
			}
			
			return result;
		}
		
		private object ExploreBreadth(string targetObjectId, Stack<string> resolutionPath) {
			foreach(string setId in GetSetIdsFromTargetObjectId(targetObjectId)) {
				resolutionPath.Push("{Set:" + setId + "}");
				
				if ((flowEngine.Process(setId)) && (flowEngine.RuleContext.ResultsMap.Contains(targetObjectId))) {
					return flowEngine.RuleContext.GetObject(targetObjectId);
				}
				
				resolutionPath.Pop();
			}
			
			return null;
		}
		
		private object ExplorePrecursors(string targetObjectId, Stack<string> resolutionPath) {
			foreach(string setId in GetSetIdsFromTargetObjectId(targetObjectId)) {
				// do not resolve a set that is already in the stack
				if (!resolutionPath.Contains("{Set:" + setId + "}")) {
					resolutionPath.Push("{Set:" + setId + "}");
					
					foreach(string objectId in GetSourceObjectIdsFromSetId(setId)) {
						Resolve(objectId, resolutionPath);
					}
					
					if ((flowEngine.Process(setId)) && (flowEngine.RuleContext.ResultsMap.Contains(targetObjectId))) {
						return flowEngine.RuleContext.GetObject(targetObjectId);
					}
					
					resolutionPath.Pop();
				} else {
					resolutionPath.Push("{Circularity}");
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Ensures rules are backward chainable.
		/// </summary>
		/// <param name="xmlRules"></param>
		internal void ValidateXmlDocumentRules() {
			// ensures all rules are in sets
			if (flowEngine.XmlDocumentRules.CreateNavigator().Select("//Rule[not(ancestor::Set) and not(starts-with(@id,'#'))] | //Retract[not(ancestor::Set)]").Count != 0) {
				throw new BREException("All rules must be defined in Sets to be backward chainable!");
			}
		}
		
		internal IList<string> GetSetIdsFromTargetObjectId(string objectId) {
			IList<string> setIds;
			
			if (setIdsFromTargetObjectId.TryGetValue(objectId, out setIds)) {
				return setIds;
			}
			else {
				setIds = new List<string>();
			
				XPathNodeIterator setIdAttributes = flowEngine.XmlDocumentRules.CreateNavigator().Select("//Set[.//Rule[@id='"
				                                                                                         + objectId
				                                                                                         + "']/@id | .//Retract[@id='"
				                                                                                         + objectId
				                                                                                         + "']]/@id");
				while(setIdAttributes.MoveNext()) {
					setIds.Add(setIdAttributes.Current.Value);
				}
				
				setIdsFromTargetObjectId.Add(objectId, setIds);
				return setIds;
			}
		}
		
		internal IList<string> GetSourceObjectIdsFromSetId(string setId) {
			IList<string> objectIds;
			
			if (sourceObjectIdsFromSetId.TryGetValue(setId, out objectIds)) {
				return objectIds;
			}
			else {
				objectIds = new List<string>();
				
				XPathNodeIterator setIdAttributes = flowEngine.XmlDocumentRules.CreateNavigator().Select("//Set[@id='"
				                                                                                         + setId
				                                                                                         + "']//Condition/Compare/@leftId | //Set[@id='"
				                                                                                         + setId
				                                                                                         + "']//Condition/Compare/@rightId");
				while(setIdAttributes.MoveNext()) {
					objectIds.Add(setIdAttributes.Current.Value);
				}
				
				sourceObjectIdsFromSetId.Add(setId, objectIds);
				return objectIds;
			}
		}
		
	}
}
