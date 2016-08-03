using System;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.SQL;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_CreateTag : BaseDialogForm
    {
        #region Constants

        private const string Title = "Create Server Group Tag";
        private const string TitleEdit = "Edit Server Group Tag";

        private const string ErrorTitle = @"Error Creating Tag";
        private const string ErrorMessageTitle = "The tag must have a name. Please enter a name before attempting to create the tag.";
        private const string ErrorMessageExists = "The tag with this name already exists. Please enter a another name before attempting to create the tag.";

        #endregion

        #region Ctors

        public Form_CreateTag(Tag tag)
        {
            InitializeComponent();

            this.Text = TitleEdit;
            if (tag == null)
            {
                tag = new Tag();
                this.Text = Title;
            }


            ServerTag = tag;
            if (tag.IsDefault)
            {
                _textBox_TagName.ReadOnly = true;
            }
            _textBox_TagName.Text = ServerTag.Name;
            _textBox_Description.Text = ServerTag.Description;



        }


        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_CreateTag");
        private Tag m_tag;

        #endregion

        #region Properties

        public string TagName
        {
            get { return _textBox_TagName.Text; }
        }

        public string TagDescription
        {
            get { return _textBox_Description.Text; }
        }

        public Tag ServerTag
        {
            get
            {
                return m_tag;
            }

            set
            {
                m_tag = value;
            }
        }



        #endregion

        #region Methods


        public static int Process(Tag tag)
        {

            // Create the form.

            try
            {
                Form_CreateTag form = new Form_CreateTag(tag);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    TagWorker.UpdateCreateTag(form.ServerTag);
                    return TagWorker.GetTagByName(form.ServerTag.Name).Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error during tag edit\\create", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return -1;


        }

        #endregion

        #region Events


        private void _button_OK_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            string tagName = _textBox_TagName.Text.Trim();
            if (string.IsNullOrEmpty(tagName))
            {

                MsgBox.ShowError(ErrorTitle, ErrorMessageTitle);
                DialogResult = DialogResult.None;

                Cursor = Cursors.Default;
                return;
            }
            var tag = TagWorker.GetTagByName(tagName);
            if (tag != null)
            {
                MsgBox.ShowError(ErrorTitle, ErrorMessageExists);
                DialogResult = DialogResult.None;

                Cursor = Cursors.Default;
            }



        }

        private void _button_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void Form_SelectDatabase_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.ManageTagsHelpTopic);
        }

        #endregion

        private void _textBox_TagName_TextChanged(object sender, EventArgs e)
        {
            ServerTag.Name = _textBox_TagName.Text;
        }

        private void _textBox_Description_TextChanged(object sender, EventArgs e)
        {
            ServerTag.Description = _textBox_Description.Text;
        }
    }
}

