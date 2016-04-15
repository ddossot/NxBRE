namespace NxBRE.FlowEngine.Core
{
	using System;
	
	using NxBRE.FlowEngine;
	/// <summary>An helper class that allows adding business objects in the result context
	/// </summary>
	/// <seealso cref="NxBRE.FlowEngine.Core.AbstractBRERuleResult">
	/// </seealso>
	/// <author>  David Dossot
	/// </author>
	internal sealed class BRERuleObject:AbstractBRERuleResult
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
		/// <param name="aObject">The object to add to the result context
		/// </param>
		public BRERuleObject(object aObject):base(null, aObject)
		{
		}
	}
}
