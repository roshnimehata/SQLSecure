using Idera.SQLsecure.UI.Console.Forms;
using Idera.SQLsecure.UI.Console.Utility;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class AddEditFolders : UserControl
    {
        #region Fields

        private readonly Form_FolderPath m_frmFolder = new Form_FolderPath();

        #endregion

        #region Ctors

        public AddEditFolders()
        {
            InitializeComponent();
            EnableEditRemoveButtons(false);
        }

        #endregion

        #region public string TargetServerName

        private string m_targetServerName = string.Empty;

        public string TargetServerName
        {
            get { return m_targetServerName; }
            set { m_targetServerName = value.ToLower(); }
        } 

        #endregion

        #region Private Methods

        private void EnableEditRemoveButtons(bool enable)
        {
            _btn_Edit_Folder.Enabled = enable;
            _btn_Remove_Folder.Enabled = enable;
        }

        private string ConvertLocalPathToUNCPath(string path)
        {
            string UNCPath = path;
            if (path[1] == ':')
            {
                UNCPath = string.Format(@"\\{0}\{1}${2}", m_targetServerName, path[0], path.Substring(2));
            }
            return UNCPath;
        }

        private string ConvertUNCPathToLocalPath(string path)
        {
            string localPath = path;

            if (path.Contains(m_targetServerName))
            {
                if (path.Contains(@"$\"))
                {
                    localPath =
                        string.Format(@"{0}:\{1}", path[path.IndexOf(@"$\") - 1], path.Substring(path.IndexOf(@"$\") + 2));
                }
                else if (path.EndsWith(@"$"))
                {
                    localPath =
                        string.Format(@"{0}:\", path[path.IndexOf(m_targetServerName) + m_targetServerName.Length + 1]);
                }
            }

            return localPath;
        }

        private void ShowFoundFolderPathWarning(int foundIndex, string folderPath)
        {
            MsgBox.ShowWarning(ErrorMsgs.FolderExistsCaption, string.Format(ErrorMsgs.FolderExistsMsg, folderPath));
            lstFilePermissionFolders.SelectedIndex = foundIndex;
            lstFilePermissionFolders.Focus();
        }

        private int FindFolderInList(string folder)
        {
            int foundIndex = Constants.NOT_FOUND;

            for (int index=0;index < lstFilePermissionFolders.Items.Count; index++)
            {
                string item = (string)lstFilePermissionFolders.Items[index];
                if (folder.ToLower() == item.ToLower())
                {
                    foundIndex = index;
                    break;
                }
            }

            return foundIndex;
        }

        private int TryToFindExistingFolder(string folderPath)
        {
            int foundIndex = FindFolderInList(folderPath);

            if (foundIndex > Constants.NOT_FOUND)
            {
                ShowFoundFolderPathWarning(foundIndex, folderPath);
            }
            else
            {
                //try to find equivalent folder path
                string path;

                Uri pathUri = new Uri(folderPath);

                if (pathUri.IsUnc)
                {
                    path = ConvertUNCPathToLocalPath(folderPath);
                }
                else
                {
                    path = ConvertLocalPathToUNCPath(folderPath);
                }

                foundIndex = FindFolderInList(path);
                if (foundIndex > Constants.NOT_FOUND)
                {
                    ShowFoundFolderPathWarning(foundIndex, folderPath);
                }
            }

            return foundIndex;
        }

        #endregion

        #region Public Methods

        public void SetFolders(string[] folderList)
        {
            lstFilePermissionFolders.Items.Clear();

            if (folderList!=null &&
                folderList.Length > 0)
            { 
                lstFilePermissionFolders.Items.AddRange(folderList);
            }
        }

        public string[] GetFolders()
        {
            List<string> folders = new List<string>(lstFilePermissionFolders.Items.Count);

            foreach (object item in lstFilePermissionFolders.Items)
            {
                folders.Add(item.ToString());
            }

            return folders.ToArray();
        }

        #endregion

        public event EventHandler FoldersUpdated;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnFoldersUpdated()
        {
            if (FoldersUpdated != null)
            {
                FoldersUpdated(this, EventArgs.Empty);
            }
                
        }

        #region Event Handlers

        private void _btn_Add_Folder_Click(object sender, EventArgs e)
        {
            m_frmFolder.Text = string.Format(ErrorMsgs.AddEditFolderPathCaption, ErrorMsgs.AadNewCaption);
            m_frmFolder.FolderPath = string.Empty;
            DialogResult result = m_frmFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                int foundIndex = TryToFindExistingFolder(m_frmFolder.FolderPath.ToLower());

                if (foundIndex == Constants.NOT_FOUND)
                {
                    lstFilePermissionFolders.Items.Add(m_frmFolder.FolderPath);
                    OnFoldersUpdated();
                }
            }
        }

        private void _btn_Edit_Folder_Click(object sender, EventArgs e)
        {
            string oldValue = lstFilePermissionFolders.Items[lstFilePermissionFolders.SelectedIndex].ToString();
            m_frmFolder.Text = string.Format(ErrorMsgs.AddEditFolderPathCaption, ErrorMsgs.EditCaption);
            m_frmFolder.FolderPath = oldValue;

            DialogResult result = m_frmFolder.ShowDialog();

            if (result == DialogResult.OK &&
                oldValue != m_frmFolder.FolderPath)
            {
                int foundIndex = TryToFindExistingFolder(m_frmFolder.FolderPath.ToLower());

                if (foundIndex == Constants.NOT_FOUND)
                {
                    lstFilePermissionFolders.Items[lstFilePermissionFolders.SelectedIndex] = m_frmFolder.FolderPath;
                    OnFoldersUpdated();
                }
            }
        }

        private void _btn_Remove_Folder_Click(object sender, EventArgs e)
        {
            string selectedFolder = lstFilePermissionFolders.Items[lstFilePermissionFolders.SelectedIndex].ToString();
            string removeFolderMessage = string.Format(ErrorMsgs.ConfirmAuditFolderPathDeleteMsg, selectedFolder);
            if (MsgBox.ShowConfirm(ErrorMsgs.DeleteAuditFolderCaption, removeFolderMessage) == DialogResult.Yes)
            {
                lstFilePermissionFolders.Items.RemoveAt(lstFilePermissionFolders.SelectedIndex);
                OnFoldersUpdated();
            }
        }

        private void lstFilePermissionFolders_MouseClick(object sender, MouseEventArgs e)
        {
            if (lstFilePermissionFolders.SelectedIndex == Constants.NOT_SELECTED)
            {
                EnableEditRemoveButtons(false);
            }
            else
            {
                EnableEditRemoveButtons(true); 
            }
        }

        #endregion
    }
}
