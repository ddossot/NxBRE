namespace NxBRE.Test.InferenceEngine
{
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	
	public class TestEngineMiscThreadSafeWM:TestEngineMisc
	{
		protected override void NewIEImpl(IBinder bob) {
			if (bob != null) ie = new IEImpl(bob, ThreadingModelTypes.Multi);
			else ie = new IEImpl(ThreadingModelTypes.Multi);
		}
	}
}
