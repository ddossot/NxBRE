namespace NxBRE.FlowEngine
{
	using System;
	using System.Xml.XPath;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.IO;
	
	/// <summary>
	/// This interface defines the Flow Engine (FE) of NxBRE.
	/// </summary>
	/// <author>David Dossot</author>
	public interface IFlowEngine : IBREDispatcher, ICloneable {
			/// <summary> Returns or Sets the RuleContext in it's current state.
			/// If the developer wishes to have a private copy, make sure
			/// to use Clone().
			/// This method allows developers to provide an already populated BRERuleContext.
			/// This is provided to allow for RuleFactories that have already been created, thus 
			/// allowing a more stateful RuleFactory
			/// </summary>
			/// <returns> The RuleContext in its current state</returns>
			IBRERuleContext RuleContext
			{
				get;
				
				set;
			}
			
			/// <summary> Returns the loaded XML Rules in the native NxBRE syntax
			/// </summary>
			/// <returns> The loaded XmlDocumentRules</returns>
			XPathDocument XmlDocumentRules
			{
				get;
			}
			
			/// <summary> Running state of the engine, i.e. when processing.
			/// </summary>
			/// <returns> True if the engine is processing. </returns>
			bool Running
			{
				get;
			}
			
			/// <summary>
			/// Initialize the engine with a specific rule base.
			/// </summary>
			/// <param name="rulebase"></param>
			/// <returns></returns>
			bool Init(XPathDocument rulebase);
			
			/// <summary>
			/// Initialize the engine by loading rules from a rules driver.
			/// </summary>
			/// <param name="rulesDriver"></param>
			/// <returns></returns>
			bool Init(IRulesDriver rulesDriver);
			
			/// <summary>
			/// Process the rule base, only executing the rules not defined in any set.
			/// </summary>
			/// <returns>
			/// True if successful, False otherwise
			/// </returns>
			bool Process();
			
			
			/// <summary>
			/// Process the rule base, only executing the rules not defined in any set
			/// <b>and</b> defined in the set whose Id is passed as a parameter.
			/// </summary>
			/// <param name="setId">The ID of the set to execute</param>
			/// <returns> True if successful, False otherwise
			/// </returns>
			bool Process(string setId);
			
			/// <summary>
			/// Schedule the execution of sets to try to create the passed object ID in the rule context (backward chaining). 
			/// </summary>
			/// <param name="objectId">The ID to resolve.</param>
			/// <returns>The value of the object ID to resolve, or null if the resolution was not possible.</returns>
			object Resolve(string objectId);
			
			/// <summary>
			/// Schedule the execution of sets to try to create the passed object ID in the rule context (backward chaining). 
			/// </summary>
			/// <param name="objectId">The ID to resolve.</param>
			/// <param name="defaultValue">The value to return if the ID was not resolvable.</param>
			/// <returns>The value of the object ID to resolve, or defaultValue if the resolution was not possible.</returns>
			object Resolve(string objectId, object defaultValue);
			
			/// <summary> Violently stop the BRE 
			/// </summary>
			void Stop();
			
			/// <summary>Reset the context's call stack and results
			/// </summary>
			void Reset();
		}
}
