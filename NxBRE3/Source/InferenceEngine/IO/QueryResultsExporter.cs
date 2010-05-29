namespace NxBRE.InferenceEngine.IO
{
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// A utility class that allows exporting query results.
	/// </summary>
	public abstract class QueryResultsExporter
	{
		private QueryResultsExporter() {}
		
		public static void ExportResults(IList<IList<Fact>> results, IFactBaseAdapter factBaseAdapter) {
			ExportResults(results, factBaseAdapter, String.Empty);
		}
		
		public static void ExportResults(IList<IList<Fact>> results, IFactBaseAdapter factBaseAdapter, string factBaseLabel) {
			if (factBaseAdapter == null) {
				throw new ApplicationException("A non-null fact base adapter must be provided");
			}
			if (factBaseLabel == null) {
				throw new ApplicationException("A non-null fact base label must be provided");
			}
			
			List<Fact> flattenedResults = new List<Fact>();
			foreach(IList<Fact> row in results) {
				flattenedResults.AddRange(row);
			}
			
			using(factBaseAdapter) {
				factBaseAdapter.Direction = String.Empty;
				factBaseAdapter.Label = factBaseLabel;
				factBaseAdapter.Facts = flattenedResults;
			}
		}
	}
}
