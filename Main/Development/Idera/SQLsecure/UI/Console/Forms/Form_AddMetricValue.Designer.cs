namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_AddMetricValue
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
            this.ultraButtonCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonOK = new Infragistics.Win.Misc.UltraButton();
            this.textBox_Values = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButtonCancel);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButtonOK);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 243);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(505, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButtonOK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButtonCancel, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.label1);
            this._bf_MainPanel.Controls.Add(this.textBox_Values);
            this._bf_MainPanel.Size = new System.Drawing.Size(505, 190);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(505, 53);
            // 
            // ultraButtonCancel
            // 
            this.ultraButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButtonCancel.Location = new System.Drawing.Point(421, 8);
            this.ultraButtonCancel.Name = "ultraButtonCancel";
            this.ultraButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonCancel.TabIndex = 1;
            this.ultraButtonCancel.Text = "&Cancel";
            // 
            // ultraButtonOK
            // 
            this.ultraButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButtonOK.Location = new System.Drawing.Point(340, 9);
            this.ultraButtonOK.Name = "ultraButtonOK";
            this.ultraButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonOK.TabIndex = 2;
            this.ultraButtonOK.Text = "&OK";
            // 
            // textBox_Values
            // 
            this.textBox_Values.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Values.BackColor = System.Drawing.Color.White;
            this.textBox_Values.Location = new System.Drawing.Point(21, 27);
            this.textBox_Values.MaxLength = 4000;
            this.textBox_Values.Multiline = true;
            this.textBox_Values.Name = "textBox_Values";
            this.textBox_Values.Size = new System.Drawing.Size(458, 157);
            this.textBox_Values.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(21, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Specify multiple items one per line";
            // 
            // Form_AddMetricValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(505, 283);
            this.Name = "Form_AddMetricValue";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.edit_policy_491;
            this.Text = "Edit Values for Security Check";
            this.Load += new System.EventHandler(this.Form_AddMetricValue_Load);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButtonCancel;
        private Infragistics.Win.Misc.UltraButton ultraButtonOK;
        private System.Windows.Forms.TextBox textBox_Values;
        private System.Windows.Forms.Label label1;
    }
}
