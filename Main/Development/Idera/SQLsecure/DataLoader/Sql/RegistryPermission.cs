using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Data;
using System.Data.SqlClient;
using System.Management;
using Microsoft.Win32;
using System.Security.Principal;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Interop;
using Path = System.IO.Path;

namespace Idera.SQLsecure.Collector.Sql
{

    public class RegistryPermission
    {
        #region fields
        private const int EmbptyDatabaseID = int.MinValue;
        private int osObjectId;
        private enumOSObjectType objectType;    // Disk, File Directory, File, Installation Directory, Database file, Registry key
        private int databaseId = int.MinValue;  // Null if not a database file
        private string objectName;              // upto 260 char limit
        private string longObjectname = null;   // upto 32k
        private Sid ownersid;
        private string disktype;      // NTFS, FAT or null if not file object
        private string objectSid;
        private List<FileAccessRight> accessRightsList = new List<FileAccessRight>();
        private List<FileAuditSetting> auditSettingsList = new List<FileAuditSetting>();
        #endregion

        #region Properties
        public void AddFileAccessRight(FileAccessRight fa)
        {
            accessRightsList.Add(fa);
        }

        public void AddFileAuditSetting(FileAuditSetting fa)
        {
            auditSettingsList.Add(fa);
        }

        public List<FileAccessRight> AccessRightList
        {
            get { return accessRightsList; }
        }

        public List<FileAuditSetting> AuditSettingList
        {
            get { return auditSettingsList; }
        }

