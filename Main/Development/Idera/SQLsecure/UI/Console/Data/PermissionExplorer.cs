/******************************************************************
 * Name: PermissionExplorer.cs
 *
 * Description: Permission Explorer view data class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Data
{
    class PermissionExplorer : Interfaces.IDataContext
    {
        #region Fields

        private Sql.RegisteredServer m_ServerInstance;
        private String m_Name = null;
        private int m_SnapShotId = 0;
        private Sql.User m_User = null;
        private string m_DatabaseName = null;
        private Views.View_PermissionExplorer.Tab m_Tab = Views.View_PermissionExplorer.Tab.None;

        #endregion

        #region Ctors

        public PermissionExplorer()
        {
            m_ServerInstance = null;
            Debug.Assert(false,"Permission Explorer called with no server");
        }

        public PermissionExplorer(Sql.RegisteredServer serverIn)
        {
            Debug.Assert(serverIn != null, "Permission Explorer called with null server");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
        }

        public PermissionExplorer(Sql.RegisteredServer serverIn, Views.View_PermissionExplorer.Tab showTabIn)
        {
            Debug.Assert(serverIn != null, "Permission Explorer called with null server");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
            m_Tab = showTabIn;

            Program.gController.SetCurrentServer(m_ServerInstance);
        }

        public PermissionExplorer(Sql.RegisteredServer serverIn, int snapShotId)
        {
            Debug.Assert(serverIn != null, "Permission Explorer called with null server");
            Debug.Assert(snapShotId != 0, "Permission Explorer called with invalid SnapShotId");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
            m_SnapShotId = snapShotId;

            Program.gController.SetCurrentSnapshot(m_ServerInstance, snapShotId);
        }

        public PermissionExplorer(Sql.RegisteredServer serverIn, int snapShotId, Views.View_PermissionExplorer.Tab showTabIn)
        {
            Debug.Assert(serverIn != null, "Permission Explorer called with null server");
            Debug.Assert(snapShotId != 0, "Permission Explorer called with invalid SnapShotId");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
            m_SnapShotId = snapShotId;
            m_Tab = showTabIn;

            Program.gController.SetCurrentSnapshot(m_ServerInstance, snapShotId);
        }

        public PermissionExplorer(Sql.RegisteredServer serverIn, int snapShotId, Sql.User user, Views.View_PermissionExplorer.Tab showTabIn)
        {
            Debug.Assert(serverIn != null, "Permission Explorer called with null server");
            Debug.Assert(snapShotId != 0, "Permission Explorer called with invalid SnapShotId");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
            m_SnapShotId = snapShotId;
            m_User = user;
            m_Tab = showTabIn;

            Program.gController.SetCurrentSnapshot(m_ServerInstance, snapShotId);
        }

        public PermissionExplorer(Sql.RegisteredServer serverIn, 
                                  int snapShotId, 
                                  Sql.User user, 
                                  string databaseName,
                                  Views.View_PermissionExplorer.Tab showTabIn)
        {
            Debug.Assert(serverIn != null, "Permission Explorer called with null server");
            Debug.Assert(snapShotId != 0, "Permission Explorer called with invalid SnapShotId");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
            m_SnapShotId = snapShotId;
            m_User = user;
            m_DatabaseName = databaseName;
            m_Tab = showTabIn;

            Program.gController.SetCurrentSnapshot(m_ServerInstance, snapShotId);
        }

        #endregion

        #region IDataContext

        String Interfaces.IDataContext.Name
        {
            get { return m_Name; }
        }

        public Sql.RegisteredServer ServerInstance
        {
            get { return m_ServerInstance; }
        }

        public int SnapShotId
        {
            get { return m_SnapShotId; }
        }

        public Sql.User User
        {
            get { return m_User; }
        }

        public string DatabaseName
        {
            get { return m_DatabaseName; }
        }

        public Views.View_PermissionExplorer.Tab Tab
        {
            get { return m_Tab; }
        }

        #endregion
    }
}
