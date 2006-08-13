namespace NxBRE.Util
{
	using System;
	
	/// <summary>
	/// A helper object for building hashcode based certain objects that belongs to a particular class.
	/// </summary>
	public class HashCodeBuilder
	{
		private int hashCode;
		
		public HashCodeBuilder() {
			hashCode = 17;
		}
		
		public HashCodeBuilder(int hashCode) {
			this.hashCode = hashCode;
		}
		
		public HashCodeBuilder Append(object o) {
			if (o != null) {
				long temp = Math.BigMul(37, hashCode);
				hashCode = (Convert.ToInt32(temp & Int32.MaxValue) ^ (int)(temp >> 32)) ^ o.GetHashCode();
			}
			
			return this;
		}
		
		public int Value {
			get {
				return hashCode;
			}
		}
	}
}
