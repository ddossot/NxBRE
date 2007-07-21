namespace NxDSL {
	using System;
	
	using Antlr.Runtime;

	public class DslException : Exception {
		public DslException(RecognitionException re):base(BuildMessage(re)) {
		}
		
		private static string BuildMessage(RecognitionException re) {
			return "Can not parse rule file: syntax error on line " + re.Line
					+ ", column " + re.CharPositionInLine
					+ " " + re.Token;
		}
	}
}
