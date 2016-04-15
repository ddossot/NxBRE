/*
 * Created by Compiler Generator Coco/R
 *            Copyright © 1990, 2004 Hanspeter Mössenböck
 *            University of Linz
 * 
 * Grammar by: Andre Weber
 * Modified for RuleML086 by Ron Evans
 * 
 */
namespace NxBRE.InferenceEngine.IO.Hrf086
{
	
	using System;
	using System.IO;
	using System.Collections;
	using System.Text;
	
	internal class Token {
		public int kind;    // token kind
		public int pos;     // token position in the source text (starting at 0)
		public int col;     // token column (starting at 0)
		public int line;    // token line (starting at 1)
		public string val;  // token value
		public Token next;  // AW 2003-03-07 Tokens are kept in linked list
	}
	
}
