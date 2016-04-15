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
	using System.IO;
	using System.Text;
	using System.Reflection;
	
	using System;
	
	internal class Parser {
		const int _EOF = 0;
		const int _notOperator = 1;
		const int _andOperator = 2;
		const int _orOperator = 3;
		const int _imply = 4;
		const int _argsep = 5;
		const int _openBracket = 6;
		const int _closeBracket = 7;
		const int _tok = 8;
		const int _openParen = 9;
		const int _closeParen = 10;
		const int maxT = 16;

		const bool T = true;
		const bool x = false;
		const int minErrDist = 2;
	
		private Token t;    // last recognized token
		private Token la;   // lookahead token
		private int errDist = minErrDist;
	
		private string direction = "";
		private bool rbaseLabFound = false;
		private bool rbaseLabAllowed = true;

		private bool IsAnd()
		{
			scanner.ResetPeek();
			Token x = la ;
			while (x.kind != _closeBracket && x.kind != _EOF)
				x = scanner.Peek();
			
			x = scanner.Peek();
			bool returnVal = (x.kind == _andOperator);
			return returnVal ;
		}

		private bool IsOr()
		{
			scanner.ResetPeek();
			Token x = la ;
			while (x.kind != _closeBracket && x.kind != _EOF)
				x = scanner.Peek();
			
			x = scanner.Peek();
			return (x.kind == _orOperator);
		}

		private Errors errors;
		private Scanner scanner;
		private StreamWriter sw;
	
	/*------------------------------------------------------------------------*
	 *----- SCANNER DESCRIPTION ----------------------------------------------*
	 *------------------------------------------------------------------------*/
	
		public Parser(Scanner scanner) {
			errors = new Errors();
			this.scanner = scanner;
		}
	
		public Errors ParserErrors {
			get {
				return errors;
			}
		}
	
		private void SynErr (int n) {
			if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
			errDist = 0;
		}
	
		public void SemErr (string msg) {
			if (errDist >= minErrDist) errors.Error(t.line, t.col, msg);
			errDist = 0;
		}
		
		private void Get () {
			for (;;) 
			{
				t = la;
				la = scanner.Scan();
				if (la.kind <= maxT) { ++errDist; break; }
				if (la.kind == 17) 
				{
					if (la.val.Equals("#DIRECTION_FORWARD")) direction = "forward";	
					else if (la.val.Equals("#DIRECTION_BACKWARD")) direction = "backward";
					else if (la.val.Equals("#DIRECTION_BIDIRECTIONAL")) direction = "bidirectional";
					else errors.Error(t.line, t.col, "unknown pragma :" + la.val);
				
				}

				la = t;
			}
		}
		
		private void Expect (int n) {
			if (la.kind==n) Get(); else { SynErr(n); }
		}
		
		private bool StartOf (int s) {
			return set[s, la.kind];
		}
		
		private void ExpectWeak (int n, int follow) {
			if (la.kind == n) Get();
			else {
				SynErr(n);
				while (!StartOf(follow)) Get();
			}
		}
		
		private bool WeakSeparator (int n, int syFol, int repFol) {
			bool[] s = new bool[maxT+1];
			if (la.kind == n) { Get(); return true; }
			else if (StartOf(repFol)) return false;
			else {
				for (int i=0; i <= maxT; i++) {
					s[i] = set[syFol, i] || set[repFol, i] || set[0, i];
				}
				SynErr(n);
				while (!s[la.kind]) Get();
				return StartOf(syFol);
			}
		}
		
		private void Rules() {
			sw.WriteLine("<?xml version=\"1.0\" encoding=\"us-ascii\"?>");
			sw.Write("<rulebase xmlns=\"http://www.ruleml.org/0.86/xsd\" xsi:schemaLocation=\"http://www.ruleml.org/0.86/xsd ruleml-0_86-nafdatalog.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
			if (!String.Empty.Equals(direction)) sw.Write(" direction=\"" + direction + "\"");
			sw.WriteLine(">");
			
			imp();
			while (StartOf(1)) 
			{
				imp();
			}
			sw.WriteLine("</rulebase>");
		}
	
		private void imp() {
			string impName = ""; 
			string atomListInXml=""; 
			string headAtom = ""; 
			int numberOfAtoms = 0;
			int numberOfAtomsInHead = 0;
			bool isFact = false;
		
			if (la.kind == 12) 
			{
				header(out impName);
			}
			if (StartOf(2)) 
			{
				if (IsAnd()) 
				{
					andRelation(ref atomListInXml, ref numberOfAtoms);
				} 
				else if (IsOr()) 
				{
					orRelation(ref atomListInXml, ref numberOfAtoms);
				} 
				else 
				{
					atom(ref atomListInXml, ref numberOfAtoms, ref isFact);
				}
				if (la.kind == 4) 
				{
					Get();
					atom(ref headAtom, ref numberOfAtomsInHead, ref isFact);
				}
			}
			Expect(11);
			if (numberOfAtomsInHead == 0)
			{
				if (numberOfAtoms ==0 ) 
				{
					if (!rbaseLabFound && rbaseLabAllowed  && !impName.Equals(""))
					{
						/* file header : name of rules definition */
						sw.WriteLine("<_rbaselab><ind>{0}</ind></_rbaselab>",impName);
						rbaseLabFound = true;
					}
				}
				else 
				{
					/* fact or query */
					rbaseLabAllowed = false;
					if (! isFact) 
					{
						/*  query */
						sw.WriteLine("<query>");
						if (!impName.Equals("")) 
						{
							sw.WriteLine("<_rlab><ind>" + impName + "</ind></_rlab>");
						}
						sw.WriteLine("<_body>");
						sw.WriteLine(atomListInXml);
						sw.WriteLine("</_body>");
						sw.WriteLine("</query>");						
					}
					else 
					{
						/*  Fact */
						sw.WriteLine("<fact>");
						if (!impName.Equals("")) 
						{
							sw.WriteLine("<_rlab><ind>" + impName + "</ind></_rlab>");
						}
						sw.WriteLine("<_head>");
						sw.WriteLine(atomListInXml);
						sw.WriteLine("</_head>");
						sw.WriteLine("</fact>");
					}
				}
			}
			else 
			{
				/*  implication */
				rbaseLabAllowed = false;
				sw.WriteLine("<imp>");
				if (!impName.Equals("")) 
				{
					sw.WriteLine("<_rlab><ind>" + impName + "</ind></_rlab>");
				}
				sw.WriteLine("<_head>");
				sw.WriteLine(headAtom);
				sw.WriteLine("</_head>");
				sw.WriteLine("<_body>");
				sw.WriteLine(atomListInXml);
				sw.WriteLine("</_body>");
				sw.WriteLine("</imp>"); 	
			}		
		}
	
		private void header(out string label) {
			bool isThereSeparator= false;
			label = "";
		
			Expect(12);
			while (la.kind == 8) 
			{
				Get();
				if (label.Equals("")) 
				{
					label = t.val;
				}
				else 
				{
					label += " " + t.val;
				}
			
			}
			while (la.kind == 5 || la.kind == 11) 
			{
				if (la.kind == 11) 
				{
					Get();
				} 
				else 
				{
					Get();
				}
				label += t.val;
				isThereSeparator= true;
			
				while (la.kind == 8) 
				{
					Get();
					if (isThereSeparator) label += t.val;
					else label += " " + t.val;
					isThereSeparator = false;
				
				}
			}
			Expect(13);
		}

		private void andRelation(ref string atomListInXml, ref int numberOfAtoms) 
		{
			string lhInXml = "" ;
			string rhInXml = "" ;
			string rhPartInXml = "" ;
			bool isFact = false ;	
		
			if (la.kind == 9) 
			{
				Get();
			}
			atom(ref lhInXml, ref numberOfAtoms, ref isFact);
			while (la.kind == 2) 
			{
				Get();
				if (IsOr()) 
				{
					orRelation(ref rhPartInXml, ref numberOfAtoms);
				} 
				else if (la.kind == 1 || la.kind == 8 || la.kind == 14) 
				{
					atom(ref rhPartInXml, ref numberOfAtoms, ref isFact);
				} 
				else SynErr(17);
				rhInXml += rhPartInXml ;
			}
			if (la.kind == 10) 
			{
				Get();
			}
			atomListInXml = lhInXml + rhInXml ; 
			atomListInXml = "<and>" + atomListInXml + "</and>"; 		
		
		}

		private void orRelation(ref string atomListInXml, ref int numberOfAtoms) 
		{
			string lhInXml = "" ;		
			string rhInXml = "" ;		
			string rhPartInXml = "" ;
			bool isFact = false ;	
		
			if (la.kind == 9) 
			{
				Get();
			}
			atom(ref lhInXml, ref numberOfAtoms, ref isFact);
			while (la.kind == 3) 
			{
				Get();
				if (IsAnd()) 
				{
					andRelation(ref rhPartInXml, ref numberOfAtoms);
				} 
				else if (la.kind == 1 || la.kind == 8 || la.kind == 14) 
				{
					atom(ref rhPartInXml, ref numberOfAtoms, ref isFact);
				} 
				else SynErr(18);
				rhInXml += rhPartInXml ;
			}
			if (la.kind == 10) 
			{
				Get();
			}
			atomListInXml = lhInXml + rhInXml ;
			atomListInXml = "<or>" + atomListInXml + "</or>"; 	
		}	
	
		private void atom(ref string atomListInXml, ref int numberOfAtoms, ref bool isFact) 
		{
			bool isNAF = false ;
			string name="";  string atomInXml="";
			numberOfAtoms++; 
			bool isThereVar = false;
		
			if (la.kind == 1 || la.kind == 14) 
			{
				if (la.kind == 1) 
				{
					Get();
					isNAF = true ;
				
				} 
				else 
				{
					Get();
					isFact = true ;
				
				}
			}
			identifier(out name);
			atomInXml = "<_opr><rel>" + name + "</rel></_opr>"; 
		
		
			Expect(6);
			arg(out name, out isThereVar);
			
			if (name == String.Empty) {
				SynErr(6);
				throw new InvalidDataException("Bad HRF syntax: impossible to locate atom members!");
			}
			
			if (isThereVar)
			{
				atomInXml = atomInXml + "<var>" +  name + "</var>"; 
			}
			else 
			{
				atomInXml = atomInXml + "<ind>" +  name + "</ind>"; 
			}
		
			while (la.kind == 5) 
			{
				Get();
				arg(out name, out isThereVar);
				if (isThereVar)
				{
					atomInXml = atomInXml + "<var>" +  name + "</var>"; 
				}
				else 
				{
					atomInXml = atomInXml + "<ind>" +  name + "</ind>"; 
				}
			
			}
			Expect(7);
			if ( isNAF )
				atomInXml = "<naf><atom>" + atomInXml + "</atom></naf>"; 
			else
				atomInXml = "<atom>" + atomInXml + "</atom>"; 
		
			atomListInXml = atomInXml ; 		
		}


		private void identifier(out string name) {
			Expect(8);
			name = t.val; 
		
			while (la.kind == 8) 
			{
				Get();
				name = name + " " + t.val; 
			
			}
		}
	
		private void arg(out string xmlArg, out bool isThereVar) {
			string name = "";
			string fctName = "";
			string id = ""; 
			string argList = "";
			xmlArg = "";
			isThereVar = false;
		
			if (la.kind == 15) 
			{
				Get();
				identifier(out name);
				xmlArg = name; 
				isThereVar = true;
			
			} 
			else if (la.kind == 8) 
			{
				identifier(out fctName);
				while (la.kind == 11) 
				{
					Get();
					identifier(out id);
					fctName += ";" + id;
				
				}
				if (la.kind == 9) 
				{
					Get();
					arg(out name, out isThereVar);
					argList = name;
				
					while (la.kind == 5) 
					{
						Get();
						arg(out name, out isThereVar);
						argList = argList + "," + name;
					
					}
					Expect(10);
				}
				xmlArg = fctName;
				if (!argList.Equals(""))
				{
					xmlArg += '(' + argList + ')'; 
				}
			
			} 
			else SynErr(19);
		}
	
	
	
		public void Parse(Stream stream) {
			sw = new StreamWriter(stream);
			la = new Token();
			la.val = "";		
			Get();
			Rules();
			sw.Flush();
		}
	
		private bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x},
		{x,T,x,x, x,x,x,x, T,T,x,T, T,x,T,x, x,x},
		{x,T,x,x, x,x,x,x, T,T,x,x, x,x,T,x, x,x}

							  };
	} // end Parser
	
}
