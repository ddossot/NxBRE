namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections;
	using System.Data;
	
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;
	
	/// <summary>
	/// A fact storage class that uses a DataTable for storing facts.
	/// </summary>
	internal class DataTableMatchedFactStorage : IMatchedFactStorage {
		/// <summary>
		/// The initial size of the data tables
		/// </summary>
		/// <remarks>
		/// One data table is created per fact "signature" (type and number of predicates)
		/// </remarks>
		public static int DATA_TABLE_MINIMUM_CAPACITY = 1000; //TODO: make this a global parameter

		private readonly Atom template;
		private readonly DataTable table;
		
		public DataTableMatchedFactStorage(Atom template) {
			this.template = template;
			this.table = BuildDataTable(template);
		}
		
		private DataTableMatchedFactStorage(Atom template, DataTable table) {
			this.template = template;
			this.table = table;
		}
		
		public object Clone() {
			return new DataTableMatchedFactStorage(template, table.Copy());
		}
		
		public void Add(Fact fact, Atom matchingAtom) {
			try {
				DataRow row = table.NewRow();
			
				//using long mysteriously fails while string works...
				row["hashcode"] = fact.StringLongHashCode;
				row["fact"] = fact;
	     	
     		Fact resolved = Fact.Resolve(fact, matchingAtom);

     		for(int i=0; i<resolved.Members.Length; i++) {
     			object predicateValue = resolved.GetPredicateValue(i);
	     		row["predicate" + i] = Misc.GetStringHashcode(predicateValue);
	     	}
     	
				table.Rows.Add(row);
				
			} catch(System.Data.ConstraintException) {
				// ignore primary key violations: the adding attempt is simply ignored
			}
		}
		
		public void Remove(Fact fact) {
			foreach(DataRow row in table.Select("hashcode='" + fact.StringLongHashCode + "'"))
				table.Rows.Remove(row);
		}
		
		public IEnumerator Select(Atom filter, ArrayList excludedHashcodes) {
			string selectString = BuildSelectString(filter);

			if (selectString == String.Empty)
				// no select string: we must return everything
				return new DataRowCollectionEnumerator(table.Rows, excludedHashcodes);
			else
				return new DataRowArrayEnumerator(table.Select(selectString), excludedHashcodes);
		}
		
		// Private members ---------------------------------------------------------		

		private static DataTable BuildDataTable(Atom atom) {
		  DataTable table = new DataTable(atom.Signature);
		  table.CaseSensitive = true;
		  table.MinimumCapacity = DATA_TABLE_MINIMUM_CAPACITY;
		  
		  // prepare the PK array
		  DataColumn[] keys = new DataColumn[1 + atom.Members.Length];

		  // add the hashcode
		  DataColumn dc = new DataColumn();
	    dc.DataType = typeof(string);
	    dc.ColumnName = "hashcode";
	    dc.AutoIncrement = false;
	    dc.ReadOnly = true;
	    dc.Unique = false;
	    table.Columns.Add(dc);
	    keys[0] = dc;
	    	    
	    // add the fact reference
			dc = new DataColumn();
	    dc.DataType = typeof(Fact);
	    dc.ColumnName = "fact";
	    dc.AutoIncrement = false;
	    dc.ReadOnly = true;
	    dc.Unique = false;
	    table.Columns.Add(dc);		 
		    
	    // add the other string columns
	    for (int i=0; i<atom.Members.Length; i++) {
				dc = new DataColumn();
		    dc.DataType = typeof(string);
		    dc.ColumnName = "predicate" + i;
		    dc.AutoIncrement = false;
		    dc.ReadOnly = true;
		    dc.Unique = false;
		    table.Columns.Add(dc);		    	
	    	keys[i+1] = dc;
	    }
	    
	    // set the PK
	    table.PrimaryKey = keys;
			return table;
		}
		
		private string EscapeStringCriteria(string criteria) {
			return criteria.Replace("'", "''");
		}
		
		private string BuildSelectString(Atom atom) {
			string query = String.Empty;
			
			for (int i=0; i<atom.Members.Length; i++) {
				if (atom.Members[i] is Individual) {
					string predicateValue = Misc.GetStringHashcode(atom.PredicateValues[i]);
					if (predicateValue.IndexOf("'") > -1)	predicateValue = EscapeStringCriteria(predicateValue);
					query = AppendCriteria(query, "predicate" + i + "='" + predicateValue + "'");
				}
			}
			
			return query;
		}
	
		private string AppendCriteria(string query, string criteria) {
			return query + ((query != String.Empty)?" AND ":String.Empty) + criteria;
		}
		
		private class DataRowArrayEnumerator : AbstractFactEnumerator {
			private readonly DataRow[] dataRowArray;
			
			public DataRowArrayEnumerator(DataRow[] dataRowArray, IList excludedHashCodes):base(excludedHashCodes) {
				this.dataRowArray = dataRowArray;
			}
			
			protected override bool DoMoveNext() {
				if (position >= dataRowArray.Length) return false;
				currentFact = (Fact)dataRowArray[position]["fact"];
				return true;
			}
		}
		
		private class DataRowCollectionEnumerator : AbstractFactEnumerator {
			private readonly DataRowCollection dataRowCollection;
			
			public DataRowCollectionEnumerator(DataRowCollection dataRowCollection, IList excludedHashCodes):base(excludedHashCodes) {
				this.dataRowCollection = dataRowCollection;
			}
			
			protected override bool DoMoveNext() {
				if (position >= dataRowCollection.Count) return false;
				currentFact = (Fact)dataRowCollection[position]["fact"];
				return true;
			}
		}
		
	}

}
