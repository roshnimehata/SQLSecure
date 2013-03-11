/******************************************************************
 * Name: ExploreSnapshots.cs
 *
 * Description: ExploreSnapshots view data class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Data
{
    class ExploreSnapshots : Interfaces.IDataContext
    {
        #region Fields

        private Sql.RegisteredServer m_ServerInstance;
        private String m_Name;

        #endregion

        #region Ctors

        public ExploreSnapshots(Sql.RegisteredServer serverIn)
        {
            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
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

        #endregion
    }
}
