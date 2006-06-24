namespace NxBRE.InferenceEngine.Core
{
	/// <summary>
	/// A factory for creating WorkingMemory objects.
	/// </summary>
	internal abstract class WorkingMemoryFactory
	{
		private WorkingMemoryFactory() {}
		
		public static IWorkingMemory NewWorkingMemory(ThreadingModelTypes threadingModelType) {
			if (threadingModelType == ThreadingModelTypes.Single) return new WorkingMemory();
			else if (threadingModelType == ThreadingModelTypes.Multi) return new ThreadSafeWorkingMemory(false);
			else if (threadingModelType == ThreadingModelTypes.MultiHotSwap) return new ThreadSafeWorkingMemory(true);
			else throw new BREException("Unsupported threading model type: " + threadingModelType);
		}
	}
}
