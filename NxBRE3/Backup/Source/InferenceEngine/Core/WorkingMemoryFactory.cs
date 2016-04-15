namespace NxBRE.InferenceEngine.Core
{
	/// <summary>
	/// A factory for creating WorkingMemory objects.
	/// </summary>
	internal abstract class WorkingMemoryFactory
	{
		private WorkingMemoryFactory() {}
		
		public static IWorkingMemory NewWorkingMemory(ThreadingModelTypes threadingModelType, int lockTimeOut) {
			if (threadingModelType == ThreadingModelTypes.Single) return new WorkingMemory();
			else if (threadingModelType == ThreadingModelTypes.Multi) return new ThreadSafeWorkingMemory(false, lockTimeOut);
			else if (threadingModelType == ThreadingModelTypes.MultiHotSwap) return new ThreadSafeWorkingMemory(true, lockTimeOut);
			else throw new BREException("Unsupported threading model type: " + threadingModelType);
		}
	}
}
