namespace org.nxbre.gui {
	using System;
	using System.Collections;
	using System.IO;
	
	using org.nxbre.ie.adapters;
	using org.nxbre.ie.rule;
	
	public delegate void FactDumperTarget(string fact);
	
	/// <summary>
	/// 
	/// Basic Console for NxBRE Inference Engine
	/// 
	/// </summary>
	/// <remarks>
	/// Shamlessly uncommented code.
	/// </remarks>
	public sealed class FactDumperAdapter : IFactBaseAdapter {

		private FactDumperTarget fdt;
		
		public FactDumperAdapter(FactDumperTarget factDumperTarget) {
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
		
		public ArrayList Facts {
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
