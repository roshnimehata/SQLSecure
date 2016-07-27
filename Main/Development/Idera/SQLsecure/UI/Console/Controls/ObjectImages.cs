using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class ObjectImages : UserControl
    {
        private List<Image> m_Images16 = new List<Image>();
        private List<Image> m_Images32 = new List<Image>();
        private List<Image> m_Images48 = new List<Image>();
        private List<Image> m_ImagesCommonTask = new List<Image>();
        private List<Image> m_ImagesCommonTaskSmall = new List<Image>();

        public List<Image> Images16 { get { return m_Images16; } }
        public List<Image> Images32 { get { return m_Images32; } }
        public List<Image> Images48 { get { return m_Images48; } }
        public List<Image> ImagesCommonTask { get { return m_ImagesCommonTask; } }
        public List<Image> ImagesCommonTaskSmall { get { return m_ImagesCommonTaskSmall; } }

        public ObjectImages()
        {
            InitializeComponent();

            //Cache the icons from the imagelists into a List for better performance
            //because returning images from the imagelist into grids is really slow for volumes > 1000
            foreach (Image image in AppImageList16.Images)
            {
                m_Images16.Add(image);
            }
            foreach (Image image in AppImageList32.Images)
            {
                m_Images32.Add(image);
            }
            foreach (Image image in imageList48.Images)
            {
                m_Images48.Add(image);
            }
            foreach (Image image in AppImageListCommonTask.Images)
            {
                m_ImagesCommonTask.Add(image);
            }
            foreach (Image image in AppImageListCommonTaskSmall.Images)
            {
                m_ImagesCommonTaskSmall.Add(image);
            }
        }
    }

    public static class AppIcons
    {
        private static ObjectImages m_ObjectImages = new ObjectImages();

        // This enum is used as an index to AppImageList16 images,
        // so its critical that if the enum position is changed then
        // the image list be adjusted accordingly.
        public enum Enum
        {
            SnapshotSM =  0,
            SnapshotBaseline,
            SnapshotError,
            SnapshotWarnings,
            SnapshotInProgress,
            SnapshotMore,
            SnapshotWarningPlusBaseline,
            SQLsecureActivity,
            ActivityInfo,
            ActivityWarn,
            ActivityError,           // 10
            ActivitySuccessAudit,
            ActivityFailureAudit,    
            WindowsUser,
            WindowsGroup,
            Folder,
            FolderUp,
            GridFilter,
            GridFieldChooser,
            GridGroupBy,
            GridSaveToExcel,        // 20
            AuditFilter,
            NewAuditFilter,         
            ShowTasks,
            HideTasks,
            ServerOK,
            ServerInProgress,
            ServerWarn,
            ServerError,

            // File - menu items
            Connect,
            ViewConnectionProperties,  // 30 
            AuditSQLServer,
            NewSQLsecureLogin,      
            ManageLicense,
            Print,

            // Edit - menu items
            Remove,
            ConfigureAuditSettingsSM,
            Properties,

            // Explore - menu items
            UserPermissions,
            ObjectExplorer,

            // Snapshots
            CollectDataSnapshot,    // 40 
            MarkAsBaseline,
            GroomingSchedule,       

            // Misc
            HelpSM,
            Unknown,

            // View
            Refresh,

            // Explorer bar small images
            SecuritySummary,
            ExplorePermissions,
            ReportsSM,
            ManageSqlSecure,

            // Application icon
            SQLsecure,                     // 50

            // Permissions Tab Page Icons
            AssignedPermissions,
            EffectivePermissions,
            Summary,

            // Report Icons
            Report_VulnerableFixedRoles,
            Report_AuditedSQServers,
            Report_CrossServerLoginCheck,
            Report_DangerousWindowsGroups,
            Report_DataCollectionFilters,
            Report_DBChainingEnabled,
            Report_GuestEnabledDatabases,   // 60
            Report_MailVulnerability,
            Report_MixedModeAuth,
            Report_ServerLogins,
            Report_SuspectWindowsAccounts,
            Report_SystemAdminVulnerability,
            Report_OSVulnerabitlityViaXP,
            Report_ActivityHistory,
            Report_Users,
            Report_UserPermissions,
            Report_AllObjectsWithPermissions,    // 70
            Report_ServerRoles,
            Report_DatabaseRoles,
            Report_RiskAssessment,
            Report_AssessmentComparison,
            Report_SnapshotComparison,
            Report_LoginVulnerabiliy,

            RadioButtonUnChecked,
            RadioButtonChecked,            

            // Policy Images
            Policy,
            Policy_New,
            Policy_Edit,                    // 80
            Policy_Delete,
            Folder_Assessment_Draft,        
            Folder_Assessment_Published,
            Folder_Assessment_Approved,
            High_Risk,
            Medium_Risk,
            Low_Risk,
            High_Risk_Explained,
            Medium_Risk_Explained,
            Low_Risk_Explained,
            No_Risk,

            SequenceObjects,
            Report_SuspectSqlLogins,
            ImportServers
        }

        // This enum is used as an index to AppImageList32 images
        public enum Enum32
        {
            // Explorer Bar Large images
            ManageSqlSecure = 0,
            ExplorePermissions,
            Reports,                        // also a common task
            Snapshot,
            Help,
            NewLogin,
            RegisterSQLserver,
            Report,
            ConfigureAuditSettings,
            SecuritySummary,
            Report_SuspectSqlLogins,
            ImportServers
        }

        public enum EnumImageList48
        {
            AgentStarted,
            AgentStopped,
            StatusGood,
            StatusWarning,
            StatusError
        }

        public enum EnumCommonTask
        {
            // Common Tasks
            ConfigureAuditSettingsHoverCT,
            ReportsHoverCT,
            ObjectExplorerHoverCT,
            AuditSQLServerHoverCT,
            UserPermissionsHoverCT,
            LicenseHoverCT,
            CollectDataHoverCT,
            SQLsecureLoginCT,
            Report_VulnerableFixedRoles,
            Report_AuditedSQServers,
            Report_CrossServerLoginCheck,
            Report_DangerousWindowsGroups,
            Report_DataCollectionFilters,
            Report_DBChainingEnabled,
            Report_GuestEnabledDatabases,
            Report_MailVulnerability,
            Report_MixedModeAuth,
            Report_ServerLogins,
            Report_SuspectWindowsAccounts,
            Report_SystemAdminVulnerability,
            Report_OSVulnerabitlityViaXP,
            Report_LoginVulnerability,
            Report_SuspectSqlLogins
        }

        public enum EnumCommonTaskSmall
        {
            // Common Tasks
            ConfigureAuditSettingsCTSmall,
            ReportsCTSmall,
            ObjectExplorerCTSmall,
            AuditSQLServerCTSmall,
            UserPermissionsCTSmall,
            LicenseCTSmall,
            CollectDataCTSmall,
            SQLsecureLoginCTSmall,
            Report_VulnerableFixedRoles,
            Report_AuditedSQServers,
            Report_CrossServerLoginCheck,
            Report_DangerousWindowsGroups,
            Report_DataCollectionFilters,
            Report_DBChainingEnabled,
            Report_GuestEnabledDatabases,
            Report_MailVulnerability,
            Report_MixedModeAuth,
            Report_ServerLogins,
            Report_SuspectWindowsAccounts,
            Report_SystemAdminVulnerability,
            Report_OSVulnerabitlityViaXP,
            Report_LoginVulnerability,
            Report_SuspectSqlLogins
        }

        //16x16 image handlers
        public static ImageList AppImageList16()
        {
            return m_ObjectImages.AppImageList16;
        }

        public static Image AppImage16(Enum e)
        {
            // Use the internally cached list array for performance
            return m_ObjectImages.Images16[(int)e];
        }

        public static int AppImageIndex16(Enum e)
        {
            return (int)e;
        }

        //32x32 image handlers
        public static ImageList AppImageList32()
        {
            return m_ObjectImages.AppImageList32;
        }

        public static Image AppImage32(Enum32 e)
        {
            // Use the internally cached list array for performance
            return m_ObjectImages.Images32[(int)e];
        }

        public static int AppImageIndex32(Enum32 e)
        {
            return (int)e;
        }

        //48x48 image handlers
        public static ImageList AppImageList48()
        {
            return m_ObjectImages.imageList48;
        }

        public static Image AppImage48(EnumImageList48 e)
        {
            // Use the internally cached list array for performance
            return m_ObjectImages.Images48[(int)e];
        }

        public static int AppImageIndex48(EnumImageList48 e)
        {
            return (int)e;
        }

        //Common Task image handlers
        public static ImageList AppImageListCommonTask()
        {
            return m_ObjectImages.AppImageListCommonTask;
        }

        public static Image AppImageCommonTask(EnumCommonTask e)
        {
            return m_ObjectImages.ImagesCommonTask[(int)e];
        }

        public static int AppImageIndexCommonTask(EnumCommonTask e)
        {
            return (int)e;
        }

        public static ImageList AppImageListCommonTaskSmall()
        {
            return m_ObjectImages.AppImageListCommonTaskSmall;
        }

        public static Image AppImageCommonTaskSmall(EnumCommonTaskSmall e)
        {
            return m_ObjectImages.ImagesCommonTaskSmall[(int)e];
        }

        public static int AppImageIndexCommonTaskSmall(EnumCommonTask e)
        {
            return (int)e;
        }
    }
}