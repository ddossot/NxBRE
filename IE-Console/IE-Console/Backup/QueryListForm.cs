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
	public class QueryListForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ListBox listBoxQueries;
		private System.Windows.Forms.Panel panelOKCancel;
		
		public int SelectedQueryIndex {
			get {
				return listBoxQueries.SelectedIndex;
			}
		}
		
		public QueryListForm(IList<string> queryLabels)
		{
			InitializeComponent();
			
			int i = 1;
			foreach(string queryLabel in queryLabels)
				listBoxQueries.Items.Add((i++) +
				                         ": " +
				                         ((queryLabel != null)?queryLabel:"<anonymous>"));
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(QueryListForm));
			this.panelOKCancel = new System.Windows.Forms.Panel();
			this.listBoxQueries = new System.Windows.Forms.ListBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.panelOKCancel.SuspendLayout();
			this.SuspendLayout();
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
			// listBoxQueries
			// 
			this.listBoxQueries.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxQueries.ItemHeight = 17;
			this.listBoxQueries.Location = new System.Drawing.Point(0, 0);
			this.listBoxQueries.Name = "listBoxQueries";
			this.listBoxQueries.Size = new System.Drawing.Size(392, 344);
			this.listBoxQueries.TabIndex = 0;
			this.listBoxQueries.DoubleClick += new System.EventHandler(this.ListBoxQueriesDoubleClick);
			this.listBoxQueries.SelectedIndexChanged += new System.EventHandler(this.ListBoxQueriesSelectedIndexChanged);
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Enabled = false;
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
			// QueryListForm
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(392, 344);
			this.Controls.Add(this.panelOKCancel);
			this.Controls.Add(this.listBoxQueries);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "QueryListForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Query List";
			this.panelOKCancel.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
		void ListBoxQueriesDoubleClick(object sender, System.EventArgs e)
		{
			if (SelectedQueryIndex >= 0) this.DialogResult = DialogResult.OK;
		}
		
		void ListBoxQueriesSelectedIndexChanged(object sender, System.EventArgs e)
		{
			buttonOK.Enabled = (SelectedQueryIndex >= 0);
		}
				
	}
}
