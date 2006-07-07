namespace NxBRE.Examples
{
	using System;
	
	public class Money {
		private string code;
		private double amount;
		
		public string Code {
			get {
				return code;
			}
			set {
				code = value;
			}
		}
		
		public double Amount {
			get {
				return amount;
			}
			set {
				amount = value;
			}
		}
		
		public Money(string code, double amount)
		{
			this.code = code;
			this.amount = amount;
		}
		
		public override string ToString()
		{
			return Amount + Code;
		}

		public override int GetHashCode()
		{
			return Code.GetHashCode() ^ Amount.GetHashCode();
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Money) return (((Money)obj).Code == Code) & (((Money)obj).Amount == Amount);
			
			return false;
		}
		
	}
}
