namespace org.nxbre.examples
{
	using System;
	
	public class Transaction {
		private int pk;
		private DateTime dtm;
		private Money amount;
		
		public DateTime DTM {
			get {
				return dtm;
			}
			set {
				dtm = value;
			}
		}
		
		public Money Amount {
			get {
				return amount;
			}
			set {
				amount = value;
			}
		}
		
		public int PK {
			get {
				return pk;
			}
			set {
				pk = value;
			}
		}
		
		public Transaction(int pk, DateTime dtm, Money amount)
		{
			this.pk = pk;
			this.dtm = dtm;
			this.amount = amount;
		}
		
		public override string ToString()
		{
			return "T[PK:" + PK + "," + Amount + "@" + DTM.ToShortDateString() + "]";
		}
		
		public override int GetHashCode() {
			return pk;
		}

	}
}
