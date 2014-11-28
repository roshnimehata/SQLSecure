using System;
using System.IO;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_FolderPath : Form
    {
        #region Ctors

        public Form_FolderPath()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public string FolderPath
        {
            get { return txtFolderPath.Text.Trim(); }
            set { txtFolderPath.Text = value; }
        } 

        #endregion

        #region Private Methods

        private void ShowNotValidFolderPathMessage()
        {
            MsgBox.ShowError(ErrorMsgs.FolderPathNotValidCaption,ErrorMsgs.FolderPathNotValidMsg);
        }

        private bool ValidateFolderPath()
        {
            if (FolderPath.Length == 0)
            {
                MsgBox.ShowError(ErrorMsgs.FolderPathMissingCaption, ErrorMsgs.FolderPathMissingMsg);
                return false;
            }

            try
            {
                Uri uriFolderPath = new Uri(FolderPath);

                if (uriFolderPath.Scheme != Constants.FILE_SCHEME)
                {
                    ShowNotValidFolderPathMessage();
                    return false;   
                }
            }
            catch (UriFormatException)
            {
                ShowNotValidFolderPathMessage();
                return false;
            }

            return true;
        } 

        #endregion

        #region Event Handlers

        private void _btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateFolderPath())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                txtFolderPath.Focus();
            }
        } 

        #endregion
    }
}