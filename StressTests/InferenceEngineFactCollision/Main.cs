namespace NxBRE.StressTests
{
	using System;
	
	public class MainClass
	{
		public static void Main(string[] args) 
		{
			new WeightError(Int32.Parse(args[0]), args[1]).PerformProcess(new CustomBinder());
		}
	}
}
