namespace NxBRE.InferenceEngine.Core
{
	using System;
	using System.Threading;
	
	/// <summary>
	/// The WorkingMemory is the core class of the inference engine.
	/// </summary>	
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	internal abstract class AbstractWorkingMemory:IWorkingMemory
	{
		protected FactBase globalFactBase;
		
		private LocalDataStoreSlot workingTypeSlot = Thread.AllocateDataSlot();
		private LocalDataStoreSlot workingFactBaseSlot = Thread.AllocateDataSlot();
		
		protected WorkingMemoryTypes WorkingType {
			get {
				return (WorkingMemoryTypes) Thread.GetData(workingTypeSlot);
			}
			set {
				Thread.SetData(workingTypeSlot, value);
			}
		}
		
		protected FactBase WorkingFactBase {
			get {
				return (FactBase) Thread.GetData(workingFactBaseSlot);
			}
			set {
				Thread.SetData(workingFactBaseSlot, value);
			}
		}
		
		
		public FactBase FB {
			get {
				if (Type == WorkingMemoryTypes.Global) return globalFactBase;
				else return WorkingFactBase;
			}
		}
		
		public void DisposeIsolated() {
			if (Type == WorkingMemoryTypes.Global)
				throw new BREException("Current Working Memory is not Isolated and can not be committed.");
			
			Type = WorkingMemoryTypes.Global;
		}
		
		public abstract WorkingMemoryTypes Type {	get; set; }
		public abstract void CommitIsolated();
		public abstract void PrepareInitialization();
		public abstract void FinishInitialization();
	}
}
