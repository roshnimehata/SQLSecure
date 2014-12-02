using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.SQL
{
    public class AvailabilityGroup
    {
        private const string QueryAvailabilityGroups =
            @"select 
                    av.groupid,                             
                    av.name,                                
                    av.resourceid,                          
                    av.resourcegroupid,                     
                    av.failureconditionlevel,               
                    av.healthchecktimeout,                  
                    av.automatedbackuppreference,           
                    av.automatedbackuppreferencedesc,       
                    av.snapshotid,                          
                    av.servergroupId                       
                from                                        
                   SQLSecure.dbo.availabilitygroups av      
                where                                       
                    av.snapshotid = @snapshotid             
                order by                                    
                    av.name       ";                    

        private const string QueryAvailabilityGroup =
          @" select 
                    av.groupid,                             
                    av.name,                                
                    av.resourceid,                          
                    av.resourcegroupid,                     
                    av.failureconditionlevel,               
                    av.healthchecktimeout,                  
                    av.automatedbackuppreference,           
                    av.automatedbackuppreferencedesc,       
                    av.snapshotid,                          
                    av.servergroupId                       
                from                                        
                   SQLSecure.dbo.availabilitygroups av      
                where                                       
                    av.snapshotid = @snapshotid  
                   and av.groupid =@servergroupId      
                order by                                    
                    av.name "                                ;



        private const string QueryGetReplicas =@"Select 
                                                         ar.replicaid,                           
                                                         ar.serverreplicaid,                     
                                                         ar.snapshotid,                          
                                                         ar.groupid,                             
                                                         ar.replicaservername,                   
                                                         ar.ownersid,                            
                                                         ar.endpointurl,                         
                                                         ar.availabilitymode,                    
                                                         ar.availabilitymodedesc,                
                                                         ar.failovermode,                        
                                                         ar.failovermodedesc,                    
                                                     isnull(ar.createdate,'') createdate,   
                                                          isnull(ar.modifydate,'')   modifydate,
                                                         dp.name                                 
                                                     from  SQLSecure.dbo.availabilityreplicas ar  
                                                         left outer join SQLSecure.dbo.databaseprincipal dp
                                                             on ar.snapshotid = dp.snapshotid    
                                                                and dp.usersid = ar.ownersid     
                                                     where                                       
                                                         ar.snapshotid = @snapshotid                
                                                 group by 
                                                         ar.replicaid,                           
                                                         ar.serverreplicaid,                     
                                                         ar.snapshotid,                          
                                                         ar.groupid,                             
                                                         ar.replicaservername,                   
                                                         ar.ownersid,                            
                                                         ar.endpointurl,                         
                                                         ar.availabilitymode,                    
                                                         ar.availabilitymodedesc,                
                                                         ar.failovermode,                        
                                                         ar.failovermodedesc,                    
                                                         ar.createdate,                          
                                                         ar.modifydate,                          
                                                         dp.name    ";

        private const string QueryGetReplica =@" Select 
                                                         ar.replicaid,                           
                                                         ar.serverreplicaid,                     
                                                         ar.snapshotid,                          
                                                         ar.groupid,                             
                                                         ar.replicaservername,                   
                                                         ar.ownersid,                            
                                                         ar.endpointurl,                         
                                                         ar.availabilitymode,                    
                                                         ar.availabilitymodedesc,                
                                                         ar.failovermode,                        
                                                         ar.failovermodedesc,                    
                                                         isnull(ar.createdate,'') createdate,   
                                                          isnull(ar.modifydate,'')   modifydate,
                                                         dp.name                                 
                                                     from  SQLSecure.dbo.availabilityreplicas ar  
                                                         left outer join SQLSecure.dbo.databaseprincipal dp
                                                             on ar.snapshotid = dp.snapshotid    
                                                                and dp.usersid = ar.ownersid     
                                                     where                                       
                                                         ar.snapshotid = @snapshotid             
                                                 and    ar.groupid =@groupId                     
                                                 group by 
                                                         ar.replicaid,                           
                                                         ar.serverreplicaid,                     
                                                         ar.snapshotid,                          
                                                         ar.groupid,                             
                                                         ar.replicaservername,                   
                                                         ar.ownersid,                            
                                                         ar.endpointurl,                         
                                                         ar.availabilitymode,                    
                                                         ar.availabilitymodedesc,                
                                                         ar.failovermode,                        
                                                         ar.failovermodedesc,                    
                                                         ar.createdate,                          
                                                         ar.modifydate,                          
                                                         dp.name ";


        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Sql.AvailabilityGroup");
        private const string ParamSnapshotid = "snapshotid";
        private const string ParamGroupId = "groupId";
        private const string ParamReplicaGroupId = "groupId";

        private Guid _groupId;
        private string _name;
        private string _resourceId;
        private string _resourceGroupId;
        private int _failureConditionLevel;
        private int _healthCheckTimeout;
        private byte _automatedBackuppReference;
        private string _automatedBackuppReferenceDesc;
        private int _snapshotId;
        private int _serverGroupId;
        private List<AvailabilityGroupReplica> _replicas = new List<AvailabilityGroupReplica>();

        public Guid GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string ResourceId
        {
            get { return _resourceId; }
            set { _resourceId = value; }
        }

        public string ResourceGroupId
        {
            get { return _resourceGroupId; }
            set { _resourceGroupId = value; }
        }

        public int FailureConditionLevel
        {
            get { return _failureConditionLevel; }
            set { _failureConditionLevel = value; }
        }

        public int HealthCheckTimeout
        {
            get { return _healthCheckTimeout; }
            set { _healthCheckTimeout = value; }
        }

        public byte AutomatedBackuppReference
        {
            get { return _automatedBackuppReference; }
            set { _automatedBackuppReference = value; }
        }

        public string AutomatedBackuppReferenceDesc
        {
            get { return _automatedBackuppReferenceDesc; }
            set { _automatedBackuppReferenceDesc = value; }
        }

        public int SnapshotId
        {
            get { return _snapshotId; }
            set { _snapshotId = value; }
        }

        public int ServerGroupId
        {
            get { return _serverGroupId; }
            set { _serverGroupId = value; }
        }

        public List<AvailabilityGroupReplica> Replicas
        {
            get { return _replicas; }
            set { _replicas = value; }
        }


        public AvailabilityGroup(SqlGuid groupId, SqlString name, SqlString resourceId, SqlString resourceGroupId,
            SqlInt32 failureConditionLevel, SqlInt32 healthCheckTimeout, SqlByte automatedBackuppReference,
            SqlString automatedBackuppReferenceDesc, SqlInt32 snapshotId, SqlInt32 serverGroupId)
        {
            GroupId = groupId.Value;
            Name = name.Value;
            ResourceId = resourceId.Value;
            ResourceGroupId = resourceGroupId.Value;
            FailureConditionLevel = failureConditionLevel.Value;
            HealthCheckTimeout = healthCheckTimeout.Value;
            AutomatedBackuppReference = automatedBackuppReference.Value;
            AutomatedBackuppReferenceDesc = automatedBackuppReferenceDesc.Value;
            SnapshotId = snapshotId.Value;
            ServerGroupId = serverGroupId.Value;
        }

        private enum AvailabilityGroupColumns
        {
            GroupId = 0,
            Name,
            ResourceId,
            ResourceGroupId,
            FailureConditionLevel,
            HealthchecktTmeout,
            AutomatedBbackupPreference,
            AutomatedBackupPreferenceDesc,
            SnapshotId,
            ServergroupId

        };




        public static List<AvailabilityGroup> GetAvailabilityGroups(int snapshotid)
        {

            List<AvailabilityGroup> groupList = new List<AvailabilityGroup>();
            try
            {
                // Retrieve list of snapshot dbs.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString)
                    )
                {
                    // Connect to repository.
                    connection.Open();

                    // Create parameter.
                    SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);

                    // Query for 
                    //using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                    //    QueryAvailabilityGroups, new SqlParameter[] { paramSnapshotid }))
                    SqlCommand cmd = new SqlCommand(QueryAvailabilityGroups + @" ; " + QueryGetReplicas, connection);
                    cmd.Parameters.Add(paramSnapshotid);
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                    using (DataSet set = new DataSet())
                    {
                        sqlAdapter.Fill(set);
                        foreach (DataRow row in set.Tables[0].Rows)
                        {
                            AvailabilityGroup db = GetGroupFromDAtaRow(row, set);

                            groupList.Add(db);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Retrieving Availability Group  list for Snapshot", ex);
                groupList.Clear();
            }



            return groupList;
        }



        public static AvailabilityGroup GetAvailabilityGroup(int snapshotid, Guid groupid)
        {

            AvailabilityGroup group = null;
            try
            {
                // Retrieve list server objects.
                using (SqlConnection connection = new SqlConnection(Program.gController.Repository.ConnectionString))
                {
                    // Connect to repository.
                    connection.Open();

                    // Create parameter.
                    SqlParameter paramSnapshotid = new SqlParameter(ParamSnapshotid, snapshotid);
                    SqlParameter paramGroupId = new SqlParameter(ParamGroupId, groupid.ToString());

                    // Query for 
                    //using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(connection, null, CommandType.Text,
                    //    QueryAvailabilityGroups, new SqlParameter[] { paramSnapshotid }))
                    SqlCommand cmd = new SqlCommand(QueryAvailabilityGroup + @" ; " + QueryGetReplica, connection);
                    cmd.Parameters.Add(paramSnapshotid);
                    cmd.Parameters.Add(paramGroupId);
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                    using (DataSet set = new DataSet())
                    {
                        sqlAdapter.Fill(set);
                        foreach (DataRow row in set.Tables[0].Rows)
                        {
                            group = GetGroupFromDAtaRow(row, set);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Retrieving Availability Group   for Snapshot", ex);
            }

            return group;
        }


        private static AvailabilityGroup GetGroupFromDAtaRow(DataRow row, DataSet set)
        {
            AvailabilityGroup db = new AvailabilityGroup(
                new Guid(row[(int)AvailabilityGroupColumns.GroupId].ToString()),
                row[(int)AvailabilityGroupColumns.Name].ToString(),
                row[(int)AvailabilityGroupColumns.ResourceId].ToString(),
                row[(int)AvailabilityGroupColumns.ResourceGroupId].ToString(),
                int.Parse(row[(int)AvailabilityGroupColumns.FailureConditionLevel].ToString()),
                int.Parse(row[(int)AvailabilityGroupColumns.HealthchecktTmeout].ToString()),
                byte.Parse(row[(int)AvailabilityGroupColumns.AutomatedBbackupPreference].ToString()),
                row[(int)AvailabilityGroupColumns.AutomatedBackupPreferenceDesc].ToString(),
                int.Parse(row[(int)AvailabilityGroupColumns.SnapshotId].ToString()),
                int.Parse(row[(int)AvailabilityGroupColumns.ServergroupId].ToString()));
            if (set.Tables.Count > 1)
            {
                DataRow[] replicRows = set.Tables[1].Select(string.Format("groupid = '{0}'",
                    row[(int)AvailabilityGroupColumns.GroupId].ToString()));

                foreach (DataRow dataRow in replicRows)
                {
                    AvailabilityGroupReplica rep = new AvailabilityGroupReplica();
                    
                        rep.AvailabilityMode =
                            byte.Parse(dataRow[(int)AvailabilityGroupReplicaColumns.AvailabilityMode].ToString());
                        rep.CreateDate = DateTime.Parse(dataRow[(int)AvailabilityGroupReplicaColumns.CreateDate].ToString());
                        rep.AvailabilityModeDesc = dataRow[(int)AvailabilityGroupReplicaColumns.AvailabilityModeDesc].ToString();
                        rep.EndpointUrl = dataRow[(int)AvailabilityGroupReplicaColumns.EndpointUrl].ToString();
                        rep.FailoverMode = byte.Parse(dataRow[(int)AvailabilityGroupReplicaColumns.FailoverMode].ToString());
                        rep.FailoverModeDesc = dataRow[(int)AvailabilityGroupReplicaColumns.FailoverModeDesc].ToString();
                        rep.GroupId = new Guid(dataRow[(int)AvailabilityGroupReplicaColumns.GroupId].ToString());
                        rep.ModifyDate = DateTime.Parse(dataRow[(int)AvailabilityGroupReplicaColumns.ModifyDate].ToString());
                        rep.Ownersid = dataRow[(int)AvailabilityGroupReplicaColumns.Ownersid].ToString();
                        rep.ReplicaServerName = dataRow[(int)AvailabilityGroupReplicaColumns.ReplicaServerName].ToString();
                        rep.Replicaid = new Guid(dataRow[(int)AvailabilityGroupReplicaColumns.Replicaid].ToString());
                        rep.ServerreplicaId = int.Parse(dataRow[(int)AvailabilityGroupReplicaColumns.ServerreplicaId].ToString());
                        rep.SnapshotId = int.Parse(dataRow[(int)AvailabilityGroupReplicaColumns.SnapshotId].ToString());
                        rep.OwnerName = dataRow[(int)AvailabilityGroupReplicaColumns.OwnerName].ToString();
                    
                    db.Replicas.Add(rep);
                }
            }
            return db;
        }


       
    }
}
