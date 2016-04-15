//
// NUnit v2.1 Test Class for Compilation
//
namespace NxBRE.Test
{
	using System;
	using System.Collections;
	using System.Reflection;
	using System.Reflection.Emit;

	using NxBRE.Util;

	using NUnit.Framework;
	
	[TestFixture]
	public class TestCompilation
	{
		[Test][ExpectedException(typeof(BREException))]
		public void BadExpressionSyntax() {
			Compilation.Evaluate("5 + System.Math.Pow(2d, 5d");
		}
		
		[Test]
		public void NoArgumentExpressions() {
			Assert.AreEqual(5d, Compilation.Evaluate("5d"), "Evaluation without expr:");

			// should work even if prefixed with expr:
			Assert.AreEqual(10d, Compilation.Evaluate("expr:10d"), "Evaluation with expr:");
		}
		
		[Test]
		public void DirectDictionaryExpressions() {
			IDictionary values = new Hashtable();
			values.Add("a", 4d);
			Assert.AreEqual(21d, Compilation.Evaluate("5 + System.Math.Pow(2d, {var:a})", @"\{var:(?<1>[^}]*)\}", values), "Evaluation with var:");
			Assert.AreEqual(31d, Compilation.Evaluate("15 + System.Math.Pow(2d, {$a})", @"\{\$(?<1>[^}]*)\}", values), "Evaluation with $");

			// should work even if prefixed with expr:
			Assert.AreEqual(21d, Compilation.Evaluate("expr:5 + System.Math.Pow(2d, {var:a})", @"\{var:(?<1>[^}]*)\}", values), "Evaluation with var:");
			Assert.AreEqual(31d, Compilation.Evaluate("expr:15 + System.Math.Pow(2d, {$a})", @"\{\$(?<1>[^}]*)\}", values), "Evaluation with $");
		}
		
		[Test]
		public void CompiledDictionaryExpressions() {
			IDictionary values = new Hashtable();
			values.Add("a", 4d);
			
			// compile the evaluator a first time, using the values IDictionary as a template for
			// argument types
			IDictionaryEvaluator compiledEvaluator = Compilation.NewEvaluator("5 + System.Math.Pow(2d, {var:a})", @"\{var:(?<1>[^}]*)\}", values);
			
			// then use the evaluator
			Assert.AreEqual(21d, compiledEvaluator.Run(values));
			
			// and use it again with different values
			values = new Hashtable();
			values.Add("a", 3d);
			Assert.AreEqual(13d, compiledEvaluator.Run(values));
		}

		[Test]
		public void MixedDictionaryExpressions() {
			IDictionary values = new Hashtable();
			values.Add("a", 4d);
			values.Add(1, 3d);
			
			Assert.AreEqual(64d, Compilation.Evaluate("System.Math.Pow({stringVarName:a}, {intVarName:1})",
			                                          @"\{(stringVarName:|intVarName:)(?<1>[^}]*)\}",
			                                          @"\{intVarName:([^}]*)\}",
			                                          values),
			                     "Evaluation with String and Int32 arguments");
		}

		[Test][ExpectedException(typeof(BREException))]
		public void MissingArgumentDictionaryExpressions() {
			IDictionary values = new Hashtable();
			values.Add("b", 4d);
			Assert.AreEqual(21d, Compilation.Evaluate("5 + System.Math.Pow(2d, {var:a})", @"\{var:(?<1>[^}]*)\}", values), "Evaluation with var:");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void MissingArgumentMixedDictionaryExpressions() {
			IDictionary values = new Hashtable();
			values.Add("a", 4d);
			values.Add(2, 3d);
			
			Assert.AreEqual(64d, Compilation.Evaluate("System.Math.Pow({stringVarName:a}, {intVarName:1})",
			                                          @"\{(stringVarName:|intVarName:)(?<1>[^}]*)\}",
			                                          @"\{intVarName:([^}]*)\}",
			                                          values),
			                     "Evaluation with String and Int32 arguments");
		}
		
		[Test][ExpectedException(typeof(System.InvalidCastException))]
		public void CompileThenChangeTypeError() {
			IDictionary values = new Hashtable();
			values.Add("a", 4d);
			
			IDictionaryEvaluator compiledEvaluator = Compilation.NewEvaluator("5 + System.Math.Pow(2d, {var:a})", @"\{var:(?<1>[^}]*)\}", values);
			Assert.AreEqual(21d, compiledEvaluator.Run(values));
			
			// change the type of a, the evaluation should fail
			values = new Hashtable();
			values.Add("a", "4");
			Assert.AreEqual(21d, compiledEvaluator.Run(values));
		}
		
		[Test]
		public void DirectListExpressions() {
			IList variables = new ArrayList();
			variables.Add("a");
			variables.Add("b");
			IList values = new ArrayList();
			values.Add(5d);
			values.Add(4d);
			Assert.AreEqual(21d, Compilation.Evaluate("{var:a} + System.Math.Pow(2d, {var:b})", @"\{var:(?<1>[^}]*)\}", variables, values), "Evaluation with var:");
			
			// should work even if prefixed with expr:
			values = new ArrayList();
			values.Add(15d);
			values.Add(4d);
			Assert.AreEqual(31d, Compilation.Evaluate("expr:{$a} + System.Math.Pow(2d, {$b})", @"\{\$(?<1>[^}]*)\}", variables, values), "Evaluation with $");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void MissingValueDirectListExpressions() {
			IList variables = new ArrayList();
			variables.Add("a");
			variables.Add("b");
			IList values = new ArrayList();
			values.Add(5d);
			Assert.AreEqual(21d, Compilation.Evaluate("{var:a} + System.Math.Pow(2d, {var:b})", @"\{var:(?<1>[^}]*)\}", variables, values), "Evaluation with var:");
		}
		
		[Test]
		public void DynamicallyBuiltAssemblyInDomain() {
			// Regression test for bug 1482753
			AssemblyName an = new AssemblyName();
			an.Version = new Version(1, 0, 0, 0);
			an.Name = "TestDynamicAssembly";
			AssemblyBuilder ab = System.AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Save);
			
			Assert.AreEqual(5d,	Compilation.Evaluate("5d"), "Evaluation without expr:");
		}
		
	}
}
