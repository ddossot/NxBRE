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
	
	using System;
	using System.IO;
	using System.Collections;
	using System.Text;
		
	internal class Scanner {
		private const int eofSym = 0; /* pdt */
		private const char EOF = '\0';
		private const char EOL = '\n';
		private const int charSetSize = 256;
		private const int maxT = 16;
		private const int noSym = 16;

		static short[] start = {
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  1,  0, 12,  0,  0,  2,  0, 10, 11,  0, 17,  6,  4,  0,  0,
								   9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  0, 14,  0,  0,  0, 18,
								   0,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,
								   9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9, 15,  0, 16,  0,  9,
								   0,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,
								   9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  7,  3,  8,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
								   -1};
			
	
		private Token t;          // current token
		private char ch;          // current input character
		private int pos;          // column number of current character
		private int line;         // line number of current character
		private int lineStart;    // start position of current line
		private int oldEols;      // EOLs that appeared in a comment;
		private BitArray ignore;  // set of characters to be ignored by the scanner
	
		private Token tokens;     // the complete input token stream
		private Token pt;         // current peek token
		
		private Buffer buffer;
		
		public void Init(string fileName) {
			FileStream s = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			Init(s);
			s.Close();
		}
		
		public void Init(Stream s) {
			buffer = new Buffer();
			buffer.Fill(s);
			pos = -1; line = 1; lineStart = 0;
			oldEols = 0;
			NextCh();
			ignore = new BitArray(charSetSize + 1);
			ignore[9] = true; ignore[10] = true; ignore[13] = true; ignore[32] = true; 
			
			//--- AW: fill token list
			tokens = new Token();  // first token is a dummy
			Token node = tokens;
			do {
				node.next = NextToken();
				node = node.next;
			} while (node.kind != eofSym);	/* AW: 0 == EOF */
			node.next = node;
			node.val = "EOF";
			t = pt = tokens;
		}
		
		private void NextCh() {
			if (oldEols > 0) { ch = EOL; oldEols--; } 
			else {
				ch = (char)buffer.Read(); pos++;
				// replace isolated '\r' by '\n' in order to make
				// eol handling uniform across Windows, Unix and Mac
				if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
				if (ch == EOL) { line++; lineStart = pos + 1; }
			}
		}
	
		private bool Comment0() {
			int level = 1, line0 = line, lineStart0 = lineStart;
			NextCh();
			if (ch == '*') {
				NextCh();
				for(;;) {
					if (ch == '*') {
						NextCh();
						if (ch == '/') {
							level--;
							if (level == 0) { oldEols = line - line0; NextCh(); return true; }
							NextCh();
						}
					} else if (ch == EOF) return false;
					else NextCh();
				}
			} else {
				if (ch==EOL) {line--; lineStart = lineStart0;}
				pos = pos - 2; buffer.Pos = pos+1; NextCh();
			}
			return false;
		}
	
		
		private void CheckLiteral() {
			switch (t.val) {
				default: break;
			}
		}
	
		/* AW Scan() renamed to NextToken() */
		private Token NextToken() {
			while (ignore[ch]) NextCh();
			if (ch == '/' && Comment0()) return NextToken();
			t = new Token();
			t.pos = pos; t.col = pos - lineStart + 1; t.line = line; 
			int state = start[ch];
			StringBuilder buf = new StringBuilder(16);
			buf.Append(ch); NextCh();
		
			switch (state) 
			{
				case -1: { t.kind = eofSym; goto done; } // NextCh already done /* pdt */
				case 0: { t.kind = noSym; goto done; }   // NextCh already done
				case 1:
				{t.kind = 1; goto done;}
				case 2:
				{t.kind = 2; goto done;}
				case 3:
				{t.kind = 3; goto done;}
				case 4:
					if (ch == '>') {buf.Append(ch); NextCh(); goto case 5;}
					else {t.kind = noSym; goto done;}
				case 5:
				{t.kind = 4; goto done;}
				case 6:
				{t.kind = 5; goto done;}
				case 7:
				{t.kind = 6; goto done;}
				case 8:
				{t.kind = 7; goto done;}
				case 9:
					if ((ch >= '*' && ch <= '+' || ch >= '-' && ch <= ':' || ch >= 'A' && ch <= 'Z' || ch == 92 || ch == '_' || ch >= 'a' && ch <= 'z')) {buf.Append(ch); NextCh(); goto case 9;}
					else {t.kind = 8; goto done;}
				case 10:
				{t.kind = 9; goto done;}
				case 11:
				{t.kind = 10; goto done;}
				case 12:
					if ((ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z')) {buf.Append(ch); NextCh(); goto case 13;}
					else {t.kind = noSym; goto done;}
				case 13:
					if ((ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z')) {buf.Append(ch); NextCh(); goto case 13;}
					else {t.kind = 17; goto done;}
				case 14:
				{t.kind = 11; goto done;}
				case 15:
				{t.kind = 12; goto done;}
				case 16:
				{t.kind = 13; goto done;}
				case 17:
				{t.kind = 14; goto done;}
				case 18:
				{t.kind = 15; goto done;}
			}
			done: 
				t.val = buf.ToString();
			return t;
		}
		
		/* AW 2003-03-07 get the next token, move on and synch peek token with current */
		public Token Scan () {
			t = pt = t.next;
			return t;
		}
	
		/* AW 2003-03-07 get the next token, ignore pragmas */
		public Token Peek () {
			do {                      // skip pragmas while peeking
				pt = pt.next;
			} while (pt.kind > maxT);
			return pt;
		}
		
		/* AW 2003-03-11 to make sure peek start at current scan position */
		public void ResetPeek () { pt = t; }
	} // end Scanner
	
}
