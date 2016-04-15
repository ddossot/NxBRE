namespace NxBRE.FlowEngine {
	
	/// <summary>
	/// This interface must be implemented by any class that wants to handle BRE events
	/// </summary>
	/// <author>David Dossot</author>
	/// <remarks>
	///  The delegate of the same name allows creating custom implementation of the handler.
	/// </remarks>
	public interface IBREDispatcher {
		event DispatchRuleResult ResultHandlers;
	}
	
	public delegate void DispatchRuleResult(object sender, IBRERuleResult ruleResult);
}
