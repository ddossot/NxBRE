namespace NxBRE.InferenceEngine {
	using System;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// Delegate for listening new events for Facts. This class is immutable.
	/// </summary>
	public delegate void NewFactEvent(NewFactEventArgs e);
	
	/// <summary>
	/// New Fact event definition.
	/// </summary>
	/// <remarks>
	/// DO NOT ASSERT OR RETRACT FACTS OR ALTER THE EVENT CONTEXT WHEN HANDLING THIS EVENT!
	/// </remarks>
	public sealed class NewFactEventArgs: EventArgs {
		private readonly IEventContext context;
		private readonly Fact fact;
		private readonly Fact otherFact;
		
		/// <summary>
		/// The context in which the event happened.
		/// </summary>
		public IEventContext Context {
			get { return context; }
		}
		
		/// <summary>
		/// The fact that has generated the event.
		/// </summary>
		public Fact Fact {
			get {
				return fact;
			}
		}
		
		/// <summary>
		/// The optional other fact involved in the event, or null.
		/// </summary>
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
		public NewFactEventArgs(Fact fact):this(fact, null, null) {}
		
		/// <summary>
		/// Instantiates a new Fact event definition.
		/// </summary>
		/// <remarks>
		/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
		/// </remarks>
		/// <param name="fact">The Fact that generated the event.</param>
		/// <param name="context">The context of the event.</param>
		public NewFactEventArgs(Fact fact, IEventContext context):this(fact, null, context) {}
		
		/// <summary>
		/// Instantiates a new Fact event definition.
		/// </summary>
		/// <remarks>
		/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
		/// </remarks>
		/// <param name="fact">The Fact that generated the event.</param>
		/// <param name="otherFact">The Other Fact that generated the event.</param>
		public NewFactEventArgs(Fact fact, Fact otherFact):this(fact, otherFact, null) {}
		
		/// <summary>
		/// Instantiates a new Fact event definition.
		/// </summary>
		/// <remarks>
		/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
		/// </remarks>
		/// <param name="fact">The Fact that generated the event.</param>
		/// <param name="otherFact">The Other Fact that generated the event.</param>
		/// <param name="context">The context of the event.</param>
		public NewFactEventArgs(Fact fact, Fact otherFact, IEventContext context) {
			this.fact = fact;
			this.otherFact = otherFact;
			this.context = context;
		}
	}
}
