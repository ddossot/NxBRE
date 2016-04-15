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
			short result = operand[0];
			for(int i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static int Multiply(params int[] operand) {
			int result = operand[0];
			for(int i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static long Multiply(params long[] operand) {
			long result = operand[0];
			for(int i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static float Multiply(params float[] operand) {
			float result = operand[0];
			for(int i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static double Multiply(params double[] operand) {
			double result = operand[0];
			for(int i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static decimal Multiply(params decimal[] operand) {
			decimal result = operand[0];
			for(int i=1;i<operand.Length;i++) result *= operand[i];
			return result;
		}
		
		public static short Divide(params short[] operand) {
			short result = operand[0];
			for(int i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static int Divide(params int[] operand) {
			int result = operand[0];
			for(int i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static long Divide(params long[] operand) {
			long result = operand[0];
			for(int i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static float Divide(params float[] operand) {
			float result = operand[0];
			for(int i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static double Divide(params double[] operand) {
			double result = operand[0];
			for(int i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}
		
		public static decimal Divide(params decimal[] operand) {
			decimal result = operand[0];
			for(int i=1;i<operand.Length;i++) result /= operand[i];
			return result;
		}

		public static short Add(params short[] operand) {
			short result = operand[0];
			for(int i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static int Add(params int[] operand) {
			int result = operand[0];
			for(int i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static long Add(params long[] operand) {
			long result = operand[0];
			for(int i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static float Add(params float[] operand) {
			float result = operand[0];
			for(int i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static double Add(params double[] operand) {
			double result = operand[0];
			for(int i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}
		
		public static decimal Add(params decimal[] operand) {
			decimal result = operand[0];
			for(int i=1;i<operand.Length;i++) result += operand[i];
			return result;
		}

		public static short Subtract(params short[] operand) {
			short result = operand[0];
			for(int i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static int Subtract(params int[] operand) {
			int result = operand[0];
			for(int i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static long Subtract(params long[] operand) {
			long result = operand[0];
			for(int i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static float Subtract(params float[] operand) {
			float result = operand[0];
			for(int i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static double Subtract(params double[] operand) {
			double result = operand[0];
			for(int i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
		public static decimal Subtract(params decimal[] operand) {
			decimal result = operand[0];
			for(int i=1;i<operand.Length;i++) result -= operand[i];
			return result;
		}
		
	}		
}
