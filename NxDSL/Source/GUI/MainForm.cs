using System;
using System.Drawing;

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
			
			Definitions definitions = new Definitions("../../../../Rulefiles/discount.nxdsl.defs");
			
			HtmlBuilderTokenSource hbts =
						new HtmlBuilderTokenSource(
							new InferenceRules_ENLexer(
								new ANTLRFileStream("../../../../Rulefiles/discount.nxdsl")),
							definitions);
			
			InferenceRules_ENParser ipr = new InferenceRules_ENParser(new CommonTokenStream(hbts));
			
			try {
				ipr.rulebase();
			} catch(Exception re) {
				hbts.PrependToHtml("<font color='#FF0000'><b>" + re.Message + "</b></font><br/><br/>");
			}
			
			this.Html = hbts.Html;
		}
	}
}
