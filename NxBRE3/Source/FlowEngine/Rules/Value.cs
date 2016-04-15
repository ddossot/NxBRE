namespace NxBRE.FlowEngine.Rules
{
    using System;
    using System.Collections;

    using FlowEngine;
    using Util;

    /// <summary> This class is designed to be used anytime you wish to set
    /// a value in the XML itself. 
    /// <P>
    /// The value can be of any type and of any value.
    /// </P>
    /// </summary>
    /// <author>  Sloan Seaman
    /// </author>
    public sealed class Value : IBRERuleFactory
    {

        public const string VALUE = "Value";

        public const string TYPE = "Type";

        /// <summary> Returns a value cast to a specific type
        /// *
        /// </summary>
        /// <param name="aBrc">- The BRERuleContext object
        /// </param>
        /// <param name="aMap">- The IDictionary of parameters from the XML
        /// </param>
        /// <param name="aStep">- The step that it is on
        /// </param>
        /// <returns> The value cast to the specified type
        /// 
        /// </returns>
        public object ExecuteRule(IBRERuleContext aBrc, IDictionary aMap, object aStep)
        {
            if (!aMap.Contains(TYPE))
            {
                throw new BRERuleException("Parameter 'Type' not found");
            }
            else
            {
                var type = (string)aMap[TYPE];

                if (aMap.Contains(VALUE)) return Reflection.CastValue(aMap[VALUE], type);
                var ol = new ObjectLookup();
                var arguments = ol.GetArguments(aMap);
                return CreateValue(type, arguments);
            }
        }

        private object CreateValue(string type, object[] arguments)
        {
            if (type == "System.String" && arguments.Length == 0)
            {
                // special case for bug 2933080
                return String.Empty;
            }
            return Reflection.ClassNew(type, arguments);
        }
    }
}
