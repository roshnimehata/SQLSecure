/******************************************************************
 * Name: IRefresh
 *
 * Description: Declares IRefresh interface.   This interface
 * provides a refresh method for view to be called globally
 * 
 * Common commands that act on the views, call this interface
 * method to process the command.
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
    interface IRefresh
    {
        void RefreshView();
    }
}
