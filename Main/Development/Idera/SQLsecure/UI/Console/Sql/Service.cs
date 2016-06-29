/******************************************************************
 * Name: Service.cs
 *
 * Description: Encapsulates an O/S Service.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2007 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Web.Services.Description;
using Idera.SQLsecure.Core.Accounts;

namespace Idera.SQLsecure.UI.Console.Sql
{
    class Service
    {
        // Must match enum in Collector
        public enum ServiceType
        {
            MSSQLSERVER,                // SQL Server
            SQLSERVERAGENT,             // SQL Server Agent
            msftesql,                   // SQL Server Full Text Search for 2005
            MSSQLServerOLAPService,     // SQL Server Analysis Services
            SQLBrowser,                 // SQL Server Browser
            MsDtsServer,                // SQL Server Integration Services
            ReportServer,               // SQL Server Reporting Services
            SQLWriter,                  // SQL Server VSS Writer
            MSSQLServerADHelper,        // SQL Server Active Directory Helper   
            NSService,                  // SQL Server Notification Services
            MSSearch,                   // SQL Server Full Text Search for 2000
            NSWildcard,                 // SQL Server Notification Services wildcard to search for 2005 notification services.
            MsDtsServer100,             // SQL Server Integration Services 10.0 (SQL2008)
            MSSQLServerADHelper100,     // SQL Active Directory Helper Service 10.0 (SQL2008)
            MSSQLFDLauncher,            // SQL Full-text Filter Daemon Launcher (SQL2008)
            MsDtsServer110,             // SQL Server Integration Services 11.0 (SQL2012)
            MsDtsServer120,             // SQL Server Integration Services 12.0 (SQL2013)
            MsDtsServer130,             // SQL Server Integration Services 13.0 (SQL2014)
            Unknown
        }

        #region Fields

        private ServiceType m_Type;
        private string m_Name;
        private string m_DisplayName;
        private string m_Path;
        private string m_StartupType;
        private string m_State;
        private string m_LoginName;

        #endregion

        #region Helpers

        #endregion

        #region Queries

        // Get services saved in a specific snapshot.
        private const string QueryGetSnapshotServices
                                = @"SELECT
                                        servicetype,
                                        servicename,
                                        displayname,
                                        servicepath,
                                        startuptype,
                                        state,
                                        loginname
                                    FROM SQLsecure.dbo.vwservice
                                    WHERE snapshotid = @snapshotid";
        private static string QueryGetSnapshotService
                                = QueryGetSnapshotServices + " AND servicename = @servicename";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamServiceName = "servicename";
        private enum ServiceColumns
        {
            Type = 0,
            Name,
            DisplayName,
            Path,
            StartupType,
            State,
            LoginName
        }

        #endregion

        #region Ctors

        public Service(
                SqlInt32 serviceType,
                SqlString serviceName,
                SqlString displayName,
                SqlString servicePath,
                SqlString startupType,
                SqlString state,
                SqlString loginName
            )
        {
            if (serviceType.IsNull || serviceType >= (int)ServiceType.Unknown )
            {
                m_Type = ServiceType.Unknown;
            }
            else
            {
                m_Type = (ServiceType)serviceType.Value;
            }
            m_Name = serviceName.Value;
            m_DisplayName = displayName.Value;
            m_Path = servicePath.Value;
            m_StartupType = startupType.Value;
            m_State = state.Value;
            m_LoginName = loginName.Value;
        }

        #endregion

        #region Properties

        public ServiceType Type
        {
            get { return m_Type; }
        }
        public string Name
        {
            get { return m_Name; }
        }
        public string DisplayName
        {
            get { return m_DisplayName; }
        }
        public string Path
        {
            get { return m_Path; }
        }
        public string StartupType
        {
            get { return m_StartupType; }
        }
        public string State
        {
            get { return m_State; }
        }
        public string LoginName
        {
            get { return m_LoginName; }
        }


        #endregion

        #region Methods

        public static List<Service> GetSnapshotServices(
                int snapshotid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<Service> services = new List<Service>();

            // Open connection to repository and retrieve services.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for services for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotServices, new SqlParameter[] { paramSnapshotid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 type = rdr.GetSqlInt32((int)ServiceColumns.Type);
                        SqlString name = rdr.GetSqlString((int)ServiceColumns.Name);
                        SqlString display = rdr.GetSqlString((int)ServiceColumns.DisplayName);
                        SqlString path = rdr.GetSqlString((int)ServiceColumns.Path);
                        SqlString startupType = rdr.GetSqlString((int)ServiceColumns.StartupType);
                        SqlString state = rdr.GetSqlString((int)ServiceColumns.State);
                        SqlString login = rdr.GetSqlString((int)ServiceColumns.LoginName);

                        // Create the service and add to list.
                        Service s = new Service(type, name, display, path, startupType, state, login);
                        services.Add(s);
                    }
                }
            }

            return services;
        }

        public static Service GetSnapshotService(
                int snapshotid,
                string servicename
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            Service service = null;

            // Open connection to repository and retrieve services.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for services for the server.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramServiceName = new SqlParameter(ParamServiceName, servicename);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotService, new SqlParameter[] { paramSnapshotid, paramServiceName }))
                {
                    if (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 type = rdr.GetSqlInt32((int)ServiceColumns.Type);
                        SqlString name = rdr.GetSqlString((int)ServiceColumns.Name);
                        SqlString display = rdr.GetSqlString((int)ServiceColumns.DisplayName);
                        SqlString path = rdr.GetSqlString((int)ServiceColumns.Path);
                        SqlString startupType = rdr.GetSqlString((int)ServiceColumns.StartupType);
                        SqlString state = rdr.GetSqlString((int)ServiceColumns.State);
                        SqlString login = rdr.GetSqlString((int)ServiceColumns.LoginName);

                        // Create the service.
                        service = new Service(type, name, display, path, startupType, state, login);
                    }
                }
            }

            return service;
        }

        #endregion
    }
}
