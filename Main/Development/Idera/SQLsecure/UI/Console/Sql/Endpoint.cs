/******************************************************************
 * Name: Endpoint.cs
 *
 * Description: Encapsulates SQL Server endpoint object.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Idera.SQLsecure.UI.Console.Sql
{
    class Endpoint
    {
        #region Fields

        private int m_Id;
        private string m_Name;
        private string m_EndpointType;
        private string m_PrincipalName;
        private string m_Protocol;
        private bool m_IsAdminEndpoint;

        #endregion

        #region Queries

        // Get endpoints saved in a specific snapshot.
        private const string QueryGetSnapshotEndpoints
                                = @"SELECT 
 	                                    snapshotid, 
	                                    endpointid, 
	                                    principalname,
	                                    name, 
	                                    type, 
	                                    protocol, 
	                                    state, 
	                                    isadminendpoint 
                                    FROM SQLsecure.dbo.vwendpoint
                                    WHERE snapshotid = @snapshotid";
        private const string QueryGetSnapshotEndpoint
                                = @"SELECT 
 	                                    snapshotid, 
	                                    endpointid, 
	                                    principalname,
	                                    name, 
	                                    type, 
	                                    protocol, 
	                                    state, 
	                                    isadminendpoint 
                                    FROM SQLsecure.dbo.vwendpoint
                                    WHERE snapshotid = @snapshotid AND endpointid = @endpointid";
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamEndpointid = "endpointid";
        private enum Columns
        {
            SnapshotId = 0,
            EndpointId,
            PrincipalName,
            Name,
            Type,
            Protocol,
            State,
            IsAdminEndpoint
        };

        #endregion

        #region Helpers

        private static string yesNo(bool flag)
        {
            return (flag ? "Yes" : "No");
        }

        #endregion

        #region Ctors

        public Endpoint(
                SqlInt32 id,
                SqlString name,
                SqlString type,
                SqlString principalname,
                SqlString protocol,
                SqlString isadminendpoint
            )
        {
            m_Id = id.IsNull ? -1 : id.Value;
            m_Name = name.IsNull ? string.Empty : name.Value;
            m_EndpointType = type.IsNull ? string.Empty : type.Value;
            m_PrincipalName = principalname.IsNull ? string.Empty : principalname.Value;
            m_Protocol = protocol.IsNull ? string.Empty : protocol.Value;
            m_IsAdminEndpoint = ObjectType.ToBoolean(isadminendpoint);
        }

        #endregion

        #region Properties

        public int Id
        {
            get { return m_Id; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public string EndpointType
        {
            get { return m_EndpointType; }
        }

        public string PrincipalName
        {
            get { return m_PrincipalName; }
        }

        public string Protocol
        {
            get { return m_Protocol; }
        }

        public bool IsAdminEndpoint
        {
            get { return m_IsAdminEndpoint; }
        }

        public string IsAdminEndpointStr
        {
            get { return yesNo(m_IsAdminEndpoint); }
        }

        #endregion

        #region Methods

        public static List<Endpoint> GetSnapshotEndpoints(
                int snapshotid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            List<Endpoint> endpoints = new List<Endpoint>();

            // Open connection to repository and endpoints.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for endpoints.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotEndpoints, new SqlParameter[] { paramSnapshotid }))
                {
                    while (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)Columns.EndpointId);
                        SqlString name = rdr.GetSqlString((int)Columns.Name);
                        SqlString type = rdr.GetSqlString((int)Columns.Type);
                        SqlString principalname = rdr.GetSqlString((int)Columns.PrincipalName);
                        SqlString protocol = rdr.GetSqlString((int)Columns.Protocol);
                        SqlString isadminendpoint = rdr.GetSqlString((int)Columns.IsAdminEndpoint);

                        // Create the endpoint and add to list.
                        Endpoint e = new Endpoint(id, name, type, principalname, protocol, isadminendpoint);
                        endpoints.Add(e);
                    }
                }
            }

            return endpoints;
        }

        public static Endpoint GetSnapshotEndpoint(
                int snapshotid,
                int endpointid
            )
        {
            Debug.Assert(snapshotid != 0);

            // Init return.
            Endpoint endpoint = null;

            // Open connection to repository.
            using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
            {
                // Open the connection.
                connection.Open();

                // Query the repository for endpoint.
                SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                SqlParameter paramEndpointid = new SqlParameter(ParamEndpointid, endpointid);
                using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                            QueryGetSnapshotEndpoint, new SqlParameter[] { paramSnapshotid, paramEndpointid}))
                {
                    if (rdr.Read())
                    {
                        // Read the fields.
                        SqlInt32 id = rdr.GetSqlInt32((int)Columns.EndpointId);
                        SqlString name = rdr.GetSqlString((int)Columns.Name);
                        SqlString type = rdr.GetSqlString((int)Columns.Type);
                        SqlString principalname = rdr.GetSqlString((int)Columns.PrincipalName);
                        SqlString protocol = rdr.GetSqlString((int)Columns.Protocol);
                        SqlString isadminendpoint = rdr.GetSqlString((int)Columns.IsAdminEndpoint);

                        // Create the endpoint and add to list.
                        endpoint = new Endpoint(id, name, type, principalname, protocol, isadminendpoint);
                    }
                }
            }

            return endpoint;
        }

        #endregion
    }
}
