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
	/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
	/// </remarks>
	public sealed class NewFactEventArgs: EventArgs {
		[ThreadStatic]
		private static IIEventContext context;
		
		private readonly Fact fact;
		private readonly Fact otherFact;
		
		public static IIEventContext Context {
			get { return context; }
		}
		
		internal static void SetContext(IIEventContext newContext) {
			context = newContext;
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
		public NewFactEventArgs(Fact fact):this(fact, null) {}
		
		/// <summary>
		/// Instantiates a new Fact event definition.
		/// </summary>
		/// <remarks>
		/// DO NOT ASSERT OR RETRACT FACTS WHEN HANDLING THIS EVENT!
		/// </remarks>
		/// <param name="fact">The Fact that generated the event.</param>
		/// <param name="otherFact">The Other Fact that generated the event.</param>
		public NewFactEventArgs(Fact fact, Fact otherFact)
		{
			this.fact = fact;
			this.otherFact = otherFact;
		}
	}
}
