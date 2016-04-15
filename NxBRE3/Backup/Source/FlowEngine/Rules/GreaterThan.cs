namespace NxBRE.FlowEngine.Rules {
	using System;
	using System.Collections;

	using NxBRE.FlowEngine;

	/// <summary> Compares two objects to see if one is greater than the other
	/// </summary>
	public sealed class GreaterThan : AbstractComparisonOperator, IBREOperator {
		protected override bool AnalyzeCompared(int compare) {
			return compare > 0;
		}
	}
}
