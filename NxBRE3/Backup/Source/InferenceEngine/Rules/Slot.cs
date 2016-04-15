namespace NxBRE.InferenceEngine.Rules {
	using System;
	
	/// <summary>
	/// A Slot is a holder for a named predicate. It is immutable.
	/// </summary>
	/// <author>David Dossot</author>
	public sealed class Slot:IPredicate {
		private readonly string name;
		private readonly IPredicate predicate;
		
		/// <summary>
		/// The predicate contained in the slot.
		/// </summary>
		/// <remarks>
		/// Prefer the Predicate property to avoid casting.
		/// </remarks>
		public object Value {
			get {
				return predicate;
			}
		}
		
		/// <summary>
		/// The name of the slot.
		/// </summary>
		public string Name {
			get {
				return name;
			}
		}
		
		/// <summary>
		/// The predicate contained in the slot.
		/// </summary>
		public IPredicate Predicate {
			get {
				return predicate;
			}
		}
		
		/// <summary>
		/// Instantiates a new slot that will hold a named predicate.
		/// </summary>
		/// <param name="name">The name of the predicate</param>
		/// <param name="predicate">The predicate itself</param>
		public Slot(string name, IPredicate predicate) {
			if ((name == null) || (name == String.Empty)) throw new ArgumentException("The name of a slot can not be null or empty");
			if (predicate == null) throw new ArgumentException("The predicate of a slot can not be null");
			if (predicate is Slot) throw new ArgumentException("A slot can not contain another slot");
			
			this.name = name;
			this.predicate = predicate;
		}
		
		public object Clone() {
			return new Slot(name, predicate);
		}
		
		public override string ToString() {
			return Name + "=" + Value.ToString();
		}
	}
	
}

