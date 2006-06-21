namespace org.nxbre.ie.core {
	using System;
	using System.Collections;
	
	using org.nxbre.ie.rule;
	
	/// <summary>
	/// QueryResultSet is a read-only collection of Atom arrays.
	/// </summary>
	/// <description>
	/// Each entry corresponds to a result, and each result is an Atom array,
	/// which size depends on the number of atoms in the query.
	/// </description>
	/// <author>David Dossot</author>
	public class QueryResultSet:ReadOnlyCollectionBase {
		/// <summary>
		/// Instantiates a new QueryResultSet on the basis of an existing ArrayList of atoms.
		/// </summary>
		public QueryResultSet(ArrayList results) {
			InnerList.AddRange(results);
		}
		
		/// <summary>
		/// The Index of the QueryResultSet is an Array of facts that represents one QueryResultSet row.
		/// </summary>
		public Fact[] this[int index]  {
		  get  {
		    return((Fact[])InnerList[index]);
		  }
		}
	}
}
