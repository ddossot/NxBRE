namespace NxBRE.InferenceEngine.IO {

	using System;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>
	/// Provides attributes for defining saving format of rule base
	/// </summary>
	public enum SaveFormatAttributes {
		/// <summary>
		/// Compact RuleML syntax.
		/// </summary>
		Compact = 1,
		/// <summary>
		/// Standard RuleML syntax.
		/// </summary>
		Standard = 2,
		/// <summary>
		/// Expanded RuleML syntax.
		/// </summary>
		Expanded = 4,
		/// <summary>
		/// Forces non-string individuals to be saved as XML Schema typed data, even if they were not defined as such in the
		/// original rulebase.
		/// </summary>
		ForceDataTyping = 8
	};

	
	/// <summary>
	/// NxBRE Inference Engine extended rulebase adapter interface.
	/// The engine calls the properties in this order: Binder, Direction, Label, Equivalents, Queries, Implications, Facts.
	/// </summary>
	/// <description>
	/// Reading is supported by the getter of each member, while writing is supported by setters.
	/// The engine calls dispose at the end of the load or save operation.</description>
	/// <remarks>
	/// This class is currently internal, it will be made public when stabilized, ie when RuleML 1.0 will be released.
	/// </remarks>
	/// <see cref="NxBRE.InferenceEngine.IEImpl"/>
	/// <author>David Dossot</author>
	internal interface IExtendedRuleBaseAdapter:IRuleBaseAdapter {

		/// <summary>
		/// Collection containing all the facts to assert.
		/// </summary>
		/// <remarks>
		/// This will make IFactBaseAdapter.Facts obsolete.
		/// </remarks>
		IList<Fact> Assertions {
			get;
			set;
		}

		/// <summary>
		/// Collection containing all the facts to retract.
		/// </summary>
		IList<Fact> Retractions {
			get;
			set;
		}
		
		/// <summary>
		/// Collection containing all the equivalent atom pairs in the rulebase.
		/// </summary>
		IList<Equivalent> Equivalents {
			get;
			set;
		}
		
		/// <summary>
		/// Collection containing all the integrity queries.
		/// </summary>
		IList<Query> IntegrityQueries {
			get;
			set;
		}
		
	}
	
}
