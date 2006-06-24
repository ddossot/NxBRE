namespace net.ideaity.util.events
{
	using System;
	/// <summary> Reference implementation of Log
	/// *
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  .90
	/// </version>
	public sealed class LogEventImpl:AbstractLogEvent
	{
		/// <summary> DEBUG = 1
		/// </summary>
		public const int DEBUG = 1;
		
		/// <summary> FATAL = 2
		/// </summary>
		public const int FATAL = 2;
		
		/// <summary> ERROR = 3
		/// </summary>
		public const int ERROR = 3;
		
		/// <summary> WARN = 4
		/// </summary>
		public const int WARN = 4;
		
		/// <summary> INFO = 5
		/// </summary>
		public const int INFO = 5;
		
		/// <summary> Constructor
		/// *
		/// </summary>
		/// <param name="aPriority">The priority of the log message
		/// </param>
		/// <param name="aMsg">The log message
		/// 
		/// </param>
		public LogEventImpl(int aPriority, string aMsg):this(aPriority, aMsg, null)
		{
		}
			
		/// <summary> Constructor
		/// *
		/// </summary>
		/// <param name="aPriority">The priority of the log message
		/// </param>
		/// <param name="aMsg">The log message
		/// </param>
		/// <param name="aError">The error to log</param>
		public LogEventImpl(int aPriority, string aMsg, System.Exception aError):base(aPriority, aMsg, DateTime.Now, aError)
		{
		}
		
		/// <summary> Returns a String representation of the Priority
		/// *
		/// </summary>
		/// <returns> The log priority
		/// 
		/// </returns>
		public static string getPriorityAsString(int aPriority)
		{
			switch (aPriority)
			{
				
				case 1:  {
						return "DEBUG";
					}
				
				case 2:  {
						return "FATAL";
					}
				
				case 3:  {
						return "ERROR";
					}
				
				case 4:  {
						return "WARN";
					}
				
				case 5:  {
						return "INFO";
					}
				
				default:  {
						return "UNKNOWN";
					}
				
			}
		}
	}
}
