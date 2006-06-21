namespace net.ideaity.util
{
	using System;
	
	public class InitializationException:System.Exception
	{
		
		public InitializationException():base()
		{
		}
		
		public InitializationException(string _msg):base(_msg)
		{
		}
	}
}
