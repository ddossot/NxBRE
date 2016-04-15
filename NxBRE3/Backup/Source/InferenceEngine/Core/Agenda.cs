namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;

	///<summary>
	/// The agenda is the "maestro" of the inference engine.
	/// It manages the scheduling of the implication and provides them to the engine 
	/// ordered by their weigth.
	/// </summary>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal sealed class Agenda:ICloneable {
		private List<Implication> scheduledImplication;
		private ImplicationComparer implicationComparer;
		
		public int Count {
			get {
				return scheduledImplication.Count;
			}
		}
		
		public Agenda() {
			scheduledImplication = new List<Implication>();
			implicationComparer = new ImplicationComparer();
		}
		
		public object Clone() {
			Agenda agenda = new Agenda();
			agenda.scheduledImplication = new List<Implication>(this.scheduledImplication);
			agenda.implicationComparer = this.implicationComparer;
			return agenda;
		}
		
		public void PrepareExecution() {
			scheduledImplication.Sort(implicationComparer);
			
			if (Logger.IsInferenceEngineVerbose)
				Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
				                                        0,
				                                        "Execution prepared: " + Misc.IListToString<Implication>(scheduledImplication));
		}
		
		/// <summary>
		/// Schedule a single implication.
		/// </summary>
		/// <param name="implication">The implication to schedule.</param>
		public void Schedule(Implication implication) {
			if (Logger.IsInferenceEngineVerbose)
				Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
				                                        0,
				                                        "Scheduling one implication: " + implication);
			
			if (!scheduledImplication.Contains(implication)) scheduledImplication.Add(implication);
		}
		
		/// <summary>
		/// Schedule all implications that are listening the facts in the fact base
		/// except if no new fact of the listening type where asserted in the previous iteration.
		/// </summary>
		/// <param name="positiveImplications">Null if it is the first iteration, else en ArrayList of positive implications of current iteration.</param>
		/// <param name="IB">The current ImplicationBase.</param>
		public void Schedule(IList<Implication> positiveImplications, ImplicationBase implicationBase) {
			if (positiveImplications == null) {
				// schedule all implications
				foreach(Implication implication in implicationBase) Schedule(implication);
			}
			else {
				foreach(Implication positiveImplication in positiveImplications) {
					if (positiveImplication.Action != ImplicationAction.Retract) {
						// for positive implications, schedule only the implications
						// relevant to the newly asserted facts.
						IList<Implication> listeningImplications = implicationBase.GetListeningImplications(positiveImplication.Deduction);
						if (listeningImplications != null)
							foreach(Implication implication in listeningImplications)
								Schedule(implication);
					}
					else {
						// for negative implications, schedule only the implications
						// that can potentially assert a fact of same type that was retracted
						foreach(Implication implication in implicationBase)
							if (implication.Deduction.Type == positiveImplication.Deduction.Type)
								Schedule(implication);
					}
					
					// schedule implications potentially pre-condition unlocked
					foreach(Implication implication in implicationBase.GetPreconditionChildren(positiveImplication))
						Schedule(implication);
				}
			}
		}
		
		public bool HasMoreToExecute {
			get {
				if (scheduledImplication == null)
					throw new BREException("The Executing Agenda is null.");
				
				return (scheduledImplication.Count > 0);
			}
		}
		
		public Implication NextToExecute {
			get {
				if (!HasMoreToExecute)
					throw new BREException("The Executing Agenda can not provide any more Implication.");
				
				Implication executingImplication = (Implication)scheduledImplication[0];
				scheduledImplication.RemoveAt(0);
				return executingImplication;
			}
		}
		
		private class ImplicationComparer : IComparer<Implication>  {
	    public int Compare(Implication x, Implication y)  {
	    	return ((Implication)y).Weight - ((Implication)x).Weight;
	    }
		}
		
	}
	
}
