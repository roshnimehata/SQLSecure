using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    class Form_RemoveRegisteredServer
    {
        public static void Process(string toDeleteConnection)
        {
            Debug.Assert(!string.IsNullOrEmpty(toDeleteConnection));

            // Display confirmation, if user confirms remove the registered SQL Server.
            string caption = ErrorMsgs.RemoveSqlServerCaption + " - " + toDeleteConnection;
            if (MsgBox.ShowConfirm(caption,ErrorMsgs.RemoveSQLServerConfirmMsg) == DialogResult.Yes)
            {
                try
                {
                    Sql.RegisteredServer.RemoveServer(Program.gController.Repository.ConnectionString,
                                                        toDeleteConnection);
                    //Utility.MsgBox.ShowInfo(Utility.ErrorMsgs.RemoveSqlServerCaption,
                    //                            toDeleteConnection.ToUpper() + Utility.ErrorMsgs.RemoveSqlServerSuccessful);
                    Program.gController.SignalRefreshServersEvent(true, toDeleteConnection.ToUpper());
                }
                catch (Exception ex)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.RemoveSqlServerCaption, Utility.ErrorMsgs.RemoveSqlServerFailedMsg, ex);
                }
            }
        }
    }
}
