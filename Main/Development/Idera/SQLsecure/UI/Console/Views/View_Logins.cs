using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.Utility;
using System.Data.SqlClient;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Controls;


namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class View_Logins : Idera.SQLsecure.UI.Console.Controls.BaseView, Interfaces.IView
    {

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Title = "Logins"; // Move to constants
            loadDataSource(true);
        }
        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.LoginsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.LoginsConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return Title; }
        }

        #endregion
        

        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.View_Logins");
        private DataTable _dt_logins = new DataTable();
        private bool m_gridCellClicked = false;
        #endregion


        #region CTOR
        public View_Logins()
        {
            InitializeComponent();

            m_menuConfiguration = new Utility.MenuConfiguration();

            this._label_Summary.Text = Utility.Constants.ViewSummary_Logins;
            
            _smallTask_NewLogin.TaskText = Utility.Constants.Task_Title_NewLogins;
            _smallTask_NewLogin.TaskImage = AppIcons.AppImage32(AppIcons.Enum32.NewLogin);
            _smallTask_NewLogin.TaskHandler += new System.EventHandler(this.newLogin);


            _dt_logins.Columns.Add(colTypeIcon, typeof(Image));
            _dt_logins.Columns.Add(colHeaderName, typeof(String));
            _dt_logins.Columns.Add(colHeaderType, typeof(String));
            _dt_logins.Columns.Add(colHeaderServerAccess, typeof(String));
            _dt_logins.Columns.Add(colHeaderPermission, typeof(String));

            newLoginToolStripMenuItem.Image = AppIcons.AppImage16(AppIcons.Enum.NewSQLsecureLogin);
            deleteToolStripMenuItem.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            refreshToolStripMenuItem.Image = AppIcons.AppImage16(AppIcons.Enum.Refresh);
            propertiesToolStripMenuItem.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);

            _grid_Logins.DrawFilter = new Utility.HideFocusRectangleDrawFilter();
        }

        #endregion


        #region Queries & Columns & Constants

        private const string QueryGetLogins = "SQLsecure.dbo.isp_sqlsecure_getaccessinfo";


        // Columns for handling the login query results
        enum LoginQueryColumns
        {
            SID,
            name,
            type,
            serveraccess,
            applicationpermission
        }



        // UI Column Headings
        private const string colTypeIcon = "TypeIcon";
        private const string colHeaderName = "Name";
        private const string colHeaderType = "Type";
        private const string colHeaderServerAccess = "ServerAccess";
        private const string colHeaderPermission = "Permissions";

        private const string HeaderText = @"SQLsecure Logins";
        private const string HeaderDisplay = HeaderText + " ({0} items)";

        private const string Permissions_None = "None";
        private const string Permissions_View = "ReadOnly";
        private const string Permissions_Admin = "Permit";
        private const string Access_Grant = "Grant";
        private const string Access_Deny = "Deny";
        #endregion


        #region Overrides

        protected override void ShowRefresh()
        {
            Cursor = Cursors.WaitCursor;
            this._grid_Logins.BeginUpdate();

            string activeRowName = null;
            if (_grid_Logins.ActiveRow != null)
            {
                activeRowName = _grid_Logins.ActiveRow.Cells[colHeaderName].Text;
            }

            loadDataSource(false);

            SetActiveRow(activeRowName);

            this._grid_Logins.EndUpdate();

            Cursor = Cursors.Default;
        }

        protected override void showDelete()
        {
            if (_grid_Logins.ActiveRow != null)
            {
                string loginName = _grid_Logins.ActiveRow.Cells[colHeaderName].Text;
                deleteLogin(loginName);
                ShowRefresh();
            }
        }

        protected override void showProperties()
        {
            if (_grid_Logins.ActiveRow != null)
            {
                string loginName = _grid_Logins.ActiveRow.Cells[colHeaderName].Text;
                string access = _grid_Logins.ActiveRow.Cells[colHeaderServerAccess].Text;
                string permission = _grid_Logins.ActiveRow.Cells[colHeaderPermission].Text;
                Forms.Form_LoginProperties.Process(loginName, access, permission);
                ShowRefresh();
                SetActiveRow(loginName);
            }
        }

        protected override void showNewLogin()
        {
            string newLogin = Forms.Form_WizardNewLogin.Process();

            if (newLogin != null)
            {
                ShowRefresh();
                SetActiveRow(newLogin);
            }
        }

        #endregion


        #region Helpers

        private void loadDataSource(bool bUpdate)
        {
            _dt_logins.Clear();
            Image iconImage;
            string type = null;
            string access = null;
            string permission = null;
            try
            {
                // Retrieve activity information.
                logX.loggerX.Info("Retrieve SQLsecure logins");
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    connection.Open();
                    using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.StoredProcedure,
                                                    QueryGetLogins, null))
                    {
                        while (rdr.Read())
                        {

                            // icon and type of login
                            type = (string)rdr[(int)LoginQueryColumns.type];
                            switch (type)
                            {
                                case Utility.Logins.Login_WindowsGroup:
                                    type = Utility.Logins.Login_WindowsGroup;
                                    iconImage = Sql.ObjectType.TypeImage16(ObjectType.TypeEnum.WindowsGroupLogin);
                                    break;
                                case Utility.Logins.Login_WindowsUser:
                                    type = Utility.Logins.Login_WindowsUser;
                                    iconImage = Sql.ObjectType.TypeImage16(ObjectType.TypeEnum.WindowsUserLogin);
                                    break;
                                default:
                                    continue;
//                                    iconImage = Sql.ObjectType.TypeImage16(ObjectType.TypeEnum.SqlLogin);
//                                    type = Utility.Logins.Login_SQLLogin;
//                                    break;
                            }

                            // access
                            access = (string)rdr[(int)LoginQueryColumns.serveraccess];
                            if (string.Compare(Access_Deny, access, true) == 0)
                            {
                                access = Utility.Logins.Login_Deny;
                            }
                            else
                            {
                                access = Utility.Logins.Login_Permit;
                            }

                            // permissions
                            permission = (string)rdr[(int)LoginQueryColumns.applicationpermission];
                            if (string.Compare(permission, Permissions_Admin, true) == 0)
                            {
                                permission = Utility.Logins.Login_CanConfigure;
                            }
                            else if (string.Compare(permission, Permissions_View, true) == 0)
                            {
                                permission = Utility.Logins.Login_CanView;
                            }
                            else
                            {
                                permission = Utility.Logins.Login_None;
                            }
         

                            _dt_logins.Rows.Add(iconImage,
                                                (string)rdr[(int)LoginQueryColumns.name],
                                                type,
                                                access,
                                                permission);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to retrieve logins", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetLoginsCaption, ex);
            }

            this._grid_Logins.BeginUpdate();
            this._grid_Logins.DataSource = _dt_logins;
            this._grid_Logins.DataMember = "";
            if (bUpdate)
            {
                this._grid_Logins.EndUpdate();
            }

            label_Header.Text = string.Format(HeaderDisplay, _grid_Logins.Rows.Count.ToString());

        }

        private void updateContextMenuAndSetMenuConfiguration()
        {
            bool enableRowSelectedMenuItems = true;

            if (_grid_Logins.Selected.Rows.Count <= 0)
            {
                enableRowSelectedMenuItems = false;
            }
            if (_grid_Logins.ActiveRow != null)
            {
                if (_grid_Logins.ActiveRow.Cells[colHeaderType].Text == Utility.Logins.Login_SQLLogin)
                {
                    enableRowSelectedMenuItems = false;
                }
            }

            contextMenuStripLogins.Items["deleteToolStripMenuItem"].Enabled = enableRowSelectedMenuItems;
            contextMenuStripLogins.Items["propertiesToolStripMenuItem"].Enabled = enableRowSelectedMenuItems;

            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Remove] = enableRowSelectedMenuItems;
            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.Properties] = enableRowSelectedMenuItems;

            m_menuConfiguration.EditItems[(int)Utility.MenuItems_Edit.ConfigureDataCollection] = false;
            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);

        }

        private void deleteLogin(string loginName)
        {
            // Confirm delete
            // --------------
            string text = string.Format(Utility.ErrorMsgs.DeleteLoginWarningMsg, loginName);
            DialogResult choice =
               MessageBox.Show(text,
                                Utility.ErrorMsgs.DeleteLoginCaption,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

            if (choice == DialogResult.No)
            {
                return;
            }

            Sql.Repository.deleteLogin(loginName);
        }

        private void newLogin(object sender, EventArgs e)
        {
            showNewLogin();
        }

        private void SetActiveRow(string loginName)
        {
            if (!string.IsNullOrEmpty(loginName))
            {
                Infragistics.Win.UltraWinGrid.RowsCollection sr = _grid_Logins.Rows;
                if (sr.Count > 0)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in sr)
                    {
                        row.Selected = false;
                        if (row.Cells[colHeaderName].Text == loginName)
                        {
                            row.Selected = true;
                            _grid_Logins.ActiveRow = row;
                        }
                    }
                }
            }
        }

        #endregion


        #region Events

        #region grid

        private void _grid_Logins_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Columns[colTypeIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colTypeIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colTypeIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colTypeIcon].Width = 22;

            e.Layout.Bands[0].Columns[colHeaderName].Header.Caption = "Name";
            e.Layout.Bands[0].Columns[colHeaderName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colHeaderName].Width = 180;

            e.Layout.Bands[0].Columns[colHeaderType].Header.Caption = "Type";
            e.Layout.Bands[0].Columns[colHeaderType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colHeaderType].Width = 120;

            e.Layout.Bands[0].Columns[colHeaderServerAccess].Header.Caption = "SQL Server Access";
            e.Layout.Bands[0].Columns[colHeaderServerAccess].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colHeaderServerAccess].Width = 120;

            e.Layout.Bands[0].Columns[colHeaderPermission].Header.Caption = "Permissions in SQLsecure";
            e.Layout.Bands[0].Columns[colHeaderPermission].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;

        }

        // Make right click select row.
        // Also, make clicking off of an element clear selected row
        // --------------------------------------------------------
        private void _grid_Logins_MouseDown(object sender, MouseEventArgs e)
        {
            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;

            elementMain = this._grid_Logins.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));
            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;
                if (cell != null)
                {
                    m_gridCellClicked = true;
                    Infragistics.Win.UltraWinGrid.SelectedRowsCollection sr = _grid_Logins.Selected.Rows;
                    if (sr.Count > 0)
                    {
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in sr)
                        {
                            row.Selected = false;
                        }
                    }
                    cell.Row.Selected = true;
                    _grid_Logins.ActiveRow = cell.Row;
                }
                else
                {
                    m_gridCellClicked = false;
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;
                    if (he == null && ce == null && re == null)
                    {
                        _grid_Logins.Selected.Rows.Clear();
                        _grid_Logins.ActiveRow = null;
                    }
                }
            }

            updateContextMenuAndSetMenuConfiguration();
        }

        private void _grid_Logins_DoubleClickCell(object sender, Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs e)
        {
            if (m_gridCellClicked)
            {
                showProperties();
            }
        }

        #endregion

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowRefresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showDelete();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showProperties();
        }

        private void newLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showNewLogin();
        }

        private void contextMenuStripLogins_Opening(object sender, CancelEventArgs e)
        {
            updateContextMenuAndSetMenuConfiguration();
        }

        #endregion
    }
}

