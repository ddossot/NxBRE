namespace NxBRE.InferenceEngine.Registry {

	using System;

	/// <summary>
	/// Defines a registry of preloaded NxBRE Inference Engines instances where each engine is identified in the registry by a String ID.
	/// </summary>
	public interface IRegistry
	{
		IInferenceEngine GetEngine(string engineID);
	}
}
