namespace NxBRE.InferenceEngine {
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// IQueryResultSet is a read-only collection of IFact arrays.
	/// </summary>
	/// <description>
	/// Each entry corresponds to a result, and each result is an Atom array,
	/// which size depends on the number of atoms in the query.
	/// </description>
	/// <author>David Dossot</author>
	public interface IQueryResultSet:ICollection, IEnumerable {
		/// <summary>
		/// The Index of the IQueryResultSet is an Array of facts that represents one IQueryResultSet row.
		/// </summary>
		//FIXME: return IFact, using Rules should not be needed
		Fact[] this[int index]  {
		  get;
		}
		
	}
}
