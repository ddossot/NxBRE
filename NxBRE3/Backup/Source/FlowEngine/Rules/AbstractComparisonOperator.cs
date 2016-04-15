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
				
		public bool Init(object defaultValue) {
			if (defaultValue is bool) {
				this.defaultValue = (bool) defaultValue;
				return true;
			} else {
				return false;
			}
		}
		
		protected abstract bool AnalyzeCompared(int compare);
		
		public bool ExecuteComparison(IBRERuleContext ruleContext, IDictionary arguments, object left, object right) {
			if ((left == null) || (right == null)) {
				return defaultValue;	
			}
			else if ((left is IComparable) && (right is IComparable)) {
				if ((! right.GetType().IsInstanceOfType(left)) || (! right.GetType().IsSubclassOf(left.GetType()))) {
					right = Convert.ChangeType(right, left.GetType());
				}
				
				return AnalyzeCompared(((IComparable) left).CompareTo(right));
			}
			else {
				return defaultValue;
			}
		}

	}
}
