using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Infragistics.Win.UltraWinGrid;
using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_Main_ManageSQLsecure : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            loadDataSource();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ManageSQLsecureHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ManageSQLsecureConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region fields
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.View_Main_ManageSQLsecure");
        private DataTable _dt_servers = new DataTable();
        private Sql.Repository.RegisteredServerList _servers;

        private int m_AuditedServers;
        private int m_LicensedServers;

        private bool m_gridCellClicked = false;

        #endregion

        #region Ctors

        public View_Main_ManageSQLsecure()
        {
            InitializeComponent();

            // Initialize base class fields.
            this._label_Summary.Text = Utility.Constants.ViewSummary_ManageSQLsecure;
            m_menuConfiguration = new Utility.MenuConfiguration();

            // Set the title.
            Title = Utility.Constants.ViewTitle_ManageSQLsecure;

            // Set the grid columns.
            _dt_servers.Columns.Add(colIcon, typeof(Image));
            _dt_servers.Columns.Add(colServer, typeof(String));
            _dt_servers.Columns.Add(colStatus, typeof(String));
            _dt_servers.Columns.Add(colLastCollectionTime, typeof(DateTime));
            _dt_servers.Columns.Add(colCurrentCollectionTime, typeof(DateTime));
            _dt_servers.Columns.Add(colJobStatus, typeof(String));

            // Setup tasks.
            _smallTask_Register.TaskText = Utility.Constants.Task_Title_Register;
            _smallTask_Register.TaskImage = AppIcons.AppImage32(AppIcons.Enum32.RegisterSQLserver);
            _smallTask_Register.TaskHandler += new System.EventHandler(this.registerServer);

            _smallTask_Collect.TaskText = Utility.Constants.Task_Title_CollectData;
            _smallTask_Collect.TaskImage = AppIcons.AppImage32(AppIcons.Enum32.Snapshot);
            _smallTask_Collect.TaskHandler += new System.EventHandler(this.collectDataTask);

            _smallTask_Collect.Visible = false;

            _smallTask_Configure.TaskText = Utility.Constants.Task_Title_Configure;
            _smallTask_Configure.TaskImage = AppIcons.AppImage32(AppIcons.Enum32.ConfigureAuditSettings);
            _smallTask_Configure.TaskHandler += new System.EventHandler(this.configureServerTask);

            _smallTask_Configure.Visible = false;

            _cmsi_Server_exploreUserPermissions.Image = AppIcons.AppImage16(AppIcons.Enum.UserPermissions);
            _cmsi_Server_exploreSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.ObjectExplorer);
            _cmsi_Server_properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);
            _cmsi_Server_refresh.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
            _cmsi_Server_registerSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.AuditSQLServer);
            _cmsi_Server_removeSQLServer.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            _cmsi_Server_collectDataSnapshot.Image = AppIcons.AppImage16(AppIcons.Enum.CollectDataSnapshot);
            _cmsi_Server_configureDataCollection.Image = AppIcons.AppImage16(AppIcons.Enum.ConfigureAuditSettingsSM);

            _grid.DisplayLayout.GroupByBox.Hidden = true;
            _grid.DrawFilter = new Utility.HideFocusRectangleDrawFilter();

            _label_AuditedBar.BackColor = Color.Transparent;
            _label_RemainingLicensesBar.BackColor = Color.Transparent;
            _label_LicenseBar.BackColor = Color.Transparent;
        }

        #endregion

        #region Columns & Constants

        private const string colIcon = @"Icon";
        private const string colServer = @"SQL Server";
        private const string colStatus = @"Status";
        private const string colLastCollectionTime = @"Last Successful Collection";
        private const string colCurrentCollectionTime = @"Last Collection";
        private const string colJobStatus = @"Collector Job Status";

        private const string jobStatus_Idle = "Idle";
        private const string jobStatus_Running = "Running";
        private const string jobStatus_Missing = "No Job Assigned";

        private const string HeaderText = @"Audited SQL Servers";
        private const string HeaderDisplay = HeaderText + " ({0} items)";

        #endregion

        #region Overrides

        protected override void showDelete()
        {
            Debug.Assert(!(_grid.ActiveRow == null), "No selected server row in grid");

            string server = _grid.ActiveRow.Cells[colServer].Text;
            Forms.Form_RemoveRegisteredServer.Process(server);
            ShowRefresh();
        }

        protected override void showProperties()
        {
            Debug.Assert(!(_grid.ActiveRow == null), "No selected server row in grid");

            string server = _grid.ActiveRow.Cells[colServer].Text;
            Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.GeneralProperties, Program.gController.isAdmin);
        }

        protected override void ShowRefresh()
        {
            loadDataSource();
        }

        protected override void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            try
            {
                if (_grid.ActiveRow == null)
                {
                    if (tabIn == Views.View_PermissionExplorer.Tab.UserPermissions)
                    {
                        Forms.Form_WizardUserPermissions.Process();
                    }
                    else if (tabIn == Views.View_PermissionExplorer.Tab.ObjectPermissions)
                    {
                        Forms.Form_WizardObjectPermissions.Process();
                    }
                    else
                    {
                        Sql.RegisteredServer server = Forms.Form_SelectRegisteredServer.GetServer(true);
                        if (server != null)
                        {
                            //Program.gController.SetCurrentServer(server);
                            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(server, tabIn),
                                                                        Utility.View.PermissionExplorer));
                        }
                    }
                }
                else
                {
                    Sql.RegisteredServer registeredServer = Sql.RegisteredServer.GetServer(_grid.ActiveRow.Cells[colServer].Text);
                    if (registeredServer != null)
                    {
                        //Program.gController.SetCurrentServer(registeredServer);
                        Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(registeredServer, tabIn),
                                                                        Utility.View.PermissionExplorer));
                    }
                    else
                    {
                        Utility.MsgBox.ShowError(this.Title, Utility.ErrorMsgs.ServerNotRegistered);
                        Program.gController.SignalRefreshServersEvent(false, null);
                        ShowRefresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(this.Title, Utility.ErrorMsgs.ErrorProcessUserPermissions, ex);
            }

        }

        protected override void showCollect()
        {
            collectData();
        }

        protected override void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();

            ShowRefresh();
        }

        #endregion

        #region Helpers

        public void loadDataSource()
        {
            _systemStatus.UpdateStatus();

            //Force a refresh of the servers to get current snapshot info
            Program.gController.Repository.RefreshRegisteredServers();

            if (!string.IsNullOrEmpty(Program.gController.Repository.ConnectionString))
            {
                if (Sql.ScheduleJob.IsSQLAgentStarted(Program.gController.Repository.ConnectionString))
                {
                    label_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStarted;
                    _pictureBox_AgentStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStarted);
                }
                else
                {
                    label_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStopped;
                    _pictureBox_AgentStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStopped);
                }
            }


            _servers = Program.gController.Repository.RegisteredServers;
            _dt_servers.Clear();
            String status;
            Image iconImage = Console.Controls.AppIcons.AppImage16(Console.Controls.AppIcons.Enum.AuditSQLServer);

            foreach (Sql.RegisteredServer server in _servers)
            {
                status = server.CurrentCollectionStatus;

                if (string.Compare(status, Utility.Snapshot.StatusSuccessfulText, true) == 0)
                {
                    iconImage = AppIcons.AppImage16(AppIcons.Enum.ServerOK);
                }
                else if (string.Compare(status, Utility.Snapshot.StatusInProgressText, true) == 0)
                {
                    iconImage = AppIcons.AppImage16(AppIcons.Enum.ServerInProgress);
                }
                else if (string.Compare(status, Utility.Snapshot.StatusWarningText, true) == 0)
                {
                    iconImage = AppIcons.AppImage16(AppIcons.Enum.ServerWarn);
                }
                else if (string.Compare(status, Utility.Snapshot.StatusErrorText, true) == 0)
                {
                    iconImage = AppIcons.AppImage16(AppIcons.Enum.ServerError);
                }
                else
                {
                    iconImage = AppIcons.AppImage16(AppIcons.Enum.ServerInProgress);
                }

                if (string.IsNullOrEmpty(status))
                {
                    logX.loggerX.Warn("Warning - unknown Server status encountered", server.CurrentCollectionStatus);
                    status = @"Unknown";
                }
                DataRow dr = _dt_servers.NewRow();
                dr[colIcon] = iconImage;
                dr[colServer] = server.ConnectionName;
                dr[colStatus] = status;
                if (server.LastCollectionTime.Length > 0)
                {
                    dr[colLastCollectionTime] = Convert.ToDateTime(server.LastCollectionTime);
                }
                if (server.CurrentCollectionTime.Length > 0)
                {
                    dr[colCurrentCollectionTime] = Convert.ToDateTime(server.CurrentCollectionTime);
                }

                // Get Job Status
                // --------------
                string jobStatusStr = Sql.ScheduleJob.GetJobStatus(Program.gController.Repository.ConnectionString,
                                                                                   server.JobId);

                dr[colJobStatus] = jobStatusStr;

                _dt_servers.Rows.Add(dr);

            }
            this._grid.BeginUpdate();
            this._grid.DataSource = _dt_servers;
            this._grid.DataMember = "";
            this._grid.EndUpdate();

            m_AuditedServers = _grid.Rows.Count;
            m_LicensedServers = Program.gController.Repository.GetNumLicensedServers();

            label_AuditedCount.Text = m_AuditedServers.ToString();
            label_LicensedCount.Text = (m_LicensedServers == -1) ? "Unlimited" : m_LicensedServers.ToString();
            label_RemainingLicensesCount.Text = (m_LicensedServers == -1) ? "Unlimited" : (m_LicensedServers - m_AuditedServers).ToString();

            if(m_LicensedServers == -1)
            {
                m_LicensedServers = int.MaxValue;
            }

            _label_AuditedBar.Refresh();
            _label_LicenseBar.Refresh();
            _label_RemainingLicensesBar.Refresh();

            toolStripLabel_Header.Text = string.Format(HeaderDisplay, m_AuditedServers.ToString());

        }

        private void updateContextMenuAndSetMenuConfiguration()
        {
            bool enableSelectedRowItems = true;
            if (_grid.Selected.Rows.Count <= 0)
            {
                enableSelectedRowItems = false;
            }

            _contextMenuStrip_Server.Items["_cmsi_Server_removeSQLServer"].Enabled = enableSelectedRowItems;
            _contextMenuStrip_Server.Items["_cmsi_Server_properties"].Enabled = enableSelectedRowItems;
            _contextMenuStrip_Server.Items["_cmsi_Server_configureDataCollection"].Enabled = enableSelectedRowItems;
            _contextMenuStrip_Server.Items["_cmsi_Server_exploreUserPermissions"].Enabled = enableSelectedRowItems;
            _contextMenuStrip_Server.Items["_cmsi_Server_exploreSnapshot"].Enabled = enableSelectedRowItems;

            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = enableSelectedRowItems;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = enableSelectedRowItems;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.ConfigureDataCollection] = enableSelectedRowItems;
            m_menuConfiguration.SnapshotItems[(int)Utility.MenuItems_Snapshots.Collect] = enableSelectedRowItems;
            m_menuConfiguration.SnapshotItems[(int)Utility.MenuItems_Snapshots.Baseline] = false;

            _smallTask_Register.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.AuditSQLServer);
            _smallTask_Collect.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
            _smallTask_Configure.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ConfigureAuditSettings);


            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private void registerServer(object sender, EventArgs e)
        {
            showNewAuditServer();
        }

        private void collectData()
        {
            Debug.Assert((_grid.ActiveRow != null), "No selected server row in grid");
            if (_grid.ActiveRow != null)
            {
                string server = _grid.ActiveRow.Cells[colServer].Text;
                Sql.RegisteredServer registeredServer = null;
                Sql.RegisteredServer.GetServer(Program.gController.Repository.ConnectionString, server, out registeredServer);
                collectData(registeredServer);
            }
        }

        private void collectData(Sql.RegisteredServer registeredServer)
        {
            if (registeredServer != null)
            {
                try
                {
                    Forms.Form_StartSnapshotJobAndShowProgress.Process(registeredServer.ConnectionName);
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.CantRunDataCollection, ex);
                }

                if (_grid.ActiveRow != null)
                {
                    this._grid.BeginUpdate();
                    DataRow dr = _dt_servers.Rows[_grid.ActiveRow.Index];
                    dr[colJobStatus] = "Running";
                    this._grid.DataSource = _dt_servers;
                    this._grid.DataMember = "";
                    this._grid.EndUpdate();
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    ShowRefresh();
                }
            }
            else
            {
                Utility.MsgBox.ShowWarning(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.ServerNotRegistered);
            }
        }

        private void configureServer()
        {
            Debug.Assert(!(_grid.ActiveRow == null), "No selected server row in grid");
            string server = _grid.ActiveRow.Cells[colServer].Text;
            configureServer(server);
        }

        private void configureServer(string server)
        {
            Debug.Assert(!String.IsNullOrEmpty(server), "No server selected");
            if (!String.IsNullOrEmpty(server))
            {
                Forms.Form_SqlServerProperties.Process(server, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
            }
        }


        private void collectDataTask(object sender, EventArgs e)
        {
            if (_grid.ActiveRow == null)
            {
                collectData(Forms.Form_SelectRegisteredServer.GetServer());
            }
            else
            {
                collectData();
            }
        }

        private void configureServerTask(object sender, EventArgs e)
        {
            if (_grid.ActiveRow == null)
            {
                Sql.RegisteredServer rServer = Forms.Form_SelectRegisteredServer.GetServer();
                if (rServer != null)
                {
                    configureServer(rServer.ConnectionName);
                }
            }
            else
            {
                configureServer();
            }
        }

        #endregion

        #region Events

        #region View events

        private void View_Main_ManageSQLsecure_Load(object sender, EventArgs e)
        {
            updateContextMenuAndSetMenuConfiguration();
        }

        private void View_Main_ManageSQLsecure_Enter(object sender, EventArgs e)
        {
            updateContextMenuAndSetMenuConfiguration();
        }

        private void View_Main_ManageSQLsecure_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }

        #endregion

        private void label_LicenseBar_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = _label_LicenseBar.ClientRectangle;            
            if (m_LicensedServers >= m_AuditedServers)
            {
                e.Graphics.FillRectangle(Brushes.SteelBlue, rect);
            }
            else if(m_AuditedServers > 0)
            {
                rect.Width = (int)(rect.Width * ((float)m_LicensedServers / (float)m_AuditedServers));
                e.Graphics.FillRectangle(Brushes.SteelBlue, rect);
            }
        }

        private void label_AuditedBar_Paint(object sender, PaintEventArgs e)
        {
            if (m_LicensedServers != 0)
            {
                Rectangle rect = _label_AuditedBar.ClientRectangle;
                if (m_LicensedServers > 0 && m_LicensedServers >= m_AuditedServers)
                {
                    if (m_LicensedServers == int.MaxValue)
                    {
                        rect.Width = (int)(rect.Width * .05);
                    }
                    else
                    {
                        rect.Width = (int)(rect.Width * ((float)m_AuditedServers / (float)m_LicensedServers));
                    }
                    e.Graphics.FillRectangle(Brushes.Gold, rect);
                }
                else if (m_AuditedServers > 0)
                {
                    e.Graphics.FillRectangle(Brushes.Crimson, rect);
                    rect.Width = (int)(rect.Width * ((float)m_LicensedServers / (float)m_AuditedServers));
                    e.Graphics.FillRectangle(Brushes.Gold, rect);

                }
            }
        }

        private void label_RemainingLicensesBar_Paint(object sender, PaintEventArgs e)
        {
            if (m_LicensedServers != 0)
            {
                Rectangle rect = _label_RemainingLicensesBar.ClientRectangle;
                int remaining = m_LicensedServers - m_AuditedServers;
                if (remaining >= 0)
                {
                    rect.Width = (int)(rect.Width * ((float)remaining / (float)m_LicensedServers));
                    e.Graphics.FillRectangle(Brushes.Green, rect);
                }
                else
                {
                    remaining = m_AuditedServers - m_LicensedServers;
                    rect.Width = (int)(rect.Width * ((float)remaining / (float)m_AuditedServers));
                    rect.Offset(_label_LicenseBar.ClientRectangle.Width - rect.Width, 0);
                    e.Graphics.FillRectangle(Brushes.Crimson, rect);
                }
            }
        }

        #region Grid events

        private void _grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colIcon].Width = 22;

            //e.Layout.Bands[0].Columns[colServer].Header.ToolTipText = Utility.Snapshot.ToopTipStatus;
            e.Layout.Bands[0].Columns[colServer].Width = 200;

            //e.Layout.Bands[0].Columns[colStatus].Header.ToolTipText = Utility.Snapshot.ToopTipStatus;
            e.Layout.Bands[0].Columns[colStatus].Width = 72;

            e.Layout.Bands[0].Columns[colLastCollectionTime].Format = Utility.Constants.DATETIME_FORMAT;

            e.Layout.Bands[0].Columns[colCurrentCollectionTime].Format = Utility.Constants.DATETIME_FORMAT;

            if (_grid.Visible)
            {
                _grid.Focus();
            }
        }

        private void _grid_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            updateContextMenuAndSetMenuConfiguration();
        }

        private void _grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_gridCellClicked && e.KeyCode == Keys.Delete)
            {
                showDelete();
            }
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
                    m_gridCellClicked = true;
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
                    m_gridCellClicked = false;
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

            updateContextMenuAndSetMenuConfiguration();
        }

        private void _grid_DoubleClick(object sender, EventArgs e)
        {
            if (m_gridCellClicked && _grid.ActiveRow != null)
            {
                showProperties();
            }
        }

        #endregion

        #region Context Menu events

        private void _contextMenuStrip_Server_Opening(object sender, CancelEventArgs e)
        {
            bool isServer = (_grid.Selected.Rows.Count == 1);
            bool canExplore = false;
            if (isServer)
            {
                Sql.RegisteredServer server = Program.gController.Repository.RegisteredServers.Find(_grid.Selected.Rows[0].Cells[colServer].Text);

                canExplore = (server.LastCollectionSnapshotId != 0);
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

            Sql.RegisteredServer server = Program.gController.Repository.RegisteredServers.Find(_grid.Selected.Rows[0].Cells[colServer].Text);

            Program.gController.SetCurrentServer(server);

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_registerSQLServer_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            registerServer(sender, e);

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

            configureServer();

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

            ShowRefresh();

            Cursor = Cursors.Default;
        }

        private void _cmsi_Server_properties_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showProperties();

            Cursor = Cursors.Default;
        }

        #endregion

        #endregion
    }
}

