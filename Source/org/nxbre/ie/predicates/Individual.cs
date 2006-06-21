namespace org.nxbre.ie.predicates {
	using System;
	using System.Collections;
	using System.Text.RegularExpressions;
	using System.Security.Cryptography;
	using System.Threading;

	/// <summary>
	/// An Individual is a predicate that represents a fixed value.
	/// </summary>
	/// <author>David Dossot</author>
	public sealed class Individual:AbstractPredicate {
		[ThreadStatic]
		private static HashAlgorithm ha;
		
		private readonly string sourceType;
		
		public string SourceType {
			get {
				return sourceType;
			}
		}

		public Individual(object predicate):this(predicate, null) {}
	
		public Individual(object predicate, string sourceType):base(predicate) {
			this.sourceType = sourceType;
		}
	
		/// <summary>
		/// Returns a shallow clone of the Individual, the base value is not cloned.
		/// </summary>
		/// <returns>A clone of the current Individual.</returns>
		public override object Clone() {
			return new Individual(Value, SourceType);
		}
		
		/// <summary>
		/// A helper method for easily creating an array of Individual predicates.
		/// </summary>
		/// <param name="predicates">The array of predicate values.</param>
		/// <returns>The array of Individual built on the predicate values.</returns>
		public static Individual[] NewArray(params object[] predicates) {
			Individual[] individuals = new Individual[predicates.Length];
			
			for (int i=0; i<predicates.Length; i++)
				individuals[i] = new Individual(predicates[i]);
				
			return individuals;
		}
		
		/// <summary>
		/// The long hashcode is computed by using MD5 in order to prevent problems with poorly
		/// implemented predicate object hashcodes.
		/// </summary>
		/// <returns>The long hashcode of the predicate.</returns>
		public override long GetLongHashCode() {
			int hashCode = GetType().GetHashCode() ^ Value.GetHashCode();
			
			// bug #1086498, solved by mwherman2000
			byte[] dataArray = new byte[] { (byte)(hashCode & 0xFF),
																			(byte)((hashCode >> 8) & 0xFF),
																			(byte)((hashCode >> 16) & 0xFF),
																			(byte)((hashCode >> 24) & 0xFF) };
			
			byte[] result = HA.ComputeHash(dataArray);
			
			long longHashCode = 0;
			for(int i=0; i<16; i+=8)
				longHashCode ^= ((long)result[i] |
						             ((long)result[i+1] << 8) |
						             ((long)result[i+2] << 16) |
						             ((long)result[i+3] << 24) |
						             ((long)result[i+4] << 32) |
						             ((long)result[i+5] << 40) |
						             ((long)result[i+6] << 48) |
						             ((long)result[i+7] << 56));
			
			return longHashCode;
		}
		
		private static HashAlgorithm HA {
			get {
				if (ha == null) ha = new MD5CryptoServiceProvider();
				return ha;
			}
		}
	}
}
