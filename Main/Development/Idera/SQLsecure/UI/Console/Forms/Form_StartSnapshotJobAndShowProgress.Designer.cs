namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_StartSnapshotJobAndShowProgress
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
            this.components = new System.ComponentModel.Container();
            this.button_OK = new Infragistics.Win.Misc.UltraButton();
            this.timer_Status = new System.Windows.Forms.Timer(this.components);
            this.pictureBox_Step1 = new System.Windows.Forms.PictureBox();
            this.label_Step1 = new System.Windows.Forms.Label();
            this.pictureBox_Step2 = new System.Windows.Forms.PictureBox();
            this.label_Step2 = new System.Windows.Forms.Label();
            this.pictureBox_Step3 = new System.Windows.Forms.PictureBox();
            this.pictureBox_Step4 = new System.Windows.Forms.PictureBox();
            this.label_Step3 = new System.Windows.Forms.Label();
            this.pictureBox_Step5 = new System.Windows.Forms.PictureBox();
            this.label_Step5 = new System.Windows.Forms.Label();
            this.pictureBox_Step6 = new System.Windows.Forms.PictureBox();
            this.label_Step6 = new System.Windows.Forms.Label();
            this.button_HideandNotify = new Infragistics.Win.Misc.UltraButton();
            this.label_Step4 = new System.Windows.Forms.Label();
            this.pictureBox_FinalStatus = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_FinalStatus = new System.Windows.Forms.TextBox();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FinalStatus)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.button_OK);
            this._bfd_ButtonPanel.Controls.Add(this.button_HideandNotify);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 441);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(474, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_HideandNotify, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.button_OK, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.label_Step1);
            this._bf_MainPanel.Controls.Add(this.pictureBox_Step1);
            this._bf_MainPanel.Controls.Add(this.pictureBox_Step2);
            this._bf_MainPanel.Controls.Add(this.groupBox1);
            this._bf_MainPanel.Controls.Add(this.pictureBox_Step3);
            this._bf_MainPanel.Controls.Add(this.label_Step4);
            this._bf_MainPanel.Controls.Add(this.label_Step2);
            this._bf_MainPanel.Controls.Add(this.pictureBox_Step4);
            this._bf_MainPanel.Controls.Add(this.label_Step6);
            this._bf_MainPanel.Controls.Add(this.pictureBox_Step5);
            this._bf_MainPanel.Controls.Add(this.label_Step5);
            this._bf_MainPanel.Controls.Add(this.label_Step3);
            this._bf_MainPanel.Controls.Add(this.pictureBox_Step6);
            this._bf_MainPanel.Size = new System.Drawing.Size(474, 388);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(474, 53);
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_OK.Location = new System.Drawing.Point(387, 7);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 27);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "&Close";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // timer_Status
            // 
            this.timer_Status.Interval = 500;
            this.timer_Status.Tick += new System.EventHandler(this.timer_Staus_Tick);
            // 
            // pictureBox_Step1
            // 
            this.pictureBox_Step1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Step1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            this.pictureBox_Step1.Location = new System.Drawing.Point(14, 20);
            this.pictureBox_Step1.Name = "pictureBox_Step1";
            this.pictureBox_Step1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Step1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Step1.TabIndex = 4;
            this.pictureBox_Step1.TabStop = false;
            // 
            // label_Step1
            // 
            this.label_Step1.AutoSize = true;
            this.label_Step1.BackColor = System.Drawing.Color.Transparent;
            this.label_Step1.Location = new System.Drawing.Point(52, 30);
            this.label_Step1.Name = "label_Step1";
            this.label_Step1.Size = new System.Drawing.Size(108, 13);
            this.label_Step1.TabIndex = 7;
            this.label_Step1.Text = "Starting collection job";
            // 
            // pictureBox_Step2
            // 
            this.pictureBox_Step2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Step2.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            this.pictureBox_Step2.Location = new System.Drawing.Point(14, 55);
            this.pictureBox_Step2.Name = "pictureBox_Step2";
            this.pictureBox_Step2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Step2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Step2.TabIndex = 6;
            this.pictureBox_Step2.TabStop = false;
            // 
            // label_Step2
            // 
            this.label_Step2.AutoSize = true;
            this.label_Step2.BackColor = System.Drawing.Color.Transparent;
            this.label_Step2.Location = new System.Drawing.Point(52, 65);
            this.label_Step2.Name = "label_Step2";
            this.label_Step2.Size = new System.Drawing.Size(294, 13);
            this.label_Step2.TabIndex = 9;
            this.label_Step2.Text = "Collecting SQL Server environment details from the target server";
            // 
            // pictureBox_Step3
            // 
            this.pictureBox_Step3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Step3.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            this.pictureBox_Step3.Location = new System.Drawing.Point(14, 90);
            this.pictureBox_Step3.Name = "pictureBox_Step3";
            this.pictureBox_Step3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Step3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Step3.TabIndex = 8;
            this.pictureBox_Step3.TabStop = false;
            // 
            // pictureBox_Step4
            // 
            this.pictureBox_Step4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Step4.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            this.pictureBox_Step4.Location = new System.Drawing.Point(14, 125);
            this.pictureBox_Step4.Name = "pictureBox_Step4";
            this.pictureBox_Step4.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Step4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Step4.TabIndex = 10;
            this.pictureBox_Step4.TabStop = false;
            // 
            // label_Step3
            // 
            this.label_Step3.AutoSize = true;
            this.label_Step3.BackColor = System.Drawing.Color.Transparent;
            this.label_Step3.Location = new System.Drawing.Point(52, 100);
            this.label_Step3.Name = "label_Step3";
            this.label_Step3.Size = new System.Drawing.Size(148, 13);
            this.label_Step3.TabIndex = 13;
            this.label_Step3.Text = "Collecting SQL Server objects";
            // 
            // pictureBox_Step5
            // 
            this.pictureBox_Step5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Step5.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            this.pictureBox_Step5.Location = new System.Drawing.Point(14, 159);
            this.pictureBox_Step5.Name = "pictureBox_Step5";
            this.pictureBox_Step5.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Step5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Step5.TabIndex = 12;
            this.pictureBox_Step5.TabStop = false;
            // 
            // label_Step5
            // 
            this.label_Step5.AutoSize = true;
            this.label_Step5.BackColor = System.Drawing.Color.Transparent;
            this.label_Step5.Location = new System.Drawing.Point(52, 169);
            this.label_Step5.Name = "label_Step5";
            this.label_Step5.Size = new System.Drawing.Size(137, 13);
            this.label_Step5.TabIndex = 15;
            this.label_Step5.Text = "Collecting database objects";
            // 
            // pictureBox_Step6
            // 
            this.pictureBox_Step6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Step6.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            this.pictureBox_Step6.Location = new System.Drawing.Point(14, 192);
            this.pictureBox_Step6.Name = "pictureBox_Step6";
            this.pictureBox_Step6.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Step6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Step6.TabIndex = 14;
            this.pictureBox_Step6.TabStop = false;
            // 
            // label_Step6
            // 
            this.label_Step6.AutoSize = true;
            this.label_Step6.BackColor = System.Drawing.Color.Transparent;
            this.label_Step6.Location = new System.Drawing.Point(52, 202);
            this.label_Step6.Name = "label_Step6";
            this.label_Step6.Size = new System.Drawing.Size(159, 13);
            this.label_Step6.TabIndex = 17;
            this.label_Step6.Text = "Updating SQLsecure Repository";
            // 
            // button_HideandNotify
            // 
            this.button_HideandNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_HideandNotify.Location = new System.Drawing.Point(205, 7);
            this.button_HideandNotify.Name = "button_HideandNotify";
            this.button_HideandNotify.Size = new System.Drawing.Size(176, 27);
            this.button_HideandNotify.TabIndex = 18;
            this.button_HideandNotify.Text = "Hide and Notify when Complete";
            this.button_HideandNotify.Click += new System.EventHandler(this.button_HideandNotify_Click);
            // 
            // label_Step4
            // 
            this.label_Step4.AutoSize = true;
            this.label_Step4.BackColor = System.Drawing.Color.Transparent;
            this.label_Step4.Location = new System.Drawing.Point(52, 134);
            this.label_Step4.Name = "label_Step4";
            this.label_Step4.Size = new System.Drawing.Size(185, 13);
            this.label_Step4.TabIndex = 19;
            this.label_Step4.Text = "Collecting Active Directory information";
            // 
            // pictureBox_FinalStatus
            // 
            this.pictureBox_FinalStatus.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_FinalStatus.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._32_SystemOK;
            this.pictureBox_FinalStatus.Location = new System.Drawing.Point(9, 23);
            this.pictureBox_FinalStatus.Name = "pictureBox_FinalStatus";
            this.pictureBox_FinalStatus.Size = new System.Drawing.Size(50, 50);
            this.pictureBox_FinalStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_FinalStatus.TabIndex = 21;
            this.pictureBox_FinalStatus.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.textBox_FinalStatus);
            this.groupBox1.Controls.Add(this.pictureBox_FinalStatus);
            this.groupBox1.Location = new System.Drawing.Point(12, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 146);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Collection Status";
            // 
            // textBox_FinalStatus
            // 
            this.textBox_FinalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_FinalStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_FinalStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox_FinalStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_FinalStatus.Location = new System.Drawing.Point(65, 23);
            this.textBox_FinalStatus.Multiline = true;
            this.textBox_FinalStatus.Name = "textBox_FinalStatus";
            this.textBox_FinalStatus.ReadOnly = true;
            this.textBox_FinalStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_FinalStatus.Size = new System.Drawing.Size(377, 117);
            this.textBox_FinalStatus.TabIndex = 22;
            // 
            // Form_StartSnapshotJobAndShowProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(474, 481);
            this.Description = "Data Collection Progress";
            this.Name = "Form_StartSnapshotJobAndShowProgress";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.camera_49;
            this.Text = "Snapshot Collection";
            this.Load += new System.EventHandler(this.Form_StartSnapshotJobAndShowProgress_Load);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FinalStatus)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton button_OK;
        private System.Windows.Forms.Timer timer_Status;
        private System.Windows.Forms.PictureBox pictureBox_Step1;
        private System.Windows.Forms.Label label_Step1;
        private System.Windows.Forms.PictureBox pictureBox_Step2;
        private System.Windows.Forms.Label label_Step2;
        private System.Windows.Forms.PictureBox pictureBox_Step3;
        private System.Windows.Forms.PictureBox pictureBox_Step4;
        private System.Windows.Forms.Label label_Step3;
        private System.Windows.Forms.PictureBox pictureBox_Step5;
        private System.Windows.Forms.Label label_Step5;
        private System.Windows.Forms.PictureBox pictureBox_Step6;
        private System.Windows.Forms.Label label_Step6;
        private Infragistics.Win.Misc.UltraButton button_HideandNotify;
        private System.Windows.Forms.Label label_Step4;
        private System.Windows.Forms.PictureBox pictureBox_FinalStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_FinalStatus;
    }
}
