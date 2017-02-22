using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.SQL;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class ObjectExplorer : UserControl, Interfaces.IView, Interfaces.ICommandHandler
    {
        #region Constants

        public const string SnapshotLblPrefix = "Snapshot: ";
        public const string NoSnapshotAvailable = "No audit data available";
        public const string WindowsGroup = "G";
        public const string WindowsUser = "U";
        public const string SQLLogin = "S";

        public const string PrintHeaderDisplay = "Object Explorer for {0} on SQL Server {1}";

        #endregion

        #region Grid Stuff

        #region Columns

        private const string colTypeIcon = "TypeIcon";
        private const string colName = "Name";
        private const string colType = "Object Type";
        private const string colTag = "Tag";
        private const string colOwner = "Owner";
        private const string colSchema = "Schema";
        private const string colSchemaOwner = "Schema Owner";
        private const string colLogin = "Login";
        private const string colHasAccess = "Has Access";
        private const string colHasIsAliased = "Is Aliased";
        private const string colMemberOf = "Member Of";
        private const string colServerAccess = "Server Access";
        private const string colServerDeny = "Server Deny";
        private const string colState = "State";
        private const string colIsContainedUser = "Is User Contained";

        enum columnsToShow
        {
            Default,
            ServerLogins,
            DBUsers,
            DBRolesAndSchema,
            DBObjects,
            Files,
            RegistryKeys,
            Services
        }

        #endregion

        #region Update Grid Methods


        private void getDBObjectNames(Sql.ObjectTag tag, 
        ref string name,
        ref string owner,
        ref string schema,
        ref string schemaowner
    )
        {
            Sql.DatabaseObject.Properties p = Sql.DatabaseObject.Properties.Get(tag.SnapshotId,
                            tag.DatabaseId, tag.ClassId, tag.ParentObjectId, tag.ObjectId);
            if (p != null)
            {
                name = tag.DatabaseName
                                 + "." + (m_Version == Sql.ServerVersion.SQL2000 ? p.Owner : p.SchemaName)
                                 + "." + tag.ObjectName;
                owner = p.Owner;
                schema = p.SchemaName;
                schemaowner = p.SchemaOwner;
            }
        }


        private string getMembersOf(Sql.ObjectTag tag)
        {
            List<Sql.ServerRole> roles = Sql.Login.GetSnapshotLoginRoles(tag.SnapshotId, tag.ObjectId);
            string ret = string.Empty;
            for (int i = 0; i < roles.Count; ++i)
            {
                ret += roles[i].Name;
                if (i != (roles.Count - 1)) { ret += ", "; }
            }
            return ret;
        }

        private void fillSnapshotNode()
        {
            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Server;
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
        }

        private void fillServerNode()
        {
            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Environment;
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.NodeName, tag.NodeName, tag);

            type = Sql.ObjectType.TypeEnum.ServerSecurity;
            tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
            {
                type = Sql.ObjectType.TypeEnum.ServerObjects;
                tag = new Sql.ObjectTag(m_SnapshotId, type);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
            }

            type = Sql.ObjectType.TypeEnum.Databases;
            tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
        }

        private void fillEnvironmentNode()
        {
            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.FileSystem;
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.NodeName, tag.NodeName, tag);

            type = Sql.ObjectType.TypeEnum.Registry;
            tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.NodeName, tag.NodeName, tag);

            type = Sql.ObjectType.TypeEnum.Services;
            tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.NodeName, tag.NodeName, tag);
        }

        private void fillFileSystemObjectsNode()
        {
            try
            {
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.File;
                // Get a list of file system objects for the snapshot.
                List<Sql.FileSystemObject> fsobjects = Sql.FileSystemObject.GetSnapshotObjects(m_SnapshotId);

                // Fill the grid.
                foreach (Sql.FileSystemObject fsobject in fsobjects)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, fsobject.OsObjectId, fsobject.Name,null);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(tag.ObjType), tag.ObjectName, fsobject.TypeDescription, tag,
                                        fsobject.Owner);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotFilesFailed, ex);
            }
        }

        private void fillRegistryKeysNode()
        {
            try
            {
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.RegistryKey;
                // Get a list of registry keys for the snapshot.
                List<Sql.RegistryKey> keys = Sql.RegistryKey.GetSnapshotKeys(m_SnapshotId);

                // Fill the grid.
                foreach (Sql.RegistryKey key in keys)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, key.OsObjectId, key.Name, null);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(tag.ObjType), tag.ObjectName, tag.TypeName, tag,
                                        key.Owner);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotRegistryFailed, ex);
            }
        }

        private void fillServicesNode()
        {
            try
            {
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Service;
                // Get a list of services for the snapshot.
                List<Sql.Service> services = Sql.Service.GetSnapshotServices(m_SnapshotId);

                // Fill the grid.
                foreach (Sql.Service service in services)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, 0, service.Name, null);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(tag.ObjType), service.DisplayName, tag.TypeName, tag,
                                        null, null, null, service.LoginName, null, null, null, null, null,
                                        service.State);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotServicesFailed, ex);
            }
        }

        private void fillServerSecurityNode()
        {
            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Logins;
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            type = Sql.ObjectType.TypeEnum.ServerRoles;
            tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
        }

        private void fillLoginsNode()
        {
            try
            {
                // Get a list of logins for the snapshot.
                List<Sql.Login> logins = Sql.Login.GetSnapshotLogins(m_SnapshotId);

                // Fill the grid.
                foreach (Sql.Login login in logins)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, login.Type, login.Id, login.Name, null);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(tag.ObjType), tag.ObjectName, tag.TypeName, tag,
                                        null, null, null, null, null, null,
                                        getMembersOf(tag), login.ServerAccess, login.ServerDeny);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotLoginsFailed, ex);
            }
        }

        private void fillServerRolesNode()
        {
            try
            {
                // Get a list of server roles for the snapshot.
                List<Sql.ServerRole> roles = Sql.ServerRole.GetSnapshotServerRoles(m_SnapshotId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.ServerRole;
                foreach (Sql.ServerRole role in roles)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, role.Id, role.Name, null);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotServerRolesFailed, ex);
            }
        }

        private void fillServerObjectsNode()
        {
            Debug.Assert(m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported);

            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Endpoints;
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            if (m_Version >= ServerVersion.SQL2012 && m_Version != ServerVersion.Unsupported)
            {
                type = Sql.ObjectType.TypeEnum.AvailabilityGroups;
                tag = new Sql.ObjectTag(m_SnapshotId, type);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
            }
        }

        private void fillEndpointsNode()
        {
            try
            {
                // Get a list of endpoints.
                List<Sql.Endpoint> endpoints = Sql.Endpoint.GetSnapshotEndpoints(m_SnapshotId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Endpoint;
                foreach (Sql.Endpoint endpoint in endpoints)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, endpoint.Id, endpoint.Name, null);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotEndpointsFailed, ex);
            }
        }

        private void fillDatabasesNode()
        {
            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Database;
            foreach (Sql.Database db in m_Databases)
            {
                Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, db, 0, db.DbId, db.Name);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag);
            }
        }

        private void fillDatabaseNode(Sql.Database database)
        {
            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.DatabaseSecurity;
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            type = Sql.ObjectType.TypeEnum.Tables;
            tag = new Sql.ObjectTag(m_SnapshotId, type, database);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            type = Sql.ObjectType.TypeEnum.Views;
            tag = new Sql.ObjectTag(m_SnapshotId, type, database);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
            {
                type = Sql.ObjectType.TypeEnum.Synonyms;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
            }

            type = Sql.ObjectType.TypeEnum.StoredProcedures;
            tag = new Sql.ObjectTag(m_SnapshotId, type, database);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            type = Sql.ObjectType.TypeEnum.Functions;
            tag = new Sql.ObjectTag(m_SnapshotId, type, database);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            if (database.IsMasterDb)
            {
                type = Sql.ObjectType.TypeEnum.ExtendedStoredProcedures;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
            }

            if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
            {
                type = Sql.ObjectType.TypeEnum.Assemblies;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

                type = Sql.ObjectType.TypeEnum.UserDefinedDataTypes;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

                type = Sql.ObjectType.TypeEnum.XMLSchemaCollections;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

                type = Sql.ObjectType.TypeEnum.FullTextCatalogs;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
            }
            if (m_Version >= Sql.ServerVersion.SQL2012 && m_Version <= Sql.ServerVersion.SQL2016)
            {
                type = Sql.ObjectType.TypeEnum.SequenceObjects;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
            }
        }

        private void fillDatabaseSecurityNode(Sql.Database database)
        {
            Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Users;
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            type = Sql.ObjectType.TypeEnum.DatabaseRoles;
            tag = new Sql.ObjectTag(m_SnapshotId, type, database);
            m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

            if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
            {
                type = Sql.ObjectType.TypeEnum.Schemas;
                tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

                //type = Sql.ObjectType.TypeEnum.Keys;
                //tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                //m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);

                //type = Sql.ObjectType.TypeEnum.Certificates;
                //tag = new Sql.ObjectTag(m_SnapshotId, type, database);
                //m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.TypeName, tag.TypeName, tag);
            }
        }

        private void fillDatabaseUsersNode(Sql.Database database)
        {
            try
            {
                // Get a list of users for the snapshot.
                List<Sql.DatabasePrincipal> users = Sql.DatabasePrincipal.GetSnapshotUsers(m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.User;
                foreach (Sql.DatabasePrincipal user in users)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, user.Id, user.Name);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag,
                        null, null, null,
                        user.Login, user.HasAccess, user.IsAlias,
                        null, null, null,null,user.IsContainedUser);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotUsersFailed, ex);
            }
        }

        private void fillDatabaseRolesNode(Sql.Database database)
        {
            try
            {
                // Get a list of roles for the snapshot.
                List<Sql.DatabasePrincipal> roles = Sql.DatabasePrincipal.GetSnapshotDbRoles(m_SnapshotId, database.DbId);

                // Fill the grid.
                //Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.DatabaseRole;  Not used: each role could be different
                foreach (Sql.DatabasePrincipal role in roles)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, role.TypeEnum, database, role.Id, role.Name);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(tag.ObjType), tag.ObjectName, tag.TypeName, tag,
                                        role.Owner, null, null, null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotRolesFailed, ex);
            }
        }

        private void fillDatabaseSchemasNode(Sql.Database database)
        {
            try
            {
                // Get a list of schemas for the snapshot.
                List<Sql.Schema> schemas = Sql.Schema.GetSnapshotSchemas(m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Schema;
                foreach (Sql.Schema schema in schemas)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, schema.ClassId, schema.SchemaId, schema.SchemaName);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag,
                              schema.OwnerName, null, null, null, null, null, null, null, null);

                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotSchemasFailed, ex);
            }
        }

        private void fillDatabaseKeysNode(Sql.Database database)
        {
            //try
            //{
            //    // Get a list of keys for the snapshot.
            //    List<Sql.DatabasePrincipal> keys = Sql.DatabasePrincipal.GetSnapshotKeys(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

            //    // Fill the grid.
            //    Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Key;
            //    foreach (Sql.DatabasePrincipal key in keys)
            //    {
            //        Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, key.Id, key.Name);
            //        m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotKeysFailed, ex);
            //}
        }

        private void fillDatabaseCertificatesNode(Sql.Database database)
        {
            //try
            //{
            //    // Get a list of certs for the snapshot.
            //    List<Sql.DatabasePrincipal> certs = Sql.DatabasePrincipal.GetSnapshotCertificates(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

            //    // Fill the grid.
            //    Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Certificate;
            //    foreach (Sql.DatabasePrincipal cert in certs)
            //    {
            //        Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, cert.Id, cert.Name);
            //        m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotCertificatesFailed, ex);
            //}
        }

        private void fillDatabaseTablesNode(Sql.Database database)
        {
            try
            {
                // Get a list of tables for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotTables(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Table;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner, 
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotTablesFailed, ex);
            }
        }

        private void fillDatabaseViewsNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotViews(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.View;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);

                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotViewsFailed, ex);
            }
        }

        private void fillDatabaseSynonymsNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotSynonyms(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Synonym;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);

                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotSynonymsFailed, ex);
            }
        }

        private void fillDatabaseStoredProceduresNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotStoredProcedures(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.StoredProcedure;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotStoredProceduresFailed, ex);
            }
        }

        private void fillDatabaseFunctionsNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotFunctions(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Function;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotFunctionsFailed, ex);
            }
        }

        private void fillDatabaseExtendedStoredProceduresNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotExtendedStoredProcedures(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.ExtendedStoredProcedure;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotExtendedStoredProceduresFailed, ex);
            }
        }

        private void fillDatabaseAssembliesNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotAssemblies(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.Assembly;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotAssembliesFailed, ex);
            }
        }

        private void fillDatabaseUserDefinedDataTypesNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotUserDefinedDataTypes(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.UserDefinedDataType;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotUserDefinedDataTypeFailed, ex);
            }
        }

        private void fillDatabaseXMLSchemaCollectionsNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotXMLSchemaCollections(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.XMLSchemaCollection;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotXMLSchemaCollectionsFailed, ex);
            }
        }

        private void fillDatabaseFullTextCatalogsNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotFullTextCatalogs(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.FullTextCatalog;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotFullTextCatalogsFailed, ex);
            }
        }

        private void fillDatabaseSequenceObjectsNode(Sql.Database database)
        {
            try
            {
                // Get a list of objs for the snapshot.
                List<Sql.DatabaseObject> objs = Sql.DatabaseObject.GetSnapshotSequenceObjects(Program.gController.Repository.ConnectionString, m_SnapshotId, database.DbId);

                // Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.SequenceObject;
                foreach (Sql.DatabaseObject obj in objs)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, database, obj.ClassId, obj.ParentObjectId, obj.ObjectId, obj.Name);
                    string name = string.Empty;
                    string owner = string.Empty;
                    string schema = string.Empty;
                    string schemaowner = string.Empty;
                    getDBObjectNames(tag, ref name, ref owner, ref schema, ref schemaowner);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                                         tag.TypeName, tag, owner, schema, schemaowner,
                                         null, null, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotFullTextCatalogsFailed, ex);
            }
        }

        private void fillAvailabilityGroupReplicaNode(object obj)
        {
            try
            {
                AvailabilityGroupReplica gr = obj as AvailabilityGroupReplica;

                if (gr != null)
                {
                    //// Fill the grid.
                    Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.AvailabilityGroupReplica;


                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, gr.ServerreplicaId,
                            gr.ReplicaServerName, gr);


                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                        tag.TypeName, tag, null, null, null,
                        null, null, null, null, null, null);
                }

            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotFullTextCatalogsFailed, ex);
            }
        }

        private void fillAvailabilityGroupNode(object obj)
        {
            try
            {
                AvailabilityGroup gr = obj as AvailabilityGroup;

                if (gr != null)
                {
                    //// Fill the grid.
                    Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.AvailabilityGroupReplica;

                    foreach (AvailabilityGroupReplica replica in gr.Replicas)
                    {
                        Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, replica.ServerreplicaId,
                            replica.ReplicaServerName, replica);

                        m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName,
                            tag.TypeName, tag, null, null, null,
                            null, null, null, null, null, null);
                    }
                }

            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotFullTextCatalogsFailed, ex);
            }
        }
        private void fillAvailabilityGroupsNode()
        {
            try
            {
                //// Fill the grid.
                Sql.ObjectType.TypeEnum type = Sql.ObjectType.TypeEnum.AvailabilityGroup;
                foreach (AvailabilityGroup mAvailabilityGroup in m_availabilityGroups)
                {
                    Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, type, mAvailabilityGroup.ServerGroupId, mAvailabilityGroup.Name, mAvailabilityGroup);
                    m_DataTable.Rows.Add(Sql.ObjectType.TypeImage16(type), tag.ObjectName, tag.TypeName, tag);

                }

            }
            catch (Exception ex)
            {
                Utility.MsgBox.ShowError(Utility.ErrorMsgs.ObjectExplorerCaption, Utility.ErrorMsgs.GetSnapshotFullTextCatalogsFailed, ex);
            }
        }

        private void updateGridView(
                Sql.ObjectType.TypeEnum objType,
                Sql.Database database,
                object tagObj
            )
        {
            // Clear the data table.
            m_DataTable.Clear();           
            
            // Fill the data table based on node type.
            switch (objType)
            {
                case Sql.ObjectType.TypeEnum.Snapshot:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillSnapshotNode();
                    break;
                case Sql.ObjectType.TypeEnum.Server:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillServerNode();
                    break;
                case Sql.ObjectType.TypeEnum.Environment:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillEnvironmentNode();
                    break;
                case Sql.ObjectType.TypeEnum.FileSystem:
                    m_ColumnsToShow = columnsToShow.Files;
                    fillFileSystemObjectsNode();
                    break;
                case Sql.ObjectType.TypeEnum.Registry:
                    m_ColumnsToShow = columnsToShow.RegistryKeys;
                    fillRegistryKeysNode();
                    break;
                case Sql.ObjectType.TypeEnum.Services:
                    m_ColumnsToShow = columnsToShow.Services;
                    fillServicesNode();
                    break;
                case Sql.ObjectType.TypeEnum.ServerSecurity:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillServerSecurityNode();
                    break;
                case Sql.ObjectType.TypeEnum.Logins:
                    m_ColumnsToShow = columnsToShow.ServerLogins;
                    fillLoginsNode();
                    break;
                case Sql.ObjectType.TypeEnum.ServerRoles:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillServerRolesNode();
                    break;
                case Sql.ObjectType.TypeEnum.ServerObjects:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillServerObjectsNode();
                    break;
                case Sql.ObjectType.TypeEnum.Endpoints:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillEndpointsNode();
                    break;
                case Sql.ObjectType.TypeEnum.Databases:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillDatabasesNode();
                    break;
                case Sql.ObjectType.TypeEnum.Database:
                    m_ColumnsToShow = columnsToShow.Default;
                    if (database.IsAvailable) // If database is not available, then don't show contents
                    {
                        fillDatabaseNode(database);
                    }
                    break;
                case Sql.ObjectType.TypeEnum.DatabaseSecurity:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillDatabaseSecurityNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Users:
                    m_ColumnsToShow = columnsToShow.DBUsers;
                    fillDatabaseUsersNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.DatabaseRoles:
                    m_ColumnsToShow = columnsToShow.DBRolesAndSchema;
                    fillDatabaseRolesNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Schemas:
                    m_ColumnsToShow = columnsToShow.DBRolesAndSchema;
                    fillDatabaseSchemasNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Keys:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillDatabaseKeysNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Certificates:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillDatabaseCertificatesNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Tables:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseTablesNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Views:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseViewsNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Synonyms:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseSynonymsNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.StoredProcedures:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseStoredProceduresNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Functions:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseFunctionsNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.ExtendedStoredProcedures:
                    fillDatabaseExtendedStoredProceduresNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.Assemblies:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseAssembliesNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.UserDefinedDataTypes:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseUserDefinedDataTypesNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.XMLSchemaCollections:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseXMLSchemaCollectionsNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.FullTextCatalogs:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseFullTextCatalogsNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.SequenceObjects:
                    m_ColumnsToShow = columnsToShow.DBObjects;
                    fillDatabaseSequenceObjectsNode(database);
                    break;
                case Sql.ObjectType.TypeEnum.AvailabilityGroups:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillAvailabilityGroupsNode();
                    break;
                case Sql.ObjectType.TypeEnum.AvailabilityGroup:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillAvailabilityGroupNode(tagObj);
                    break;
                case Sql.ObjectType.TypeEnum.AvailabilityGroupReplica:
                    m_ColumnsToShow = columnsToShow.Default;
                    fillAvailabilityGroupReplicaNode(tagObj);
                    break;
                default:
                    m_ColumnsToShow = columnsToShow.Default;
                    break;
            }

            // Update the item count label.
            _tslbl_NodeName.Text = _tslbl_NodeName.Text + " (" + m_DataTable.Rows.Count.ToString() + " Item"
                                            + (m_DataTable.Rows.Count == 1 ? ")" : "s)");

            // If no rows in data table, fill no data collected message.
            if (m_DataTable.Rows.Count == 0)
            {
                m_DataTable.Rows.Add(null, "No data collected", string.Empty, null, string.Empty,
                    string.Empty, string.Empty, string.Empty, false, false, string.Empty, false, false);
            }

            // Update the grid.
            _ultraGrid.BeginUpdate();
            _ultraGrid.DataSource = m_DataTable;
            _ultraGrid.DataMember = "";
            _ultraGrid.EndUpdate();

            // Sort leaf rows.
            if (objType == Sql.ObjectType.TypeEnum.Logins
                || objType == Sql.ObjectType.TypeEnum.ServerRoles
                || objType == Sql.ObjectType.TypeEnum.Endpoints
                || objType == Sql.ObjectType.TypeEnum.Databases
                || objType == Sql.ObjectType.TypeEnum.Users
                || objType == Sql.ObjectType.TypeEnum.DatabaseRoles
                || objType == Sql.ObjectType.TypeEnum.Schemas
                || objType == Sql.ObjectType.TypeEnum.Keys
                || objType == Sql.ObjectType.TypeEnum.Certificates
                || objType == Sql.ObjectType.TypeEnum.Tables
                || objType == Sql.ObjectType.TypeEnum.Views
                || objType == Sql.ObjectType.TypeEnum.Synonyms
                || objType == Sql.ObjectType.TypeEnum.StoredProcedures
                || objType == Sql.ObjectType.TypeEnum.Functions
                || objType == Sql.ObjectType.TypeEnum.ExtendedStoredProcedures
                || objType == Sql.ObjectType.TypeEnum.Assemblies
                || objType == Sql.ObjectType.TypeEnum.UserDefinedDataTypes
                || objType == Sql.ObjectType.TypeEnum.XMLSchemaCollections
                || objType == Sql.ObjectType.TypeEnum.FullTextCatalogs)
            {
                _ultraGrid.DisplayLayout.Bands[0].Columns[colType].SortIndicator = SortIndicator.Descending;
                _ultraGrid.DisplayLayout.Bands[0].Columns[colName].SortIndicator = SortIndicator.Ascending;
            }
            else
            {
                _ultraGrid.DisplayLayout.Bands[0].Columns[colType].SortIndicator = SortIndicator.None;
                _ultraGrid.DisplayLayout.Bands[0].Columns[colName].SortIndicator = SortIndicator.None;
            }

            UpdateColumns(_ultraGrid.DisplayLayout.Bands[0].Columns);
            
        }

        #endregion

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Controls.ObjectExplorer");
        Utility.MenuConfiguration m_menuConfiguration;
        private Sql.RegisteredServer m_ServerInstance;
        private Sql.ServerVersion m_Version;
        private Sql.Snapshot m_Snapshot;
        private int m_SnapshotId;
        private List<Sql.Database> m_Databases;
        private List<Sql.DataCollectionFilter> m_AuditFilters;
        private DataTable m_DataTable;
        private Control m_FilterPageFocusControl;
        private string m_ObjectTypeString;

        private columnsToShow m_ColumnsToShow = columnsToShow.Default;

        private bool m_gridCellClicked = false;
        private List<AvailabilityGroup> m_availabilityGroups;

        #endregion

        #region Query and Columns



        #endregion

        #region Helpers

        protected void showGridColumnChooser(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            string gridHeading = _tslbl_NodeName.Text;
            if (gridHeading.IndexOf("(") > 0)
            {
                gridHeading = gridHeading.Remove(gridHeading.IndexOf("(") - 1);
            }

            Forms.Form_GridColumnChooser.Process(grid, gridHeading);
        }

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

        private bool isSnapshotValid
        {
            get
            {
                return m_ServerInstance != null && m_SnapshotId != 0
                          && m_Snapshot != null && m_Snapshot.HasValidPermissions;
            }
        }

        private void loadSnapshotData()
        {
            Debug.Assert(m_SnapshotId != 0);

            // Get the list of databases.
            m_Databases = Sql.Database.GetSnapshotDatabases(m_SnapshotId);
            m_Databases.Sort();

            //Get list of availability groups 
            m_availabilityGroups = AvailabilityGroup.GetAvailabilityGroups(m_SnapshotId);

            // Get audit filters.
            m_AuditFilters = Sql.DataCollectionFilter.GetSnapshotFilters(m_ServerInstance.ConnectionName, m_SnapshotId);
        }

        private void initControl()
        {
            m_Snapshot = Sql.Snapshot.GetSnapShot(m_SnapshotId);
            // Initialize snapshot label.
            if (m_ServerInstance != null && m_Snapshot != null)
            {
                _pictureBox_Snapshot.Image = m_Snapshot.Icon;
                _lnklbl_Snapshot.Text = SnapshotLblPrefix + m_Snapshot.SnapshotName;
                _lnklbl_Snapshot.LinkArea = new LinkArea(SnapshotLblPrefix.Length, m_Snapshot.SnapshotName.Length);
                _lbl_Instructions.Enabled = true;
            }
            else
            {
                if (m_ServerInstance == null
                    || Sql.Snapshot.SnapshotCount(m_ServerInstance.ConnectionName) == 0)
                {
                    _lnklbl_Snapshot.Text = Utility.ErrorMsgs.ServerNoSnapshots;
                    _lnklbl_Snapshot.LinkArea = new LinkArea(0, 0);
                }
                else
                {
                    _lnklbl_Snapshot.Text = Utility.ErrorMsgs.ServerMissingSnapshot;
                    _lnklbl_Snapshot.LinkArea = new LinkArea(0, _lnklbl_Snapshot.Text.Length);
                }
                _lbl_Instructions.Enabled = false;
            }

            Program.gController.SetCurrentSnapshot(m_ServerInstance, m_SnapshotId);

            // Fill the tree view.
            fillTreeView();
        }

        private void fillTreeView()
        {
            // Start update of the tree view.
            _treeview.BeginUpdate();

            // Clear the tree view node and add the root node.
            _treeview.Nodes.Clear();
            Sql.ObjectTag tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Snapshot);
            TreeNode tnSnapshot = _treeview.Nodes.Add(tag.NodeName, tag.NodeName);
            tnSnapshot.Tag = tag;
            tnSnapshot.ImageIndex = tnSnapshot.SelectedImageIndex = tag.ImageIndex;

            // If there is a valid server instance and a valid snapshot, fill
            // the rest of the tree view.
            if (isSnapshotValid)
            {
                // Add server node.
                TreeNode tn = null;
                tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Server);
                TreeNode tnServer = tnSnapshot.Nodes.Add(tag.NodeName, tag.NodeName);
                tnServer.Tag = tag;
                tnServer.ImageIndex = tnServer.SelectedImageIndex = tag.ImageIndex;

                //SQLsecure 3.1 (Tushar)--Added support for AzureSQLDatabase.
                if (m_ServerInstance.ServerType != ServerType.AzureSQLDatabase)
                {
                    // Add server level environment node and environment level container nodes.
                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Environment);
                    TreeNode tnServerEnvironment = tnServer.Nodes.Add(tag.NodeName, tag.NodeName);
                    tnServerEnvironment.Tag = tag;
                    tnServerEnvironment.ImageIndex = tnServerEnvironment.SelectedImageIndex = tag.ImageIndex;

                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.FileSystem);
                    tn = tnServerEnvironment.Nodes.Add(tag.NodeName, tag.NodeName);
                    tn.Tag = tag;
                    tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Registry);
                    tn = tnServerEnvironment.Nodes.Add(tag.NodeName, tag.NodeName);
                    tn.Tag = tag;
                    tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Services);
                    tn = tnServerEnvironment.Nodes.Add(tag.NodeName, tag.NodeName);
                    tn.Tag = tag;
                    tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;
                }
                // Add server level security node.
                tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.ServerSecurity);
                TreeNode tnServerSecurity = tnServer.Nodes.Add(tag.NodeName, tag.NodeName);
                tnServerSecurity.Tag = tag;
                tnServerSecurity.ImageIndex = tnServerSecurity.SelectedImageIndex = tag.ImageIndex;

                tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Logins);
                tn = tnServerSecurity.Nodes.Add(tag.NodeName, tag.NodeName);
                tn.Tag = tag;
                tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.ServerRoles);
                tn = tnServerSecurity.Nodes.Add(tag.NodeName, tag.NodeName);
                tn.Tag = tag;
                tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                //SQLsecure 3.1 (Tushar)--Added support for AzureSQLDatabase.
                // Add server objects node if 2005 or higher.
                if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported && m_ServerInstance.ServerType != ServerType.AzureSQLDatabase)
                {
                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.ServerObjects);
                    TreeNode svObjects = tnServer.Nodes.Add(tag.NodeName, tag.NodeName);
                    svObjects.Tag = tag;
                    svObjects.ImageIndex = svObjects.SelectedImageIndex = tag.ImageIndex;

                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Endpoints);
                    tn = svObjects.Nodes.Add(tag.NodeName, tag.NodeName);
                    tn.Tag = tag;
                    tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.AvailabilityGroups);
                    tn = svObjects.Nodes.Add(tag.NodeName, tag.NodeName);
                    tn.Tag = tag;
                    tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                    if (m_availabilityGroups.Count != 0)
                    {
                        foreach (AvailabilityGroup mAvailabilityGroup in m_availabilityGroups)
                        {
                            ObjectTag grTag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.AvailabilityGroup, mAvailabilityGroup.ServerGroupId, mAvailabilityGroup.Name, mAvailabilityGroup);
                            TreeNode tbGr = tn.Nodes.Add(mAvailabilityGroup.Name, mAvailabilityGroup.Name);
                            tbGr.Tag = grTag;
                            tbGr.ImageIndex = tbGr.SelectedImageIndex = grTag.ImageIndex;
                            foreach (AvailabilityGroupReplica groupReplicas in mAvailabilityGroup.Replicas)
                            {
                                ObjectTag repTag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.AvailabilityGroupReplica, mAvailabilityGroup.ServerGroupId, mAvailabilityGroup.Name, groupReplicas);
                                TreeNode tbRep = tbGr.Nodes.Add(groupReplicas.ReplicaServerName, groupReplicas.ReplicaServerName);
                                tbRep.Tag = repTag;
                                tbRep.ImageIndex = tbRep.SelectedImageIndex = repTag.ImageIndex;
                            }

                        }
                    }


                }

                // Add databases node.
                tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Databases);
                TreeNode tnDatabases = tnServer.Nodes.Add(tag.NodeName, tag.NodeName);
                tnDatabases.Tag = tag;
                tnDatabases.ImageIndex = tnDatabases.SelectedImageIndex = tag.ImageIndex;
                foreach (Sql.Database db in m_Databases)
                {
                    // Add db node (mark any unavailable database as Unavailable)
                    string dbName = db.Name + (db.IsAvailable ? "" : " (Unavailable)");
                    tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Database, db, 0, db.DbId, db.Name);
                    TreeNode tnDb = tnDatabases.Nodes.Add(dbName, dbName);
                    tnDb.Tag = tag;
                    tnDb.ImageIndex = tnDb.SelectedImageIndex = tag.ImageIndex;

                    // If the database is unavailable then do not fill the nodes.
                    if (db.IsAvailable)
                    {
                        // Add db security node.
                        tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.DatabaseSecurity, db);
                        TreeNode tnDbSecurity = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                        tnDbSecurity.Tag = tag;
                        tnDbSecurity.ImageIndex = tnDbSecurity.SelectedImageIndex = tag.ImageIndex;

                        tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Users, db);
                        tn = tnDbSecurity.Nodes.Add(tag.NodeName, tag.NodeName);
                        tn.Tag = tag;
                        tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                        tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.DatabaseRoles, db);
                        tn = tnDbSecurity.Nodes.Add(tag.NodeName, tag.NodeName);
                        tn.Tag = tag;
                        tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                        if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
                        {
                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Schemas, db);
                            tn = tnDbSecurity.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                            //tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Keys, db);
                            //tn = tnDbSecurity.Nodes.Add(tag.NodeName, tag.NodeName);
                            //tn.Tag = tag;
                            //tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                            //tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Certificates, db);
                            //tn = tnDbSecurity.Nodes.Add(tag.NodeName, tag.NodeName);
                            //tn.Tag = tag;
                            //tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;
                        }

                        // Add tables node.
                        tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Tables, db);
                        tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                        tn.Tag = tag;
                        tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                        // Add views node.
                        tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Views, db);
                        tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                        tn.Tag = tag;
                        tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                        // Add synonyms node (SQL 2005)
                        if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
                        {
                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Synonyms, db);
                            tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;
                        }

                        // Add stored procs node.
                        tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.StoredProcedures, db);
                        tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                        tn.Tag = tag;
                        tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                        // Add functions node.
                        tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Functions, db);
                        tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                        tn.Tag = tag;
                        tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                        // Add extended stored proc (if master db).
                        if (string.Compare(db.Name, "master", true) == 0)
                        {
                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.ExtendedStoredProcedures, db);
                            tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;
                        }

                        // Add assemblies, user-defined data types, xml schema collections
                        // and Full-text catalogs nodes (SQL 2005)
                        if (m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
                        {
                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.Assemblies, db);
                            tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.UserDefinedDataTypes, db);
                            tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.XMLSchemaCollections, db);
                            tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.FullTextCatalogs, db);
                            tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;

                            tag = new Sql.ObjectTag(m_SnapshotId, Sql.ObjectType.TypeEnum.SequenceObjects, db);
                            tn = tnDb.Nodes.Add(tag.NodeName, tag.NodeName);
                            tn.Tag = tag;
                            tn.ImageIndex = tn.SelectedImageIndex = tag.ImageIndex;
                        }
                    }
                }

                // Expand server & databases tree nodes.
                tnSnapshot.Expand();
                tnServer.Expand();
                tnDatabases.Expand();
            }

            // End update of the tree view.
            _treeview.EndUpdate();

            // Select the root node & set focus on the tree.
            _treeview.SelectedNode = tnSnapshot;
        }

        private void setMenuConfiguration(bool isProperties)
        {
            m_menuConfiguration.FileItems[(int)MenuItems_File.Print] = false;

            m_menuConfiguration.EditItems[(int)MenuItems_Edit.Remove] = false;
            m_menuConfiguration.EditItems[(int)MenuItems_Edit.ConfigureDataCollection] = false;
            m_menuConfiguration.EditItems[(int)MenuItems_Edit.Properties] = isProperties && isSnapshotValid;

            m_menuConfiguration.ViewItems[(int)MenuItems_View.CollapseAll] = true;
            m_menuConfiguration.ViewItems[(int)MenuItems_View.ExpandAll] = true;
            m_menuConfiguration.ViewItems[(int)MenuItems_View.GroupByColumn] = false;
            m_menuConfiguration.ViewItems[(int)MenuItems_View.Refresh] = true;

            m_menuConfiguration.PermissionsItems[(int)MenuItems_Permissions.UserPermissions] = true;
            m_menuConfiguration.PermissionsItems[(int)MenuItems_Permissions.ObjectPermissions] = false;

            m_menuConfiguration.SnapshotItems[(int)MenuItems_Snapshots.Baseline] = false;
            m_menuConfiguration.SnapshotItems[(int)MenuItems_Snapshots.Collect] = false;
            m_menuConfiguration.SnapshotItems[(int)MenuItems_Snapshots.GroomingSchedule] = false;

            Program.gController.SetMenuConfiguration(m_menuConfiguration, this);
        }

        private static string nodeNameStr(Sql.ObjectTag tag)
        {
            string ret = string.Empty;
            string db = tag.DatabaseName;
            if (tag.ObjType == Sql.ObjectType.TypeEnum.Database)
            {
                ret = db;
            }
            else
            {
                if (!string.IsNullOrEmpty(db)) { db += @"\"; }
                ret = db + tag.TypeName;
            }
            return ret;
        }

        private void setTypeNameAndPath(Sql.ObjectTag tag)
        {
            _tslbl_NodeName.Image = tag.Image16;
            _tslbl_NodeName.Text = nodeNameStr(tag);
        }

        private void displayObjProperties(Sql.ObjectTag tag)
        {
            this.Cursor = Cursors.WaitCursor;
            switch (tag.ObjType)
            {
                case Sql.ObjectType.TypeEnum.Snapshot:
                    Forms.Form_SnapshotProperties.Process(tag);
                    break;
                case Sql.ObjectType.TypeEnum.Server:
                    Forms.Form_SnapshotServerProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.File:
                    Forms.Form_SnapshotFileProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.RegistryKey:
                    Forms.Form_SnapshotRegistryProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.Service:
                    Forms.Form_SnapshotServiceProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.WindowsUserLogin:
                case Sql.ObjectType.TypeEnum.WindowsGroupLogin:
                case Sql.ObjectType.TypeEnum.SqlLogin:
                    Forms.Form_SnapshotLoginProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.ServerRole:
                    Forms.Form_SnapshotServerRoleProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.Endpoint:
                    Forms.Form_SnapshotEndpointProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.Database:
                    Forms.Form_SnapshotDatabaseProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.User:
                    Forms.Form_SnapshotUserProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.DatabaseRole:
                case Sql.ObjectType.TypeEnum.ApplicationRole:
                    Forms.Form_SnapshotDbRoleProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.Key:
                    break;
                case Sql.ObjectType.TypeEnum.Certificate:
                    break;
                case Sql.ObjectType.TypeEnum.Schema:
                    Forms.Form_SnapshotSchemaProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.Table:
                case Sql.ObjectType.TypeEnum.View:
                case Sql.ObjectType.TypeEnum.Synonym:
                case Sql.ObjectType.TypeEnum.StoredProcedure:
                case Sql.ObjectType.TypeEnum.Function:
                case Sql.ObjectType.TypeEnum.ExtendedStoredProcedure:
                case Sql.ObjectType.TypeEnum.Assembly:
                case Sql.ObjectType.TypeEnum.UserDefinedDataType:
                case Sql.ObjectType.TypeEnum.XMLSchemaCollection:
                case Sql.ObjectType.TypeEnum.FullTextCatalog:
                case Sql.ObjectType.TypeEnum.SequenceObject:
                    Forms.Form_SnapshotDbObjProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.AvailabilityGroup:
                    Form_SnapshotAvailabilityGroupProperties.Process(m_Version, tag);
                    break;
                case Sql.ObjectType.TypeEnum.AvailabilityGroupReplica:
                    Form_SnapshotAvailabilityGroupReplicaProperties.Process(m_Version, tag);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            this.Cursor = Cursors.Default;
        }

        private void selectTreeViewNode(Sql.ObjectTag tag)
        {
            Debug.Assert(!tag.IsLeafTag);

            // Select the child node of the currently selected 
            // node whose names matches the selected grid row type.
            if (_treeview.SelectedNode != null)
            {
                // Find the node that now has to be selected, and 
                // mark it as the selected node.
                TreeNode[] matchingNodes = _treeview.SelectedNode.Nodes.Find(tag.NodeName, false);
                Debug.Assert(matchingNodes.Length == 1);
                _treeview.SelectedNode = matchingNodes[0];
            }
        }

        #endregion

        #region Ctors

        public ObjectExplorer()
            : base()
        {
            InitializeComponent();

            // Set the tree view image list.
            this._treeview.ImageList = Sql.ObjectType.TypeImageList16();

            // Set button images.
            _tsbtn_ColumnChooser.Image = AppIcons.AppImage16(AppIcons.Enum.GridFieldChooser);
            _toolStripButton_GroupBy.Image = AppIcons.AppImage16(AppIcons.Enum.GridGroupBy);
            _toolStripButton_Save.Image = AppIcons.AppImage16(AppIcons.Enum.GridSaveToExcel);
            _toolStripButton_Print.Image = AppIcons.AppImage16(AppIcons.Enum.Print);
            _tsbtn_Up.Image = AppIcons.AppImage16(AppIcons.Enum.FolderUp);
            _mi_Grid_Properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);
            _mi_Tree_Properties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);

            // hook the toolbar labels to the grids so the heading can be used for printing
            _ultraGrid.Tag = _tslbl_NodeName;

            // hook the grids to the toolbars so they can be used for button processing
            _toolHdr.Tag = _ultraGrid;

            _ultraGrid.DisplayLayout.GroupByBox.Hidden = Utility.Constants.InitialState_GroupByBoxHidden;

            // Hide the focus rectangles on tabs and grids
            _ultraGrid.DrawFilter = new HideFocusRectangleDrawFilter();

            // Initialize base class fields.
            m_menuConfiguration = new Utility.MenuConfiguration();

            // Create the grid data table and add columns.
            m_DataTable = new DataTable();
            m_DataTable.Columns.Add(colTypeIcon, typeof(Image));
            m_DataTable.Columns.Add(colName, typeof(string));
            m_DataTable.Columns.Add(colType, typeof(string));
            m_DataTable.Columns.Add(colTag, typeof(Sql.ObjectTag));
            m_DataTable.Columns.Add(colOwner, typeof(string));
            m_DataTable.Columns.Add(colSchema, typeof(string));
            m_DataTable.Columns.Add(colSchemaOwner, typeof(string));
            m_DataTable.Columns.Add(colLogin, typeof(string));
            m_DataTable.Columns.Add(colHasAccess, typeof(bool));
            m_DataTable.Columns.Add(colHasIsAliased, typeof(bool));
            m_DataTable.Columns.Add(colMemberOf, typeof(string));
            m_DataTable.Columns.Add(colServerAccess, typeof(bool));
            m_DataTable.Columns.Add(colServerDeny, typeof(bool));
            m_DataTable.Columns.Add(colState, typeof(string));
            m_DataTable.Columns.Add(colIsContainedUser, typeof(bool));
        }

        #endregion

        #region IView

        void Interfaces.IView.SetContext(Interfaces.IDataContext contextIn)
        {
            // Get the instance name from the context.
            Debug.Assert(((Data.PermissionExplorer)contextIn).ServerInstance != null);
            string instance = ((Data.PermissionExplorer)contextIn).ServerInstance.ConnectionName;
            int snapshotid = ((Data.PermissionExplorer)contextIn).SnapShotId;
            Debug.Assert(!string.IsNullOrEmpty(instance));

            setMenuConfiguration(false);

            // IF   servers are not changing
            //      OR the current snapshot is invalid
            //      OR current & new snapshots are valid and do not match.
            // THEN refresh the data.
            if ((m_ServerInstance == null || string.IsNullOrEmpty(m_ServerInstance.ConnectionName)
                    || string.Compare(instance,m_ServerInstance.ConnectionName,true) != 0)
                || m_SnapshotId == 0 
                || (m_SnapshotId != 0 && snapshotid != 0 && m_SnapshotId != snapshotid))
            {
                // Clear all the fields.
                m_ServerInstance = null;
                m_Version = Sql.ServerVersion.Unsupported;
                m_SnapshotId = 0;
                m_Databases = null;
                m_AuditFilters = null;
                m_DataTable.Clear();

                // Retrieve information from the repository.
                bool isOk = true;
                try
                {
                    // Get the registered server information from the repository.
                    Sql.RegisteredServer.GetServer(Program.gController.Repository.ConnectionString,
                                                        instance, out m_ServerInstance);

                    // Get the current snapshot id & timestamp, server version and databases.
                    if (m_ServerInstance != null)
                    {
                        // Get server version.
                        m_Version = Sql.SqlHelper.ParseVersion(m_ServerInstance.Version);

                        // If specified snapshot is invalid, then get the last collection snapshotid,
                        // otherwise use the specified snapshot id.
                        m_SnapshotId = snapshotid == 0 ? m_ServerInstance.LastCollectionSnapshotId : snapshotid;

                        // Get databases, and sort them.
                        // Get audit filters.
                        if (m_SnapshotId != 0)
                        {
                            loadSnapshotData();
                        }
                    }
                    else
                    {
                        MsgBox.ShowWarning(ErrorMsgs.ObjectExplorerCaption, ErrorMsgs.ServerNotRegistered);
                        m_SnapshotId = 0;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ObjectExplorerCaption, ErrorMsgs.GetAuditServerInfoFromRepositoryFailed, ex);
                    isOk = false;
                }

                // Initialize the control, based on the retrieved information.
                if (isOk)
                {
                    initControl();
                }
            }
            Program.gController.SetCurrentSnapshot(m_ServerInstance, m_SnapshotId);
        }

        String Interfaces.IView.HelpTopic
        {
            get { return Utility.Help.ObjectPermissionsHelpTopic; }
        }
        String Interfaces.IView.ConceptTopic
        {
            get { return Utility.Help.PermissionExplorerConceptTopic; }
        }
        String Interfaces.IView.Title
        {
            get { return ""; }
        }

        #endregion

        #region ICommandHandler Members

        void Interfaces.ICommandHandler.ProcessCommand(Utility.ViewSpecificCommand command)
        {
            switch (command)
            {
                case Utility.ViewSpecificCommand.NewAuditServer:
                    showNewAuditServer();
                    break;
                case Utility.ViewSpecificCommand.NewLogin:
                    showNewLogin();
                    break;
                case Utility.ViewSpecificCommand.Baseline:
                    showBaseline();
                    break;
                case Utility.ViewSpecificCommand.Collect:
                    showCollect();
                    break;
                case Utility.ViewSpecificCommand.Configure:
                    showConfigure();
                    break;
                case Utility.ViewSpecificCommand.Delete:
                    showDelete();
                    break;
                case Utility.ViewSpecificCommand.Properties:
                    showProperties();
                    break;
                case Utility.ViewSpecificCommand.Refresh:
                    showRefresh();
                    break;
                case Utility.ViewSpecificCommand.UserPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.UserPermissions);
                    break;
                case Utility.ViewSpecificCommand.ObjectPermissions:
                    showPermissions(Views.View_PermissionExplorer.Tab.ObjectPermissions);
                    break;
                default:
                    Debug.Assert(false, "Unknown command passed to ObjectExplorer");
                    break;
            }
        }

        protected virtual void showNewAuditServer()
        {
            Forms.Form_WizardRegisterSQLServer.Process();
        }

        protected virtual void showNewLogin()
        {
            Forms.Form_WizardNewLogin.Process();
        }

        protected virtual void showBaseline()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ObjectExplorer showBaseline command called erroneously");
        }

        protected virtual void showCollect()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ObjectExplorer showCollect command called erroneously");
        }

        protected virtual void showConfigure()
        {
            Forms.Form_SqlServerProperties.Process(m_ServerInstance.ConnectionName, Forms.Form_SqlServerProperties.RequestedOperation.EditCofiguration, Program.gController.isAdmin);
        }

        protected virtual void showDelete()
        {
            // This should be overriden if needed by the View and should never be called
            logX.loggerX.Error("Error - ObjectExplorer showDelete command called erroneously");
        }

        protected virtual void showProperties()
        {
            // Get the object tag based on which control has focus.
            Sql.ObjectTag tag = null;
            if (_ultraGrid.Focused && _ultraGrid.Selected.Rows.Count == 1)
            {
                // Get the tag.
                tag = _ultraGrid.Selected.Rows[0].Cells[colTag].Value as Sql.ObjectTag;
                Debug.Assert(tag != null);
            }
            else
            {
                // Get the tag.
                Debug.Assert(_treeview.SelectedNode != null);
                tag = _treeview.SelectedNode.Tag as Sql.ObjectTag;
                Debug.Assert(tag != null);
            }

            // Display properties.
            if (tag != null)
            {
                Debug.Assert(tag.IsPropertyTag);
                displayObjProperties(tag);
            }
        }

        protected virtual void showRefresh()
        {
            // if the snapshot is in progress, there is partial info, so refresh until it is finished
            if (m_Snapshot != null && m_Snapshot.Status == Utility.Snapshot.StatusInProgress)
            {
                // Get the snapshot data and time stamp.
                try
                {
                    loadSnapshotData();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ObjectExplorerCaption, ErrorMsgs.GetAuditServerInfoFromRepositoryFailed, ex);
                }

                // Initialize the control.
                initControl();
            }
        }

        protected virtual void showPermissions(Views.View_PermissionExplorer.Tab tabIn)
        {
            Program.gController.ShowRootView(new Utility.NodeTag(new Data.PermissionExplorer(m_ServerInstance, m_SnapshotId, tabIn),
                                                        Utility.View.PermissionExplorer));
        }

        #endregion

        #region Event Handlers

        private void _splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_FilterPageFocusControl != null)
            {
                m_FilterPageFocusControl.Focus();
                m_FilterPageFocusControl = null;
            }
        }

        private void _splitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            m_FilterPageFocusControl = getFocused(this.Controls);
        }

        private void _lnklbl_Snapshot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            // Display the snapshot selection form, and get the snapshot selection.
            int newid = Forms.Form_SelectSnapshot.GetSnapshotId(m_ServerInstance);

            // If no new id & existing snapshot id is valid,
            // or if both ids are the same, don't do anything.
            if (!((newid == 0 && m_SnapshotId != 0) || (newid == m_SnapshotId)))
            {
                // Set the snapshot id.
                m_SnapshotId = newid;

                // Get the snapshot data and time stamp.
                try
                {
                    loadSnapshotData();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ObjectExplorerCaption, ErrorMsgs.GetAuditServerInfoFromRepositoryFailed, ex);
                }

                // Initialize the control.
                initControl();
            }
            Cursor = Cursors.Default;
        }

        private void _tsbtn_Up_Click(object sender, EventArgs e)
        {
            if (_treeview.SelectedNode != null && _treeview.SelectedNode.Level != 0)
            {
                _treeview.SelectedNode = _treeview.SelectedNode.Parent;
            }
        }

        private void _treeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if the node is not selected, make it the selected one for either left or right click
            //so context menu will work correctly at different levels
            TreeNode tn = ((TreeView)sender).SelectedNode;
            if (tn != null)
            {
                if (tn.Equals(e.Node))
                {
                    Sql.ObjectTag tag = tn.Tag as Sql.ObjectTag;
                    if (tag != null)
                    {
                        setMenuConfiguration(tag.IsPropertyTag);
                    }
                }
                else
                {
                    ((TreeView)sender).SelectedNode = e.Node;
                }
            }
        }

        private void _treeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Get the tree node tag.
            Cursor = Cursors.WaitCursor;
            Sql.ObjectTag tag = e.Node.Tag as Sql.ObjectTag;
            if (tag != null)
            {
                // If its the root node, disable the up button.
                _tsbtn_Up.Enabled = e.Node.Level != 0;

                // Set the label and path based on selected node.
                setTypeNameAndPath(tag);

                // Set the menu configuration.
                setMenuConfiguration(tag.IsPropertyTag);

                // Update the grid if the snapshot is valid.
                if (isSnapshotValid)
                {
                    m_ObjectTypeString = Sql.ObjectType.TypeName(tag.ObjType);
                    // Update the grid view.
                    updateGridView(tag.ObjType, tag.Database, tag.Tag);

                    // Clear any colors set for the grid selection.
                    _ultraGrid.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.White;
                    _ultraGrid.DisplayLayout.Override.ActiveRowAppearance.ForeColor = Color.Black;

                    // Set focus on the tree view.
                    _treeview.Focus();
                }
            }
            Cursor = Cursors.Default;
        }

        private void _cntxtMenu_Tree_Opening(object sender, CancelEventArgs e)
        {
            if (!isSnapshotValid)
            {
                _mi_Tree_Properties.Enabled = false;
            } 
            else if (_treeview.SelectedNode != null)
            {
                Sql.ObjectTag tag = _treeview.SelectedNode.Tag as Sql.ObjectTag;
                if (tag != null)
                {
                    _mi_Tree_Properties.Enabled = tag.IsPropertyTag;
                }
                else
                {
                    _mi_Tree_Properties.Enabled = false;
                }
            }
            else
            {
                _mi_Tree_Properties.Enabled = false;
            }
        }

        private void _mi_Tree_Properties_Click(object sender, EventArgs e)
        {
            Debug.Assert(_treeview.SelectedNode != null);
            Sql.ObjectTag tag = _treeview.SelectedNode.Tag as Sql.ObjectTag;
            if (tag != null)
            {
                displayObjProperties(tag);
            }
        }

        private void UpdateColumns(Infragistics.Win.UltraWinGrid.ColumnsCollection columns)
        {
            columns[colTypeIcon].Header.Caption = "";
            columns[colTypeIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            columns[colTypeIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colTypeIcon].Width = 22;
            columns[colTypeIcon].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            columns[colName].Header.Caption = "Name";
            columns[colName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            columns[colName].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colName].PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
            if (columns[colName].Width < 180)
            {
                columns[colName].Width = 180;
            }
            else if (columns[colName].Width > _ultraGrid.Width * .65)
            {
                columns[colName].Width = (int)Math.Floor(_ultraGrid.Width * .65);
            }

            columns[colType].Header.Caption = "Object Type";
            columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colType].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colType].Width = 100;

            columns[colTag].Header.Caption = "Tag";
            columns[colTag].Hidden = true;
            columns[colTag].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

            // Owner column
            columns[colOwner].Header.Caption = "Owner";
            columns[colOwner].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colOwner].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colOwner].Width = 100;
            if (m_ColumnsToShow == columnsToShow.DBRolesAndSchema || m_ColumnsToShow == columnsToShow.DBObjects)
            {
                columns[colOwner].Hidden = false;
                columns[colOwner].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            else if (m_ColumnsToShow == columnsToShow.Files || m_ColumnsToShow == columnsToShow.RegistryKeys)
            {
                columns[colOwner].Hidden = false;
                columns[colOwner].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colOwner].PerformAutoResize(PerformAutoSizeType.AllRowsInBand, true);
            }
            else
            {
                columns[colOwner].Hidden = true;
                columns[colOwner].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            // Schema & Schema owner columns
            columns[colSchema].Header.Caption = "Schema";
            columns[colSchema].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colSchema].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colSchema].Width = 100;
            columns[colSchemaOwner].Header.Caption = "Schema Owner";
            columns[colSchemaOwner].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colSchemaOwner].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colSchemaOwner].Width = 100;

            if (m_ColumnsToShow == columnsToShow.DBObjects && m_Version > Sql.ServerVersion.SQL2000 && m_Version != Sql.ServerVersion.Unsupported)
            {
                columns[colSchema].Hidden = false;
                columns[colSchema].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colSchemaOwner].Hidden = false;
                columns[colSchemaOwner].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            else
            {
                columns[colSchema].Hidden = true;
                columns[colSchema].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                columns[colSchemaOwner].Hidden = true;
                columns[colSchemaOwner].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            // Login, Has Access, and Is Aliased columns
            columns[colLogin].Header.Caption = "Login";
            columns[colLogin].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colLogin].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colLogin].Width = 100;
            columns[colHasAccess].Header.Caption = "Has Access";
            columns[colHasAccess].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colHasAccess].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colHasAccess].Width = 80;
            columns[colHasIsAliased].Header.Caption = "Is Aliased";
            columns[colHasIsAliased].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colHasIsAliased].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colHasIsAliased].Width = 80;

            if (m_ColumnsToShow == columnsToShow.DBUsers)
            {
                columns[colLogin].Hidden = false;
                columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colHasAccess].Hidden = false;
                columns[colHasAccess].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colHasIsAliased].Hidden = false;
                columns[colHasIsAliased].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colIsContainedUser].Hidden = false;
                columns[colIsContainedUser].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            else
            {
                columns[colLogin].Hidden = true;
                columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                columns[colHasAccess].Hidden = true;
                columns[colHasAccess].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                columns[colHasIsAliased].Hidden = true;
                columns[colHasIsAliased].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                columns[colIsContainedUser].Hidden = true;
                columns[colIsContainedUser].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            // Member Of, Server Access, and Server Deny columns
            columns[colMemberOf].Header.Caption = "Member Of";
            columns[colMemberOf].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colMemberOf].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colMemberOf].Width = 100;
            columns[colServerAccess].Header.Caption = "Server Access";
            columns[colServerAccess].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colServerAccess].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colServerAccess].Width = 80;
            columns[colServerDeny].Header.Caption = "Server Deny";
            columns[colServerDeny].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
