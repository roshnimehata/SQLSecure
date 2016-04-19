namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_TestSMTPEmail
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
            this._btnOk = new System.Windows.Forms.Button();
            this._lblDirections = new System.Windows.Forms.Label();
            this._btnCancel = new System.Windows.Forms.Button();
            this._tbRecepient = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Location = new System.Drawing.Point(68, 65);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 6;
            this._btnOk.Text = "&OK";
            // 
            // _lblDirections
            // 
            this._lblDirections.Location = new System.Drawing.Point(12, 9);
            this._lblDirections.Name = "_lblDirections";
            this._lblDirections.Size = new System.Drawing.Size(216, 16);
            this._lblDirections.TabIndex = 4;
            this._lblDirections.Text = "&Enter a recipient for the SMTP test email:";
            this._lblDirections.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(156, 65);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 7;
            this._btnCancel.Text = "&Cancel";
            // 
            // _tbRecepient
            // 
            this._tbRecepient.Location = new System.Drawing.Point(12, 33);
            this._tbRecepient.Name = "_tbRecepient";
            this._tbRecepient.Size = new System.Drawing.Size(216, 20);
            this._tbRecepient.TabIndex = 5;
            // 
            // Form_TestSMTPEmail
            // 
            this.AcceptButton = this._btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(246, 103);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._lblDirections);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._tbRecepient);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TestSMTPEmail";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Test SMTP Email";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnOk;
        private System.Windows.Forms.Label _lblDirections;
        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.TextBox _tbRecepient;
    }
}