namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_CreateTag
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
            this._button_Help = new Infragistics.Win.Misc.UltraButton();
            this._button_OK = new Infragistics.Win.Misc.UltraButton();
            this._button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._textBox_Description = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._textBox_TagName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._button_OK);
            this._bfd_ButtonPanel.Controls.Add(this._button_Help);
            this._bfd_ButtonPanel.Controls.Add(this._button_Cancel);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 302);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(565, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_OK, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.label4);
            this._bf_MainPanel.Controls.Add(this._textBox_Description);
            this._bf_MainPanel.Controls.Add(this._textBox_TagName);
            this._bf_MainPanel.Controls.Add(this.label1);
            this._bf_MainPanel.GradientBorderMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.None;
            this._bf_MainPanel.GradientCornerMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.Square;
            this._bf_MainPanel.Size = new System.Drawing.Size(565, 249);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.GradientBorderMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.None;
            this._bf_HeaderPanel.GradientCornerMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.Square;
            this._bf_HeaderPanel.Size = new System.Drawing.Size(565, 53);
            // 
            // _button_Help
            // 
            this._button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Help.Location = new System.Drawing.Point(467, 10);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 4;
            this._button_Help.Text = "&Help";
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(306, 10);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 2;
            this._button_OK.Text = "&OK";
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(386, 10);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 3;
            this._button_Cancel.Text = "&Cancel";
            // 
            // _textBox_Description
            // 
            this._textBox_Description.AcceptsReturn = true;
            this._textBox_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_Description.Location = new System.Drawing.Point(101, 78);
            this._textBox_Description.Multiline = true;
            this._textBox_Description.Name = "_textBox_Description";
            this._textBox_Description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBox_Description.Size = new System.Drawing.Size(441, 140);
            this._textBox_Description.TabIndex = 1;
            this._textBox_Description.TextChanged += new System.EventHandler(this._textBox_Description_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(26, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Description";
            // 
            // _textBox_TagName
            // 
            this._textBox_TagName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_TagName.Location = new System.Drawing.Point(101, 35);
            this._textBox_TagName.MaxLength = 128;
            this._textBox_TagName.Name = "_textBox_TagName";
            this._textBox_TagName.Size = new System.Drawing.Size(441, 20);
            this._textBox_TagName.TabIndex = 0;
            this._textBox_TagName.TextChanged += new System.EventHandler(this._textBox_TagName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(25, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Name";
            // 
            // Form_CreateTag
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(565, 342);
            this.Description = "Create Server Group Tag";
            this.Name = "Form_CreateTag";
            this.Text = "Create Tag";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.ServerTag_48;
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectDatabase_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _button_Cancel;
        private System.Windows.Forms.TextBox _textBox_Description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _textBox_TagName;
        private System.Windows.Forms.Label label4;
        private Infragistics.Win.Misc.UltraButton _button_Help;
        private Infragistics.Win.Misc.UltraButton _button_OK;
    }
}
