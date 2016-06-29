namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SelectUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SelectUser));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this._button_OK = new System.Windows.Forms.Button();
            this._button_Help = new System.Windows.Forms.Button();
            this._button_Cancel = new System.Windows.Forms.Button();
            this._button_BrowseDomain = new System.Windows.Forms.Button();
            this._panel_Users = new System.Windows.Forms.Panel();
            this._headerStrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_Server = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_Print = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripButton_GroupBy = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_ColumnChooser = new System.Windows.Forms.ToolStripButton();
            this._grid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._label_Descr = new System.Windows.Forms.Label();
            this._panel_Users.SuspendLayout();
            this._headerStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(256, 297);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 4;
            this._button_OK.Text = "&OK";
            this._button_OK.UseVisualStyleBackColor = true;
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Help
            // 
            this._button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Help.Location = new System.Drawing.Point(418, 297);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 6;
            this._button_Help.Text = "&Help";
            this._button_Help.UseVisualStyleBackColor = true;
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(337, 297);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 5;
            this._button_Cancel.Text = "&Cancel";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _button_BrowseDomain
            // 
            this._button_BrowseDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._button_BrowseDomain.Location = new System.Drawing.Point(20, 297);
            this._button_BrowseDomain.Name = "_button_BrowseDomain";
            this._button_BrowseDomain.Size = new System.Drawing.Size(156, 23);
            this._button_BrowseDomain.TabIndex = 3;
            this._button_BrowseDomain.Text = "Browse Active Directory ...";
            this._button_BrowseDomain.UseVisualStyleBackColor = true;
            this._button_BrowseDomain.Visible = false;
            this._button_BrowseDomain.Click += new System.EventHandler(this._button_BrowseDomain_Click);
            // 
            // _panel_Users
            // 
            this._panel_Users.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panel_Users.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._panel_Users.Controls.Add(this._headerStrip);
            this._panel_Users.Controls.Add(this._grid);
            this._panel_Users.Location = new System.Drawing.Point(20, 24);
            this._panel_Users.Name = "_panel_Users";
            this._panel_Users.Size = new System.Drawing.Size(473, 263);
            this._panel_Users.TabIndex = 15;
            // 
            // _headerStrip
            // 
            this._headerStrip.AutoSize = false;
            this._headerStrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._headerStrip.ForeColor = System.Drawing.Color.Black;
            this._headerStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_Server,
            this._toolStripButton_Print,
            this._toolStripButton_Save,
            this.toolStripSeparator2,
            this._toolStripButton_GroupBy,
            this._toolStripButton_ColumnChooser});
            this._headerStrip.Location = new System.Drawing.Point(0, 0);
            this._headerStrip.Name = "_headerStrip";
            this._headerStrip.Size = new System.Drawing.Size(469, 19);
            this._headerStrip.TabIndex = 0;
            // 
            // _label_Server
            // 
            this._label_Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Server.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._label_Server.Name = "_label_Server";
            this._label_Server.Size = new System.Drawing.Size(83, 16);
            this._label_Server.Text = "Users on Server";
            // 
            // _toolStripButton_Print
            // 
            this._toolStripButton_Print.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_Print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_Print.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_Print.Image")));
            this._toolStripButton_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_Print.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_Print.Name = "_toolStripButton_Print";
            this._toolStripButton_Print.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_Print.Text = "Print";
            this._toolStripButton_Print.Click += new System.EventHandler(this._toolStripButton_GridPrint_Click);
            // 
            // _toolStripButton_Save
            // 
            this._toolStripButton_Save.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_Save.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_Save.Image")));
            this._toolStripButton_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_Save.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_Save.Name = "_toolStripButton_Save";
            this._toolStripButton_Save.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_Save.Text = "Save";
            this._toolStripButton_Save.Click += new System.EventHandler(this._toolStripButton_GridSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 19);
            // 
            // _toolStripButton_GroupBy
            // 
            this._toolStripButton_GroupBy.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_GroupBy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_GroupBy.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_GroupBy.Image")));
            this._toolStripButton_GroupBy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_GroupBy.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_GroupBy.Name = "_toolStripButton_GroupBy";
            this._toolStripButton_GroupBy.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_GroupBy.Text = "Group By Box";
            this._toolStripButton_GroupBy.Click += new System.EventHandler(this._toolStripButton_GridGroupBy_Click);
            // 
            // _toolStripButton_ColumnChooser
            // 
            this._toolStripButton_ColumnChooser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ColumnChooser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ColumnChooser.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ColumnChooser.Image")));
            this._toolStripButton_ColumnChooser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ColumnChooser.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ColumnChooser.Name = "_toolStripButton_ColumnChooser";
            this._toolStripButton_ColumnChooser.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ColumnChooser.Text = "Select Columns";
            this._toolStripButton_ColumnChooser.ToolTipText = "Select Columns";
            this._toolStripButton_ColumnChooser.Click += new System.EventHandler(this._toolStripButton_ColumnChooser_Click);
            // 
            // _grid
            // 
            this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._grid.DisplayLayout.Appearance = appearance1;
            this._grid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this._grid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._grid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this._grid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this._grid.DisplayLayout.MaxColScrollRegions = 1;
            this._grid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._grid.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this._grid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._grid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this._grid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this._grid.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._grid.DisplayLayout.Override.CellAppearance = appearance8;
            this._grid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._grid.DisplayLayout.Override.CellPadding = 0;
            this._grid.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._grid.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._grid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
            this._grid.DisplayLayout.Override.HeaderAppearance = appearance10;
            this._grid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._grid.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.LightGray;
            appearance12.TextVAlign = Infragistics.Win.VAlign.Middle;
            this._grid.DisplayLayout.Override.RowAppearance = appearance12;
            this._grid.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.Highlight;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this._grid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this._grid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grid.Location = new System.Drawing.Point(0, 19);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(469, 241);
            this._grid.TabIndex = 1;
            this._grid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_InitializeLayout);
            this._grid.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this._grid_DoubleClickRow);
            // 
            // _ultraGridPrintDocument
            // 
            this._ultraGridPrintDocument.SaveSettingsFormat = Infragistics.Win.SaveSettingsFormat.Xml;
            this._ultraGridPrintDocument.SettingsKey = "UserPermissions._ultraGridPrintDocument";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // _label_Descr
            // 
            this._label_Descr.Location = new System.Drawing.Point(17, 7);
            this._label_Descr.Name = "_label_Descr";
            this._label_Descr.Size = new System.Drawing.Size(473, 13);
            this._label_Descr.TabIndex = 16;
            this._label_Descr.Text = "Select a Windows Account from the list, or click Browse Active Directory to locat" +
                "e users by name";
            // 
            // Form_SelectUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(513, 333);
            this.Controls.Add(this._label_Descr);
            this.Controls.Add(this._panel_Users);
            this.Controls.Add(this._button_BrowseDomain);
            this.Controls.Add(this._button_OK);
            this.Controls.Add(this._button_Help);
            this.Controls.Add(this._button_Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(446, 200);
            this.Name = "Form_SelectUser";
            this.Text = "Select User";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_SelectUser_HelpRequested);
            this._panel_Users.ResumeLayout(false);
            this._headerStrip.ResumeLayout(false);
            this._headerStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.Button _button_Help;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.Button _button_BrowseDomain;
        private System.Windows.Forms.Panel _panel_Users;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip _headerStrip;
        private System.Windows.Forms.ToolStripLabel _label_Server;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Print;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GroupBy;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ColumnChooser;
        private Infragistics.Win.UltraWinGrid.UltraGrid _grid;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private System.Windows.Forms.Label _label_Descr;
    }
}
