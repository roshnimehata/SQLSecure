namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_GroomingSchedule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GroomingSchedule));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_EnableScheduling = new System.Windows.Forms.CheckBox();
            this.button_ChangeSchedule = new Infragistics.Win.Misc.UltraButton();
            this.textBox_ScheduleDescription = new System.Windows.Forms.TextBox();
            this.button_Help = new Infragistics.Win.Misc.UltraButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_AgentStatus = new System.Windows.Forms.Label();
            this.pictureBox_AgentStatus = new System.Windows.Forms.PictureBox();
            this.label_AgentStatus1 = new System.Windows.Forms.Label();
            this.ultraButton_OK = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AgentStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_OK);
            this._bfd_ButtonPanel.Controls.Add(this.button_Help);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 378);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(538, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Cancel, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.groupBox1);
            this._bf_MainPanel.Controls.Add(this.groupBox2);
            this._bf_MainPanel.Size = new System.Drawing.Size(538, 325);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(538, 53);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBox_EnableScheduling);
            this.groupBox1.Controls.Add(this.button_ChangeSchedule);
            this.groupBox1.Controls.Add(this.textBox_ScheduleDescription);
            this.groupBox1.Location = new System.Drawing.Point(22, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 185);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grooming Schedule";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(469, 40);
            this.label2.TabIndex = 1017;
            this.label2.Text = "Depending on the amount of data collected, grooming can be a performance intensiv" +
                "e operation. It is recommended that grooming be scheduled for a time when the re" +
                "pository is less active.";
            // 
            // checkBox_EnableScheduling
            // 
            this.checkBox_EnableScheduling.AutoSize = true;
            this.checkBox_EnableScheduling.Location = new System.Drawing.Point(13, 72);
            this.checkBox_EnableScheduling.Name = "checkBox_EnableScheduling";
            this.checkBox_EnableScheduling.Size = new System.Drawing.Size(155, 17);
            this.checkBox_EnableScheduling.TabIndex = 1016;
            this.checkBox_EnableScheduling.Text = "&Enable Grooming Schedule";
            this.checkBox_EnableScheduling.UseVisualStyleBackColor = true;
            this.checkBox_EnableScheduling.CheckedChanged += new System.EventHandler(this.checkBox_EnableScheduling_CheckedChanged);
            // 
            // button_ChangeSchedule
            // 
            this.button_ChangeSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_ChangeSchedule.Location = new System.Drawing.Point(400, 145);
            this.button_ChangeSchedule.Name = "button_ChangeSchedule";
            this.button_ChangeSchedule.Size = new System.Drawing.Size(80, 23);
            this.button_ChangeSchedule.TabIndex = 2;
            this.button_ChangeSchedule.Text = "&Change...";
            this.button_ChangeSchedule.Click += new System.EventHandler(this.button_ChangeSchedule_Click);
            // 
            // textBox_ScheduleDescription
            // 
            this.textBox_ScheduleDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ScheduleDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_ScheduleDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_ScheduleDescription.Location = new System.Drawing.Point(13, 98);
            this.textBox_ScheduleDescription.Multiline = true;
            this.textBox_ScheduleDescription.Name = "textBox_ScheduleDescription";
            this.textBox_ScheduleDescription.ReadOnly = true;
            this.textBox_ScheduleDescription.Size = new System.Drawing.Size(381, 70);
            this.textBox_ScheduleDescription.TabIndex = 3;
            this.textBox_ScheduleDescription.TabStop = false;
            // 
            // button_Help
            // 
            this.button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Help.Location = new System.Drawing.Point(438, 9);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(75, 23);
            this.button_Help.TabIndex = 9;
            this.button_Help.Text = "&Help";
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label_AgentStatus);
            this.groupBox2.Controls.Add(this.pictureBox_AgentStatus);
            this.groupBox2.Controls.Add(this.label_AgentStatus1);
            this.groupBox2.Location = new System.Drawing.Point(22, 211);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(495, 102);
            this.groupBox2.TabIndex = 1022;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SQL Server Agent Status";
            // 
            // label_AgentStatus
            // 
            this.label_AgentStatus.AutoSize = true;
            this.label_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.label_AgentStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_AgentStatus.Location = new System.Drawing.Point(21, 84);
            this.label_AgentStatus.Name = "label_AgentStatus";
            this.label_AgentStatus.Size = new System.Drawing.Size(48, 13);
            this.label_AgentStatus.TabIndex = 14;
            this.label_AgentStatus.Text = "Started";
            // 
            // pictureBox_AgentStatus
            // 
            this.pictureBox_AgentStatus.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_AgentStatus.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_AgentStatus.Image")));
            this.pictureBox_AgentStatus.Location = new System.Drawing.Point(21, 24);
            this.pictureBox_AgentStatus.Name = "pictureBox_AgentStatus";
            this.pictureBox_AgentStatus.Size = new System.Drawing.Size(48, 56);
            this.pictureBox_AgentStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_AgentStatus.TabIndex = 13;
            this.pictureBox_AgentStatus.TabStop = false;
            // 
            // label_AgentStatus1
            // 
            this.label_AgentStatus1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_AgentStatus1.BackColor = System.Drawing.Color.Transparent;
            this.label_AgentStatus1.Location = new System.Drawing.Point(102, 24);
            this.label_AgentStatus1.Name = "label_AgentStatus1";
            this.label_AgentStatus1.Size = new System.Drawing.Size(374, 56);
            this.label_AgentStatus1.TabIndex = 12;
            this.label_AgentStatus1.Text = "SQLsecure uses the SQL Server Agent for data collection and grooming. This agent " +
                "is located on the SQL server hosting the Repository database.";
            // 
            // ultraButton_OK
            // 
            this.ultraButton_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButton_OK.Location = new System.Drawing.Point(258, 9);
            this.ultraButton_OK.Name = "ultraButton_OK";
            this.ultraButton_OK.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_OK.TabIndex = 10;
            this.ultraButton_OK.Text = "&OK";
            this.ultraButton_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // ultraButton_Cancel
            // 
            this.ultraButton_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton_Cancel.Location = new System.Drawing.Point(348, 9);
            this.ultraButton_Cancel.Name = "ultraButton_Cancel";
            this.ultraButton_Cancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Cancel.TabIndex = 11;
            this.ultraButton_Cancel.Text = "&Cancel";
            this.ultraButton_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // Form_GroomingSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(538, 418);
            this.Description = "Specify Grooming Schedule for the SQLsecure Repository";
            this.Name = "Form_GroomingSchedule";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_ActivityHistory_481;
            this.Text = "Grooming Schedule";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_GroomingSchedule_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_AgentStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton button_ChangeSchedule;
        private System.Windows.Forms.TextBox textBox_ScheduleDescription;
        private System.Windows.Forms.CheckBox checkBox_EnableScheduling;
        private Infragistics.Win.Misc.UltraButton button_Help;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_AgentStatus;
        private System.Windows.Forms.PictureBox pictureBox_AgentStatus;
        private System.Windows.Forms.Label label_AgentStatus1;
        private System.Windows.Forms.Label label2;
        private Infragistics.Win.Misc.UltraButton ultraButton_Cancel;
        private Infragistics.Win.Misc.UltraButton ultraButton_OK;
    }
}
