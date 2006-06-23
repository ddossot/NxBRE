namespace org.nxbre.test.ie
{
	using org.nxbre.ie;
	using org.nxbre.ie.adapters;
	
	public class TestEngineMiscThreadSafeWM:TestEngineMisc
	{
		protected override void NewIEImpl(IBinder bob) {
			if (bob != null) ie = new IEImpl(bob, ThreadingModelTypes.Multi);
			else ie = new IEImpl(ThreadingModelTypes.Multi);
		}
	}
}
