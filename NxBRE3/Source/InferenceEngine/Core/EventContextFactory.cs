namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// A factory for building event contexts.
	/// </summary>
	internal abstract class EventContextFactory {
		private sealed class ImmutableEventContext:IEventContext {
			private readonly IList<IList<Fact>> facts;
			
			private readonly Implication implication;
			
			public ImmutableEventContext(IList<IList<Fact>> facts, Implication implication) {
				this.facts = facts;
				this.implication = implication;
			}
			
			public IList<IList<Fact>> Facts {
				get {
					return facts;
				}
			}
			
			public Implication Implication {
				get {
					return implication;
				}
			}
		}
		
		private EventContextFactory() {
			// NOOP
		}
		
		//FIXME accept IList<IList<FactBase.PositiveMatchResult>>
		internal static IEventContext NewEventContext(IList<IList<Fact>> facts, Implication implication) {
			return new ImmutableEventContext(facts, implication);
		}
		
		//FIXME accept IList<FactBase.PositiveMatchResult>
		internal static IEventContext NewEventContext(IList<Fact> facts, Implication implication) {
			IList<IList<Fact>> wrappedFacts = new List<IList<Fact>>();
			wrappedFacts.Add(facts);
			return new ImmutableEventContext(wrappedFacts, implication);
		}
	}
}
