using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectRole : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region Ctors

        public Form_SelectRole(int snapshotId, int dbId)
        {
            InitializeComponent();

            m_Snapshot = Snapshot.GetSnapShot(snapshotId);
            m_DbId = dbId;
        }

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_SelectRole");
        private Sql.Snapshot m_Snapshot;
        private int m_DbId;
        private string m_SelectedRole;

        #endregion

        #region Properties

        public string SelectedRole
        {
            get { return m_SelectedRole; }
        }

        #endregion

        #region Helpers

        private void LoadRoles()
        {
            // Open connection to repository and query permissions.
            logX.loggerX.Info("Retrieve Snapshot Roles");

            if (m_Snapshot != null)
            {
                List<Sql.DatabasePrincipal> rolelist = Sql.DatabasePrincipal.GetSnapshotDbRoles(m_Snapshot.SnapshotId, m_DbId);
                foreach (Sql.DatabasePrincipal role in rolelist)
                {
                    _listView_Roles.Items.Add(role.Name);
                    _listView_Roles.Items[_listView_Roles.Items.Count - 1].SubItems.Add(role.TypeStr);
                }
            }
        }

        public static string GetRoleName(int snapshotId, int dbid)
        {
            Form_SelectRole form = new Form_SelectRole(snapshotId, dbid);
            form.LoadRoles();
            DialogResult rc = form.ShowDialog();
            if (rc == DialogResult.OK)
            {
                return form.SelectedRole;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Events

        private void _button_OK_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            m_SelectedRole = _listView_Roles.SelectedItems[0].Text;
        }

        private void _listView_Roles_DoubleClick(object sender, EventArgs e)
        {
            _button_OK_Click(sender, e);
            DialogResult = DialogResult.OK;
        }

        private void _listView_Roles_SelectedIndexChanged(object sender, EventArgs e)
        {
            _button_OK.Enabled = _listView_Roles.SelectedItems.Count != 0;
        }

        #endregion

        private void button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_SelectRole_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SelectRoleHelpTopic);
        }
    }
}

