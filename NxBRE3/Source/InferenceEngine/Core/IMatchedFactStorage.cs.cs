namespace org.nxbre.ie.core {
	using System;
	using System.Collections;
	
	using org.nxbre.ie.predicates;
	using org.nxbre.ie.rule;
	
	/// <summary>
	/// Defines an interface that all fact storage should implement to be usable by NxBRE.
	/// </summary>
	internal interface IMatchedFactStorage : ICloneable {
		void Add(Fact fact, Atom matchingAtom);
		void Remove(Fact fact);
		IEnumerator Select(Atom filter, ArrayList excludedHashcodes);
	}
	
}
