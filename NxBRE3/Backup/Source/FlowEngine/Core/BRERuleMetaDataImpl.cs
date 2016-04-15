namespace NxBRE.FlowEngine.Core
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;
	
	/// <summary> Strict implementation of BRERuleMetaData
	/// <P>
	/// This class is sealed so that no one tries to extend it. 
	/// Implementation Inheritance can be a dangerous thing.
	/// </P>
	/// <P>
	/// If a developer wishes to extend this in some way they should use
	/// AbstractBRERuleContext instead since it is designed to be added on to.
	/// </P>
	/// *
	/// </summary>
	/// <seealso cref="NxBRE.FlowEngine.Core.AbstractBRERuleMetaData">
	/// </seealso>
	/// <author>  Sloan Seaman
	/// </author>
	internal sealed class BRERuleMetaDataImpl:AbstractBRERuleMetaData
	{
		/// <summary> Creates a new instance of the object
		/// <P> 
		/// This constructor makes a call to a protected constructor
		/// within AbstractBRERuleContext.  This is another reason
		/// why this class in final, because this technically breaks
		/// encapsulation.  For more info see:</P><P>
		/// <I>Effective Java</I> (first printing) by Joshua Bloch. 
		/// Item 14, pg. 71
		/// </P>
		/// *
		/// </summary>
		/// <param name="aId">The UID of the RuleResult
		/// </param>
		/// <param name="aFactory">The RuleFactory that create the RuleResult
		/// </param>
		/// <param name="aParams">A Map of the parameters
		/// </param>
		/// <param name="aStackLoc">The location on the stack of this RuleResult
		/// </param>
		/// <param name="aStep">The step within the rule
		/// 
		/// </param>
		public BRERuleMetaDataImpl(object aId, IBRERuleFactory aFactory, Hashtable aParams, int aStackLoc, object aStep):base(aId, aFactory, aParams, aStackLoc, aStep)
		{
		}
	}
}
