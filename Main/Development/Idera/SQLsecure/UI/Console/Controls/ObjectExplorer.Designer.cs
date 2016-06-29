namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class ObjectExplorer
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObjectExplorer));
            this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._treeview = new System.Windows.Forms.TreeView();
            this._cntxtMenu_Tree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._mi_Tree_Properties = new System.Windows.Forms.ToolStripMenuItem();
            this._ultraGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._cntxtMenu_Grid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._mi_Grid_Properties = new System.Windows.Forms.ToolStripMenuItem();
            this._toolHdr = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._tsbtn_Up = new System.Windows.Forms.ToolStripButton();
            this._tslbl_NodeName = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_Print = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_Save = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_GroupBy = new System.Windows.Forms.ToolStripButton();
            this._tsbtn_ColumnChooser = new System.Windows.Forms.ToolStripButton();
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._pnl_Header = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._lbl_Instructions = new System.Windows.Forms.Label();
            this._pictureBox_Snapshot = new System.Windows.Forms.PictureBox();
            this._lnklbl_Snapshot = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._cntxtMenu_Tree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ultraGrid)).BeginInit();
            this._cntxtMenu_Grid.SuspendLayout();
            this._toolHdr.SuspendLayout();
            this._pnl_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_Snapshot)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabControl1
            // 
            this.ultraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl1.Name = "ultraTabControl1";
            this.ultraTabControl1.SharedControlsPage = null;
            this.ultraTabControl1.Size = new System.Drawing.Size(200, 100);
            this.ultraTabControl1.TabIndex = 0;
            // 
            // _splitContainer
            // 
            this._splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(0, 34);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._treeview);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._ultraGrid);
            this._splitContainer.Panel2.Controls.Add(this._toolHdr);
            this._splitContainer.Size = new System.Drawing.Size(644, 428);
            this._splitContainer.SplitterDistance = 199;
            this._splitContainer.TabIndex = 1;
            this._splitContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this._splitContainer_MouseDown);
            this._splitContainer.MouseUp += new System.Windows.Forms.MouseEventHandler(this._splitContainer_MouseUp);
            // 
            // _treeview
            // 
            this._treeview.BackColor = System.Drawing.Color.White;
            this._treeview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._treeview.ContextMenuStrip = this._cntxtMenu_Tree;
            this._treeview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeview.FullRowSelect = true;
            this._treeview.HideSelection = false;
            this._treeview.Location = new System.Drawing.Point(0, 0);
            this._treeview.Name = "_treeview";
            this._treeview.Size = new System.Drawing.Size(195, 424);
            this._treeview.TabIndex = 0;
            this._treeview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeview_AfterSelect);
            this._treeview.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this._treeview_NodeMouseClick);
            // 
            // _cntxtMenu_Tree
            // 
            this._cntxtMenu_Tree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mi_Tree_Properties});
            this._cntxtMenu_Tree.Name = "_cntxtMenu";
            this._cntxtMenu_Tree.Size = new System.Drawing.Size(135, 26);
            this._cntxtMenu_Tree.Opening += new System.ComponentModel.CancelEventHandler(this._cntxtMenu_Tree_Opening);
            // 
            // _mi_Tree_Properties
            // 
            this._mi_Tree_Properties.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Properties_16;
            this._mi_Tree_Properties.Name = "_mi_Tree_Properties";
            this._mi_Tree_Properties.Size = new System.Drawing.Size(134, 22);
            this._mi_Tree_Properties.Text = "Properties";
            this._mi_Tree_Properties.Click += new System.EventHandler(this._mi_Tree_Properties_Click);
            // 
            // _ultraGrid
            // 
            this._ultraGrid.ContextMenuStrip = this._cntxtMenu_Grid;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._ultraGrid.DisplayLayout.Appearance = appearance1;
            this._ultraGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            ultraGridBand1.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._ultraGrid.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this._ultraGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._ultraGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this._ultraGrid.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this._ultraGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this._ultraGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this._ultraGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this._ultraGrid.DisplayLayout.MaxColScrollRegions = 1;
            this._ultraGrid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ultraGrid.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this._ultraGrid.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this._ultraGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._ultraGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGrid.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._ultraGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGrid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._ultraGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._ultraGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this._ultraGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this._ultraGrid.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._ultraGrid.DisplayLayout.Override.CellAppearance = appearance8;
            this._ultraGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._ultraGrid.DisplayLayout.Override.CellPadding = 0;
            this._ultraGrid.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._ultraGrid.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._ultraGrid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this._ultraGrid.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this._ultraGrid.DisplayLayout.Override.HeaderAppearance = appearance10;
            this._ultraGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._ultraGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._ultraGrid.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.LightGray;
            appearance12.TextVAlignAsString = "Middle";
            this._ultraGrid.DisplayLayout.Override.RowAppearance = appearance12;
            this._ultraGrid.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._ultraGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.Highlight;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._ultraGrid.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this._ultraGrid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._ultraGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._ultraGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this._ultraGrid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this._ultraGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this._ultraGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._ultraGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._ultraGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._ultraGrid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._ultraGrid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._ultraGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._ultraGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ultraGrid.Location = new System.Drawing.Point(0, 19);
            this._ultraGrid.Name = "_ultraGrid";
            this._ultraGrid.Size = new System.Drawing.Size(437, 405);
            this._ultraGrid.TabIndex = 5;
            this._ultraGrid.Text = "ultraGrid1";
            this._ultraGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this._ultraGrid_MouseDown);
            this._ultraGrid.DoubleClickRow += new Infragistics.Win.UltraWinGrid.DoubleClickRowEventHandler(this._ultraGrid_DoubleClickRow);
            this._ultraGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._ultraGrid_InitializeLayout);
            // 
            // _cntxtMenu_Grid
            // 
            this._cntxtMenu_Grid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mi_Grid_Properties});
            this._cntxtMenu_Grid.Name = "_cntxtMenu_Grid";
            this._cntxtMenu_Grid.Size = new System.Drawing.Size(135, 26);
            this._cntxtMenu_Grid.Opening += new System.ComponentModel.CancelEventHandler(this._cntxtMenu_Grid_Opening);
            // 
            // _mi_Grid_Properties
            // 
            this._mi_Grid_Properties.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Properties_16;
            this._mi_Grid_Properties.Name = "_mi_Grid_Properties";
            this._mi_Grid_Properties.Size = new System.Drawing.Size(134, 22);
            this._mi_Grid_Properties.Text = "Properties";
            this._mi_Grid_Properties.Click += new System.EventHandler(this._mi_Grid_Properties_Click);
            // 
            // _toolHdr
            // 
            this._toolHdr.AutoSize = false;
            this._toolHdr.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this._toolHdr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._toolHdr.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolHdr.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._toolHdr.HotTrackEnabled = false;
            this._toolHdr.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._tsbtn_Up,
            this._tslbl_NodeName,
            this._toolStripButton_Print,
            this._toolStripButton_Save,
            this._toolStripButton_GroupBy,
            this._tsbtn_ColumnChooser});
            this._toolHdr.Location = new System.Drawing.Point(0, 0);
            this._toolHdr.Name = "_toolHdr";
            this._toolHdr.Size = new System.Drawing.Size(437, 19);
            this._toolHdr.TabIndex = 0;
            // 
            // _tsbtn_Up
            // 
            this._tsbtn_Up.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_Up.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsbtn_Up.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.folder_up;
            this._tsbtn_Up.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_Up.Margin = new System.Windows.Forms.Padding(0);
            this._tsbtn_Up.Name = "_tsbtn_Up";
            this._tsbtn_Up.Size = new System.Drawing.Size(23, 19);
            this._tsbtn_Up.Text = "Up";
            this._tsbtn_Up.Click += new System.EventHandler(this._tsbtn_Up_Click);
            // 
            // _tslbl_NodeName
            // 
            this._tslbl_NodeName.Image = ((System.Drawing.Image)(resources.GetObject("_tslbl_NodeName.Image")));
            this._tslbl_NodeName.Name = "_tslbl_NodeName";
            this._tslbl_NodeName.Size = new System.Drawing.Size(94, 16);
            this._tslbl_NodeName.Text = "toolStripLabel1";
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
            this._toolStripButton_Print.Click += new System.EventHandler(this._toolStripButton_LoginsServerPrint_Click);
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
            this._toolStripButton_Save.Click += new System.EventHandler(this._toolStripButton_LoginsServerSave_Click);
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
            this._toolStripButton_GroupBy.Click += new System.EventHandler(this._toolStripButton_LoginsServerGroupBy_Click);
            // 
            // _tsbtn_ColumnChooser
            // 
            this._tsbtn_ColumnChooser.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._tsbtn_ColumnChooser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._tsbtn_ColumnChooser.Image = ((System.Drawing.Image)(resources.GetObject("_tsbtn_ColumnChooser.Image")));
            this._tsbtn_ColumnChooser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._tsbtn_ColumnChooser.Margin = new System.Windows.Forms.Padding(0);
            this._tsbtn_ColumnChooser.Name = "_tsbtn_ColumnChooser";
            this._tsbtn_ColumnChooser.Size = new System.Drawing.Size(23, 19);
            this._tsbtn_ColumnChooser.Text = "Select Columns";
            this._tsbtn_ColumnChooser.Click += new System.EventHandler(this._tsbtn_ColumnChooser_Click);
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
            // _ultraPrintPreviewDialog
            // 
            this._ultraPrintPreviewDialog.Name = "_ultraPrintPreviewDialog";
            // 
            // _pnl_Header
            // 
            this._pnl_Header.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Header.Controls.Add(this._lbl_Instructions);
            this._pnl_Header.Controls.Add(this._pictureBox_Snapshot);
            this._pnl_Header.Controls.Add(this._lnklbl_Snapshot);
            this._pnl_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_Header.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._pnl_Header.Location = new System.Drawing.Point(0, 0);
            this._pnl_Header.Name = "_pnl_Header";
            this._pnl_Header.Rotation = 90F;
            this._pnl_Header.Size = new System.Drawing.Size(644, 34);
            this._pnl_Header.TabIndex = 0;
            // 
            // _lbl_Instructions
            // 
            this._lbl_Instructions.AutoSize = true;
            this._lbl_Instructions.BackColor = System.Drawing.Color.Transparent;
            this._lbl_Instructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbl_Instructions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this._lbl_Instructions.Location = new System.Drawing.Point(252, 5);
            this._lbl_Instructions.Name = "_lbl_Instructions";
            this._lbl_Instructions.Size = new System.Drawing.Size(383, 26);
            this._lbl_Instructions.TabIndex = 13;
            this._lbl_Instructions.Text = "1. Drill down through the tree and select an object\r\n2. View in window or right c" +
                "lick to view properties and permissions";
            // 
            // _pictureBox_Snapshot
            // 
            this._pictureBox_Snapshot.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox_Snapshot.InitialImage = null;
            this._pictureBox_Snapshot.Location = new System.Drawing.Point(14, 9);
            this._pictureBox_Snapshot.Name = "_pictureBox_Snapshot";
            this._pictureBox_Snapshot.Size = new System.Drawing.Size(16, 16);
            this._pictureBox_Snapshot.TabIndex = 12;
            this._pictureBox_Snapshot.TabStop = false;
            // 
            // _lnklbl_Snapshot
            // 
            this._lnklbl_Snapshot.AutoEllipsis = true;
            this._lnklbl_Snapshot.BackColor = System.Drawing.Color.Transparent;
            this._lnklbl_Snapshot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lnklbl_Snapshot.Location = new System.Drawing.Point(36, 12);
            this._lnklbl_Snapshot.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this._lnklbl_Snapshot.Name = "_lnklbl_Snapshot";
            this._lnklbl_Snapshot.Size = new System.Drawing.Size(195, 13);
            this._lnklbl_Snapshot.TabIndex = 10;
            this._lnklbl_Snapshot.TabStop = true;
            this._lnklbl_Snapshot.Text = "No Snapshot Selected";
            this._lnklbl_Snapshot.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this._lnklbl_Snapshot.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._lnklbl_Snapshot_LinkClicked);
            // 
            // ObjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._splitContainer);
            this.Controls.Add(this._pnl_Header);
            this.Name = "ObjectExplorer";
            this.Size = new System.Drawing.Size(644, 462);
            this.Leave += new System.EventHandler(this.ObjectExplorer_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.ResumeLayout(false);
            this._cntxtMenu_Tree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ultraGrid)).EndInit();
            this._cntxtMenu_Grid.ResumeLayout(false);
            this._toolHdr.ResumeLayout(false);
            this._toolHdr.PerformLayout();
            this._pnl_Header.ResumeLayout(false);
            this._pnl_Header.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_Snapshot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
        private GradientPanel _pnl_Header;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.LinkLabel _lnklbl_Snapshot;
        private System.Windows.Forms.TreeView _treeview;
        private HeaderStrip _toolHdr;
        private System.Windows.Forms.ToolStripButton _tsbtn_Up;
        private Infragistics.Win.UltraWinGrid.UltraGrid _ultraGrid;
        private System.Windows.Forms.ContextMenuStrip _cntxtMenu_Tree;
        private System.Windows.Forms.ToolStripMenuItem _mi_Tree_Properties;
        private System.Windows.Forms.ContextMenuStrip _cntxtMenu_Grid;
        private System.Windows.Forms.ToolStripMenuItem _mi_Grid_Properties;
        private System.Windows.Forms.ToolStripLabel _tslbl_NodeName;
        private System.Windows.Forms.PictureBox _pictureBox_Snapshot;
        private System.Windows.Forms.ToolStripButton _tsbtn_ColumnChooser;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Print;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Save;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GroupBy;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private System.Windows.Forms.Label _lbl_Instructions;
    }
}
