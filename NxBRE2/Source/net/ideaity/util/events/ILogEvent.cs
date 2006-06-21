namespace net.ideaity.util.events
{
	using System;
	/// <summary> Defines a log interface
	/// *
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  .90
	/// </version>
	public interface ILogEvent
		{
			/// <summary> Returns the exception that may have occurred(nullable)
			/// *
			/// </summary>
			/// <returns> The exception that may have occurred
			/// 
			/// </returns>
			System.Exception Exception
			{
				get;
				
			}
			/// <summary> Returns the log message
			/// *
			/// </summary>
			/// <returns> The log message
			/// 
			/// </returns>
			string Message
			{
				get;
				
			}
			/// <summary> Returns the priority of the log message
			/// *
			/// </summary>
			/// <returns> The log priority
			/// 
			/// </returns>
			int Priority
			{
				get;
				
			}

			/// <summary> Returns the timestamp
			/// *
			/// </summary>
			/// <returns> Calendar containing timestamp
			/// 
			/// </returns>
			DateTime Timestamp
			{
				get;
				
			}
		}
}
