/******************************************************************
 * Name: ErrorMessage.cs
 *
 * Description: Error message display helper functions.
 *
 *
 * Assemblies/DLLs needed:
 *
 * (C) 2006 - Idera, a division of BBS Technologies, Inc.
 *******************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace Idera.SQLsecure.UI.Console.Utility
{
    internal static class MsgBox
    {
        static public DialogResult ShowInfo(
                String caption,
                String msg
            )
        {
            return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static public DialogResult ShowWarningConfirm(
                string caption,
                string msg
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(caption));
            Debug.Assert(!string.IsNullOrEmpty(msg));

            return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }

        static public DialogResult ShowWarning(
                string caption,
                string msg
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(caption));
            Debug.Assert(!string.IsNullOrEmpty(msg));

            return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        static public DialogResult ShowConfirm(
                string caption,
                string msg
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(caption));
            Debug.Assert(!string.IsNullOrEmpty(msg));

            return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        static public DialogResult ShowConfirmHelp(
                string caption,
                string msg,
                string helppath
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(caption));
            Debug.Assert(!string.IsNullOrEmpty(msg));

            return MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0,
                Utility.Help.GetHelpFilePath(), HelpNavigator.Topic, string.Format(Utility.Help.LINK_FMT, helppath));
        }

        static public DialogResult ShowQuestion(
                string caption,
                string msg
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(caption));
            Debug.Assert(!string.IsNullOrEmpty(msg));

            return MessageBox.Show(msg, caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        static public DialogResult ShowError(
                string caption,
                Exception exception
            )
        {
            return MessageBox.Show(exception.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        static public DialogResult ShowError(
                string caption,
                string task,
                Exception exception
            )
        {
            String dispStr = String.Format("{0} \n\nDetails: \n{1}", task, exception.Message);
            return MessageBox.Show(dispStr, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        static public DialogResult ShowError(
                string caption,
                string msg
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(caption));
            Debug.Assert(!string.IsNullOrEmpty(msg));

            return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}
