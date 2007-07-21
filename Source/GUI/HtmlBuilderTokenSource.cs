namespace NxDSL.GUI {
	using System;
	using System.Text;
	
	using NxDSL;
	
	using Antlr.Runtime;
	
	public class HtmlBuilderTokenSource : ITokenSource	{
		
		private readonly ITokenSource source;
		
		private readonly Definitions definitions;
		
		private readonly StringBuilder htmlBuffer;
		
		private bool inQuote = false;
		
		private StringBuilder statementBuffer = new StringBuilder();
		
		private bool htmlFinalized = false;
		
		public HtmlBuilderTokenSource(ITokenSource source, Definitions definitions) {
			this.source = source;
			this.definitions = definitions;
			this.htmlBuffer = new StringBuilder();
		}
		
		public string Html {
			get {
				if (htmlFinalized) {
					throw new NotSupportedException("HTML can only be fetched once!");
				}
					
				htmlFinalized = true;
				
				return htmlBuffer.Insert(0, "<html><body><font face='courier' color='#000000'>").Append("</font></body></html>").ToString();
			}
		}
		
		public void PrependToHtml(string text) {
			htmlBuffer.Insert(0, text);
		}	
		
		public void AppendToHtml(string text) {
			htmlBuffer.Append(text);
		}
		
		public IToken NextToken()
		{
			IToken token = source.NextToken();
			
			if (token.Type == InferenceRules_ENLexer.NEWLINE) {
				string statement = statementBuffer.ToString();
					
				if (definitions.Contains(statement)) {
					htmlBuffer.Append("<font color='#006600'>");
				} else {
					htmlBuffer.Append("<font color='#FF0000'>");
				}
				
				htmlBuffer.Append(statement).Append("</font>");
				
				htmlBuffer.Append("<br/>");
				statementBuffer = new StringBuilder();
			}
			else if (token.Type == InferenceRules_ENLexer.TAB) {
				htmlBuffer.Append("&nbsp;&nbsp;");
			}
			else {
				if ((token.Type == InferenceRules_ENLexer.RULE)
				    || (token.Type == InferenceRules_ENLexer.FACT)
				    || (token.Type == InferenceRules_ENLexer.QUERY)){
					htmlBuffer.Append("<br/>");
				}
				
				if ((token.Type == InferenceRules_ENLexer.QUOTE) && (!inQuote)) {
					htmlBuffer.Append("<font color='#0000FF'>");
					htmlBuffer.Append(token.Text);
					inQuote = true;
				}
				else if ((token.Type == InferenceRules_ENLexer.QUOTE) && (inQuote)) {
					htmlBuffer.Append(token.Text);
					htmlBuffer.Append("</font>");
					inQuote = false;
				}
				else if (inQuote) {
					htmlBuffer.Append(token.Text);
				}
				else if ((token.Type == InferenceRules_ENLexer.CHAR)
				        || (token.Type == InferenceRules_ENLexer.SPACE)
				        || (token.Type == InferenceRules_ENLexer.NUMERIC)) {
					
					statementBuffer.Append(token.Text);
				}
				else if ((token.Type == InferenceRules_ENLexer.COUNT)
				        || (token.Type == InferenceRules_ENLexer.DEDUCT)
				        || (token.Type == InferenceRules_ENLexer.FORGET)
				        || (token.Type == InferenceRules_ENLexer.MODIFY)) {
					
					htmlBuffer.Append("<font color='#990066'><b>").Append(token.Text).Append(" ").Append("</b></font>");
				}
				else {
					htmlBuffer.Append("<b>").Append(token.Text).Append(" ").Append("</b>");
				}
			}
			
			return token;
		}
		
	}
}
