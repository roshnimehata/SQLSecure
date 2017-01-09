namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_FilterProperties
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
            this._btn_Help = new Infragistics.Win.Misc.UltraButton();
            this._btn_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._btn_OK = new Infragistics.Win.Misc.UltraButton();
            this._txtbx_Name = new System.Windows.Forms.TextBox();
            this._grpbx_FilterModificationInfo = new System.Windows.Forms.GroupBox();
            this._lbl_LastModifiedByLbl = new System.Windows.Forms.Label();
            this._lbl_LastModifiedBy = new System.Windows.Forms.Label();
            this._lbl_CreatedOn = new System.Windows.Forms.Label();
            this._lbl_LastModifiedOnLbl = new System.Windows.Forms.Label();
            this._lbl_CreatedOnLbl = new System.Windows.Forms.Label();
            this._lbl_LastModifiedOn = new System.Windows.Forms.Label();
            this._lbl_CreatedBy = new System.Windows.Forms.Label();
            this._lbl_CreatedByLbl = new System.Windows.Forms.Label();
            this._txtbx_Description = new System.Windows.Forms.TextBox();
            this._lbl_DescriptionLbl = new System.Windows.Forms.Label();
            this._lbl_NameLbl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.filterSelection1 = new Idera.SQLsecure.UI.Console.Controls.FilterSelection();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this._grpbx_FilterModificationInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._btn_Cancel);
            this._bfd_ButtonPanel.Controls.Add(this._btn_Help);
            this._bfd_ButtonPanel.Controls.Add(this._btn_OK);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 587);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(547, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Cancel, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._grpbx_FilterModificationInfo);
            this._bf_MainPanel.Controls.Add(this._lbl_NameLbl);
            this._bf_MainPanel.Controls.Add(this._lbl_DescriptionLbl);
            this._bf_MainPanel.Controls.Add(this.panel1);
            this._bf_MainPanel.Controls.Add(this._txtbx_Description);
            this._bf_MainPanel.Controls.Add(this._txtbx_Name);
            this._bf_MainPanel.Size = new System.Drawing.Size(547, 534);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(547, 53);
            // 
            // _btn_Help
            // 
            this._btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Help.Location = new System.Drawing.Point(450, 9);
            this._btn_Help.Name = "_btn_Help";
            this._btn_Help.Size = new System.Drawing.Size(75, 23);
            this._btn_Help.TabIndex = 3;
            this._btn_Help.Text = "&Help";
            this._btn_Help.Click += new System.EventHandler(this._btn_Help_Click);
            // 
            // _btn_Cancel
            // 
            this._btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btn_Cancel.Location = new System.Drawing.Point(367, 9);
            this._btn_Cancel.Name = "_btn_Cancel";
            this._btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this._btn_Cancel.TabIndex = 2;
            this._btn_Cancel.Text = "&Cancel";
            // 
            // _btn_OK
            // 
            this._btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btn_OK.Location = new System.Drawing.Point(284, 9);
            this._btn_OK.Name = "_btn_OK";
            this._btn_OK.Size = new System.Drawing.Size(75, 23);
            this._btn_OK.TabIndex = 1;
            this._btn_OK.Text = "&OK";
            this._btn_OK.Click += new System.EventHandler(this._btn_OK_Click);
            // 
            // _txtbx_Name
            // 
            this._txtbx_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtbx_Name.Location = new System.Drawing.Point(83, 5);
            this._txtbx_Name.Name = "_txtbx_Name";
            this._txtbx_Name.Size = new System.Drawing.Size(452, 20);
            this._txtbx_Name.TabIndex = 1;
            this._txtbx_Name.TextChanged += new System.EventHandler(this._txtbx_Name_TextChanged);
            // 
            // _grpbx_FilterModificationInfo
            // 
            this._grpbx_FilterModificationInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_FilterModificationInfo.BackColor = System.Drawing.Color.Transparent;
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_LastModifiedByLbl);
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_LastModifiedBy);
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_CreatedOn);
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_LastModifiedOnLbl);
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_CreatedOnLbl);
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_LastModifiedOn);
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_CreatedBy);
            this._grpbx_FilterModificationInfo.Controls.Add(this._lbl_CreatedByLbl);
            this._grpbx_FilterModificationInfo.Location = new System.Drawing.Point(12, 53);
            this._grpbx_FilterModificationInfo.Name = "_grpbx_FilterModificationInfo";
            this._grpbx_FilterModificationInfo.Size = new System.Drawing.Size(523, 67);
            this._grpbx_FilterModificationInfo.TabIndex = 4;
            this._grpbx_FilterModificationInfo.TabStop = false;
            // 
            // _lbl_LastModifiedByLbl
            // 
            this._lbl_LastModifiedByLbl.AutoSize = true;
            this._lbl_LastModifiedByLbl.Location = new System.Drawing.Point(6, 16);
            this._lbl_LastModifiedByLbl.Name = "_lbl_LastModifiedByLbl";
            this._lbl_LastModifiedByLbl.Size = new System.Drawing.Size(86, 13);
            this._lbl_LastModifiedByLbl.TabIndex = 2;
            this._lbl_LastModifiedByLbl.Text = "Last modified by:";
            // 
            // _lbl_LastModifiedBy
            // 
            this._lbl_LastModifiedBy.AutoEllipsis = true;
            this._lbl_LastModifiedBy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_LastModifiedBy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_LastModifiedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_LastModifiedBy.Location = new System.Drawing.Point(98, 15);
            this._lbl_LastModifiedBy.Name = "_lbl_LastModifiedBy";
            this._lbl_LastModifiedBy.Size = new System.Drawing.Size(190, 18);
            this._lbl_LastModifiedBy.TabIndex = 7;
            this._lbl_LastModifiedBy.Text = "label5";
            // 
            // _lbl_CreatedOn
            // 
            this._lbl_CreatedOn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_CreatedOn.AutoEllipsis = true;
            this._lbl_CreatedOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_CreatedOn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_CreatedOn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_CreatedOn.Location = new System.Drawing.Point(319, 39);
            this._lbl_CreatedOn.Name = "_lbl_CreatedOn";
            this._lbl_CreatedOn.Size = new System.Drawing.Size(198, 18);
            this._lbl_CreatedOn.TabIndex = 12;
            this._lbl_CreatedOn.Text = "label10";
            // 
            // _lbl_LastModifiedOnLbl
            // 
            this._lbl_LastModifiedOnLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_LastModifiedOnLbl.AutoSize = true;
            this._lbl_LastModifiedOnLbl.Location = new System.Drawing.Point(294, 16);
            this._lbl_LastModifiedOnLbl.Name = "_lbl_LastModifiedOnLbl";
            this._lbl_LastModifiedOnLbl.Size = new System.Drawing.Size(19, 13);
            this._lbl_LastModifiedOnLbl.TabIndex = 8;
            this._lbl_LastModifiedOnLbl.Text = "on";
            // 
            // _lbl_CreatedOnLbl
            // 
            this._lbl_CreatedOnLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_CreatedOnLbl.AutoSize = true;
            this._lbl_CreatedOnLbl.Location = new System.Drawing.Point(294, 39);
            this._lbl_CreatedOnLbl.Name = "_lbl_CreatedOnLbl";
            this._lbl_CreatedOnLbl.Size = new System.Drawing.Size(19, 13);
            this._lbl_CreatedOnLbl.TabIndex = 11;
            this._lbl_CreatedOnLbl.Text = "on";
            // 
            // _lbl_LastModifiedOn
            // 
            this._lbl_LastModifiedOn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_LastModifiedOn.AutoEllipsis = true;
            this._lbl_LastModifiedOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_LastModifiedOn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_LastModifiedOn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_LastModifiedOn.Location = new System.Drawing.Point(319, 15);
            this._lbl_LastModifiedOn.Name = "_lbl_LastModifiedOn";
            this._lbl_LastModifiedOn.Size = new System.Drawing.Size(198, 18);
            this._lbl_LastModifiedOn.TabIndex = 9;
            this._lbl_LastModifiedOn.Text = "label9";
            // 
            // _lbl_CreatedBy
            // 
            this._lbl_CreatedBy.AutoEllipsis = true;
            this._lbl_CreatedBy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_CreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_CreatedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_CreatedBy.Location = new System.Drawing.Point(98, 39);
            this._lbl_CreatedBy.Name = "_lbl_CreatedBy";
            this._lbl_CreatedBy.Size = new System.Drawing.Size(190, 18);
            this._lbl_CreatedBy.TabIndex = 10;
            this._lbl_CreatedBy.Text = "label12";
            // 
            // _lbl_CreatedByLbl
            // 
            this._lbl_CreatedByLbl.AutoSize = true;
            this._lbl_CreatedByLbl.Location = new System.Drawing.Point(6, 39);
            this._lbl_CreatedByLbl.Name = "_lbl_CreatedByLbl";
            this._lbl_CreatedByLbl.Size = new System.Drawing.Size(61, 13);
            this._lbl_CreatedByLbl.TabIndex = 3;
            this._lbl_CreatedByLbl.Text = "Created by:";
            // 
            // _txtbx_Description
            // 
            this._txtbx_Description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtbx_Description.Location = new System.Drawing.Point(83, 30);
            this._txtbx_Description.Name = "_txtbx_Description";
            this._txtbx_Description.Size = new System.Drawing.Size(452, 20);
            this._txtbx_Description.TabIndex = 3;
            this._txtbx_Description.TextChanged += new System.EventHandler(this._txtbx_Description_TextChanged);
            // 
            // _lbl_DescriptionLbl
            // 
            this._lbl_DescriptionLbl.AutoSize = true;
            this._lbl_DescriptionLbl.BackColor = System.Drawing.Color.Transparent;
            this._lbl_DescriptionLbl.Location = new System.Drawing.Point(9, 34);
            this._lbl_DescriptionLbl.Name = "_lbl_DescriptionLbl";
            this._lbl_DescriptionLbl.Size = new System.Drawing.Size(63, 13);
            this._lbl_DescriptionLbl.TabIndex = 2;
            this._lbl_DescriptionLbl.Text = "&Description:";
            // 
            // _lbl_NameLbl
            // 
            this._lbl_NameLbl.AutoSize = true;
            this._lbl_NameLbl.BackColor = System.Drawing.Color.Transparent;
            this._lbl_NameLbl.Location = new System.Drawing.Point(9, 9);
            this._lbl_NameLbl.Name = "_lbl_NameLbl";
            this._lbl_NameLbl.Size = new System.Drawing.Size(38, 13);
            this._lbl_NameLbl.TabIndex = 0;
            this._lbl_NameLbl.Text = "&Name:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.filterSelection1);
            this.panel1.Location = new System.Drawing.Point(10, 136);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(525, 392);
            this.panel1.TabIndex = 5;
            // 
            // filterSelection1
            // 
            this.filterSelection1.AutoSize = true;
            this.filterSelection1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterSelection1.Location = new System.Drawing.Point(0, 0);
            this.filterSelection1.Name = "filterSelection1";
            this.filterSelection1.Size = new System.Drawing.Size(525, 392);
            this.filterSelection1.TabIndex = 0;
            this.filterSelection1.Load += new System.EventHandler(this.filterSelection1_Load);
            // 
            // Form_FilterProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 627);
            this.Description = "Manage properties of Collection Filters.";
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "Form_FilterProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Report_DataCollectionFilters_48;
            this.Text = "Filter Rule Properties - ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FilterProperties_FormClosing);
            this.Load += new System.EventHandler(this.Form_FilterProperties_Load);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_FilterProperties_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            this._grpbx_FilterModificationInfo.ResumeLayout(false);
            this._grpbx_FilterModificationInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _btn_Help;
        private Infragistics.Win.Misc.UltraButton _btn_Cancel;
        private Infragistics.Win.Misc.UltraButton _btn_OK;
        private System.Windows.Forms.Label _lbl_CreatedByLbl;
        private System.Windows.Forms.Label _lbl_LastModifiedByLbl;
        private System.Windows.Forms.Label _lbl_DescriptionLbl;
        private System.Windows.Forms.Label _lbl_NameLbl;
        private System.Windows.Forms.Label _lbl_CreatedOn;
        private System.Windows.Forms.Label _lbl_CreatedOnLbl;
        private System.Windows.Forms.Label _lbl_CreatedBy;
        private System.Windows.Forms.Label _lbl_LastModifiedOn;
        private System.Windows.Forms.Label _lbl_LastModifiedOnLbl;
        private System.Windows.Forms.Label _lbl_LastModifiedBy;
        private System.Windows.Forms.TextBox _txtbx_Description;
        private System.Windows.Forms.GroupBox _grpbx_FilterModificationInfo;
        private System.Windows.Forms.TextBox _txtbx_Name;
        private System.Windows.Forms.Panel panel1;
        private Idera.SQLsecure.UI.Console.Controls.FilterSelection filterSelection1;
    }
}