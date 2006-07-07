namespace NxBRE.Examples {
	using System;
	
	public class CallData {
		private static int pkGen = 0;
		private int pk;
		private DateTime time;
		// obviously missing duration, but not part of the sample
		private string type;
		private string clientScheme;
		private string destination;
		private string expectedRatingScheme;
		private string ratingScheme;
		
		public string RatingScheme {
			get {
				return ratingScheme;
			}
			set {
				ratingScheme = value;
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
		
		public DateTime Time {
			get {
				return time;
			}
			set {
				time = value;
			}
		}

		public string Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}
		public string ClientScheme {
			get {
				return clientScheme;
			}
			set {
				clientScheme = value;
			}
		}
		public string Destination {
			get {
				return destination;
			}
			set {
				destination = value;
			}
		}
		
		public CallData(DateTime time, string type, string clientScheme, string destination, string expectedRatingScheme)
		{
			pk = ++pkGen;
			this.time = time;
			this.type = type;
			this.clientScheme = clientScheme;
			this.destination = destination;
			this.expectedRatingScheme = expectedRatingScheme;
		}
		
		public override string ToString() {
			return Type + "->" + Destination + "@" + Time.Hour + "^" + ClientScheme;
		}

		public override int GetHashCode() {
			return pk;
		}
		
		public void CheckValidity() {
			if (RatingScheme != expectedRatingScheme)
				throw new Exception("RatingScheme: expected=" + 
				                    expectedRatingScheme +
				                    " - assigned=" +
				                    RatingScheme);
		}
		
	}
	
}
