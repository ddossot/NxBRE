namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// The ImplicationBase is the repository of implications for the inference engine.
	/// Implications are organized in sub-collections grouped by atom types for performance reasons.
	/// Each implication "listens" to specific facts, based on their signatures.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class ImplicationBase:IEnumerable<Implication> {
		private IList<Implication> implications;
		private IDictionary<string, IList<Implication>> implicationsMap;

		public int Count {
			get {
				return implications.Count;
			}
		}

		public ImplicationBase() {
			implications = new List<Implication>();
			implicationsMap = new Dictionary<string, IList<Implication>>();
		}

		public void Add(Implication implication) {
			int indexOfDuplicate = implications.IndexOf(implication);
			
			if (indexOfDuplicate >= 0)
				throw new BREException("When adding: " + implication +
				                       " the knowledge base detected a duplicated with: " + implications[indexOfDuplicate]);
			
			implications.Add(implication);
			
			IList<Implication> typeListeners;
			
			foreach(Atom atom in implication.AtomGroup.AllAtoms) {
				if (!implicationsMap.ContainsKey(atom.Signature)) {
					typeListeners = new List<Implication>();
					implicationsMap.Add(atom.Signature, typeListeners);
				}
				else {
					typeListeners = implicationsMap[atom.Signature];
				}
				
				if (!typeListeners.Contains(implication))	typeListeners.Add(implication);
			}
		}
		
		public IEnumerator<Implication> GetEnumerator() {
			return implications.GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return implications.GetEnumerator();
		}
		
		public Implication Get(string implicationLabel) {
			foreach(Implication implication in implications)
				if (implication.Label == implicationLabel)
					return implication;
			
			return null;
		}
		
		/// <summary>
		/// Lists all the implications that are children of the provided one on the basis of pre-condition relationship ;
		/// ie all the implications that will fire only if the passed one is positive.
		/// </summary>
		/// <param name="parentImplication">The parent of pre-conditioned implications.</param>
		/// <returns>The list of pre-conditioned implications.</returns>
		public IList<Implication> GetPreconditionChildren(Implication parentImplication) {
			IList<Implication> result = new List<Implication>();
			
			foreach(Implication implication in implications)
				if (implication.PreconditionImplication == parentImplication)
					result.Add(implication);
			
			return result;
		}
		
		public IList<Implication> GetListeningImplications(Atom atom) {
			IList<Implication> result;
			if (implicationsMap.TryGetValue(atom.Signature, out result)) return result;
			else return null;
		}

		public override string ToString() {
			string result = "";
			
			foreach(Implication implication in implications)
				result = result + implication.ToString() + "\n";
			
			result += "\n";
			
			foreach (string signature in implicationsMap.Keys)
				result = result + (implicationsMap[signature]).Count +
													" listening atoms of signature '" + signature + "'\n";
				
			return result;	
		}
		
	}
}
