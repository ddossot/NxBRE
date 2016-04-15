namespace NxBRE.FlowEngine.Rules
{
    using FlowEngine;

	/// <summary> Compares two objects to see if they are less than or equal to each other.
	/// </summary>
	public sealed class LessThanEqualTo : AbstractComparisonOperator, IBREOperator {
		protected override bool AnalyzeCompared(int compare) {
			return compare <= 0;
		}
	}
}
