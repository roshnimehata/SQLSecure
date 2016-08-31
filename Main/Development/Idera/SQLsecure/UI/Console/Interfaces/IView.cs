/******************************************************************
 * Name: IViewContext
 *
 * Description: Declares IViewContext interface.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Idera.SQLsecure.UI.Console.Interfaces
{
    interface IView
    {
        void SetContext(IDataContext contextIn);
        String HelpTopic
        {
            get;
        }
        String ConceptTopic
        {
            get;
        }
        String Title
        {
            get;
        }
    }
}
