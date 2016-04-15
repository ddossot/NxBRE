namespace NxBRE.InferenceEngine.Core {
	/// <summary>
	/// The WorkingMemory is the core class of the inference engine.
	/// It contains references to the FactBase.
	/// </summary>	
	/// <description>
	/// The WorkingMemory supports two types: Global and Isolated.
	/// It is important to understand that Isolated memory is forked from the Global memory.
	/// Isolated memory is usefull in multi-threaded environments, where the engine must run
	/// a common base of implications/facts against facts specific to each thread.
	/// </description>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	/// <see cref="NxBRE.InferenceEngine.IEImpl"/>
	internal interface IWorkingMemory {
		WorkingMemoryTypes Type {	get; set; }
		FactBase FB {	get; }
		void CommitIsolated();
		void PrepareInitialization();
		void FinishInitialization();
		void DisposeIsolated();
	}

}
