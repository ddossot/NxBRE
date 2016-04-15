using System.Linq;

namespace NxBRE.InferenceEngine.Core {
	using System.Collections.Generic;
	
	using Rules;

	///<summary>Abstract class for managing chains of implications engaged in relationships
	/// like in mutex or pre-conditions.</summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal abstract class AbstractChainManager {
		private readonly string type;
		private IList<IList<Implication>> chains;
		private ImplicationBase ib;

		protected IList<IList<Implication>> Chains {
			get {
				return chains;
			}
			set {
				chains = value;
			}
		}

		protected ImplicationBase IB {
			get {
				return ib;
			}
		}

		protected AbstractChainManager(string type, ImplicationBase ib) {
			this.type = type;
			this.ib = ib;
		}

		public override string ToString()
		{
			return chains.Aggregate("", (current, chain) => current + (ChainToString(chain) + "\n"));
		}

		private string ChainToString(IList<Implication> chain) {
			var result = type + " chain: ";
			return chain.Aggregate(result, (current, implication) => current + (implication.Label + "[Weight: " + implication.Weight + "], "));
		}
		
	}
	
}
