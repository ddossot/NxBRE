/*
 * Created by SharpDevelop.
 * User: David Dossot
 * Date: 6/9/2007
 * Time: 3:03 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

using NxDSL;

using Antlr.Runtime;

namespace NxDSL.Examples
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			InferenceRules_ENParser ipr = new InferenceRules_ENParser(
								new CommonTokenStream(
									new InferenceRules_ENLexer(
										new ANTLRFileStream("../../../../Rulefiles/discount.nxdsl"))));
			ipr.rulebase();
		}
	}
}
