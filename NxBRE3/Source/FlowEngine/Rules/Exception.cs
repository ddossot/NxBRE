namespace NxBRE.FlowEngine.Rules
{
    using System.Collections;
    using System.Diagnostics;

    using FlowEngine;
    using Util;

    /// <summary> This rule will always throw an exception.
    /// </summary>
    public class Exception : IBRERuleFactory
    {

        public const string MESSAGE = "Message";

        /// <summary> Throws a BRERuleException with the message "Test Exception"
        /// unless the parameter Message provides a specific message.
        /// *
        /// </summary>
        /// <param name="aBrc">- The BRERuleContext object
        /// </param>
        /// <param name="aMap">- The IDictionary of parameters from the XML
        /// </param>
        /// <param name="aStep">- The step that it is on
        /// </param>
        /// <returns>The Exception</returns>
        public virtual object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
        {
            var message = (string)aMap[MESSAGE];

            var breRuleException = message == null ? new BRERuleException() : new BRERuleException(message);

            if (Logger.IsFlowEngineRuleBaseError) Logger.FlowEngineRuleBaseSource.TraceData(TraceEventType.Error, 0, breRuleException);

            return breRuleException;
        }
    }
}
