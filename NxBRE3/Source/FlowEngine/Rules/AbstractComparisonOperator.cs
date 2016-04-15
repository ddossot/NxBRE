namespace NxBRE.FlowEngine.Rules {
	using System;
	using System.Collections;

	public abstract class AbstractComparisonOperator : IInitializable {
		public bool AcceptsNulls {
			get {
				return true;
			}
		}
		
		private bool defaultValue = false;
				
		public bool Init(object defaultValue)
		{
		    if (!(defaultValue is bool)) return false;
		    this.defaultValue = (bool) defaultValue;
		    return true;
		}

	    protected abstract bool AnalyzeCompared(int compare);
		
		public bool ExecuteComparison(IBRERuleContext ruleContext, IDictionary arguments, object left, object right)
		{
		    if ((left == null) || (right == null)) {
				return defaultValue;	
			}
		    var comparable = left as IComparable;
		    if ((comparable == null) || (!(right is IComparable))) return defaultValue;
		    if ((! right.GetType().IsInstanceOfType(comparable)) || (! right.GetType().IsSubclassOf(comparable.GetType()))) {
		        right = Convert.ChangeType(right, comparable.GetType());
		    }
				
		    return AnalyzeCompared(comparable.CompareTo(right));
		}
	}
}
