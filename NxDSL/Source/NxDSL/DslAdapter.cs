namespace NxDSL {
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using System.IO;
	
	using Antlr.Runtime;

	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>An adapter supporting NxBRE DSL Format.</summary>
	/// <remarks>Only READ is supported!</remarks>
	/// <author>David Dossot</author>
	public class DslAdapter : IRuleBaseAdapter	{
		public enum GrammarLanguages {EN, FR};
		
		private readonly IRuleBaseAdapter adapter;
		
		public DslAdapter(string dslFile):this(dslFile, dslFile + ".defs", GrammarLanguages.EN) {
		}
			
		public DslAdapter(string dslFile, GrammarLanguages grammarLanguage):this(dslFile, dslFile + ".defs", grammarLanguage) {
		}
			
		public DslAdapter(string dslFile, string definitionFile):this(dslFile, definitionFile, GrammarLanguages.EN) {
		}
			
		public DslAdapter(string dslFile, string definitionFile, GrammarLanguages grammarLanguage) {
			RuleBaseBuilder rbb = new RuleBaseBuilder(new Definitions(definitionFile));
			
			string ruleml = null;
			
			if (grammarLanguage == GrammarLanguages.EN) {
				InferenceRules_ENParser ipr = new InferenceRules_ENParser(
												new CommonTokenStream(
													new InferenceRules_ENLexer(
														new ANTLRFileStream(dslFile))));
				ipr.rbb = rbb;
				ipr.rulebase();
				ruleml = ipr.rbb.RuleML;
			} else if (grammarLanguage == GrammarLanguages.FR) {
				InferenceRules_FRParser ipr = new InferenceRules_FRParser(
												new CommonTokenStream(
													new InferenceRules_FRLexer(
														new ANTLRFileStream(dslFile))));
				ipr.rbb = rbb;
				ipr.rulebase();
				ruleml = ipr.rbb.RuleML;
			}
			
			adapter = new RuleML09NafDatalogAdapter(new MemoryStream(new UTF8Encoding().GetBytes(ruleml)), FileAccess.Read);
		}
		
		public IBinder Binder {
			set {
				adapter.Binder = value;
			}
		}
		
		public void Dispose() {
			adapter.Dispose();
		}
		
		public IList<Query> Queries {
			get {
				return adapter.Queries;
			}
			set {
				throw new NotSupportedException("This adapter does not support changes");
			}
		}
		
		public IList<Implication> Implications {
			get {
				return adapter.Implications;
			}
			set {
				throw new NotSupportedException("This adapter does not support changes");
			}
		}
		
		public string Direction {
			get {
				return adapter.Direction;
			}
			set {
				throw new NotSupportedException("This adapter does not support changes");
			}
		}
		
		public string Label {
			get {
				return adapter.Label;
			}
			set {
				throw new NotSupportedException("This adapter does not support changes");
			}
		}
		
		public IList<Fact> Facts {
			get {
				return adapter.Facts;
			}
			set {
				throw new NotSupportedException("This adapter does not support changes");
			}
		}
		
	}
}
