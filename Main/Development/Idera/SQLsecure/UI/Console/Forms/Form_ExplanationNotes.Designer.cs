namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_ExplanationNotes
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
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ExplanationNotes));
            this._grid_ExplanationNotes = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._headerStrip_ExplanationNotes = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_ExplanationNotes = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_ExplanationNotesPrint = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_ExplanationNotesSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._button_OK = new Infragistics.Win.Misc.UltraButton();
            this._button_Help = new Infragistics.Win.Misc.UltraButton();
            this._button_Cancel = new Infragistics.Win.Misc.UltraButton();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._label_Note = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid_ExplanationNotes)).BeginInit();
            this._headerStrip_ExplanationNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._button_OK);
            this._bfd_ButtonPanel.Controls.Add(this._button_Help);
            this._bfd_ButtonPanel.Controls.Add(this._button_Cancel);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Cancel, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_Help, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._button_OK, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._label_Note);
            this._bf_MainPanel.Controls.Add(this._grid_ExplanationNotes);
            this._bf_MainPanel.Controls.Add(this._headerStrip_ExplanationNotes);
            // 
            // _grid_ExplanationNotes
            // 
            appearance61.BackColor = System.Drawing.Color.White;
            appearance61.BorderColor = System.Drawing.Color.Transparent;
            this._grid_ExplanationNotes.DisplayLayout.Appearance = appearance61;
            this._grid_ExplanationNotes.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this._grid_ExplanationNotes.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._grid_ExplanationNotes.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance62.BackColor = System.Drawing.Color.Transparent;
            appearance62.BackColor2 = System.Drawing.Color.Transparent;
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance62.BorderColor = System.Drawing.SystemColors.Window;
            this._grid_ExplanationNotes.DisplayLayout.GroupByBox.Appearance = appearance62;
            appearance63.BackColor = System.Drawing.Color.Transparent;
            appearance63.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance63.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid_ExplanationNotes.DisplayLayout.GroupByBox.BandLabelAppearance = appearance63;
            this._grid_ExplanationNotes.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this._grid_ExplanationNotes.DisplayLayout.GroupByBox.Hidden = true;
            appearance64.BackColor = System.Drawing.Color.Transparent;
            appearance64.BackColor2 = System.Drawing.Color.Transparent;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance64.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid_ExplanationNotes.DisplayLayout.GroupByBox.PromptAppearance = appearance64;
            this._grid_ExplanationNotes.DisplayLayout.MaxColScrollRegions = 1;
            this._grid_ExplanationNotes.DisplayLayout.MaxRowScrollRegions = 1;
            this._grid_ExplanationNotes.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grid_ExplanationNotes.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._grid_ExplanationNotes.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grid_ExplanationNotes.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._grid_ExplanationNotes.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._grid_ExplanationNotes.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this._grid_ExplanationNotes.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
            this._grid_ExplanationNotes.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
            appearance65.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._grid_ExplanationNotes.DisplayLayout.Override.CellAppearance = appearance65;
            this._grid_ExplanationNotes.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.Edit;
            this._grid_ExplanationNotes.DisplayLayout.Override.CellPadding = 0;
            this._grid_ExplanationNotes.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._grid_ExplanationNotes.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._grid_ExplanationNotes.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance66.BackColor = System.Drawing.SystemColors.Control;
            appearance66.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance66.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance66.BorderColor = System.Drawing.SystemColors.Window;
            this._grid_ExplanationNotes.DisplayLayout.Override.GroupByRowAppearance = appearance66;
            appearance67.TextHAlignAsString = "Left";
            this._grid_ExplanationNotes.DisplayLayout.Override.HeaderAppearance = appearance67;
            this._grid_ExplanationNotes.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid_ExplanationNotes.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance68.BorderColor = System.Drawing.Color.LightGray;
            appearance68.TextVAlignAsString = "Middle";
            this._grid_ExplanationNotes.DisplayLayout.Override.RowAppearance = appearance68;
            this._grid_ExplanationNotes.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid_ExplanationNotes.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance69.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid_ExplanationNotes.DisplayLayout.Override.SelectedRowAppearance = appearance69;
            this._grid_ExplanationNotes.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._grid_ExplanationNotes.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid_ExplanationNotes.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid_ExplanationNotes.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid_ExplanationNotes.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance70.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid_ExplanationNotes.DisplayLayout.Override.TemplateAddRowAppearance = appearance70;
            this._grid_ExplanationNotes.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid_ExplanationNotes.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid_ExplanationNotes.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid_ExplanationNotes.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid_ExplanationNotes.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid_ExplanationNotes.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grid_ExplanationNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this._grid_ExplanationNotes.Location = new System.Drawing.Point(0, 19);
            this._grid_ExplanationNotes.MinimumSize = new System.Drawing.Size(250, 100);
            this._grid_ExplanationNotes.Name = "_grid_ExplanationNotes";
            this._grid_ExplanationNotes.Size = new System.Drawing.Size(570, 285);
            this._grid_ExplanationNotes.TabIndex = 119;
            this._grid_ExplanationNotes.Text = "ultraGrid1";
            this._grid_ExplanationNotes.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_ExplanationNotes_InitializeLayout);
            // 
            // _headerStrip_ExplanationNotes
            // 
            this._headerStrip_ExplanationNotes.AutoSize = false;
            this._headerStrip_ExplanationNotes.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._headerStrip_ExplanationNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this._headerStrip_ExplanationNotes.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip_ExplanationNotes.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip_ExplanationNotes.HotTrackEnabled = false;
            this._headerStrip_ExplanationNotes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_ExplanationNotes,
            this._toolStripButton_ExplanationNotesPrint,
            this._toolStripButton_ExplanationNotesSave,
            this.toolStripSeparator1});
            this._headerStrip_ExplanationNotes.Location = new System.Drawing.Point(0, 0);
            this._headerStrip_ExplanationNotes.Name = "_headerStrip_ExplanationNotes";
            this._headerStrip_ExplanationNotes.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this._headerStrip_ExplanationNotes.Size = new System.Drawing.Size(570, 19);
            this._headerStrip_ExplanationNotes.TabIndex = 118;
            // 
            // _label_ExplanationNotes
            // 
            this._label_ExplanationNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_ExplanationNotes.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._label_ExplanationNotes.Name = "_label_ExplanationNotes";
            this._label_ExplanationNotes.Size = new System.Drawing.Size(127, 16);
            this._label_ExplanationNotes.Text = "Server Explanation Notes";
            // 
            // _toolStripButton_ExplanationNotesPrint
            // 
            this._toolStripButton_ExplanationNotesPrint.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ExplanationNotesPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ExplanationNotesPrint.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ExplanationNotesPrint.Image")));
            this._toolStripButton_ExplanationNotesPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ExplanationNotesPrint.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ExplanationNotesPrint.Name = "_toolStripButton_ExplanationNotesPrint";
            this._toolStripButton_ExplanationNotesPrint.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ExplanationNotesPrint.Text = "Print";
            this._toolStripButton_ExplanationNotesPrint.Click += new System.EventHandler(this._toolStripButton_GridPrint_Click);
            // 
            // _toolStripButton_ExplanationNotesSave
            // 
            this._toolStripButton_ExplanationNotesSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._toolStripButton_ExplanationNotesSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._toolStripButton_ExplanationNotesSave.Image = ((System.Drawing.Image)(resources.GetObject("_toolStripButton_ExplanationNotesSave.Image")));
            this._toolStripButton_ExplanationNotesSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._toolStripButton_ExplanationNotesSave.Margin = new System.Windows.Forms.Padding(0);
            this._toolStripButton_ExplanationNotesSave.Name = "_toolStripButton_ExplanationNotesSave";
            this._toolStripButton_ExplanationNotesSave.Size = new System.Drawing.Size(23, 19);
            this._toolStripButton_ExplanationNotesSave.Text = "Save";
            this._toolStripButton_ExplanationNotesSave.Click += new System.EventHandler(this._toolStripButton_GridSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 19);
            // 
            // _button_OK
            // 
            this._button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(315, 9);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 8;
            this._button_OK.Text = "&OK";
            this._button_OK.Click += new System.EventHandler(this._button_OK_Click);
            // 
            // _button_Help
            // 
            this._button_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Help.Location = new System.Drawing.Point(476, 9);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 10;
            this._button_Help.Text = "&Help";
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(395, 9);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 9;
            this._button_Cancel.Text = "&Cancel";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // _ultraGridPrintDocument
            // 
            this._ultraGridPrintDocument.Grid = this._grid_ExplanationNotes;
            this._ultraGridPrintDocument.SaveSettingsFormat = Infragistics.Win.SaveSettingsFormat.Xml;
            this._ultraGridPrintDocument.SettingsKey = "UserPermissions._ultraGridPrintDocument";
            // 
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Document = this._ultraGridPrintDocument;
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // _label_Note
            // 
            this._label_Note.AutoSize = true;
            this._label_Note.BackColor = System.Drawing.Color.Transparent;
            this._label_Note.ForeColor = System.Drawing.Color.Black;
            this._label_Note.Location = new System.Drawing.Point(12, 317);
            this._label_Note.Name = "_label_Note";
            this._label_Note.Size = new System.Drawing.Size(388, 13);
            this._label_Note.TabIndex = 120;
            this._label_Note.Text = "Note: When a security check is explained, the finding status is changed to \"OK\".";
            // 
            // Form_ExplanationNotes
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_Cancel;
            this.ClientSize = new System.Drawing.Size(570, 436);
            this.Description = "Add or edit explanation notes for each server and mark a finding as explained if " +
                "needed.";
            this.Name = "Form_ExplanationNotes";
            this.Text = "Edit Explanation Notes";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_ExplanationNotes_HelpRequested);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._bf_MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid_ExplanationNotes)).EndInit();
            this._headerStrip_ExplanationNotes.ResumeLayout(false);
            this._headerStrip_ExplanationNotes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid _grid_ExplanationNotes;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip _headerStrip_ExplanationNotes;
        private System.Windows.Forms.ToolStripLabel _label_ExplanationNotes;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ExplanationNotesPrint;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ExplanationNotesSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Infragistics.Win.Misc.UltraButton _button_OK;
        private Infragistics.Win.Misc.UltraButton _button_Help;
        private Infragistics.Win.Misc.UltraButton _button_Cancel;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private System.Windows.Forms.Label _label_Note;
    }
}
