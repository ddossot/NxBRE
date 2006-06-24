namespace net.ideaity.util.events {
	/// <summary>
	/// This interface must be implemented by any class that wants to handle log events
	/// </summary>
	/// <author>David Dossot</author>
	/// <remarks>
	///  The delegate of the same name allows creating custom implementation of the handler.
	/// </remarks>
	public interface ILogDispatcher {
		event DispatchLog LogHandlers;

		///<summary>
		/// The message to be logged together with a numeric identifier for the priority.
		/// </summary>
		void DispatchLog(string message, int aPriority);
		
		bool HasLogListener { get; }
		
		void ForceDispatchLog(string message, int aPriority);
	}

	public delegate void DispatchLog(object sender, ILogEvent le);
}

