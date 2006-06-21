namespace org.nxbre.rule {
	using net.ideaity.util.events;
	
	/// <summary>
	/// Provides default implementations for BRE implementations, which deal with not only
	/// log and exception events, but also rule events.
	/// </summary>
	/// <author>David Dossot</author>
	public abstract class AbstractBREDispatcher:AbstractLogExceptionDispatcher, IBREDispatcher {
		public event DispatchRuleResult ResultHandlers;
		
		public void DispatchRuleResult(IBRERuleResult ruleResult) {
			if (ResultHandlers != null)
				ResultHandlers(this, ruleResult);
		}
		
		protected DispatchRuleResult GetResultHandlers() {
			return ResultHandlers;
		}
	}
}
