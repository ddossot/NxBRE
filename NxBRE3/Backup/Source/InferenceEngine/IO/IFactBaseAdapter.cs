namespace NxBRE.InferenceEngine.IO {

	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// NxBRE Inference Engine factbase adapter interface.
	/// The engine calls the properties in this order: Binder, Direction, Binder, Facts.
	/// </summary>
	/// <description>Reading is supported by the getter of each member, while writing is supported by setters.
	/// The engine calls dispose at the end of the load or save operation.
	/// </description>
	/// <see cref="NxBRE.InferenceEngine.IEImpl"/>
	/// <author>David Dossot</author>
	public interface IFactBaseAdapter:IDisposable {

		/// <summary>
		/// Optional direction of the rulebase: forward, backward or bidirectional.
		/// </summary>
		string Direction {
			get;
			set;
		}
		
		/// <summary>
		/// Optional label of the rulebase.
		/// </summary>
		string Label {
			get;
			set;
		}
		
		/// <summary>
		/// Collection containing all the facts in the factbase.
		/// </summary>
		IList<Fact> Facts {
			get;
			set;
		}
		
		/// <summary>
		/// Returns an instance of the associated Binder or null.
		/// </summary>
		IBinder Binder {
			set;
		}
			
	}
}
