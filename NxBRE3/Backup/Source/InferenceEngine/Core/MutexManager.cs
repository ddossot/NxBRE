namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;
	
	///<summary>Class for managing chains of implications engaged in mutex relationships.
	/// It analyzes the consistency of the mutual exclusion references in implications.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class MutexManager : AbstractChainManager {
		
		public MutexManager(ImplicationBase ib):base("Mutex", ib) {}

		public void AnalyzeImplications() {
			Chains = new List<IList<Implication>>();
			Implication mutex;
			bool found;
			
			// parse all the implications and store in same chains the ones that
			// are linked together by mutex
			foreach(Implication implication in IB)
				if (implication.Mutex != String.Empty) {
					// get the mutexed implication and die if it does not exist
					mutex = IB.Get(implication.Mutex);
					if (mutex == null) throw new BREException("Implication " +
					                                          implication.Label +
					                                          " tries to Mutex the missing implication " +
					                                          implication.Mutex);
					
					// try first to find if the current implication or its mutexed one is somewhere
					// in a chain, else create a new chain.
					found = false;
					foreach(IList<Implication> chain in Chains)
						if ((chain.Contains(implication)) || (chain.Contains(mutex))) {
							found = true;
							if (!chain.Contains(implication)) chain.Add(implication);
							if (!chain.Contains(mutex)) chain.Add(mutex);
							break;
						}
					
					if (!found) {
						IList<Implication> chain = new List<Implication>();
						chain.Add(implication);
						chain.Add(mutex);
						Chains.Add(chain);
					}
				}

			// parse a second time to establish all cross references
			// are linked together by mutex
			foreach(Implication implication in IB) 
				foreach(IList<Implication> chain in Chains) 
					if (chain.Contains(implication)) {
						implication.MutexChain = chain;
						break;
					}
		}
		
	}
	
}
