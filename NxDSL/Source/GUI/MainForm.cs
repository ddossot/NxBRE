/*
 * Created by SharpDevelop.
 * User: David Dossot
 * Date: 6/9/2007
 * Time: 3:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using NxDSL;

using Antlr.Runtime;

namespace NxDSL.GUI
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: using the parser would probably be a better idea because the lexer does not recognize
			// statements hence does not color CHARs that are recognized as tokens even if they are in
			// a statement
			//
			
			InferenceRules_ENLexer irl = new InferenceRules_ENLexer(
								new ANTLRFileStream("../../../../Rulefiles/discount.nxdsl"));
			
			StringBuilder sb = new StringBuilder("<html><body><font face='arial' color='#000000'>");
			
			IToken token;
			while((token = irl.NextToken()) != Token.EOF_TOKEN) {
				Console.Write(token.Text + "/" + token.Type);
				
				if (token.Type == InferenceRules_ENLexer.NEWLINE) {
					sb.Append("<br/>");
				}
				else {
					if ((token.Type == InferenceRules_ENLexer.RULE)
					    || (token.Type == InferenceRules_ENLexer.FACT)
					    || (token.Type == InferenceRules_ENLexer.QUERY)){
						sb.Append("<br/>");
					}
					
					string color = null;
					
					if ((token.Type == InferenceRules_ENLexer.CHAR)
					    || (token.Type == InferenceRules_ENLexer.NUMERIC)) {
						color = "#0000FF";
					}
					
					if (color != null) {
						sb.Append("<font color='").Append(color).Append("'>");
					} else {
						sb.Append("<b>");
					}
					
					sb.Append(token.Text);
					
					if (color != null) {
						sb.Append("</font>");
					} else {
						sb.Append("</b>");
					}
				}
			}
			
			Html = sb.Append("</font></body></html>").ToString();
		}
	}
}
