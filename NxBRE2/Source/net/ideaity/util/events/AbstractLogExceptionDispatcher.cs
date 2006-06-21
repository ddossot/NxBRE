namespace net.ideaity.util.events {
	using System;
	
	/// <summary>
	/// Provides default implementations for business objects that would deal with log
	/// or exception events.
	/// </summary>
	/// <author>David Dossot</author>
	public abstract class AbstractLogExceptionDispatcher:AbstractLogDispatcher,IExceptionDispatcher {
		public event DispatchException ExceptionHandlers;
		
	///<summary>Overloaded method for loging exceptions</summary>
	/// <param name="message">The exception message to log</param>
	/// <param name="aPriority">Priority parameter</param>
		public void DispatchException(string message, int aPriority) {
			if (ExceptionHandlers != null)
				ExceptionHandlers(this, new ExceptionEventImpl(new Exception(message), aPriority));
		}

	///<summary>Overloaded method for loging exceptions</summary>
	/// <param name="aException">The exception to log</param>
	/// <param name="aPriority">Priority parameter</param>
		public void DispatchException(Exception aException, int aPriority) {
			if (ExceptionHandlers != null)
				ExceptionHandlers(this, new ExceptionEventImpl(aException, aPriority));
		}

		protected DispatchException GetExceptionHandlers() {
			return ExceptionHandlers;
		}
		
	}
}
