namespace NxDSL {
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Security;
	
	internal class RuleBaseBuilder {
		private string label;
		
		private readonly Definitions definitions;
		
		private readonly IList<Statement> statements = new List<Statement>();
		
		private int statementIndex;
		
		private readonly IDictionary<int, string> logicBlocks = new Dictionary<int, string>();
		
		private readonly StringBuilder facts = new StringBuilder();
		
		private readonly StringBuilder queries = new StringBuilder();
		
		private readonly StringBuilder implications = new StringBuilder();
		
		private ImplicationData currentImplicationData;
		
		private struct Statement {
			public int depth;
			public string text;
			
			public Statement(int depth, string text) {
				this.depth = depth;
				this.text = text;
			}
		}
		
		public class ImplicationData {
			public string action;
			public string priority;
			public string mutex;
			public string precondition;
		}
		
		public RuleBaseBuilder(Definitions definitions) {
			this.definitions = definitions;
			PurgeAccumulators();
		}
		
		public string Label {
			set { label = value; }
		}
		
		public ImplicationData CurrentImplicationData {
			get { return currentImplicationData; }
		}
		
		public void AddStatement(int depth, string text) {
			statements.Add(new Statement(depth, text));
		}
		
		public void AddLogicBlock(int depth, string newOperator) {
			string existingOperator;
			
			if (logicBlocks.TryGetValue(depth, out existingOperator)) {
				if (!newOperator.Equals(existingOperator)) {
					throw new DslException("Operator mismatch: found an '" + newOperator
								 + "' at the same level of an '" + existingOperator + "'!");
					}
			}
			else {
				logicBlocks.Add(depth, newOperator);
			}
		}
		
		public void AddFact(string label) {
			if (statements.Count != 1) {
				throw new DslException("A fact must contain one level of statements.");
			}
			
			string atom = definitions.GetResolvedContent(statements[0].text);
			
			if (label != null) {
				atom = atom.Replace("<Atom>", "<Atom><oid><Ind>" + SecurityElement.Escape(label) + "</Ind></oid>");
			}
			
			facts.AppendLine(atom);
			
			PurgeAccumulators();
		}
		
		public void AddQuery(string label) {
			queries.AppendLine("<Query>");
			queries.Append("<oid><Ind>").Append(SecurityElement.Escape(label)).AppendLine("</Ind></oid>");
			queries.Append(BuildLogicBlock());
			queries.AppendLine("</Query>");
			
			PurgeAccumulators();
		}
		
		public void AddImplies(string label) {
			string deduction = statements[statements.Count-1].text;
			statements.RemoveAt(statements.Count-1);
			
			implications.AppendLine("<Implies>");
			implications.Append("<oid><Ind>label:").Append(SecurityElement.Escape(label));
			implications.Append(";action:").Append(currentImplicationData.action);
			
			if (currentImplicationData.priority != null) {
				implications.Append(";priority:").Append(currentImplicationData.priority);
			}
			
			if (currentImplicationData.mutex != null) {
				implications.Append(";mutex:").Append(currentImplicationData.mutex);
			}
			
			if (currentImplicationData.precondition != null) {
				implications.Append(";precondition:").Append(currentImplicationData.precondition);
			}
			
			implications.AppendLine(";</Ind></oid>");
			implications.Append(BuildLogicBlock());
			implications.AppendLine(definitions.GetResolvedContent(deduction));
			implications.AppendLine("</Implies>");
			
			PurgeAccumulators();
		}
		
		public string RuleML {
			get {
				StringBuilder ruleml = new StringBuilder("<?xml version='1.0' encoding='utf-8'?>").AppendLine();
				ruleml.AppendLine("<RuleML xmlns='http://www.ruleml.org/0.9/xsd' xmlns:xs='http://www.w3.org/2001/XMLSchema'>");
				ruleml.Append("<oid><Ind>").Append(SecurityElement.Escape(label)).AppendLine("</Ind></oid>");
				
				ruleml.Append(queries.ToString());
				
				ruleml.AppendLine("<Assert mapDirection='forward'>");
				ruleml.Append(implications.ToString());
				ruleml.AppendLine("</Assert>");
				
				ruleml.AppendLine("<Assert mapDirection='forward'>");
				ruleml.Append(facts.ToString());
				ruleml.AppendLine("</Assert>");
				
				return ruleml.Append("</RuleML>").ToString();
			}
		}
		
		private string BuildLogicBlock() {
			StringBuilder result = new StringBuilder();
			
			statementIndex = 0;
			
			do {
				result.AppendLine(GetContiguousStatementsAtSameDepth());
				statementIndex++;
			} while (statementIndex < statements.Count);
			
			return result.ToString();
		}
		
		private string GetContiguousStatementsAtSameDepth() {
			StringBuilder result = new StringBuilder();
			
			int referenceDepth = statements[statementIndex].depth;
			int depth = referenceDepth;
			
			do {
				result.AppendLine(definitions.GetResolvedContent(statements[statementIndex].text));
				statementIndex++;
			} while ((statementIndex < statements.Count) && ((depth = statements[statementIndex].depth) == referenceDepth));
			
			
			return SurroundWithLogicalOperator(referenceDepth, result.ToString());
		}
		
		private string SurroundWithLogicalOperator(int depth, string content) {
			if (logicBlocks.ContainsKey(depth)) {
				StringBuilder result = new StringBuilder();
				
				result.Append("<").Append(logicBlocks[depth]).AppendLine(">");
				result.AppendLine(content);
				result.Append("</").Append(logicBlocks[depth]).AppendLine(">");
				
				return result.ToString();
			} else {
				return content;
			}
		}
		
		private void PurgeAccumulators() {
			statements.Clear();
			logicBlocks.Clear();
			currentImplicationData = new ImplicationData();
		}

	}
}
