namespace NxBRE.InferenceEngine.Console {
	using System;
	using System.Diagnostics;
	
	public delegate void ConsoleWriter(string message);

	/// <summary>
	/// Description of IEGUIConsoleTraceListener.
	/// </summary>
	public class IEGUIConsoleTraceListener:TraceListener {
		private readonly ConsoleWriter cw;
			
		public IEGUIConsoleTraceListener(ConsoleWriter cw) {
			this.cw = cw;
		}
						
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data) {
			HandleEvent(eventType, data);
		}
		
		public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data) {
			HandleEvent(eventType, data);
		}
		
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id) {
			HandleEvent(eventType, null);
		}
		
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args) {
			HandleEvent(eventType, args);
		}
		
		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message) {
			HandleEvent(eventType, message);
		}
		
		private void HandleEvent(TraceEventType eventType, params object[] data) {
			if (data.Length == 1) Write("[" + eventType.ToString()[0] + "] " + data[0]);
		}
		
		public override void Write(string message) {
			cw(message);
		}
		
		public override void WriteLine(string message) {
			Write(message);
			cw(String.Empty);
		}
		
	}
}
