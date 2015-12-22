using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Idera.SQLsecure.UI.Console.Utility;
using Idera.SQLsecure.UI.Console.Data;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_FilterProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private Sql.DataCollectionFilter m_Filter;
        private ServerInfo m_ServerInfo;
        private List<string> m_FiltersInListView;
        private bool m_IsEditAllowed;
        private TabControl m_HiddenPagesTabControl = new TabControl();
        private bool m_IsDirty = false;

        #endregion

        #region Helpers

        private void showHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.FilterPropertiesHelpTopic);
        }

        private void disablePropertyControls()
        {
            // General
            _txtbx_Name.Enabled = false;
            _txtbx_Description.Enabled = false;

        }

        private void fillPages()
        {
            // Fill the general page properties.
            _txtbx_Name.Text = m_Filter.FilterName;
            _txtbx_Description.Text = m_Filter.Description;
            _lbl_LastModifiedBy.Text = m_Filter.LastModifiedBy;
            _lbl_LastModifiedOn.Text = m_Filter.LastModificationTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);
            _lbl_CreatedBy.Text = m_Filter.CreatedBy;
            _lbl_CreatedOn.Text = m_Filter.CreationTime.ToLocalTime().ToString(Utility.Constants.DATETIME_FORMAT);


            // TODO: handle read only
            filterSelection1.Initialize(m_Filter, m_ServerInfo);

        }

        private void checkForChangesBeforeCancel()
        {
            // If changes have been made, warn the user.
            if (IsDirty)
            {
                if (MsgBox.ShowWarningConfirm(ErrorMsgs.FilterPropertiesCaption, ErrorMsgs.FilterSaveChangesBeforeCancelMsg) == DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                }
            }
        }

        private bool validateGeneral()
        {
            // No name flag error.
            if (_txtbx_Name.Text.Length == 0)
            {
                MsgBox.ShowError(ErrorMsgs.FilterPropertiesCaption, ErrorMsgs.FilterNoNameSpecificedMsg);
                return false;
            }

            // If filter rule name has changed and already exists for the server display error.
            if (string.Compare(_txtbx_Name.Text, m_Filter.FilterName, true) != 0
                && m_FiltersInListView.Contains(_txtbx_Name.Text))
            {
                MsgBox.ShowError(ErrorMsgs.FilterPropertiesCaption, Utility.ErrorMsgs.FilterRuleAlreadyExistsMsg);
                return false;
            }

            return true;
        }

        private void updateGeneral()
        {
            m_Filter.FilterName = _txtbx_Name.Text;
            m_Filter.Description = _txtbx_Description.Text;
        }

        #endregion

        #region Ctors

        public Form_FilterProperties(
                Sql.DataCollectionFilter filter,
                ServerInfo serverInfo,
                List<string> filtersInListView,
                bool isEditAllowed
            )
        {
            Debug.Assert(filter != null);
            Debug.Assert(serverInfo.version != Sql.ServerVersion.Unsupported);

            InitializeComponent();

            // Initialize the fields.
            m_Filter = filter;
            m_ServerInfo = serverInfo;
            m_FiltersInListView = filtersInListView;
            m_IsEditAllowed = isEditAllowed;

            // Set minimum size to the form size.
//            MinimumSize = Size;

        }

        #endregion

        #region Properties

        private bool IsDirty
        {
            get { return m_IsDirty; }
            set { m_IsDirty = value; }
        }

        #endregion

        #region Methods

        public static DialogResult Process(
                Sql.DataCollectionFilter filter,
                ServerInfo serverInfo,
                List<string> filtersInListView,
                bool isEditAllowed
            )
        {
            Debug.Assert(filter != null);
            Debug.Assert(serverInfo.version != Sql.ServerVersion.Unsupported);

            Form_FilterProperties form = new Form_FilterProperties(filter, serverInfo, filtersInListView, isEditAllowed);
            return form.ShowDialog();
        }

        #endregion

        #region Event Handlers

        private void Form_FilterProperties_Load(object sender, EventArgs e)
        {
            // Set the form text.
            Text = "Filter Properties - " + m_Filter.FilterName;

            // If the form is being displayed in non-edit mode, disable all the controls.
            if (!m_IsEditAllowed)
            {
                disablePropertyControls();
            }

            // Fill the pages.
            fillPages();            

            // Clear the dirty flag.
            IsDirty = false;
        }

        private void Form_FilterProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkForChangesBeforeCancel();
        }

        private void Form_FilterProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Cursor = Cursors.WaitCursor;

            showHelpTopic();

            Cursor = Cursors.Default;
        }

        #region General

        private void _txtbx_Name_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void _txtbx_Description_TextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        #endregion

        private void _btn_OK_Click(object sender, EventArgs e)
        {
            // If the dirty flag is set, save changes to the repository.
            bool isFilterDirty;
            isFilterDirty = filterSelection1.GetFilter(out m_Filter);
            if (isFilterDirty)
            {
                // Set filter disposition.
                m_Filter.FilterDisposition = Sql.DataCollectionFilter.Disposition.Modified;
                IsDirty = true;
            }
            if (IsDirty)
            {
                // Update header information.
                bool isOk = validateGeneral();
                if (isOk)
                {
                    updateGeneral();
                }

                // Process based on status.
                if (isOk)
                {
                    // Clear the dirty flag.
                    IsDirty = false;
                }
                else
                {
                    DialogResult = DialogResult.None;
                }
            }
        }

        private void _btn_Help_Click(object sender, EventArgs e)
        {
            showHelpTopic();
        }

        #endregion
    }
}
