// Dummy object for unit testing
namespace NxBRE.Test
{
	using System;
	
	using NxBRE.Util;
	
	public class TestObject {
		
		public  bool MyField;
		private Int32 property;
		private string method;
	
		public object GetNull() {
			return null;
		}
	
		public void VoidMember() {
			// NOOP
		}
	
		public static Int32 Add(int a, int b) {
			return a+b;
		}
	
		public static Double Operate1(double a, params int[] b) {
			return a * Maths.Multiply(b);
		}
	
		public static Int32 Operate2(int a, params int[] b) {
			return Add(a, Maths.Multiply(b));
		}
	
		public TestObject(bool myField, Int32 myProperty, string myMethod)
		{
			this.MyField = myField;
			this.property = myProperty;
			this.method = myMethod;
		}
		
		public Int32 MyProperty {
			get {
				return property;
			}
			set {
				property = value;
			}
		}
		
		public string MyMethod() {
				return method;
		}
		
		public void	MyMethod(string value) {
				method = value;
		}
		
		public void InheritMethod(TestObject to) {
			MyMethod(to.MyMethod());
		}
		
		public override string ToString() {
			// this value is used in the dynamic rule set invocation test
			return "ANOTHER";
		}
		
	}
}
