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
    using System.IO;
    using System.Text;
	
	internal class Buffer {
		private const char EOF = (char)256;
		private byte[] buf;
		private int bufLen;
		private int pos;
		
		public void Fill (Stream s) {
			bufLen = (int) s.Length;
			buf = new byte[bufLen];
			s.Read(buf, 0, bufLen); 
			pos = 0;
		}
		
		public int Read ()
		{
		    if (pos < bufLen) return buf[pos++];
		    return EOF;
		}

	    public int Peek ()
		{
		    if (pos < bufLen) return buf[pos];
		    return EOF;
		}

	    /* AW 2003-03-10 moved this from ParserGen.cs */
		public string GetString (int beg, int end) {
			var s = new StringBuilder(64);
			var oldPos = Pos;
			Pos = beg;
			while (beg < end) { s.Append((char)Read()); beg++; }
			Pos = oldPos;
			return s.ToString();
		}
	
		public int Pos {
			get { return pos; }
			set {
				if (value < 0) pos = 0; 
				else if (value >= bufLen) pos = bufLen; 
				else pos = value;
			}
		}
	}
}
