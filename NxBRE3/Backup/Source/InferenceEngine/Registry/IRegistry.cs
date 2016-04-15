namespace NxBRE.InferenceEngine.Registry {

	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Defines a registry of preloaded NxBRE Inference Engines instances where each engine is identified in the registry by a String ID.
	/// </summary>
	public interface IRegistry
	{
		int Count {
			get;
		}
		
		ICollection<string> EngineIDs {
			get;
		}
		
		IInferenceEngine GetEngine(string engineID);
	}
}
