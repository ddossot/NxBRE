namespace NxBRE.Test.InferenceEngine
{
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	
	public class TestEngineMiscHotSwapWM:TestEngineMisc
	{
		protected override void NewIEImpl(IBinder bob) {
			if (bob != null) ie = new IEImpl(bob, ThreadingModelTypes.MultiHotSwap);
			else ie = new IEImpl(ThreadingModelTypes.MultiHotSwap);
		}
	}
}
