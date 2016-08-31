/******************************************************************
 * Name: ICommandHandler
 *
 * Description: Declares ICommandHandler interface.   This interface
 * is registered with the controller when a view/form modifies the
 * menu/tool bars.
 * 
 * Common commands that act on the current selection, call this interface
 * method to process the command.   For example the Delete menu option will
 * call a method on this interface to process the request.
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
    interface ICommandHandler
    {
        void ProcessCommand(Utility.ViewSpecificCommand command);
    }
}
