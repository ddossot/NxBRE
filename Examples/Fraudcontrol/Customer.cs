namespace NxBRE.Examples
{
	using System;
	using System.Collections;
	
	public class Customer {
		private bool fraudulent;
		private int pk;
		private string countryCode;
		private ArrayList transactions;
		
		public bool Fraudulent {
			get {
				return fraudulent;
			}
			set {
				fraudulent = value;
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
		
		public string CountryCode {
			get {
				return countryCode;
			}
			set {
				countryCode = value;
			}
		}
		
		public ArrayList Transactions {
			get {
				return transactions;
			}
			set {
				transactions = value;
			}
		}
		
		public Customer(int pk, string countryCode, ArrayList transactions)
		{
			this.fraudulent = false;
			this.pk = pk;
			this.countryCode = countryCode;
			this.transactions = transactions;
		}
		
		public override string ToString() {
			return "Customer-"+PK+((Fraudulent)?"-F!":"");
		}
		
		public override int GetHashCode() {
			return pk;
		}
		
	}
}
