/******************************************************************
 * Name: UnderConstruction.cs
 *
 * Description: Under construction view dummy data class.
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
    class UnderConstruction : Interfaces.IDataContext
    {
        #region Fields

        private String m_Name;

        #endregion

        #region Ctors

        public UnderConstruction (String nameIn)
        {
            m_Name = nameIn;
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
