using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Data;
using System.Data.SqlClient;
using System.Management;
using System.Security.Principal;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Interop;
using Path = System.IO.Path;

namespace Idera.SQLsecure.Collector.Sql
{    

    public enum enumOSObjectType
    {
        Unk,        // Unknown type
        FDir,       // Directory of Database file
        File,       // Any file
        IDir,       // Installation Directory or any Sub-directory
        DB,         // Database or log file read from SQL Server
        Reg         // Registry Object
    }   

    public class FileAuditSetting
    {
        private int auditFlags;             // Failure, Success, None
        private int fileSystemRights;
        private char isInHerited;
        private Sid id;
        public int AuditFlags
        {
            get { return auditFlags; }
            set { auditFlags = value; }
        }
        public int FileSystemRights
        {
            get { return fileSystemRights; }
            set { fileSystemRights = value; }
        }
        public char IsInHerited
        {
            get { return isInHerited; }
            set { isInHerited = value; }
        }
        public Sid ID
        {
            get { return id; }
            set { id = value; }
        }

    }

    public class FileAccessRight
    {
        private int fileSystemRights;
        private int accessType;   // Grant or Deny
        private char isInHerited;
        private Sid id;

        public int FileSystemRights
        {
            get { return fileSystemRights; }
            set { fileSystemRights = value; }
        }
        public int AccessType
        {
            get { return accessType; }
            set { accessType = value; }
        }
        public char IsInHerited
        {
            get { return isInHerited; }
            set { isInHerited = value; }
        }
        public Sid ID
        {
            get { return id; }
            set { id = value; }
        }

    }

    public class FilePermission
    {
        #region fields
        private const int EmbptyDatabaseID = int.MinValue;
        private int osObjectId;
        private enumOSObjectType objectType;    // File, File Directory, Installation Directory, Database file, Registry key
        private int databaseId = int.MinValue;  // Null if not a database file
        private string objectName;              // upto 260 char limit
        private string longObjectname = null;   // upto 32k
        private Sid ownersid;
        //private string ownerName;
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

    public class FilePermissions
    {

        #region fields
        private static LogX logX = new LogX("Idera.SQLsecure.Collector.Sql.FilePermissions");
        List<FilePermission> filePermissionsList = new List<FilePermission>();
        private int m_snapshotId;
        private string m_targetServerName;
        private int m_OSObjectCount;
        private Sql.ServerVersion m_SQLVersionEnum;

        #endregion

        #region CTOR
        public FilePermissions(int snapshotID, string targetServerName, Sql.ServerVersion SQLVersionEnum)
        {
            m_snapshotId = snapshotID;
            m_targetServerName = targetServerName;
            m_SQLVersionEnum = SQLVersionEnum;
        }
        #endregion

        private bool Is2000
        {
            get { return m_SQLVersionEnum == Sql.ServerVersion.SQL2000; }
        }
        private bool Is2005
        {
            get { return m_SQLVersionEnum == Sql.ServerVersion.SQL2005; }
        }
        private bool IsAtLeast2008
        {
            get { return m_SQLVersionEnum > Sql.ServerVersion.SQL2005 && m_SQLVersionEnum < Sql.ServerVersion.Unsupported; }
        }

        #region Public Methods

        public int NumOSObjectsWrittenToRepository
        {
            get { return m_OSObjectCount; }
        }

        public int LoadFilePermissionForServices(List<SQLService> services)
        {
            int numWarnings = 0;
            using (logX.loggerX.DebugCall())
            {
                Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
                foreach (SQLService s in services)
                {
                    try
                    {
                        // Get Disk Type (NTFS or FAT)
                        string diskType = GetDiskTypeFromLocalPath(s.FullFilePath);

                        // Convert to UNC file Name
                        string fileName = ConvertLocalPathToUNCPath(s.FullFilePath);

                        if (fileName.Contains(".exe"))
                        {
                            fileName = fileName.Substring(0, fileName.IndexOf(".exe") + 4);
                        }
                        else if (fileName.Contains(".EXE"))
                        {
                            fileName = fileName.Substring(0, fileName.IndexOf(".EXE") + 4);
                        }

                        numWarnings = GetFilePermission(fileName, enumOSObjectType.File, diskType, 0);

                        string dirName = Path.GetDirectoryName(fileName);

                        numWarnings +=
                            GetDirectoryPermissions(dirName, enumOSObjectType.IDir, diskType, 0);
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(
                            string.Format("Error Getting Service {0} File Permissions: {1}", s.FullFilePath, ex.Message));
                    }
                }

                Program.RestoreImpersonationContext(wi);
            }
            return numWarnings;
        }

