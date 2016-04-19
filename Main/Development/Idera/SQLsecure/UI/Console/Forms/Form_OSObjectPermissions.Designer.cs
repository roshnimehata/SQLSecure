namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_OSObjectPermissions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_OSObjectPermissions));
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
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
            this._label_Contains = new System.Windows.Forms.Label();
            this._button_Cancel = new System.Windows.Forms.Button();
            this._labelPermission = new System.Windows.Forms.Label();
            this._panel_Objects = new System.Windows.Forms.Panel();
            this._headerStrip = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this._label_Header = new System.Windows.Forms.ToolStripLabel();
            this._toolStripButton_Print = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._toolStripButton_GroupBy = new System.Windows.Forms.ToolStripButton();
            this._toolStripButton_ColumnChooser = new System.Windows.Forms.ToolStripButton();
            this._grid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this._ultraGridExcelExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this._ultraGridPrintDocument = new Infragistics.Win.UltraWinGrid.UltraGridPrintDocument(this.components);
            this._ultraPrintPreviewDialog = new Infragistics.Win.Printing.UltraPrintPreviewDialog(this.components);
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._groups_Panel = new System.Windows.Forms.Panel();
            this.groupsGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.headerStrip_Permissions = new Idera.SQLsecure.UI.Console.Controls.HeaderStrip();
            this.groupsHeaderLabel = new System.Windows.Forms.ToolStripLabel();
            this.groupsPrint = new System.Windows.Forms.ToolStripButton();
            this.groupsSave = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.userGroupsLabel = new System.Windows.Forms.Label();
            this._panel_Objects.SuspendLayout();
            this._headerStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this._groups_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupsGrid)).BeginInit();
            this.headerStrip_Permissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // _label_Contains
            // 
            this._label_Contains.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_Contains.Location = new System.Drawing.Point(19, 25);
            this._label_Contains.Name = "_label_Contains";
            this._label_Contains.Size = new System.Drawing.Size(729, 13);
            this._label_Contains.TabIndex = 8;
            this._label_Contains.Text = "has access to the following File and Registry Objects";
            // 
            // _button_Cancel
            // 
            this._button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._button_Cancel.Location = new System.Drawing.Point(671, 534);
            this._button_Cancel.Name = "_button_Cancel";
            this._button_Cancel.Size = new System.Drawing.Size(75, 23);
            this._button_Cancel.TabIndex = 3;
            this._button_Cancel.Text = "&Close";
            this._button_Cancel.UseVisualStyleBackColor = true;
            // 
            // _labelPermission
            // 
            this._labelPermission.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._labelPermission.AutoEllipsis = true;
            this._labelPermission.Location = new System.Drawing.Point(19, 12);
            this._labelPermission.Name = "_labelPermission";
            this._labelPermission.Size = new System.Drawing.Size(729, 13);
            this._labelPermission.TabIndex = 7;
            this._labelPermission.Text = "User (principal)";
            // 
            // _panel_Objects
            // 
            this._panel_Objects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._panel_Objects.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._panel_Objects.Controls.Add(this._headerStrip);
            this._panel_Objects.Controls.Add(this._grid);
            this._panel_Objects.Location = new System.Drawing.Point(21, 41);
            this._panel_Objects.Name = "_panel_Objects";
            this._panel_Objects.Size = new System.Drawing.Size(727, 220);
            this._panel_Objects.TabIndex = 16;
            // 
            // _headerStrip
            // 
            this._headerStrip.AutoSize = false;
            this._headerStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._headerStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._headerStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._headerStrip.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this._headerStrip.HotTrackEnabled = false;
            this._headerStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._label_Header,
            this._toolStripButton_Print,
            this._toolStripButton_Save,
            this.toolStripSeparator2,
            this._toolStripButton_GroupBy,
            this._toolStripButton_ColumnChooser});
            this._headerStrip.Location = new System.Drawing.Point(0, 0);
            this._headerStrip.Name = "_headerStrip";
            this._headerStrip.Size = new System.Drawing.Size(723, 19);
            this._headerStrip.TabIndex = 0;
            // 
            // _label_Header
            // 
            this._label_Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Header.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this._label_Header.Name = "_label_Header";
            this._label_Header.Size = new System.Drawing.Size(61, 16);
            this._label_Header.Text = "OS Objects";
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
            this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance15.BackColor = System.Drawing.Color.White;
            appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this._grid.DisplayLayout.Appearance = appearance15;
            this._grid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this._grid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this._grid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.Color.Transparent;
            appearance16.BackColor2 = System.Drawing.Color.Transparent;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.GroupByBox.Appearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.Transparent;
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance17.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance17;
            this._grid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance18.BackColor = System.Drawing.Color.Transparent;
            appearance18.BackColor2 = System.Drawing.Color.Transparent;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance18.ForeColor = System.Drawing.SystemColors.GrayText;
            this._grid.DisplayLayout.GroupByBox.PromptAppearance = appearance18;
            this._grid.DisplayLayout.MaxColScrollRegions = 1;
            this._grid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            appearance19.ForeColor = System.Drawing.SystemColors.ControlText;
            this._grid.DisplayLayout.Override.ActiveCellAppearance = appearance19;
            appearance20.BackColor = System.Drawing.SystemColors.Highlight;
            appearance20.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid.DisplayLayout.Override.ActiveRowAppearance = appearance20;
            this._grid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._grid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this._grid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this._grid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._grid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this._grid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance21.BackColor = System.Drawing.Color.Transparent;
            this._grid.DisplayLayout.Override.CardAreaAppearance = appearance21;
            appearance22.BorderColor = System.Drawing.Color.Silver;
            appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this._grid.DisplayLayout.Override.CellAppearance = appearance22;
            this._grid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this._grid.DisplayLayout.Override.CellPadding = 0;
            this._grid.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this._grid.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this._grid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance23.BackColor = System.Drawing.SystemColors.Control;
            appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance23.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance23.BorderColor = System.Drawing.SystemColors.Window;
            this._grid.DisplayLayout.Override.GroupByRowAppearance = appearance23;
            appearance24.TextHAlignAsString = "Left";
            this._grid.DisplayLayout.Override.HeaderAppearance = appearance24;
            this._grid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._grid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this._grid.DisplayLayout.Override.RowAlternateAppearance = appearance25;
            appearance26.BorderColor = System.Drawing.Color.LightGray;
            appearance26.TextVAlignAsString = "Middle";
            this._grid.DisplayLayout.Override.RowAppearance = appearance26;
            this._grid.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this._grid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance27.BackColor = System.Drawing.SystemColors.Highlight;
            appearance27.BorderColor = System.Drawing.Color.Black;
            appearance27.ForeColor = System.Drawing.SystemColors.HighlightText;
            this._grid.DisplayLayout.Override.SelectedRowAppearance = appearance27;
            this._grid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this._grid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLight;
            this._grid.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this._grid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this._grid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this._grid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this._grid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this._grid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this._grid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._grid.Location = new System.Drawing.Point(0, 20);
            this._grid.Name = "_grid";
            this._grid.Size = new System.Drawing.Size(723, 196);
            this._grid.TabIndex = 1;
            this._grid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._grid_InitializeLayout);
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
            // _saveFileDialog
            // 
            this._saveFileDialog.DefaultExt = "xls";
            this._saveFileDialog.Filter = "Excel Workbook (*.xls)|*.xls";
            this._saveFileDialog.Title = "Save as Excel Spreadsheet";
            // 
            // _groups_Panel
            // 
            this._groups_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._groups_Panel.Controls.Add(this.groupsGrid);
            this._groups_Panel.Controls.Add(this.headerStrip_Permissions);
            this._groups_Panel.Location = new System.Drawing.Point(23, 300);
            this._groups_Panel.Name = "_groups_Panel";
            this._groups_Panel.Size = new System.Drawing.Size(727, 220);
            this._groups_Panel.TabIndex = 17;
            // 
            // groupsGrid
            // 
            this.groupsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.groupsGrid.DisplayLayout.Appearance = appearance1;
            this.groupsGrid.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            this.groupsGrid.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.groupsGrid.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BackColor2 = System.Drawing.Color.Transparent;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.groupsGrid.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupsGrid.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.groupsGrid.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.groupsGrid.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BackColor2 = System.Drawing.Color.Transparent;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupsGrid.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.groupsGrid.DisplayLayout.MaxColScrollRegions = 1;
            this.groupsGrid.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupsGrid.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.groupsGrid.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.groupsGrid.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.groupsGrid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.groupsGrid.DisplayLayout.Override.AllowMultiCellOperations = Infragistics.Win.UltraWinGrid.AllowMultiCellOperation.None;
            this.groupsGrid.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            this.groupsGrid.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.False;
            this.groupsGrid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.groupsGrid.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.groupsGrid.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            this.groupsGrid.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.groupsGrid.DisplayLayout.Override.CellAppearance = appearance8;
            this.groupsGrid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.groupsGrid.DisplayLayout.Override.CellPadding = 0;
            this.groupsGrid.DisplayLayout.Override.ColumnAutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            this.groupsGrid.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            this.groupsGrid.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.groupsGrid.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.groupsGrid.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.groupsGrid.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.groupsGrid.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.groupsGrid.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.LightGray;
            appearance12.TextVAlignAsString = "Middle";
            this.groupsGrid.DisplayLayout.Override.RowAppearance = appearance12;
            this.groupsGrid.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows;
            this.groupsGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.Highlight;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.groupsGrid.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this.groupsGrid.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.groupsGrid.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.groupsGrid.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.groupsGrid.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.None;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupsGrid.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this.groupsGrid.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
            this.groupsGrid.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.groupsGrid.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.groupsGrid.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.groupsGrid.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.groupsGrid.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.groupsGrid.Location = new System.Drawing.Point(0, 20);
            this.groupsGrid.Name = "groupsGrid";
            this.groupsGrid.Size = new System.Drawing.Size(727, 200);
            this.groupsGrid.TabIndex = 2;
            this.groupsGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this._groupsGrid_InitializeLayout);
            // 
            // headerStrip_Permissions
            // 
            this.headerStrip_Permissions.AutoSize = false;
            this.headerStrip_Permissions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.headerStrip_Permissions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.headerStrip_Permissions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.headerStrip_Permissions.HeaderStyle = Idera.SQLsecure.UI.Console.Controls.AreaHeaderStyle.Small;
            this.headerStrip_Permissions.HotTrackEnabled = false;
            this.headerStrip_Permissions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.groupsHeaderLabel,
            this.groupsPrint,
            this.groupsSave});
            this.headerStrip_Permissions.Location = new System.Drawing.Point(0, 0);
            this.headerStrip_Permissions.Name = "headerStrip_Permissions";
            this.headerStrip_Permissions.Size = new System.Drawing.Size(727, 19);
            this.headerStrip_Permissions.TabIndex = 1;
            // 
            // groupsHeaderLabel
            // 
            this.groupsHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupsHeaderLabel.Margin = new System.Windows.Forms.Padding(3, 1, 0, 2);
            this.groupsHeaderLabel.Name = "groupsHeaderLabel";
            this.groupsHeaderLabel.Size = new System.Drawing.Size(64, 16);
            this.groupsHeaderLabel.Text = "Members Of";
            // 
            // groupsPrint
            // 
            this.groupsPrint.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.groupsPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.groupsPrint.Image = ((System.Drawing.Image)(resources.GetObject("groupsPrint.Image")));
            this.groupsPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.groupsPrint.Margin = new System.Windows.Forms.Padding(0);
            this.groupsPrint.Name = "groupsPrint";
            this.groupsPrint.Size = new System.Drawing.Size(23, 19);
            this.groupsPrint.Text = "Print";
            this.groupsPrint.Click += new System.EventHandler(this.groupsPrint_Click);
            // 
            // groupsSave
            // 
            this.groupsSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.groupsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.groupsSave.Image = ((System.Drawing.Image)(resources.GetObject("groupsSave.Image")));
            this.groupsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.groupsSave.Margin = new System.Windows.Forms.Padding(0);
            this.groupsSave.Name = "groupsSave";
            this.groupsSave.Size = new System.Drawing.Size(23, 19);
            this.groupsSave.Text = "Save";
            this.groupsSave.Click += new System.EventHandler(this.groupsSave_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(20, 284);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(729, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "is a member of the following groups that have access to SQL Server File and Regis" +
                "try Objects";
            // 
            // userGroupsLabel
            // 
            this.userGroupsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userGroupsLabel.AutoEllipsis = true;
            this.userGroupsLabel.Location = new System.Drawing.Point(20, 271);
            this.userGroupsLabel.Name = "userGroupsLabel";
            this.userGroupsLabel.Size = new System.Drawing.Size(729, 13);
            this.userGroupsLabel.TabIndex = 18;
            this.userGroupsLabel.Text = "User (principal)";
            // 
            // Form_OSObjectPermissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(769, 569);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userGroupsLabel);
            this.Controls.Add(this._groups_Panel);
            this.Controls.Add(this._panel_Objects);
            this.Controls.Add(this._label_Contains);
            this.Controls.Add(this._button_Cancel);
            this.Controls.Add(this._labelPermission);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(200, 607);
            this.Name = "Form_OSObjectPermissions";
            this.Text = "View OS Object Permissions";
            this._panel_Objects.ResumeLayout(false);
            this._headerStrip.ResumeLayout(false);
            this._headerStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this._groups_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupsGrid)).EndInit();
            this.headerStrip_Permissions.ResumeLayout(false);
            this.headerStrip_Permissions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _label_Contains;
        private System.Windows.Forms.Button _button_Cancel;
        private System.Windows.Forms.Label _labelPermission;
        private System.Windows.Forms.Panel _panel_Objects;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip _headerStrip;
        private System.Windows.Forms.ToolStripLabel _label_Header;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Print;
        private System.Windows.Forms.ToolStripButton _toolStripButton_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _toolStripButton_GroupBy;
        private System.Windows.Forms.ToolStripButton _toolStripButton_ColumnChooser;
        private Infragistics.Win.UltraWinGrid.UltraGrid _grid;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter _ultraGridExcelExporter;
        private Infragistics.Win.UltraWinGrid.UltraGridPrintDocument _ultraGridPrintDocument;
        private Infragistics.Win.Printing.UltraPrintPreviewDialog _ultraPrintPreviewDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.Panel _groups_Panel;
        private Infragistics.Win.UltraWinGrid.UltraGrid groupsGrid;
        private Idera.SQLsecure.UI.Console.Controls.HeaderStrip headerStrip_Permissions;
        private System.Windows.Forms.ToolStripLabel groupsHeaderLabel;
        private System.Windows.Forms.ToolStripButton groupsPrint;
        private System.Windows.Forms.ToolStripButton groupsSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label userGroupsLabel;
    }
}
