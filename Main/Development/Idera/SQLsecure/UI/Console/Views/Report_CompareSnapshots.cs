using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;
using Snapshot=Idera.SQLsecure.UI.Console.Sql.Snapshot;

namespace Idera.SQLsecure.UI.Console.Views
{
    public partial class Report_CompareSnapshots : Idera.SQLsecure.UI.Console.Controls.BaseReport, Interfaces.IView
    {
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Views.Report_CompareSnapshots");

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            Data.Report context = (Data.Report)contextIn;

            if (context.RunReport && _button_RunReport.Enabled)
            {
                runReport();
            }
        }

        #endregion

        #region Fields

        private Sql.Snapshot.SnapshotList m_SnapshotList = new Snapshot.SnapshotList();

        #endregion

        #region Ctors

        public Report_CompareSnapshots()
        {
            InitializeComponent();
            
            //set the global title, the report title in the header graphic, and the title in the
            //status bar
            m_Title                     =
                _label_ReportTitle.Text =
                _label_Status.Text      = Utility.Constants.ReportTitle_CompareSnapshots;

            //set the description and getting started text
            _label_Description.Text = Utility.Constants.ReportSummary_CompareSnapshots;

            int i = 1;
            StringBuilder instructions = new StringBuilder(Utility.Constants.ReportRunInstructions_MultiStep);
            instructions.Append(newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_Select_Target_Instance, newline);
            instructions.AppendFormat(instructionformat, i++, Utility.Constants.ReportRunInstructions_SnapshotsToCompare, newline);
            instructions.AppendFormat(instructionformat, i, Utility.Constants.ReportRunInstructions_NoParameters, newline);
            instructions.AppendFormat(warningformat, Utility.Constants.ReportWarning_Resources);
            _label_Instructions.Text = instructions.ToString();

            _comboBox_Snapshot1.Enabled =
                _comboBox_Snapshot2.Enabled = false;
            _button_RunReport.Enabled = false;
        }

        #endregion

        #region Queries & Constants

        private const string QueryDataSource1 = @"SQLsecure.dbo.isp_sqlsecure_getsnapshotcomparison";
        private const string DataSourceName1 = @"ReportsDataset_isp_sqlsecure_getsnapshotcomparison";

        private const string QueryDataSource2 = @"SQLsecure.dbo.isp_sqlsecure_report_getcomparesnapshotinfo";
        private const string DataSourceName2 = @"ReportsDataset_isp_sqlsecure_report_getcomparesnapshotinfo";

        private const string ComboTextChooseServer = "<Choose a server to compare snapshots>";
        private const string ComboTextChooseSnapshot = "<Choose a snapshot to compare>";

        #endregion

        #region Helpers

        protected internal override void checkSelections()
        {
            _button_RunReport.Enabled = false;

            //they need to pick a server
            if ((int)_comboBox_Server.SelectedValue == 0
                || _comboBox_Snapshot1.SelectedValue == null
                || (int)_comboBox_Snapshot1.SelectedValue == 0
                || _comboBox_Snapshot2.SelectedValue == null
                || (int)_comboBox_Snapshot2.SelectedValue == 0) return;

            _button_RunReport.Enabled = true;
        }

        protected internal override void runReport()
        {
            if (_comboBox_Server.SelectedValue == null)
            {
                _comboBox_Server.SelectedItem = _comboBox_Server.Items[0];
            }
            m_ServerId = (int) _comboBox_Server.SelectedValue;
            m_ServerName = _comboBox_Server.Text;
            base.runReport();

            logX.loggerX.Info(@"Retrieve data for report Compare Snapshots");

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Setup stored procedures
                    SqlCommand cmd = new SqlCommand(QueryDataSource1, connection);
                    SqlCommand cmd2 = new SqlCommand(QueryDataSource2, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                    cmd2.CommandTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();

                    // Build parameters
                    SqlParameter paramSnapshotId1 = new SqlParameter(SqlParamSnapshotid, (int)_comboBox_Snapshot1.SelectedValue);
                    SqlParameter paramSnapshotId2 = new SqlParameter(SqlParamSnapshotid2, (int)_comboBox_Snapshot2.SelectedValue);

                    cmd.Parameters.Add(paramSnapshotId1);
                    cmd.Parameters.Add(paramSnapshotId2);

                    SqlParameter paramSnapshotId3 = new SqlParameter(SqlParamSnapshotid, (int)_comboBox_Snapshot1.SelectedValue);
                    SqlParameter paramSnapshotId4 = new SqlParameter(SqlParamSnapshotid2, (int)_comboBox_Snapshot2.SelectedValue);

                    cmd2.Parameters.Add(paramSnapshotId3);
                    cmd2.Parameters.Add(paramSnapshotId4);

                    // Get data for first dataset
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = DataSourceName1;
                    rds.Value = ds.Tables[0];
                    _reportViewer.LocalReport.DataSources.Clear();
                    _reportViewer.LocalReport.DataSources.Add(rds);

                    // Get data for second dataset
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataSet ds2 = new DataSet();
                    da2.Fill(ds2);

                    ReportDataSource rds2 = new ReportDataSource();
                    rds2.Name = DataSourceName2;
                    rds2.Value = ds2.Tables[0];
                    _reportViewer.LocalReport.DataSources.Add(rds2);
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error(@"Error - Unable to get report data", ex);
                MsgBox.ShowError(m_Title, ErrorMsgs.CantGetReportData, ex);
            }

            //add report parameters
            ReportParameter[] Param = new ReportParameter[6];
            Param[0] = new ReportParameter("ReportTitle", string.Format(Utility.Constants.REPORTS_TITLE_STR, m_Title));
            Param[1] = new ReportParameter("UserRange", m_reportDateDisplay);
            Param[2] = new ReportParameter("serverName", _comboBox_Server.Text);
            Param[3] = new ReportParameter("Expand_All", _isExpanded.ToString());
            Param[4] = new ReportParameter("SnapshotName1", _comboBox_Snapshot1.SelectedText);
            Param[5] = new ReportParameter("SnapshotName2", _comboBox_Snapshot2.SelectedText);

            _reportViewer.LocalReport.SetParameters(Param);

            // Make sure _reportViewer is created.
            // We had issues on some servers where the _reportViewer was not yet created
            // and this caused a crash.
            if (!_reportViewer.IsHandleCreated)
            {
                if (!_reportViewer.Created)
                {
                    _reportViewer.CreateControl();
                }
            }

            _reportViewer.RefreshReport();
        }

