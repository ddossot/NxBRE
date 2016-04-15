//
// NUnit Test Class for NxBRE
//
namespace NxBRE.Test.FlowEngine
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.Core;
	using NxBRE.FlowEngine.Rules;

	using NUnit.Framework;
	
	[TestFixture]
	public class TestRule
	{
		private class MockBRERuleFactory : IBRERuleFactory {
			public object ExecuteRule(IBRERuleContext aBRC, IDictionary aMap, object aStep) {
				return null;
			}
		}
		
		[Test]
		public void IBRERuleResultToString() {
			// regression test for bug 1502909
			IBRERuleMetaData rmd = new BRERuleMetaDataImpl("md-id", new MockBRERuleFactory(), null, 0, null);
			
			IBRERuleResult rr = new BRERuleResultImpl(rmd, "result1");
			Assert.AreEqual("MetaData   :\nID Type : System.String\nID Str  : md-id\nFactory : NxBRE.Test.FlowEngine.TestRule+MockBRERuleFactory\nStack Loc: 0\nResult Type: System.String\nResult Str : result1\n", rr.ToString());
			
			rr = new BRERuleResultImpl(null, "result2");
			Assert.AreEqual("Result Type: System.String\nResult Str : result2\n", rr.ToString());
			
			rr = new BRERuleResultImpl(rmd, null);
			Assert.AreEqual("MetaData   :\nID Type : System.String\nID Str  : md-id\nFactory : NxBRE.Test.FlowEngine.TestRule+MockBRERuleFactory\nStack Loc: 0\nResult: Null", rr.ToString());
		}
		
		[Test]
		public void MatchesOperator() {
			Matches matches = new Matches();
			
			Assert.IsFalse(matches.ExecuteComparison(null, null, null, null));
			
	    object[] tests = {"-42", true, "19.99", true, "0.001", false, "100 USD", false, null, false};
	    for(int j=0; j<2; j++) {
	    	// we do it twice to see if the internal regex cache does not have any bad side effect
				for(int i=0; i<tests.Length; i+=2)
					Assert.AreEqual(tests[i+1], matches.ExecuteComparison(null, null, tests[i], @"^-?\d+(\.\d{2})?$"));
	    }
		}
		
	}
}
