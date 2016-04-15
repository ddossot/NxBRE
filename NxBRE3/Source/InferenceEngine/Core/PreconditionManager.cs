using System.Linq;

namespace NxBRE.InferenceEngine.Core {
    using System.Collections.Generic;
	
	using Rules;

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

		    // parse all the implications and analyze precondition for errors
			foreach (var implication in IB.Where(implication => implication.Precondition != string.Empty))
			{
			    // get the precondition implication and die if it does not exist
			    var preconditionImplication = IB.Get(implication.Precondition);
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
			    var found = false;
			    foreach(var chain in Chains) {
			        var posImplication = chain.IndexOf(implication);
			        var posPreconditionImplication = chain.IndexOf(preconditionImplication);
						
			        if ((posImplication >= 0) && (posPreconditionImplication >= 0)) {
			            found = true;
			            break;							
			        }
			        if (posImplication >= 0) {
			            found = true;
			            chain.Insert(posImplication, preconditionImplication);
			            break;							
			        }
			        if (posPreconditionImplication < 0) continue;
			        found = true;
			        chain.Insert(posPreconditionImplication + 1, implication);
			        break;
			    }

			    if (found) continue;
			    {
			        IList<Implication> chain = new List<Implication>();
			        chain.Add(preconditionImplication);
			        chain.Add(implication);
			        Chains.Add(chain);
			    }
			}

		    foreach(var chain in Chains)
			{
			    var salience = chain.Count;
			    foreach(var implication in chain) {
					if (implication.Salience < salience)
						implication.Salience = salience;
					salience --;
				}
			}
		}
		
	}
	
}
