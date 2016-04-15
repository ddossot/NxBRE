namespace NxBRE.Util
{
	using System;
	using System.Configuration;

	/// <summary>NxBRE Math utilities.</summary>
	/// <remarks>This class provides basic methods, working only on parameters of same types.
	/// It is a demonstrator of what is feasable.
	/// The user is invited to create his/her own maths utilities.
	/// </remarks>
	/// <author>David Dossot</author>
	public abstract class Maths {
		private Maths() {}
		
		public static short Multiply(params short[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static int Multiply(params int[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static long Multiply(params long[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static float Multiply(params float[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static double Multiply(params double[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static decimal Multiply(params decimal[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static short Divide(params short[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static int Divide(params int[] operand) {

            var result = operand[0];
			for(var i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static long Divide(params long[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static float Divide(params float[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static double Divide(params double[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static decimal Divide(params decimal[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}

		public static short Add(params short[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static int Add(params int[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static long Add(params long[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static float Add(params float[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static double Add(params double[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static decimal Add(params decimal[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}

		public static short Subtract(params short[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static int Subtract(params int[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static long Subtract(params long[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static float Subtract(params float[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static double Subtract(params double[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static decimal Subtract(params decimal[] operand) {
			var result = operand[0];
			for(var i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
	}		
}
