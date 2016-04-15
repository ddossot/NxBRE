namespace NxBRE
{
	using System;
	
	/// <summary> The BREException is main exception that can
	/// be thrown from the Business Rule Engine (BRE)
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	/// <author>  David Dossot
	/// </author>
	public class BREException:Exception
	{
		/// <summary> Defines a new BREException
		/// </summary>
		public BREException():base() {}
		
		/// <summary> Defines a new BREException with a specific msg </summary>
		/// <param name="aMsg">The error message</param>
		public BREException(string aMsg):base(aMsg) {}
		
		/// <summary> Defines a new BREException with a specific msg </summary>
		/// <param name="aMsg">The error message</param>
		/// <param name="wrappedException">An inner exception wrapped by this one</param>
		public BREException(string aMsg, Exception wrappedException):base(aMsg, wrappedException) {}
	}
}
