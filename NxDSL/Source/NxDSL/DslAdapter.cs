namespace NxDSL {
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.IO;
	
	using Antlr.Runtime;

	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	/// <summary>An adapter supporting NxBRE DSL Format.</summary>
	/// <remarks>Only READ is supported!</remarks>
	/// <author>David Dossot</author>
	public class DslAdapter : IRuleBaseAdapter	{
		private readonly IRuleBaseAdapter adapter;
			
		public DslAdapter(string dslFile) {
			string definitionFile = dslFile + ".defs";
			
			Definitions definitions = new Definitions(definitionFile);
			
			InferenceRules_ENParser ipr = new InferenceRules_ENParser(
											new CommonTokenStream(
												new InferenceRules_ENLexer(
													new ANTLRFileStream(dslFile))));
			
			ipr.rbb = new RuleBaseBuilder(new Definitions(definitionFile));
			
			ipr.rulebase();
			
			adapter = new RuleML09NafDatalogAdapter(new MemoryStream(new UTF8Encoding().GetBytes(ipr.rbb.RuleML)), FileAccess.Read);
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