        #endregion

        #region Events

        private void Report_CompareSnapshots_Load(object sender, EventArgs e)
        {
            //start off filling server list
            _comboBox_Server_DropDown(this, null);
        }

        private void _comboBox_Server_DropDown(object sender, EventArgs e)
        {
            //Initialize the combobox for server selection to refresh on every open
            List<MyComboBoxItem> li = new List<MyComboBoxItem>();
            if (m_ServerId == 0)
            {
                li.Add(new MyComboBoxItem(ComboTextChooseServer, 0));
            }

            //Keep the last selection for the user
            string selection = _comboBox_Server.Text;

            foreach (Sql.RegisteredServer server in Program.gController.Repository.RegisteredServers)
            {
                li.Add(new MyComboBoxItem(server.ConnectionName, server.RegisteredServerId));
            }

            _comboBox_Server.DisplayMember = "Label";
            _comboBox_Server.ValueMember = "Index";
            _comboBox_Server.DataSource = li;

            //Put the last selection back for the user
            _comboBox_Server.Text = selection;
        }

        private void _comboBox_Server_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int serverId = (int)_comboBox_Server.SelectedValue;
            string selectedSnapshot1 = string.Empty;
            string selectedSnapshot2 = string.Empty;
            if (m_ServerId == serverId)
            {
                selectedSnapshot1 = _comboBox_Snapshot1.Text;
                selectedSnapshot2 = _comboBox_Snapshot2.Text;
                m_ServerId = serverId;
            }

            m_ServerId = serverId;
            // load the snapshot list for the selected server
            m_SnapshotList = Sql.Snapshot.LoadSnapshots(_comboBox_Server.Text);

            // Clear the snapshots for any previous server

            List<MyComboBoxItem> li1 = new List<MyComboBoxItem>();
            List<MyComboBoxItem> li2 = new List<MyComboBoxItem>();

            li2.Add(new MyComboBoxItem(ComboTextChooseSnapshot, 0));

            foreach (Snapshot snapshot in m_SnapshotList)
            {
                if (snapshot.HasValidPermissions)
                {
                    li1.Add(new MyComboBoxItem(snapshot.SnapshotName, snapshot.SnapshotId));
                    li2.Add(new MyComboBoxItem(snapshot.SnapshotName, snapshot.SnapshotId));
                }
            }

            _comboBox_Snapshot1.DisplayMember = "Label";
            _comboBox_Snapshot1.ValueMember = "Index";
            _comboBox_Snapshot1.DataSource = li1;
            _comboBox_Snapshot1.Text = selectedSnapshot1;

            _comboBox_Snapshot2.DisplayMember = "Label";
            _comboBox_Snapshot2.ValueMember = "Index";
            _comboBox_Snapshot2.DataSource = li2;
            _comboBox_Snapshot2.Text = selectedSnapshot2;

            _comboBox_Snapshot1.Enabled =
               _comboBox_Snapshot2.Enabled = m_SnapshotList.Count > 0;
 
            checkSelections();
        }

        private void _comboBox_Snapshot_DropDown(object sender, EventArgs e)
        {
            // Used for both snapshot combo boxes
            ComboBox comboBox = (ComboBox)sender;

            //Keep the last selection for the user
            string selection = comboBox.Text;

            List<MyComboBoxItem> li = new List<MyComboBoxItem>();

            if (((List<MyComboBoxItem>)comboBox.DataSource).Count > 0
                && ((List<MyComboBoxItem>)comboBox.DataSource)[0].Index == 0)
            {
                li.Add(new MyComboBoxItem(ComboTextChooseSnapshot, 0));
            }

            //comboBox.Items.Clear();

            Sql.RegisteredServer server = Program.gController.Repository.RegisteredServers.Find(m_ServerId);
            if (server != null)
            {
                // load the snapshot list for the selected server
                m_SnapshotList = Sql.Snapshot.LoadSnapshots(_comboBox_Server.Text);
                bool selectionFound = false;

                foreach (Snapshot snapshot in m_SnapshotList)
                {
                    if (snapshot.HasValidPermissions)
                    {
                        li.Add(new MyComboBoxItem(snapshot.SnapshotName, snapshot.SnapshotId));
                        if (snapshot.SnapshotName == selection)
                        {
                            selectionFound = true;
                        }
                    }
                }

                comboBox.DisplayMember = "Label";
                comboBox.ValueMember = "Index";
                comboBox.DataSource = li;
                if (selectionFound)
                {
                    comboBox.Text = selection;
                }
            }
        }

        private void _comboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Used for both snapshot combo boxes
            checkSelections();
        }

        #endregion

        #region MyComboBoxItem

        public class MyComboBoxItem
        {
            private string _name;
            private int _value;

            public MyComboBoxItem(string name, int value)
            {
                _name = name;
                _value = value;
            }

            public string Label
            {
                get { return _name; }
                set { _name = value; }
            }
            public int Index
            {
                get { return _value; }
                set { _value = value; }
            }
        }

        #endregion
    }
}