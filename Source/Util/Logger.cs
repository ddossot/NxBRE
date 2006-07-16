namespace NxBRE.Util
{
	using System;
	using System.Diagnostics;

	public abstract class Logger
	{
		private Logger() {}
		
		static Logger() {
			RefreshBooleanSwitches();
		}
		
		/// <summary>
		/// A preloaded array of possible TraceEventTypes
		/// </summary>
		private static TraceEventType[] traceEventTypeValues = (TraceEventType[])Enum.GetValues(typeof(TraceEventType));
		
		// Trace sources used by NxBRE
		private static readonly TraceSource flowEngineSource = new TraceSource("NxBRE.FlowEngine", SourceLevels.Warning);
		private static readonly TraceSource flowEngineRuleBaseSource = new TraceSource("NxBRE.FlowEngine.RuleBase", SourceLevels.Warning);
		private static readonly TraceSource inferenceEngineSource = new TraceSource("NxBRE.InferenceEngine", SourceLevels.Warning);
		private static readonly TraceSource utilSource = new TraceSource("NxBRE.Util", SourceLevels.Warning);
		
		// Trace switches used by NxBRE
		private static bool isFlowEngineVerbose;
		private static bool isFlowEngineInformation;
		private static bool isFlowEngineWarning;
		private static bool isFlowEngineError;
		private static bool isFlowEngineCritical;
		private static bool isFlowEngineRuleBaseError;
		private static bool isFlowEngineRuleBaseCritical;
		private static bool isInferenceEngineVerbose;
		private static bool isInferenceEngineInformation;
		private static bool isInferenceEngineWarning;
		private static bool isUtilVerbose;
		private static bool isUtilWarning;
		
		/// <summary>
		/// The Trace Source used by NxBRE.FlowEngine
		/// </summary>
		public static TraceSource FlowEngineSource {
			get {
				return flowEngineSource;
			}
		}
		
		/// <summary>
		/// The Trace Source used by NxBRE.FlowEngine.RuleBase generated traces
		/// </summary>
		public static TraceSource FlowEngineRuleBaseSource {
			get {
				return flowEngineRuleBaseSource;
			}
		}
		
		/// <summary>
		/// The Trace Source used by NxBRE.InferenceEngine
		/// </summary>
		public static TraceSource InferenceEngineSource {
			get {
				return inferenceEngineSource;
			}
		}
		
		/// <summary>
		/// The Trace Source used by NxBRE.Util
		/// </summary>
		public static TraceSource UtilSource {
			get {
				return utilSource;
			}
		}
		
		/// <summary>
		/// Set the different switches used by NxBRE to the levels defined in the different souces
		/// (usually set up in the configuration file).
		/// </summary>
		public static void RefreshBooleanSwitches() {
			isFlowEngineVerbose = FlowEngineSource.Switch.ShouldTrace(TraceEventType.Verbose);
			isFlowEngineInformation = FlowEngineSource.Switch.ShouldTrace(TraceEventType.Information);
			isFlowEngineWarning = FlowEngineSource.Switch.ShouldTrace(TraceEventType.Warning);
			isFlowEngineError = FlowEngineSource.Switch.ShouldTrace(TraceEventType.Error);
			isFlowEngineCritical = FlowEngineSource.Switch.ShouldTrace(TraceEventType.Critical);
			
			isFlowEngineRuleBaseError = FlowEngineRuleBaseSource.Switch.ShouldTrace(TraceEventType.Error);
			isFlowEngineRuleBaseCritical = FlowEngineRuleBaseSource.Switch.ShouldTrace(TraceEventType.Critical);
			
			isInferenceEngineVerbose = InferenceEngineSource.Switch.ShouldTrace(TraceEventType.Verbose);
			isInferenceEngineInformation = InferenceEngineSource.Switch.ShouldTrace(TraceEventType.Information);
			isInferenceEngineWarning = InferenceEngineSource.Switch.ShouldTrace(TraceEventType.Warning);
			
			isUtilVerbose = UtilSource.Switch.ShouldTrace(TraceEventType.Verbose);
			isUtilWarning = UtilSource.Switch.ShouldTrace(TraceEventType.Warning);
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsFlowEngineVerbose {
			get {
				return isFlowEngineVerbose;
			}
			set {
				isFlowEngineVerbose = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsFlowEngineInformation {
			get {
				return isFlowEngineInformation;
			}
			set {
				isFlowEngineInformation = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsFlowEngineWarning {
			get {
				return isFlowEngineWarning;
			}
			set {
				isFlowEngineWarning = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsFlowEngineError {
			get {
				return isFlowEngineError;
			}
			set {
				isFlowEngineError = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsFlowEngineCritical {
			get {
				return isFlowEngineCritical;
			}
			set {
				isFlowEngineCritical = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsFlowEngineRuleBaseError {
			get {
				return isFlowEngineRuleBaseError;
			}
			set {
				isFlowEngineRuleBaseError = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsFlowEngineRuleBaseCritical {
			get {
				return isFlowEngineRuleBaseCritical;
			}
			set {
				isFlowEngineRuleBaseCritical = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsInferenceEngineVerbose {
			get {
				return isInferenceEngineVerbose;
			}
			set {
				isInferenceEngineVerbose = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsInferenceEngineInformation {
			get {
				return isInferenceEngineInformation;
			}
			set {
				isInferenceEngineInformation = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsInferenceEngineWarning {
			get {
				return isInferenceEngineWarning;
			}
			set {
				isInferenceEngineWarning = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsUtilVerbose {
			get {
				return isUtilVerbose;
			}
			set {
				isUtilVerbose = value;
			}
		}
		
		/// <summary>
		/// A trace switch used by NxBRE. It can be changed at runtime by the user.
		/// </summary>
		/// <remarks>
		/// To revert to the value defined in the configuration file, call InitializeSwitches (<see cref="NxBRE.Util.Logger.InitializeSwitches"/>).
		/// </remarks>
		public static bool IsUtilWarning {
			get {
				return isUtilWarning;
			}
			set {
				isUtilWarning = value;
			}
		}

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
