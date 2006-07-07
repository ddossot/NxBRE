namespace NxBRE.Examples
{
	using System;
	
	/// <summary> This is just a shell of an Order object to get CalculateTotal to compile
	/// </summary>
	public class Order
	{
		private Int32 quantity;
		private Double totalCost;
		private string clientRating;
		
		public Order(Int32 quantity, Double totalCost, string clientRating)
		{
			this.quantity = quantity;
			this.totalCost = totalCost;
			this.clientRating = clientRating;
		}

		public Int32 Quantity {
			get {
				return quantity;
			}
		}
		public Double TotalCost {
			get {
				return totalCost;
			}
		}
		public string ClientRating {
			get {
				return clientRating;
			}
		}
		
	}
}
