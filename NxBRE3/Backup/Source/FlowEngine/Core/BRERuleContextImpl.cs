namespace NxBRE.FlowEngine.Core
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;
	/// <summary> Strict implementation of BRERuleContext.
	/// <P>
	/// This class is sealed so that no one tries to extend it. 
	/// Implementation Inheritance can be a dangerous thing.
	/// </P>
	/// <P>
	/// If a developer wishes to extend this in some way they should use
	/// AbstractBRERuleContext instead since it is designed to be added on to.
	/// </P>
	/// </summary>
	/// <seealso cref="NxBRE.FlowEngine.Core.AbstractBRERuleContext">
	/// </seealso>
	/// <author>  Sloan Seaman </author>
	/// <author>  David Dossot </author>
	internal sealed class BRERuleContextImpl:AbstractBRERuleContext
	{
		/// <summary> Creates a new instance of the object
		/// <P> 
		/// This constructor makes a call to a protected constructor
		/// within AbstractBRERuleContext.  This is another reason
		/// why this class is sealed, because this technically breaks
		/// encapsulation.  For more info see:</P><P>
		/// <I>Effective Java</I> (first printing) by Joshua Bloch.
		/// Item 14, pg. 71
		/// </P>
		/// *
		/// </summary>
		/// <param name="aStack">The Stack for the call stack
		/// </param>
		/// <param name="aFactories">The Map for the RuleFactory's
		/// </param>
		/// <param name="aOperators">The Map for the Operators
		/// </param>
		/// <param name="aResults">The Map for the RuleResults
		/// </param>
		public BRERuleContextImpl(Stack aStack, Hashtable aFactories, Hashtable aOperators, Hashtable aResults):base(aStack, aFactories, aOperators, aResults)
		{
		}
		
		/// <summary> Performs a shallow copy of the Rule Context, i.e. returns a new RuleContext
		/// containing shallow copies of its internal hashtables and stack
		/// </summary>
		public override object Clone() {
			return new BRERuleContextImpl(new Stack(internalCallStack),
			                              new Hashtable(factories),
			                              new Hashtable(operators),
			                              new Hashtable(results));
		}
		
		/// <summary> Sets a business object
		/// *
		/// </summary>
		/// <param name="aId">The UID of the business object
		/// </param>
		/// <param name="aObject">The business object
		/// 
		/// </param>
		public override void SetObject(object aId, object aObject)
		{
			SetResult(aId, new BRERuleObject(aObject));
		}

		/// <summary> Returns a business object
		/// *
		/// </summary>
		/// <param name="aId">The UID of the business object
		/// </param>
		/// <returns> The requested business object</returns>
		/// <remarks>Never use this method in the engine itself: the engine should only rely on GetResult.</remarks>
		public override object GetObject(object aId) {
			IBRERuleResult ruleResult = GetResult(aId);
			if (ruleResult != null) { 
				return ruleResult.Result;
			} else {
				return null;
			}
		}
	}
}
