/*
 * ajma.Utils.InputBoxForm
 * Displays a prompt in a dialog box, waits for the user to input text or click a button, and then returns a string containing the contents of the text box.
 *  
 * Andrew J. Ma
 * ajmaonline@hotmail.com
 */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ajma.Utils
{
	/// <summary>
	/// Summary description for InputBoxForm.
	/// </summary>
	internal class InputBoxForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblText;
		private System.Windows.Forms.TextBox txtResult;
		private System.Windows.Forms.Button btnCancel;
		private string strReturnValue;
		private Point pntStartLocation;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public InputBoxForm()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			this.strReturnValue = String.Empty;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtResult = new System.Windows.Forms.TextBox();
			this.lblText = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(288, 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtResult
			// 
			this.txtResult.Location = new System.Drawing.Point(8, 80);
			this.txtResult.Name = "txtResult";
			this.txtResult.Size = new System.Drawing.Size(344, 24);
			this.txtResult.TabIndex = 0;
			this.txtResult.Text = "";
			// 
			// lblText
			// 
			this.lblText.Location = new System.Drawing.Point(16, 8);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(264, 64);
			this.lblText.TabIndex = 3;
			this.lblText.Text = "InputBox";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(288, 8);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// InputBoxForm
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(362, 111);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.txtResult);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputBoxForm";
			this.Text = "InputBox";
			this.Load += new System.EventHandler(this.InputBoxForm_Load);
			this.ResumeLayout(false);
		}
		#endregion

		private void InputBoxForm_Load(object sender, System.EventArgs e)
		{
			if (!this.pntStartLocation.IsEmpty) 
			{
				this.Top = this.pntStartLocation.X;
				this.Left = this.pntStartLocation.Y;
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.strReturnValue = this.txtResult.Text;
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public string Title
		{
			set
			{
				this.Text = value;
			}
		}

		public string Prompt
		{
			set
			{
				this.lblText.Text = value;
			}
		}

		public string ReturnValue
		{
			get
			{
				return strReturnValue;
			}
		}

		public string DefaultResponse
		{
			set
			{
				this.txtResult.Text = value;
				this.txtResult.SelectAll();
			}
		}

		public Point StartLocation
		{
			set
			{
				this.pntStartLocation = value;
			}
		}
	}
}
