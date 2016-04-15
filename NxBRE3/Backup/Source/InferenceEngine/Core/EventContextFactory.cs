namespace NxBRE.InferenceEngine.Core {
	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Rules;
	using NxBRE.Util;
	
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
			
			public override string ToString()
			{
				return string.Format("[EventContext: Implication={0} - Facts={1}]", Implication.Label, Misc.IListToString(Facts));
			}
			
		}
		
		private EventContextFactory() {
			// NOOP
		}
		
		internal static IEventContext NewEventContext(IList<IList<FactBase.PositiveMatchResult>> facts, Implication implication) {
			return new ImmutableEventContext(FactBase.ExtractFacts(facts), implication);
		}
		
		internal static IEventContext NewEventContext(IList<FactBase.PositiveMatchResult> facts, Implication implication) {
			IList<IList<Fact>> wrappedFacts = new List<IList<Fact>>();
			wrappedFacts.Add(FactBase.ExtractFacts(facts));
			return new ImmutableEventContext(wrappedFacts, implication);
		}
	}
}
