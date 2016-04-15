namespace NxBRE.FlowEngine
{
	using System;
	using System.Collections;

	public delegate object ExecuteRuleDelegate(IBRERuleContext aBRC, IDictionary aMap, object aStep);
	
	/// <summary>An helper class that allows connecting business rules and custom functions
	/// called by delegates.<br/>
	/// It supports two constructors, one expecting a delegate,
	/// the other one able to build the delegate on-the-fly (this probably carries a performance
	/// issue due to reflection) and prevents compile time type-checking.
	/// </summary>
	/// <author>  David Dossot
	/// </author>
	public class BRERuleFactory : IBRERuleFactory
	{
		private ExecuteRuleDelegate executeRuleDelegate = null;
		
		public BRERuleFactory(ExecuteRuleDelegate executeRuleDelegate) {
			this.executeRuleDelegate = executeRuleDelegate;
		}
		
		public BRERuleFactory(object target, string method) {
			executeRuleDelegate = (ExecuteRuleDelegate) Delegate.CreateDelegate(typeof(ExecuteRuleDelegate),
			                                                                    target,
			                                                                    method);
		}
		
		public object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
		{
			return executeRuleDelegate(aBrc, aMap, aStep);
		}
	}
}

