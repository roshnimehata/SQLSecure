/******************************************************************
 * Name: Reports.cs
 *
 * Description: Reports view generic data class to call with no data
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
    class Report : Interfaces.IDataContext
    {
        #region Fields

        private string m_Name;
        private string m_ServerInstance;
        private bool m_RunReport;

        #endregion

        #region Ctors

        public Report(string nameIn) : this(nameIn, string.Empty, false) {}

        public Report(string nameIn, bool runReport) : this(nameIn, string.Empty, runReport) { }

        public Report(string nameIn, string ServerInstance, bool runReport)
        {
            m_Name = nameIn;
            m_ServerInstance = ServerInstance;
            m_RunReport = runReport;
        }

        #endregion

        #region IDataContext

        string Interfaces.IDataContext.Name
        {
            get { return m_Name; }
        }

        public string ServerInstance
        {
            get { return m_ServerInstance; }
        }

        public bool RunReport
        {
            get { return m_RunReport; }
        }

        #endregion
    }
}
