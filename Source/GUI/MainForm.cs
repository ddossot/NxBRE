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
			// The InitializeComponent() call is required for Windows Forms designer support.
			InitializeComponent();
			
			
			InferenceRules_ENLexer irl = new InferenceRules_ENLexer(
								new ANTLRFileStream("../../../../Rulefiles/discount.nxdsl"));
			
			StringBuilder sb = new StringBuilder("<html><body><font face='courier' color='#000000'>");
			
			IToken token;
			bool inQuote = false;
			
			while((token = irl.NextToken()) != Token.EOF_TOKEN) {
				Console.Write(token.Text + "/" + token.Type);
				
				if (token.Type == InferenceRules_ENLexer.NEWLINE) {
					sb.Append("<br/>");
				}
				else if (token.Type == InferenceRules_ENLexer.TAB) {
					sb.Append("&nbsp;&nbsp;");
				}
				else {
					if ((token.Type == InferenceRules_ENLexer.RULE)
					    || (token.Type == InferenceRules_ENLexer.FACT)
					    || (token.Type == InferenceRules_ENLexer.QUERY)){
						sb.Append("<br/>");
					}
					
					if ((token.Type == InferenceRules_ENLexer.QUOTE) && (!inQuote)) {
						sb.Append("<font color='#0000FF'>");
						inQuote = true;
						sb.Append(token.Text);
					}
					else if ((token.Type == InferenceRules_ENLexer.QUOTE) && (inQuote)) {
						sb.Append(token.Text);
						sb.Append("</font>");
						inQuote = false;
					}
					else if ((inQuote)
					        || (token.Type == InferenceRules_ENLexer.CHAR)
					        || (token.Type == InferenceRules_ENLexer.SPACE)
					        || (token.Type == InferenceRules_ENLexer.NUMERIC)) {
						sb.Append(token.Text);
					}
					else if ((token.Type == InferenceRules_ENLexer.COUNT)
					        || (token.Type == InferenceRules_ENLexer.DEDUCT)
					        || (token.Type == InferenceRules_ENLexer.FORGET)
					        || (token.Type == InferenceRules_ENLexer.MODIFY)) {
						sb.Append("<font color='#990066'><b>").Append(token.Text).Append("</b></font>");
					}
					else {
						sb.Append("<b>").Append(token.Text).Append("</b>");
					}
				}
			}
			
			Html = sb.Append("</font></body></html>").ToString();
		}
	}
}
