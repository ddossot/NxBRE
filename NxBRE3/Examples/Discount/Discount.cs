namespace NxBRE.Examples
{
	using System;
	
	/// <summary> This is just a little util class that could/would calculate a discount for us.
	/// <P>
	/// Very simple one shown here :)
	/// </summary>
	public class Discount
	{
		public static System.Double CalculateDiscount(System.Double cost, System.Double percent)
		{
			return cost * percent;
		}
	}
}
