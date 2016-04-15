namespace NxBRE.InferenceEngine {
	using System;
	
	/// <summary>
	/// An exception thrown by the engine when an integrity error has been detected.
	/// </summary>
	public class IntegrityException:Exception {
		/// <summary> Instantiates a new IntegrityException with a specific message</summary>
		/// <param name="message">The error message</param>
		public IntegrityException(string message):base(message) {}
	}
	
}
