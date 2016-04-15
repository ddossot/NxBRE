namespace NxBRE.FlowEngine
{
	using System;
	
	/// <summary> This error is thrown when the Flow Engine processing is stopped
	/// amid course in order to early terminate its execution. It should not be surfaced to end users.
	/// </summary>
	public class BREProcessInterruptedException:BREException
	{
		public BREProcessInterruptedException(string aMsg):base(aMsg)	{}
	}
}
