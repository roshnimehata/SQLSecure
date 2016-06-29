/******************************************************************
 * Name: NodeTag.cs
 *
 * Description: Left pane tree view tag object class.
 *
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
    internal class NodeTag
    {
        #region Fields

        private Interfaces.IDataContext m_DataContext;
        private Utility.View m_View;

        #endregion

        #region Ctors

        public NodeTag(
                Interfaces.IDataContext dataContextIn,
                Utility.View viewIn
            )
        {
            m_DataContext = dataContextIn;
            m_View = viewIn;
        }

        #endregion

        #region Properties

        public Interfaces.IDataContext DataContext
        {
            get { return m_DataContext; }
        }
        public Utility.View View
        {
            get { return m_View; }
        }

        #endregion
    }
}
