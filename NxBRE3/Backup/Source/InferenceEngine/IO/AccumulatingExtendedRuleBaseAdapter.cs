namespace NxBRE.InferenceEngine.IO {
	using System;
	using System.Collections.Generic;
	using System.IO;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// An abstract rule base adapter that accumulate all collections and restitutes them in one method
	/// at dispose time.
	/// </summary>
	/// <remarks>
	/// Currently extends RuleML09NafDatalogAdapter but will probably be higher in the class hierarchy later.
	/// </remarks>
	public abstract class AccumulatingExtendedRuleBaseAdapter:RuleML09NafDatalogAdapter {
		private	IList<Fact> accumulatedFactsAssertions;
		private	IList<Fact> accumulatedFactsRetractions;
		private	IList<Query> accumulatedQueries;
		private	IList<Implication> accumulatedImplications;
		private	IList<Equivalent> accumulatedEquivalents;
		private	IList<Query> accumulatedIntegrityQueries;
		
		/// <summary>
		/// Only delegates to base constructor.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public AccumulatingExtendedRuleBaseAdapter(Stream streamRuleML, FileAccess mode):base(streamRuleML, mode) {
			// NOOP
		}

		/// <summary>
		/// Only delegates to base constructor.
		/// </summary>
		/// <param name="streamRuleML">The stream to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="attributes">The RuleML format attributes, only used if writing.</param>
		public AccumulatingExtendedRuleBaseAdapter(Stream streamRuleML, FileAccess mode, SaveFormatAttributes attributes):base(streamRuleML, mode) {
			// NOOP
		}

		/// <summary>
		/// Only delegates to base constructor.
		/// </summary>
		/// <param name="uriRuleML">The URI to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		public AccumulatingExtendedRuleBaseAdapter(string uriRuleML, FileAccess mode):base(uriRuleML, mode) {
			// NOOP
		}
		
		/// <summary>
		/// Only delegates to base constructor.
		/// </summary>
		/// <param name="uriRuleML">The URI to read from or write to.</param>
		/// <param name="mode">The FileAccess mode.</param>
		/// <param name="attributes">The RuleML format attributes, only used if writing.</param>
		public AccumulatingExtendedRuleBaseAdapter(string uriRuleML, FileAccess mode, SaveFormatAttributes attributes):base(uriRuleML, mode) {
			// NOOP
		}
		
		public override IList<Query> Queries {
			get {
				return base.Queries;
			}
			set {
				accumulatedQueries = value;
			}
		}
		
		public override IList<Implication> Implications {
			get {
				return base.Implications;
		 	}
			set {
				accumulatedImplications = value;
			}
		}
		
		public override IList<Fact> Assertions {
			get {
				return base.Assertions;
		 	}
			set {
				accumulatedFactsAssertions = value;
			}
		}
		
		public override IList<Fact> Retractions {
			get {
				//TODO FR-1546485: implement get Retractions
				throw new NotSupportedException("Must be implemented");
			}
			
			set {
				accumulatedFactsRetractions = value;
			}
		}
		
		public override IList<Query> IntegrityQueries {
			get {
				return base.IntegrityQueries;
		 	}
			
			set {
				accumulatedIntegrityQueries = value;
			}
		}
		
		public override IList<Equivalent> Equivalents {
			get {
				return base.Equivalents;
		 	}
			
			set {
				accumulatedEquivalents = value;
			}
		}
		
		public override void Dispose()
		{
			if (AdapterState == State.Write) {
				BuildDomRulebase(accumulatedFactsAssertions, accumulatedQueries, accumulatedImplications, accumulatedEquivalents, accumulatedIntegrityQueries);
				
				base.Dispose();
				
				accumulatedFactsAssertions = null;
				accumulatedQueries = null;
				accumulatedImplications = null;
				accumulatedEquivalents = null;
				accumulatedIntegrityQueries = null;
			}
			else {
				base.Dispose();
			}
		}
		
		protected abstract void BuildDomRulebase(IList<Fact> facts,
		                                         IList<Query> queries,
		                                         IList<Implication> implications,
		                                         IList<Equivalent> equivalents,
		                                         IList<Query> integrityQueries);
		
	}
}
