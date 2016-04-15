namespace NxBRE.InferenceEngine {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;

	/// <summary>
	/// Defines the information available in the event context.
	/// </summary>
	/// <author>David Dossot</author>
	public interface IEventContext
	{
		/// <summary>
		/// The facts that are source of this event.
		/// </summary>
		/// <returns>An <code>IList&lt;IList&lt;Fact>></code> containing the source facts.</returns>
		IList<IList<Fact>> Facts { get; }
		
		/// <summary>
		/// The implication source of this event.
		/// </summary>
		/// <returns>The source implication.</returns>
		Implication Implication { get; }
	}
}
