namespace NxBRE.InferenceEngine.Console {
	using System;
	using System.Collections.Generic;
	using System.IO;
	
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// 
	/// Basic Console for NxBRE Inference Engine
	/// 
	/// </summary>
	/// <remarks>
	/// Shamlessly uncommented code.
	/// </remarks>
	public sealed class FactDumperAdapter : IFactBaseAdapter {

		private ConsoleWriter fdt;
		
		public FactDumperAdapter(ConsoleWriter factDumperTarget) {
			fdt = factDumperTarget;
		}
		
		public string Direction {
			get {
				throw new NotImplementedException();
			}
			set {
				// ignored
			}
		}
		
		public string Label {
			get {
				throw new NotImplementedException();
			}
			set {
				// ignored
			}
		}
		
		public IList<Fact> Facts {
			get {
				throw new NotImplementedException();
			}
			set {
				foreach(Fact fact in value) fdt(" " + fact.ToString()); 
			}
		}
		
		public IBinder Binder {
			set {
			// ignored
			}
		}
		
		public void Dispose() {
			// ignored
		}
	}
}
