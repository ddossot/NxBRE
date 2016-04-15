namespace NxBRE.InferenceEngine.Core {
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// The WorkingMemory is the core class of the inference engine.
	/// It contains references to the FactBase.
	/// </summary>	
	/// <description>
	/// The WorkingMemory supports two types: Global and Isolated.
	/// It is important to understand that Isolated memory is forked from the Global memory.
	/// </description>
	/// <remarks>Core classes are not supposed to be used directly.</remarks>
	/// <author>David Dossot</author>
	/// <see cref="NxBRE.InferenceEngine.IEImpl"/>
	internal sealed class WorkingMemory:AbstractWorkingMemory {
		public override WorkingMemoryTypes Type {
			get {
				return WorkingType;
			}
			set
			{
			    // Depending on the working memory isolation type
			    switch (value)
			    {
			        case WorkingMemoryTypes.Isolated:
			            WorkingFactBase = (FactBase)globalFactBase.Clone();
			            break;
			        case WorkingMemoryTypes.IsolatedEmpty:
			            WorkingFactBase = new FactBase();
			            break;
			        case WorkingMemoryTypes.Global:
			            break;
			        default:
			            WorkingFactBase = globalFactBase;
			            break;
			    }

			    WorkingType = value;
			}
		}
		
		public override void PrepareInitialization() {
			WorkingType = WorkingMemoryTypes.Global;
			globalFactBase = new FactBase();
			WorkingFactBase = globalFactBase;
		}
		
		public override void FinishInitialization() {}
				
		public override void CommitIsolated()
		{
		    switch (Type)
		    {
		        case WorkingMemoryTypes.Isolated:
		            globalFactBase = WorkingFactBase;
		            Type = WorkingMemoryTypes.Global;
		            break;
		        case WorkingMemoryTypes.IsolatedEmpty:
		            foreach(Fact f in WorkingFactBase) globalFactBase.Assert(f);
		            Type = WorkingMemoryTypes.Global;
		            break;
		        case WorkingMemoryTypes.Global:
		            break;
		        default:
		            throw new BREException("Current Working Memory is not Isolated and can not be committed.");
		    }
		}
	}
}