        public int LoadFilePermissionsForInstallationDirectory(string directory)
        {
            int numWarnings = 0;
            using (logX.loggerX.DebugCall())
            {
                if (!string.IsNullOrEmpty(directory))
                {
                    Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
                    try
                    {
                        // Convert to UNC file Name
                        string dirName = ConvertLocalPathToUNCPath(directory);

                        // Get Disk Type (NTFS or FAT)
                        string diskType = GetDiskTypeFromLocalPath(directory);

                        numWarnings = ProcessDirectory(dirName, enumOSObjectType.IDir, diskType);
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(
                            string.Format("Failed to load permissions for Installation Directory '{0}': {1}",
                                          directory, ex.Message));
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
                else
                {
                    logX.loggerX.Error(
                        "Failed to load permissions for Installation Directory '{0}': invalid directory name");
                }
            }
            return numWarnings;
        }

        public int LoadFilePermissionsForAuditDirectory(string directory)
        {
            int numWarnings = 0;
            using (logX.loggerX.DebugCall())
            {
                if (!string.IsNullOrEmpty(directory))
                {
                    Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
                    try
                    {
                        // Convert to UNC file Name
                        string dirName = ConvertLocalPathToUNCPath(directory);

                        // Get Disk Type (NTFS or FAT)
                        string diskType = GetDiskTypeFromLocalPath(directory);

                        numWarnings = ProcessDirectory(dirName, enumOSObjectType.IDir, diskType);
                    }
                    catch (Exception ex)
                    {
                        numWarnings++;
                        logX.loggerX.Error(
                            string.Format("Failed to load permissions for Audit Directory '{0}': {1}",
                                          directory, ex.Message));
                    }
                    finally
                    {
                        Program.RestoreImpersonationContext(wi);
                    }
                }
                else
                {
                    logX.loggerX.Error(
                        "Failed to load permissions for Audit Directory '{0}': invalid directory name");
                }
            }
            return numWarnings;
        }

        public int  GetDatabaseFilePermissions(List<Database> databases)
        {
            int numWarnings = 0;
            using (logX.loggerX.DebugCall())
            {
                Program.ImpersonationContext wi = Program.SetTargetImpersonationContext();
                try
                {                    
                    foreach (Database db in databases)
                    {
                        if (db.DatabaseFileNames != null)
                        {
                            if(db.DbId == 1) // Master
                            {
                                // Also include the special DBs distmdl and mssqlsystemresource
                                if(db.DatabaseFileNames.Count > 0)
                                {
                                    string path = Path.GetDirectoryName(db.DatabaseFileNames[0]);
                                    numWarnings += AddSpecialSystemDatabaseFiles(path);
                                }
                            }
                            foreach (string f in db.DatabaseFileNames)
                            {
                                try
                                {
                                    // Convert to UNC file Name
                                    string fileName = ConvertLocalPathToUNCPath(f);

                                    // Get Disk Type (NTFS or FAT)
                                    string diskType = GetDiskTypeFromLocalPath(f);

                                    numWarnings +=
                                        GetFilePermission(fileName, enumOSObjectType.DB, diskType, db.DbId);

                                    string dirName = Path.GetDirectoryName(fileName);

                                    numWarnings +=
                                        GetDirectoryPermissions(dirName, enumOSObjectType.FDir, diskType, db.DbId);
                                }
                                catch (Exception ex)
                                {
                                    numWarnings++;
                                    logX.loggerX.Error(
                                        string.Format("Error Getting Database {0} File Permissions: {1}", f,
                                                      ex.Message));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    numWarnings++;
                    logX.loggerX.Error(string.Format("Error Getting Database File Permissions: {0}", ex.Message));
                }
                finally
                {
                    Program.RestoreImpersonationContext(wi);
                }
            }
            return numWarnings;
        }

        public bool WriteFilePermissionToRepository(string repositoryConnectionString, int startID)
        {
            bool isOK = true;
            m_OSObjectCount = startID;
            Program.ImpersonationContext wi = Program.SetLocalImpersonationContext();
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
                                foreach (FilePermission fp in filePermissionsList)
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
                                    dr[SQLServerObjectTable.ParamDiskType] = fp.Disktype.ToString();

                                    dataTableObject.Rows.Add(dr);
                                    if (dataTableObject.Rows.Count >= Constants.RowBatchSize)
                                    {
                                        try
                                        {
                                            bcpObject.WriteToServer(dataTableObject);
                                            dataTableObject.Rows.Clear();
                                        }
                                        catch (Exception ex)
                                        {
                                            logX.loggerX.Error("WriteFilePermissionToRepository failed to write Objects to Repository: ", ex.Message);
                                            throw;
                                        }

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
                                            try
                                            {
                                                bcpPermissions.WriteToServer(dataTablePermissions);
                                                dataTablePermissions.Rows.Clear();
                                            }
                                            catch (Exception ex)
                                            {
                                                logX.loggerX.Error("WriteFilePermissionToRepository failed to write Permissions Settings to Repository: ", ex.Message);
                                                throw;
                                            }
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
                                            try
                                            {
                                                bcpPermissions.WriteToServer(dataTablePermissions);
                                                dataTablePermissions.Rows.Clear();
                                            }
                                            catch (Exception ex)
                                            {
                                                logX.loggerX.Error("WriteFilePermissionToRepository failed to write Audit Settings to Repository: ", ex.Message);
                                                throw;
                                            }
                                        }
                                    }
                                }

                                if (dataTablePermissions.Rows.Count > 0)
                                {
                                    try
                                    {
                                        bcpPermissions.WriteToServer(dataTablePermissions);
                                        dataTablePermissions.Rows.Clear();
                                    }
                                    catch (Exception ex)
                                    {
                                        logX.loggerX.Error("WriteFilePermissionToRepository failed to write Permissions to Repository: ", ex.Message);
                                        throw;
                                    }
                                }
                                if (dataTableObject.Rows.Count > 0)
                                {
                                    try 
                                    {
                                        bcpObject.WriteToServer(dataTableObject);
                                    }
                                    catch (Exception ex)
                                    {
                                        logX.loggerX.Error("WriteFilePermissionToRepository failed to write Objects to Repository: ", ex.Message);
                                        throw;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                isOK = false;
                logX.loggerX.Error("WriteFilePermissionToRepository failed: ", ex.Message);
            }
            finally
            {
                Program.RestoreImpersonationContext(wi);
            }

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

                    foreach (FilePermission fp in filePermissionsList)
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
                                    if(Account.CreateUnknownAccount(fr.ID, fr.ID.SidString, out account))
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

        private int AddSpecialSystemDatabaseFiles(string path)
        {
            int numWarnings = 0;
            List<string> specialDBFiles = new List<string>();
            specialDBFiles.Add(path + "\\distmdl.mdf");
            specialDBFiles.Add(path + "\\distmdl.ldf");
            specialDBFiles.Add(path + "\\mssqlsystemresource.mdf");
            specialDBFiles.Add(path + "\\mssqlsystemresource.ldf");

            foreach (string f in specialDBFiles)
            {
                try
                {
                    // Convert to UNC file Name
                    string fileName = ConvertLocalPathToUNCPath(f);

                    // Get Disk Type (NTFS or FAT)
                    string diskType = GetDiskTypeFromLocalPath(f);

                    // must use UNC names for shares (i.e. clusters) or File.Exists always returns False
                    if (File.Exists(fileName))
                    {
                        GetFilePermission(fileName, enumOSObjectType.DB, diskType, 0);
                    }
                    else
                    {
                        // Note this check only applies to 2005 because the files don't exist in 2000 and are not with master in 2008
                        if (Is2005)
                        {
                            numWarnings++;
                            logX.loggerX.Warn(string.Format("Warning Special Database {0} does not exist", f));
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    logX.loggerX.Warn(string.Format("Error Processing Special Database {0}: {1}", f, ex.Message));
                }
            }
            return numWarnings;
        }

        private void AddFilePermision(FilePermission fp)
        {
            FilePermission fpInList;
            bool isFound = false;
            if(fp.ObjectType == enumOSObjectType.DB || fp.ObjectType == enumOSObjectType.IDir || fp.ObjectType == enumOSObjectType.FDir || fp.ObjectType == enumOSObjectType.File)
            {
                for(int x = 0; x < filePermissionsList.Count; x++)
                {
                    fpInList = filePermissionsList[x];
                    if (string.IsNullOrEmpty(fp.LongObjectname) && string.IsNullOrEmpty(fpInList.LongObjectname))
                    {
                        if (fp.ObjectName.ToLower() == fpInList.ObjectName.ToLower())
                        {
                            filePermissionsList[x] = fp;
                            isFound = true;
                            break;
                        }
                    }
                    else
                    {
                        if (fp.ObjectName.ToLower() == fpInList.ObjectName.ToLower() &&
                                fp.LongObjectname.ToLower() == fpInList.LongObjectname.ToLower())
                        {
                            filePermissionsList[x] = fp;
                            isFound = true;
                            break;
                        }
                    }
                }
            }
            if(!isFound)
            {
                filePermissionsList.Add(fp);
            }
        }
       
        private int ProcessDirectory(string dirName, enumOSObjectType eObjectType, string diskType)
        {
            int numWarnings = 0;
            enumOSObjectType useObjectType = eObjectType;
            bool skip = false;
            string[] ignoreList = {
                                    ".gif",
                                    ".htm",
                                    ".css",
                                    ".htc",
                                    ".hxi",
                                    ".log",
                                    ".out"
                                };

            logX.loggerX.Debug(string.Format("Processing Directory: {0}", dirName));

            numWarnings += GetDirectoryPermissions(dirName, useObjectType, diskType, int.MinValue);

            foreach (string fileName in Directory.GetFiles(dirName))
            {
                skip = false;
                useObjectType = eObjectType;
                foreach(string s in ignoreList)
                {
                    if(fileName.ToLower().Contains(s))
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                {
                    // note this check is only for 2008 because the files don't exist in 2000 and in 2005 are gathered as db files and
                    //     the type doesn't need to be fixed on them
                    if (IsAtLeast2008)
                    {
                        if (fileName.Contains("mssqlsystemresource.ldf"))
                        {
                            useObjectType = enumOSObjectType.DB;
                        }
                        else if (fileName.Contains("mssqlsystemresource.mdf"))
                        {
                            useObjectType = enumOSObjectType.DB;
                        }
                        else if (fileName.Contains("distmdl.mdf"))
                        {
                            useObjectType = enumOSObjectType.DB;
                        }
                        else if (fileName.Contains("distmdl.ldf"))
                        {
                            useObjectType = enumOSObjectType.DB;
                        }
                    }
                    numWarnings += GetFilePermission(fileName, useObjectType, diskType, int.MinValue);
                }
            }

            foreach (string dir in Directory.GetDirectories(dirName))
            {
                numWarnings += ProcessDirectory(dir, eObjectType, diskType);
            }

            return numWarnings;
        }

        private int GetDirectoryPermissions(string dirName, enumOSObjectType eObjectType, string diskType, int dbId)
        {
            int numWarnings = 0;
            try
            {
                FilePermission fp = new FilePermission();
                fp.DatabaseId = dbId;

                string localDirName = ConvertUNCPathToLocalPath(dirName);
                if (localDirName.Length > 260)
                {
                    fp.ObjectName = localDirName.Substring(0, 260);
                    fp.LongObjectname = localDirName;
                }
                else
                {
                    fp.ObjectName = localDirName;
                }
                fp.Disktype = diskType;
                if (eObjectType == enumOSObjectType.IDir ||
                    eObjectType == enumOSObjectType.FDir)
                {
                    fp.ObjectType = eObjectType;
                }
                else
                {
                    logX.loggerX.Error("GetDirectoryPermissions - Invalid OS Type : ", eObjectType.ToString());
                }
                DirectorySecurity ds = Directory.GetAccessControl(dirName, AccessControlSections.All);

                // Get Owner SID
                // -------------
                fp.OwnerSid = new Sid(ds.GetOwner(Type.GetType("System.Security.Principal.SecurityIdentifier")).Value);

                // Get Owner Name
                // --------------
//                fp.OwnerName = ds.GetOwner(Type.GetType("System.Security.Principal.NTAccount")).Value;


                // Get Access Rules collection
                // ---------------------------
                AuthorizationRuleCollection obAccessRules =
                    ds.GetAccessRules(true, true, Type.GetType("System.Security.Principal.SecurityIdentifier"));
                if (null != obAccessRules)
                {
                    foreach (FileSystemAccessRule fsAccessRule in obAccessRules)
                    {
                        if (fsAccessRule.InheritanceFlags == InheritanceFlags.None || fsAccessRule.PropagationFlags != PropagationFlags.InheritOnly)
                        {
                            FileAccessRight ar = new FileAccessRight();
                            ar.IsInHerited = fsAccessRule.IsInherited == true
                                                 ? Collector.Constants.Yes
                                                 : Collector.Constants.No;
                            ar.ID = new Sid(fsAccessRule.IdentityReference.Value);
                            ar.FileSystemRights = (int) fsAccessRule.FileSystemRights;
                            ar.AccessType = (int) fsAccessRule.AccessControlType;
                            fp.AddFileAccessRight(ar);
                        }
                    }
                }
                // Get Audit Rules collection
                // ---------------------------
                AuthorizationRuleCollection fsAuditRules =
                    ds.GetAuditRules(true, true, Type.GetType("System.Security.Principal.SecurityIdentifier"));
                if (null != fsAuditRules)
                {
                    foreach (FileSystemAuditRule fsAuditRule in fsAuditRules)
                    {
                        if (fsAuditRule.InheritanceFlags == InheritanceFlags.None || fsAuditRule.PropagationFlags != PropagationFlags.InheritOnly)
                        {
                            FileAuditSetting fas = new FileAuditSetting();
                            fas.IsInHerited = fsAuditRule.IsInherited == true
                                                  ? Collector.Constants.Yes
                                                  : Collector.Constants.No;
                            fas.ID = new Sid(fsAuditRule.IdentityReference.Value);
                            fas.FileSystemRights = (int) fsAuditRule.FileSystemRights;
                            fas.AuditFlags = (int) fsAuditRule.AuditFlags;
                            fp.AddFileAuditSetting(fas);
                        }
                    }
                }

                AddFilePermision(fp);
            }
            catch (Exception ex)
            {
                numWarnings++;
                string msg = string.Format("Failed to get directory permissions for {0}: {1}", dirName, ex);
                logX.loggerX.Warn(msg);
            }

            return numWarnings;
        }

        private int GetFilePermission(string fileName, enumOSObjectType eObjectType, string diskType, int dbId)
        {
            int numWarnings = 0;
            try
            {
                logX.loggerX.Verbose(string.Format("Get File Permissions for: {0}", fileName));
                FilePermission fp = new FilePermission();
                fp.DatabaseId = dbId;
                string localFileName = ConvertUNCPathToLocalPath(fileName);
                if (localFileName.Length > 260)
                {
                    fp.ObjectName = localFileName.Substring(0, 260);
                    fp.LongObjectname = localFileName;
                }
                else
                {
                    fp.ObjectName = localFileName;
                }
                fp.Disktype = diskType;
                if (eObjectType == enumOSObjectType.IDir ||
                    eObjectType == enumOSObjectType.FDir ||
                    eObjectType == enumOSObjectType.File)
                {
                    fp.ObjectType = enumOSObjectType.File;
                }
                else if (eObjectType == enumOSObjectType.DB)
                {
                    fp.ObjectType = enumOSObjectType.DB;
                }
                else
                {
                    logX.loggerX.Error("GetFilePermission - Invalid OS Type: ", eObjectType.ToString());
                }

                FileSecurity fs = File.GetAccessControl(fileName, AccessControlSections.All);

                // Get Owner SID
                // -------------
                fp.OwnerSid = new Sid(fs.GetOwner(Type.GetType("System.Security.Principal.SecurityIdentifier")).Value);

                // Get Owner Name
                // --------------
//                fp.OwnerName = fs.GetOwner(Type.GetType("System.Security.Principal.NTAccount")).Value;

                // Get Access Rules collection
                // ---------------------------
                AuthorizationRuleCollection obAccessRules =
                    fs.GetAccessRules(true, true, Type.GetType("System.Security.Principal.SecurityIdentifier"));
                if (null != obAccessRules)
                {
                    foreach (FileSystemAccessRule fsAccessRule in obAccessRules)
                    {
                        if (fsAccessRule.InheritanceFlags == InheritanceFlags.None || fsAccessRule.PropagationFlags != PropagationFlags.InheritOnly)
                        {
                            FileAccessRight ar = new FileAccessRight();
                            ar.IsInHerited = fsAccessRule.IsInherited == true
                                                 ? Collector.Constants.Yes
                                                 : Collector.Constants.No;
                            ar.ID = new Sid(fsAccessRule.IdentityReference.Value);
                            ar.FileSystemRights = (int) fsAccessRule.FileSystemRights;
                            ar.AccessType = (int) fsAccessRule.AccessControlType;
                            fp.AddFileAccessRight(ar);
                        }
                    }
                }
                // Get Audit Rules collection
                // ---------------------------
                AuthorizationRuleCollection fsAuditRules =
                    fs.GetAuditRules(true, true, Type.GetType("System.Security.Principal.SecurityIdentifier"));
                if (null != fsAuditRules)
                {
                    foreach (FileSystemAuditRule fsAuditRule in fsAuditRules)
                    {
                        if (fsAuditRule.InheritanceFlags == InheritanceFlags.None || fsAuditRule.PropagationFlags != PropagationFlags.InheritOnly)
                        {
                            FileAuditSetting fas = new FileAuditSetting();
                            fas.IsInHerited = fsAuditRule.IsInherited == true
                                                  ? Collector.Constants.Yes
                                                  : Collector.Constants.No;
                            fas.ID = new Sid(fsAuditRule.IdentityReference.Value);
                            fas.FileSystemRights = (int) fsAuditRule.FileSystemRights;
                            fas.AuditFlags = (int) fsAuditRule.AuditFlags;
                            fp.AddFileAuditSetting(fas);
                        }
                    }
                }

                AddFilePermision(fp);

            }
            catch (Exception ex)
            {
                numWarnings++;
                string msg = string.Format("Failed to get file permissions for {0}: {1}", fileName, ex);
                logX.loggerX.Warn(msg);
            }
            return numWarnings;
        }

        private string ConvertLocalPathToUNCPath(string path)
        {
            string UNCPath = path;
            if (path[1] == ':')
            {
                UNCPath = string.Format(@"\\{0}\{1}${2}", m_targetServerName, path[0], path.Substring(2));
            }
            return UNCPath;
        }

        private string ConvertUNCPathToLocalPath(string path)
        {
            string localPath = path;

            if (path.Contains(m_targetServerName))
            {
                if (path.Contains(@"$\"))
                {
                    localPath =
                        string.Format(@"{0}:\{1}", path[path.IndexOf(@"$\") - 1], path.Substring(path.IndexOf(@"$\") + 2));
                }
                else if (path.EndsWith(@"$"))
                {
                    localPath =
                        string.Format(@"{0}:\", path[path.IndexOf(m_targetServerName) + m_targetServerName.Length + 1]);
                }
            }

            return localPath;
        }

        private string GetDiskTypeFromLocalPath(string path)
        {
            string diskType = "Unknown";
            try
            {
                string drive = string.Empty;
                if (path[1] == ':')
                {
                    drive = path[0].ToString() + ":";
                }
                else
                {
                    string localPath = ConvertUNCPathToLocalPath(path);
                    if (localPath[1] == ':')
                    {
                        drive = localPath[0].ToString() + ":";
                    }
                }

                StringBuilder scopeStr = null;
                scopeStr = new StringBuilder();
                scopeStr.Append(@"\\" + m_targetServerName);
                scopeStr.Append(Idera.SQLsecure.Core.Accounts.Constants.Cimv2Root);
                // Create management scope and connect.
                ConnectionOptions options = new ConnectionOptions();

                if (Idera.SQLsecure.Core.Accounts.Path.NonWhackPrefixComputer(m_targetServerName) != Environment.MachineName)
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

                string queryString = string.Format(
                    "SELECT FileSystem FROM Win32_LogicalDisk where DeviceID = '{0}'", drive);
                SelectQuery query = new SelectQuery(queryString);
                using (
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject disk in searcher.Get())
                    {
                        diskType = (string)disk["FileSystem"];
                    }
                }
            }
            catch(Exception ex)
            {
                logX.loggerX.Error(string.Format("Error getting File System type for path {0}, {1}", path, ex.Message));
            }
            return diskType;
        }

        #endregion

    }
}
