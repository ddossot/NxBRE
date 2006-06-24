namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine.Rules;

	///<summary>
	/// The agenda is the "maestro" of the inference engine.
	/// It manages the scheduling of the implication and provides them to the engine 
	/// ordered by their weigth.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class Agenda:ICloneable {
		private ArrayList agenda;
		private ImplicationComparer implicationComparer;
		
		public int Count {
			get {
				return agenda.Count;
			}
		}
		
		public Agenda() {
			agenda = ArrayList.Synchronized(new ArrayList());
			implicationComparer = new ImplicationComparer();
		}
		
		public object Clone() {
			Agenda agenda = new Agenda();
			agenda.agenda = (ArrayList)this.agenda.Clone();
			agenda.implicationComparer = this.implicationComparer;
			return agenda;
		}
		
		public void PrepareExecution() {
			agenda.Sort(implicationComparer);
		}
		
		/// <summary>
		/// Schedule a single implication.
		/// </summary>
		/// <remarks>
		/// This method is public just for unit test purposes.
		/// </remarks>
		/// <param name="implication">The implication to schedule.</param>
		public void Schedule(Implication implication) {
			if (!agenda.Contains(implication))
				agenda.Add(implication);
		}
		
		/// <summary>
		/// Schedule all implications that are listening the facts in the fact base
		/// except if no new fact of the listening type where asserted in the previous iteration.
		/// </summary>
		/// <param name="positiveImplications">Null if it is the first iteration, else en ArrayList of positive implications of current iteration.</param>
		/// <param name="IB">The current ImplicationBase.</param>
		public void Schedule(ArrayList positiveImplications, ImplicationBase IB) {
			if (positiveImplications == null) {
				// schedule all implications
				foreach(Implication implication in IB) Schedule(implication);
			}
			else {
				foreach(Implication positiveImplication in positiveImplications) {
					if (positiveImplication.Action != ImplicationAction.Retract) {
						// for positive implications, schedule only the implications
						// relevant to the newly asserted facts.
						ArrayList listeningImplications = IB.GetListeningImplications(positiveImplication.Deduction.Type);
						if (listeningImplications != null)
							foreach(Implication implication in listeningImplications)
								Schedule(implication);
					}
					else {
						// for negative implications, schedule only the implications
						// that can potentially assert a fact of same type that was retracted
						foreach(Implication implication in IB)
							if (implication.Deduction.Type == positiveImplication.Deduction.Type)
								Schedule(implication);
					}
					
					// schedule implications potentially pre-condition unlocked
					foreach(Implication implication in IB.GetPreconditionChildren(positiveImplication))
						Schedule(implication);
				}
			}
		}
		
		public bool HasMoreToExecute {
			get {
				if (agenda == null)
					throw new BREException("The Executing Agenda is null.");
				
				return (agenda.Count > 0);
			}
		}
		
		public Implication NextToExecute {
			get {
				if (!HasMoreToExecute)
					throw new BREException("The Executing Agenda can not provide any more Implication.");
				
				Implication executingImplication = (Implication)agenda[0];
				agenda.RemoveAt(0);
				return executingImplication;
			}
		}
		
		private class ImplicationComparer : IComparer  {
	    int IComparer.Compare(object x, object y)  {
	    	if ((x is Implication) && (y is Implication))
	    		return ((Implication)y).Weight - ((Implication)x).Weight;
	    	else
	    		return 0;
	    }
		}
		
	}
	
}
