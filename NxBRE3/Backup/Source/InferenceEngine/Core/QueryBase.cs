namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;
	
	/// <summary>
	/// The QueryBase is the repository of queries for the inference engine.
	/// Anonymous (unlabeled) queries can be accessed by their index in the query base.
	/// It is recommanded to name queries for obvious usability reasons.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class QueryBase:IEnumerable<Query> {
		private IList<Query> queryDefs;
		
		public int Count {
			get {
				return queryDefs.Count;
			}
		}
		
		public QueryBase() {
			queryDefs = new List<Query>();
		}
		
		public void Add(Query query) {
			if (queryDefs.Contains(query)) throw new BREException("The knowledge base already contains a similar query: " + query);
			queryDefs.Add(query);
		}
		
		public IEnumerator<Query> GetEnumerator() {
			return queryDefs.GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return queryDefs.GetEnumerator();
		}
		
		public Query Get(int queryIndex) {
			if ((queryIndex < 0) || (queryIndex >= Count))
				throw new IndexOutOfRangeException("The knowledge base can not find a query definition for index="+queryIndex);

			return (Query)queryDefs[queryIndex];
		}

		public Query Get(string queryLabel) {
			foreach(Query query in queryDefs)
				if (query.Label == queryLabel)
					return query;

			return null;
		}
		
		public void Remove(Query query) {
			queryDefs.Remove(query);
		}
		
		public override string ToString() {
			return Misc.IListToString(queryDefs);
		}
		
	}
}
