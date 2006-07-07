namespace NxBRE.Examples
{
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	public class CustomBinder:AbstractBinder {
		public CustomBinder():base(BindingTypes.BeforeAfter) {}
		
		public override void BeforeProcess() {
			foreach(Customer customer in (ArrayList)BusinessObjects["CUSTOMERS"]) {
				IEF.AssertNewFactOrFail("Location", new object[]{customer, customer.CountryCode});
				
				foreach(Transaction transaction in customer.Transactions) {
					IEF.AssertNewFactOrFail("Involved In", new object[]{customer, transaction});
					IEF.AssertNewFactOrFail("Amount", new object[]{transaction, transaction.Amount});
				}
			}
		}
		
		public override void AfterProcess() {}
		
		public override NewFactEvent OnNewFact {
			get {
				return new NewFactEvent(NewFactHandler);
			}
		}
		
		public override bool Evaluate(object predicate, string function, string[] arguments) {
			switch(function) {
				case "min_Arg0_Arg1":
					return ((MoneyConverter)BusinessObjects["MNYCNV"]).Convert((Money)predicate, arguments[1])
									>= Double.Parse(arguments[0]);
			}
			
			throw new Exception("Binder can not evaluate " + function);
		}
		
		private void NewFactHandler(NewFactEventArgs nfea) {
			if (nfea.Fact.Type == "Fraudulent Customer")
				((Customer)nfea.Fact.GetPredicateValue(0)).Fraudulent = true;
		}
		
	}
}
