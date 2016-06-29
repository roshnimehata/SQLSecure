using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SetReportServer : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Fields

        private SQL.ReportsRecord m_ReportsRecord;

        #endregion

        #region Ctors

        public Form_SetReportServer()
        {
            InitializeComponent();

            this.Text = Utility.ErrorMsgs.ReportsCaption;
            _label_HeaderText.Text = Description;

            // load values
            m_ReportsRecord = new SQL.ReportsRecord();
            m_ReportsRecord.Read();

            textComputer.Text = m_ReportsRecord.reportServer;
            textFolder.Text = m_ReportsRecord.reportFolder;

            //------------------------------------------------------
            // Make controls read only unless user has admin access
            //------------------------------------------------------
            if (!Program.gController.isAdmin)
            {
                textComputer.Enabled = false;
                textFolder.Enabled = false;

                // change buttons
                _button_OK.Visible = false;
                _button_Cancel.Text = CloseText;
                _button_Cancel.Enabled = true;
                this.AcceptButton = _button_Cancel;
            }
        }

        #endregion

        #region Constants

        private static string Description = Utility.Constants.PRODUCT_STR + @" uses the " + Utility.Constants.PRODUCT_STR + @" Reports package and Microsoft Reporting Services to generate reports. If the following fields are blank, ensure you correctly installed and configured this software.";
        private const string CloseText = @"Close";

        #endregion

        #region Helpers


        private void showHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SetReportServerHelpTopic);
        }

        #endregion

        #region Events

        private void _button_Help_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            showHelpTopic();

            Cursor = Cursors.Default;
        }

        private void _button_OK_Click(object sender, EventArgs e)
        {
            if (textComputer.Text.Trim() != m_ReportsRecord.reportServer
                 || textFolder.Text.Trim() != m_ReportsRecord.reportFolder)
            {
                if (textComputer.Text.Trim().Length == 0)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ReportsCaption, Utility.ErrorMsgs.ReportsComputerBad);
                    DialogResult = DialogResult.None;
                    return;
                }
                if (textFolder.Text.Trim().Length == 0)
                {
                    Utility.MsgBox.ShowError(Utility.ErrorMsgs.ReportsCaption, Utility.ErrorMsgs.ReportsFolderBad);
                    DialogResult = DialogResult.None;
                    return;
                }

                m_ReportsRecord.reportServer = textComputer.Text;
                m_ReportsRecord.reportFolder = textFolder.Text;

                m_ReportsRecord.Write();
            }
        }

        #endregion
    }
}

