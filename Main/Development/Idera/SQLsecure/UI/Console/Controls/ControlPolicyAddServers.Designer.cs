namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class ControlPolicyAddServers
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
            this.groupBoxMainServerSelection = new System.Windows.Forms.GroupBox();
            this.radioButton2016 = new System.Windows.Forms.RadioButton();
            this.radioButton2014 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton2012 = new System.Windows.Forms.RadioButton();
            this.ultraListViewServers = new Infragistics.Win.UltraWinListView.UltraListView();
            this.radioButton2008 = new System.Windows.Forms.RadioButton();
            this.radioButton2005 = new System.Windows.Forms.RadioButton();
            this.radioButton2000 = new System.Windows.Forms.RadioButton();
            this.radioButtonAll = new System.Windows.Forms.RadioButton();
            this.radioButtonManual = new System.Windows.Forms.RadioButton();
            this.radioButtonAzure = new System.Windows.Forms.RadioButton();
            this.groupBoxMainServerSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewServers)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxMainServerSelection
            // 
            this.groupBoxMainServerSelection.Controls.Add(this.radioButtonAzure);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButton2016);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButton2014);
            this.groupBoxMainServerSelection.Controls.Add(this.panel1);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButton2012);
            this.groupBoxMainServerSelection.Controls.Add(this.ultraListViewServers);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButton2008);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButton2005);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButton2000);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButtonAll);
            this.groupBoxMainServerSelection.Controls.Add(this.radioButtonManual);
            this.groupBoxMainServerSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMainServerSelection.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMainServerSelection.Name = "groupBoxMainServerSelection";
            this.groupBoxMainServerSelection.Size = new System.Drawing.Size(634, 443);
            this.groupBoxMainServerSelection.TabIndex = 0;
            this.groupBoxMainServerSelection.TabStop = false;
            this.groupBoxMainServerSelection.Text = "Select SQL Servers to include in this Policy";
            // 
            // radioButton2016
            // 
            this.radioButton2016.AutoSize = true;
            this.radioButton2016.ForeColor = System.Drawing.Color.Navy;
            this.radioButton2016.Location = new System.Drawing.Point(449, 33);
            this.radioButton2016.Name = "radioButton2016";
            this.radioButton2016.Size = new System.Drawing.Size(170, 17);
            this.radioButton2016.TabIndex = 10;
            this.radioButton2016.TabStop = true;
            this.radioButton2016.Text = "All SQL Server 2016 Instances";
            this.radioButton2016.UseVisualStyleBackColor = true;
            // 
            // radioButton2014
            // 
            this.radioButton2014.AutoSize = true;
            this.radioButton2014.ForeColor = System.Drawing.Color.Navy;
            this.radioButton2014.Location = new System.Drawing.Point(223, 81);
            this.radioButton2014.Name = "radioButton2014";
            this.radioButton2014.Size = new System.Drawing.Size(170, 17);
            this.radioButton2014.TabIndex = 9;
            this.radioButton2014.TabStop = true;
            this.radioButton2014.Text = "All SQL Server 2014 Instances";
            this.radioButton2014.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.panel1.Location = new System.Drawing.Point(20, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 2);
            this.panel1.TabIndex = 8;
            // 
            // radioButton2012
            // 
            this.radioButton2012.AutoSize = true;
            this.radioButton2012.ForeColor = System.Drawing.Color.Navy;
            this.radioButton2012.Location = new System.Drawing.Point(223, 58);
            this.radioButton2012.Name = "radioButton2012";
            this.radioButton2012.Size = new System.Drawing.Size(170, 17);
            this.radioButton2012.TabIndex = 5;
            this.radioButton2012.TabStop = true;
            this.radioButton2012.Text = "All SQL Server 2012 Instances";
            this.radioButton2012.UseVisualStyleBackColor = true;
            // 
            // ultraListViewServers
            // 
            this.ultraListViewServers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraListViewServers.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraListViewServers.GroupHeadersVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListViewServers.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.None;
            this.ultraListViewServers.Location = new System.Drawing.Point(32, 148);
            this.ultraListViewServers.MainColumn.Key = "Server";
            this.ultraListViewServers.MainColumn.Text = " Audited SQL Server";
            this.ultraListViewServers.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListViewServers.MainColumn.Width = 400;
            this.ultraListViewServers.Name = "ultraListViewServers";
            this.ultraListViewServers.Size = new System.Drawing.Size(592, 289);
            this.ultraListViewServers.TabIndex = 7;
            this.ultraListViewServers.Text = "ultraListView1";
            this.ultraListViewServers.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListViewServers.ViewSettingsDetails.AllowColumnMoving = false;
            this.ultraListViewServers.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            // 
            // radioButton2008
            // 
            this.radioButton2008.AutoSize = true;
            this.radioButton2008.ForeColor = System.Drawing.Color.Navy;
            this.radioButton2008.Location = new System.Drawing.Point(223, 33);
            this.radioButton2008.Name = "radioButton2008";
            this.radioButton2008.Size = new System.Drawing.Size(170, 17);
            this.radioButton2008.TabIndex = 3;
            this.radioButton2008.TabStop = true;
            this.radioButton2008.Text = "All SQL Server 2008 Instances";
            this.radioButton2008.UseVisualStyleBackColor = true;
            // 
            // radioButton2005
            // 
            this.radioButton2005.AutoSize = true;
            this.radioButton2005.ForeColor = System.Drawing.Color.Navy;
            this.radioButton2005.Location = new System.Drawing.Point(11, 83);
            this.radioButton2005.Name = "radioButton2005";
            this.radioButton2005.Size = new System.Drawing.Size(170, 17);
            this.radioButton2005.TabIndex = 2;
            this.radioButton2005.TabStop = true;
            this.radioButton2005.Text = "All SQL Server 2005 Instances";
            this.radioButton2005.UseVisualStyleBackColor = true;
            // 
            // radioButton2000
            // 
            this.radioButton2000.AutoSize = true;
            this.radioButton2000.ForeColor = System.Drawing.Color.Navy;
            this.radioButton2000.Location = new System.Drawing.Point(11, 58);
            this.radioButton2000.Name = "radioButton2000";
            this.radioButton2000.Size = new System.Drawing.Size(170, 17);
            this.radioButton2000.TabIndex = 1;
            this.radioButton2000.TabStop = true;
            this.radioButton2000.Text = "All SQL Server 2000 Instances";
            this.radioButton2000.UseVisualStyleBackColor = true;
            // 
            // radioButtonAll
            // 
            this.radioButtonAll.AutoSize = true;
            this.radioButtonAll.ForeColor = System.Drawing.Color.Navy;
            this.radioButtonAll.Location = new System.Drawing.Point(11, 33);
            this.radioButtonAll.Name = "radioButtonAll";
            this.radioButtonAll.Size = new System.Drawing.Size(143, 17);
            this.radioButtonAll.TabIndex = 0;
            this.radioButtonAll.TabStop = true;
            this.radioButtonAll.Text = "All SQL Server Instances";
            this.radioButtonAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonManual
            // 
            this.radioButtonManual.AutoSize = true;
            this.radioButtonManual.ForeColor = System.Drawing.Color.Navy;
            this.radioButtonManual.Location = new System.Drawing.Point(11, 121);
            this.radioButtonManual.Name = "radioButtonManual";
            this.radioButtonManual.Size = new System.Drawing.Size(162, 17);
            this.radioButtonManual.TabIndex = 6;
            this.radioButtonManual.TabStop = true;
            this.radioButtonManual.Text = "Select SQL Server Instances";
            this.radioButtonManual.UseVisualStyleBackColor = true;
            this.radioButtonManual.CheckedChanged += new System.EventHandler(this.radioButtonManual_CheckedChanged);
            // 
            // radioButtonAzure
            // 
            this.radioButtonAzure.AutoSize = true;
            this.radioButtonAzure.ForeColor = System.Drawing.Color.Navy;
            this.radioButtonAzure.Location = new System.Drawing.Point(449, 56);
            this.radioButtonAzure.Name = "radioButtonAzure";
            this.radioButtonAzure.Size = new System.Drawing.Size(139, 17);
            this.radioButtonAzure.TabIndex = 11;
            this.radioButtonAzure.TabStop = true;
            this.radioButtonAzure.Text = "All Azure SQL Database";
            this.radioButtonAzure.UseVisualStyleBackColor = true;
            // 
            // ControlPolicyAddServers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBoxMainServerSelection);
            this.Name = "ControlPolicyAddServers";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(640, 449);
            this.groupBoxMainServerSelection.ResumeLayout(false);
            this.groupBoxMainServerSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewServers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMainServerSelection;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListViewServers;
        private System.Windows.Forms.RadioButton radioButtonManual;
        private System.Windows.Forms.RadioButton radioButton2008;
        private System.Windows.Forms.RadioButton radioButton2005;
        private System.Windows.Forms.RadioButton radioButton2000;
        private System.Windows.Forms.RadioButton radioButtonAll;
        private System.Windows.Forms.RadioButton radioButton2012;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton2014;
        private System.Windows.Forms.RadioButton radioButton2016;
        private System.Windows.Forms.RadioButton radioButtonAzure;
    }
}
