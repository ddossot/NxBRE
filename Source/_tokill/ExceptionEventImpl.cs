namespace net.ideaity.util.events
{
	using System;
	/// <summary> Defines an exception interface
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  .90
	/// </version>
	public sealed class ExceptionEventImpl:AbstractExceptionEvent
	{
		/// <summary> Fatal Error = 1
		/// </summary>
		public const int FATAL = 1;
		
		/// <summary> Error = 2
		/// </summary>
		public const int ERROR = 2;
		
		/// <summary> Warn = 3
		/// </summary>
		public const int WARN = 3;
		
		/// <summary> Constructor
		/// *
		/// </summary>
		/// <param name="aException">The exception that occured
		/// </param>
		/// <param name="aPriority">The level of the error
		/// </param>
		public ExceptionEventImpl(System.Exception aException, int aPriority):base(aException, aPriority)
		{
		}
	}
}
