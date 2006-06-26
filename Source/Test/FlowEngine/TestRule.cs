//
// NUnit Test Class for NxBRE
//
namespace NxBRE.Test.FlowEngine
{
	using System;
	using System.Collections;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.Core;

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
			Assert.AreEqual("MetaData   :\nID Type : System.String\nID Str  : md-id\nFactory : NxBRE.Test.FlowEngine.TestRule+MockBRERuleFactory\nStack Loc: 0\n", rr.ToString());
		}
		
	}
}
