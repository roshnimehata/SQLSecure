/******************************************************************
 * Name: IDataContext.cs
 *
 * Description: IDataContext interface definition.
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
    interface IDataContext
    {
        String Name
        {
            get;
        }
    }
}
