namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_ImportPolicy
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
            this.openFileDialog_ImportPolicy = new System.Windows.Forms.OpenFileDialog();
            this.listView_IderaPolicies = new System.Windows.Forms.ListView();
            this.col_Name = new System.Windows.Forms.ColumnHeader();
            this.col_Count = new System.Windows.Forms.ColumnHeader();
            this.button_Browse = new Infragistics.Win.Misc.UltraButton();
            this.textBox_PolicyImportFile = new System.Windows.Forms.TextBox();
            this.ultraButton_Cancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_OK = new Infragistics.Win.Misc.UltraButton();
            this.radioButtonTemplate = new System.Windows.Forms.RadioButton();
            this.radioButtonExported = new System.Windows.Forms.RadioButton();
            this.ultraButton_Help = new Infragistics.Win.Misc.UltraButton();
            this.label_Description = new System.Windows.Forms.Label();
            this.groupBox_PolicyName = new System.Windows.Forms.GroupBox();
            this._linkLabel_HelpTemplates = new System.Windows.Forms.LinkLabel();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this.groupBox_PolicyName.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Help);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_OK);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Cancel);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 353);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(602, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Help, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._linkLabel_HelpTemplates);
            this._bf_MainPanel.Controls.Add(this.groupBox_PolicyName);
            this._bf_MainPanel.Controls.Add(this.radioButtonExported);
            this._bf_MainPanel.Controls.Add(this.button_Browse);
            this._bf_MainPanel.Controls.Add(this.radioButtonTemplate);
            this._bf_MainPanel.Controls.Add(this.textBox_PolicyImportFile);
            this._bf_MainPanel.Controls.Add(this.listView_IderaPolicies);
            this._bf_MainPanel.Size = new System.Drawing.Size(602, 300);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(602, 53);
            // 
            // openFileDialog_ImportPolicy
            // 
            this.openFileDialog_ImportPolicy.FileName = "SQLsecurePolicy.xml";
            this.openFileDialog_ImportPolicy.Filter = "SQLsecure Policy files|*.xml|All files|*.*";
            this.openFileDialog_ImportPolicy.Title = "Import SQLsecure Policy Settings";
            // 
            // listView_IderaPolicies
            // 
            this.listView_IderaPolicies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listView_IderaPolicies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_Name,
            this.col_Count});
            this.listView_IderaPolicies.FullRowSelect = true;
            this.listView_IderaPolicies.Location = new System.Drawing.Point(31, 36);
            this.listView_IderaPolicies.MultiSelect = false;
            this.listView_IderaPolicies.Name = "listView_IderaPolicies";
            this.listView_IderaPolicies.Size = new System.Drawing.Size(280, 188);
            this.listView_IderaPolicies.TabIndex = 1;
            this.listView_IderaPolicies.UseCompatibleStateImageBehavior = false;
            this.listView_IderaPolicies.View = System.Windows.Forms.View.Details;
            this.listView_IderaPolicies.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_IderaPolicies_ItemSelectionChanged);
            // 
            // col_Name
            // 
            this.col_Name.Text = "Template";
            this.col_Name.Width = 218;
            // 
            // col_Count
            // 
            this.col_Count.Text = "Checks";
            this.col_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.col_Count.Width = 58;
            // 
            // button_Browse
            // 
            this.button_Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Browse.Location = new System.Drawing.Point(514, 258);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_Browse.TabIndex = 6;
            this.button_Browse.Text = "&Browse...";
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBox_PolicyImportFile
            // 
            this.textBox_PolicyImportFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_PolicyImportFile.Location = new System.Drawing.Point(31, 261);
            this.textBox_PolicyImportFile.Name = "textBox_PolicyImportFile";
            this.textBox_PolicyImportFile.Size = new System.Drawing.Size(475, 20);
            this.textBox_PolicyImportFile.TabIndex = 5;
            // 
            // ultraButton_Cancel
            // 
            this.ultraButton_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton_Cancel.Location = new System.Drawing.Point(426, 9);
            this.ultraButton_Cancel.Name = "ultraButton_Cancel";
            this.ultraButton_Cancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Cancel.TabIndex = 8;
            this.ultraButton_Cancel.Text = "&Cancel";
            // 
            // ultraButton_OK
            // 
            this.ultraButton_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButton_OK.Location = new System.Drawing.Point(339, 9);
            this.ultraButton_OK.Name = "ultraButton_OK";
            this.ultraButton_OK.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_OK.TabIndex = 7;
            this.ultraButton_OK.Text = "&OK";
            this.ultraButton_OK.Click += new System.EventHandler(this._bdf_OKBtn_Click);
            // 
            // radioButtonTemplate
            // 
            this.radioButtonTemplate.AutoSize = true;
            this.radioButtonTemplate.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonTemplate.Location = new System.Drawing.Point(13, 16);
            this.radioButtonTemplate.Name = "radioButtonTemplate";
            this.radioButtonTemplate.Size = new System.Drawing.Size(150, 17);
            this.radioButtonTemplate.TabIndex = 0;
            this.radioButtonTemplate.TabStop = true;
            this.radioButtonTemplate.Text = "Import from policy template";
            this.radioButtonTemplate.UseVisualStyleBackColor = false;
            this.radioButtonTemplate.CheckedChanged += new System.EventHandler(this.radioButtonTemplate_CheckedChanged);
            // 
            // radioButtonExported
            // 
            this.radioButtonExported.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonExported.AutoSize = true;
            this.radioButtonExported.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonExported.Location = new System.Drawing.Point(13, 241);
            this.radioButtonExported.Name = "radioButtonExported";
            this.radioButtonExported.Size = new System.Drawing.Size(178, 17);
            this.radioButtonExported.TabIndex = 4;
            this.radioButtonExported.TabStop = true;
            this.radioButtonExported.Text = "Import previously exported policy";
            this.radioButtonExported.UseVisualStyleBackColor = false;
            this.radioButtonExported.CheckedChanged += new System.EventHandler(this.radioButtonExported_CheckedChanged);
            // 
            // ultraButton_Help
            // 
            this.ultraButton_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_Help.Location = new System.Drawing.Point(513, 9);
            this.ultraButton_Help.Name = "ultraButton_Help";
            this.ultraButton_Help.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Help.TabIndex = 9;
            this.ultraButton_Help.Text = "&Help";
            this.ultraButton_Help.Click += new System.EventHandler(this.ultraButton_Help_Click);
            // 
            // label_Description
            // 
            this.label_Description.BackColor = System.Drawing.Color.Transparent;
            this.label_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Description.Location = new System.Drawing.Point(3, 16);
            this.label_Description.Name = "label_Description";
            this.label_Description.Size = new System.Drawing.Size(265, 173);
            this.label_Description.TabIndex = 3;
            this.label_Description.Text = "Description";
            // 
            // groupBox_PolicyName
            // 
            this.groupBox_PolicyName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_PolicyName.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_PolicyName.Controls.Add(this.label_Description);
            this.groupBox_PolicyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_PolicyName.Location = new System.Drawing.Point(319, 32);
            this.groupBox_PolicyName.Name = "groupBox_PolicyName";
            this.groupBox_PolicyName.Size = new System.Drawing.Size(271, 192);
            this.groupBox_PolicyName.TabIndex = 2;
            this.groupBox_PolicyName.TabStop = false;
            this.groupBox_PolicyName.Text = "Policy Name";
            // 
            // _linkLabel_HelpTemplates
            // 
            this._linkLabel_HelpTemplates.AutoSize = true;
            this._linkLabel_HelpTemplates.BackColor = System.Drawing.Color.Transparent;
            this._linkLabel_HelpTemplates.Location = new System.Drawing.Point(169, 18);
            this._linkLabel_HelpTemplates.Name = "_linkLabel_HelpTemplates";
            this._linkLabel_HelpTemplates.Size = new System.Drawing.Size(70, 13);
            this._linkLabel_HelpTemplates.TabIndex = 12;
            this._linkLabel_HelpTemplates.TabStop = true;
            this._linkLabel_HelpTemplates.Text = "Tell me more.";
            this._linkLabel_HelpTemplates.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkLabel_HelpTemplates_LinkClicked);
            // 
            // Form_ImportPolicy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(602, 393);
            this.Description = "Select which policy you would like to import as a new policy.";
            this.MinimumSize = new System.Drawing.Size(610, 400);
            this.Name = "Form_ImportPolicy";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.import_policy_49;
            this.Text = "Import Policy";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_ImportPolicy_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this.groupBox_PolicyName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_ImportPolicy;
        private Infragistics.Win.Misc.UltraButton button_Browse;
        private System.Windows.Forms.ListView listView_IderaPolicies;
        private System.Windows.Forms.TextBox textBox_PolicyImportFile;
        private Infragistics.Win.Misc.UltraButton ultraButton_OK;
        private Infragistics.Win.Misc.UltraButton ultraButton_Cancel;
        private System.Windows.Forms.RadioButton radioButtonExported;
        private System.Windows.Forms.RadioButton radioButtonTemplate;
        private Infragistics.Win.Misc.UltraButton ultraButton_Help;
        private System.Windows.Forms.Label label_Description;
        private System.Windows.Forms.ColumnHeader col_Name;
        private System.Windows.Forms.ColumnHeader col_Count;
        private System.Windows.Forms.GroupBox groupBox_PolicyName;
        private System.Windows.Forms.LinkLabel _linkLabel_HelpTemplates;
    }
}
