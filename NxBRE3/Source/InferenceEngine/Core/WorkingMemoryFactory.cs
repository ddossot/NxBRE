namespace NxBRE.InferenceEngine.Core
{
	/// <summary>
	/// A factory for creating WorkingMemory objects.
	/// </summary>
	internal abstract class WorkingMemoryFactory
	{
		private WorkingMemoryFactory() {}
		
		public static IWorkingMemory NewWorkingMemory(ThreadingModelTypes threadingModelType, int lockTimeOut)
		{
		    switch (threadingModelType)
		    {
		        case ThreadingModelTypes.Single:
		            return new WorkingMemory();
		        case ThreadingModelTypes.Multi:
		            return new ThreadSafeWorkingMemory(false, lockTimeOut);
		        case ThreadingModelTypes.MultiHotSwap:
		            return new ThreadSafeWorkingMemory(true, lockTimeOut);
		        default:
		            throw new BREException("Unsupported threading model type: " + threadingModelType);
		    }
		}
	}
}
