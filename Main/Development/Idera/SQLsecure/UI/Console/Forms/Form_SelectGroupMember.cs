using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Sql;

using Infragistics.Win.UltraWinGrid;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectGroupMember : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectGroupMember(int snapshotId, Sql.User user)
        {
            InitializeComponent();

            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            _grid.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _grid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            m_Snapshot = Snapshot.GetSnapShot(snapshotId);
            m_user = user;
            _label_Group.Text = @"Group " + m_user.Name;
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_SelectGroupMember");

        private Sql.Snapshot m_Snapshot;
        private Sql.User m_user;
        private string m_SelectedUser;
        private string m_SelectedType;
        private Sid m_SelectedSid;

        #endregion

        #region Properties

        public string SelectedUser
        {
            get { return m_SelectedUser; }
        }

        public Sid SelectedSid
        {
            get { return m_SelectedSid; }
        }

        public string SelectedType
        {
            get { return m_SelectedType; }
        }

        #endregion

        #region Queries, Columns & Constants

        private static string QueryGetMembers = @"SELECT name, type, sid FROM SQLsecure.dbo.vwwindowsgroupmembers WHERE snapshotid = {0} and groupsid = {1} ORDER BY name";
        private static string QueryGetOsMembers = @"SELECT name, type, sid FROM SQLsecure.dbo.vwoswindowsgroupmembers WHERE snapshotid = {0} and groupsid = {1} ORDER BY name";

        // Columns for handling the Snapshot query results
        private const string colName = @"name";
        private const string colType = @"type";
        private const string colSid = @"sid";

        private const string HeaderDisplay = "Members ({0} items)";
        private const string PrintTitle = @"Users";
        private const string PrintHeaderDisplay = "Group Members for '{0}' as of {1}";

        #endregion

        #region Helpers

        private void showHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.ViewGroupMemberHelpTopic);
        }

        private bool LoadUsers(bool osMembers)
        {
            bool isOK = true;
            try
            {
                // Open connection to repository and query group members
                logX.loggerX.InfoFormat("Retrieve Snapshot Group Members{0}", osMembers ? " for OS Windows Account" : string.Empty);

                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    string sidString = "0x";
                    foreach (Byte sidchar in m_user.Sid.BinarySid)
                    {
                        sidString += sidchar.ToString("X2");
                    }
                    string query = string.Format(osMembers ? QueryGetOsMembers : QueryGetMembers, m_Snapshot.SnapshotId, sidString);

                    //Get Users
                    // Execute stored procedure and get the users.
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        DataView dv = new DataView(ds.Tables[0]);
                        dv.Sort = colName;

                        _grid.BeginUpdate();
                        _grid.SetDataBinding(dv, null);
                        _grid.EndUpdate();

                        foreach (DataRowView drv in dv)
                        {
                            string loginType;
                            switch (drv[colType].ToString().Trim())
                            {
                                case ServerLoginTypes.SqlLogin:
                                    loginType = ServerLoginTypes.SqlLoginText;
                                    break;
                                case ServerLoginTypes.WindowsGroup:
                                    loginType = ServerLoginTypes.WindowsGroupText;
                                    break;
                                case ServerLoginTypes.WindowsUser:
                                    loginType = ServerLoginTypes.WindowsUserText;
                                    break;
                                case ServerLoginTypes.LocalGroup:
                                    loginType = ServerLoginTypes.LocalGroupText;
                                    break;
                                case ServerLoginTypes.GlobalGroup:
                                    loginType = ServerLoginTypes.GlobalGroupText;
                                    break;
                                case ServerLoginTypes.UniversalGroup:
                                    loginType = ServerLoginTypes.UniversalGroupText;
                                    break;
                                case ServerLoginTypes.DistributionGroup:
                                    loginType = ServerLoginTypes.DistributionGroupText;
                                    break;
                                case ServerLoginTypes.WellknownGroup:
                                    loginType = ServerLoginTypes.WellknownGroupText;
                                    break;
                                case ServerLoginTypes.User:
                                    loginType = ServerLoginTypes.UserText;
                                    break;
                                default:
                                    //Debug.Assert(false, "Unknown status encountered");
                                    logX.loggerX.Warn("Warning - unknown User Type encountered", drv[colType]);
                                    loginType = @"Unknown";
                                    break;
                            }
                            drv[colType] = loginType;
                        }
                    }
                    _label_Header.Text = string.Format(HeaderDisplay, _grid.Rows.Count);
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Info("ERROR - unable to load Group Members from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.GroupMembersCaption, ex.Message);
                isOK = false;
            }
            catch (Exception ex)
            {
                isOK = false;
                logX.loggerX.Info("ERROR - unable to load Group Members from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.GroupMembersCaption, Utility.ErrorMsgs.CantGetSnapshot);

            }

            return isOK;
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
                                        m_user.Name,
                                        DateTime.Now.ToShortDateString()
                                    );
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ExportToExcelCaption, Utility.ErrorMsgs.FailedToExportToExcelFile, ex);
                }
            }
        }

        #endregion

        #region Methods

        public static Sql.User GetUser(int snapshotId, Sql.User user)
        {
            return GetUser(snapshotId, user, false);
        }

        public static Sql.User GetUser(int snapshotId, Sql.User user, bool osUser)
        {
            Sql.User selectedUser = null;

            Form_SelectGroupMember form = new Form_SelectGroupMember(snapshotId, user);
            if (form.LoadUsers(osUser))
            {
                DialogResult rc = form.ShowDialog();

                if (rc == DialogResult.OK)
                {
                    selectedUser = new Sql.User(form.SelectedUser, form.SelectedSid, Sql.LoginType.WindowsLogin, User.UserSource.Snapshot);
                }
            }
            return selectedUser;
        }

        #endregion

        #region Events

        private void _button_Help_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showHelpTopic();

            Cursor = Cursors.Default;
        }

        private void _button_OK_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (_grid.ActiveRow == null)
            {
                DialogResult = DialogResult.None;
                Cursor = Cursors.Default;
            }
            else
            {
                m_SelectedUser = _grid.ActiveRow.Cells[colName].Text;
                m_SelectedType = _grid.ActiveRow.Cells[colType].Text;
                m_SelectedSid = new Sid((byte[])_grid.ActiveRow.Cells[colSid].Value);
            }
        }

        private void Form_SelectGroupMember_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            _button_Help_Click(sender, new EventArgs());
        }

        private void _toolStripButton_ColumnChooser_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showGridColumnChooser(_grid);

            Cursor = Cursors.Default;
        }

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

        private void _grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.GroupByColumnsHidden = Infragistics.Win.DefaultableBoolean.True;
            e.Layout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;

            UltraGridBand band;

            band = e.Layout.Bands[0];

            band.Columns[colName].Header.Caption = @"Name";
            band.Columns[colName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colName].Width = 230;

            band.Columns[colType].Header.Caption = @"Type";
            band.Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;

            band.Columns[colSid].Hidden = true;
            band.Columns[colSid].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        }

        private void _grid_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            _button_OK_Click(sender, e);
            DialogResult = DialogResult.OK;
        }

        private void _grid_AfterRowActivate(object sender, EventArgs e)
        {
            _button_OK.Enabled = _grid.ActiveRow != null;
        }

        #endregion
    }
}

