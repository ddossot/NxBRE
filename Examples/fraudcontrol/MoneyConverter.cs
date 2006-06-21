namespace org.nxbre.examples
{
	using System;
	
	public class MoneyConverter {
		public double Convert(Money moneyIn, string currencyCodeOut) {
			if (moneyIn.Code == currencyCodeOut) return moneyIn.Amount;
			
			if ((moneyIn.Code == "USD") && (currencyCodeOut == "EUR"))
				return moneyIn.Amount * .85d;
			
			if ((moneyIn.Code == "EUR") && (currencyCodeOut == "USD"))
				return moneyIn.Amount / .85d;
	
			throw new Exception("CurrencyConverter can not convert from " +
			                    moneyIn.Code +
			                    " to " +
			                    currencyCodeOut);
		}
	}
}
