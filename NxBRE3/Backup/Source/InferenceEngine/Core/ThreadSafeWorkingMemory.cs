namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Threading;

	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// This is the ThreadSafe version of the WorkingMemory, which is the core class of the inference engine.
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
	internal sealed class ThreadSafeWorkingMemory:AbstractWorkingMemory {
		private ILockStrategy lockStrategy;
		private int lockTimeOut;
		
		public ThreadSafeWorkingMemory(bool supportHotSwap, int lockTimeOut) {
			this.lockTimeOut = lockTimeOut;
			
			if (supportHotSwap) lockStrategy = new ReaderWriterLockStrategy();
			else lockStrategy = new NoLockStrategy();
		}
		
		public override void PrepareInitialization() {
			lockStrategy.AcquireUniqueLock(lockTimeOut);
			
			WorkingType = WorkingMemoryTypes.Global;
			globalFactBase = new FactBase();
		}
		
		public override void FinishInitialization() {
			lockStrategy.ReleaseUniqueLock();
		}
		
		public override WorkingMemoryTypes Type {
			get {
				return WorkingType;
			}
			set {
				// we release any shared lock to give a chance for a unique lock to be set
				lockStrategy.ReleaseSharedLock();
				
				// Depending on the working memory isolation type
				if (value == WorkingMemoryTypes.Isolated) {
					lockStrategy.AcquireSharedLock(lockTimeOut);

					// Clone the global base as the working base
					WorkingFactBase = (FactBase) globalFactBase.Clone();
				}
				else if (value == WorkingMemoryTypes.IsolatedEmpty) {
					lockStrategy.AcquireSharedLock(lockTimeOut);

					// Create an empty base as the working base
					WorkingFactBase = new FactBase();
				}
				else {
					// Release the working memory
					WorkingFactBase = null;
				}
				
				WorkingType = value;
			}
		}
		
		public override void CommitIsolated() {
			if (Type == WorkingMemoryTypes.Global)
				throw new BREException("Current Working Memory is not Isolated and can not be committed.");
			
			lockStrategy.AcquireUniqueLock(lockTimeOut);
			
			foreach(Fact f in FB) globalFactBase.Assert(f);
			WorkingType = WorkingMemoryTypes.Global;
			
			lockStrategy.ReleaseUniqueLock();
		}
		
		private interface ILockStrategy {
			void AcquireSharedLock(int lockTimeOut);
			void ReleaseSharedLock();
			void AcquireUniqueLock(int lockTimeOut);
			void ReleaseUniqueLock();
		}
		
		private class NoLockStrategy:ILockStrategy{
			public void AcquireSharedLock(int lockTimeOut) {}
			public void ReleaseSharedLock() {}
			public void AcquireUniqueLock(int lockTimeOut) {}
			public void ReleaseUniqueLock() {}
		}
		
		private class ReaderWriterLockStrategy:ILockStrategy{
			private ReaderWriterLock rwl = new ReaderWriterLock();
			
			public void AcquireSharedLock(int lockTimeOut) {
				rwl.AcquireReaderLock(lockTimeOut);
			}
			
			public void ReleaseSharedLock() {
				if (rwl.IsReaderLockHeld) rwl.ReleaseReaderLock();
			}
			
			public void AcquireUniqueLock(int lockTimeOut) {
				rwl.AcquireWriterLock(lockTimeOut);
			}
			
			public void ReleaseUniqueLock() {
				if (rwl.IsWriterLockHeld) rwl.ReleaseWriterLock();
			}
		}
		
	}
}
