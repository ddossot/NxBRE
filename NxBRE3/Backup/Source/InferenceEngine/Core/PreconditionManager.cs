namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;

	///<summary>Class for managing chains of implications engaged in pre-condition relationships.
	/// It analyzes the consistency of the pre-condition references in implications.
	/// It also calculate the salience of each implication.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class PreconditionManager : AbstractChainManager {
		
		public PreconditionManager(ImplicationBase ib):base("Precondition", ib) {}

		public void AnalyzeImplications() {
			Chains = new List<IList<Implication>>();
			Implication preconditionImplication;
			bool found;
			int posImplication;
			int posPreconditionImplication;

			// parse all the implications and analyze precondition for errors
			foreach(Implication implication in IB)
				if (implication.Precondition != String.Empty) {
					// get the precondition implication and die if it does not exist
					preconditionImplication = IB.Get(implication.Precondition);
					if (preconditionImplication == null)
						throw new BREException("Implication " +
	                                  implication.Label +
	                                  " is preconditionned by the missing implication " +
	                                  implication.Precondition);
					
					// check if the current implication and its precondition are not involved into
					// a common mutex chain, and die if it happends
					if ((implication.MutexChain != null) && (implication.MutexChain.Contains(preconditionImplication)))
						throw new BREException("Implication " +
	                                  implication.Label +
	                                  " is preconditionned by the mutexed implication " +
	                                  preconditionImplication.Label);
					
					// assign the precondition implication to save time at execution
					implication.PreconditionImplication = preconditionImplication;
					
					// try first to find if the current implication or its precondition one is somewhere
					// in a chain, else create a new chain.
					found = false;
					foreach(IList<Implication> chain in Chains) {
						posImplication = chain.IndexOf(implication);
						posPreconditionImplication = chain.IndexOf(preconditionImplication);
						
						if ((posImplication >= 0) && (posPreconditionImplication >= 0)) {
							found = true;
							break;							
						}
						else if (posImplication >= 0) {
							found = true;
							chain.Insert(posImplication, preconditionImplication);
							break;							
						}
						else if (posPreconditionImplication >= 0) {
							found = true;
							chain.Insert(posPreconditionImplication + 1, implication);
							break;							
						}
					}
					
					if (!found) {
						IList<Implication> chain = new List<Implication>();
						chain.Add(preconditionImplication);
						chain.Add(implication);
						Chains.Add(chain);
					}
				}

			int salience;
			foreach(IList<Implication> chain in Chains) {
				salience = chain.Count;
				foreach(Implication implication in chain) {
					if (implication.Salience < salience)
						implication.Salience = salience;
					salience --;
				}
			}
		}
		
	}
	
}
