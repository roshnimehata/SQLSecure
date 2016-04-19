namespace Idera.SQLsecure.UI.Console.Forms
{
	 partial class Form_Connecting
	 {
		  /// <summary>
		  /// Required designer variable.
		  /// </summary>
		  private System.ComponentModel.IContainer components = null;

		  /// <summary>
		  /// Clean up any resources being used.
		  /// </summary>
		  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		  protected override void Dispose(bool disposing)
		  {
				if (disposing && (components != null))
				{
					 components.Dispose();
				}
				base.Dispose(disposing);
		  }

		  #region Windows Form Designer generated code

		  /// <summary>
		  /// Required method for Designer support - do not modify
		  /// the contents of this method with the code editor.
		  /// </summary>
		  private void InitializeComponent()
		  {
				Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
				Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
				this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
				this.labelServer = new Infragistics.Win.Misc.UltraLabel();
				this.SuspendLayout();
				// 
				// ultraLabel1
				// 
				appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
				this.ultraLabel1.Appearance = appearance1;
				this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				this.ultraLabel1.Location = new System.Drawing.Point(0, 30);
				this.ultraLabel1.Name = "ultraLabel1";
				this.ultraLabel1.Size = new System.Drawing.Size(410, 16);
				this.ultraLabel1.TabIndex = 0;
				this.ultraLabel1.Text = "Connecting to SQL secure Repository:";
				this.ultraLabel1.WrapText = false;
				// 
				// labelServer
				// 
				appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
				this.labelServer.Appearance = appearance2;
				this.labelServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				this.labelServer.Location = new System.Drawing.Point(40, 66);
				this.labelServer.Name = "labelServer";
				this.labelServer.Size = new System.Drawing.Size(330, 32);
				this.labelServer.TabIndex = 3;
				this.labelServer.Text = "server instance";
				// 
				// Form_Connecting
				// 
				this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
				this.ClientSize = new System.Drawing.Size(410, 114);
				this.ControlBox = false;
				this.Controls.Add(this.labelServer);
				this.Controls.Add(this.ultraLabel1);
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
				this.MaximizeBox = false;
				this.Name = "Form_Connecting";
				this.ShowInTaskbar = false;
				this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
				this.Text = "Connecting...";
				this.ResumeLayout(false);

		  }

		  #endregion

		  private Infragistics.Win.Misc.UltraLabel ultraLabel1;
		  private Infragistics.Win.Misc.UltraLabel labelServer;
	 }
}