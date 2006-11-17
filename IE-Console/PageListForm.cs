namespace NxBRE.InferenceEngine.Console
{
	using System;
	using System.Collections.Generic;
	using System.Windows.Forms;

	/// <summary>
	/// 
	/// Basic Console for NxBRE Inference Engine
	/// 
	/// </summary>
	/// <remarks>
	/// Shamlessly uncommented code.
	/// </remarks>
	public class PageListForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panelOKCancel;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ListBox listBoxPages;
		
		public string[] SelectedPages {
			get {
				string[] result = new string[listBoxPages.SelectedItems.Count];
				listBoxPages.SelectedItems.CopyTo(result, 0);
				return result;
			}
		}
		
		public PageListForm(IList<string> pageNames)
		{
			InitializeComponent();
			
			foreach(string pageName in pageNames)	{
				listBoxPages.Items.Add(pageName);
				listBoxPages.SelectedItem = pageName;
			}
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this.listBoxPages = new System.Windows.Forms.ListBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.panelOKCancel = new System.Windows.Forms.Panel();
			this.panelOKCancel.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBoxPages
			// 
			this.listBoxPages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxPages.ItemHeight = 16;
			this.listBoxPages.Location = new System.Drawing.Point(0, 0);
			this.listBoxPages.Name = "listBoxPages";
			this.listBoxPages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxPages.Size = new System.Drawing.Size(392, 340);
			this.listBoxPages.Sorted = true;
			this.listBoxPages.TabIndex = 0;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(104, 8);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 32);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(208, 8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 32);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			// 
			// panelOKCancel
			// 
			this.panelOKCancel.Controls.Add(this.buttonCancel);
			this.panelOKCancel.Controls.Add(this.buttonOK);
			this.panelOKCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelOKCancel.Location = new System.Drawing.Point(0, 296);
			this.panelOKCancel.Name = "panelOKCancel";
			this.panelOKCancel.Size = new System.Drawing.Size(392, 48);
			this.panelOKCancel.TabIndex = 3;
			// 
			// PageListForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(392, 344);
			this.Controls.Add(this.panelOKCancel);
			this.Controls.Add(this.listBoxPages);
			this.Name = "PageListForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Page List";
			this.panelOKCancel.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
				
	}
}
