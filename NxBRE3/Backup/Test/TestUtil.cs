//
// NUnit Test Class for NxBRE
//
namespace NxBRE.Test
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	
	using NxBRE.FlowEngine.IO;
	using NxBRE.Util;

	using NUnit.Framework;
	
	[TestFixture]
	public class TestUtil
	{
		private string testFile;
		private string outputFolder;
		
		private const double DELTA = .000001;
		
		[TestFixtureSetUp]
		public void InitTest()
		{
			testFile = Parameter.GetString("unittest.inputfile");
			outputFolder = Parameter.GetString("unittest.outputfolder");
		}
		
		[Test]
		public void PseudoCodeRendering()
		{
			PseudoCodeRenderer pcr = new PseudoCodeRenderer(testFile);

			// body
			FileInfo fi = new FileInfo(outputFolder + "/pseudocodebody.html");
			if (fi.Exists) fi.Delete();
			pcr.RenderBody(fi.FullName);
			fi = new FileInfo(outputFolder + "/pseudocodebody.html");
			Assert.IsTrue(fi.Exists, "pseudocodebody.html");

			// index
			fi = new FileInfo(outputFolder + "/pseudocodeindex.html");
			if (fi.Exists) fi.Delete();
			pcr.RenderIndex(fi.FullName, null, "pseudocodebody.html");
			fi = new FileInfo(outputFolder + "/pseudocodeindex.html");
			Assert.IsTrue(fi.Exists, "pseudocodeindex.html");

			// frames
			fi = new FileInfo(outputFolder + "/pseudocodeframes.html");
			if (fi.Exists) fi.Delete();
			pcr.RenderFrameset(fi.FullName,
			                   "NxBRE - Unit Test Rules",
			                   "pseudocodeindex.html",
			                   "pseudocodebody.html");
			fi = new FileInfo(outputFolder + "/pseudocodeframes.html");
			Assert.IsTrue(fi.Exists, "pseudocodeframes.html");
		}
		
		[Test]
		public void GetTaggedInfo() {
			Assert.AreEqual(String.Empty, Parameter.GetTaggedInfo(null, null), "Null Null");
			Assert.AreEqual(String.Empty, Parameter.GetTaggedInfo("foo", null), "Null missing");
			Assert.AreEqual(String.Empty, Parameter.GetTaggedInfo(null, "missing"), "Missing in null");
			Assert.AreEqual(String.Empty, Parameter.GetTaggedInfo("foo", "missing"), "Missing in foo");
			Assert.AreEqual("one", Parameter.GetTaggedInfo("single:one", "single"), "Single");
			Assert.AreEqual("end", Parameter.GetTaggedInfo("first:one;last:end;", "last"), "Last");
			Assert.AreEqual("two", Parameter.GetTaggedInfo("first:one;second:two;last:three", "second"), "Second");
			Assert.AreEqual(String.Empty, Parameter.GetTaggedInfo("first:one;second:two;last:three", "two"), "Confused two");
			Assert.AreEqual(String.Empty, Parameter.GetTaggedInfo("first:one;second:two;last:three", "Second"), "Case matters");
		}
		
		
		[Test]
		public void AreIntersecting() {
			IList a1 = new ArrayList();
			a1.Add("a");
			a1.Add(1);
			a1.Add(3.14);
			IList a2 = new ArrayList();
			a2.Add("b");
			a2.Add(1);
			a2.Add(6.28);
			IList a3 = new ArrayList();
			a3.Add("c");
			a3.Add(2);
			a3.Add(9.42);
			
			Assert.IsTrue(Misc.AreIntersecting(a1, a2), "Intersecting");
			Assert.IsFalse(Misc.AreIntersecting(a1, a3), "Not-Intersecting");
		}

		[Test]
		public void MathOperators() {
			Assert.AreEqual(3, Maths.Multiply(3), "3 * = 3");
			Assert.AreEqual(10, Maths.Multiply(2, 5), "2 * 5 = 10");
			Assert.AreEqual(-210, Maths.Multiply(2, 5, -3, 7), "2 * 5 * -3 * 7 = -210");
			
			Assert.AreEqual(2.5, Maths.Multiply(2.5d),DELTA , "2.5 * = 2.5");
			Assert.AreEqual(-8.75d, Maths.Multiply(2.5d, -3.5d),DELTA , "2.5 * (-3.5) = -8.75");
			Assert.AreEqual(37.625d, Maths.Multiply(2.5d, 3.5d, 4.3d),DELTA , "2.5 * 3.5 * 4.3 = 37.625");
			
			Assert.AreEqual(3, Maths.Add(3), "3 + = 3");
			Assert.AreEqual(7, Maths.Add(2, 5), "2 + 5 = 7");
			Assert.AreEqual(-3, Maths.Add(2, 5, -3, -7), "2 + 5 + (-3) + (-7) = -3");
			
			Assert.AreEqual(2.5, Maths.Add(2.5d),DELTA , "2.5 + = 2.5");
			Assert.AreEqual(-1.0d, Maths.Add(2.5d, -3.5d),DELTA , "2.5 + (-3.5) = -1.0");
			Assert.AreEqual(1.7d, Maths.Add(2.5d, 3.5d, -4.3d), DELTA, "2.5 + 3.5 + (-4.3) = 1.7");
			
			Assert.AreEqual(3, Maths.Subtract(3), "3 - = 3");
			Assert.AreEqual(-3, Maths.Subtract(2, 5), "2 - 5 = -3");
			Assert.AreEqual(7, Maths.Subtract(2, 5, -3, -7), "2 - 5 - (-3) - (-7) = 7");
			
			Assert.AreEqual(2.5d, Maths.Subtract(2.5d),DELTA , "2.5 - = 2.5");
			Assert.AreEqual(6.0d, Maths.Subtract(2.5d, -3.5d),DELTA , "2.5 - (-3.5) = 6.0");
			Assert.AreEqual(3.3d, Maths.Subtract(2.5d, 3.5d, -4.3d),DELTA , "2.5 - 3.5 - (-4.3) = 3.3");
			
			Assert.AreEqual(3, Maths.Divide(3), "3 / = 3");
			Assert.AreEqual(4, Maths.Divide(20, 5), "20 / 5 = 4");
			Assert.AreEqual(1, Maths.Divide(100, 5, -4, -5), "100 / 5 / (-4) / (-5) = 1");
			
			Assert.AreEqual(2.5d, Maths.Divide(2.5d),DELTA , "2.5 / = 2.5");
			Assert.AreEqual(.4d, Maths.Divide(2d, 5d),DELTA , "2 / 5 = .4");
			Assert.AreEqual(.1d, Maths.Divide(2d, 5d, -.4d, -10d),DELTA , "2 / 5 / (-.4) / (-10) = .1");
		}
		
		[Test]
		public void RowSetAccess() {
			TestDataSet.Table1DataTable dt = new TestDataSet.Table1DataTable();
			TestDataSet.Table1Row row = dt.NewTable1Row();
			row.col1="foo";
			row.col2=2004;
			
			Assert.AreEqual("foo", DataAccess.GetDataRowColumnValue(row, "col1"), "(1)GetDataRowColumnValue");
			Assert.AreEqual(2004, DataAccess.GetDataRowColumnValue(row, "col2"), "(2)GetDataRowColumnValue");
			DataAccess.SetDataRowColumnValue(row, "col1", "turnip");
			DataAccess.SetDataRowColumnValue(row, "col2", 1969);
			Assert.AreEqual("turnip", DataAccess.GetDataRowColumnValue(row, "col1"), "(3)GetDataRowColumnValue");
			Assert.AreEqual(1969, DataAccess.GetDataRowColumnValue(row, "col2"), "(4)GetDataRowColumnValue");
		}
		
		[Test]
		public void CastValue() {
			Assert.AreEqual(123, Reflection.CastValue("123", typeof(byte)), "string to byte");
			Assert.AreEqual("987", Reflection.CastValue(987, typeof(string)), "int to string");
			
			Assert.AreEqual(new System.Exception("dummy").ToString(), Reflection.CastValue("dummy", typeof(Exception)).ToString(), "string to exception");
			
			// added for bug 1474032
			Assert.AreEqual(FileAccess.ReadWrite, Reflection.CastValue("ReadWrite", typeof(FileAccess)), "enum");
		}
		
		[Test]
		public void CastToStrongTypeUnchanged() {
			object o1 = 25;
			object o2 = 75;
			ObjectPair pair = new ObjectPair(o1, o2);
			Reflection.CastToStrongType(pair);
			o1 = pair.First;
			o2 = pair.Second;
			
			Assert.IsTrue(o1 is Int32, "(1) o1 type");
			Assert.AreEqual(o1, 25, "(1) o1 value");
			Assert.IsTrue(o2 is Int32, "(1) o2 type");
			Assert.AreEqual(o2, 75, "(1) o2 value");

			o1 = "25";
			o2 = "75";
			pair = new ObjectPair(o1, o2);
			Reflection.CastToStrongType(pair);
			o1 = pair.First;
			o2 = pair.Second;

			Assert.IsTrue(o1 is String, "(2) o1 type");
			Assert.AreEqual(o1, "25", "(2) o1 value");
			Assert.IsTrue(o2 is String, "(2) o2 type");
			Assert.AreEqual(o2, "75", "(2) o2 value");
		}
		
		[Test]
		public void CastToStrongTypeChanged() {
			object o1 = "25";
			object o2 = 75;
			ObjectPair pair = new ObjectPair(o1, o2);
			Reflection.CastToStrongType(pair);
			o1 = pair.First;
			o2 = pair.Second;

			Assert.IsTrue(o1 is Int32, "(1) o1 type");
			Assert.AreEqual(o1, 25, "(1) o1 value");
			Assert.IsTrue(o2 is Int32, "(1) o2 type");
			Assert.AreEqual(o2, 75, "(1) o2 value");

			o1 = Math.PI;
			o2 = "-6.55";
			pair = new ObjectPair(o1, o2);
			Reflection.CastToStrongType(pair);
			o1 = pair.First;
			o2 = pair.Second;

			Assert.IsTrue(o1 is Double, "(2) o1 type");
			Assert.AreEqual(o1, Math.PI, "(2) o1 value");
			Assert.IsTrue(o2 is Double, "(2) o2 type");
			Assert.AreEqual(o2, -6.55d, "(2) o2 value");
		}
		
		[Test]
		public void EmptyNotMutable() {
			Assert.IsTrue(Misc.EMPTY_DICTIONARY.IsReadOnly, "EMPTY_DICTIONARY");
			Assert.IsTrue(Misc.EMPTY_LIST.IsReadOnly, "EMPTY_LIST");
		}
		
		[Test]
		public void ParseOperatorCall() {
			ObjectPair result = Parameter.ParseOperatorCall(@"Matches(^-?\d+(\.\d{2})?$)");
			Assert.AreEqual("Matches", result.First);
			Assert.AreEqual(@"^-?\d+(\.\d{2})?$", result.Second);
		}
		
		[Test]
		public void GroupFinalNoNullValues() {
			// Regression test for bug 1926553
			object[] args = new object[]{"foo", 1, 2, 3};
			object[] result = Parameter.GroupFinal(args);
			
			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(args[0], result[0]);
			Assert.IsTrue(result[1] is int[]);
			Assert.AreEqual(3, ((int[])result[1]).Length);
		}
		
		[Test]
		public void GroupFinalWithMiddleNullValue() {
			// Regression test for bug 1926553 
			object[] args = new object[]{"foo", null, 1, 2, 3};
			object[] result = Parameter.GroupFinal(args);
			
			Assert.AreEqual(3, result.Length);
			Assert.AreEqual(args[0], result[0]);
			Assert.IsNull(result[1]);
			Assert.IsTrue(result[2] is int[]);
			Assert.AreEqual(3, ((int[])result[2]).Length);			
		}
		
		[Test]
		public void GroupFinalWithBeginNullValue() {
			// Regression test for bug 1926553 
			object[] args = new object[]{null, "foo", 1, 2, 3};
			object[] result = Parameter.GroupFinal(args);
			
			Assert.AreEqual(3, result.Length);
			Assert.IsNull(result[0]);
			Assert.AreEqual(args[1], result[1]);
			Assert.IsTrue(result[2] is int[]);
			Assert.AreEqual(3, ((int[])result[2]).Length);			
		}		
		
		[Test]
		public void GroupFinalWithEndNullValue() {
			// Regression test for bug 1926553 
			object[] args = new object[]{"foo", 1, 2, 3, null};
			
			Assert.AreEqual(args, Parameter.GroupFinal(args));
		}
				
		[Test]
		public void GroupFinalWithEndNullValues() {
			// Regression test for bug 1926553 
			object[] args = new object[]{"foo", null, null, null};
			
			Assert.AreEqual(args, Parameter.GroupFinal(args));
		}	
	}
}
