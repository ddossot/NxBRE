namespace NxDSL.Examples {
	using System;
	using System.Collections.Generic;
	
	using NxDSL;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	
	using Antlr.Runtime;

	public class MainClass {
		public static void Main(string[] args) {
			string dslFile = "../../../../Rulefiles/discount.nxdsl";

			IEImpl ie = new IEImpl();
			ie.LoadRuleBase(new DslAdapter(dslFile));
			ie.Process();
			
			for(IEnumerator<Fact> e = ie.Facts; e.MoveNext();) {
				Console.WriteLine(e.Current);
			}
		}
	}
}
