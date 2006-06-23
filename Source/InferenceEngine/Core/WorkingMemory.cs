namespace org.nxbre.ie.core {
	using org.nxbre.ie;
	using org.nxbre.ie.rule;
	
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
	/// <see cref="org.nxbre.ie.IEImpl"/>
	/// <version>2.4</version>
	internal sealed class WorkingMemory:AbstractWorkingMemory {
		public override WorkingMemoryTypes Type {
			get {
				return WorkingType;
			}
			set {
				// Depending on the working memory isolation type
				if (value == WorkingMemoryTypes.Isolated)
					// Clone the global base as the working base
					WorkingFactBase = (FactBase)globalFactBase.Clone();
				else if (value == WorkingMemoryTypes.IsolatedEmpty)
					// Create an empty base as the working base
					WorkingFactBase = new FactBase();
				else 
					// Use the global base as the working base
					WorkingFactBase = globalFactBase;
				
				WorkingType = value;
			}
		}
		
		public override void PrepareInitialization() {
			WorkingType = WorkingMemoryTypes.Global;
			globalFactBase = new FactBase();
			WorkingFactBase = globalFactBase;
		}
		
		public override void FinishInitialization() {}
				
		public override void CommitIsolated() {
			if (Type == WorkingMemoryTypes.Isolated) {
				globalFactBase = WorkingFactBase;
				Type = WorkingMemoryTypes.Global;
			}
			else if (Type == WorkingMemoryTypes.IsolatedEmpty) {
				foreach(Fact f in WorkingFactBase) globalFactBase.Assert(f);
				Type = WorkingMemoryTypes.Global;
			}
			else
				throw new BREException("Current Working Memory is not Isolated and can not be committed.");
		}
		
	}
}
