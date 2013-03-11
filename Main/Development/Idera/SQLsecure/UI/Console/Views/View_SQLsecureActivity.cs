using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_SQLsecureActivity : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            loadDataSource();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.SQLsecureActivityHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.SQLsecureActivityConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.View_SQLsecureActivity");
        private DataTable _dt_activity = new DataTable();

        #endregion

        #region Ctors

        public View_SQLsecureActivity()
        {
            InitializeComponent();

            // Initialize base class fields.
            this._label_Summary.Text = Utility.Constants.ViewSummary_SQLsecureActivity;
            m_menuConfiguration = new Utility.MenuConfiguration();

            // Setup the crumb and regular title.
            Title = Utility.Constants.ViewTitle_SQLsecureActivity;

            // Setup the grid columns.
            _dt_activity.Columns.Add(colActivityType, typeof(String));
            _dt_activity.Columns.Add(colEventDate, typeof(DateTime));
            _dt_activity.Columns.Add(colEventTime, typeof(DateTime));
            //_dt_activity.Columns.Add(colApplicationSource, typeof(String));
            _dt_activity.Columns.Add(colConnectionName, typeof(String));
            _dt_activity.Columns.Add(colServerLogin, typeof(String));
            _dt_activity.Columns.Add(colCategory, typeof(String));
            _dt_activity.Columns.Add(colEventCode, typeof(String));
            _dt_activity.Columns.Add(colDescription, typeof(String));

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid.Tag = _label_Activity;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_Activity.Tag = _grid;

            // Hookup all application images
            _toolStripButton_ActivityColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_ActivityGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_ActivitySave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ActivityPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            // set all grids to start in the same initial display mode
            _grid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            // load value lists for grid display
            ValueList statusValueList = new ValueList();
            statusValueList.Key = ValueListStatus;
            statusValueList.DisplayStyle = ValueListDisplayStyle.DisplayTextAndPicture;
            _grid.DisplayLayout.ValueLists.Add(statusValueList);

            ValueListItem listItem;

            statusValueList.ValueListItems.Clear();
            listItem = new ValueListItem(Utility.Activity.TypeInfo, Utility.Activity.TypeInfoText);
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.ActivityInfo);
            statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Activity.TypeWarning, Utility.Activity.TypeWarningText);
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.ActivityWarn);
            statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Activity.TypeError, Utility.Activity.TypeErrorText);
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.ActivityError);
            statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Activity.TypeAuditSuccess, Utility.Activity.TypeAuditSuccessText);
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.ActivitySuccessAudit);
            statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(Utility.Activity.TypeAuditFailure, Utility.Activity.TypeAuditFailureText);
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.ActivityFailureAudit);
            statusValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(DBNull.Value, Utility.Activity.TypeUnknownText);
            listItem.Appearance.Image = AppIcons.AppImage16(AppIcons.Enum.Unknown);
            statusValueList.ValueListItems.Add(listItem);

            // hide the focus rectangle
            _grid.DrawFilter = new Utility.HideFocusRectangleDrawFilter();

            _cmsi_refresh.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
        }

        #endregion

        #region Queries & Columns & Constants

        private const string QueryGetActivity =
                    @"SELECT TOP 10000 * FROM SQLsecure.dbo.vwapplicationactivity ORDER BY eventtimestamp DESC";

        // Columns for handling the Snapshot query results

        private const string colEventTimeStamp = @"eventtimestamp";
        private const string colActivityType = @"activitytype";
        private const string colApplicationSource = @"applicationsource";
        private const string colConnectionName = @"connectionname";
        private const string colServerLogin = @"serverlogin";
        private const string colEventCode = @"eventcode";
        private const string colCategory = @"category";
        private const string colDescription = @"description";
        // Columns for displaying info in the grid
        private const string colEventDate = @"eventdate";
        private const string colEventTime = @"eventtime";

        private const string HeaderText = @"SQLsecure Activity Log";
        private const string HeaderDisplay = HeaderText + " ({0} items)";

        private const string PrintHeaderDisplay = HeaderDisplay + " as of {1}";
        private const string PrintEmptyHeaderDisplay = HeaderText;

        private const string ValueListStatus = @"Status";

        #endregion

        #region overrides

        protected override void showRefresh()
        {
            Cursor = Cursors.WaitCursor;

            loadDataSource();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Helpers

        private void loadDataSource()
        {
            _dt_activity.Clear();
            Image iconImage;
            String status;
            try
            {
                // Retrieve activity information.
                logX.loggerX.Info("Retrieve SQLsecure application activity log");
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                                                    QueryGetActivity, null))
                    {
                        while (rdr.Read())
                        {
                            // Time stamp retrieved from the database must be converted to local time, its
                            // stored in UTC time zone.
                            _dt_activity.Rows.Add(rdr[colActivityType],
                                                    ((DateTime)rdr[colEventTimeStamp]).ToLocalTime().Date,
                                                    ((DateTime)rdr[colEventTimeStamp]).ToLocalTime(),
                                                    //rdr[colApplicationSource],
                                                    rdr[colConnectionName],
                                                    rdr[colServerLogin],
                                                    rdr[colCategory],
                                                    rdr[colEventCode],
                                                    rdr[colDescription]
                                                    );
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve application activity", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ActivityCaption, ex);
            }

            this._grid.BeginUpdate();
            this._grid.DataSource = _dt_activity;
            this._grid.DataMember = "";
            this._grid.EndUpdate();

            _label_Activity.Text = string.Format(HeaderDisplay, _grid.Rows.Count.ToString());

        }

        #region Grid

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            //_ultraGridPrintDocument.FitWidthToPages = 2;
            if (_dt_activity.Rows.Count > 0)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        grid.Rows.FilteredInNonGroupByRowCount + " of " + _dt_activity.Rows.Count,
                                        DateTime.Now
                                    );
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft = PrintEmptyHeaderDisplay;
            }
            //_ultraGridPrintDocument.Footer.TextCenter =
            //    string.Format("Page {0}",
            //                    _ultraGridPrintDocument.PageNumber,
            //                    _ultraPrintPreviewDialog.pag
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            //bool iconHidden = false;
            _saveFileDialog.FileName = "SQLsecureActivity.xls";
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ////save the current state of the icon column and then hide it before exporting
                    //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                    //{
                    //    // this column doesn't exist in the raw data hack
                    //    iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                    //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                    //    // Fix the sub list display on the effective grid
                    //    if (grid.DisplayLayout.Bands.Count > 1
                    //        && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                    //    {
                    //        grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = true;
                    //    }
                    //}
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                //if (grid.DisplayLayout.Bands[0].Columns.Exists(colIcon))
                //{
                //    // this column doesn't exist in the raw data hack
                //    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
                //}
                //// Fix the sub list display on the effective grid
                //if (grid.DisplayLayout.Bands.Count > 1
                //    && grid.DisplayLayout.Bands[1].Columns.Exists(colIcon))
                //{
                //    grid.DisplayLayout.Bands[1].Columns[colIcon].Hidden = iconHidden;
                //}
            }
        }

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // set any column chooser options before showing????
            string gridHeading = ((ToolStripLabel)grid.Tag).Text;
            if (gridHeading.IndexOf("(") > 0)
            {
                gridHeading = gridHeading.Remove(gridHeading.IndexOf("(") - 1);
            }

            Forms.Form_GridColumnChooser.Process(grid, gridHeading);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        #endregion

        #endregion

        #region Events

        #region View events

        private void View_SQLsecureActivity_Enter(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private void View_SQLsecureActivity_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion

        #region Grid events

        private void _grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns[colActivityType].Header.Caption = @"Type";
            e.Layout.Bands[0].Columns[colActivityType].ValueList = e.Layout.ValueLists[ValueListStatus];
            e.Layout.Bands[0].Columns[colActivityType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colActivityType].Width = 100;

            e.Layout.Bands[0].Columns[colEventDate].Header.Caption = @"Date";
            e.Layout.Bands[0].Columns[colEventDate].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colEventDate].Format = Utility.Constants.DATE_FORMAT;
            e.Layout.Bands[0].Columns[colEventDate].Width = 65;

            e.Layout.Bands[0].Columns[colEventTime].Header.Caption = @"Time";
            e.Layout.Bands[0].Columns[colEventTime].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colEventTime].GroupByMode = Infragistics.Win.UltraWinGrid.GroupByMode.Hour;
            e.Layout.Bands[0].Columns[colEventTime].Format = Utility.Constants.TIME_FORMAT;
            e.Layout.Bands[0].Columns[colEventTime].Width = 69;

            //e.Layout.Bands[0].Columns[colApplicationSource].Header.Caption = @"Source";
            //e.Layout.Bands[0].Columns[colApplicationSource].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;

            e.Layout.Bands[0].Columns[colConnectionName].Header.Caption = @"SQL Server";
            e.Layout.Bands[0].Columns[colConnectionName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colConnectionName].Width = 100;

            e.Layout.Bands[0].Columns[colServerLogin].Header.Caption = @"User";
            e.Layout.Bands[0].Columns[colServerLogin].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colServerLogin].Width = 85;

            e.Layout.Bands[0].Columns[colCategory].Header.Caption = @"Category";
            e.Layout.Bands[0].Columns[colCategory].Width = 55;

            e.Layout.Bands[0].Columns[colEventCode].Header.Caption = @"Code";
            e.Layout.Bands[0].Columns[colEventCode].Width = 40;

            e.Layout.Bands[0].Columns[colDescription].Header.Caption = @"Description";
            e.Layout.Bands[0].Columns[colDescription].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
        }

        // Make right click select row.
        // Also, make clicking off of an element clear selected row
        // --------------------------------------------------------
        private void _grid_MouseDown(object sender, MouseEventArgs e)
        {
            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;

            elementMain = this._grid.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));
            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;
                if (cell != null)
                {
                    Infragistics.Win.UltraWinGrid.SelectedRowsCollection sr = _grid.Selected.Rows;
                    if (sr.Count > 0)
                    {
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in sr)
                        {
                            row.Selected = false;
                        }
                    }
                    cell.Row.Selected = true;
                    _grid.ActiveRow = cell.Row;
                }
                else
                {
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;
                    if (he == null && ce == null && re == null)
                    {
                        _grid.Selected.Rows.Clear();
                        _grid.ActiveRow = null;
                    }
                }
            }
        }

        #endregion

        #region Context Menu

        private void _contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this._cmsi_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
        }

        private void _cmsi_grid_ColumnChooser_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_viewGroupByBox_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_Print_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_grid_Save_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid context menus
            Cursor = Cursors.WaitCursor;

            UltraGrid grid = (UltraGrid)((ContextMenuStrip)((ToolStripItem)sender).Owner).SourceControl;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showRefresh();

            Cursor = Cursors.Default;
        } 

        #endregion

        #region tool strips

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion
    }
}
