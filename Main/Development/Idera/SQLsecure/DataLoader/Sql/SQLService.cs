using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Management;
using System.Data.SqlClient;
using System.Data;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Core.Accounts;

namespace Idera.SQLsecure.Collector.Sql
{
    public class SQLService
    {
        // Do not change the order because these are stored in the repository
        // Be sure to update the versions to remove a new service from older versions in the SQLServices constructor below
        // Also, must match enum in Console SQL.Services
        // Also, the isp_sqlsecure_getpolicyassessment stored procedure may need to be updated to take into account the new service type
        public enum ServiceType
        {
            MSSQLSERVER,              // SQL Server
            SQLSERVERAGENT,           // SQL Server Agent
            msftesql,                 // SQL Server Full Text Search for 2005
            MSSQLServerOLAPService,   // SQL Server Analysis Services
            SQLBrowser,               // SQL Server Browser
            MsDtsServer,              // SQL Server Integration Services
            ReportServer,             // SQL Server Reporting Services
            SQLWriter,                // SQL Server VSS Writer
            MSSQLServerADHelper,      // SQL Server Active Directory Helper   
            NSService,                // SQL Server Notification Services
            MSSearch,                 // SQL Server Full Text Search for 2000
            NSWildcard,               // SQL Server Notification Services wildcard to search for 2005 notification services.
            MsDtsServer100,           // SQL Server Integration Services 10.0 (SQL2008)
            MSSQLServerADHelper100,   // SQL Active Directory Helper Service 10.0 (SQL2008)
            MSSQLFDLauncher,          // SQL Full-text Filter Daemon Launcher (SQL2008)
            MsDtsServer110,           // SQL Server Integration Services 11.0 (SQL2012)
            MsDtsServer120,           // SQL Server Integration Services 12.0 (SQL2014)
            MsDtsServer130,           // SQL Server Integration Services 13.0 (SQL2016)
        }

        private string m_Name;
        private string m_DisplayName;
        private string m_FullFilePath;
        private ServiceType m_Type;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public string DisplayName
        {
            get { return m_DisplayName; }
            set { m_DisplayName = value; }
        }
        public string FullFilePath
        {
            get { return m_FullFilePath; }
            set { m_FullFilePath = value; }
        }
        public ServiceType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
   
    }

    class SQLServices
    {
        private string m_TargetComputerName;
        private string m_TargetInstance;
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.SQLServices");

        private List<SQLService> m_possibleServices = new List<SQLService>();
        private List<SQLService> m_foundServices = new List<SQLService>();

        public List<SQLService> Services
        {
            get { return m_foundServices; }
        }

