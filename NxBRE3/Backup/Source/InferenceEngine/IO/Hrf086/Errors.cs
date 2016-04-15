/*
 * Created by Compiler Generator Coco/R
 *            Copyright ? 1990, 2004 Hanspeter M?ssenb?ck
 *            University of Linz
 * 
 * Grammar by: Andre Weber
 * Modified for RuleML086 by Ron Evans
 * 
 */
namespace NxBRE.InferenceEngine.IO.Hrf086
{
	using System.Collections;
	using System.Text;
	using System.Reflection;
	
	using System;
	
	internal class Errors {
	  public const string ERRMSGFORMAT = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text
		
		private ArrayList list;
		
		public Errors() {
			list = new ArrayList();
		}
		
		public ArrayList List {
			get {
				return list;
			}
		}
		
		public void SynErr (int line, int col, int n) {
			string s;
			switch (n) {
				case 0: s = "EOF expected"; break;
				case 1: s = "notOperator expected"; break;
				case 2: s = "andOperator expected"; break;
				case 3: s = "orOperator expected"; break;
				case 4: s = "imply expected"; break;
				case 5: s = "argsep expected"; break;
				case 6: s = "openBracket expected"; break;
				case 7: s = "closeBracket expected"; break;
				case 8: s = "tok expected"; break;
				case 9: s = "openParen expected"; break;
				case 10: s = "closeParen expected"; break;
				case 11: s = "\";\" expected"; break;
				case 12: s = "\"[\" expected"; break;
				case 13: s = "\"]\" expected"; break;
				case 14: s = "\"+\" expected"; break;
				case 15: s = "\"?\" expected"; break;
				case 16: s = "??? expected"; break;
				case 17: s = "invalid andRelation"; break;
				case 18: s = "invalid orRelation"; break;
				case 19: s = "invalid arg"; break;

				default: s = "error " + n; break;
			}
			
			list.Add(String.Format(ERRMSGFORMAT, line, col, s));
		}
	
		public void SemErr (int line, int col, int n) {
			list.Add(String.Format(ERRMSGFORMAT, line, col, ("error " + n)));
		}
	
		public void Error (int line, int col, string s) {
			list.Add(String.Format(ERRMSGFORMAT, line, col, s));
		}
	
		public void Exception (string s) {
			list.Add(s);
		}
	} // Errors
	
}
