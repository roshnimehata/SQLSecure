namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_StatusDetails
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
            this._btn_Close = new System.Windows.Forms.Button();
            this._lbl_0 = new System.Windows.Forms.LinkLabel();
            this._lbl_1 = new System.Windows.Forms.LinkLabel();
            this._lbl_2 = new System.Windows.Forms.LinkLabel();
            this._lbl_3 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // _btn_Close
            // 
            this._btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btn_Close.Location = new System.Drawing.Point(296, 228);
            this._btn_Close.Name = "_btn_Close";
            this._btn_Close.Size = new System.Drawing.Size(75, 23);
            this._btn_Close.TabIndex = 0;
            this._btn_Close.Text = "Close";
            this._btn_Close.UseVisualStyleBackColor = true;
            // 
            // _lbl_0
            // 
            this._lbl_0.Location = new System.Drawing.Point(12, 9);
            this._lbl_0.Name = "_lbl_0";
            this._lbl_0.Size = new System.Drawing.Size(359, 51);
            this._lbl_0.TabIndex = 1;
            this._lbl_0.TabStop = true;
            this._lbl_0.Text = "lbl0";
            // 
            // _lbl_1
            // 
            this._lbl_1.Location = new System.Drawing.Point(12, 64);
            this._lbl_1.Name = "_lbl_1";
            this._lbl_1.Size = new System.Drawing.Size(359, 51);
            this._lbl_1.TabIndex = 2;
            this._lbl_1.TabStop = true;
            this._lbl_1.Text = "lbl0";
            // 
            // _lbl_2
            // 
            this._lbl_2.Location = new System.Drawing.Point(12, 119);
            this._lbl_2.Name = "_lbl_2";
            this._lbl_2.Size = new System.Drawing.Size(359, 51);
            this._lbl_2.TabIndex = 3;
            this._lbl_2.TabStop = true;
            this._lbl_2.Text = "lbl0";
            // 
            // _lbl_3
            // 
            this._lbl_3.Location = new System.Drawing.Point(12, 174);
            this._lbl_3.Name = "_lbl_3";
            this._lbl_3.Size = new System.Drawing.Size(359, 51);
            this._lbl_3.TabIndex = 4;
            this._lbl_3.TabStop = true;
            this._lbl_3.Text = "lbl0";
            // 
            // Form_StatusDetails
            // 
            this.AcceptButton = this._btn_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this._btn_Close;
            this.ClientSize = new System.Drawing.Size(383, 263);
            this.Controls.Add(this._lbl_3);
            this.Controls.Add(this._lbl_2);
            this.Controls.Add(this._lbl_1);
            this.Controls.Add(this._lbl_0);
            this.Controls.Add(this._btn_Close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_StatusDetails";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Status Details";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btn_Close;
        private System.Windows.Forms.LinkLabel _lbl_0;
        private System.Windows.Forms.LinkLabel _lbl_1;
        private System.Windows.Forms.LinkLabel _lbl_2;
        private System.Windows.Forms.LinkLabel _lbl_3;
    }
}