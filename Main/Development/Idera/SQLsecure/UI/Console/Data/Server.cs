/******************************************************************
 * Name: Server.cs
 *
 * Description: Server Summary view data class.
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
    class Server : Interfaces.IDataContext
    {
        #region Fields

        private Sql.RegisteredServer m_ServerInstance;
        private String m_Name;

        #endregion

        #region Ctors

        public Server(Sql.RegisteredServer serverIn)
        {
            Debug.Assert(serverIn != null, "Permission Explorer called with null server");

            m_ServerInstance = serverIn;
            m_Name = serverIn.ConnectionName;
        }

        #endregion

        #region Properties

        public Sql.RegisteredServer ServerInstance
        {
            get { return m_ServerInstance; }
        }

        #endregion

        #region IDataContext

        String Interfaces.IDataContext.Name
        {
            get { return m_Name; }
        }

        #endregion
    }
}