        public int OSObjectId
        {
            get { return osObjectId; }
            set { osObjectId = value; }
        }
        public enumOSObjectType ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }
        public int DatabaseId
        {
            get { return databaseId; }
            set { databaseId = value; }
        }
        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        public string LongObjectname
        {
            get { return longObjectname; }
            set { longObjectname = value; }
        }
        public Sid OwnerSid
        {
            get { return ownersid; }
            set { ownersid = value; }
        }
        public string Disktype
        {
            get { return disktype; }
            set { disktype = value; }
        }
        public string ObjectSid
        {
            get { return objectSid; }
            set { objectSid = value; }
        }
        #endregion
    }

    public class SQLPROTOCOL
    {
        private string m_protocolname = null;
        private bool m_enabled = true;
        private bool m_active = true;
        private string m_IPAddress = null;
        private bool m_dynamicPort = false;
        private string m_Port = null;

        #region Properties

        public string ProtocolName
        {
            get { return m_protocolname; }
            set { m_protocolname = value; }
        }
        public bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }
        public bool Active
        {
            get { return m_active; }
            set { m_active = value; }
        }
        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }
        public bool DynamicPort
        {
            get { return m_dynamicPort; }
            set { m_dynamicPort = value; }
        }
        public string Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        #endregion


    }

    public class RegistryPermissions
    {

        #region fields
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.RegistryPermissions");
        List<RegistryPermission> registryPermissionsList = new List<RegistryPermission>();
        private int m_snapshotId;
        private string m_targetServerName;
        private string m_targetInstanceName;
        private string m_InstallPath;
        private string m_ErrorLogPath;
        private int m_NumErrorLogs;
        private string m_AgentErrorLogPath;
        private Sql.ServerVersion m_SQLVersionEnum;
        private string m_regInstancePath;

        // Security Settings
        private bool m_isHidden;
        private string m_loginAuditMode;
        private string m_emialProfile;
        private List<SQLPROTOCOL> m_protocols = new List<SQLPROTOCOL>();

        private int m_OSObjectCount;
        #endregion

        #region CTOR
        public RegistryPermissions(int snapshotID, string computerName, string targetInstance, Sql.ServerVersion SQLVersionEnum)
        {
            Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
            m_snapshotId = snapshotID;
            m_targetServerName = computerName;
            m_targetInstanceName = targetInstance;
            m_SQLVersionEnum = SQLVersionEnum;
            m_regInstancePath = GetRegistryInstancePath();
            m_InstallPath = GetInstallPath();
            m_ErrorLogPath = GetErrorLogPath();
            m_NumErrorLogs = GetNumErrorLogs();
            m_AgentErrorLogPath = GetAgentErrorLogPath();
            Program.RestoreImpersonationContext(wi);
        }
        #endregion

        #region Properties

        private bool Is2000
        {
            get { return m_SQLVersionEnum == Sql.ServerVersion.SQL2000; }
        }
        private bool Is2005
        {
            get { return m_SQLVersionEnum == Sql.ServerVersion.SQL2005; }
        }
        private bool Is2008
        {
            get { return m_SQLVersionEnum == Sql.ServerVersion.SQL2008 || m_SQLVersionEnum == Sql.ServerVersion.SQL2008R2; }
        }
        private bool Is2012
        {
            get { return m_SQLVersionEnum == Sql.ServerVersion.SQL2012; }
        }

        private bool Is2014
        {
            get { return m_SQLVersionEnum == Sql.ServerVersion.SQL2014; }
        }
        public int NumOSObjectsWrittenToRepository
        {
            get { return m_OSObjectCount; }
        }

        public string InstallPath
        {
            get { return m_InstallPath; }
        }

        public string ErrorLogPath
        {
            get { return m_ErrorLogPath; }
        }
        public int NumErrorLogs
        {
            get { return m_NumErrorLogs; }
        }

        public string AgentErrorLogPath
        {
            get { return m_AgentErrorLogPath; }
        }


        public bool IsHidden
        {
            get { return m_isHidden; }
        }

        public string EmailProfile
        {
            get { return m_emialProfile; }
        }

        public List<SQLPROTOCOL> Protocols
        {
            get { return m_protocols; }
        }

        #endregion

        #region public Methods

        public int LoadRegistrySettings()
        {
            Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
            int numWarnings = 0;
            RegistryKey valueKey = null;
            RegistryKey remoteBaseKey = null;
            bool isTCPEnabled = false;
            logX.loggerX.Debug(string.Format("Loading SQL Server Registry Settings for Computer: {0} and Instance: {1}", m_targetServerName, m_targetInstanceName));
            logX.loggerX.Debug(string.Format("Instance Registry Path: {0}", m_regInstancePath));

            using (logX.loggerX.DebugCall())
            {
                try
                {
                    remoteBaseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, m_targetServerName);

                    // Is Instance Hidden?
                    try
                    {
                        m_isHidden = false;
                        if (Is2000)
                        {
                            valueKey =
                                remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLSERVER\SuperSocketNetLib\TCP");
                            m_isHidden = ((int)valueKey.GetValue("TCPHideFlag", 0)) == 0 ? false : true;
                        }
                        else
                        {
                            valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLSERVER\SuperSocketNetLib");
                            m_isHidden = ((int)valueKey.GetValue("HideInstance", 0)) == 0 ? false : true;
                        }
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(string.Format("Failed to get Is Instance Hidden for {0}\\{1}: {2}",
                                                         m_targetServerName, m_targetInstanceName, ex.Message));
                    }
                    finally
                    {
                        if (valueKey != null)
                        {
                            valueKey.Close();
                            valueKey = null;
                        }
                    }

                    // SQL Agent Email Profile?
                    try
                    {
                        m_emialProfile = string.Empty;
                        valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\SQLServerAgent");
                        bool useDBmail = ((int)valueKey.GetValue("UseDatabaseMail", 0)) == 0
                                             ? false
                                             : true;
                        if (useDBmail)
                        {
                            m_emialProfile = (string)valueKey.GetValue("DatabaseMailProfile", string.Empty);
                        }
                        else
                        {
                            m_emialProfile = (string)valueKey.GetValue("EmailProfile", string.Empty);
                        }
                        if (m_emialProfile.Length > 128)
                        {
                            logX.loggerX.Warn(
                                string.Format("Email profile name clipped to 128 (max allowed by repository): {0}",
                                              m_emialProfile));
                            m_emialProfile = m_emialProfile.Substring(0, 128);
                        }
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(string.Format("Failed to get Email profile for {0}\\{1}: {2}",
                                                         m_targetServerName, m_targetInstanceName, ex.Message));
                    }
                    finally
                    {
                        if (valueKey != null)
                        {
                            valueKey.Close();
                            valueKey = null;
                        }
                    }

                    // Audit Mode
                    try
                    {
                        valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLServer");
                        int mode = (int)valueKey.GetValue("AuditLevel", 0);
                        switch (mode)
                        {
                            case Collector.Constants.LoginAuditModeNone:
                                m_loginAuditMode = Collector.Constants.LoginAuditModeNoneStr;
                                break;

                            case Collector.Constants.LoginAuditModeSuccess:
                                m_loginAuditMode = Collector.Constants.LoginAuditModeSuccessStr;
                                break;

                            case Collector.Constants.LoginAuditModeFailure:
                                m_loginAuditMode = Collector.Constants.LoginAuditModeFailureStr;
                                break;

                            case Collector.Constants.LoginAuditModeAll:
                                m_loginAuditMode = Collector.Constants.LoginAuditModeAllStr;
                                break;

                            default:
                                logX.loggerX.Warn("WARN - unknown login audit mode");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(string.Format("Failed to get AuditMode for {0}\\{1}: {2}",
                                                         m_targetServerName, m_targetInstanceName, ex.Message));
                    }
                    finally
                    {
                        if (valueKey != null)
                        {
                            valueKey.Close();
                            valueKey = null;
                        }
                    }

                    // Protocols
                    RegistryKey subkey = null;
                    try
                    {
                        if (Is2000)
                        {
                            valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLSERVER\SuperSocketNetLib");
                            string[] protcolList = (string[])valueKey.GetValue("ProtocolList", string.Empty);
                            foreach (string s in protcolList)
                            {
                                SQLPROTOCOL sp = new SQLPROTOCOL();
                                sp.ProtocolName = s.ToUpper();
                                if (sp.ProtocolName.Contains("TCP"))
                                {
                                    isTCPEnabled = true;
                                    continue;
                                }
                                else if (sp.ProtocolName.Contains("NP"))
                                {
                                    sp.ProtocolName = "Named Pipes";
                                    m_protocols.Add(sp);
                                }
                                else if (sp.ProtocolName.Contains("SPX"))
                                {
                                    sp.ProtocolName = @"NWLink IPX/SPX";
                                    m_protocols.Add(sp);
                                }
                            }
                        }
                        else
                        {
                            valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLSERVER\SuperSocketNetLib");
                            foreach (string s in valueKey.GetSubKeyNames())
                            {
                                subkey = valueKey.OpenSubKey(s);
                                bool enabled = (int)subkey.GetValue("Enabled", 0) == 0 ? false : true;
                                if (enabled)
                                {
                                    SQLPROTOCOL sp = new SQLPROTOCOL();
                                    sp.ProtocolName = (string)subkey.GetValue("DisplayName", s);
                                    sp.IPAddress = (string)subkey.GetValue("PipeName", string.Empty);
                                    if (sp.ProtocolName.ToUpper().Contains("TCP"))
                                    {
                                        isTCPEnabled = true;
                                    }
                                    else
                                    {
                                        m_protocols.Add(sp);
                                    }
                                }
                                subkey.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(string.Format("Failed to get Protocols for {0}\\{1}: {2}",
                                                         m_targetServerName, m_targetInstanceName, ex.Message));
                    }
                    finally
                    {
                        if (valueKey != null)
                        {
                            valueKey.Close();
                            valueKey = null;
                        }
                        if (subkey != null)
                        {
                            subkey.Close();
                            subkey = null;
                        }
                    }

                    // TCP Ports                
                    try
                    {
                        if (isTCPEnabled)
                        {
                            if (Is2000)
                            {
                                valueKey =
                                    remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLSERVER\SuperSocketNetLib\TCP");
                                SQLPROTOCOL sp = new SQLPROTOCOL();
                                sp.ProtocolName = @"TCP/IP";
                                sp.IPAddress = (string)valueKey.GetValue("IPAddress", string.Empty);
                                sp.Port = (string)valueKey.GetValue("TcpPort", string.Empty);
                                sp.Enabled = true;
                                sp.Active = true;
                                sp.DynamicPort =
                                    string.IsNullOrEmpty((string)valueKey.GetValue("TcpDynamicPorts", string.Empty))
                                        ? false
                                        : true;
                                m_protocols.Add(sp);
                            }
                            else
                            {
                                SQLPROTOCOL sp = new SQLPROTOCOL();
                                RegistryKey subKey = null;
                                bool listenAll = false;
                                try
                                {
                                    subKey = remoteBaseKey.OpenSubKey(m_regInstancePath +
                                                                      @"\MSSQLSERVER\SuperSocketNetLib\TCP");
                                    listenAll = (int)subKey.GetValue("ListenOnAllIPs", 0) == 0 ? false : true;
                                    if (listenAll)
                                    {
                                        valueKey = subKey.OpenSubKey("IPAll");
                                        if (valueKey != null)
                                        {
                                            SQLPROTOCOL sp2 = new SQLPROTOCOL();
                                            sp2.ProtocolName = @"TCP/IP";
                                            sp2.IPAddress = "Any";
                                            sp2.Port = (string)valueKey.GetValue("TcpPort", string.Empty);
                                            sp2.DynamicPort =
                                                string.IsNullOrEmpty(
                                                    (string)valueKey.GetValue("TcpDynamicPorts", string.Empty))
                                                    ? false
                                                    : true;
                                            sp2.Enabled = true;
                                            sp2.Active = true;
                                            m_protocols.Add(sp2);

                                            valueKey.Close();
                                            valueKey = null;
                                        }
                                    }
                                    else
                                    {
                                        foreach (string s in subKey.GetSubKeyNames())
                                        {
                                            try
                                            {
                                                if (string.Compare(s, "IPAll", true) == 0)
                                                    continue;

                                                valueKey = subKey.OpenSubKey(s);
                                                SQLPROTOCOL sp2 = new SQLPROTOCOL();
                                                sp2.ProtocolName = @"TCP/IP";
                                                sp2.IPAddress = (string)valueKey.GetValue("IPAddress", string.Empty);
                                                sp2.Port = (string)valueKey.GetValue("TcpPort", string.Empty);
                                                sp2.Enabled = (int)valueKey.GetValue("Enabled", 0) == 0 ? false : true;
                                                sp2.Active = (int)valueKey.GetValue("Active", 0) == 0 ? false : true;
                                                sp2.DynamicPort =
                                                    string.IsNullOrEmpty(
                                                        (string)valueKey.GetValue("TcpDynamicPorts", string.Empty))
                                                        ? false
                                                        : true;
                                                if (sp2.Enabled)
                                                {
                                                    m_protocols.Add(sp2);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                logX.loggerX.Error(
                                                    string.Format("Failed to enumerate TCP Ports for {0}\\{1}: {2}",
                                                                  m_targetServerName, m_targetInstanceName, ex.Message));
                                            }
                                            finally
                                            {
                                                if (valueKey != null)
                                                {
                                                    valueKey.Close();
                                                    valueKey = null;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    numWarnings++;
                                    logX.loggerX.Error(string.Format("Failed to enumerate TCP Ports  for {0}\\{1}: {2}",
                                                                     m_targetServerName, m_targetInstanceName,
                                                                     ex.Message));
                                }
                                finally
                                {
                                    if (subKey != null)
                                    {
                                        subKey.Close();
                                        subKey = null;
                                    }
                                    if (valueKey != null)
                                    {
                                        valueKey.Close();
                                        valueKey = null;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(string.Format("Failed to get TCP Ports {0}\\{1}: {2}",
                                                         m_targetServerName, m_targetInstanceName, ex.Message));
                    }
                    finally
                    {
                        if (valueKey != null)
                        {
                            valueKey.Close();
                            valueKey = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    numWarnings++;
                    logX.loggerX.Error(string.Format("Failed to load Registry Settings for {0}\\{1}: {2}",
                                                     m_targetServerName, m_targetInstanceName, ex.Message));
                }
                finally
                {
                    if (remoteBaseKey != null)
                    {
                        remoteBaseKey.Close();
                        remoteBaseKey = null;
                    }
                    Program.RestoreImpersonationContext(wi);
                }
            }
            return numWarnings++;
        }

        public int ProcessRegistryPermissions(List<SQLService> services)
        {
            Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
            int numWarnings = 0;
            RegistryKey remoteBaseKey = null;
            RegistryKey valueKey = null;
            string regSubKey = null;

            try
            {
                using (logX.loggerX.DebugCall())
                {
                    logX.loggerX.Debug(string.Format("Processing SQL Server Registry Keys for Computer: {0} and Instance: {1}", m_targetServerName, m_targetInstanceName));
                    remoteBaseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, m_targetServerName);
                    if (remoteBaseKey != null)
                    {
                        logX.loggerX.Debug(string.Format("Successfully opened Base Key {0}", RegistryHive.LocalMachine.ToString()));
                    }
                    else
                    {
                        logX.loggerX.Error(string.Format("Failed to opened Base Key {0}", RegistryHive.LocalMachine.ToString()));
                    }
                    foreach (SQLService s in services)
                    {
                        logX.loggerX.Debug("Processing SQL Server Services Registry Keys");
                        try
                        {
                            valueKey = remoteBaseKey.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\" + s.Name,
                                                RegistryKeyPermissionCheck.ReadSubTree,
                                                RegistryRights.ReadPermissions);
                            GetRegistryPermission(valueKey);
                        }
                        catch (Exception ex)
                        {
                            numWarnings++;
                            logX.loggerX.Error(
                                string.Format(@"Failed to get permissions for SYSTEM\CurrentControlSet\Services\{0}:  {1}",
                                s.Name, ex.Message));
                        }
                        finally
                        {
                            if (valueKey != null)
                            {
                                valueKey.Close();
                                valueKey = null;
                            }
                        }
                    }
                    try
                    {
                        regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server";
                        logX.loggerX.Debug(string.Format(@"Processing HKLM\{0} Registry Keys", regSubKey));
                        valueKey = remoteBaseKey.OpenSubKey(regSubKey,
                                                            RegistryKeyPermissionCheck.ReadSubTree,
                                                            RegistryRights.ReadPermissions);
                        GetRegistryPermission(valueKey);
                        if (!Is2000)
                        {
                            if (valueKey != null)
                            {
                                valueKey.Close();
                                valueKey = null;
                            }
                            regSubKey = string.Empty;
                            if (Is2005)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Native Client";
                            }
                            else if (Is2008)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server Native Client 10.0";
                            }
                            else if (Is2012)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server Native Client 11.0";
                            }
                            else if (Is2014)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server Native Client 11.0";
                            }
                            if (!string.IsNullOrEmpty(regSubKey))
                            {
                                logX.loggerX.Debug(string.Format(@"Processing HKLM\{0} Registry Keys", regSubKey));
                                valueKey = remoteBaseKey.OpenSubKey(regSubKey,
                                                                    RegistryKeyPermissionCheck.ReadSubTree,
                                                                    RegistryRights.ReadPermissions);
                                GetRegistryPermission(valueKey);
                            }
                            if (valueKey != null)
                            {
                                valueKey.Close();
                                valueKey = null;
                            }
                            regSubKey = string.Empty;
                            if (Is2005)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server 2005 Redist";
                            }
                            else if (Is2008)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server 2008 Redist";
                            }
                            else if (Is2012)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server 2012 Redist";
                            }
                            else if (Is2014)
                            {
                                regSubKey = @"SOFTWARE\Microsoft\Microsoft SQL Server 2014 Redist";
                            }
                            if (!string.IsNullOrEmpty(regSubKey))
                            {
                                logX.loggerX.Debug(string.Format(@"Processing HKLM\{0} Registry Keys", regSubKey));
                                valueKey = remoteBaseKey.OpenSubKey(regSubKey,
                                                                    RegistryKeyPermissionCheck.ReadSubTree,
                                                                    RegistryRights.ReadPermissions);
                                GetRegistryPermission(valueKey);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(
                            string.Format(@"Failed to get permissions for HKLM\{0}:  {1}",
                            regSubKey, ex.Message));
                    }
                    finally
                    {
                        if (valueKey != null)
                        {
                            valueKey.Close();
                            valueKey = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                numWarnings++;
                logX.loggerX.Error(string.Format("Error Getting Registry Permissions: {0}", ex.Message));
            }
            finally
            {
                if (remoteBaseKey != null)
                {
                    remoteBaseKey.Close();
                    remoteBaseKey = null;
                }
                Program.RestoreImpersonationContext(wi);

            }

            return numWarnings;

        }

        public bool WriteRegistryPermissionToRepository(string repositoryConnectionString, int nStart)
        {
            Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();

            bool isOK = true;
            m_OSObjectCount = nStart;
            try
            {
                using (logX.loggerX.DebugCall())
                {
                    using (SqlConnection repository = new SqlConnection(repositoryConnectionString))
                    {
                        // Open repository connection.
                        repository.Open();
                        // Use bulk copy object to write to repository.
                        using (SqlBulkCopy bcpObject = new SqlBulkCopy(repository),
                                           bcpPermissions = new SqlBulkCopy(repository))
                        {
                            // Set the destination table.
                            bcpObject.DestinationTableName = SQLServerObjectTable.RepositoryTable;
                            bcpObject.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            bcpPermissions.DestinationTableName = SQLServerObjectPermissionTable.RepositoryTable;
                            bcpPermissions.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            // Create the datatable to write to the repository.
                            using (DataTable dataTableObject = SQLServerObjectTable.Create(),
                                             dataTablePermissions = SQLServerObjectPermissionTable.Create())
                            {
                                foreach (RegistryPermission fp in registryPermissionsList)
                                {
                                    // Update the datatable.
                                    DataRow dr = dataTableObject.NewRow();
                                    dr[SQLServerObjectTable.ParamSnapshotid] = m_snapshotId;
                                    dr[SQLServerObjectTable.ParamObjectId] = ++m_OSObjectCount;
                                    dr[SQLServerObjectTable.ParamObjectType] = fp.ObjectType.ToString();
                                    if (fp.DatabaseId == int.MinValue)
                                    {
                                        dr[SQLServerObjectTable.ParamDbId] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[SQLServerObjectTable.ParamDbId] = fp.DatabaseId;
                                    }
                                    dr[SQLServerObjectTable.ParamObjectName] = fp.ObjectName;
                                    if (string.IsNullOrEmpty(fp.LongObjectname))
                                    {
                                        dr[SQLServerObjectTable.ParamLongName] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[SQLServerObjectTable.ParamLongName] = fp.LongObjectname;
                                    }
                                    dr[SQLServerObjectTable.ParamOwnerSid] = fp.OwnerSid.BinarySid;
                                    dr[SQLServerObjectTable.ParamDiskType] = DBNull.Value;

                                    dataTableObject.Rows.Add(dr);
                                    if (dataTableObject.Rows.Count >= Constants.RowBatchSize)
                                    {
                                        bcpObject.WriteToServer(dataTableObject);
                                        dataTableObject.Rows.Clear();
                                    }

                                    foreach (FileAccessRight ar in fp.AccessRightList)
                                    {
                                        DataRow dr1 = dataTablePermissions.NewRow();
                                        dr1[SQLServerObjectPermissionTable.ParamSnapshotid] = m_snapshotId;
                                        dr1[SQLServerObjectPermissionTable.ParamObjectId] = m_OSObjectCount;
                                        dr1[SQLServerObjectPermissionTable.ParamAuditFlags] = DBNull.Value;
                                        dr1[SQLServerObjectPermissionTable.ParamFileSystemRights] = ar.FileSystemRights;
                                        dr1[SQLServerObjectPermissionTable.ParamSID] = ar.ID.BinarySid;
                                        dr1[SQLServerObjectPermissionTable.ParamAccessType] = ar.AccessType;
                                        dr1[SQLServerObjectPermissionTable.ParamIsInherited] = ar.IsInHerited.ToString();

                                        dataTablePermissions.Rows.Add(dr1);
                                        if (dataTablePermissions.Rows.Count >= Constants.RowBatchSize)
                                        {
                                            bcpPermissions.WriteToServer(dataTablePermissions);
                                            dataTablePermissions.Rows.Clear();
                                        }
                                    }
                                    foreach (FileAuditSetting ar in fp.AuditSettingList)
                                    {
                                        DataRow dr2 = dataTablePermissions.NewRow();
                                        dr2[SQLServerObjectPermissionTable.ParamSnapshotid] = m_snapshotId;
                                        dr2[SQLServerObjectPermissionTable.ParamObjectId] = m_OSObjectCount;
                                        dr2[SQLServerObjectPermissionTable.ParamAuditFlags] = ar.AuditFlags;
                                        dr2[SQLServerObjectPermissionTable.ParamFileSystemRights] = ar.FileSystemRights;
                                        dr2[SQLServerObjectPermissionTable.ParamSID] = ar.ID.BinarySid;
                                        dr2[SQLServerObjectPermissionTable.ParamAccessType] = DBNull.Value;
                                        dr2[SQLServerObjectPermissionTable.ParamIsInherited] = ar.IsInHerited.ToString();

                                        dataTablePermissions.Rows.Add(dr2);
                                        if (dataTablePermissions.Rows.Count >= Constants.RowBatchSize)
                                        {
                                            bcpPermissions.WriteToServer(dataTablePermissions);
                                            dataTablePermissions.Rows.Clear();
                                        }
                                    }
                                }

                                if (dataTablePermissions.Rows.Count > 0)
                                {
                                    bcpPermissions.WriteToServer(dataTablePermissions);
                                }
                                if (dataTableObject.Rows.Count > 0)
                                {
                                    bcpObject.WriteToServer(dataTableObject);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isOK = false;
                logX.loggerX.Error("WriteFilePermissionToRepository failed: ", ex.Message);
            }

            Program.RestoreImpersonationContext(ic);

            return isOK;
        }

        public bool WriteRegistrySettingsToRepository(string repositoryConnectionString)
        {
            Program.ImpersonationContext ic = Program.SetLocalImpersonationContext();
            bool isOK = true;
            try
            {
                using (logX.loggerX.DebugCall())
                {
                    using (SqlConnection repository = new SqlConnection(repositoryConnectionString))
                    {
                        // Open repository connection.
                        repository.Open();
                        // Use bulk copy object to write to repository.
                        using (SqlBulkCopy bcp = new SqlBulkCopy(repository))
                        {
                            // Set the destination table.
                            bcp.DestinationTableName = SQLserverprotocolTable.RepositoryTable;
                            bcp.BulkCopyTimeout = SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry();
                            // Create the datatable to write to the repository.
                            using (DataTable dataTable = SQLserverprotocolTable.Create())
                            {
                                foreach (SQLPROTOCOL p in m_protocols)
                                {
                                    // Update the datatable.
                                    DataRow dr = dataTable.NewRow();
                                    dr[SQLserverprotocolTable.ParamSnapshotid] = m_snapshotId;
                                    dr[SQLserverprotocolTable.ParamProtocolName] = p.ProtocolName;
                                    if (string.IsNullOrEmpty(p.Port))
                                    {
                                        dr[SQLserverprotocolTable.ParamPort] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[SQLserverprotocolTable.ParamPort] = p.Port;
                                    }
                                    if (string.IsNullOrEmpty(p.IPAddress))
                                    {
                                        dr[SQLserverprotocolTable.ParamIPAddress] = DBNull.Value;
                                    }
                                    else
                                    {
                                        dr[SQLserverprotocolTable.ParamIPAddress] = p.IPAddress;
                                    }
                                    dr[SQLserverprotocolTable.ParamDynamicPort] = p.DynamicPort == false
                                                                                      ? Collector.Constants.No.ToString()
                                                                                      : Collector.Constants.Yes.ToString();
                                    dr[SQLserverprotocolTable.ParamEnabled] = p.Enabled == false
                                                                                  ? Collector.Constants.No.ToString()
                                                                                  : Collector.Constants.Yes.ToString();
                                    dr[SQLserverprotocolTable.ParamActive] = p.Active == false
                                                                                 ? Collector.Constants.No.ToString()
                                                                                 : Collector.Constants.Yes.ToString();

                                    dataTable.Rows.Add(dr);
                                }
                                bcp.WriteToServer(dataTable);
                            }
                        }

                        // Update Server Snapshot table
                        string queryUpdateServerSnapshotTable = @"update SQLsecure.dbo.serversnapshot 
set 
hideinstance = '{0}',
loginauditmode = '{1}',
agentmailprofile = '{2}',
numerrorlogs = {4}
from 
    SQLsecure.dbo.serversnapshot
where
    snapshotid = {3}";
                        string query = string.Format(queryUpdateServerSnapshotTable,
                                                     m_isHidden ? Collector.Constants.Yes : Collector.Constants.No,
                                                     m_loginAuditMode,
                                                     m_emialProfile,
                                                     m_snapshotId,
                                                     m_NumErrorLogs);
                        Sql.SqlHelper.ExecuteNonQuery(repository, CommandType.Text, query, null);

                    }
                }
            }
            catch (Exception ex)
            {
                isOK = false;
                logX.loggerX.Error("WriteRegistrySettingsToRepository failed: ", ex.Message);
            }
            Program.RestoreImpersonationContext(ic);
            return isOK;
        }

        public int GetUsersAndGroups(ref List<Account> users, ref List<Account> groups)
        {
            int numWarnings = 0;
            List<string> processSids = new List<string>();
            using (logX.loggerX.DebugCall())
            {
                Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
                try
                {
                    string name = string.Empty;
                    string domain = string.Empty;
                    Account account;
                    SID_NAME_USE peUse;

                    foreach (RegistryPermission fp in registryPermissionsList)
                    {
                        if (!processSids.Contains(fp.OwnerSid.SidString))
                        {
                            processSids.Add(fp.OwnerSid.SidString);
                            try
                            {
                                Sid.LookupAccountName(m_targetServerName, fp.OwnerSid, out name, out domain, out peUse);
                                string sam = domain + @"\" + name;
                                if (peUse == SID_NAME_USE.SidTypeUser)
                                {
                                    if (Account.CreateUserAccount(fp.OwnerSid, sam, out account))
                                    {
                                        if (!users.Contains(account))
                                        {
                                            users.Add(account);
                                        }
                                    }
                                }
                                else if (peUse == SID_NAME_USE.SidTypeGroup ||
                                         peUse == SID_NAME_USE.SidTypeWellKnownGroup ||
                                         peUse == SID_NAME_USE.SidTypeAlias)
                                {
                                    if (Account.CreateGroupAccount(fp.OwnerSid, sam, out account))
                                    {
                                        if (!groups.Contains(account))
                                        {
                                            groups.Add(account);
                                        }
                                    }
                                }
                                else
                                {
                                    numWarnings++;
                                    if (Account.CreateUnknownAccount(fp.OwnerSid, fp.OwnerSid.SidString, out account))
                                    {
                                        if (!users.Contains(account))
                                        {
                                            users.Add(account);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                numWarnings++;
                                logX.loggerX.Error("Failed to create account for File Owner: ", ex.Message);
                            }
                        }

                        foreach (FileAccessRight fr in fp.AccessRightList)
                        {
                            if (processSids.Contains(fr.ID.SidString))
                            {
                                continue;
                            }
                            processSids.Add(fr.ID.SidString);
                            try
                            {
                                Sid.LookupAccountName(m_targetServerName, fr.ID, out name, out domain, out peUse);
                                string sam = domain + @"\" + name;
                                if (peUse == SID_NAME_USE.SidTypeUser)
                                {
                                    if (Account.CreateUserAccount(fr.ID, sam, out account))
                                    {
                                        if (!users.Contains(account))
                                        {
                                            users.Add(account);
                                        }
                                    }
                                }
                                else if (peUse == SID_NAME_USE.SidTypeGroup ||
                                         peUse == SID_NAME_USE.SidTypeWellKnownGroup ||
                                         peUse == SID_NAME_USE.SidTypeAlias)
                                {
                                    if (Account.CreateGroupAccount(fr.ID, sam, out account))
                                    {
                                        if (!groups.Contains(account))
                                        {
                                            groups.Add(account);
                                        }
                                    }
                                }
                                else
                                {
                                    numWarnings++;
                                    if (Account.CreateUnknownAccount(fr.ID, fr.ID.SidString, out account))
                                    {
                                        if (!users.Contains(account))
                                        {
                                            users.Add(account);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                numWarnings++;
                                logX.loggerX.Error("Failed to create account for File Access Rights: ", ex.Message);
                            }
                        }
                        foreach (FileAuditSetting fr in fp.AuditSettingList)
                        {
                            if (processSids.Contains(fr.ID.SidString))
                            {
                                continue;
                            }
                            processSids.Add(fr.ID.SidString);
                            try
                            {
                                Sid.LookupAccountName(m_targetServerName, fr.ID, out name, out domain, out peUse);
                                string sam = domain + @"\" + name;
                                if (peUse == SID_NAME_USE.SidTypeUser)
                                {
                                    if (Account.CreateUserAccount(fr.ID, sam, out account))
                                    {
                                        if (!users.Contains(account))
                                        {
                                            users.Add(account);
                                        }
                                    }
                                }
                                else if (peUse == SID_NAME_USE.SidTypeGroup ||
                                         peUse == SID_NAME_USE.SidTypeWellKnownGroup ||
                                         peUse == SID_NAME_USE.SidTypeAlias)
                                {
                                    if (Account.CreateGroupAccount(fr.ID, sam, out account))
                                    {
                                        if (!groups.Contains(account))
                                        {
                                            groups.Add(account);
                                        }
                                    }
                                }
                                else
                                {
                                    numWarnings++;
                                    if (Account.CreateUnknownAccount(fr.ID, fr.ID.SidString, out account))
                                    {
                                        if (!users.Contains(account))
                                        {
                                            users.Add(account);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                numWarnings++;
                                logX.loggerX.Error("Failed to create account for File Audit Settings: ", ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    numWarnings++;
                    logX.loggerX.Error("Failed to process Users and Groups Accounts: ", ex.Message);
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }
            return numWarnings;
        }


        #endregion

        #region Helpers

        private string GetRegistryInstancePath()
        {
            string regInstancePath = string.Empty;
            RegistryKey instanceKey = null;
            RegistryKey remoteBaseKey = null;
            using (logX.loggerX.DebugCall())
            {
                logX.loggerX.Debug(string.Format("Get Registry Instance Path for Computer: {0} and Instance: {1}", m_targetServerName, m_targetInstanceName));
                try
                {
                    if (Is2000)
                    {
                        if (string.IsNullOrEmpty(m_targetInstanceName))
                        {
                            regInstancePath = @"Software\Microsoft\MSSQLSERVER";
                        }
                        else
                        {
                            regInstancePath =
                                string.Format(@"Software\Microsoft\Microsoft SQL Server\{0}", m_targetInstanceName);
                        }
                    }
                    else
                    {
                        remoteBaseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, m_targetServerName);
                        if (remoteBaseKey != null)
                        {
                            logX.loggerX.Debug(string.Format("Successfully opened Base Key {0}", RegistryHive.LocalMachine.ToString()));
                        }
                        else
                        {
                            logX.loggerX.Error(string.Format("Failed to open Base Key {0}", RegistryHive.LocalMachine.ToString()));
                        }
                        instanceKey =
                            remoteBaseKey.OpenSubKey(@"Software\Microsoft\Microsoft SQL Server\Instance Names\SQL");
                        if (instanceKey == null)
                        {
                            logX.loggerX.Error(@"Failed to open Instance sub-Key: Software\Microsoft\Microsoft SQL Server\Instance Names\SQL");
                        }
                        string instanceKeyName =
                            (string)instanceKey.GetValue(string.IsNullOrEmpty(m_targetInstanceName)
                                                              ? "MSSQLSERVER"
                                                              : m_targetInstanceName);
                        regInstancePath =
                            string.Format(@"Software\Microsoft\Microsoft SQL Server\{0}", instanceKeyName);
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(string.Format("Failed to get Registry Instance Path for {0}\\{1}: {2}",
                                                     m_targetServerName, m_targetInstanceName, ex.Message));
                }
                finally
                {
                    if (instanceKey != null)
                        instanceKey.Close();
                    if (remoteBaseKey != null)
                        remoteBaseKey.Close();
                }
                logX.loggerX.Debug(string.Format("Registry Instance Path: {0}", regInstancePath));

            }
            return regInstancePath;
        }

        private string GetInstallPath()
        {
            string installPath = string.Empty;
            RegistryKey valueKey = null;
            RegistryKey remoteBaseKey = null;
            using (logX.loggerX.DebugCall())
            {
                try
                {
                    remoteBaseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, m_targetServerName);
                    valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\setup");
                    installPath = (string)valueKey.GetValue("SQLPath", string.Empty);
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(string.Format("Failed to read Installation Path for {0}\\{1}: {2}",
                                                     m_targetServerName, m_targetInstanceName, ex.Message));
                }
                finally
                {
                    if (remoteBaseKey != null)
                        remoteBaseKey.Close();
                    if (valueKey != null)
                        valueKey.Close();
                }
            }
            return installPath;
        }

        private string GetErrorLogPath()
        {
            string logPath = string.Empty;
            RegistryKey valueKey = null;
            RegistryKey remoteBaseKey = null;
            using (logX.loggerX.DebugCall())
            {
                try
                {
                    remoteBaseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, m_targetServerName);
                    valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLServer\Parameters");
                    logPath = (string)valueKey.GetValue("SQLArg1", string.Empty);
                    if (logPath.StartsWith("-e"))
                    {
                        logPath = logPath.Substring(2);
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(string.Format("Failed to read Error Log Path for {0}\\{1}: {2}",
                                                     m_targetServerName, m_targetInstanceName, ex.Message));
                }
                finally
                {
                    if (remoteBaseKey != null)
                        remoteBaseKey.Close();
                    if (valueKey != null)
                        valueKey.Close();
                }
            }
            return logPath;
        }

        private int GetNumErrorLogs()
        {
            int numErrorLogs = 0;
            RegistryKey valueKey = null;
            RegistryKey remoteBaseKey = null;
            using (logX.loggerX.DebugCall())
            {
                try
                {
                    remoteBaseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, m_targetServerName);
                    valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\MSSQLServer");
                    if (valueKey != null) numErrorLogs = (int)valueKey.GetValue("NumErrorLogs", 0);
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(string.Format("Failed to read Num Error Logs for {0}\\{1}: {2}",
                                                     m_targetServerName, m_targetInstanceName, ex.Message));
                }
                finally
                {
                    if (remoteBaseKey != null)
                        remoteBaseKey.Close();
                    if (valueKey != null)
                        valueKey.Close();
                }
            }
            return numErrorLogs;
        }
        private string GetAgentErrorLogPath()
        {
            string logPath = string.Empty;
            RegistryKey valueKey = null;
            RegistryKey remoteBaseKey = null;
            using (logX.loggerX.DebugCall())
            {
                try
                {
                    remoteBaseKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, m_targetServerName);
                    valueKey = remoteBaseKey.OpenSubKey(m_regInstancePath + @"\SQLServerAgent");
                    logPath = (string)valueKey.GetValue("ErrorLogFile", string.Empty);
                }
                catch (Exception ex)
                {
                    logX.loggerX.Error(string.Format("Failed to read Agent Error Log Path for {0}\\{1}: {2}",
                                                     m_targetServerName, m_targetInstanceName, ex.Message));
                }
                finally
                {
                    if (remoteBaseKey != null)
                        remoteBaseKey.Close();
                    if (valueKey != null)
                        valueKey.Close();
                }
            }
            return logPath;
        }

        private int ProcessSubKeys(RegistryKey rk)
        {
            int numWarnings = 0;

            numWarnings += GetRegistryPermission(rk);
            RegistryKey valueKey = null;

            foreach (string s in rk.GetSubKeyNames())
            {
                try
                {
                    valueKey = rk.OpenSubKey(s);
                    numWarnings += ProcessSubKeys(valueKey);
                }
                catch (Exception ex)
                {
                    numWarnings++;
                    logX.loggerX.Error(string.Format("Failed to Process Registry Key {0}: {1}",
                        rk.Name, ex.Message));
                }
                finally
                {
                    if (valueKey != null)
                    {
                        valueKey.Close();
                        valueKey = null;
                    }
                }
            }
            return numWarnings;
        }

        private int GetRegistryPermission(RegistryKey rk)
        {
            int numWarnings = 0;
            try
            {
                logX.loggerX.Debug(string.Format("Gettting Permission for Registry Key {0}", rk.Name));
                RegistryPermission fp = new RegistryPermission();
                fp.DatabaseId = int.MinValue;
                string localFileName = rk.Name;
                if (localFileName.Length > 260)
                {
                    fp.ObjectName = localFileName.Substring(0, 260);
                    fp.LongObjectname = localFileName;
                }
                else
                {
                    fp.ObjectName = localFileName;
                }
                fp.Disktype = string.Empty;
                fp.ObjectType = enumOSObjectType.Reg;

                RegistrySecurity rs = rk.GetAccessControl(AccessControlSections.All);

                // Get Owner SID
                // -------------
                fp.OwnerSid = new Sid(rs.GetOwner(Type.GetType("System.Security.Principal.SecurityIdentifier")).Value);

                // Get Owner Name (do name lookup later)
                // --------------
                //                fp.OwnerName = rs.GetOwner(Type.GetType("System.Security.Principal.NTAccount")).Value;

                // Get Access Rules collection
                // ---------------------------
                AuthorizationRuleCollection obAccessRules =
                    rs.GetAccessRules(true, true, Type.GetType("System.Security.Principal.SecurityIdentifier"));
                if (null != obAccessRules)
                {
                    foreach (RegistryAccessRule fsAccessRule in obAccessRules)
                    {
                        if (fsAccessRule.InheritanceFlags == InheritanceFlags.None || fsAccessRule.PropagationFlags != PropagationFlags.InheritOnly)
                        {
                            FileAccessRight ar = new FileAccessRight();
                            ar.IsInHerited = fsAccessRule.IsInherited == true
                                                 ? Collector.Constants.Yes
                                                 : Collector.Constants.No;
                            ar.ID = new Sid(fsAccessRule.IdentityReference.Value);
                            ar.FileSystemRights = (int)fsAccessRule.RegistryRights;
                            ar.AccessType = (int)fsAccessRule.AccessControlType;
                            fp.AddFileAccessRight(ar);
                        }
                    }
                }
                // Get Audit Rules collection
                // ---------------------------
                AuthorizationRuleCollection fsAuditRules =
                    rs.GetAuditRules(true, true, Type.GetType("System.Security.Principal.SecurityIdentifier"));
                if (null != fsAuditRules)
                {
                    foreach (RegistryAuditRule fsAuditRule in fsAuditRules)
                    {
                        if (fsAuditRule.InheritanceFlags == InheritanceFlags.None || fsAuditRule.PropagationFlags != PropagationFlags.InheritOnly)
                        {
                            FileAuditSetting fas = new FileAuditSetting();
                            fas.IsInHerited = fsAuditRule.IsInherited == true
                                                  ? Collector.Constants.Yes
                                                  : Collector.Constants.No;
                            fas.ID = new Sid(fsAuditRule.IdentityReference.Value);
                            fas.FileSystemRights = (int)fsAuditRule.RegistryRights;
                            fas.AuditFlags = (int)fsAuditRule.AuditFlags;
                            fp.AddFileAuditSetting(fas);
                        }
                    }
                }

                registryPermissionsList.Add(fp);

            }
            catch (Exception ex)
            {
                numWarnings++;
                string msg = string.Format("Failed to get registry permissions for {0}: {1}", rk.Name, ex);
                logX.loggerX.Warn(msg);
            }
            return numWarnings;
        }

        #endregion

    }
}
