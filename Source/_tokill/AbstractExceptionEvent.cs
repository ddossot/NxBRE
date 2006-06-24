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
	public abstract class AbstractExceptionEvent:EventArgs, IExceptionEvent
	{
		/// <summary> Returns the exception that may have occurred(nullable)
		/// *
		/// </summary>
		/// <returns> The exception that may have occurred
		/// 
		/// </returns>
		virtual public System.Exception Exception
		{
			get
			{
				return exception;
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
		
		private System.Exception exception;
		
		private int priority = 0;
		
		/// <summary> Protected Constructor
		/// *
		/// </summary>
		/// <param name="aException">The exception that occured
		/// </param>
		/// <param name="aPriority">The level of the error
		/// 
		/// </param>
		protected internal AbstractExceptionEvent(System.Exception aException, int aPriority)
		{
			exception = aException;
			priority = aPriority;
		}
		
		
	}
}
