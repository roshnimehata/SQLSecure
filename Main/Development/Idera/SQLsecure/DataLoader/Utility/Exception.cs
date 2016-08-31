/******************************************************************
 * Name: Exception
 *
 * Description: Data loader exception class.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace Idera.SQLsecure.Collector.Utility
{
    /// <summary>
    /// Data loader exception class.
    /// </summary>
    [Serializable]
    class DlException : System.Exception
    {
        #region Fields
        #endregion
        
        #region Helpers
        #endregion
        
        #region Ctors
        public DlException ()        
        {
        }
        public DlException(string message)
            : base(message)
        {
        }
        public DlException(
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        }
        protected DlException(
                SerializationInfo info,
                StreamingContext context
            )
            : base(info, context)
        {
        }
        #endregion
        
        #region Properties
        #endregion
        
        #region Methods
        #endregion
    }
          
}
