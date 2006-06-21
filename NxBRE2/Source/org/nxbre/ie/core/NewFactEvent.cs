namespace org.nxbre.ie.core {
	using System;

	using org.nxbre.ie.rule;
	
	/// <summary>
	/// Delegate for listening new events for Facts. This class is immutable.
	/// </summary>
	public delegate void NewFactEvent(NewFactEventArgs e);
	
	/// <summary>
	/// New Fact event definition.
	/// </summary>
	/// <remarks>
	/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
	/// </remarks>
	public sealed class NewFactEventArgs: EventArgs {
	  private readonly Fact fact;
	  private readonly Fact otherFact;
	
		public Fact Fact {
			get {
				return fact;
			}
		}
		
		public Fact OtherFact {
			get {
				return otherFact;
			}
		}
		
		/// <summary>
		/// Instantiates a new Fact event definition.
		/// </summary>
		/// <remarks>
		/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
		/// </remarks>
		/// <param name="fact">The Fact that generated the event.</param>
		public NewFactEventArgs(Fact fact):this(fact, null) {}
		
		/// <summary>
		/// Instantiates a new Fact event definition.
		/// </summary>
		/// <remarks>
		/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
		/// </remarks>
		/// <param name="fact">The Fact that generated the event.</param>
		/// <param name="fact">The Other Fact that generated the event.</param>
		public NewFactEventArgs(Fact fact, Fact otherFact)
		{
			this.fact = fact;
			this.otherFact = otherFact;
		}
	}
}
