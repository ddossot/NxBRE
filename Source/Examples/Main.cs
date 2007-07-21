namespace NxDSL.Examples {
	using System;
	using System.Collections.Generic;
	
	using NxDSL;
	
	using Antlr.Runtime;

	public class MainClass {
		public static void Main(string[] args)
		{
			
			InferenceRules_ENParser ipr = new InferenceRules_ENParser(
								new CommonTokenStream(
									new InferenceRules_ENLexer(
										new ANTLRFileStream("../../../../Rulefiles/discount.nxdsl"))));
			try {
				ipr.rulebase();
			} catch(DslException de) {
				Console.WriteLine(de.Message);
			}
		}
	}
}
