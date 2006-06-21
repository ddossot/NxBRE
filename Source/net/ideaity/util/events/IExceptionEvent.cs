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
	public interface IExceptionEvent
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
			/// <summary> Returns the priority of the exception
			/// *
			/// </summary>
			/// <returns> The priority
			/// 
			/// </returns>
			int Priority
			{
				get;
				
			}
		}
}
