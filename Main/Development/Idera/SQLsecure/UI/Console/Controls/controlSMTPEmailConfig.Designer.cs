namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class controlSMTPEmailConfig
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.timeoutEditor = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraNumeric_ServerPort = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.timeoutSlider = new System.Windows.Forms.TrackBar();
            this.textbox_ServerAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkbox_RequiresAuthentication = new System.Windows.Forms.CheckBox();
            this.textbox_Password = new System.Windows.Forms.TextBox();
            this.textbox_LogonName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.textbox_FromAddress = new System.Windows.Forms.TextBox();
            this.textbox_FromName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumeric_ServerPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraGroupBox2.Appearance = appearance2;
            this.ultraGroupBox2.Controls.Add(this.timeoutEditor);
            this.ultraGroupBox2.Controls.Add(this.ultraNumeric_ServerPort);
            this.ultraGroupBox2.Controls.Add(this.timeoutLabel);
            this.ultraGroupBox2.Controls.Add(this.label5);
            this.ultraGroupBox2.Controls.Add(this.timeoutSlider);
            this.ultraGroupBox2.Controls.Add(this.textbox_ServerAddress);
            this.ultraGroupBox2.Controls.Add(this.label3);
            this.ultraGroupBox2.Controls.Add(this.label4);
            this.ultraGroupBox2.Location = new System.Drawing.Point(3, 3);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(446, 119);
            this.ultraGroupBox2.TabIndex = 3;
            this.ultraGroupBox2.Text = "Email Server Information";
            this.ultraGroupBox2.UseAppStyling = false;
            // 
            // timeoutEditor
            // 
            this.timeoutEditor.AutoSize = false;
            this.timeoutEditor.Location = new System.Drawing.Point(246, 66);
            this.timeoutEditor.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.timeoutEditor.MaskInput = "nnn";
            this.timeoutEditor.MaxValue = 600;
            this.timeoutEditor.MinValue = 10;
            this.timeoutEditor.Name = "timeoutEditor";
            this.timeoutEditor.Nullable = true;
            this.timeoutEditor.Size = new System.Drawing.Size(54, 21);
            this.timeoutEditor.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.timeoutEditor.SpinWrap = true;
            this.timeoutEditor.TabIndex = 6;
            this.timeoutEditor.UseAppStyling = false;
            this.timeoutEditor.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.timeoutEditor.Value = 90;
            this.timeoutEditor.ValueChanged += new System.EventHandler(this.timeoutEditor_ValueChanged);
            // 
            // ultraNumeric_ServerPort
            // 
            this.ultraNumeric_ServerPort.AutoSize = false;
            this.ultraNumeric_ServerPort.Location = new System.Drawing.Point(84, 44);
            this.ultraNumeric_ServerPort.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.ultraNumeric_ServerPort.MaskInput = "nnnnn";
            this.ultraNumeric_ServerPort.MaxValue = 65530;
            this.ultraNumeric_ServerPort.MinValue = 2;
            this.ultraNumeric_ServerPort.Name = "ultraNumeric_ServerPort";
            this.ultraNumeric_ServerPort.Nullable = true;
            this.ultraNumeric_ServerPort.Size = new System.Drawing.Size(73, 21);
            this.ultraNumeric_ServerPort.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.ultraNumeric_ServerPort.SpinWrap = true;
            this.ultraNumeric_ServerPort.TabIndex = 3;
            this.ultraNumeric_ServerPort.UseAppStyling = false;
            this.ultraNumeric_ServerPort.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // timeoutLabel
            // 
            this.timeoutLabel.Location = new System.Drawing.Point(311, 74);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(119, 13);
            this.timeoutLabel.TabIndex = 7;
            this.timeoutLabel.Text = "1 minute 30 seconds";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Timeout:";
            // 
            // timeoutSlider
            // 
            this.timeoutSlider.BackColor = System.Drawing.Color.White;
            this.timeoutSlider.LargeChange = 10;
            this.timeoutSlider.Location = new System.Drawing.Point(76, 72);
            this.timeoutSlider.Maximum = 600;
            this.timeoutSlider.Minimum = 10;
            this.timeoutSlider.Name = "timeoutSlider";
            this.timeoutSlider.Size = new System.Drawing.Size(160, 45);
            this.timeoutSlider.TabIndex = 5;
            this.timeoutSlider.TickStyle = System.Windows.Forms.TickStyle.None;
            this.timeoutSlider.Value = 90;
            this.timeoutSlider.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // textbox_ServerAddress
            // 
            this.textbox_ServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_ServerAddress.Location = new System.Drawing.Point(85, 19);
            this.textbox_ServerAddress.Name = "textbox_ServerAddress";
            this.textbox_ServerAddress.Size = new System.Drawing.Size(351, 20);
            this.textbox_ServerAddress.TabIndex = 1;
            this.textbox_ServerAddress.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Address:";
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraGroupBox3.Appearance = appearance1;
            this.ultraGroupBox3.Controls.Add(this.checkbox_RequiresAuthentication);
            this.ultraGroupBox3.Controls.Add(this.textbox_Password);
            this.ultraGroupBox3.Controls.Add(this.textbox_LogonName);
            this.ultraGroupBox3.Controls.Add(this.label6);
            this.ultraGroupBox3.Controls.Add(this.label7);
            this.ultraGroupBox3.Location = new System.Drawing.Point(3, 128);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(446, 103);
            this.ultraGroupBox3.TabIndex = 4;
            this.ultraGroupBox3.Text = "Logon Information";
            this.ultraGroupBox3.UseAppStyling = false;
            // 
            // checkbox_RequiresAuthentication
            // 
            this.checkbox_RequiresAuthentication.AutoSize = true;
            this.checkbox_RequiresAuthentication.Location = new System.Drawing.Point(18, 21);
            this.checkbox_RequiresAuthentication.Name = "checkbox_RequiresAuthentication";
            this.checkbox_RequiresAuthentication.Size = new System.Drawing.Size(167, 17);
            this.checkbox_RequiresAuthentication.TabIndex = 0;
            this.checkbox_RequiresAuthentication.Text = "Server requires authentication";
            this.checkbox_RequiresAuthentication.UseVisualStyleBackColor = true;
            this.checkbox_RequiresAuthentication.CheckedChanged += new System.EventHandler(this.requiresAuthentication_CheckedChanged);
            // 
            // textbox_Password
            // 
            this.textbox_Password.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_Password.Location = new System.Drawing.Point(84, 70);
            this.textbox_Password.Name = "textbox_Password";
            this.textbox_Password.Size = new System.Drawing.Size(351, 20);
            this.textbox_Password.TabIndex = 4;
            this.textbox_Password.UseSystemPasswordChar = true;
            this.textbox_Password.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // textbox_LogonName
            // 
            this.textbox_LogonName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_LogonName.Location = new System.Drawing.Point(84, 44);
            this.textbox_LogonName.Name = "textbox_LogonName";
            this.textbox_LogonName.Size = new System.Drawing.Size(351, 20);
            this.textbox_LogonName.TabIndex = 2;
            this.textbox_LogonName.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Password:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "User Name:";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackColor2 = System.Drawing.Color.Transparent;
            this.ultraGroupBox1.Appearance = appearance3;
            this.ultraGroupBox1.Controls.Add(this.textbox_FromAddress);
            this.ultraGroupBox1.Controls.Add(this.textbox_FromName);
            this.ultraGroupBox1.Controls.Add(this.label2);
            this.ultraGroupBox1.Controls.Add(this.label1);
            this.ultraGroupBox1.Location = new System.Drawing.Point(3, 237);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(446, 85);
            this.ultraGroupBox1.TabIndex = 5;
            this.ultraGroupBox1.Text = "Sender Information";
            this.ultraGroupBox1.UseAppStyling = false;
            // 
            // textbox_FromAddress
            // 
            this.textbox_FromAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_FromAddress.Location = new System.Drawing.Point(84, 46);
            this.textbox_FromAddress.Name = "textbox_FromAddress";
            this.textbox_FromAddress.Size = new System.Drawing.Size(351, 20);
            this.textbox_FromAddress.TabIndex = 3;
            this.textbox_FromAddress.Text = "myname@isp.com";
            this.textbox_FromAddress.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // textbox_FromName
            // 
            this.textbox_FromName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textbox_FromName.Location = new System.Drawing.Point(84, 20);
            this.textbox_FromName.Name = "textbox_FromName";
            this.textbox_FromName.Size = new System.Drawing.Size(351, 20);
            this.textbox_FromName.TabIndex = 1;
            this.textbox_FromName.Text = "SQLsecure";
            this.textbox_FromName.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "E-mail:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // controlSMTPEmailConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "controlSMTPEmailConfig";
            this.Size = new System.Drawing.Size(454, 331);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraNumeric_ServerPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor timeoutEditor;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor ultraNumeric_ServerPort;
        private System.Windows.Forms.Label timeoutLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar timeoutSlider;
        private System.Windows.Forms.TextBox textbox_ServerAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.CheckBox checkbox_RequiresAuthentication;
        private System.Windows.Forms.TextBox textbox_Password;
        private System.Windows.Forms.TextBox textbox_LogonName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.TextBox textbox_FromAddress;
        private System.Windows.Forms.TextBox textbox_FromName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
