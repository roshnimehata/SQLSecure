namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_CreatePolicy
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
            this.listView_AddServers = new System.Windows.Forms.ListView();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_PolicyName = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.controlConfigurePolicyVulnerabilities1 = new Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_AddServers
            // 
            this.listView_AddServers.CheckBoxes = true;
            this.listView_AddServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_AddServers.Location = new System.Drawing.Point(3, 3);
            this.listView_AddServers.Name = "listView_AddServers";
            this.listView_AddServers.Size = new System.Drawing.Size(570, 493);
            this.listView_AddServers.TabIndex = 0;
            this.listView_AddServers.UseCompatibleStateImageBehavior = false;
            this.listView_AddServers.View = System.Windows.Forms.View.List;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(500, 570);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(415, 570);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "&OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Policy Name";
            // 
            // textBox_PolicyName
            // 
            this.textBox_PolicyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PolicyName.Location = new System.Drawing.Point(83, 13);
            this.textBox_PolicyName.Name = "textBox_PolicyName";
            this.textBox_PolicyName.Size = new System.Drawing.Size(497, 20);
            this.textBox_PolicyName.TabIndex = 5;
            this.textBox_PolicyName.TextChanged += new System.EventHandler(this.textBox_PolicyName_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(5, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 525);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.controlConfigurePolicyVulnerabilities1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 499);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Vulnerabilities";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // controlConfigurePolicyVulnerabilities1
            // 
            this.controlConfigurePolicyVulnerabilities1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlConfigurePolicyVulnerabilities1.Location = new System.Drawing.Point(3, 3);
            this.controlConfigurePolicyVulnerabilities1.Name = "controlConfigurePolicyVulnerabilities1";
            this.controlConfigurePolicyVulnerabilities1.Size = new System.Drawing.Size(570, 493);
            this.controlConfigurePolicyVulnerabilities1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listView_AddServers);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(576, 499);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SQL Servers Instances";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form_CreatePolicy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(592, 602);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textBox_PolicyName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "Form_CreatePolicy";
            this.Text = "Create Policy";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_AddServers;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_PolicyName;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Idera.SQLsecure.UI.Console.Controls.controlConfigurePolicyVulnerabilities controlConfigurePolicyVulnerabilities1;
    }
}
