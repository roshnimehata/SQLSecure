namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class ServerPrincipalPermissionsGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerPrincipalPermissionsGrid));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this._toolstrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._tsbtn_Sep = new System.Windows.Forms.ToolStripSeparator();
            this._lbl_PermissionType = new System.Windows.Forms.ToolStripLabel();
            this._tsbtn_Print = new System.Windows.Forms.ToolStripButton();
            this._tsbtn_SaveAs = new System.Windows.Forms.ToolStripButton();
            this._tsbtn_GroupByBox = new System.Windows.Forms.ToolStripButton();
            this._tsbtn_ColumnChooser = new System.Windows.Forms.ToolStripButton();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._lbl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this._ultraGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._mi_ColumnChooser = new System.Windows.Forms.ToolStripMenuItem();
            this._mi_ShowGroupByBox = new System.Windows.Forms.ToolStripMenuItem();
            this._mi_Sep = new System.Windows.Forms.ToolStripSeparator();
            this._mi_SaveToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this._mi_Print = new System.Windows.Forms.ToolStripMenuItem();
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._toolstrip.SuspendLayout();
            this._statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ultraGrid)).BeginInit();
            this._contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolstrip
            // 
            this._toolstrip.AutoSize = false;
            this._toolstrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._toolstrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this._toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolstrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._toolstrip.HotTrackEnabled = false;
            this._toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsbtn_Print,
            this._tsbtn_SaveAs,
            this._tsbtn_Sep,
            this._tsbtn_GroupByBox,
            this._lbl_PermissionType,
            this._tsbtn_ColumnChooser});
            this._toolstrip.Location = new System.Drawing.Point(0, 0);
            this._toolstrip.Name = "_toolstrip";
            this._toolstrip.Size = new System.Drawing.Size(516, 19);
            this._toolstrip.TabIndex = 6;
            this._toolstrip.Text = "headerStrip1";
            // 
            // _tsbtn_Sep
            // 
            this._tsbtn_Sep.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_Sep.Name = "_tsbtn_Sep";
            this._tsbtn_Sep.Size = new System.Drawing.Size(6, 19);
            // 
            // _lbl_PermissionType
            // 
            this._lbl_PermissionType.Name = "_lbl_PermissionType";
            this._lbl_PermissionType.Size = new System.Drawing.Size(84, 16);
            this._lbl_PermissionType.Text = "Permission Type";
            // 
            // _tsbtn_Print
            // 
            this._tsbtn_Print.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_Print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsbtn_Print.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_print;
            this._tsbtn_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_Print.Name = "_tsbtn_Print";
            this._tsbtn_Print.Size = new System.Drawing.Size(23, 16);
            this._tsbtn_Print.Text = "Print";
            this._tsbtn_Print.Click += new System.EventHandler(this._tsbtn_Print_Click);
            // 
            // _tsbtn_SaveAs
            // 
            this._tsbtn_SaveAs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_SaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsbtn_SaveAs.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.save_as;
            this._tsbtn_SaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_SaveAs.Name = "_tsbtn_SaveAs";
            this._tsbtn_SaveAs.Size = new System.Drawing.Size(23, 16);
            this._tsbtn_SaveAs.Text = "Save as Excel file";
            this._tsbtn_SaveAs.Click += new System.EventHandler(this._tsbtn_SaveAs_Click);
            // 
            // _tsbtn_GroupByBox
            // 
            this._tsbtn_GroupByBox.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_GroupByBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsbtn_GroupByBox.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.GroupByBox_16;
            this._tsbtn_GroupByBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_GroupByBox.Name = "_tsbtn_GroupByBox";
            this._tsbtn_GroupByBox.Size = new System.Drawing.Size(23, 16);
            this._tsbtn_GroupByBox.Text = "Show group by box";
            this._tsbtn_GroupByBox.Click += new System.EventHandler(this._tsbtn_GroupByBox_Click);
            // 
            // _tsbtn_ColumnChooser
            // 
            this._tsbtn_ColumnChooser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_ColumnChooser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsbtn_ColumnChooser.Image = ((System.Drawing.Image)(resources.GetObject("_tsbtn_ColumnChooser.Image")));
            this._tsbtn_ColumnChooser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_ColumnChooser.Name = "_tsbtn_ColumnChooser";
            this._tsbtn_ColumnChooser.Size = new System.Drawing.Size(23, 16);
            this._tsbtn_ColumnChooser.Text = "Select Columns";
            this._tsbtn_ColumnChooser.Click += new System.EventHandler(this._tsbtn_ColumnChooser_Click);
            // 
            // _statusStrip
            // 
            this._statusStrip.BackColor = System.Drawing.Color.Transparent;
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._progressBar,
            this._lbl_Status});
            this._statusStrip.Location = new System.Drawing.Point(0, 325);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(516, 22);
            this._statusStrip.SizingGrip = false;
            this._statusStrip.TabIndex = 8;
            this._statusStrip.Text = "_statusStrip";
            // 
            // _progressBar
            // 
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // _lbl_Status
            // 
            this._lbl_Status.Name = "_lbl_Status";
            this._lbl_Status.Size = new System.Drawing.Size(38, 17);
            this._lbl_Status.Text = "Status";
            // 
            // _ultraGrid
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._ultraGrid.DisplayLayout.Appearance = appearance1;
            this._ultraGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this._ultraGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this._ultraGrid.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this._ultraGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this._ultraGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this._ultraGrid.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this._ultraGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this._ultraGrid.DisplayLayout.MaxColScrollRegions = 1;
            this._ultraGrid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraGrid.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            this._ultraGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._ultraGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this._ultraGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this._ultraGrid.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._ultraGrid.DisplayLayout.Override.CellAppearance = appearance8;
            this._ultraGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._ultraGrid.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this._ultraGrid.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this._ultraGrid.DisplayLayout.Override.HeaderAppearance = appearance10;
            this._ultraGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._ultraGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._ultraGrid.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this._ultraGrid.DisplayLayout.Override.RowAppearance = appearance12;
            this._ultraGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this._ultraGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this._ultraGrid.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this._ultraGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._ultraGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._ultraGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._ultraGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ultraGrid.Location = new System.Drawing.Point(0, 19);
            this._ultraGrid.Name = "_ultraGrid";
            this._ultraGrid.Size = new System.Drawing.Size(516, 306);
            this._ultraGrid.TabIndex = 9;
            this._ultraGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._ultraGrid_InitializeLayout);
            // 
            // _contextMenu
            // 
            this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mi_ColumnChooser,
            this._mi_ShowGroupByBox,
            this._mi_Sep,
            this._mi_SaveToExcel,
            this._mi_Print});
            this._contextMenu.Name = "_contextMenu";
            this._contextMenu.Size = new System.Drawing.Size(179, 120);
            // 
            // _mi_ColumnChooser
            // 
            this._mi_ColumnChooser.Name = "_mi_ColumnChooser";
            this._mi_ColumnChooser.Size = new System.Drawing.Size(178, 22);
            this._mi_ColumnChooser.Text = "Select columns";
            this._mi_ColumnChooser.Click += new System.EventHandler(this._mi_ColumnChooser_Click);
            // 
            // _mi_ShowGroupByBox
            // 
            this._mi_ShowGroupByBox.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.GroupByBox_16;
            this._mi_ShowGroupByBox.Name = "_mi_ShowGroupByBox";
            this._mi_ShowGroupByBox.Size = new System.Drawing.Size(178, 22);
            this._mi_ShowGroupByBox.Text = "Show group by box";
            this._mi_ShowGroupByBox.Click += new System.EventHandler(this._mi_ShowGroupByBox_Click);
            // 
            // _mi_Sep
            // 
            this._mi_Sep.Name = "_mi_Sep";
            this._mi_Sep.Size = new System.Drawing.Size(175, 6);
            // 
            // _mi_SaveToExcel
            // 
            this._mi_SaveToExcel.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.save_as;
            this._mi_SaveToExcel.Name = "_mi_SaveToExcel";
            this._mi_SaveToExcel.Size = new System.Drawing.Size(178, 22);
            this._mi_SaveToExcel.Text = "Save as Excel file";
            this._mi_SaveToExcel.Click += new System.EventHandler(this._mi_SaveToExcel_Click);
            // 
            // _mi_Print
            // 
            this._mi_Print.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources._16_print;
            this._mi_Print.Name = "_mi_Print";
            this._mi_Print.Size = new System.Drawing.Size(178, 22);
            this._mi_Print.Text = "Print";
            this._mi_Print.Click += new System.EventHandler(this._mi_Print_Click);
            // 
            // _ultraGridPrintDocument
            // 
            this._ultraGridPrintDocument.DocumentName = "Object Permissions";
            this._ultraGridPrintDocument.Grid = this._ultraGrid;
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Document = this._ultraGridPrintDocument;
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // ServerPrincipalPermissionsGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._ultraGrid);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._toolstrip);
            this.Name = "ServerPrincipalPermissionsGrid";
            this.Size = new System.Drawing.Size(516, 347);
            this._toolstrip.ResumeLayout(false);
            this._toolstrip.PerformLayout();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ultraGrid)).EndInit();
            this._contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HeaderStrip _toolstrip;
        private System.Windows.Forms.ToolStripButton _tsbtn_Print;
        private System.Windows.Forms.ToolStripButton _tsbtn_SaveAs;
        private System.Windows.Forms.ToolStripSeparator _tsbtn_Sep;
        private System.Windows.Forms.ToolStripButton _tsbtn_GroupByBox;
        private System.Windows.Forms.ToolStripLabel _lbl_PermissionType;
        private System.Windows.Forms.ToolStripButton _tsbtn_ColumnChooser;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripProgressBar _progressBar;
        private System.Windows.Forms.ToolStripStatusLabel _lbl_Status;
        private Infragistics.Win.UltraWinGrid.UltraGrid _ultraGrid;
        private System.Windows.Forms.ContextMenuStrip _contextMenu;
        private System.Windows.Forms.ToolStripMenuItem _mi_ColumnChooser;
        private System.Windows.Forms.ToolStripMenuItem _mi_ShowGroupByBox;
        private System.Windows.Forms.ToolStripSeparator _mi_Sep;
        private System.Windows.Forms.ToolStripMenuItem _mi_SaveToExcel;
        private System.Windows.Forms.ToolStripMenuItem _mi_Print;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
    }
}
