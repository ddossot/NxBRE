namespace NxDSL.Examples {
	using System;
	using System.Collections.Generic;
	
	using NxDSL;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	
	using Antlr.Runtime;

	public class MainClass {
		public static void Main(string[] args) {
			ProcessRuleBase(new DslAdapter("../../../../Rulefiles/discount.nxdsl"));
			
			Console.WriteLine();
			
			ProcessRuleBase(new DslAdapter("../../../../Rulefiles/remise.nxdsl", DslAdapter.GrammarLanguages.FR));
		}
		
		private static void ProcessRuleBase(DslAdapter adapter) {
			IEImpl ie = new IEImpl();
			ie.LoadRuleBase(adapter);
			ie.Process();
			
			for(IEnumerator<Fact> e = ie.Facts; e.MoveNext();) {
				Console.WriteLine(e.Current);
			}			
		}
	}
}
