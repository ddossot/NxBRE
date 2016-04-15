namespace NxBRE.FlowEngine.Core
{
	using System;
	
	using NxBRE.FlowEngine;
	
	/// <summary> Strict implementation of BRERuleRuleResult
	/// *
	/// This class is sealed so that no one tries to extend it. 
	/// Implementation Inheritance can be a dangerous thing.
	/// <P>
	/// If a developer wishes to extend this in some way they should use
	/// AbstractBRERuleContext instead since it is designed to be added on to.
	/// </P>
	/// *
	/// </summary>
	/// <seealso cref="NxBRE.FlowEngine.Core.AbstractBRERuleResult">
	/// </seealso>
	/// <author>  Sloan Seaman
	/// </author>
	internal sealed class BRERuleResultImpl:AbstractBRERuleResult
	{
		
		/// <summary> Creates a new instance of the object
		/// <P> 
		/// This constructor makes a call to a protected constructor
		/// within AbstractBRERuleContext.  This is another reason
		/// why this class in final, because this technically breaks
		/// encapsulation.  For more info see:</P><P>
		/// <I>Effective Java</I> (first printing) by Joshua Bloch.</P>
		/// Item 14, pg. 71
		/// *
		/// </summary>
		/// <param name="aMetaData">The RuleMetaData of the RuleResult
		/// </param>
		/// <param name="aResult">The actual result
		/// 
		/// </param>
		public BRERuleResultImpl(IBRERuleMetaData aMetaData, object aResult):base(aMetaData, aResult)
		{
		}
	}
}
