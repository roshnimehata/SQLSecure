using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win.UltraWinListView;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SnapshotProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private const string NoFilters = @"No filters specified";
        private const string IntroUnresolvedWindowsAccounts = @"SQLsecure was unable to collect data for the accounts listed in the table below.  This can happen when accounts are deleted or when SQLsecure does not have privileges to collect this information.  It can lead to incomplete SQL Server permissions information for Windows accounts.  Tell me more.";
        private const string IntroUnavailableDatabases = "SQLsecure was unable to collect SQL Server security data for the databases listed in the table below.   This can happen when a database is unavailable during SQLsecure data collection.   For example, a database being backed up is unavailable for data collection.  Tell me more.";
        private const int TellMeMoreLen = 12;

        private const string colIcon = "Icon";
        private const string colDomain = "Domain";
        private const string colAccount = "Account";
        private const string colType = "Type";
        private const string colDatabase = "Database";
        private const string colStatus = "Status";

        private Control m_FilterPageFocusControl;
        private string m_SnapshotName;

        #endregion

        #region Helpers

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

        private void addNoFilterLvi()
        {
            UltraListViewItem li = ultraListView_Filters.Items.Add(null, NoFilters);
            li.Tag = null;
            li.Appearance.ForeColor = Color.Red;
        }

        private void fillGeneralPage(Sql.Snapshot snapshot)
        {
            _lbl_StartTime.Text = snapshot.StartTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
            TimeSpan duration = snapshot.EndTime - snapshot.StartTime;
            _lbl_Duration.Text = duration.ToString();
            _lbl_Status.Text = snapshot.SnapshotComment;
            _lbl_Version.Text = snapshot.CollectorVersion;
            _lbl_IsBaseline.Text = snapshot.Baseline;
            _lbl_BaselineComment.Text = snapshot.BaselineComment;
            _lbl_NumObjects.Text = snapshot.NumObject.ToString();
            _lbl_NumPermissions.Text = snapshot.NumPermission.ToString();
            _lbl_NumLogins.Text = snapshot.NumLogin.ToString();
            _lbl_NumWindowsGroupMembers.Text = snapshot.NumWindowsGroupMember.ToString();
        }

        private void fillFiltersPage(List<Sql.DataCollectionFilter> filters)
        {

            // There should be some filter.
            if (filters.Count == 0)
            {
                addNoFilterLvi();
            }
            else
            {
                foreach (Sql.DataCollectionFilter filter in filters)
                {
                    UltraListViewItem li = ultraListView_Filters.Items.Add(null, filter.FilterName);
                    li.Tag = filter;
                    li.SubItems["Description"].Value = filter.Description;
                }
                ultraListView_Filters.SelectedItems.Clear();
                ultraListView_Filters.SelectedItems.Add(ultraListView_Filters.Items[0]);
                ultraListView_Filters.ActiveItem = ultraListView_Filters.Items[0];
            }

            // Set the list view selected index.
            ultraListView_Filters.Select();
        }

        private void fillSuspectAccountsPage(Sql.Snapshot snapshot)
        {
            // Get a list of suspect windows accounts.
            List<Sql.WindowsAccount> wal = Sql.WindowsAccount.GetSuspectAccounts(snapshot.SnapshotId);

            // If list is not null, fill the grid.
            if (wal != null)
            {
                _lbl_NumUnresolvedWindowsAccounts.Text = wal.Count.ToString();
                _lbl_ItemsUnresolvedWindowsAccounts.Text = wal.Count.ToString() + (wal.Count != 1 ? " Items" : " Item");

                // Create the data table.
                DataTable dt = new DataTable();
                dt.Columns.Add(colIcon, typeof(Image));
                dt.Columns.Add(colDomain, typeof(string));
                dt.Columns.Add(colAccount, typeof(string));
                dt.Columns.Add(colType, typeof(string));

                // Fill the data table.
                int numWellknownGroups = 0;
                foreach (Sql.WindowsAccount wa in wal)
                {
                    // Determine the image.
                    Image icon = null;
                    switch (wa.AccountType)
                    {
                        case Sql.WindowsAccount.Type.Group:
                        case Sql.WindowsAccount.Type.LocalGroup:
                        case Sql.WindowsAccount.Type.GlobalGroup:
                        case Sql.WindowsAccount.Type.UniversalGroup:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                            break;
                        case Sql.WindowsAccount.Type.WellKnownGroup:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsGroup);
                            ++numWellknownGroups;
                            break;
                        case Sql.WindowsAccount.Type.User:
                            icon = AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                            break;
                        default:
                            icon = AppIcons.AppImage16(AppIcons.Enum.Unknown);
                            break;
                    }
                    //AppIcons.AppImage16(AppIcons.Enum.WindowsUser);
                    dt.Rows.Add(icon, wa.Domain, wa.Account, wa.AccountTypeString);
                }

                // Set the number of wellknown groups count.
                _lbl_WellKnownGroups.Text = numWellknownGroups.ToString();

                // Update the grid.
                _ultraGridUnresolvedWindowsAccounts.BeginUpdate();
                _ultraGridUnresolvedWindowsAccounts.DataSource = dt;
                _ultraGridUnresolvedWindowsAccounts.DataMember = "";
                _ultraGridUnresolvedWindowsAccounts.EndUpdate();
            }
        }

        private void fillUnavailableDatabases(Sql.Snapshot snapshot)
        {
            // Get a list of snapshot databases.
            List<Sql.Database> list = Sql.Database.GetSnapshotDatabases(snapshot.SnapshotId);

            // Create the data table.
            DataTable dt = new DataTable();
            dt.Columns.Add(colIcon, typeof(Image));
            dt.Columns.Add(colDatabase, typeof(string));
            dt.Columns.Add(colStatus, typeof(string));

            // Fill the grid.
            int num = 0;
            Image icon = Sql.ObjectType.TypeImage16(Sql.ObjectType.TypeEnum.Database);
            foreach (Sql.Database db in list)
            {
                if (!db.IsAvailable)
                {
                    // Increment count.
                    ++num;

                    // Fill data table row.
                    dt.Rows.Add(icon, db.Name, db.Status);
                }
            }

            // Update the counts.
            _lbl_ItemsUnavailableDatabases.Text = num.ToString() + (num != 1 ? " Items" : " Item");
            _lbl_UnavailableDatabases.Text = num.ToString();

            // Update the grid.
            _ultraGridUnavailableDatabases.BeginUpdate();
            _ultraGridUnavailableDatabases.DataSource = dt;
            _ultraGridUnavailableDatabases.DataMember = "";
            _ultraGridUnavailableDatabases.EndUpdate();
        }

        private void toggleGroupByBox(bool isWindowsUnresolvedAccts)
        {
            if (isWindowsUnresolvedAccts)
            {
                _ultraGridUnresolvedWindowsAccounts.DisplayLayout.GroupByBox.Hidden = !_ultraGridUnresolvedWindowsAccounts.DisplayLayout.GroupByBox.Hidden;
            }
            else
            {
                _ultraGridUnavailableDatabases.DisplayLayout.GroupByBox.Hidden = !_ultraGridUnavailableDatabases.DisplayLayout.GroupByBox.Hidden;
            }
        }

        private void saveGrid(bool isWindowsUnresolvedAccts)
        {
            Cursor = Cursors.WaitCursor;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (isWindowsUnresolvedAccts)
                    {
                        _ultraGridExcelExporter.Export(_ultraGridUnresolvedWindowsAccounts, _saveFileDialog.FileName);
                    }
                    else
                    {
                        _ultraGridExcelExporter.Export(_ultraGridUnavailableDatabases, _saveFileDialog.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
            }
            Cursor = Cursors.Default;
        }

        private void printGrid(bool isWindowsUnresolvedAccts)
        {
            _ultraGridPrintDocument.DocumentName = "Snapshot - " + m_SnapshotName + "  Suspect Windows Accounts";
            if (isWindowsUnresolvedAccts)
            {
                _ultraGridPrintDocument.Grid = _ultraGridUnresolvedWindowsAccounts;
            }
            else
            {
                _ultraGridPrintDocument.Grid = _ultraGridUnavailableDatabases;
            }
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;
            _ultraPrintPreviewDialog.ShowDialog();
        }

        #endregion

        #region Ctors

        public Form_SnapshotProperties(
                Sql.Snapshot snapshot,
                List<Sql.DataCollectionFilter> filters
            )
        {
            Debug.Assert(snapshot != null);
            Debug.Assert(filters != null);

            InitializeComponent();

            // Set minimum size & icon.
            this.MinimumSize = this.Size;

            // Set form text based on snapshot name.
            m_SnapshotName = snapshot.SnapshotName;
            this.Text = "Snapshot Properties - " + m_SnapshotName;

            // Set the icons.
            _ultraGridUnresolvedWindowsAccounts.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _tsbtn_GroupByBoxUnresolvedWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _tsbtn_PrintUnresolvedWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_SaveAsUnresolvedWindowsAccounts.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _ultraGridUnavailableDatabases.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _tsbtn_GroupByBoxUnavailableDatabases.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _tsbtn_PrintUnavailableDatabases.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_SaveAsUnavailableDatabases.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);

            // Setup missing windows accounts and unavailable database intro.
            _lbl_IntroUnresolvedWindowsAccounts.Text = IntroUnresolvedWindowsAccounts;
            _lbl_IntroUnresolvedWindowsAccounts.LinkArea = new LinkArea(IntroUnresolvedWindowsAccounts.Length - TellMeMoreLen - 1, TellMeMoreLen);
            _lbl_IntroUnavailableDatabases.Text = IntroUnavailableDatabases;
            _lbl_IntroUnavailableDatabases.LinkArea = new LinkArea(IntroUnavailableDatabases.Length - TellMeMoreLen - 1, TellMeMoreLen);

            // Setup the print document format.
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.FitWidthToPages = 1;

            // Fill the pages.
            fillGeneralPage(snapshot);
            fillFiltersPage(filters);
            fillSuspectAccountsPage(snapshot);
            fillUnavailableDatabases(snapshot);
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ObjectTag tag
            )
        {
            Debug.Assert(tag != null);

            // Validate input.
            if (tag == null) { return; }

            // Retrieve snapshot & its filters.
            Sql.Snapshot snapshot = null;
            List<Sql.DataCollectionFilter> filters = null;
            snapshot = Sql.Snapshot.GetSnapShot(tag.SnapshotId);
            if (snapshot != null)
            {
                if (string.Compare(snapshot.Status, Utility.Snapshot.StatusInProgress) == 0)
                {
                    Sql.RegisteredServer rServer = Program.gController.Repository.GetServer(snapshot.ConnectionName);
                    if (rServer != null)
                    {
                        rServer.ShowDataCollectionProgress();
                    }
                }
                else
                {
                    filters = Sql.DataCollectionFilter.GetSnapshotFilters(snapshot.ConnectionName, tag.SnapshotId);

                    // If snapshot retrieved, then display the form.
                    if (snapshot != null && filters != null)
                    {
                        Form_SnapshotProperties form = new Form_SnapshotProperties(snapshot, filters);
                        form.ShowDialog();
                    }
                }
            }

        }

        #endregion

        #region Event Handlers

        private void _spltcntnr_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_FilterPageFocusControl != null)
            {
                m_FilterPageFocusControl.Focus();
                m_FilterPageFocusControl = null;
            }
        }

        private void _spltcntnr_MouseDown(object sender, MouseEventArgs e)
        {
            m_FilterPageFocusControl = getFocused(this.Controls);
        }


        private void ultraListView_Filters_ItemSelectionChanged(object sender, Infragistics.Win.UltraWinListView.ItemSelectionChangedEventArgs e)
        {
            if(ultraListView_Filters.SelectedItems.Count > 0)
            {
                object tag = null;
                tag = ultraListView_Filters.SelectedItems[0].Tag;
                // Set filter details.
                _rtbx_FilterDetails.Rtf = (tag != null) ? ((Sql.DataCollectionFilter)tag).FilterDetails : "";
            }
        }


        private void _ultraGridUnresolvedWindowsAccounts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Override.CellAppearance.BorderAlpha = Alpha.Transparent;
            //e.Layout.Override.RowAppearance.BorderColor = Color.White;

            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colIcon].Width = 22;

            e.Layout.Bands[0].Columns[colDomain].Header.Caption = "Domain";
            e.Layout.Bands[0].Columns[colDomain].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDomain].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDomain].Width = 200;

            e.Layout.Bands[0].Columns[colAccount].Header.Caption = "Account";
            e.Layout.Bands[0].Columns[colAccount].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colAccount].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colAccount].Width = 200;

            e.Layout.Bands[0].Columns[colType].Header.Caption = "Type";
            e.Layout.Bands[0].Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colType].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colType].Width = 112;
        }

        private void _tsbtn_GroupByBoxUnresolvedWindowsAccounts_Click(object sender, EventArgs e)
        {
            toggleGroupByBox(true);
        }

        private void _tsbtn_SaveAsUnresolvedWindowsAccounts_Click(object sender, EventArgs e)
        {
            saveGrid(true);
        }

        private void _tsbtn_PrintUnresolvedWindowsAccounts_Click(object sender, EventArgs e)
        {
            printGrid(true);
        }

        private void _ultraGridUnavailableDatabases_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            //e.Layout.Override.CellAppearance.BorderAlpha = Alpha.Transparent;
            //e.Layout.Override.RowAppearance.BorderColor = Color.White;

            e.Layout.Bands[0].Columns[colIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colIcon].Width = 22;

            e.Layout.Bands[0].Columns[colDatabase].Header.Caption = "Database";
            e.Layout.Bands[0].Columns[colDatabase].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDatabase].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colDatabase].Width = 250;

            e.Layout.Bands[0].Columns[colStatus].Header.Caption = "Status";
            e.Layout.Bands[0].Columns[colStatus].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colStatus].AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.Bands[0].Columns[colStatus].Width = 250;
        }

        private void _ultraGridUnavailableDatabases_Enter(object sender, EventArgs e)
        {
            _ultraGridUnavailableDatabases.DisplayLayout.Override.ActiveRowAppearance.BackColor = SystemColors.Highlight;
            _ultraGridUnavailableDatabases.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.White;
        }

        private void _ultraGridUnavailableDatabases_Leave(object sender, EventArgs e)
        {
            if (_ultraGridUnavailableDatabases.ActiveRow != null)
            {
                if (_ultraGridUnavailableDatabases.ActiveRow.Selected)
                {
                    _ultraGridUnavailableDatabases.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.LightGray;
                    _ultraGridUnavailableDatabases.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
                }
            }
            else
            {
                _ultraGridUnavailableDatabases.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.White;
                _ultraGridUnavailableDatabases.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;
            }
        }

        private void _tsbtn_PrintUnavailableDatabases_Click(object sender, EventArgs e)
        {
            printGrid(false);
        }

        private void _tsbtn_SaveAsUnavailableDatabases_Click(object sender, EventArgs e)
        {
            saveGrid(false);
        }

        private void _tsbtn_GroupByBoxUnavailableDatabases_Click(object sender, EventArgs e)
        {
            toggleGroupByBox(false);
        }

        #endregion

        private void _lbl_IB_Click(object sender, EventArgs e)
        {

        }

        private void _lbl_IntroUnavailableDatabases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.MissingDatabasesHelpTopic);
        }

        private void _lbl_IntroUnresolvedWindowsAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.gController.ShowTopic(Utility.Help.MissingUsersHelpTopic);
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_SnapshotProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SnapshotPropertiesHelpTopic);
        }

       

    }
}