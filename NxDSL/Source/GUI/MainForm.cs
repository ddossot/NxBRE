using System;
using System.Drawing;

using System.Windows.Forms;

using NxDSL;

using Antlr.Runtime;

namespace NxDSL.GUI
{
	public partial class MainForm : Form
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		public MainForm() {
			InitializeComponent();
			
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "NxDSL files|*.nxdsl";
			openFileDialog.Title = "Load DSL File";
			
			if (DialogResult.OK == openFileDialog.ShowDialog(this)) {
				string dslFile = openFileDialog.FileName;
				string definitionFile = dslFile + ".defs";
				
				Definitions definitions = new Definitions(definitionFile);
				
				HtmlBuilderTokenSource hbts =
							new HtmlBuilderTokenSource(
								new InferenceRules_ENLexer(
									new ANTLRFileStream(dslFile)),
								definitions);
				
				InferenceRules_ENParser ipr = new InferenceRules_ENParser(new CommonTokenStream(hbts));
				
				ipr.rbb = new RuleBaseBuilder(definitions);
				
				try {
					ipr.rulebase();
				} catch(Exception re) {
					hbts.PrependToHtml("<font color='#FF0000'><b>" + re.Message + "</b></font><br/><br/>");
				}
				
				this.Html = hbts.Html;
			} else {
				this.Html = "Next time, select a DSL file!";
			}

		}
	}
}
