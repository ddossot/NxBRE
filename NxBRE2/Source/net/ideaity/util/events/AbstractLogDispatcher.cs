namespace net.ideaity.util.events {
	using System;
	
	/// <summary>
	/// Provides default implementations for business objects that would deal with log events.
	/// </summary>
	/// <author>David Dossot</author>
	public abstract class AbstractLogDispatcher:ILogDispatcher {
		public event DispatchLog LogHandlers;
		
		///<summary>Method for logging messages if at least one listener has subscribed to the event.</summary>
		/// <param name="message">The message to log</param>
		/// <param name="aPriority">Priority parameter</param>
		public virtual void DispatchLog(string message, int aPriority) {
			if (LogHandlers != null)
				LogHandlers(this, new LogEventImpl(aPriority, message));
		}

		///<summary>Method for logging messages, even if no-one is listening to the event.</summary>
		/// <remarks>This version allows for testing the existence of listeners prior to build the log
		/// message, for performance reasons.</remarks>
		/// <param name="message">The message to log</param>
		/// <param name="aPriority">Priority parameter</param>
		public virtual void ForceDispatchLog(string message, int aPriority) {
			LogHandlers(this, new LogEventImpl(aPriority, message));
		}

		protected DispatchLog GetLogHandlers() {
			return LogHandlers;
		}
		
		public bool HasLogListener {
			get {
				return (LogHandlers != null);
			}
		}

	}
}
