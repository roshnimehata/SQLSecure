using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.ActiveDirectory;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Sql;

using Infragistics.Win.UltraWinGrid;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectUser : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectUser(int snapshotId, string loginType)
        {
            InitializeComponent();

            _toolStripButton_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);

            _grid.DisplayLayout.FilterDropDownButtonImage = AppIcons.AppImage16(AppIcons.Enum.GridFilter);
            _grid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            m_LoginType = loginType;

            if (m_LoginType == Sql.LoginType.WindowsLogin)
            {
                Width = 551;
                Text = @"Select Windows User";
                _button_BrowseDomain.Visible = true;
                _label_Server.Text = @"Windows Users and Groups on ";
            }
            else
            {
                Width = 351;
                Text = @"Select SQL Login";
                _button_BrowseDomain.Visible = false;
                _label_Server.Text = @"SQL Logins on ";
                _label_Descr.Visible = false;
                _panel_Users.Top = 20;
            }

            m_Snapshot = Snapshot.GetSnapShot(snapshotId);
            if (m_Snapshot != null)
            {
                _label_Server.Text += m_Snapshot.ConnectionName;
            }
        }

        #endregion

        #region Fields
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_SelectUser");
        private Sql.Snapshot m_Snapshot;
        private string m_LoginType; 
        private string m_SelectedUser;
        private string m_SelectedType;
        private Sid m_SelectedSid;
        private Sql.User.UserSource m_SelectedUserSource;

        #endregion

        #region Properties

        public string SelectedUser
        {
            get { return m_SelectedUser; }
        }

        public User.UserSource SelectedUserSource
        {
            get { return m_SelectedUserSource; }
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

        private static string QueryGetLogins = @"SELECT name, type, login, sid FROM SQLsecure.dbo.vwallaccounts WHERE snapshotid = {0} and type in ({1}) ORDER BY name";

        // Columns for handling the Snapshot query results
        private enum Col
        {
            Name = 0,
            Type,
            Login,
            Sid,
            Domain
        }
        private const string colName = @"name";
        private const string colType = @"type";
        private const string colSid = @"sid";
        private const string colLogin = @"login";
        private const string colDomain = @"domain";
        private const string colAccount = @"account";

        private const string DelimFieldSep = @"','";
        private const string Delim = @"'";
        private const string UnknownValue = @"Unknown";

        private const string HeaderWindowsUsers = @"Windows Users";
        private const string HeaderSqlUsers = @"SQL Server Users";
        private const string HeaderDisplay = "{0} ({1} items)";
        private const string PrintTitle = @"Users";
        private const string PrintHeaderDisplay = "Server Users as of {0}";

        #endregion

        #region Helpers

        private void showHelpTopic()
        {
            if (m_LoginType == Sql.LoginType.SqlLogin)
            {
                Program.gController.ShowTopic(Utility.Help.SelectSQLUserHelpTopic);
            }
            else
            {
                Program.gController.ShowTopic(Utility.Help.SelectWindowsUserHelpTopic);
            }
        }

        private void LoadUsers()
        {
            string loginType;
            string accessType;
            string domain;
            string login;

            try
            {
                // Open connection to repository and query permissions.
                logX.loggerX.Info("Retrieve Snapshot Users");

                if (m_Snapshot != null)
                {
                    using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                    {
                        // Open the connection.
                        connection.Open();

                        string query;
                        if (m_LoginType == Sql.LoginType.SqlLogin)
                        {
                            query = string.Format(QueryGetLogins, m_Snapshot.SnapshotId, Delim + Sql.LoginType.SqlLogin + Delim);
                        }
                        else
                        {
                            string groups = Delim + Sql.LoginType.WindowsUser + DelimFieldSep
                                                + Sql.LoginType.WindowsGroup + DelimFieldSep
                                                + Core.Accounts.ObjectClass.User.ToString() + DelimFieldSep
                                                + Core.Accounts.ObjectClass.Group.ToString() + DelimFieldSep
                                                + Core.Accounts.ObjectClass.LocalGroup.ToString() + DelimFieldSep
                                                + Core.Accounts.ObjectClass.GlobalGroup.ToString() + DelimFieldSep
                                                + Core.Accounts.ObjectClass.UniversalGroup.ToString() + DelimFieldSep
                                                + Core.Accounts.ObjectClass.DistributionGroup.ToString() + DelimFieldSep
                                                + Core.Accounts.ObjectClass.WellknownGroup.ToString() + Delim;
                            query = string.Format(QueryGetLogins, m_Snapshot.SnapshotId, groups);
                        }


                        //Get Users
                        // Execute stored procedure and get the users.
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.CommandType = CommandType.Text;

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            ds.Tables[0].Columns.Add(colDomain);
                            ds.Tables[0].Columns[colDomain].SetOrdinal((int)Col.Name + 1);
                            ds.Tables[0].Columns.Add(colAccount);
                            ds.Tables[0].Columns[colAccount].SetOrdinal((int)Col.Name + 2);
                            DataView dv = new DataView(ds.Tables[0]);
                            dv.Sort = colName;

                            foreach (DataRowView dvr in dv)
                            {
                                switch (dvr[colType].ToString().Trim())
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
                                        logX.loggerX.Warn("Warning - unknown User Type encountered", dvr[colType]);
                                        loginType = UnknownValue;
                                        break;
                                }
                                dvr[colType] = loginType;

                                switch (dvr[colLogin].ToString())
                                {
                                    case ServerAccessTypes.Direct:
                                        accessType = ServerAccessTypes.DirectText;
                                        break;
                                    case ServerAccessTypes.Indirect:
                                        accessType = ServerAccessTypes.IndirectText;
                                        break;
                                    default:
                                        //Debug.Assert(false, "Unknown status encountered");
                                        logX.loggerX.Warn("Warning - unknown Source (Access Type) encountered", dvr[colLogin]);
                                        accessType = UnknownValue;
                                        break;
                                }
                                dvr[colLogin] = accessType;

                                Path.SplitSamPath(dvr[colName].ToString(), out domain, out login);
                                dvr[colDomain] = domain;
                                dvr[colAccount] = login;
                            }

                            _grid.BeginUpdate();
                            _grid.SetDataBinding(dv, null);
                            _grid.EndUpdate();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("ERROR - unable to load Users from the selected Snapshot.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ex.Message);
            }

            if (_grid.Rows.Count == 0)
            {
                _button_OK.Enabled = false;
            }
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

        public static Sql.User GetUser(int snapshotId, string loginType)
        {
            Form_SelectUser form = new Form_SelectUser(snapshotId, loginType);
            form.LoadUsers();
            DialogResult rc = form.ShowDialog();

            Sql.User user = null;

            if (rc == DialogResult.OK)
            {
                user = new Sql.User(form.SelectedUser, form.SelectedSid, loginType, form.SelectedUserSource);
            }

            return user;
        }

        #endregion

        #region Events

        private void Form_SelectUser_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Cursor = Cursors.WaitCursor;

            showHelpTopic();

            Cursor = Cursors.Default;
        }

        private void _button_BrowseDomain_Click(object sender, EventArgs e)
        {
            try
            {
                ADObject[] results;
                ObjectPickerWrapper picker = new ObjectPickerWrapper();
                results = picker.ShowObjectPicker(this.Handle);
                if (results == null)
                {
                    return;
                }
                Debug.Assert(results.Length == 1);

                for (int i = 0; i <= results.Length - 1; i++)
                {
                    m_SelectedUserSource = User.UserSource.ActiveDirectory;
                    m_SelectedUser = results[i].SamAccountName;
                    m_SelectedType = results[i].ClassName;
                    m_SelectedSid = results[i].Sid;
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error - Unable to browse Domain for Users & Groups", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetUsersCaption, ex);
            }
        }

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
                m_SelectedUserSource = User.UserSource.Snapshot;
                m_SelectedUser = _grid.ActiveRow.Cells[colName].Text;
                m_SelectedType = _grid.ActiveRow.Cells[colType].Text;
                m_SelectedSid = new Sid((byte[])_grid.ActiveRow.Cells[colSid].Value);
            }
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
            band.Columns[colName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            band.Columns[colName].Hidden = true;
            band.Columns[colName].Width = 230;

            band.Columns[colDomain].Header.Caption = @"Domain";
            band.Columns[colDomain].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colDomain].Hidden = false;
            band.Columns[colDomain].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colDomain].Width = 120;

            band.Columns[colAccount].Header.Caption = @"Account";
            band.Columns[colAccount].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colAccount].Hidden = false;
            band.Columns[colAccount].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            band.Columns[colAccount].Width = 164;

            band.Columns[colType].Header.Caption = @"Type";
            band.Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            band.Columns[colType].Width = 120;

            band.Columns[colSid].Hidden = true;
            band.Columns[colSid].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            if (m_LoginType == LoginType.SqlLogin)
            {
                band.Columns[colName].Hidden = false; // show the name column

                band.Columns[colLogin].Hidden = true;
                band.Columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colDomain].Hidden = true;
                band.Columns[colDomain].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                band.Columns[colAccount].Hidden = true;
                band.Columns[colAccount].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
            else
            {
                band.Columns[colLogin].Header.Caption = @"Access";
                band.Columns[colLogin].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
                band.Columns[colLogin].Hidden = false;
                band.Columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[colLogin].Width = 78;
            }
        }

        private void _grid_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            _button_OK_Click(sender, e);
            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}

