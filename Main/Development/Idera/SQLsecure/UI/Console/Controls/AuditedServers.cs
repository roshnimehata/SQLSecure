using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinListView;
using Infragistics.Win.UltraWinToolTip;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class AuditedServers : UserControl
    {
        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.AuditedServers");
        private bool m_Initialized = false;
        private Utility.MenuConfiguration m_menuConfiguration;
        private Control m_focused = null; // To prevent focus from switching to the splitters, etc.
        private bool m_gridCellClicked = false;
        
        private DataTable m_ServersTable;
        private DataTable m_DatabasesTable;

        private List<Sql.RegisteredServer> m_Servers = null;
        private string m_SelectedServer = string.Empty;

        #endregion

        #region Ctors

        public AuditedServers()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // hook the toolbar labels to the grids so the heading can be used for printing
            _grid_Servers.Tag = _label_AuditedServers;
            _grid_Databases.Tag = _label_Databases;

            // hook the grids to the toolbars so they can be used for button processing
            _headerStrip_Servers.Tag = _grid_Servers;
            _headerStrip_Databases.Tag = _grid_Databases;

            _toolStripButton_ServersColumnChooser.Image = 
                _toolStripButton_DatabasesColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_ServersGroupBy.Image =
                _toolStripButton_DatabasesGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_ServersSave.Image =
                _toolStripButton_DatabasesSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_ServersPrint.Image =
                _toolStripButton_DatabasesPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            _cmsi_Server_exploreUserPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.UserPermissions);
            _cmsi_Server_exploreSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.ObjectExplorer);
            _cmsi_Server_properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);
            _cmsi_Server_refresh.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
            _cmsi_Server_registerSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.AuditSQLServer);
            _cmsi_Server_removeSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            _cmsi_Server_collectDataSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.CollectDataSnapshot);
            _cmsi_Server_configureDataCollection.Image = AppIcons.AppImage16(AppIcons.Enum.ConfigureAuditSettingsSM);


            // load value lists for grid display
            ValueList severityValueList = new ValueList();
            severityValueList.Key = ValueListYesNo;
            severityValueList.DisplayStyle = ValueListDisplayStyle.DisplayText;
            _grid_Databases.DisplayLayout.ValueLists.Add(severityValueList);

            ValueListItem listItem;

            severityValueList.ValueListItems.Clear();
            listItem = new ValueListItem(true, "Yes");
            severityValueList.ValueListItems.Add(listItem);
            listItem = new ValueListItem(false, "No");
            severityValueList.ValueListItems.Add(listItem);

            // Initialize the grids
            initDataSources();

            _grid_Servers.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _grid_Servers.DrawFilter = new Utility.HideFocusRectangleDrawFilter();
            _grid_Servers.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            _grid_Databases.DrawFilter = new Utility.HideFocusRectangleDrawFilter();
            _grid_Databases.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;
        }

        #endregion

        #region Queries, Columns and Constants

        private const string QueryGetDatabases = @"SELECT databasename FROM SQLsecure.dbo.vwdatabases WHERE snapshotid = {0} ORDER BY databasename";

        // Server columns
        private const string colIcon = "Icon";
        private const string colServer = "Server";
        private const string colVersion = "Version";
        private const string colCollectionTime = "CollectionTime";
        private const string colCollectionStatus = "CollectionStatus";

        // Database columns
        private const string colDatabase = "Database";
        private const string colOwner = "Owner";
        private const string colGuestEnabled = "Guest Enabled";
        private const string colAvailable = "Available";
        private const string colStatus = "Status";

        // Constants
        private const string HeaderServersGrid = @"Audited SQL Servers";
        private const string HeaderServersDisplay = HeaderServersGrid + " ({0} items)";
        private const string PrintTitle = HeaderServersGrid;
        private const string PrintServersHeaderDisplay = "Repository {0} - {1} as of {2}";

        private const string HeaderDatabasesGrid = @"Databases";
        private const string HeaderDatabasesDisplay = "{0} Databases ({1} items)";
        private const string PrintDatabasesHeaderDisplay = "{0} " + HeaderDatabasesGrid + " as of {1}";

        private const string ValueListYesNo = @"YesNo";

        #endregion

        #region properties

        #endregion

        #region Helpers

        //protected void setMenuConfiguration()
        //{
        //    Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        //}

        // Find the control with focus
        private Control getFocused(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c.Focused)
                {
                    return c;
                }
                else if (c.ContainsFocus)
                {
                    return getFocused(c.Controls);
                }
            }

            return null;
        }

        private DataTable createDataSourceServers()
        {
            // Create Assessment Details default datasources
            DataTable dt = new DataTable();
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colServer, typeof(string));
            dt.Columns.Add(colVersion, typeof(string));
            dt.Columns.Add(colCollectionTime, typeof(string));
            dt.Columns.Add(colCollectionStatus, typeof(string));

            return dt;
        }

        private DataTable createDataSourceDatabases()
        {
            // Create Assessment Details default datasources
            DataTable dt = new DataTable();
            dt.Columns.Add(colDatabase, typeof(string));
            dt.Columns.Add(colStatus, typeof(string));
            dt.Columns.Add(colOwner, typeof(string));
            dt.Columns.Add(colGuestEnabled, typeof(bool));
            dt.Columns.Add(colAvailable, typeof(bool));

            return dt;
        }

        private void initDataSources()
        {
            // Create the servers grid datasource
            m_ServersTable = createDataSourceServers();

            // Create the databases grid datasource
            m_DatabasesTable = createDataSourceDatabases();

            _label_AuditedServers.Text = HeaderServersGrid;
            _grid_Servers.SetDataBinding(m_ServersTable, null);

            _label_Databases.Text = HeaderDatabasesGrid;
            _grid_Databases.SetDataBinding(m_DatabasesTable, null);
        }

        private void updateData()
        {
            logX.loggerX.Info("Load Audited Servers");

            // Clear the grid.
            m_ServersTable.Clear();

            // Process each server in the list, if available.
            m_Servers = Sql.RegisteredServer.LoadRegisteredServers(Program.gController.Repository.ConnectionString);

            if (m_Servers != null)
            {
                Image icon = null;
                foreach (Sql.RegisteredServer srvr in m_Servers)
                {
                    // Get the icon image based on the status.
                    if (string.Compare(srvr.CurrentCollectionStatus, Utility.Snapshot.StatusSuccessfulText, true) == 0)
                    {
                        icon = AppIcons.AppImage16(AppIcons.Enum.ServerOK);
                    }
                    else if (string.Compare(srvr.CurrentCollectionStatus, Utility.Snapshot.StatusInProgressText, true) == 0)
                    {
                        icon = AppIcons.AppImage16(AppIcons.Enum.ServerInProgress);
                    }
                    else if (string.Compare(srvr.CurrentCollectionStatus, Utility.Snapshot.StatusWarningText, true) == 0)
                    {
                        icon = AppIcons.AppImage16(AppIcons.Enum.ServerWarn);
                    }
                    else if (string.Compare(srvr.CurrentCollectionStatus, Utility.Snapshot.StatusErrorText, true) == 0)
                    {
                        icon = AppIcons.AppImage16(AppIcons.Enum.ServerError);
                    }
                    else
                    {
                        icon = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                    }

                    // Add the table entry.
                    m_ServersTable.Rows.Add(icon,
                                            srvr.ConnectionName, srvr.VersionFriendly, srvr.CurrentCollectionTime,
                                                srvr.CurrentCollectionStatus);
                }
            }

            // Update the grid.
            _grid_Servers.BeginUpdate();

            _grid_Servers.SetDataBinding(m_ServersTable, null);

            if (!m_Initialized)
            {
                foreach (UltraGridColumn col in _grid_Servers.DisplayLayout.Bands[0].Columns)
                {
                    col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                }
                _grid_Servers.DisplayLayout.Bands[0].SortedColumns.Add(colServer, false, false);
            }

            if (_grid_Servers.Rows.Count > 0 && _grid_Servers.Selected.Rows.Count == 0)
            {
                UltraGridRow row = _grid_Servers.DisplayLayout.Rows.GetAllNonGroupByRows()[0];
                row.Selected = true;
                row.Activate();
            }
            else
            {
                m_SelectedServer = string.Empty;
                updateDatabases();
            }

            _grid_Servers.EndUpdate();

            _label_AuditedServers.Text = string.Format(HeaderServersDisplay, _grid_Servers.Rows.Count.ToString());

            _panel_AuditedServers.Enabled = Program.gController.Repository.IsValid;

            m_Initialized = true;
        }

        private void updateDatabases()
        {
            m_DatabasesTable.Clear();

            if (_grid_Servers.ActiveRow != null && _grid_Servers.ActiveRow.IsDataRow)
            {
                Sql.RegisteredServer server = Program.gController.Repository.RegisteredServers.Find(_grid_Servers.ActiveRow.Cells[colServer].Text);

                if (server != null)
                {
                    int snapshotId = server.LastCollectionSnapshotId;
                    List<Sql.Database> dblist = Sql.Database.GetSnapshotDatabases(snapshotId);

                    foreach (Sql.Database db in dblist)
                    {
                        DataRow dbRow = m_DatabasesTable.NewRow();
                        dbRow[colDatabase] = db.Name;
                        dbRow[colOwner] = db.Owner;
                        dbRow[colGuestEnabled] = db.IsGuestEnabled;
                        dbRow[colAvailable] = db.IsAvailable;
                        dbRow[colStatus] = db.Status;
                        m_DatabasesTable.Rows.Add(dbRow);
                    }
                }
            }

            _grid_Databases.BeginUpdate();

            _grid_Databases.SetDataBinding(m_DatabasesTable, null);
 
            //if (!m_Initialized)
            {
                foreach (UltraGridColumn col in _grid_Databases.DisplayLayout.Bands[0].Columns)
                {
                    col.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
                }
                _grid_Databases.DisplayLayout.Bands[0].SortedColumns.Add(colDatabase, false, false);
            }

            _grid_Databases.EndUpdate();
            _label_Databases.Text = string.Format(HeaderDatabasesDisplay, m_SelectedServer, _grid_Databases.Rows.Count.ToString());

            _panel_Databases.Enabled = Program.gController.Repository.IsValid;
        }

        #region grids

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // for consistency with other forms that require it
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            if (grid == _grid_Servers)
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintServersHeaderDisplay,
                                  Program.gController.Repository.Instance,
                                  _label_AuditedServers,
                                  DateTime.Now.ToShortDateString()
                        );
            }
            else
            {
                _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintDatabasesHeaderDisplay,
                                  m_SelectedServer,
                                  DateTime.Now.ToShortDateString()
                        );
            }

            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            bool iconHidden = false;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (grid == _grid_Servers)
                    {
                        //save the current state of the icon column and then hide it before exporting
                        iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                        grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                    }
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ExportToExcelCaption, Utility.ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                if (grid == _grid_Servers)
                {
                    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
                }
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

        #region Menu Functions (same as BaseView overrides)

        protected void showDelete()
        {
            Debug.Assert(!(_grid_Servers.ActiveRow == null), "No selected server row in grid");

            if (_grid_Servers.ActiveRow != null)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;

                Forms.Form_RemoveRegisteredServer.Process(server);
                showRefresh();
            }
        }

        protected void showProperties()
        {
            Debug.Assert(!(_grid_Servers.ActiveRow == null), "No selected server row in grid");

            if (_grid_Servers.ActiveRow != null)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;

                Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties, Program.gController.isAdmin);
            }
        }

        protected void showRefresh()
        {
            UpdateStatus();
        }

        protected void showPermissions(Views.View_PermissionExplorer.Tab tab)
        {
            Debug.Assert(!(_grid_Servers.ActiveRow == null), "No selected server row in grid");

            if (_grid_Servers.ActiveRow != null)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;

                Sql.RegisteredServer registeredServer = Program.gController.Repository.RegisteredServers.Find(server);

                Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(registeredServer, tab),
                                                                Utility.View.PermissionExplorer));
            }
        }

        protected void showServer()
        {
            Debug.Assert(!(_grid_Servers.ActiveRow == null), "No selected server row in grid");

            if (_grid_Servers.ActiveRow != null)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;

                Sql.RegisteredServer registeredServer = Program.gController.Repository.RegisteredServers.Find(server);

                Program.gController.SetCurrentServer(registeredServer);
            }
        }

        protected void showConfigure()
        {
            Debug.Assert(!(_grid_Servers.ActiveRow == null), "No selected server row in grid");

            if (_grid_Servers.ActiveRow != null)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;

                Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
            }
        }

        protected void showCollect()
        {
            Debug.Assert((_grid_Servers.ActiveRow != null), "No selected server row in grid");

            if (_grid_Servers.ActiveRow != null)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;

                try
                {
                    Forms.Form_StartSnapshotJobAndShowProgress.Process(server);

                    this._grid_Servers.BeginUpdate();
                    _grid_Servers.ActiveRow.Cells[colIcon].Value = AppIcons.AppImage16(AppIcons.Enum.ServerInProgress);
                    _grid_Servers.ActiveRow.Cells[colCollectionStatus].Value = @"Running";
                    this._grid_Servers.EndUpdate();
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunDataCollection, ex);
                }
            }
        }

        protected void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();

            showRefresh();
        }

        #endregion

        #endregion

        #region Methods

        public void UpdateStatus()
        {
            updateData();
        }

        #endregion

        #region Event Handlers

        #region Splitter Handlers

        private void _splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            m_focused = getFocused(this.Controls);
        }

        private void _splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_focused != null)
            {
                m_focused.Focus();
                m_focused = null;
            }

        }

        #endregion

        #region Grid

        private void _grid_Servers_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;

            e.Layout.Bands[0].Columns[colServer].Header.Caption = "Server";
            e.Layout.Bands[0].Columns[colServer].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

            e.Layout.Bands[0].Columns[colVersion].Header.Caption = "Version";
            e.Layout.Bands[0].Columns[colVersion].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

            e.Layout.Bands[0].Columns[colCollectionTime].Header.Caption = "Last Audit";
            e.Layout.Bands[0].Columns[colCollectionTime].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

            e.Layout.Bands[0].Columns[colCollectionStatus].Header.Caption = "Audit Status";
            e.Layout.Bands[0].Columns[colCollectionStatus].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
        }

        private void _grid_Servers_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_gridCellClicked && e.KeyCode == Keys.Delete)
            {
                showDelete();
            }
        }

        private void _grid_Servers_AfterRowActivate(object sender, EventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;

            if (grid.ActiveRow.IsDataRow)
            {
                m_SelectedServer = grid.ActiveRow.Cells[colServer].Text;
            }
            else
            {
                m_SelectedServer = string.Empty;
            }

            updateDatabases();
        }

        private void _grid_Servers_DoubleClick(object sender, EventArgs e)
        {
            if (m_gridCellClicked && _grid_Servers.ActiveRow != null)
            {
                showProperties();
            }
        }

        private void _grid_Databases_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.AllowRowFiltering = DefaultableBoolean.False;

            e.Layout.Bands[0].Columns[colGuestEnabled].ValueList = e.Layout.ValueLists[ValueListYesNo];

            e.Layout.Bands[0].Columns[colAvailable].Hidden = true;
            e.Layout.Bands[0].Columns[colAvailable].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        }

        private void _grid_MouseDown(object sender, MouseEventArgs e)
        {
            // Note: this event handler is used for the MouseDown event on all grids
            UltraGrid grid = (UltraGrid)sender;

            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;

            elementMain = grid.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));
            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;
                if (cell != null)
                {
                    m_gridCellClicked = true;
                    grid.Selected.Rows.Clear();
                    cell.Row.Selected = true;
                    grid.ActiveRow = cell.Row;
                }
                else
                {
                    m_gridCellClicked = false;
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;
                    if (he == null && ce == null && re == null)
                    {
                        grid.Selected.Rows.Clear();
                        grid.ActiveRow = null;
                    }
                }
            }
        }

        #endregion

        #region Header Strip buttons

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            printGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            saveGrid(grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridColumnChooser_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #region Context Menu events

        private void _contextMenuStrip_Server_Opening(object sender, CancelEventArgs e)
        {
            bool isServer = (_grid_Servers.ActiveRow != null);
            bool canExplore = false;
            if (isServer)
            {
                string server = _grid_Servers.ActiveRow.Cells[colServer].Text;

                Sql.RegisteredServer registeredServer = Program.gController.Repository.RegisteredServers.Find(server);

                canExplore = (registeredServer.LastCollectionSnapshotId != 0);
            }

            // Enable/disable based on the node type.
            _cmsi_Server_exploreUserPermissions.Enabled = isServer && canExplore && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _cmsi_Server_exploreSnapshot.Enabled = isServer && canExplore && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            _cmsi_Server_viewAuditHistory.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
            _cmsi_Server_registerSQLServer.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);
            _cmsi_Server_removeSQLServer.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            _cmsi_Server_configureDataCollection.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);
            _cmsi_Server_collectDataSnapshot.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
            _cmsi_Server_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
            _cmsi_Server_properties.Enabled = isServer && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
        }

        private void _cmsi_Server_exploreUserPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_exploreSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_viewAuditHistory_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showServer();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_registerSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showNewAuditServer();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_removeSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showDelete();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_configureDataCollection_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showConfigure();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_collectDataSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showCollect();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showRefresh();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showProperties();

            Cursor = Cursors.Default;
        }

        private void _contextMenuStrip_Database_Opening(object sender, CancelEventArgs e)
        {
            bool isDatabase = (_grid_Databases.ActiveRow != null);
            bool canExplore = true;

            // Enable/disable based on the node type.
            _cmsi_Database_exploreUserPermissions.Enabled = isDatabase && canExplore && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _cmsi_Database_exploreSnapshot.Enabled = isDatabase && canExplore && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            _cmsi_Database_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
            _cmsi_Database_properties.Enabled = isDatabase && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
        }

        private void _cmsi_Database_exploreUserPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Database_exploreSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);

            Cursor = Cursors.Default;

        }

        private void _cmsi_Database_refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showRefresh();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Database_properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showProperties();

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion
    }
}
