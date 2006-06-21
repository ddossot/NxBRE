namespace net.ideaity.util.events
{
	using System;
	/// <summary> Defines an abstract implementation of the log interface
	/// *
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  .90
	/// </version>
	public abstract class AbstractLogEvent:EventArgs, ILogEvent
	{
		/// <summary> Returns the exception that may have occurred(nullable)
		/// *
		/// </summary>
		/// <returns> The exception that may have occurred
		/// 
		/// </returns>
		public virtual System.Exception Exception
		{
			get
			{
				return error;
			}
			
		}
		
		/// <summary> Returns the log message
		/// *
		/// </summary>
		/// <returns> The log message
		/// 
		/// </returns>
		virtual public string Message
		{
			get
			{
				return msg;
			}
			
		}
		/// <summary> Returns the priority of the log message
		/// *
		/// </summary>
		/// <returns> The log priority
		/// 
		/// </returns>
		virtual public int Priority
		{
			get
			{
				return priority;
			}
			
		}
		/// <summary> Returns the timestamp
		/// *
		/// </summary>
		/// <returns> Calendar containing timestamp
		/// 
		/// </returns>
		virtual public DateTime Timestamp
		{
			get
			{
				return timestamp;
			}
			
		}
		private const string dateFormat = "u";
		private int priority = 0;
		private string msg = null;
		private System.Exception error = null;
		private System.DateTime timestamp;
		
		/// <summary> Protected constructor
		/// *
		/// </summary>
		/// <param name="aPriority">The priority of the log message
		/// </param>
		/// <param name="aMsg">The log message
		/// </param>
		/// <param name="aTimeStamp">The time stamp of the log message</param>"
		protected internal AbstractLogEvent(int aPriority, string aMsg, DateTime aTimeStamp):this(aPriority, aMsg, aTimeStamp, null)
		{
		}
		
		/// <summary> Protected constructor
		/// *
		/// </summary>
		/// <param name="aPriority">The priority of the log message
		/// </param>
		/// <param name="aMsg">The log message
		/// </param>
		/// <param name="aTimeStamp">The time stamp of the log entry</param>
		/// <param name="aError">The error that may have occurred
		/// 
		/// </param>
		protected internal AbstractLogEvent(int aPriority, string aMsg, DateTime aTimeStamp, System.Exception aError)
		{
			priority = aPriority;
			msg = aMsg;
			error = aError;
			timestamp = aTimeStamp;
		}
		/// <summary> Display method
		/// 
		/// </summary>
		/// <returns> A formatted string
		/// 
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			
			sb.Append("Priority   : ")
				.Append(priority)
				.Append("\n")
				.Append("Message    : ")
				.Append(msg)
				.Append("\n")
				.Append("Timestamp  : ")
				.Append(timestamp.ToString(dateFormat))
				.Append("\n");
			
			if (error != null)
				sb.Append("Exception Type: ")
					.Append(error.GetType().FullName)
					.Append("\n")
					.Append("Exception Str : ")
					.Append(error.Message);
			
			return sb.ToString();
		}
	}
}
