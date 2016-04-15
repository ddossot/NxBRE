namespace NxBRE.FlowEngine
{
	using System;
	
	/// <summary> This error is thrown when an error is generated from
	/// the Busines Rule (BRF), usually from the executeRule().
	/// Not really used much....
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	public class BRERuleException:BREException
	{
		/// <summary> Returns a RuleResult that may have been created when the
		/// exception occurred.  This object can be null.
		/// </summary>
		/// <returns> The RuleResult
		/// 
		/// </returns>
		virtual public IBRERuleResult RuleResult
		{
			get
			{
				return brr;
			}
		}
		
		private IBRERuleResult brr = null;
		
		/// <summary>
		/// Defines a new BRERuleFatalException with a specific msg
		/// </summary>
		public BRERuleException():base()	{}
		
		/// <summary> Defines a new BRERuleFatalException with a specific msg
		/// </summary>
		/// <param name="aMsg">The error message
		/// </param>
		public BRERuleException(string aMsg):base(aMsg)	{}
		
		/// <summary> Defines a new BRERuleException with a specific msg,
		/// and the resultSet that may have been created (nullable)
		/// </summary>
		/// <param name="aMsg">The error message
		/// </param>
		/// <param name="aBRR">The BusinessRuleResult that may have been created
		/// </param>
		public BRERuleException(string aMsg, IBRERuleResult aBRR):base(aMsg)
		{
			brr = aBRR;
		}
		
	}
}
