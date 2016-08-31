namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_ScheduleJob
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
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.scheduleControlForm1 = new Idera.SQLsecure.UI.Console.Forms.ScheduleControlForm();
            this.SuspendLayout();
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(409, 327);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "&Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(320, 327);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "&OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // scheduleControlForm1
            // 
            this.scheduleControlForm1.BackColor = System.Drawing.Color.Transparent;
            this.scheduleControlForm1.Location = new System.Drawing.Point(11, 12);
            this.scheduleControlForm1.Name = "scheduleControlForm1";
            this.scheduleControlForm1.Size = new System.Drawing.Size(489, 296);
            this.scheduleControlForm1.TabIndex = 0;
            // 
            // Form_ScheduleJob
            // 
            this.AcceptButton = this.button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(510, 373);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.scheduleControlForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form_ScheduleJob";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Job Schedule";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_ScheduleJob_HelpRequested);
            this.ResumeLayout(false);

        }

        #endregion

        private ScheduleControlForm scheduleControlForm1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_OK;

    }
}