namespace org.nxbre.examples
{
	using System;
	using System.Collections;

	public class DummyData {
		// Singleton for generating dummy customers & transactions
		private static DummyData dd = null;
		
		private int nbDecaCustomers;
		private int generatedCustomers = 0;
		private int generatedTransactions = 0;
		private Random rnd = new Random();
			
		public static DummyData GetInstance() {
			if (dd == null) dd = new DummyData();
			return dd;
		}
		
		public Hashtable GetBusinessObjects(int nbDecaCustomers) {
			this.nbDecaCustomers = nbDecaCustomers;
			generatedCustomers = 0;
			generatedTransactions = 0;
			
			ArrayList customers = GetCustomers();

			if (FraudControl.LOG_LEVEL <= net.ideaity.util.events.LogEventImpl.INFO)
				Console.WriteLine("\nGenerated {0} customers and {1} transactions.\n",
				                  generatedCustomers,
				                  generatedTransactions);
			
			Hashtable businessObjects = new Hashtable();
			businessObjects.Add("CUSTOMERS", customers);
			businessObjects.Add("MNYCNV", new MoneyConverter());
			
			return businessObjects;
		}
	
		private ArrayList GetCustomers() {
			ArrayList customers = new ArrayList();
			
			for(int i=0; i<nbDecaCustomers; i++) {
				// A bunch of jolly cool customers
				customers.Add(new Customer(1 + i*10, "CC01", GetTransactions("EUR", 0)));
				customers.Add(new Customer(2 + i*10, "CC02", GetTransactions("USD", 0)));
				customers.Add(new Customer(3 + i*10, "CC03", GetTransactions("EUR", 0)));
				customers.Add(new Customer(4 + i*10, "CC04", GetTransactions("USD", 0)));
				
				// A bunch of suspicious though still jolly cool customers
				customers.Add(new Customer(5 + i*10, "CC01", GetTransactions("EUR", 149999d)));
				customers.Add(new Customer(6 + i*10, "CC02", GetTransactions("USD", 150000d)));
				customers.Add(new Customer(7 + i*10, "CC03", GetTransactions("EUR", 200000d)));
				customers.Add(new Customer(8 + i*10, "CC04", GetTransactions("USD", 300000d)));
		
				// A bunch of darn perpetrators
				customers.Add(new Customer(9 + i*10, "CC01", GetTransactions("EUR", 200000d)));
				customers.Add(new Customer(10 + i*10, "CC02", GetTransactions("USD", 300000d)));
			}
			
			generatedCustomers += customers.Count;
			return customers;
		}
	
		private ArrayList GetTransactions(string currencyCode, double extraTransactionAmount) {
			ArrayList transactions = new ArrayList();
			
			// generate non-suspicious transactions
			int size = 1+rnd.Next(50);
			for(int i=0; i<size; i++)
				transactions.Add(new Transaction(++generatedTransactions,
				                                 DateTime.Now.AddHours(-1 - generatedTransactions),
				                                 new Money(currencyCode, 1+rnd.Next(10000))));
			
			// a suspicious transaction was added
			if (extraTransactionAmount != 0)
				transactions.Add(new Transaction(++generatedTransactions,
				                                 DateTime.Now,
				                                 new Money(currencyCode, extraTransactionAmount)));

			return transactions;
		}	

	}
}
