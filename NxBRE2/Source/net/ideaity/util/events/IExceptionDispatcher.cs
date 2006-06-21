namespace net.ideaity.util.events {
	using System;

	/// <summary>
	/// This interface must be implemented by any class that wants to handle exception events
	/// </summary>
	/// <author>David Dossot</author>
	/// <remarks>
	///  The delegate of the same name allows creating custom implementation of the handler.
	/// </remarks>
	public interface IExceptionDispatcher {
		event DispatchException ExceptionHandlers;
		
		void DispatchException(string message, int aPriority);
		void DispatchException(Exception aException, int aPriority);
	}

	public delegate void DispatchException(object sender, IExceptionEvent ee);
}
