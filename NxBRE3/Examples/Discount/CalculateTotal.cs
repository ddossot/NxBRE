namespace NxBRE.Examples
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.IO;
	using System.Xml;
	using System.Xml.Schema;
	using System.Xml.Xsl;

	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.Factories;
	using NxBRE.FlowEngine.IO;
	using NxBRE.FlowEngine.Rules;
	
	using NxBRE.Util;
	
	public class CalculateTotal
	{
		public const string ORDER = "CurrentOrder";
		public const string APPLIED_DISCOUNT = "AppliedDiscount";
		public const string PERCENT = "Percent";
		
		private IFlowEngine bre;
		private bool reusing = false;
		
		public CalculateTotal(string aXMLFile, SourceLevels engineTraceLevel, SourceLevels ruleBaseTraceLevel)
		{
			bre = new BREFactoryConsole(engineTraceLevel, ruleBaseTraceLevel).NewBRE(new XBusinessRulesFileDriver(aXMLFile));
			if (bre == null) throw new System.Exception("BRE Not Properly Initialized!");
			
			bre.RuleContext.SetFactory(APPLIED_DISCOUNT,
			    						           new BRERuleFactory(new ExecuteRuleDelegate(AppliedDiscount)));
			
			// example on how to export native rules file
			FileStream fs = new FileStream("discount.bre", FileMode.Create);
			Xml.IdentityXSLT.Transform(bre.XmlDocumentRules, null, fs);
			fs.Flush();
			fs.Close();
		}
		
		public bool IsValid {
			get {
				return (bre != null);
			}
		}

		
		// <summary> Delegate that benefits from the rule context to calculate the discount.
		// The result of the discount calculation will be placed in the Rule Context as a Result object.
		// This result will have to be read later (in the GetTotal method).
		// 
		// Instead of this, we could have used reflection calls on static methods and Order object
		// from the rules file to directly modify the business object.
		// </summary> 
		public object AppliedDiscount(IBRERuleContext aBrc, IDictionary aMap, object aStep) {
			return Discount.CalculateDiscount(((Order)aBrc.GetObject(ORDER)).TotalCost,
			                                  (double)Reflection.CastValue(aMap[PERCENT], typeof(System.Double)));
		}
		
		/// <summary> Lets pretent that we have an Object called Order and it has all
		/// relevant order information including an Object for the Product that
		/// is ordered
		/// </summary>
		public double GetTotal(Order aOrder)
		{
			if (IsValid) {
				// Since we are re-using this engine and because we do not want the previous results
				// to remain in context, let's reset.
				if (reusing) bre.Reset();
				else reusing = true;
				
				bre.RuleContext.SetObject(ORDER, aOrder);
				bre.Process();
				if (MainClass.SHOWSTACK) Console.Out.WriteLine(bre.RuleContext);
				
				// At this point, calling GetResult(APPLIED_DISCOUNT).Result gets the value
				// placed in the rule context by the call to AppliedDiscount made by the engine
				// with the discount rate coming from the rule file.
				return (System.Double) bre.RuleContext.GetResult(APPLIED_DISCOUNT).Result;
			}
			else return -1;
		}
		
	}
}
