namespace NxBRE.Util
{
	using System;
	
	/// <summary>
	/// A helper object for building hashcode based certain objects that belongs to a particular class.
	/// </summary>
	/// <remarks>
	/// This helper follows Joshua Bloch's Effective Java recommandations on computing hashcode.
	/// </remarks>
	public class HashCodeBuilder
	{
		private int hashCode;
		
		/// <summary>
		/// Creates a new HashCodeBuilder initialized for computing a new hashcode.
		/// </summary>
		public HashCodeBuilder() {
			hashCode = 17;
		}
		
		/// <summary>
		/// Creates a new HashCodeBuilder initialized for extending an existing hashcode.
		/// </summary>
		/// <param name="hashCode">An actual existing hashcode.</param>
		/// <remarks>Do not call this method with a random number, if you do not have an existing hashcode use the empty constructor.</remarks>
		public HashCodeBuilder(int hashCode) {
			this.hashCode = hashCode;
		}
		
		/// <summary>
		/// Append a new object (usually an object's field) to the current hashcode.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public HashCodeBuilder Append(object o) {
			if (o != null) hashCode = unchecked(37*hashCode + o.GetHashCode());
			return this;
		}
		
		/// <summary>
		/// The current hashcode value computed by this instance of HashCodeBuilder.
		/// </summary>
		public int Value {
			get {
				return hashCode;
			}
		}
	}
}