//            columns[colServerDeny].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            columns[colServerDeny].Width = 80;

            if (m_ColumnsToShow == columnsToShow.ServerLogins)
            {
                columns[colMemberOf].Hidden = false;
                columns[colMemberOf].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colServerAccess].Hidden = false;
                columns[colServerAccess].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colServerDeny].Hidden = false;
                columns[colServerDeny].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            else
            {
                columns[colMemberOf].Hidden = true;
                columns[colMemberOf].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                columns[colServerAccess].Hidden = true;
                columns[colServerAccess].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                columns[colServerDeny].Hidden = true;
                columns[colServerDeny].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }

            if (m_ColumnsToShow == columnsToShow.Services)
            {
                columns[colLogin].Hidden = false;
                columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                columns[colLogin].Width = 180;
                columns[colState].Hidden = false;
                columns[colState].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
            }
            else
            {
                columns[colLogin].Hidden = true;
                columns[colLogin].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                columns[colState].Hidden = true;
                columns[colState].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            }
        }

        private void _ultraGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
//            e.Layout.Override.CellAppearance.BorderAlpha = Alpha.Transparent;
//            e.Layout.Override.RowAppearance.BorderColor = Color.White;

            UpdateColumns(e.Layout.Bands[0].Columns);
        }

        private void _ultraGrid_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            if (m_gridCellClicked && _ultraGrid.Selected.Rows.Count > 0)
            {
                Debug.Assert(_ultraGrid.Selected.Rows.Count == 1);

                // Get the tag.
                Sql.ObjectTag tag = _ultraGrid.Selected.Rows[0].Cells[colTag].Value as Sql.ObjectTag;
                if (tag != null)
                {
                    // If the object is a leaf object, show its properties.
                    // Else synchronize with the tree view node.
                    if (tag.IsLeafTag)
                    {
                        displayObjProperties(tag);
                    }
                    else
                    {
                        // Select the tree view node that corresponds to the
                        // row object type.
                        selectTreeViewNode(tag);
                    }
                }
            }
        }

        private void _ultraGrid_MouseDown(object sender, MouseEventArgs e)
        {
            Infragistics.Win.UIElement elementMain;
            Infragistics.Win.UIElement elementUnderMouse;

            elementMain = _ultraGrid.DisplayLayout.UIElement;

            elementUnderMouse = elementMain.ElementFromPoint(new Point(e.X, e.Y));
            if (elementUnderMouse != null)
            {
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = elementUnderMouse.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell)) as Infragistics.Win.UltraWinGrid.UltraGridCell;
                if (cell != null)
                {
                    m_gridCellClicked = true;
                    Infragistics.Win.UltraWinGrid.SelectedRowsCollection sr = _ultraGrid.Selected.Rows;
                    if (sr.Count > 0)
                    {
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in sr)
                        {
                            row.Selected = false;
                        }
                    }
                    cell.Row.Selected = true;
                    _ultraGrid.ActiveRow = cell.Row;

                    // Set focus on ultra grid.
                    _ultraGrid.Focus();
                }
                else
                {
                    m_gridCellClicked = false;
                    Infragistics.Win.UltraWinGrid.HeaderUIElement he = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.HeaderUIElement)) as Infragistics.Win.UltraWinGrid.HeaderUIElement;
                    Infragistics.Win.UltraWinGrid.ColScrollbarUIElement ce = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.ColScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.ColScrollbarUIElement;
                    Infragistics.Win.UltraWinGrid.RowScrollbarUIElement re = elementUnderMouse.GetAncestor(typeof(Infragistics.Win.UltraWinGrid.RowScrollbarUIElement)) as Infragistics.Win.UltraWinGrid.RowScrollbarUIElement;
                    if (he == null && ce == null && re == null)
                    {
                        _ultraGrid.Selected.Rows.Clear();
                        _ultraGrid.ActiveRow = null;
                    }
                }
            }
            if (_ultraGrid.Selected.Rows.Count == 1)
            {
                // Set menu configuration.
                Debug.Assert(Tag == null);
                Sql.ObjectTag tag = _ultraGrid.Selected.Rows[0].Cells[colTag].Value as Sql.ObjectTag;
                if (tag != null)
                {
                    setMenuConfiguration(tag.IsPropertyTag);
                }
            }
            else
            {
                setMenuConfiguration(false);
            }
        }

        private void _cntxtMenu_Grid_Opening(object sender, CancelEventArgs e)
        {
            if (!isSnapshotValid)
            {
                _mi_Grid_Properties.Enabled = false;
            }
            else if (_ultraGrid.Selected.Rows.Count > 0)
            {
                Debug.Assert(_ultraGrid.Selected.Rows.Count == 1);
                Sql.ObjectTag tag = _ultraGrid.Selected.Rows[0].Cells[colTag].Value as Sql.ObjectTag;
                if (tag != null)
                {
                    _mi_Grid_Properties.Enabled = tag.IsPropertyTag;
                }
                else
                {
                    _mi_Grid_Properties.Enabled = false;
                }
            }
            else
            {
                _mi_Grid_Properties.Enabled = false;
            }
        }

        private void _mi_Grid_Properties_Click(object sender, EventArgs e)
        {
            if (_ultraGrid.Selected.Rows.Count > 0)
            {
                Debug.Assert(_ultraGrid.Selected.Rows.Count == 1);
                Sql.ObjectTag tag = _ultraGrid.Selected.Rows[0].Cells[colTag].Value as Sql.ObjectTag;
                if (tag != null)
                {
                    displayObjProperties(tag);
                }
            }
        }

        private void _tsbtn_ColumnChooser_Click(object sender, EventArgs e)
        {
            showGridColumnChooser(_ultraGrid);
        }

        private void _toolStripButton_LoginsServerGroupBy_Click(object sender, EventArgs e)
        {
            // Note: This event handler is used by all grid toolbars
            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            Cursor = Cursors.WaitCursor;

            toggleGridGroupByBox(grid);

            Cursor = Cursors.Default;

        }

        private void _toolStripButton_LoginsServerSave_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            saveGrid(grid);

            Cursor = Cursors.Default;

        }

        private void _toolStripButton_LoginsServerPrint_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Debug.Assert(((UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag).GetType() == typeof(UltraGrid));
            UltraGrid grid = (UltraGrid)((HeaderStrip)((ToolStripItem)sender).Owner).Tag;

            printGrid(grid);

            Cursor = Cursors.Default;

        }

        protected void printGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            Debug.Assert(grid.Tag.GetType() == typeof(ToolStripLabel));

            // Associate the print document with the grid & preview dialog here
            // in case we want to change documents for each grid
            _ultraGridPrintDocument.Grid = grid;
            _ultraGridPrintDocument.DefaultPageSettings.Landscape = true;
            _ultraGridPrintDocument.DefaultPageSettings.Color = false;
            _ultraGridPrintDocument.DocumentName = ErrorMsgs.UserPermissionsCaption;
            _ultraGridPrintDocument.FitWidthToPages = 2;
            _ultraGridPrintDocument.Header.TextLeft = 
                string.Format(PrintHeaderDisplay, m_ObjectTypeString,
                m_ServerInstance == null ? String.Empty : m_ServerInstance.ConnectionName);
            //if (m_user_shown != null)
            //{m_
            //    _ultraGridPrintDocument.Header.TextLeft =
            //        string.Format(PrintHeaderDisplay,
            //                            m_user_shown.Name,
            //                            string.Format(DottedNameDisplay,
            //                                            m_serverInstance_shown,
            //                                            m_database_shown),
            //                            m_snapshotTime_shown,
            //                            string.Format(PrintPermissionsHeaderDisplay,
            //                                            _tabControl_User.SelectedTab.Text,
            //                                            ((ToolStripLabel)grid.Tag).Text)
            //                        );
            //}
            //else
            //{
            //    _ultraGridPrintDocument.Header.TextLeft =
            //        string.Format(PrintEmptyHeaderDisplay,
            //                            string.Format(PrintPermissionsHeaderDisplay,
            //                                            _tabControl_User.SelectedTab.Text,
            //                                            ((ToolStripLabel)grid.Tag).Text)
            //                        );
            //}
            //_ultraGridPrintDocument.Footer.TextCenter =
            //    string.Format("Page {0}",
            //                    _ultraGridPrintDocument.PageNumber,
            //                    _ultraPrintPreviewDialog.pag
            _ultraPrintPreviewDialog.Document = _ultraGridPrintDocument;

            // Call ShowDialog to show the print preview dialog.
            _ultraPrintPreviewDialog.ShowDialog();
        }

        protected void saveGrid(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            bool iconHidden = false;
            if (_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //save the current state of the icon column and then hide it before exporting
                    if (grid.DisplayLayout.Bands[0].Columns.Exists(colTypeIcon))
                    {
                        // this column doesn't exist in the raw data hack
                        iconHidden = grid.DisplayLayout.Bands[0].Columns[colTypeIcon].Hidden;
                        grid.DisplayLayout.Bands[0].Columns[colTypeIcon].Hidden = true;
                    }
                    _ultraGridExcelExporter.Export(grid, _saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ErrorMsgs.ExportToExcelCaption, ErrorMsgs.FailedToExportToExcelFile, ex);
                }
                if (grid.DisplayLayout.Bands[0].Columns.Exists(colTypeIcon))
                {
                    // this column doesn't exist in the raw data hack
                    grid.DisplayLayout.Bands[0].Columns[colTypeIcon].Hidden = iconHidden;
                }
            }
        }

        protected void toggleGridGroupByBox(Infragistics.Win.UltraWinGrid.UltraGrid grid)
        {
            grid.DisplayLayout.GroupByBox.Hidden = !grid.DisplayLayout.GroupByBox.Hidden;
        }

        private void ObjectExplorer_Leave(object sender, EventArgs e)
        {
            Program.gController.SetMenuConfiguration(new MenuConfiguration());
        }

        #endregion
    }
}