        public SQLServices(string TargetComputer, string TargetInstance, ServerVersion TargetVersion)
        {
            m_TargetComputerName = TargetComputer;
            m_TargetInstance = TargetInstance;

            // NOTE: Must match order in enum ServiceType
            string[] serviceArray = new string[]
                {
                    "MSSQLSERVER",              // SQL Server
                    "SQLSERVERAGENT",           // SQL Server Agent
                    "msftesql",                 // SQL Server Full Text Search Service for 2005
                    "MSSQLServerOLAPService",   // SQL Server Analysis Services
                    "SQLBrowser",               // SQL Server Browser
                    "MsDtsServer",              // SQL Server Integration Services
                    "ReportServer",             // SQL Server Reporting Services
                    "SQLWriter",                // SQL Server VSS Writer
                    "MSSQLServerADHelper",      // SQL Server Active Directory Helper
                    "NSService",                // SQL Server Notification Service
                    "MSSEARCH",                 // SQL Server Full Text Search Service for 2000
                    "NS$%",                     // SQL Server Notification for 2005 
                    "MsDtsServer100",           // SQL Server Integration Services 10.0 (SQL2008)
                    "MSSQLServerADHelper100",   // SQL Active Directory Helper Service 10.0 (SQL2008)
                    "MSSQLFDLauncher",          // SQL Full-text Filter Daemon Launcher (SQL2008)
                    "MsDtsServer110",           // SQL Server Integration Services 11.0 (SQL2012)
                    "MsDtsServer120",           // SQL Server Integration Services 12.0 (SQL2014)
                    "MsDtsServer130",           // SQL Server Integration Services 13.0 (SQL2016)
                };
            string[] serviceArrayInstance = new string[]
                {
                    "MSSQL$",               // SQL Server
                    "SQLAGENT$",            // SQL Server Agent
                    "msftesql$",            // SQL Server Full Text Search
                    "MSOLAP$",              // SQL Server Analysis Services
                    "ReportServer$",        // SQL Server Reporting Services
                    "NS$",                  // SQL Server Notification Service
                    "MSSQLFDLauncher$"      // SQL Full-text Filter Daemon Launcher (SQL2008)
                };

            if (!string.IsNullOrEmpty(m_TargetInstance))
            {
                serviceArray[0] = serviceArrayInstance[0] + m_TargetInstance;
                serviceArray[1] = serviceArrayInstance[1] + m_TargetInstance;
                serviceArray[2] = serviceArrayInstance[2] + m_TargetInstance;
                serviceArray[3] = serviceArrayInstance[3] + m_TargetInstance;
                serviceArray[6] = serviceArrayInstance[4] + m_TargetInstance;
                serviceArray[9] = serviceArrayInstance[5] + m_TargetInstance;
                serviceArray[14] = serviceArrayInstance[6] + m_TargetInstance; 
            }

            Type serviceType = typeof(SQLService.ServiceType);
            //exclude services not belong to target version
            foreach (SQLService.ServiceType type in Enum.GetValues(serviceType))
            {
                // Eliminate potential duplicate services when multiple versions on same machine.
                // Ideally, I would construct this differently, but I'm just filtering out now to have minimal impact on existing code.
                switch (TargetVersion)
                {
                    case ServerVersion.SQL2000:
                        // The SQL Browser and VSS Writer do not ship with 2000, but can be used by it, I think
                        if (type == SQLService.ServiceType.msftesql ||
                            type == SQLService.ServiceType.MSSQLFDLauncher ||
                            type == SQLService.ServiceType.MSSQLServerADHelper100 ||
                            type == SQLService.ServiceType.NSService ||
                            type == SQLService.ServiceType.NSWildcard ||
                            type == SQLService.ServiceType.MsDtsServer100 ||
                            type == SQLService.ServiceType.MsDtsServer110 ||
                            type == SQLService.ServiceType.MsDtsServer120 ||
                            type == SQLService.ServiceType.MsDtsServer130)
                            continue;
                        break;
                    case ServerVersion.SQL2005:
                        if (type == SQLService.ServiceType.MSSearch ||
                            type == SQLService.ServiceType.MSSQLFDLauncher ||
                            type == SQLService.ServiceType.MSSQLServerADHelper100 ||
                            type == SQLService.ServiceType.MsDtsServer100 ||
                            type == SQLService.ServiceType.MsDtsServer110 ||
                            type == SQLService.ServiceType.MsDtsServer120 ||
                            type == SQLService.ServiceType.MsDtsServer130)
                            continue;
                        break;
                    case ServerVersion.SQL2008:
                    case ServerVersion.SQL2008R2:
                        // Notification services was removed in 2008, but appears to still be able to work with it, so I'm treating it as a shared service
                        if (type == SQLService.ServiceType.MSSearch ||
                            type == SQLService.ServiceType.msftesql ||
                            type == SQLService.ServiceType.MSSQLServerADHelper ||
                            type == SQLService.ServiceType.MsDtsServer ||
                            type == SQLService.ServiceType.MsDtsServer110 ||
                            type == SQLService.ServiceType.MsDtsServer120 ||
                            type == SQLService.ServiceType.MsDtsServer130)
                            continue;
                        break;
                    case ServerVersion.SQL2012:
                        if (type == SQLService.ServiceType.MSSearch ||
                            type == SQLService.ServiceType.msftesql ||
                            type == SQLService.ServiceType.MSSQLServerADHelper ||
                            type == SQLService.ServiceType.MsDtsServer ||
                            type == SQLService.ServiceType.MsDtsServer100 ||
                            type == SQLService.ServiceType.MsDtsServer120 ||
                            type == SQLService.ServiceType.MsDtsServer130)
                            continue;
                        break;
                    case ServerVersion.SQL2014:
                        if (type == SQLService.ServiceType.MSSearch ||
                            type == SQLService.ServiceType.msftesql ||
                            type == SQLService.ServiceType.MSSQLServerADHelper ||
                            type == SQLService.ServiceType.MsDtsServer ||
                            type == SQLService.ServiceType.MsDtsServer100 ||
                            type == SQLService.ServiceType.MsDtsServer110 ||
                            type == SQLService.ServiceType.MsDtsServer130)
                            continue;
                        break;
                    case ServerVersion.SQL2016:
                        if (type == SQLService.ServiceType.MSSearch ||
                            type == SQLService.ServiceType.msftesql ||
                            type == SQLService.ServiceType.MSSQLServerADHelper ||
                            type == SQLService.ServiceType.MsDtsServer ||
                            type == SQLService.ServiceType.MsDtsServer100 ||
                            type == SQLService.ServiceType.MsDtsServer110 ||
                            type == SQLService.ServiceType.MsDtsServer120)
                            continue;
                        break;
                    default:
                        // should not occur, but collect them all, I guess
                        logX.loggerX.Error(string.Format("Unknown SQL Server version {0} gathering Target Services. Gathering services for all versions. ", TargetVersion));                            
                        break;
                }
                SQLService s = new SQLService();
                s.Name = serviceArray[(int)type];
                s.Type = type;
                m_possibleServices.Add(s);
            }
        }

