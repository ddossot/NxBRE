namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// The ImplicationBase is the repository of implications for the inference engine.
	/// Implications are organized in sub-collections grouped by atom types for performance reasons.
	/// Each implication "listens" to specific facts, based on their types.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class ImplicationBase:IEnumerable {
		//TODO: make generic
		private ArrayList implications;
		private Hashtable implicationsMap;

		public int Count {
			get {
				return implications.Count;
			}
		}

		public ImplicationBase() {
			implications = ArrayList.Synchronized(new ArrayList());
			implicationsMap = Hashtable.Synchronized(new Hashtable());
		}

		public void Add(Implication implication) {
			int indexOfDuplicate = implications.IndexOf(implication);
			
			if (indexOfDuplicate >= 0)
				throw new BREException("When adding: " + implication +
				                       " the knowledge base detected a duplicated with: " + implications[indexOfDuplicate]);
			
			implications.Add(implication);
			
			ArrayList typeListeners;
			
			foreach(Atom atom in implication.AtomGroup.AllAtoms) {
				//TODO: base on atom.Signature, not Type, it will be more accurate
				if (!implicationsMap.ContainsKey(atom.Type)) {
					typeListeners = new ArrayList();
					implicationsMap.Add(atom.Type, typeListeners);
				}
				else
					typeListeners = (ArrayList) implicationsMap[atom.Type];
				
				if (!typeListeners.Contains(implication))	typeListeners.Add(implication);
			}
		}
		
		public IEnumerator GetEnumerator() {
			return implications.GetEnumerator();
		}
		
		public Implication Get(string implicationLabel) {
			foreach(Implication implication in implications)
				if (implication.Label == implicationLabel)
					return implication;
			
			return null;
		}
		
		public ArrayList GetPreconditionChildren(Implication parentImplication) {
			ArrayList result = new ArrayList();
			
			foreach(Implication implication in implications)
				if (implication.PreconditionImplication == parentImplication)
					result.Add(implication);
			
			return result;
		}
		
		public ArrayList GetListeningImplications(string factType) {
			return (ArrayList)implicationsMap[factType];
		}

		public override string ToString() {
			string result = "";
			
			foreach(Implication implication in implications)
				result = result + implication.ToString() + "\n";
			
			result += "\n";
			
			foreach (string type in implicationsMap.Keys)
				result = result + ((ArrayList)implicationsMap[type]).Count +
													" listening atoms of type '" + type + "'\n";
				
			return result;	
		}
		
	}
}
