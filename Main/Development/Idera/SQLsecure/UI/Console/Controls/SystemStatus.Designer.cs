namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class SystemStatus
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
            this.components = new System.ComponentModel.Container();
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._ultraToolTipManager = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this._viewSection_Status = new Idera.SQLsecure.UI.Console.Controls.ViewSection();
            this._panel_Divider = new System.Windows.Forms.Panel();
            this._linkLabel_ServersStatus = new System.Windows.Forms.LinkLabel();
            this._lbl_LastGroomed = new System.Windows.Forms.Label();
            this._linkLabel_LicenseStatus = new System.Windows.Forms.LinkLabel();
            this._lbl_SVRS = new System.Windows.Forms.Label();
            this._pictureBox_ServerStatus = new System.Windows.Forms.PictureBox();
            this._pictureBox_Servers = new System.Windows.Forms.PictureBox();
            this._linkLabel_ServerStatus = new System.Windows.Forms.LinkLabel();
            this._linkLabel_AgentStatus = new System.Windows.Forms.LinkLabel();
            this._lbl_LIC2 = new System.Windows.Forms.Label();
            this._lbl_SVR = new System.Windows.Forms.Label();
            this._pictureBox_License = new System.Windows.Forms.PictureBox();
            this._lbl_GRM = new System.Windows.Forms.Label();
            this._lbl_AGT = new System.Windows.Forms.Label();
            this._pictureBox_Agent = new System.Windows.Forms.PictureBox();
            this._lbl_Repository = new System.Windows.Forms.Label();
            this._lbl_LIC = new System.Windows.Forms.Label();
            this._lbl_SIZ = new System.Windows.Forms.Label();
            this._lbl_License = new System.Windows.Forms.Label();
            this._lbl_Size = new System.Windows.Forms.Label();
            this._viewSection_Status.ViewPanel.SuspendLayout();
            this._viewSection_Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_ServerStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_Servers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_License)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_Agent)).BeginInit();
            this.SuspendLayout();
            // 
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // _ultraGridPrintDocument
            // 
            this._ultraGridPrintDocument.SaveSettingsFormat = Infragistics.Win.SaveSettingsFormat.Xml;
            this._ultraGridPrintDocument.SettingsKey = "UserPermissions._ultraGridPrintDocument";
            // 
            // _ultraToolTipManager
            // 
            this._ultraToolTipManager.AutoPopDelay = 15000;
            this._ultraToolTipManager.ContainingControl = this;
            // 
            // _viewSection_Status
            // 
            this._viewSection_Status.BackColor = System.Drawing.Color.White;
            this._viewSection_Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_Status.HeaderGradientBorderStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DOut;
            this._viewSection_Status.HeaderGradientColor = System.Drawing.Color.DarkGray;
            this._viewSection_Status.HeaderGradientCornerStyle = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundTop;
            this._viewSection_Status.HeaderTextColor = System.Drawing.SystemColors.ControlText;
            this._viewSection_Status.Location = new System.Drawing.Point(0, 0);
            this._viewSection_Status.Name = "_viewSection_Status";
            this._viewSection_Status.Size = new System.Drawing.Size(646, 205);
            this._viewSection_Status.TabIndex = 17;
            this._viewSection_Status.Title = "System Status";
            // 
            // _viewSection_Status.Panel
            // 
            this._viewSection_Status.ViewPanel.BackColor = System.Drawing.Color.Transparent;
            this._viewSection_Status.ViewPanel.Controls.Add(this._panel_Divider);
            this._viewSection_Status.ViewPanel.Controls.Add(this._linkLabel_ServersStatus);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_LastGroomed);
            this._viewSection_Status.ViewPanel.Controls.Add(this._linkLabel_LicenseStatus);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_SVRS);
            this._viewSection_Status.ViewPanel.Controls.Add(this._pictureBox_ServerStatus);
            this._viewSection_Status.ViewPanel.Controls.Add(this._pictureBox_Servers);
            this._viewSection_Status.ViewPanel.Controls.Add(this._linkLabel_ServerStatus);
            this._viewSection_Status.ViewPanel.Controls.Add(this._linkLabel_AgentStatus);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_LIC2);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_SVR);
            this._viewSection_Status.ViewPanel.Controls.Add(this._pictureBox_License);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_GRM);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_AGT);
            this._viewSection_Status.ViewPanel.Controls.Add(this._pictureBox_Agent);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_Repository);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_LIC);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_SIZ);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_License);
            this._viewSection_Status.ViewPanel.Controls.Add(this._lbl_Size);
            this._viewSection_Status.ViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._viewSection_Status.ViewPanel.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._viewSection_Status.ViewPanel.Location = new System.Drawing.Point(0, 20);
            this._viewSection_Status.ViewPanel.Name = "Panel";
            this._viewSection_Status.ViewPanel.Rotation = 270F;
            this._viewSection_Status.ViewPanel.Size = new System.Drawing.Size(646, 185);
            this._viewSection_Status.ViewPanel.TabIndex = 1;
            // 
            // _panel_Divider
            // 
            this._panel_Divider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._panel_Divider.Location = new System.Drawing.Point(10, 365);
            this._panel_Divider.Name = "_panel_Divider";
            this._panel_Divider.Size = new System.Drawing.Size(628, 4);
            this._panel_Divider.TabIndex = 43;
            // 
            // _linkLabel_ServersStatus
            // 
            this._linkLabel_ServersStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._linkLabel_ServersStatus.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this._linkLabel_ServersStatus.Location = new System.Drawing.Point(149, 331);
            this._linkLabel_ServersStatus.Name = "_linkLabel_ServersStatus";
            this._linkLabel_ServersStatus.Size = new System.Drawing.Size(490, 27);
            this._linkLabel_ServersStatus.TabIndex = 42;
            this._linkLabel_ServersStatus.Text = "Status unknown";
            this._linkLabel_ServersStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._linkLabel_ServersStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkLabel_ServersStatus_LinkClicked);
            // 
            // _lbl_LastGroomed
            // 
            this._lbl_LastGroomed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_LastGroomed.BackColor = System.Drawing.Color.Transparent;
            this._lbl_LastGroomed.Location = new System.Drawing.Point(119, 150);
            this._lbl_LastGroomed.Name = "_lbl_LastGroomed";
            this._lbl_LastGroomed.Size = new System.Drawing.Size(520, 15);
            this._lbl_LastGroomed.TabIndex = 17;
            this._lbl_LastGroomed.Text = "Groomed";
            this._lbl_LastGroomed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _linkLabel_LicenseStatus
            // 
            this._linkLabel_LicenseStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._linkLabel_LicenseStatus.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this._linkLabel_LicenseStatus.Location = new System.Drawing.Point(149, 303);
            this._linkLabel_LicenseStatus.Name = "_linkLabel_LicenseStatus";
            this._linkLabel_LicenseStatus.Size = new System.Drawing.Size(490, 27);
            this._linkLabel_LicenseStatus.TabIndex = 40;
            this._linkLabel_LicenseStatus.Text = "Status unknown";
            this._linkLabel_LicenseStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._linkLabel_LicenseStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkLabel_LicenseStatus_LinkClicked);
            // 
            // _lbl_SVRS
            // 
            this._lbl_SVRS.AutoSize = true;
            this._lbl_SVRS.BackColor = System.Drawing.Color.Transparent;
            this._lbl_SVRS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_SVRS.Location = new System.Drawing.Point(9, 339);
            this._lbl_SVRS.Name = "_lbl_SVRS";
            this._lbl_SVRS.Size = new System.Drawing.Size(101, 13);
            this._lbl_SVRS.TabIndex = 39;
            this._lbl_SVRS.Text = "Audited Servers:";
            this._lbl_SVRS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pictureBox_ServerStatus
            // 
            this._pictureBox_ServerStatus.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox_ServerStatus.Location = new System.Drawing.Point(11, 16);
            this._pictureBox_ServerStatus.Name = "_pictureBox_ServerStatus";
            this._pictureBox_ServerStatus.Size = new System.Drawing.Size(32, 32);
            this._pictureBox_ServerStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBox_ServerStatus.TabIndex = 0;
            this._pictureBox_ServerStatus.TabStop = false;
            // 
            // _pictureBox_Servers
            // 
            this._pictureBox_Servers.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox_Servers.Location = new System.Drawing.Point(125, 333);
            this._pictureBox_Servers.Name = "_pictureBox_Servers";
            this._pictureBox_Servers.Size = new System.Drawing.Size(22, 22);
            this._pictureBox_Servers.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBox_Servers.TabIndex = 37;
            this._pictureBox_Servers.TabStop = false;
            // 
            // _linkLabel_ServerStatus
            // 
            this._linkLabel_ServerStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._linkLabel_ServerStatus.BackColor = System.Drawing.Color.Transparent;
            this._linkLabel_ServerStatus.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this._linkLabel_ServerStatus.Location = new System.Drawing.Point(49, 8);
            this._linkLabel_ServerStatus.Name = "_linkLabel_ServerStatus";
            this._linkLabel_ServerStatus.Size = new System.Drawing.Size(590, 52);
            this._linkLabel_ServerStatus.TabIndex = 0;
            this._linkLabel_ServerStatus.Text = "Repository Status";
            this._linkLabel_ServerStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._linkLabel_ServerStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lbl_Status_LinkClicked);
            // 
            // _linkLabel_AgentStatus
            // 
            this._linkLabel_AgentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._linkLabel_AgentStatus.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this._linkLabel_AgentStatus.Location = new System.Drawing.Point(149, 275);
            this._linkLabel_AgentStatus.Name = "_linkLabel_AgentStatus";
            this._linkLabel_AgentStatus.Size = new System.Drawing.Size(490, 27);
            this._linkLabel_AgentStatus.TabIndex = 36;
            this._linkLabel_AgentStatus.Text = "Status unknown";
            this._linkLabel_AgentStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._linkLabel_AgentStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkLabel_AgentStatus_LinkClicked);
            // 
            // _lbl_LIC2
            // 
            this._lbl_LIC2.AutoSize = true;
            this._lbl_LIC2.BackColor = System.Drawing.Color.Transparent;
            this._lbl_LIC2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_LIC2.Location = new System.Drawing.Point(9, 311);
            this._lbl_LIC2.Name = "_lbl_LIC2";
            this._lbl_LIC2.Size = new System.Drawing.Size(95, 13);
            this._lbl_LIC2.TabIndex = 35;
            this._lbl_LIC2.Text = "License Status:";
            this._lbl_LIC2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_SVR
            // 
            this._lbl_SVR.AutoSize = true;
            this._lbl_SVR.BackColor = System.Drawing.Color.Transparent;
            this._lbl_SVR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_SVR.Location = new System.Drawing.Point(8, 60);
            this._lbl_SVR.Name = "_lbl_SVR";
            this._lbl_SVR.Size = new System.Drawing.Size(71, 13);
            this._lbl_SVR.TabIndex = 8;
            this._lbl_SVR.Text = "Repository:";
            this._lbl_SVR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pictureBox_License
            // 
            this._pictureBox_License.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox_License.Location = new System.Drawing.Point(125, 305);
            this._pictureBox_License.Name = "_pictureBox_License";
            this._pictureBox_License.Size = new System.Drawing.Size(22, 22);
            this._pictureBox_License.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBox_License.TabIndex = 33;
            this._pictureBox_License.TabStop = false;
            // 
            // _lbl_GRM
            // 
            this._lbl_GRM.AutoSize = true;
            this._lbl_GRM.BackColor = System.Drawing.Color.Transparent;
            this._lbl_GRM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_GRM.Location = new System.Drawing.Point(8, 151);
            this._lbl_GRM.Name = "_lbl_GRM";
            this._lbl_GRM.Size = new System.Drawing.Size(89, 13);
            this._lbl_GRM.TabIndex = 16;
            this._lbl_GRM.Text = "Last Groomed:";
            this._lbl_GRM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_AGT
            // 
            this._lbl_AGT.AutoSize = true;
            this._lbl_AGT.BackColor = System.Drawing.Color.Transparent;
            this._lbl_AGT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_AGT.Location = new System.Drawing.Point(9, 283);
            this._lbl_AGT.Name = "_lbl_AGT";
            this._lbl_AGT.Size = new System.Drawing.Size(113, 13);
            this._lbl_AGT.TabIndex = 28;
            this._lbl_AGT.Text = "SQL Server Agent:";
            this._lbl_AGT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pictureBox_Agent
            // 
            this._pictureBox_Agent.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox_Agent.Location = new System.Drawing.Point(125, 277);
            this._pictureBox_Agent.Name = "_pictureBox_Agent";
            this._pictureBox_Agent.Size = new System.Drawing.Size(22, 22);
            this._pictureBox_Agent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._pictureBox_Agent.TabIndex = 26;
            this._pictureBox_Agent.TabStop = false;
            // 
            // _lbl_Repository
            // 
            this._lbl_Repository.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Repository.BackColor = System.Drawing.Color.Transparent;
            this._lbl_Repository.Location = new System.Drawing.Point(119, 60);
            this._lbl_Repository.Name = "_lbl_Repository";
            this._lbl_Repository.Size = new System.Drawing.Size(523, 34);
            this._lbl_Repository.TabIndex = 11;
            this._lbl_Repository.Text = "Server name\\r\\nversion";
            // 
            // _lbl_LIC
            // 
            this._lbl_LIC.AutoSize = true;
            this._lbl_LIC.BackColor = System.Drawing.Color.Transparent;
            this._lbl_LIC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_LIC.Location = new System.Drawing.Point(8, 95);
            this._lbl_LIC.Name = "_lbl_LIC";
            this._lbl_LIC.Size = new System.Drawing.Size(55, 13);
            this._lbl_LIC.TabIndex = 20;
            this._lbl_LIC.Text = "License:";
            this._lbl_LIC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_SIZ
            // 
            this._lbl_SIZ.AutoSize = true;
            this._lbl_SIZ.BackColor = System.Drawing.Color.Transparent;
            this._lbl_SIZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_SIZ.Location = new System.Drawing.Point(8, 123);
            this._lbl_SIZ.Name = "_lbl_SIZ";
            this._lbl_SIZ.Size = new System.Drawing.Size(82, 13);
            this._lbl_SIZ.TabIndex = 18;
            this._lbl_SIZ.Text = "Size on Disk:";
            this._lbl_SIZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_License
            // 
            this._lbl_License.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_License.BackColor = System.Drawing.Color.Transparent;
            this._lbl_License.Location = new System.Drawing.Point(119, 94);
            this._lbl_License.Name = "_lbl_License";
            this._lbl_License.Size = new System.Drawing.Size(522, 15);
            this._lbl_License.TabIndex = 21;
            this._lbl_License.Text = "License Info";
            this._lbl_License.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_Size
            // 
            this._lbl_Size.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Size.BackColor = System.Drawing.Color.Transparent;
            this._lbl_Size.Location = new System.Drawing.Point(119, 122);
            this._lbl_Size.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this._lbl_Size.Name = "_lbl_Size";
            this._lbl_Size.Size = new System.Drawing.Size(522, 15);
            this._lbl_Size.TabIndex = 19;
            this._lbl_Size.Text = "0 KB";
            this._lbl_Size.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SystemStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._viewSection_Status);
            this.Name = "SystemStatus";
            this.Size = new System.Drawing.Size(646, 205);
            this._viewSection_Status.ViewPanel.ResumeLayout(false);
            this._viewSection_Status.ViewPanel.PerformLayout();
            this._viewSection_Status.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_ServerStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_Servers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_License)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_Agent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _pictureBox_ServerStatus;
        private System.Windows.Forms.LinkLabel _linkLabel_ServerStatus;
        private ViewSection _viewSection_Status;
        private System.Windows.Forms.Label _lbl_SVR;
        private System.Windows.Forms.Label _lbl_Repository;
        private System.Windows.Forms.Label _lbl_GRM;
        private System.Windows.Forms.Label _lbl_LastGroomed;
        private System.Windows.Forms.Label _lbl_SIZ;
        private System.Windows.Forms.Label _lbl_Size;
        private System.Windows.Forms.Label _lbl_LIC;
        private System.Windows.Forms.Label _lbl_License;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private System.Windows.Forms.PictureBox _pictureBox_Agent;
        private System.Windows.Forms.Label _lbl_AGT;
        private System.Windows.Forms.Label _lbl_LIC2;
        private System.Windows.Forms.PictureBox _pictureBox_License;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager _ultraToolTipManager;
        private System.Windows.Forms.LinkLabel _linkLabel_AgentStatus;
        private System.Windows.Forms.Label _lbl_SVRS;
        private System.Windows.Forms.PictureBox _pictureBox_Servers;
        private System.Windows.Forms.LinkLabel _linkLabel_LicenseStatus;
        private System.Windows.Forms.LinkLabel _linkLabel_ServersStatus;
        private System.Windows.Forms.Panel _panel_Divider;





    }
}
