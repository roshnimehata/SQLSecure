/******************************************************************
 * Name: MenuConfiguration.cs
 *
 * Description: Menu and tool bar configuration class.   This is a 
 * helper class used to configure the menu and tool bars based on 
 * current selection.
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Utility
{
    #region Items Enum
    public enum MenuItems_File : int
    {
        // ignore always true Connect,
        ConnectionProperties,
        NewSQLServer,
        NewLogin,
        ManageLicense,
        Print,
        // ignore always true Exit,

        SIZE // always the last item, so it tells the array size
    }

    public enum MenuItems_Edit : int
    {
        Remove,
        ConfigureDataCollection,
        Properties,

        SIZE // always the last item, so it tells the array size
    }

    public enum MenuItems_View : int
    {
        // ignore always true Tasks,
        // ignore always true ConsoleTree,
        // ignore always true Toolbar,
        CollapseAll,
        ExpandAll,
        GroupByColumn,
        Refresh,

        SIZE // always the last item, so it tells the array size
    }

    public enum MenuItems_Permissions : int
    {
        UserPermissions,
        ObjectPermissions,

        SIZE // always the last item, so it tells the array size
    }

    public enum MenuItems_Snapshots : int
    {
        Collect,
        Baseline,
        GroomingSchedule,
        //CheckIntegrity,

        SIZE // always the last item, so it tells the array size
    }

    public enum ToolItems_Standard : int
    {
        NewSQLServer,
        NewLogin,
        Print,
        Remove,
        ConfigureDataCollection,
        Properties,
        Refresh, 
        UserPermissions,
        ObjectPermissions,
        Collect,
        Baseline,
        //CheckIntegrity,
        // ignore always true Help,

        SIZE // always the last item, so it tells the array size
    }

    #endregion

    #region Items State Classes
    public abstract class ItemsState
    {
        private Boolean[] m_StateArray;
        internal Security.Functions[] m_Function;

        private Boolean isIndexValid(int indexIn)
        {
            return (indexIn >= 0 && indexIn < m_StateArray.Length);
        }

        public ItemsState(int sizeIn)
        {
            m_StateArray = new Boolean[sizeIn];
            m_Function = new Security.Functions[sizeIn];
        }

        public Boolean this[int index]
        {
            get { return (isIndexValid(index) ? (m_StateArray[index] && Program.gController.Permissions.hasSecurity(m_Function[index])) : false); }
            set { if (isIndexValid(index)) { m_StateArray[index] = value; } }
        }

        public int Size
        {
            get { return m_StateArray.Length; }
        }
    }

    public class FileItemsState : ItemsState
    {
        public FileItemsState()
            : base((int)MenuItems_File.SIZE)
        {
            // Assign the menu items to security functions
            base.m_Function[(int)MenuItems_File.ConnectionProperties] = Security.Functions.Properties;
            base.m_Function[(int)MenuItems_File.NewSQLServer] = Security.Functions.AuditSQLServer;
            base.m_Function[(int)MenuItems_File.NewLogin] = Security.Functions.NewLogin;
            base.m_Function[(int)MenuItems_File.ManageLicense] = Security.Functions.ManageLicense;
            base.m_Function[(int)MenuItems_File.Print] = Security.Functions.Print;
        }
    }

    public class EditItemsState : ItemsState
    {
        public EditItemsState()
            : base((int)MenuItems_Edit.SIZE)
        {
            base.m_Function[(int)MenuItems_Edit.Remove] = Security.Functions.Delete;
            base.m_Function[(int)MenuItems_Edit.ConfigureDataCollection] = Security.Functions.ConfigureAuditSettings;
            base.m_Function[(int)MenuItems_Edit.Properties] = Security.Functions.Properties;
        }
    }

    public class ViewItemsState : ItemsState
    {
        public ViewItemsState()
            : base((int)MenuItems_View.SIZE)
        {
            base.m_Function[(int)MenuItems_View.CollapseAll] = Security.Functions.ChangeUI;
            base.m_Function[(int)MenuItems_View.ExpandAll] = Security.Functions.ChangeUI;
            base.m_Function[(int)MenuItems_View.GroupByColumn] = Security.Functions.ChangeUI;
            base.m_Function[(int)MenuItems_View.Refresh] = Security.Functions.Refresh;
        }
    }

    public class PermissionsItemsState : ItemsState
    {
        public PermissionsItemsState()
            : base((int)MenuItems_Permissions.SIZE)
        {
            base.m_Function[(int)MenuItems_Permissions.UserPermissions] = Security.Functions.UserPermissions;
            base.m_Function[(int)MenuItems_Permissions.ObjectPermissions] = Security.Functions.ObjectPermissions;
        }
    }

    public class SnapshotsItemsState : ItemsState
    {
        public SnapshotsItemsState()
            : base((int)MenuItems_Snapshots.SIZE)
        {
            base.m_Function[(int)MenuItems_Snapshots.Collect] = Security.Functions.Collect;
            base.m_Function[(int)MenuItems_Snapshots.Baseline] = Security.Functions.Baseline;
            base.m_Function[(int)MenuItems_Snapshots.GroomingSchedule] = Security.Functions.GroomingSchedule;
        }
    }

    public class ToolStandardItemsState : ItemsState
    {
        public ToolStandardItemsState()
            : base((int)ToolItems_Standard.SIZE)
        {
            base.m_Function[(int)ToolItems_Standard.NewSQLServer] = Security.Functions.AuditSQLServer;
            base.m_Function[(int)ToolItems_Standard.NewLogin] = Security.Functions.NewLogin;
            base.m_Function[(int)ToolItems_Standard.Remove] = Security.Functions.Delete;
            base.m_Function[(int)ToolItems_Standard.ConfigureDataCollection] = Security.Functions.ConfigureAuditSettings;
            base.m_Function[(int)ToolItems_Standard.Collect] = Security.Functions.Collect;
            base.m_Function[(int)ToolItems_Standard.Baseline] = Security.Functions.Baseline;
        }
    }

    #endregion

    public class MenuConfiguration
    {
        #region Fields
        private FileItemsState m_File = new FileItemsState();
        private EditItemsState m_Edit = new EditItemsState();
        private ViewItemsState m_View = new ViewItemsState();
        private PermissionsItemsState m_Permissions = new PermissionsItemsState();
        private SnapshotsItemsState m_Snapshots = new SnapshotsItemsState();
        #endregion

        #region Ctors

        public MenuConfiguration()
        {
            //This is the configuration for the connected state which is assumed
            //because if not connected or if just a viewer, there will be no security
            //which will override the values and turn them off
            m_File[(int)MenuItems_File.ConnectionProperties] = true;
            m_File[(int)MenuItems_File.NewSQLServer] = true;
            m_File[(int)MenuItems_File.NewLogin] = true;
            m_File[(int)MenuItems_File.ManageLicense] = true;
            m_File[(int)MenuItems_File.Print] = true;

            m_View[(int)MenuItems_View.Refresh] = true;

            m_Permissions[(int)MenuItems_Permissions.UserPermissions] = true;
            m_Permissions[(int)MenuItems_Permissions.ObjectPermissions] = true;

            m_Snapshots[(int)MenuItems_Snapshots.GroomingSchedule] = true;
        }

        #endregion

        #region Properties
        public FileItemsState FileItems
        {
            get { return m_File; }
            set { m_File = value; }
        }
        public EditItemsState EditItems
        {
            get { return m_Edit; }
            set { m_Edit = value; }
        }
        public ViewItemsState ViewItems
        {
            get { return m_View; }
            set { m_View = value; }
        }
        public PermissionsItemsState PermissionsItems
        {
            get { return m_Permissions; }
            set { m_Permissions = value; }
        }
        public SnapshotsItemsState SnapshotItems
        {
            get { return m_Snapshots; }
            set { m_Snapshots = value; }
        }
        public ToolStandardItemsState ToolStandardItems
        {
            get
            {
                ToolStandardItemsState ret = new ToolStandardItemsState();

                // Set status based on the various menu items statuses.
                ret[(int)ToolItems_Standard.NewSQLServer] = m_File[(int)MenuItems_File.NewSQLServer];
                ret[(int)ToolItems_Standard.NewLogin] = m_File[(int)MenuItems_File.NewLogin];
                ret[(int)ToolItems_Standard.Print] = m_File[(int)MenuItems_File.Print];
                ret[(int)ToolItems_Standard.Remove] = m_Edit[(int)MenuItems_Edit.Remove];
                ret[(int)ToolItems_Standard.ConfigureDataCollection] = m_Edit[(int)MenuItems_Edit.ConfigureDataCollection];
                ret[(int)ToolItems_Standard.Properties] = m_Edit[(int)MenuItems_Edit.Properties];
                ret[(int)ToolItems_Standard.Refresh] = m_View[(int)MenuItems_View.Refresh];
                ret[(int)ToolItems_Standard.UserPermissions] = m_Permissions[(int)MenuItems_Permissions.UserPermissions];
                ret[(int)ToolItems_Standard.ObjectPermissions] = m_Permissions[(int)MenuItems_Permissions.ObjectPermissions];
                ret[(int)ToolItems_Standard.Collect] = m_Snapshots[(int)MenuItems_Snapshots.Collect];
                ret[(int)ToolItems_Standard.Baseline] = m_Snapshots[(int)MenuItems_Snapshots.Baseline];
                // ignore always true - ret[(int)ToolItems_Standard.Help] = true; // this is always true


                return ret;
            }
        }

        #endregion
    }
}
