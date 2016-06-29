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

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_ExploreSnapshots : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {
        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Title = String.Format(Utility.Constants.ViewTitle_ExploreSnapshots, contextIn.Name);

            //Get the server info and snapshot list and load the datasource for the grid
            m_serverInstance = ((Data.ExploreSnapshots)contextIn).ServerInstance;

            m_dt_snapshots.Columns.Add(colIcon, typeof(Image));
            m_dt_snapshots.Columns.Add(colId, typeof(int));
            m_dt_snapshots.Columns.Add(colStartDate, typeof(DateTime));
            m_dt_snapshots.Columns.Add(colStartTime, typeof(DateTime));
            m_dt_snapshots.Columns.Add(colAutomated, typeof(Image));
            m_dt_snapshots.Columns.Add(colStatus, typeof(String));
            m_dt_snapshots.Columns.Add(colComments, typeof(String));
            m_dt_snapshots.Columns.Add(colBaseline, typeof(Image));
            m_dt_snapshots.Columns.Add(colBaselineComments, typeof(String));
            m_dt_snapshots.Columns.Add(colObjects, typeof(int));
            m_dt_snapshots.Columns.Add(colPermissions, typeof(int));
            m_dt_snapshots.Columns.Add(colLogins, typeof(int));
            m_dt_snapshots.Columns.Add(colWindowsGroupMembers, typeof(int));

            loadDataSource();
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ExplorePermissionsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.ExplorePermissionsConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion

        #region fields

        private Sql.RegisteredServer m_serverInstance;
        private Sql.Snapshot.SnapshotList m_snapshots;
        private DataTable m_dt_snapshots = new DataTable();

        #endregion

        #region Ctors

        public View_ExploreSnapshots() : base()
        {
            InitializeComponent();

            // Initialize base class fields.
            this._label_Summary.Text = Utility.Constants.ViewSummary_ExploreSnapshots;
            m_menuConfiguration = new Utility.MenuConfiguration();
        }

        #endregion

        #region Columns

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
        private const string colWindowsGroupMembers = @"Group Members";

        #endregion

        #region Helpers

        private void loadDataSource()
        {
            m_snapshots = Snapshot.LoadSnapshots(m_serverInstance.ConnectionName);
            m_dt_snapshots.Clear();
            Image iconImage;
            Image automatedImage;
            Image baselineImage;
            string status;
            foreach (Snapshot snap in m_snapshots)
            {
                switch (snap.Status)
                {
                    case Utility.Snapshot.StatusInProgress:
                        iconImage = _imageList.Images[0];
                        status = Utility.Snapshot.StatusInProgressText;
                        break;
                    case Utility.Snapshot.StatusWarning:
                        iconImage = _imageList.Images[0];
                        status = Utility.Snapshot.StatusWarningText;
                        break;
                    case Utility.Snapshot.StatusError:
                        iconImage = _imageList.Images[1];
                        status = Utility.Snapshot.StatusErrorText;
                        break;
                    case Utility.Snapshot.StatusSuccessful:
                        iconImage = _imageList.Images[0];
                        status = Utility.Snapshot.StatusSuccessfulText;
                        break;
                    default:
                        //Debug.Assert(false, "Unknown status encountered");
                        DiagLog.LogWarn("Warning - unknown Snapshot status encountered", snap.Status);
                        iconImage = _imageList.Images[1];
                        status = Utility.Snapshot.StatusUnknownText;
                        break;
                }
                switch (snap.Automated)
                {
                    case Utility.Snapshot.AutomatedTrue:
                        automatedImage = _imageList.Images[3];
                        break;
                    case Utility.Snapshot.AutomatedFalse:
                        automatedImage = null;
                        break;
                    default:
                        DiagLog.LogWarn("Warning - unknown Snapshot automated indicator encountered", snap.Automated);
                        automatedImage = null;
                        break;
                }
                switch (snap.Baseline)
                {
                    case Utility.Snapshot.BaselineTrue:
                        baselineImage = _imageList.Images[2];
                        break;
                    case Utility.Snapshot.BaselineFalse:
                        baselineImage = null;
                        break;
                    default:
                        DiagLog.LogWarn("Warning - unknown Snapshot baseline indicator encountered", snap.Baseline);
                        baselineImage = null;
                        break;
                }

                m_dt_snapshots.Rows.Add( iconImage,
                                        snap.SnapshotId,
                                        snap.StartTime.Date,
                                        snap.StartTime,
                                        //snap.StartTime.TimeOfDay.Subtract(new TimeSpan(0, 0, 0, 0, snap.StartTime.Millisecond)),
                                        automatedImage,
                                        status,
                                        snap.SnapshotComment,
                                        baselineImage,
                                        snap.BaselineComment,
                                        snap.NumObject,
                                        snap.NumPermission,
                                        snap.NumLogin,
                                        snap.NumWindowsGroupMember);
            }
            m_dt_snapshots.DefaultView.Sort = colStartTime + " Desc";

            this._grid.BeginUpdate();
            this._grid.DataSource = m_dt_snapshots.DefaultView;
            this._grid.DataMember = "";
            this._grid.EndUpdate();
        }

        protected void setMenuConfiguration()
        {
            switch (_grid.Selected.Rows.Count)
            {
                case 0:
                    m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = false;
                    m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
                    break;
                case 1:
                    m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = true;
                    m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = true;
                    break;
                default:
                    m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = true;
                    m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = false;
                    break;
            }

            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        protected override void showDelete()
        {
            Debug.Assert(!(_grid.Selected.Rows.Count == 0), "Attempt to delete snapshots with no selections");

            Cursor = Cursors.WaitCursor;
            Snapshot.SnapshotList deleteList = new Snapshot.SnapshotList();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in _grid.Selected.Rows)
            {
                if (row.Cells["Status"].Text == "I")
                {
                    deleteList.Clear();
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.DeleteSnapshotCaption, Utility.ErrorMsgs.DeleteSnapshotInProgressMsg);
                    Cursor = Cursors.Default;
                    return;
                }
                else
                {
                    deleteList.Add(new Snapshot(Convert.ToInt32(row.Cells[colId].Text),
                                                (DateTime)row.Cells[colStartTime].Value,
                                                //((DateTime)row.Cells[colStartDate].Value).Add((TimeSpan)row.Cells[colStartTime].Value),
                                                row.Cells[colBaseline].Text)
                                    );
                }
            }

            if (deleteList.Count > 0)
            {
                Forms.Form_DeleteSnapshot.Process(deleteList);

                Cursor = Cursors.WaitCursor;

                loadDataSource();
            }
            else
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.DeleteSnapshotCaption, Utility.ErrorMsgs.DeleteSnapshotNoSelectionMsg);
            }

            Cursor = Cursors.Default;
        }

        protected override void showProperties()
        {
            Debug.Assert(!(_grid.Selected.Rows.Count == 0), "Attempt to show snapshot properties with no selections");
            Debug.Assert(!(_grid.Selected.Rows.Count > 1), "Attempt to show snapshot properties for multiple selections");

            Cursor = Cursors.WaitCursor;

            Cursor = Cursors.Default;
        }

        protected override void showRefresh()
        {
            Cursor = Cursors.WaitCursor;

            loadDataSource();

            Cursor = Cursors.Default;
        }

        protected override void showPermissions()
        {
            Debug.Assert (_grid.ActiveRow != null);

            Cursor = Cursors.WaitCursor;

            int snapshotId = (int)_grid.ActiveRow.Cells[colId].Value;

            Program.gController.ShowChildView(new Utility.NodeTag(new Data.PermissionExplorer(m_serverInstance, snapshotId),
                                                        Utility.View.PermissionExplorer));

            Cursor = Cursors.Default;
        }

        #endregion

        #region Events

        #region View events

        private void View_ExploreSnapshots_Enter(object sender, EventArgs e)
        {
            setMenuConfiguration();
        }

        private void View_ExploreSnapshots_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new Utility.MenuConfiguration());
        }
        
        #endregion

        #region Grid events

        private void _grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colIcon].Width = 22;

            e.Layout.Bands[0].Columns[colId].Hidden = true;

            e.Layout.Bands[0].Columns[colStartDate].Header.ToolTipText = Utility.Snapshot.ToopTipDate;
            e.Layout.Bands[0].Columns[colStartDate].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colStartDate].Format = Utility.Constants.DATE_FORMAT;
            e.Layout.Bands[0].Columns[colStartDate].Width = 65;

            e.Layout.Bands[0].Columns[colStartTime].Header.ToolTipText = Utility.Snapshot.ToopTipTime;
            e.Layout.Bands[0].Columns[colStartTime].GroupByMode = Infragistics.Win.UltraWinGrid.GroupByMode.Hour;
            e.Layout.Bands[0].Columns[colStartTime].Format = Utility.Constants.TIME_FORMAT;
            e.Layout.Bands[0].Columns[colStartTime].Width = 69;

            e.Layout.Bands[0].Columns[colAutomated].Header.Caption = "";
            e.Layout.Bands[0].Columns[colAutomated].Header.ToolTipText = Utility.Snapshot.ToopTipAutomated;
            e.Layout.Bands[0].Columns[colAutomated].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colAutomated].Width = 22;

            e.Layout.Bands[0].Columns[colStatus].Header.ToolTipText = Utility.Snapshot.ToopTipStatus;
            e.Layout.Bands[0].Columns[colStatus].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colStatus].Width = 72;

            e.Layout.Bands[0].Columns[colComments].Header.ToolTipText = Utility.Snapshot.ToopTipComment;

            e.Layout.Bands[0].Columns[colBaseline].Header.ToolTipText = Utility.Snapshot.ToopTipBaseline;
            e.Layout.Bands[0].Columns[colBaseline].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colBaseline].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colBaseline].Width = 66;

            e.Layout.Bands[0].Columns[colBaselineComments].Header.ToolTipText = Utility.Snapshot.ToopTipBaselineComment;
            e.Layout.Bands[0].Columns[colBaselineComments].Hidden = true;

            e.Layout.Bands[0].Columns[colObjects].Header.ToolTipText = Utility.Snapshot.ToopTipObjects;

            e.Layout.Bands[0].Columns[colPermissions].Header.ToolTipText = Utility.Snapshot.ToopTipPermissions;

            e.Layout.Bands[0].Columns[colLogins].Header.ToolTipText = Utility.Snapshot.ToopTipLogins;

            e.Layout.Bands[0].Columns[colWindowsGroupMembers].Header.ToolTipText = Utility.Snapshot.ToopTipGroupMembers;
            e.Layout.Bands[0].Columns[colWindowsGroupMembers].Width = 90;
        }

        private void _grid_Enter(object sender, EventArgs e)
        {
            // make the active row a selected one on entry, because it is not on grid init
            // and shouldn't be until the grid gets focus
            if (_grid.ActiveRow != null)
            {
                _grid.ActiveRow.Selected = true;
            }
            _grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = SystemColors.Highlight;
            _grid.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.White;
            // make sure multiple selections show as well
            _grid.DisplayLayout.Override.SelectedRowAppearance.BackColor = SystemColors.Highlight;
            _grid.DisplayLayout.Override.SelectedRowAppearance.ForeColor = Color.White;
        }

        private void _grid_Leave(object sender, EventArgs e)
        {
            // change the visual indicators of active & selected when losing focus
            // so it won't confuse the user by appearing to be current
            if (_grid.ActiveRow != null)
            {
                if (_grid.ActiveRow.Selected)
                {
                    _grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.LightGray;
                }
                else
                {
                    if (_grid.ActiveRow.Index % 2 == 1)
                    {
                        _grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = _grid.DisplayLayout.Override.RowAlternateAppearance.BackColor;
                    }
                    else
                    {
                        _grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = _grid.DisplayLayout.Override.RowAppearance.BackColor;
                    }
                }
            }
            _grid.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
            
            _grid.DisplayLayout.Override.SelectedRowAppearance.BackColor = Color.LightGray;
            _grid.DisplayLayout.Override.SelectedRowAppearance.ForeColor = Color.Black;
        }

        private void _grid_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            setMenuConfiguration();
        }

        #endregion

        #region Context Menu events
        private void _contextMenuStrip_Snapshot_Opening(object sender, CancelEventArgs e)
        {
            bool oneRow = _grid.Selected.Rows.Count.Equals(1);
            bool noRows = _grid.Selected.Rows.Count.Equals(0);

            this._cmsi_Snapshot_exploreUserPermissions.Enabled = oneRow && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.UserPermissions);
            this._cmsi_Snapshot_exploreSnapshot.Enabled = oneRow && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.ObjectPermissions);
            this._cmsi_Snapshot_collectData.Enabled = oneRow && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Collect);
            this._cmsi_Snapshot_baselineSnapshot.Enabled = oneRow && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Baseline);
            this._cmsi_Snapshot_deleteSnapshot.Enabled = !noRows && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Delete);
            this._cmsi_Snapshot_refresh.Enabled = Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Refresh);
            this._cmsi_Snapshot_properties.Enabled = oneRow && Program.gController.Permissions.hasSecurity(Utility.Security.Functions.Properties);
        }

        private void _cmsi_Snapshot_exploreUserPermissions_Click(object sender, EventArgs e)
        {
            showPermissions();
        }

        private void _cmsi_Snapshot_exploreSnapshot_Click(object sender, EventArgs e)
        {

        }

        private void _cmsi_Snapshot_collectData_Click(object sender, EventArgs e)
        {

        }

        private void _cmsi_Snapshot_baselineSnapshot_Click(object sender, EventArgs e)
        {

        }

        private void _cmsi_Snapshot_deleteSnapshot_Click(object sender, EventArgs e)
        {
            showDelete();
        }

        private void _cmsi_Snapshot_refresh_Click(object sender, EventArgs e)
        {
            showRefresh();
        }

        private void _cmsi_Snapshot_properties_Click(object sender, EventArgs e)
        {
            showProperties();
        } 
        #endregion

        #endregion
    }
}
