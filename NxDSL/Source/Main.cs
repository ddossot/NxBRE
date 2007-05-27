/*
 * User: David Dossot
 * Date: 5/27/2007
 * Time: 1:28 PM
 */
using System;
using System.Collections.Generic;

using Antlr.Runtime;

class MainClass
{
	public static void Main(string[] args)
	{
		InferenceRules_ENParser ipr = new InferenceRules_ENParser(
										new CommonTokenStream(
											new InferenceRules_ENLexer(
												new ANTLRFileStream("../../../Rulefiles/discount.nxdsl"))));
		ipr.rulebase();
		
		Console.WriteLine("Done!");
		Console.ReadLine();
	}
}
