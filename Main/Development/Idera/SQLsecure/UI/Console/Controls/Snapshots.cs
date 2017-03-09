using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;

using Infragistics.Win.UltraWinGrid;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class Snapshots : UserControl, Interfaces.IView, Interfaces.ICommandHandler
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.Snapshots");

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            //_label_User_Context.Text = ErrorMsgs.NoPermissionsShown;
            
            m_serverInstance = ((Data.Server)contextIn).ServerInstance;
            if (m_serverInstance != null)
            {
                Sql.RegisteredServer.GetServer(Program.gController.Repository.ConnectionString,
                                                    m_serverInstance.ConnectionName, out m_serverInstance);
            }

            if (m_serverInstance == null)
            {
                m_dt_snapshots.Clear();

                _grid.BeginUpdate();
                _grid.DataSource = m_dt_snapshots.DefaultView;
                _grid.DataMember = "";
                this._grid.EndUpdate();
                _label_Snapshot.Text = HeaderSnapshots;
            }
            else
            {
                loadDataSource();
            }
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ExploreSnapshotsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ExploreSnapshotsConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return ""; }
        }

        #endregion

        #region ICommandHandler Members

        public delegate void RefreshViewHandlerDelegate();
        public RefreshViewHandlerDelegate m_RefreshParentView = null; 

        void Interfaces.ICommandHandler.ProcessCommand(Utility.ViewSpecificCommand command)
        {
            switch (command)
            {
                case Utility.ViewSpecificCommand.NewAuditServer:
                    showNewAuditServer();
                    break;
                case Utility.ViewSpecificCommand.NewLogin:
                    showNewLogin();
                    break;
                case Utility.ViewSpecificCommand.Baseline:
                    showBaseline();
                    break;
                case Utility.ViewSpecificCommand.Collect:
                    showCollect();
                    break;
                case Utility.ViewSpecificCommand.Configure:
                    showConfigure();
                    break;
                case Utility.ViewSpecificCommand.Delete:
                    showDelete();
                    break;
                case Utility.ViewSpecificCommand.Properties:
                    showProperties();
                    break;
                case Utility.ViewSpecificCommand.Refresh:
                    showRefresh();
                    break;
                case Utility.ViewSpecificCommand.UserPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                    break;
                case Utility.ViewSpecificCommand.ObjectPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                    break;
                default:
                    Debug.Assert(false, "Unknown command passed to Snapshots");
                    break;
            }

        }

        protected virtual void showNewLogin()
        {
            Forms.Form_WizardNewLogin.Process();
        }

        protected virtual void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();
        }

        protected virtual void showBaseline()
        {
            Debug.Assert(!(_grid.Selected.Rows.Count == 0), "Attempt to baseline snapshot with no selections");
            Debug.Assert(!(_grid.Selected.Rows.Count > 1), "Attempt to baseline snapshot for multiple selections");

            if (m_serverInstance != null && Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                Sql.Snapshot snap = (Snapshot)_grid.Selected.Rows[0].Cells[colSnapshot].Value;
                if (Forms.Form_BaselineSnapshot.Process(snap) == DialogResult.OK)
                {
                    loadDataSource();
                }
            }
            else
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.BaselineSnapshotCaption, Utility.ErrorMsgs.ServerNotRegistered);
                Program.gController.SignalRefreshServersEvent(false, null);
            }
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - Snapshots showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - Snapshots showConfigure command called erroneously");
        }

        protected virtual void showDelete()
        {
            Debug.Assert(!(_grid.Selected.Rows.Count == 0), "Attempt to delete snapshots with no selections");

            if (m_serverInstance != null && Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                Snapshot.SnapshotList deleteList = new Snapshot.SnapshotList();
                _grid.ActiveRow.Selected = true;
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in _grid.Selected.Rows)
                {
                    if (row.Cells[colStatus].Text == Utility.Snapshot.StatusInProgress)
                    {
                        deleteList.Clear();
                        Utility.MsgBox.ShowError(Utility.ErrorMsgs.DeleteSnapshotCaption, Utility.ErrorMsgs.DeleteSnapshotInProgressMsg);
                        Cursor = Cursors.Default;
                        return;
                    }
                    else
                    {
                        deleteList.Add((Snapshot)row.Cells[colSnapshot].Value);
                    }
                }

                if (deleteList.Count > 0)
                {
                    if (DialogResult.OK == Forms.Form_DeleteSnapshot.Process(deleteList))
                    {
                        // set the cursor back to wait because the form cleared it
                        Cursor = Cursors.WaitCursor;

                        loadDataSource();
                    }
                }
                else
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.DeleteSnapshotCaption, Utility.ErrorMsgs.DeleteSnapshotNoSelectionMsg);
                }

                Program.gController.RefreshServerInTree(m_serverInstance.ConnectionName);
            }
            else
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.DeleteSnapshotCaption, Utility.ErrorMsgs.ServerNotRegistered);
                Program.gController.SignalRefreshServersEvent(false, null);
            }
        }

        protected virtual void showProperties()
        {
            Debug.Assert(!(_grid.Selected.Rows.Count == 0), "Attempt to show snapshot properties with no selections");
            Debug.Assert(!(_grid.Selected.Rows.Count > 1), "Attempt to show snapshot properties for multiple selections");

            if (m_serverInstance != null && Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                int snapshotId = (int)_grid.ActiveRow.Cells[colId].Value;
                Sql.ObjectTag tag = new ObjectTag(snapshotId, Sql.ObjectType.TypeEnum.Snapshot);

                Forms.Form_SnapshotProperties.Process(tag);
            }
            else
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.SnapshotPropertiesCaption, Utility.ErrorMsgs.ServerNotRegistered);
                Program.gController.SignalRefreshServersEvent(false, null);
            }
        }

        protected virtual void showRefresh()
        {
            if (m_RefreshParentView != null)
            {
                m_RefreshParentView();
            }
            else
            {
                loadDataSource();
            }
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            int snapshotId = 0;

            if (m_serverInstance != null && Sql.RegisteredServer.IsServerRegistered(m_serverInstance.ConnectionName))
            {
                if (_grid.ActiveRow == null)
                {
                    snapshotId = Forms.Form_SelectSnapshot.GetSnapshotId(m_serverInstance);
                }
                else
                {
                    snapshotId = (int)_grid.ActiveRow.Cells[colId].Value;
                }
                if (snapshotId != 0)
                {
                    Cursor = Cursors.WaitCursor;

                    Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(m_serverInstance, snapshotId, tabIn),
                                                                Utility.View.PermissionExplorer));
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.UserPermissionsCaption, Utility.ErrorMsgs.ServerNotRegistered);
                Program.gController.SignalRefreshServersEvent(false, null);
            }
        }

        #endregion

        #region fields

        Utility.MenuConfiguration m_menuConfiguration;

        private Sql.RegisteredServer m_serverInstance;
        private Sql.Snapshot.SnapshotList m_snapshots;
        private DataTable m_dt_snapshots = new DataTable();

        private bool m_gridCellClicked = false;

        #endregion

        #region Ctors

        public Snapshots() : base()
        {
            InitializeComponent();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            m_dt_snapshots.Columns.Add(colIcon, typeof(Image));
            m_dt_snapshots.Columns.Add(colId, typeof(int));
            m_dt_snapshots.Columns.Add(colStartDate, typeof(DateTime));
            m_dt_snapshots.Columns.Add(colStartTime, typeof(DateTime));
            //m_dt_snapshots.Columns.Add(colAutomated, typeof(Image));
            m_dt_snapshots.Columns.Add(colStatus, typeof(String));
            m_dt_snapshots.Columns.Add(colComments, typeof(String));
            m_dt_snapshots.Columns.Add(colBaseline, typeof(String));
            m_dt_snapshots.Columns.Add(colBaselineComments, typeof(String));
            m_dt_snapshots.Columns.Add(colObjects, typeof(int));
            m_dt_snapshots.Columns.Add(colPermissions, typeof(int));
            m_dt_snapshots.Columns.Add(colLogins, typeof(int));
            m_dt_snapshots.Columns.Add(colWindowsGroupMembers, typeof(int));
            m_dt_snapshots.Columns.Add(colSnapshot, typeof(object));

            _toolStripButton_GridColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GridGroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_GridSave.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_GridPrint.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            _cmsi_Snapshot_exploreUserPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.UserPermissions);
            _cmsi_Snapshot_exploreSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.ObjectExplorer);
            _cmsi_Snapshot_baselineSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.MarkAsBaseline);
            _cmsi_Snapshot_deleteSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            //_cmsi_Snapshot_GroupByBox.Image - uses a checked value/image instead of app image
            _cmsi_Snapshot_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _cmsi_Snapshot_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _cmsi_Snapshot_refresh.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
            _cmsi_Snapshot_properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);

            _label_Snapshot.Text = HeaderSnapshots;

            _grid.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _grid.DrawFilter = new Utility.HideFocusRectangleDrawFilter();
            _grid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;
        }

        #endregion

        #region Columns & Constants

        private const string colIcon = @"Icon";
        private const string colId = @"SnapshotId";
        private const string colStartDate = @"Date";
        private const string colStartTime = @"Time";
        private const string colAutomated = @"Scheduled";
        private const string colStatus = @"Status";
        private const string colComments = @"Comments";
        private const string colBaseline = @"Baseline";
        private const string colBaselineComments = @"Baseline Comments";
        private const string colObjects = @"Objects";
        private const string colPermissions = @"Permissions";
        private const string colLogins = @"Logins";
        private const string colWindowsGroupMembers = @"Windows Accounts";
        private const string colSnapshot = @"Snapshot";
        private const string colAzureADMembers = "Azure AD Accounts";

        private const string HeaderSnapshots = @"Server Snapshots";
        private const string HeaderDisplay = HeaderSnapshots + " ({0} items)";
        private const string PrintTitle = @"Audit History";
        private const string PrintHeaderDisplay = "SQL Server {0} - {1} as of {2}";

        #endregion

        #region Helpers

        private void loadDataSource()
        {
            m_snapshots = Snapshot.LoadSnapshots(m_serverInstance.ConnectionName);
            m_dt_snapshots.Clear();
            //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            if (m_serverInstance.ServerType == Utility.ServerType.AzureSQLDatabase)
            {
                if (m_dt_snapshots.Columns.Contains(colWindowsGroupMembers))
                {
                    m_dt_snapshots.Columns[colWindowsGroupMembers].ColumnName = colAzureADMembers;
                }
            }
            else
            {
                if (m_dt_snapshots.Columns.Contains(colAzureADMembers))
                {
                    m_dt_snapshots.Columns[colAzureADMembers].ColumnName = colWindowsGroupMembers;
                }
            }
            //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            foreach (Snapshot snap in m_snapshots)
            {
                m_dt_snapshots.Rows.Add(snap.Icon,
                                        snap.SnapshotId,
                                        snap.StartTime.ToLocalTime().Date,
                                        snap.StartTime.ToLocalTime(),
                                        snap.StatusText,
                                        snap.SnapshotComment,
                                        snap.Baseline,
                                        snap.BaselineComment,
                                        snap.NumObject,
                                        snap.NumPermission,
                                        snap.NumLogin,
                                        m_serverInstance.ServerType != Utility.ServerType.AzureSQLDatabase? snap.NumWindowsGroupMember:Utility.Helper.AzureADUsersAndGroupCount(snap.SnapshotId),//SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
                                        snap);
            }
            m_dt_snapshots.DefaultView.Sort = colStartTime + @" desc";

            _grid.BeginUpdate();
            _grid.DataSource = m_dt_snapshots.DefaultView;
            _grid.DataMember = "";
            if (_grid.Rows.Count > 0)
            {
                _grid.Rows[0].Activate();
            }
            this._grid.EndUpdate();
            _label_Snapshot.Text = string.Format(HeaderDisplay, _grid.Rows.Count.ToString());
        }

        protected void setMenuConfiguration()
        {
            try
            {
                switch (_grid.Selected.Rows.Count)
                {
                    case 0:
                        m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
                        m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
                        m_menuConfiguration.SnapshotItems[(int)Utility.MenuItems_Snapshots.Baseline] = false;
                        break;
                    case 1:
                        m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = true;
                        m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = true;
                        m_menuConfiguration.SnapshotItems[(int)Utility.MenuItems_Snapshots.Baseline] = true;
                        break;
                    default:
                        m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = true;
                        m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
                        m_menuConfiguration.SnapshotItems[(int)Utility.MenuItems_Snapshots.Baseline] = false;
                        break;
                }

                Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
            }
            catch{}
        }

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Forms.Form_GridColumnChooser.Process(grid, PrintTitle);
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            // Associate the print document with the grid & preview dialog here
            // for consistency with other forms that require it
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = PrintTitle;
            _ultraGridPrintDocument.FitWidthToPages = 1;
            _ultraGridPrintDocument.Header.TextLeft =
                    string.Format(PrintHeaderDisplay,
                                        m_serverInstance == null ? "" : m_serverInstance.ConnectionName,
                                        _label_Snapshot.Text,
                                        DateTime.Now.ToShortDateString()
                                    );
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
                    //save the current state of the icon column and then hide it before exporting
                    iconHidden = grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden;
                    grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = true;
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ExportToExcelCaption, Utility.ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                grid.DisplayLayout.Bands[0].Columns[colIcon].Hidden = iconHidden;
            }
        }

        #endregion

        #region Events

        #region View events

        private void Snapshots_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                _grid.Focus();
            }
        }

        private void Snapshots_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        private void Snapshots_Leave(object sender, EventArgs e)
        {
            //Let the view handle this to keep from losing context on other parts of the view
            //Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion

        #region Grid events

        private void _grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colIcon].Header.Caption = "";
            band.Columns[colIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            band.Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colIcon].Width = 22;

            band.Columns[colId].Hidden = true;
            band.Columns[colId].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            band.Columns[colStartDate].Header.ToolTipText = Utility.Snapshot.ToolTipDate;
            band.Columns[colStartDate].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colStartDate].Format = Utility.Constants.DATE_FORMAT;
            band.Columns[colStartDate].Width = 65;

            band.Columns[colStartTime].Header.ToolTipText = Utility.Snapshot.ToolTipTime;
            band.Columns[colStartTime].GroupByMode = Infragistics.Win.UltraWinGrid.GroupByMode.Hour;
            band.Columns[colStartTime].Format = Utility.Constants.TIME_FORMAT;
            band.Columns[colStartTime].Width = 69;

            //band.Columns[colAutomated].Header.Caption = "";
            //band.Columns[colAutomated].Header.ToolTipText = Utility.Snapshot.ToolTipAutomated;
            //band.Columns[colAutomated].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            //band.Columns[colAutomated].Width = 22;

            band.Columns[colStatus].Header.ToolTipText = Utility.Snapshot.ToolTipStatus;
            band.Columns[colStatus].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colStatus].Width = 72;

            band.Columns[colComments].Header.ToolTipText = Utility.Snapshot.ToolTipComment;

            band.Columns[colBaseline].Header.ToolTipText = Utility.Snapshot.ToolTipBaseline;
            band.Columns[colBaseline].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colBaseline].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            band.Columns[colBaseline].Width = 66;

            band.Columns[colBaselineComments].Header.ToolTipText = Utility.Snapshot.ToolTipBaselineComment;
            band.Columns[colBaselineComments].Hidden = true;

            band.Columns[colObjects].Header.ToolTipText = Utility.Snapshot.ToolTipObjects;

            band.Columns[colPermissions].Header.ToolTipText = Utility.Snapshot.ToolTipPermissions;

            band.Columns[colLogins].Header.ToolTipText = Utility.Snapshot.ToolTipLogins;

            //Start-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database
            if (band.Columns.Exists(colWindowsGroupMembers))
            {
                band.Columns[colWindowsGroupMembers].Header.ToolTipText = Utility.Snapshot.ToolTipGroupMembers;
                band.Columns[colWindowsGroupMembers].Width = 90;
            }
            //End-SQLsecure 3.1 (Tushar)--Added support for Azure SQL Database

            band.Columns[colSnapshot].Hidden = true;
            band.Columns[colSnapshot].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        }

        private void _grid_Enter(object sender, EventArgs e)
        {
            // make the active row a selected one on entry, because it is not on grid init
            // and shouldn't be until the grid gets focus
            if (_grid.ActiveRow != null)
            {
                //_grid.ActiveRow.Selected = true;
                setMenuConfiguration();
            }
        }

        private void _grid_Leave(object sender, EventArgs e)
        {
            //// change the visual indicators of active & selected when losing focus
            //// so it won't confuse the user by appearing to be current
            //if (_grid.ActiveRow != null)
            //{
            //    if (_grid.ActiveRow.Selected)
            //    {
            //        _grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.LightGray;
            //    }
            //    else
            //    {
            //        if (_grid.ActiveRow.Index % 2 == 1)
            //        {
            //            _grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = _grid.DisplayLayout.Override.RowAlternateAppearance.BackColor;
            //        }
            //        else
            //        {
            //            _grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = _grid.DisplayLayout.Override.RowAppearance.BackColor;
            //        }
            //    }
            //    _grid.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
            //}

            //_grid.DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.LightGray;
            //_grid.DisplayLayout.Override.SelectedRowAppearance.ForeColor = Color.Black;
        }

        private void _grid_AfterRowActivate(object sender, EventArgs e)
        {
//            ((Infragistics.Win.UltraWinGrid.UltraGrid)sender).ActiveRow.Selected = true;
        }

        private void _grid_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            setMenuConfiguration();
        }

        // Make right click select row.
        // Also, make clicking off of an element clear selected row
        //  unless clicking on header or scroll bars
        // --------------------------------------------------------
        private void _grid_MouseDown(object sender, MouseEventArgs e)
        {
            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;
            Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)sender;

            elementMain = grid.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));
            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;
                if (cell != null)
                {
                    m_gridCellClicked = true;
                    if (!cell.Row.Selected)
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            grid.Selected.Rows.Clear();
                            cell.Row.Selected = true;
                            grid.ActiveRow = cell.Row;
                        }
                    }
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

            setMenuConfiguration();
        }

        private void _grid_DoubleClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (m_gridCellClicked && _grid.Selected.Rows.Count == 1)
            {
                //showProperties();
                showPermissions(Views.View_PermissionExplorer.Tab.SnapshotSummary);
            }

            Cursor = Cursors.Default;
        }

        #endregion

        #region Context Menu events
        private void _contextMenuStrip_Snapshot_Opening(object sender, CancelEventArgs e)
        {
            bool oneRow = _grid.Selected.Rows.Count.Equals(1);
            bool noRows = _grid.Selected.Rows.Count.Equals(0);
            bool isValid = false;
            if (oneRow)
            {
                Snapshot snap = (Snapshot)_grid.Selected.Rows[0].Cells[colSnapshot].Value;
                if (snap.HasValidPermissions)
                {
                    isValid = true;
                }
            }

            _cmsi_Snapshot_deleteSnapshot.Text = ((oneRow || noRows) ? Utility.Snapshot.MenuDelete : Utility.Snapshot.MenuDeleteMultiple);

            _cmsi_Snapshot_exploreUserPermissions.Enabled = oneRow && isValid && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            _cmsi_Snapshot_exploreSnapshot.Enabled = oneRow && isValid && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            _cmsi_Snapshot_baselineSnapshot.Enabled = oneRow && isValid && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Baseline);
            _cmsi_Snapshot_deleteSnapshot.Enabled = !noRows && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            _cmsi_Snapshot_groupBy.Enabled = true;
            _cmsi_Snapshot_Save.Enabled = true;
            _cmsi_Snapshot_Print.Enabled = true;
            _cmsi_Snapshot_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
            _cmsi_Snapshot_properties.Enabled = oneRow && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);

            _cmsi_Snapshot_groupBy.Checked = !_grid.DisplayLayout.GroupByBox.Hidden;
        }

        private void _cmsi_Snapshot_exploreUserPermissions_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_exploreSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_baselineSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showBaseline();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_deleteSnapshot_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showDelete();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_groupBy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(_grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_Save_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            saveGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_Print_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            printGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_refresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showRefresh();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Snapshot_properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showProperties();

            Cursor = Cursors.Default;
        }

        #endregion

        #region Header Strip buttons

        private void _toolStripButton_GridGroupBy_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            printGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            saveGrid(_grid);

            Cursor = Cursors.Default;
        }

        private void _toolStripButton_GridColumnChooser_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(_grid);

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion
    }
}
