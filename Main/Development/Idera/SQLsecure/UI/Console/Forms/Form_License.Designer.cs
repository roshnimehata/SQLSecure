namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_License
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
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("colServer");
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn2 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("colDaysToExpiration");
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ultraListView_Licenses = new Infragistics.Win.UltraWinListView.UltraListView();
            this.button_Delete = new Infragistics.Win.Misc.UltraButton();
            this.button_Add = new Infragistics.Win.Misc.UltraButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_LicenseType = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_DaysToExpire = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_LicenseExpiration = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_LicensedServers = new System.Windows.Forms.TextBox();
            this.textBox_LicensedFor = new System.Windows.Forms.TextBox();
            this.button_OK = new Infragistics.Win.Misc.UltraButton();
            this._btn_Help = new Infragistics.Win.Misc.UltraButton();
            this._bf_MainPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Licenses)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.groupBox1);
            this._bf_MainPanel.Controls.Add(this.button_OK);
            this._bf_MainPanel.Controls.Add(this._btn_Help);
            this._bf_MainPanel.ForeColor = System.Drawing.Color.Navy;
            this._bf_MainPanel.Size = new System.Drawing.Size(559, 444);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(559, 53);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.ultraListView_Licenses);
            this.groupBox1.Controls.Add(this.button_Delete);
            this.groupBox1.Controls.Add(this.button_Add);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Location = new System.Drawing.Point(14, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(533, 392);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Registered Licenses ";
            // 
            // ultraListView_Licenses
            // 
            this.ultraListView_Licenses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraListView_Licenses.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListView_Licenses.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.Single;
            this.ultraListView_Licenses.Location = new System.Drawing.Point(6, 17);
            this.ultraListView_Licenses.MainColumn.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListView_Licenses.MainColumn.DataType = typeof(string);
            this.ultraListView_Licenses.MainColumn.Key = "colLicenseString";
            this.ultraListView_Licenses.MainColumn.Text = "License String";
            this.ultraListView_Licenses.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListView_Licenses.MainColumn.Width = 280;
            this.ultraListView_Licenses.Name = "ultraListView_Licenses";
            this.ultraListView_Licenses.Size = new System.Drawing.Size(518, 171);
            ultraListViewSubItemColumn1.Key = "colServer";
            ultraListViewSubItemColumn1.Text = "Servers";
            ultraListViewSubItemColumn1.VisiblePositionInDetailsView = 1;
            ultraListViewSubItemColumn1.Width = 60;
            ultraListViewSubItemColumn2.Key = "colDaysToExpiration";
            ultraListViewSubItemColumn2.Text = "Days to Expiration";
            ultraListViewSubItemColumn2.VisiblePositionInDetailsView = 2;
            ultraListViewSubItemColumn2.Width = 160;
            this.ultraListView_Licenses.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1,
            ultraListViewSubItemColumn2});
            this.ultraListView_Licenses.TabIndex = 3;
            this.ultraListView_Licenses.Text = "ultraListView1";
            this.ultraListView_Licenses.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListView_Licenses.ViewSettingsDetails.AllowColumnMoving = false;
            this.ultraListView_Licenses.ViewSettingsDetails.FullRowSelect = true;
            this.ultraListView_Licenses.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.ultraListView_Licenses_ItemSelectionChanged);
            // 
            // button_Delete
            // 
            this.button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Delete.Location = new System.Drawing.Point(87, 356);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(75, 23);
            this.button_Delete.TabIndex = 2;
            this.button_Delete.Text = "&Delete";
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_Add
            // 
            this.button_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Add.Location = new System.Drawing.Point(6, 356);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Add.TabIndex = 1;
            this.button_Add.Text = "&Add";
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox_LicenseType);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.textBox_DaysToExpire);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox_LicenseExpiration);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBox_LicensedServers);
            this.groupBox3.Controls.Add(this.textBox_LicensedFor);
            this.groupBox3.Location = new System.Drawing.Point(6, 194);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(518, 156);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "License Details";
            // 
            // textBox_LicenseType
            // 
            this.textBox_LicenseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_LicenseType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_LicenseType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_LicenseType.Location = new System.Drawing.Point(106, 19);
            this.textBox_LicenseType.Name = "textBox_LicenseType";
            this.textBox_LicenseType.ReadOnly = true;
            this.textBox_LicenseType.Size = new System.Drawing.Size(406, 20);
            this.textBox_LicenseType.TabIndex = 6;
            this.textBox_LicenseType.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Type:";
            // 
            // textBox_DaysToExpire
            // 
            this.textBox_DaysToExpire.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_DaysToExpire.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_DaysToExpire.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_DaysToExpire.Location = new System.Drawing.Point(106, 97);
            this.textBox_DaysToExpire.Name = "textBox_DaysToExpire";
            this.textBox_DaysToExpire.ReadOnly = true;
            this.textBox_DaysToExpire.Size = new System.Drawing.Size(406, 20);
            this.textBox_DaysToExpire.TabIndex = 11;
            this.textBox_DaysToExpire.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(6, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Licensed For:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Navy;
            this.label6.Location = new System.Drawing.Point(6, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Days to Expiration:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Servers:";
            // 
            // textBox_LicenseExpiration
            // 
            this.textBox_LicenseExpiration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_LicenseExpiration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_LicenseExpiration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_LicenseExpiration.Location = new System.Drawing.Point(106, 71);
            this.textBox_LicenseExpiration.Name = "textBox_LicenseExpiration";
            this.textBox_LicenseExpiration.ReadOnly = true;
            this.textBox_LicenseExpiration.Size = new System.Drawing.Size(406, 20);
            this.textBox_LicenseExpiration.TabIndex = 9;
            this.textBox_LicenseExpiration.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(6, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Expires On:";
            // 
            // textBox_LicensedServers
            // 
            this.textBox_LicensedServers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_LicensedServers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_LicensedServers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_LicensedServers.Location = new System.Drawing.Point(106, 45);
            this.textBox_LicensedServers.Name = "textBox_LicensedServers";
            this.textBox_LicensedServers.ReadOnly = true;
            this.textBox_LicensedServers.Size = new System.Drawing.Size(406, 20);
            this.textBox_LicensedServers.TabIndex = 8;
            this.textBox_LicensedServers.TabStop = false;
            // 
            // textBox_LicensedFor
            // 
            this.textBox_LicensedFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_LicensedFor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.textBox_LicensedFor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textBox_LicensedFor.Location = new System.Drawing.Point(106, 123);
            this.textBox_LicensedFor.Name = "textBox_LicensedFor";
            this.textBox_LicensedFor.ReadOnly = true;
            this.textBox_LicensedFor.Size = new System.Drawing.Size(406, 20);
            this.textBox_LicensedFor.TabIndex = 7;
            this.textBox_LicensedFor.TabStop = false;
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_OK.Location = new System.Drawing.Point(388, 409);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "&OK";
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // _btn_Help
            // 
            this._btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Help.Location = new System.Drawing.Point(472, 409);
            this._btn_Help.Name = "_btn_Help";
            this._btn_Help.Size = new System.Drawing.Size(75, 23);
            this._btn_Help.TabIndex = 2;
            this._btn_Help.Text = "&Help";
            this._btn_Help.Click += new System.EventHandler(this._btn_Help_Click);
            // 
            // Form_License
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 497);
            this.Description = "SQLsecure requires a license for each Instance of SQL Server to be audited. Use t" +
                "his page to review, add or delete licenses.";
            this.Name = "Form_License";
            this.Text = "Manage SQLsecure Licenses";
            this.Shown += new System.EventHandler(this.Form_License_Shown);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_License_HelpRequested);
            this._bf_MainPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListView_Licenses)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_LicenseType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_LicenseExpiration;
        private System.Windows.Forms.TextBox textBox_LicensedServers;
        private System.Windows.Forms.TextBox textBox_LicensedFor;
        private System.Windows.Forms.TextBox textBox_DaysToExpire;
        private System.Windows.Forms.Label label6;
        private Infragistics.Win.Misc.UltraButton button_OK;
        private Infragistics.Win.Misc.UltraButton button_Delete;
        private System.Windows.Forms.GroupBox groupBox3;
        private Infragistics.Win.Misc.UltraButton button_Add;
        private Infragistics.Win.Misc.UltraButton _btn_Help;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListView_Licenses;


    }
}