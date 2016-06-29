using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SelectFolder : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        #region fields

        #endregion

        #region ctors

        public Form_SelectFolder(IEnumerable<string> folders, string selectedfolder)
        {
            InitializeComponent();

            foreach (string folder in folders)
            {
                _listView_Folders.Items.Add(folder);
                if (folder.Equals(selectedfolder,StringComparison.CurrentCultureIgnoreCase))
                {
                    _listView_Folders.Items[_listView_Folders.Items.Count - 1].Selected = true;
                }
            }
        }

        #endregion

        #region properties

        public string SelectedFolder
        {
            get
            {
                string folder = string.Empty;

                if (_listView_Folders.SelectedItems.Count == 1)
                {
                    folder = _listView_Folders.SelectedItems[0].Text;
                }
                return folder;
            }
        }

        #endregion

        #region helpers

        private static void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.SelectTargetFolderHelpTopic);
        }

        #endregion

        #region methods

        public static string Process(IEnumerable<string> folders, string selectedfolder)
        {
            Form_SelectFolder frm = new Form_SelectFolder(folders, selectedfolder);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return frm.SelectedFolder;
            }

            return string.Empty;
        }

        #endregion

        #region Events

        private void _button_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void _listView_Folders_DoubleClick(object sender, EventArgs e)
        {
            _button_OK_Click(sender, e);
        }

        private void _listView_Folders_SelectedIndexChanged(object sender, EventArgs e)
        {
            _button_OK.Enabled = _listView_Folders.SelectedItems.Count == 1;
        }

        private void Form_SelectFolder_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        #endregion
    }
}

