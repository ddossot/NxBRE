//
// NUnit v2.1 Test Class for NxBRE
//
namespace NxBRE.Test.FlowEngine
{
	using System;
	using System.IO;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.IO;
	using NxBRE.FlowEngine.Factories;
	
	using NxBRE.Util;

	using NUnit.Framework;
	
	[TestFixture]
	public class TestDrivers
	{
		private string testFile;
		private string testHTTP;
		private string testNative;
		private string identityXSL;
		
		private IFlowEngine breTest;
		
		[TestFixtureSetUp]
		public void InitTest()
		{
			testFile = Parameter.GetString("unittest.inputfile");
			testHTTP = Parameter.GetString("unittest.inputhttp");
			testNative = Parameter.GetString("unittest.inputnative");
			identityXSL = Parameter.GetString("unittest.identityxsl");
		}
		
		[SetUp]
		public void ResetBRE() {
			breTest = null;
		}
		
		[Test]
		public void BusinessRulesFileDriver()
		{
			breTest = new BREFactoryConsole(0,0).NewBRE(new BusinessRulesFileDriver(testNative));
			Assert.IsNotNull(breTest, "File System");
		}
		
		[Test]
		public void XSLTRulesFileDriver_XSLTObject()
		{
			breTest = new BREFactoryConsole(0,0).NewBRE(new XSLTRulesFileDriver(testNative, Xml.IdentityXSLT));
			Assert.IsNotNull(breTest, "XslTransform Object");
		}
		
		[Test]
		public void XSLTRulesFileDriver_FS()
		{
			breTest = new BREFactoryConsole(0,0).NewBRE(new XSLTRulesFileDriver(testNative, identityXSL));
			Assert.IsNotNull(breTest, "XSLT File System");
		}
		
		[Test]
		public void XBusinessRulesFileDriver_FS()
		{
			breTest = new BREFactoryConsole(0,0).NewBRE(new XBusinessRulesFileDriver(testFile));
			Assert.IsNotNull(breTest, "XBRE File System");
		}

		[Test]
		public void XBusinessRulesStreamDriver()
		{
			FileStream fs = new FileStream(testFile, FileMode.Open, FileAccess.Read);
			breTest = new BREFactoryConsole(0,0).NewBRE(new XBusinessRulesStreamDriver(fs));
			Assert.IsNotNull(breTest, "XBRE Stream System");
		}
		
		[Test]
		public void XBusinessRulesStringDriver()
		{
			string xmlRules = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
												"<xBusinessRules xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://nxbre.org/xBusinessRules.xsd\">" +
												"<Logic>" +
												"	<If>" +
												"		<And>" +
												"			<GreaterThanEqualTo leftId=\"Incrementor\" rightId=\"MaxAttempts\">" +
												"				<Evaluate id=\"Incrementor\"/>" +
												"				<Integer id=\"MaxAttempts\" value=\"3\"/>" +
												"			</GreaterThanEqualTo>" +
												"		</And>" +
												"		<Do>" +
												"			<False id=\"LoginResult\"/>" +
												"		</Do>" +
												"	</If>" +
												"	<Else>" +
												"		<Evaluate id=\"Incrementor\">" +
												"			<Parameter name=\"Increment\" value=\"1\"/>" +
												"		</Evaluate>" +
												"	</Else>" +
												"</Logic>" +
												"</xBusinessRules>";
			
			breTest = new BREFactoryConsole(0,0).NewBRE(new XBusinessRulesStringDriver(xmlRules));
			Assert.IsNotNull(breTest, "XBRE String");
		}
		
	}
}
