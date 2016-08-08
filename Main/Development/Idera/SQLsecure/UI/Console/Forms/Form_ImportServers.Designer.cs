using Infragistics.Win.UltraWinListView;

namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_ImportServers
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
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("colStatus");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ImportServers));
            this.ultraButton_Help = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_OK = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_Cancel = new Infragistics.Win.Misc.UltraButton();
            this.button_Browse = new Infragistics.Win.Misc.UltraButton();
            this.textBox_ServersImportFile = new System.Windows.Forms.TextBox();
            this.ofd_OpenFileToImport = new System.Windows.Forms.OpenFileDialog();
            this.cbRegisterAnyway = new System.Windows.Forms.CheckBox();
            this.lvImportStatus = new Infragistics.Win.UltraWinListView.UltraListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.colServerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colImportStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.cbDeleteCsvFileOnClose = new System.Windows.Forms.CheckBox();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvImportStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Help);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_OK);
            this._bfd_ButtonPanel.Controls.Add(this.ultraButton_Cancel);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 543);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(566, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_OK, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this.ultraButton_Help, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this.cbDeleteCsvFileOnClose);
            this._bf_MainPanel.Controls.Add(this.lvImportStatus);
            this._bf_MainPanel.Controls.Add(this.cbRegisterAnyway);
            this._bf_MainPanel.Controls.Add(this.button_Browse);
            this._bf_MainPanel.Controls.Add(this.textBox_ServersImportFile);
            this._bf_MainPanel.Size = new System.Drawing.Size(566, 490);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(566, 53);
            // 
            // ultraButton_Help
            // 
            this.ultraButton_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_Help.Location = new System.Drawing.Point(479, 6);
            this.ultraButton_Help.Name = "ultraButton_Help";
            this.ultraButton_Help.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Help.TabIndex = 12;
            this.ultraButton_Help.Text = "&Help";
            this.ultraButton_Help.Click += new System.EventHandler(this.ultraButton_Help_Click);
            // 
            // ultraButton_OK
            // 
            this.ultraButton_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_OK.Enabled = false;
            this.ultraButton_OK.Location = new System.Drawing.Point(305, 6);
            this.ultraButton_OK.Name = "ultraButton_OK";
            this.ultraButton_OK.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_OK.TabIndex = 10;
            this.ultraButton_OK.Text = "&Import";
            this.ultraButton_OK.Click += new System.EventHandler(this.ultraButton_OK_Click);
            // 
            // ultraButton_Cancel
            // 
            this.ultraButton_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton_Cancel.Location = new System.Drawing.Point(392, 6);
            this.ultraButton_Cancel.Name = "ultraButton_Cancel";
            this.ultraButton_Cancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Cancel.TabIndex = 11;
            this.ultraButton_Cancel.Text = "&Close";
            this.ultraButton_Cancel.Click += new System.EventHandler(this.ultraButton_Cancel_Click);
            // 
            // button_Browse
            // 
            this.button_Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Browse.Location = new System.Drawing.Point(481, 450);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_Browse.TabIndex = 8;
            this.button_Browse.Text = "&Browse...";
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBox_ServersImportFile
            // 
            this.textBox_ServersImportFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ServersImportFile.Location = new System.Drawing.Point(12, 452);
            this.textBox_ServersImportFile.Name = "textBox_ServersImportFile";
            this.textBox_ServersImportFile.Size = new System.Drawing.Size(463, 20);
            this.textBox_ServersImportFile.TabIndex = 7;
            // 
            // ofd_OpenFileToImport
            // 
            this.ofd_OpenFileToImport.Filter = "Comma Separated Value files|*.csv|All files|*.*";
            // 
            // cbRegisterAnyway
            // 
            this.cbRegisterAnyway.AutoSize = true;
            this.cbRegisterAnyway.Location = new System.Drawing.Point(178, 429);
            this.cbRegisterAnyway.Name = "cbRegisterAnyway";
            this.cbRegisterAnyway.Size = new System.Drawing.Size(227, 17);
            this.cbRegisterAnyway.TabIndex = 9;
            this.cbRegisterAnyway.Text = "Try to register servers even if error occures";
            this.cbRegisterAnyway.UseVisualStyleBackColor = true;
            this.cbRegisterAnyway.Visible = false;
            // 
            // lvImportStatus
            // 
            this.lvImportStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvImportStatus.ItemSettings.DefaultImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.ImportServers_48;
            this.lvImportStatus.ItemSettings.SelectionType = Infragistics.Win.UltraWinListView.SelectionType.Single;
            this.lvImportStatus.ItemSettings.SubItemsVisibleInToolTipByDefault = true;
            this.lvImportStatus.Location = new System.Drawing.Point(12, 3);
            this.lvImportStatus.MainColumn.AllowSizing = Infragistics.Win.DefaultableBoolean.False;
            this.lvImportStatus.MainColumn.AutoSizeMode = Infragistics.Win.UltraWinListView.ColumnAutoSizeMode.None;
            this.lvImportStatus.MainColumn.DataType = typeof(string);
            this.lvImportStatus.MainColumn.ShowSortIndicators = Infragistics.Win.DefaultableBoolean.False;
            this.lvImportStatus.MainColumn.Text = "Server Name";
            this.lvImportStatus.Name = "lvImportStatus";
            this.lvImportStatus.Size = new System.Drawing.Size(544, 420);
            ultraListViewSubItemColumn1.AllowSizing = Infragistics.Win.DefaultableBoolean.True;
            ultraListViewSubItemColumn1.AutoSizeMode = Infragistics.Win.UltraWinListView.ColumnAutoSizeMode.AllItems;
            ultraListViewSubItemColumn1.DataType = typeof(string);
            ultraListViewSubItemColumn1.Key = "colStatus";
            appearance1.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            ultraListViewSubItemColumn1.SubItemAppearance = appearance1;
            ultraListViewSubItemColumn1.SubItemTipStyle = Infragistics.Win.UltraWinListView.SubItemTipStyle.ShowAlways;
            ultraListViewSubItemColumn1.Text = "Import Status";
            ultraListViewSubItemColumn1.VisibleInDetailsView = Infragistics.Win.DefaultableBoolean.True;
            this.lvImportStatus.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1});
            this.lvImportStatus.TabIndex = 10;
            this.lvImportStatus.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.lvImportStatus.ViewSettingsDetails.AllowColumnMoving = false;
            this.lvImportStatus.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            this.lvImportStatus.ViewSettingsDetails.ColumnAutoSizeMode = Infragistics.Win.UltraWinListView.ColumnAutoSizeMode.None;
            this.lvImportStatus.ViewSettingsDetails.FullRowSelect = true;
            this.lvImportStatus.ViewSettingsDetails.ImageList = this.imageList1;
            this.lvImportStatus.ViewSettingsDetails.ImageSize = new System.Drawing.Size(16, 16);
            this.lvImportStatus.ViewSettingsIcons.ImageList = this.imageList1;
            this.lvImportStatus.ItemCheckStateChanged += new Infragistics.Win.UltraWinListView.ItemCheckStateChangedEventHandler(this.lvImportStatus_ItemCheckStateChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Undefined");
            this.imageList1.Images.SetKeyName(1, "Importing");
            this.imageList1.Images.SetKeyName(2, "OK");
            this.imageList1.Images.SetKeyName(3, "Warning");
            this.imageList1.Images.SetKeyName(4, "Error");
            // 
            // colServerName
            // 
            this.colServerName.Text = "Server Name";
            // 
            // colImportStatus
            // 
            this.colImportStatus.Text = "Import Status";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            // 
            // cbDeleteCsvFileOnClose
            // 
            this.cbDeleteCsvFileOnClose.AutoSize = true;
            this.cbDeleteCsvFileOnClose.Checked = true;
            this.cbDeleteCsvFileOnClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDeleteCsvFileOnClose.Location = new System.Drawing.Point(12, 429);
            this.cbDeleteCsvFileOnClose.Name = "cbDeleteCsvFileOnClose";
            this.cbDeleteCsvFileOnClose.Size = new System.Drawing.Size(160, 17);
            this.cbDeleteCsvFileOnClose.TabIndex = 11;
            this.cbDeleteCsvFileOnClose.Text = "Delete .csv file before close.";
            this.cbDeleteCsvFileOnClose.UseVisualStyleBackColor = true;
            // 
            // Form_ImportServers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 583);
            this.Description = "Import SQL Servers from CSV file";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form_ImportServers";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.ImportServers_48;
            this.Text = "Import SQL Servers";
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvImportStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton ultraButton_Help;
        private Infragistics.Win.Misc.UltraButton ultraButton_OK;
        private Infragistics.Win.Misc.UltraButton ultraButton_Cancel;
        private Infragistics.Win.Misc.UltraButton button_Browse;
        private System.Windows.Forms.TextBox textBox_ServersImportFile;
        private System.Windows.Forms.OpenFileDialog ofd_OpenFileToImport;
        private System.Windows.Forms.CheckBox cbRegisterAnyway;
        private Infragistics.Win.UltraWinListView.UltraListView lvImportStatus;
        private System.Windows.Forms.ColumnHeader colServerName;
        private System.Windows.Forms.ColumnHeader colImportStatus;
        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.CheckBox cbDeleteCsvFileOnClose;
    }
}