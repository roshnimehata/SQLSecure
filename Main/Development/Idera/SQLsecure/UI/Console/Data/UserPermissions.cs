/******************************************************************
 * Name: UserPermissions.cs
 *
 * Description: UserPermissions view data class.
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
    class UserPermissions : Interfaces.IDataContext
    {
        #region Fields

        private Sql.RegisteredServer m_ServerInstance;
        private String m_Name;
        private int m_SnapShotId = 0;

        #endregion

        #region Ctors

        public UserPermissions()
        {
            m_ServerInstance = null;
        }

        public UserPermissions(Sql.RegisteredServer serverIn)
        {
            Debug.Assert(serverIn != null, "User Permissions called with null server");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
        }

        public UserPermissions(Sql.RegisteredServer serverIn, int snapShotId)
        {
            Debug.Assert(serverIn != null, "User Permissions called with null server");
            Debug.Assert(snapShotId != 0, "User Permissions called with invalid SnapShotId");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
            m_SnapShotId = snapShotId;
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

        #endregion
    }
}
