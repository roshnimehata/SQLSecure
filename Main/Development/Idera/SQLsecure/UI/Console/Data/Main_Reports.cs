/******************************************************************
 * Name: Main_Reports.cs
 *
 * Description: Main_Reports view data class.
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
    class Main_Reports : Interfaces.IDataContext
    {
        #region Fields

        private String m_Name;
        private Views.View_Main_Reports.Tab m_Tab = Views.View_Main_Reports.Tab.None;

        #endregion

        #region Ctors

        public Main_Reports(String nameIn)
        {
            m_Name = nameIn;
        }

        public Main_Reports(String nameIn, Views.View_Main_Reports.Tab showTabIn)
        {
            m_Name = nameIn;
            m_Tab = showTabIn;

            if (m_Tab != Views.View_Main_Reports.Tab.None)
            {
                Program.gController.SetCurrentReport(Utility.DescriptionHelper.GetEnumDescription(showTabIn));
            }
        }

        #endregion

        #region IDataContext

        String Interfaces.IDataContext.Name
        {
            get { return m_Name; }
        }

        public Views.View_Main_Reports.Tab Tab
        {
            get { return m_Tab; }
        }

        #endregion
    }
}