        public int GetSQLServices(string repositoryConnectionString, int snapshotid)
        {
            int numWarnings = 0;
            try
            {
                // Get Service Information
                // -----------------------
                StringBuilder scopeStr = null;
                scopeStr = new StringBuilder();
                scopeStr.Append(m_TargetComputerName);
                scopeStr.Append(Idera.SQLsecure.Core.Accounts.Constants.Cimv2Root);
                // Create management scope and connect.
                ConnectionOptions options = new ConnectionOptions();
                if (Path.NonWhackPrefixComputer(m_TargetComputerName) != Environment.MachineName)
                {
                    if ((Program.TargetServer.ForceLocalStatus == Server.ForceLocalStatusEnum.Unknown) ||   // if we have already forced a local 
                        (Program.TargetServer.ForceLocalStatus == Server.ForceLocalStatusEnum.Failed))      // connection, then don't even try to
                    {                                                                                       // give a username/password
                        if (!string.IsNullOrEmpty(Program.TargetUserName))
                        {
                            options.Username = Program.TargetUserName;
                            options.Password = Program.targetUserPassword;
                        }
                    }
                }
                ManagementScope scope = new ManagementScope(scopeStr.ToString(), options);
                
                try
                {
                    scope.Connect();        //let's see if we can connect
                }
                catch (Exception ConnectionAttempException1)
                {
                    if (Program.TargetServer.ForceLocalStatus == Server.ForceLocalStatusEnum.Unknown)   // if we haven't tried making a local connection
                    {
                        logX.loggerX.Error("First connection attempt failed - retrying as a local connection.");
                        ManagementScope ForcedLocalScope = new ManagementScope(scopeStr.ToString());
                        try
                        {
                            ForcedLocalScope.Connect();
                            Program.TargetServer.ForceLocalStatus = Server.ForceLocalStatusEnum.Succeeded;
                            scope = ForcedLocalScope;
                            logX.loggerX.Info("Local connection attempt succeeded.");
                        }
                        catch (Exception ConnectionAttempException2)
                        {
                            Program.TargetServer.ForceLocalStatus = Server.ForceLocalStatusEnum.Failed;
                            logX.loggerX.Error("Local connection attempt failed.");
                        }
                    }
                }

                // Create the datatable to write to the repository.
                using (DataTable dataTable = SQLServicesDataTable.Create())
                {
                    foreach (SQLService s in m_possibleServices)
                    {
                        string queryString = string.Format(
                            "select name, displayname, pathname, state, startmode, startname from Win32_Service where name = '{0}'",
                            s.Name);
                        if(s.Name.Contains("%"))
                        {
                            // Since the LIKE operator is not available in Windows 2000, find all services 
                            // with name <= s.Name and name > s.Name + 0xFF.
                            s.Name = s.Name.Substring(0, s.Name.Length - 1);
                            string endName = s.Name + (char)0xFF;
                            //queryString = string.Format(
                            //"select name, displayname, pathname, state, startmode, startname from Win32_Service where name like '{0}'",
                            //s.Name);                            
                            queryString = string.Format(
                            "select name, displayname, pathname, state, startmode, startname from Win32_Service where name >= '{0}' and name < '{1}'",
                            s.Name, endName);
                        }
                        logX.m_logX.Info(string.Format("Running WMI Query:  {0}", queryString));
                        SelectQuery query = new SelectQuery(queryString);
                        try
                        {
                            using (
                                ManagementObjectSearcher searcher =
                                    new ManagementObjectSearcher(scope, query))
                            {
                                foreach (ManagementObject service in searcher.Get())
                                {
                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[SQLServicesDataTable.ParamSnapshotid] = snapshotid;
                                    dr[SQLServicesDataTable.ParamServiceType] = s.Type;
                                    dr[SQLServicesDataTable.ParamState] = service["State"];
                                    dr[SQLServicesDataTable.ParamName] = service["Name"];
                                    dr[SQLServicesDataTable.ParamDisplayName] = service["DisplayName"];
                                    dr[SQLServicesDataTable.ParamServicePath] = service["PathName"];
                                    dr[SQLServicesDataTable.ParamStartupType] = service["StartMode"];
                                    dr[SQLServicesDataTable.ParamLogonName] = service["startname"];
                                    dataTable.Rows.Add(dr);
                                    // Add to internal found SQL Services list
                                    SQLService foundService = new SQLService();
                                    foundService.Name = (string) service["name"];
                                    foundService.Type = s.Type;
                                    foundService.DisplayName = (string) service["DisplayName"];
                                    foundService.FullFilePath = (string) service["PathName"];
                                    if (foundService.FullFilePath[0] == '"')
                                    {
                                        foundService.FullFilePath =
                                            foundService.FullFilePath.Substring(1,
                                                                                foundService.FullFilePath.IndexOf("\"",
                                                                                                                  3) -
                                                                                1);
                                    }
                                    m_foundServices.Add(foundService);
                                    logX.m_logX.Info(string.Format("Found Service: {0}", foundService.DisplayName));
                                }
                            }
                        }
                        catch(Exception ex1)
                        {
                            numWarnings++;
                            logX.loggerX.Error(string.Format("Error Getting Target Service {0}: ", s.Name), ex1.Message);                            
                        }
                    }
                    logX.m_logX.Info("Writing SQL Service results to Repository");
                    using (SqlConnection repository = new SqlConnection(repositoryConnectionString))
                    {
                        // Open repository connection.
                        Program.ImpersonationContext wi2 = Program.SetLocalImpersonationContext();
                        try
                        {
                            repository.Open();
                            // Use bulk copy object to write to repository.
                            using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
                            {
                                // Set the destination table.
                                bcp.DestinationTableName = SQLServicesDataTable.RepositoryTable;
                                bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                                bcp.WriteToServer(dataTable);
                            }
                        }
                        catch (Exception ex)
                        {
                            numWarnings++;
                            logX.loggerX.Error("Error Writing Target Services to Repository: ", ex.Message);
                        }
                        finally
                        {
                            Program.RestoreImpersonationContext(wi2);
                        }
                    }
                }
            }            
            catch (Exception ex)
            {
                numWarnings++;
                logX.loggerX.Error("Error Getting Target Services: ", ex.Message);
            }
            finally
            {
            }
            return numWarnings;
        }
    }
}
