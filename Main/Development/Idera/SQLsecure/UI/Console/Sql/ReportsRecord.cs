using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.SQL
{
    /// <summary>
    /// Support the Database Table Record for handling Reportind Services Reports links.
    /// </summary>
    public class ReportsRecord
    {
        #region fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.SQL.ReportsRecord");

        private string _reportServer;
        private string _reportServerDirectory;
        private string _reportManagerDirectory;
        private int _port;
        private bool _useSsl;
        private string _userName;
        private string _password;
        private string _repository;
        private string _targetDirectory;
        private bool _overwriteExisting;
        private bool _reportsDeployed;

        private bool m_loaded;

        #endregion

        #region Properties


        public string ReportServer
        {
            get { return _reportServer; }
            set { _reportServer = value; }
        }

        public string ReportServerDirectory
        {
            get { return _reportServerDirectory; }
            set { _reportServerDirectory = value; }
        }

        public string ReportManagerDirectory
        {
            get { return _reportManagerDirectory; }
            set { _reportManagerDirectory = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public bool UseSsl
        {
            get { return _useSsl; }
            set { _useSsl = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Repository
        {
            get { return _repository; }
            set { _repository = value; }
        }

        public string TargetDirectory
        {
            get { return _targetDirectory; }
            set
            {
                _targetDirectory = value;
                if (_targetDirectory.StartsWith("/"))
                    _targetDirectory = _targetDirectory.Substring(1);
            }
        }

        public bool OverwriteExisting
        {
            get { return _overwriteExisting; }
            set { _overwriteExisting = value; }
        }

        public bool ReportsDeployed
        {
            get { return _reportsDeployed; }
        }

        #endregion

        //-------------------------------------------------------------
        // Constructor
        //-------------------------------------------------------------
        public ReportsRecord()
        {
            _overwriteExisting = false;
            _reportsDeployed = true;

            m_loaded = false;
        }

        #region Queries, Columns and Constants

        private const string QueryGetReports = "SELECT reportserver," +
                                                          "servervirtualdirectory," +
                                                          "managervirtualdirectory," +
                                                          "port,"+
                                                          "usessl,"+
                                                          "username," +
                                                          "repository,"+
                                                          "targetdirectory " + 
                                                    "FROM SQLsecure.dbo.vwreports";
        //private const string QueryUpdateReport = "UPDATE SQLsecure.dbo.vwreports set reportserver = '{0}', reportfolder = '{1}'";
        private const string QueryUpdateReport = "SQLsecure.dbo.isp_sqlsecure_updatereportconfigureinfo";
        private const string ParamReportServer = @"@" + colReportServer;
        private const string ParamServerDir = @"@" + colServerDir;
        private const string ParamcManagerDir = @"@" + colManagerDir;
        private const string ParamPort = @"@" + colPort;
        private const string ParamSsl = @"@" + colSsl;
        private const string ParamUserName = @"@" + colUserName;
        private const string ParamRepository = @"@" + colRepository;
        private const string ParamTargetDir = @"@" + colTargetDir;

        private const string colReportServer = @"reportserver";
        private const string colServerDir = @"servervirtualdirectory";
        private const string colManagerDir = @"managervirtualdirectory";
        private const string colPort = @"port";
        private const string colSsl = @"usessl";
        private const string colUserName = @"username";
        private const string colRepository = @"repository";
        private const string colTargetDir = @"targetdirectory";

        #endregion

        #region methods


        public string GetReportServerUrl()
        {
            string prefix = _useSsl ? "https" : "http";
            string folder = _reportServerDirectory;

            // Trim slashes
            if (folder.StartsWith("/"))
                folder = folder.Substring(1);
            if (folder.EndsWith("/"))
                folder = folder.Substring(0, folder.Length - 1);

            return String.Format("{0}://{1}:{2}/{3}/", prefix, _reportServer, _port, folder);
        }

        public string GetReportManagerUrl(bool showDeployed)
        {
            string prefix = _useSsl ? "https" : "http";
            string folder = _reportManagerDirectory;

            // Trim slashes
            if (folder.StartsWith("/"))
                folder = folder.Substring(1);
            if (folder.EndsWith("/"))
                folder = folder.Substring(0, folder.Length - 1);

            string deployed = _targetDirectory.Replace("/", "%2f").Replace(" ", "+");

            if (!showDeployed)
                return String.Format("{0}://{1}:{2}/{3}/", prefix, _reportServer, _port, folder);
            else
                return String.Format("{0}://{1}:{2}/{3}/Pages/Folder.aspx?ItemPath=%2f{4}&ViewMode=List", prefix, _reportServer, _port, folder, deployed);

        }

        public bool IsAdvancedConnection()
        {
            return _port != 80 ||
               _useSsl ||
               !_reportServerDirectory.Equals("ReportServer") ||
               !_reportManagerDirectory.Equals("Reports");
        }

        #endregion

        //-------------------------------------------------------------
        // Read - read record from repository
        //-------------------------------------------------------------
        public void Read()
        {
            // Open connection to repository and get reports record.
            logX.loggerX.Info("Retrieve Reports Record");

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    //Get Users
                    // Execute stored procedure and get the users.
                    using (SqlCommand cmd = new SqlCommand(QueryGetReports, connection))
                    {
                        cmd.CommandType = CommandType.Text;

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _reportServer = ds.Tables[0].Rows[0][colReportServer].ToString();
                            _reportServerDirectory = ds.Tables[0].Rows[0][colServerDir].ToString();
                            _reportManagerDirectory = ds.Tables[0].Rows[0][colManagerDir].ToString();
                            _port = (int)ds.Tables[0].Rows[0][colPort];
                            _useSsl = (ds.Tables[0].Rows[0][colSsl] == DBNull.Value || (byte)ds.Tables[0].Rows[0][colSsl] == 0) ? false : true;
                            _userName = ds.Tables[0].Rows[0][colUserName].ToString();
                            _repository = ds.Tables[0].Rows[0][colRepository].ToString();
                            _targetDirectory = ds.Tables[0].Rows[0][colTargetDir].ToString();
                            _reportsDeployed = true;
                        }
                        else
                        {
                            _reportServer = Dns.GetHostName();
                            _reportServerDirectory = "ReportServer";
                            _reportManagerDirectory = "Reports";
                            _port = 80;
                            _useSsl = false;
                            if (String.IsNullOrEmpty(Environment.UserDomainName))
                                _userName = Environment.UserName;
                            else
                                _userName = String.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
                            _repository = Program.gController.Repository.Instance;
                            _targetDirectory = String.Format("SQLsecure {0} Reports", Utility.Constants.PRODUCT_VER_STR);
                            _reportsDeployed = false;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                logX.loggerX.Error("ERROR - unable to load Reports Record.", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.CantGetReportsRecord, ex.Message);
            }
        }

        //-------------------------------------------------------------
        // Write - write record to repository
        //-------------------------------------------------------------
        public void Write()
        {
            // Open connection to repository and save reports record.
            logX.loggerX.Info("Update Reports Record");

            try
            {
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // setup parameters
                    SqlParameter paramReportServer = new SqlParameter(ParamReportServer, SqlHelper.CreateSafeString(ReportServer.Trim(), false));
                    SqlParameter paramServerDir = new SqlParameter(ParamServerDir, SqlHelper.CreateSafeString(ReportServerDirectory.Trim(), false));
                    SqlParameter paramManagerDir = new SqlParameter(ParamcManagerDir, SqlHelper.CreateSafeString(ReportManagerDirectory.Trim(), false));
                    SqlParameter paramPort = new SqlParameter(ParamPort, Port);
                    SqlParameter paramSsl = new SqlParameter(ParamSsl, UseSsl);
                    SqlParameter paramUserName = new SqlParameter(ParamUserName, SqlHelper.CreateSafeString(UserName.Trim(), false));
                    SqlParameter paramRepository = new SqlParameter(ParamRepository, SqlHelper.CreateSafeString(Repository.Trim(), false));
                    SqlParameter paramTargetDir = new SqlParameter(ParamTargetDir, SqlHelper.CreateSafeString(TargetDirectory.Trim(), false));

                    // Execute stored procedure and get the users.
                    using (SqlCommand cmd = new SqlCommand(QueryUpdateReport, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(paramReportServer);
                        cmd.Parameters.Add(paramServerDir);
                        cmd.Parameters.Add(paramManagerDir);
                        cmd.Parameters.Add(paramPort);
                        cmd.Parameters.Add(paramSsl);
                        cmd.Parameters.Add(paramUserName);
                        cmd.Parameters.Add(paramRepository);
                        cmd.Parameters.Add(paramTargetDir);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Unable to save Reports Record", ex);
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ReportsCaption, Utility.ErrorMsgs.CantSaveReportsRecord, ex);
            }
        }
    }
}
