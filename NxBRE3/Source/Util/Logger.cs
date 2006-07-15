namespace NxBRE.Util
{
	using System;
	using System.Diagnostics;

	internal abstract class Logger
	{
		private Logger() {}
		
		/// <summary>
		/// The Trace Source used by NxBRE.FlowEngine
		/// </summary>
		public static readonly TraceSource FlowEngineSource = new TraceSource("NxBRE.FlowEngine", SourceLevels.Warning);
		
		public static readonly bool IsFlowEngineWarning = FlowEngineSource.Switch.ShouldTrace(TraceEventType.Warning);
		
		/// <summary>
		/// The Trace Source used by NxBRE.FlowEngine.RuleBase generated traces
		/// </summary>
		public static readonly TraceSource FlowEngineRuleBaseSource = new TraceSource("NxBRE.FlowEngine.RuleBase", SourceLevels.Warning);
		
		/// <summary>
		/// The Trace Source used by NxBRE.InferenceEngine
		/// </summary>
		public static readonly TraceSource InferenceEngineSource = new TraceSource("NxBRE.InferenceEngine", SourceLevels.Warning);

		public static readonly bool IsInferenceEngineVerbose = InferenceEngineSource.Switch.ShouldTrace(TraceEventType.Verbose);
		public static readonly bool IsInferenceEngineInformation = InferenceEngineSource.Switch.ShouldTrace(TraceEventType.Information);
		public static readonly bool IsInferenceEngineWarning = InferenceEngineSource.Switch.ShouldTrace(TraceEventType.Warning);
		
		/// <summary>
		/// The Trace Source used by NxBRE.Util
		/// </summary>
		public static readonly TraceSource UtilSource = new TraceSource("NxBRE.Util", SourceLevels.Warning);

		public static readonly bool IsUtilVerbose = UtilSource.Switch.ShouldTrace(TraceEventType.Verbose);
		public static readonly bool IsUtilWarning = UtilSource.Switch.ShouldTrace(TraceEventType.Warning);

		private static TraceEventType[] traceEventTypeValues = (TraceEventType[])Enum.GetValues(typeof(TraceEventType));
		
		/// <summary>
		/// Converts old and obsolete int based trace levels from NxBRE v2 to .NET 2.0 trace event types
		/// </summary>
		/// <param name="traceLevel">The integer representing the trace level in the old system.</param>
		/// <returns>The matching TraceEventType or TraceEventType.Information if no match found.</returns>
		public static TraceEventType ConvertFromObsoleteIntLevel(int traceLevel) {
			if ((traceLevel >= 0) && (traceLevel < traceEventTypeValues.Length)) return traceEventTypeValues[traceLevel];
			else return TraceEventType.Information;
		}
	}
}
