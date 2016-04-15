//
// NUnit Test Class for NxBRE
//
namespace NxBRE.Test
{
	using System;
	using System.Text;
	using System.Xml;
	
	using NxBRE.Util;

	using NUnit.Framework;
	
	[TestFixture]
	public class TestSchema
	{
		private string[] testData = new string[]{ "anyURI", "http://nxbre.org/",
																							"base64Binary", "aGVsbG8gd29ybGQ=", 
																							"boolean", "true", 
																							"byte", "123", 
																							"date", "1999-12-31", 
																							"dateTime", "1999-12-31T23:59:59Z", 
																							"decimal", "123.00", 
																							"double", "123.456", 
																							"duration", "P1895D", 
																							"float", "123.456", 
																							"gDay", "---25", 
																							"gMonthDay", "--01-25", 
																							"gYear", "2005", 
																							"gYearMonth", "2005-12", 
																							"hexBinary", "322034", 
																							"ID", "A123", 
																							"ID", "B456", 
																							"IDREF", "A123", 
																							"IDREFS", "A123 B456", 
																							"int", "123", 
																							"integer", "123", 
																							"language", "en", 
																							"long", "123", 
																							"gMonth", "--12--", 
																							"Name", "NxBRE", 
																							"NCName", "NxBRE", 
																							"negativeInteger", "-123", 
																							"NMTOKEN", "NxBRE", 
																							"NMTOKENS", "NxBRE Rocks", 
																							"nonNegativeInteger", "123", 
																							"nonPositiveInteger", "-123", 
																							"normalizedString", "NxBRE", 
																							"NOTATION", "NxBRE", 
																							"positiveInteger", "123", 
																							"QName", "xsi:type", 
																							"short", "123", 
																							"string", "NxBRE", 
																							"time", "23:59:59", 
																							"token", "NxBRE", 
																							"unsignedByte", "123", 
																							"unsignedInt", "123", 
																							"unsignedLong", "123", 
																							"unsignedShort", "123"};
		
				
		private object[] testObjects = new object[] {	new Uri("http://www.nxbre.org/"), "http://www.nxbre.org/",
																									Encoding.UTF8.GetBytes("hello world") , "aGVsbG8gd29ybGQ=",
																									true, "true",
																									(sbyte)123, "123",
																									XmlConvert.ToDateTime("1999-12-31T23:59:59+01:00", XmlDateTimeSerializationMode.Utc), "1999-12-31T22:59:59Z",
																									new Decimal(987654), "987654",
																									123.456D, "123.456",
																									new TimeSpan(23, 12, 35, 13), "P23DT12H35M13S",
																									"foo", "foo",
																									987.654F, "987.654",
																									-456123, "-456123",
																									987654L, "987654",
																									new XmlQualifiedName("xsi", "type"), "xsi:type",
																									(short)16384, "16384",
																									(byte)64, "64",
																									(uint)753159, "753159",
																									(ulong)456852, "456852",
																									(ushort)4096, "4096"};

		[Test]
		public void CheckImplicitTypeSupport()
		{
			for(int i=0; i<testObjects.Length; i+=2) {
			  object clrValue = testObjects[i];
			  string stringValue = (string)testObjects[i+1];
			  
			  Assert.AreEqual(stringValue, Xml.FromClr(clrValue), "Testing object=" + clrValue + " value=" + stringValue);
			}
		}

		[Test]
		public void CheckAllTypesSupport()
		{
			for(int i=0; i<testData.Length; i+=2) {
			  string schemaType = testData[i];
				string value = testData[i+1];
			  
			  Assert.AreEqual(value, Xml.FromClr(Xml.ToClr(value, schemaType), schemaType), "Testing type=" + schemaType + " value=" + value);
			}
		}
	}
}
