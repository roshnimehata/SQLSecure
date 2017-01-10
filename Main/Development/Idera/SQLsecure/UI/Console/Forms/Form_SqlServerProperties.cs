using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Sql;
using Idera.SQLsecure.UI.Console.Utility;
using Infragistics.Win.UltraWinListView;
using Policy = Idera.SQLsecure.UI.Console.Sql.Policy;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SqlServerProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Constants & enums

        private enum FormTabs
        {
            General,
            Credentials,
            AuditFolders,
            Filters,
            Schedule,
            Email,
            Policies
        }

        public enum RequestedOperation
        {
            GeneralProperties,
            EditCofiguration,
            EditSchedule
        }

        private const string DESCR_GENERAL = @"View general properties for the selected SQL Server.";
        private const string DESCR_CREDENTIALS = @"Specify which credentials SQLsecure should use to collect audit data.";
        private const string DESCR_FILTERS = @"Specify which data collection filters should be used when taking a snapshot.";
        private const string DESCR_SCHEDULE = @"Specify when you want SQLsecure to take snapshots.";
        private const string DESCR_EMAIL = @"Configure email notification for snapshot status and findings.";
        private const string DESCR_INTERVIEW = @"Select which policies should audit this SQL Server.";
        private const string NoFilters = @"Collect only server level data.";

        #endregion

        #region Fields

        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.Form_WizardRegisterSQLServer");

        private RequestedOperation m_RequestedOperation;
        private bool m_IsEdit;
        private Sql.RegisteredServer m_RegisteredServer;
        private List<Sql.DataCollectionFilter> m_Filters;
        private bool m_IsDirty;
        private bool m_scheduleChanged;
        private bool m_newNotification;
        private Idera.SQLsecure.Core.Accounts.RegisteredServerNotification rSN;

        private Control m_FilterPageFocusControl;
        private ScheduleJob.ScheduleData m_scheduleData;

        private List<int> m_OrginalPolicyList;

        #endregion

        #region Properties

        private List<string> FiltersInListView
        {
            get
            {
                List<string> retList = new List<string>();

                foreach (UltraListViewItem lvi in ultraListView_Filters.Items)
                {
                    if (!isNoFilterLvi(lvi)) // skip the dummy "no filter" entry
                    {
                        retList.Add(lvi.Text);
                    }
                }

                return retList;
            }
        }

        #endregion

        #region Queries

        #endregion

        #region Helpers

        private Control getFocused(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c.Focused)
                {
                    return c;
                }
                else if (c.ContainsFocus)
                {
                    return getFocused(c.Controls);
                }
            }

            return null;
        }

        private void addNoFilterLvi()
        {
            UltraListViewItem li = ultraListView_Filters.Items.Add(null, NoFilters);
            li.Tag = null;
        }

        private bool isNoFilterLvi(UltraListViewItem lvi)
        {
            return lvi.Tag == null;
        }

        private static bool isCredentialValid(
                Sql.RegisteredServer rs
            )
        {
            // If sql login is specified and the windows user does not have
            // password, raise error.
            bool isValid = true;
            if (!string.IsNullOrEmpty(rs.SqlLogin)
                    && (!string.IsNullOrEmpty(rs.WindowsUser) && string.IsNullOrEmpty(rs.WindowsPassword)))
            {
                isValid = false;
            }
            return isValid;
        }

        private void initGeneralPage()
        {
            _lbl_ServerVal.Text = m_RegisteredServer.FullConnectionName;
            _lbl_SQLServerVersionVal.Text = m_RegisteredServer.VersionFriendlyLong;
            _lbl_SQLServerEditionVal.Text = m_RegisteredServer.Edition;
            _lbl_ReplicationVal.Text = m_RegisteredServer.ReplicationEnabled;
            _lbl_SaVal.Text = m_RegisteredServer.SaPasswordEmpty;

            _lbl_OsServerVal.Text = m_RegisteredServer.ServerName;
            _lbl_WindowsOSVal.Text = m_RegisteredServer.OS;
            _lbl_DcVal.Text = m_RegisteredServer.ServerIsDomainController;

            _lbl_CurrentSnapshotTimeVal.Text = m_RegisteredServer.CurrentCollectionTime;
            _lbl_CurrentSnapshotStatusVal.Text = m_RegisteredServer.CurrentCollectionStatus;
            _lbl_LastSuccessfulSnapshotAtVal.Text = m_RegisteredServer.LastCollectionTime;
        }

        private void initCredentialsPage()
        {
           // Debug.Assert(isCredentialValid(m_RegisteredServer));

            radioButton_WindowsAuth.Checked = (m_RegisteredServer.SQLServerAuthType == "W") ? true : false;
            radioButton_SQLServerAuth.Checked = !radioButton_WindowsAuth.Checked;

            if (radioButton_WindowsAuth.Checked)
            {
                textBox_SQLWindowsUser.Text = m_RegisteredServer.SqlLogin;
                textBox_SQLWindowsPassword.Text = m_RegisteredServer.SqlPassword;

                if (m_RegisteredServer.SqlLogin == m_RegisteredServer.WindowsUser
                    && m_RegisteredServer.SqlPassword == m_RegisteredServer.WindowsPassword)
                {
                    checkBox_UseSameAuth.Checked = true;
                }

                textbox_SqlLogin.Enabled = false;
                textbox_SqlLoginPassword.Enabled = false;
            }
            else
            {
                textbox_SqlLogin.Text = m_RegisteredServer.SqlLogin;
                textbox_SqlLoginPassword.Text = m_RegisteredServer.SqlPassword;

                textBox_SQLWindowsUser.Enabled = false;
                textBox_SQLWindowsPassword.Enabled = false;
            }

            textbox_WindowsUser.Text = m_RegisteredServer.WindowsUser;
            textbox_WindowsPassword.Text = m_RegisteredServer.WindowsPassword;

            // Disable the controls, if editing is not allowed.
            if (!m_IsEdit)
            {
                radioButton_WindowsAuth.Enabled = false;
                radioButton_SQLServerAuth.Enabled = false;
                textbox_SqlLogin.Enabled = false;
                textbox_SqlLoginPassword.Enabled = false;
                textBox_SQLWindowsUser.Enabled = false;
                textBox_SQLWindowsPassword.Enabled = false;
                checkBox_UseSameAuth.Enabled = false;
                textbox_WindowsUser.Enabled = false;
                textbox_WindowsPassword.Enabled = false;                
            }
        }

        private void InitializeAuditFoldersPage()
        {
            string[] folders = m_RegisteredServer.AuditFoldersString.Split(new string[] { Utility.Constants.AUDIT_FOLDER_DELIMITER},
                StringSplitOptions.RemoveEmptyEntries);
            addEditFoldersControl.SetFolders(folders);
        }

        private void initSchedulePage()
        {
            // Disable the controls, if editing is not allowed.

            if (!m_IsEdit)
            {
                foreach (Control c in ultraTabPageControl_Schedule.Controls)
                {
                    if (c.Name != _pnl_ScheduleIntro.Name)
                    {
                        c.Enabled = false;
                    }
                    else
                    {
                        c.Enabled = true;
                        _lbl_ScheduleIntroText.Text = Utility.ErrorMsgs.ScheduleNotAvaliableForNonAdmin;
                    }
                }
            }
            else
            {
                m_scheduleChanged = false;
                if (!ScheduleJob.GetJobSchedule(Program.gController.Repository.ConnectionString, m_RegisteredServer.JobId, out m_scheduleData))
                {
                    // If GetJobSchedule retruned false, then schedule couldn't be read and default schedule was set.
                    // Since we couldn't read the most likely cause is job doesn't exist. Set m_scheuldeChanged to
                    // true so job will be created.
                    Utility.MsgBox.ShowWarning(Utility.ErrorMsgs.SQLsecureDataCollection, Utility.ErrorMsgs.SQLServerNoJobFoundCreateWarning);
                    m_scheduleChanged = true;
                }
                m_scheduleData.snapshotretentionPeriod = m_RegisteredServer.SnapshotRetentionPeriod;
                ScheduleJob.BuildDescription(ref m_scheduleData);
                label_Schedule.Text = m_scheduleData.Description;
                if (m_scheduleData.snapshotretentionPeriod >= numericUpDown_KeepSnapshotDays.Minimum &&
                    m_scheduleData.snapshotretentionPeriod <= numericUpDown_KeepSnapshotDays.Maximum)
                {
                    numericUpDown_KeepSnapshotDays.Value = m_scheduleData.snapshotretentionPeriod;
                }

                _btn_ChangeSchedule.Enabled = m_scheduleData.Enabled;
                // When this check box is checked, then it causes the m_scheduleChanged
                // flag to get set.   So we remember what the state of the flag was and
                // then clear it if the state is not set.
                bool tempFlag = m_scheduleChanged;
                checkBox_EnableScheduling.Checked = m_scheduleData.Enabled;
                if (!tempFlag) { m_scheduleChanged = false; }

                if (Sql.ScheduleJob.IsSQLAgentStarted(Program.gController.Repository.ConnectionString))
                {
                    label_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStarted;
                    pictureBox_AgentStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStarted);
                }
                else
                {
                    label_AgentStatus.Text = Utility.ErrorMsgs.SQLServerAgentStopped;
                    pictureBox_AgentStatus.Image = AppIcons.AppImage48(AppIcons.EnumImageList48.AgentStopped);
                }
            }

        }

        private void initPoliciesPage()
        {
            if(!m_IsEdit)
            {
                ultraListView_DynamicPolicies.Enabled = false;
                ultraListView_Policies.Enabled = false;
            }
            m_OrginalPolicyList = RegisteredServer.GetPoliciesContainingServer(m_RegisteredServer.RegisteredServerId);

            // Load up the Polices
            foreach (Policy p in Program.gController.Repository.Policies)
            {
                if (!p.IsDynamic)
                {
                    UltraListViewItem li = ultraListView_Policies.Items.Add(null, p.PolicyName);
                    li.Tag = p;
                    if (m_OrginalPolicyList != null && m_OrginalPolicyList.Count > 0 && m_OrginalPolicyList.Contains(p.PolicyId))
                    {
                        li.CheckState = CheckState.Checked;
                    }
                }
                else
                {
                    UltraListViewItem li2 = ultraListView_DynamicPolicies.Items.Add(null, p.PolicyName);
                    li2.Tag = p;
                    if (m_OrginalPolicyList != null && m_OrginalPolicyList.Count > 0 && m_OrginalPolicyList.Contains(p.PolicyId))
                    {
                        li2.CheckState = CheckState.Checked;
                    }
                }
            }

        }

        private void initNotificationPage()
        {
            rSN = Idera.SQLsecure.Core.Accounts.RegisteredServerNotification.LoadRegisteredServerNotification(
                Program.gController.Repository.ConnectionString,
                Utility.SQLCommandTimeout.GetSQLCommandTimeoutFromRegistry(),
                m_RegisteredServer.RegisteredServerId,
                Program.gController.Repository.NotificationProvider.ProviderId);

            m_newNotification = !rSN.IsValid();

            if (rSN.IsValid())
            {
                textBox_Recipients.Text = rSN.Recipients;
                switch (rSN.SnapshotStatus)
                {
                    case "N":
                        checkBoxEmailForCollectionStatus.Checked = false;
                        radioButtonAlways.Enabled = false;
                        radioButton_SendEmailOnError.Enabled = false;
                        radioButton_SendEmailWarningOrError.Enabled = false;
                        break;
                    case "A":
                        checkBoxEmailForCollectionStatus.Checked = true;
                        radioButtonAlways.Checked = true;
                        break;
                    case "W":
                        checkBoxEmailForCollectionStatus.Checked = true;
                        radioButton_SendEmailWarningOrError.Checked = true;
                        break;
                    case "E":
                        checkBoxEmailForCollectionStatus.Checked = true;
                        radioButton_SendEmailOnError.Checked = true;
                        break;
                }
                switch (rSN.PolicyMetricSeverity)
                {
                    case 0:
                        checkBoxEmailFindings.Checked = false;
                        radioButtonSendEmailFindingAny.Enabled = false;
                        radioButtonSendEmailFindingHighMedium.Enabled = false;
                        radioButtonSendEmailFindingHigh.Enabled = false;
                        break;
                    case 1:
                        checkBoxEmailFindings.Checked = true;
                        radioButtonSendEmailFindingAny.Checked = true;
                        break;
                    case 2:
                        checkBoxEmailFindings.Checked = true;
                        radioButtonSendEmailFindingHighMedium.Checked = true;
                        break;
                    case 3:
                        checkBoxEmailFindings.Checked = true;
                        radioButtonSendEmailFindingHigh.Checked = true;
                        break;
                }
            }
            else
            {
                checkBoxEmailForCollectionStatus.Checked = false;
                checkBoxEmailFindings.Checked = false;
                radioButtonSendEmailFindingAny.Enabled = false;
                radioButtonSendEmailFindingHighMedium.Enabled = false;
                radioButtonSendEmailFindingHigh.Enabled = false;
                radioButtonAlways.Enabled = false;
                radioButton_SendEmailOnError.Enabled = false;
                radioButton_SendEmailWarningOrError.Enabled = false;
            }
            if (!m_IsEdit)
            {
                foreach (Control c in ultraTabPageControl_Email.Controls)
                {
                    c.Enabled = false;
                }
            }
        }

        private void initFilterPage()
        {
            // Set the list view sort option.
            //            _lstvw_Filters.Sorting = SortOrder.Ascending;

            // If no filters specified for the instance, fill the list
            // view with a dummy entry (no filter).
            // Else fill the list view with instance filters.
            if (m_Filters.Count == 0)
            {
                addNoFilterLvi();

                // Disable the delete/properties menu items.
                _tsbtn_FilterDelete.Enabled = _tsbtn_FilterProperties.Enabled = false;
                _cntxtmi_FilterDelete.Enabled = _cntxtmi_FilterProperties.Enabled = false;
            }
            else
            {
                if (m_Filters.Count <= 1)
                {
                    _cntxtmi_FilterDelete.Enabled = false;
                    _tsbtn_FilterDelete.Enabled = false;
                }
                foreach (Sql.DataCollectionFilter filter in m_Filters)
                {
                    UltraListViewItem li = ultraListView_Filters.Items.Add(null, filter.FilterName);
                    li.Tag = filter;
                    li.SubItems["colDescription"].Value = filter.Description;
                }
                ultraListView_Filters.SelectedItems.Clear();
                ultraListView_Filters.SelectedItems.Add(ultraListView_Filters.Items[0]);
                ultraListView_Filters.ActiveItem = ultraListView_Filters.Items[0];
            }

            // Set the list view selected index.
            ultraListView_Filters.Select();

            // Disable the controls, if editing is not allowed.
            if (!m_IsEdit)
            {
                _tsbtn_FilterNew.Enabled = false;
                _tsbtn_FilterDelete.Enabled = false;
                _cntxtmi_FilterDelete.Enabled = false;
                _cntxtmi_FilterNew.Enabled = false;
            }
        }

        private void filterNew()
        {
            Sql.DataCollectionFilter filter = Form_WizardCreateFilterRule.Process(m_RegisteredServer, FiltersInListView);
            if (filter != null)
            {
                // If the list view contains the "no filter" dummy entry,
                // then remove it from the list.   The Tag of the dummy entry
                // is always a null.
                if (ultraListView_Filters.Items.Count == 1 && ultraListView_Filters.Items[0].Tag == null)
                {
                    ultraListView_Filters.Items.Clear();
                }

                // Add the new filter to the filters list.
                m_Filters.Add(filter);

                // Add the new filter entry to the list view.
                UltraListViewItem li = ultraListView_Filters.Items.Add(null, filter.FilterName);
                li.Tag = filter;
                li.SubItems["colDescription"].Value = filter.Description;
                ultraListView_Filters.SelectedItems.Clear();
                ultraListView_Filters.SelectedItems.Add(ultraListView_Filters.Items[0]);
                ultraListView_Filters.ActiveItem = ultraListView_Filters.Items[0];

                // Set the dirty flag.
                m_IsDirty = true;
            }
        }

        private void filterDelete()
        {
            Debug.Assert(ultraListView_Filters.SelectedItems.Count > 0);
            if (ultraListView_Filters.Items.Count > 1)
            {
                // Get delete confirmation.
                if (MsgBox.ShowConfirm(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.ConfirmFilterRuleDeleteMsg) ==
                    DialogResult.Yes)
                {
                    // Mark all the selected items in the list view for delete,
                    // and remove from the list view.
                    foreach (UltraListViewItem lvi in ultraListView_Filters.SelectedItems)
                    {
                        Sql.DataCollectionFilter filter = lvi.Tag as Sql.DataCollectionFilter;
                        Debug.Assert(filter != null);
                        filter.FilterDisposition = Sql.DataCollectionFilter.Disposition.Deleted;
                        ultraListView_Filters.Items.Remove(lvi);
                    }

                    // If all items have been removed, add the "no filter" dummy entry.
                    if (ultraListView_Filters.Items.Count == 0)
                    {
                        addNoFilterLvi();
                    }

                    // Set the dirty flag.
                    m_IsDirty = true;
                }
            }
        }

        private void filterProperties()
        {
            // If no items are selected or the item is the no filter item,
            // then do nothing.
            if (ultraListView_Filters.SelectedItems.Count == 0 || isNoFilterLvi(ultraListView_Filters.SelectedItems[0]))
            {
                return;
            }

            // Get the first selected item.
            UltraListViewItem lvi = ultraListView_Filters.SelectedItems[0];

            // Display the properties form for the selected item.   The user
            // may change the filter properties, so we have to update the 
            // list view and set the dirty flag.
            Sql.DataCollectionFilter filter = lvi.Tag as Sql.DataCollectionFilter;
            Debug.Assert(filter != null);
            ServerVersion parsedVersion = Sql.SqlHelper.ParseVersion(m_RegisteredServer.Version);
            Idera.SQLsecure.UI.Console.Data.ServerInfo serverInfo = new Idera.SQLsecure.UI.Console.Data.ServerInfo(parsedVersion, m_RegisteredServer.SQLServerAuthType == "W", 
                m_RegisteredServer.SqlLogin, m_RegisteredServer.SqlPassword, m_RegisteredServer.FullConnectionName, Utility.Activity.TypeServerOnPremise);
            if (Form_FilterProperties.Process(filter, serverInfo, FiltersInListView, m_IsEdit)
                        == DialogResult.OK)
            {
                ultraListView_Filters.Items.Remove(lvi);
                UltraListViewItem li = ultraListView_Filters.Items.Add(null, filter.FilterName);
                li.Tag = filter;
                li.SubItems["colDescription"].Value = filter.Description;
                ultraListView_Filters.SelectedItems.Clear();
                ultraListView_Filters.SelectedItems.Add(li);
                m_IsDirty = true;
            }
        }

        private void checkForChangesBeforeCancel()
        {
            // If changes have been made, warn the user.
            if (m_IsDirty)
            {
                if (MsgBox.ShowWarningConfirm(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.SaveChangesBeforeCancelMsg) == DialogResult.Yes)
                {
                    DialogResult = DialogResult.None;
                }
            }
        }

        #endregion

        #region Ctors

        public Form_SqlServerProperties(
                RequestedOperation op,
                bool isEdit,
                Sql.RegisteredServer registeredServer,
                List<Sql.DataCollectionFilter> filters
            )
        {
            Debug.Assert(registeredServer != null);
            Debug.Assert(filters != null);

            InitializeComponent();
            addEditFoldersControl.FoldersUpdated += addEditFoldersControl_FoldersUpdated;

            // Set images.
            _cntxtmi_FilterNew.Image = _tsbtn_FilterNew.Image = AppIcons.AppImage16(AppIcons.Enum.NewAuditFilter);
            _cntxtmi_FilterDelete.Image = _tsbtn_FilterDelete.Image = AppIcons.AppImage16(AppIcons.Enum.Remove);
            _cntxtmi_FilterProperties.Image = _tsbtn_FilterProperties.Image = AppIcons.AppImage16(AppIcons.Enum.Properties);

            // Initialize the fields.
            m_RequestedOperation = op;
            m_IsEdit = isEdit;
            m_RegisteredServer = registeredServer;
            addEditFoldersControl.TargetServerName = m_RegisteredServer.ServerName;
            m_Filters = filters;

            ultraTabControl_ServerProperties.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region Process Form

        public static void Process(
                string server,
                RequestedOperation op,
                bool isEdit
            )
        {
            Debug.Assert(!string.IsNullOrEmpty(server));

            // Get registered server properties.
            bool isOk = true;
            Sql.RegisteredServer registeredServer = null;
            List<Sql.DataCollectionFilter> filters = null;
            try
            {
                Sql.RegisteredServer.GetServer(Program.gController.Repository.ConnectionString, server, out registeredServer);
                Sql.DataCollectionFilter.GetFilters(Program.gController.Repository.ConnectionString, server, out filters);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.RetrieveServerPropertiesFailedMsg, ex);
                isOk = false;
            }

            // Do sanity check on the data read from the repository.
            if (registeredServer == null)
            {
                MsgBox.ShowWarning(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.ServerNotRegistered);
                isOk = false;
            }
            if (isOk)
            {
                if (!isCredentialValid(registeredServer))
                {
                    MsgBox.ShowError(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.CredentialsInvalidMsg);
//                    isOk = false;
                }
            }

            // Construct the form object and diplay it.
            if (isOk)
            {
                Form_SqlServerProperties form = new Form_SqlServerProperties(op, isEdit, registeredServer, filters);
                if (form.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }

        #endregion

        #region Event Handlers

        private void Form_SqlServerProperties_Load(object sender, EventArgs e)
        {
            // Setup the title, and set the minimum size.
            Text = "Audited SQL Server Properties - " + m_RegisteredServer.ConnectionName.ToUpper();
            MinimumSize = Size;

            // Set focus to the appropriate tab ctrl page, based
            // on type of task.
            switch (m_RequestedOperation)
            {
                case RequestedOperation.GeneralProperties:
                    ultraTabControl_ServerProperties.SelectedTab = ultraTabControl_ServerProperties.Tabs["General"];
                    break;
                case RequestedOperation.EditCofiguration:
                    ultraTabControl_ServerProperties.SelectedTab = ultraTabControl_ServerProperties.Tabs["Filters"];
                    break;
                case RequestedOperation.EditSchedule:
                    ultraTabControl_ServerProperties.SelectedTab = ultraTabControl_ServerProperties.Tabs["Schedule"];
                    break;
            }

            // Initialize the tab pages.
            initGeneralPage();
            initCredentialsPage();
            InitializeAuditFoldersPage();
            initSchedulePage();
            initFilterPage();
            initNotificationPage();
            initPoliciesPage();
            m_IsDirty = false; // clear the dirty flag that was set as part of initialization.
        }

        #region Credentials

        private void checkBox_UseSameAuth_CheckedChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
            if (checkBox_UseSameAuth.Checked)
            {
                textbox_WindowsUser.Text = textBox_SQLWindowsUser.Text;
                textbox_WindowsPassword.Text = textBox_SQLWindowsPassword.Text;

                textbox_WindowsUser.Enabled = false;
                textbox_WindowsPassword.Enabled = false;
            }
            else
            {
                textbox_WindowsUser.Enabled = true;
                textbox_WindowsPassword.Enabled = true;
            }

        }

        private void radioButton_WindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;

            if (radioButton_WindowsAuth.Checked)
            {
                checkBox_UseSameAuth.Enabled = true;

                textBox_SQLWindowsUser.Enabled = true;
                textBox_SQLWindowsPassword.Enabled = true;
                textbox_SqlLogin.Enabled = false;
                textbox_SqlLoginPassword.Enabled = false;
            }
        }

        private void radioButton_SQLServerAuth_CheckedChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
            if (radioButton_SQLServerAuth.Checked)
            {
                checkBox_UseSameAuth.Enabled = false;
                checkBox_UseSameAuth.Checked = false;

                textBox_SQLWindowsUser.Enabled = false;
                textBox_SQLWindowsPassword.Enabled = false;
                textbox_SqlLogin.Enabled = true;
                textbox_SqlLoginPassword.Enabled = true;
            }

        }
        private void textBox_SQLWindowsUser_TextChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
            if (checkBox_UseSameAuth.Checked)
            {
                textbox_WindowsUser.Text = textBox_SQLWindowsUser.Text;
            }
        }

        private void textBox_SQLWindowsPassword_TextChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
            if (checkBox_UseSameAuth.Checked)
            {
                textbox_WindowsPassword.Text = textBox_SQLWindowsPassword.Text;
            }
        }
        private void _txtbx_SqlLogin_TextChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        private void _txtbx_SqlPassword_TextChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        private void _txtbx_WindowsUser_TextChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        private void _txtbx_WindowsPwd_TextChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        #endregion

        #region Schedule

        private void _btn_ChangeSchedule_Click(object sender, EventArgs e)
        {
            Form_ScheduleJob form = new Form_ScheduleJob(m_scheduleData);
            if (form.ShowDialog() == DialogResult.OK)
            {
                form.GetScheduleData(out m_scheduleData);
                ScheduleJob.BuildDescription(ref m_scheduleData);
                label_Schedule.Text = m_scheduleData.Description;
                m_scheduleChanged = true;
                m_IsDirty = true;
            }
        }



        private void checkBox_EnableScheduling_CheckedChanged(object sender, EventArgs e)
        {
            m_scheduleData.Enabled = checkBox_EnableScheduling.Checked;

            _btn_ChangeSchedule.Enabled = m_scheduleData.Enabled;

            ScheduleJob.BuildDescription(ref m_scheduleData);
            label_Schedule.Text = m_scheduleData.Description;

            m_IsDirty = true;
            m_scheduleChanged = true;
        }


        private void numericUpDown_KeepSnapshotDays_ValueChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        #endregion

        #region Filters

        private void _tsbtn_FilterNew_Click(object sender, EventArgs e)
        {
            filterNew();
        }

        private void _tsbtn_FilterDelete_Click(object sender, EventArgs e)
        {
            filterDelete();
        }

        private void _tsbtn_FilterProperties_Click(object sender, EventArgs e)
        {
            filterProperties();
        }

        private void _cntxtmi_FilterNew_Click(object sender, EventArgs e)
        {
            filterNew();
        }

        private void _cntxtmi_FilterDelete_Click(object sender, EventArgs e)
        {
            filterDelete();
        }

        private void _cntxtmi_FilterProperties_Click(object sender, EventArgs e)
        {
            filterProperties();
        }

        private void _lstvw_Filters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Delete)
            {
                filterDelete();
                e.Handled = true;
            }
            else if (e.KeyValue == (char)Keys.Enter)
            {
                filterProperties();
                e.Handled = true;
            }
        }

        private void ultraListView_Filters_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            // Enable/disable menu bar and context menu items, based on
            // number of items selected.
            object tag = null;
            switch (ultraListView_Filters.SelectedItems.Count)
            {
                case 0:
                    tag = null;
                    _tsbtn_FilterDelete.Enabled = _tsbtn_FilterProperties.Enabled = false;
                    _cntxtmi_FilterDelete.Enabled = _cntxtmi_FilterProperties.Enabled = false;
                    break;

                case 1:
                    tag = ultraListView_Filters.SelectedItems[0].Tag;

                    // "no filter" entry disable del & properties
                    if (ultraListView_Filters.Items.Count > 1)
                    {
                        _tsbtn_FilterDelete.Enabled = _cntxtmi_FilterDelete.Enabled = m_IsEdit && (tag != null); // incorporate no edit flag.
                    }
                    _tsbtn_FilterProperties.Enabled = _cntxtmi_FilterProperties.Enabled = (tag != null);

                    // "no filter" entry cancel selected items.
                    if (tag == null) { ultraListView_Filters.SelectedItems.Clear(); }
                    break;

                default:
                    tag = ultraListView_Filters.SelectedItems[0].Tag;
                    Debug.Assert(tag != null);
                    if (ultraListView_Filters.Items.Count > 1)
                    {
                        _tsbtn_FilterDelete.Enabled = _cntxtmi_FilterDelete.Enabled = m_IsEdit && true; // enable delete, incorporate no edit flag
                    }
                    _tsbtn_FilterProperties.Enabled = _cntxtmi_FilterProperties.Enabled = false; // disable properties
                    break;
            }

            // Set filter details.
            _rtbx_FilterDetails.Rtf = (tag != null) ? ((Sql.DataCollectionFilter)tag).FilterDetails : "";

        }

        private void ultraListView_Filters_DoubleClick(object sender, EventArgs e)
        {
            filterProperties();
        }

        private void _spltcntnr_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_FilterPageFocusControl != null)
            {
                m_FilterPageFocusControl.Focus();
                m_FilterPageFocusControl = null;
            }
        }

        private void _spltcntnr_MouseDown(object sender, MouseEventArgs e)
        {
            m_FilterPageFocusControl = getFocused(this.Controls);
        }

        #endregion

        private void _btn_OK_Click(object sender, EventArgs e)
        {
            bool isOk = true;
            bool allowRegisterAnyway = true;
            bool isWindowsCredentials = true;
            string version = string.Empty;
            string login = string.Empty;
            string password = string.Empty;
            WindowsImpersonationContext targetImpersonationContext = null;
            Forms.ShowWorkingProgress showWorking = new Forms.ShowWorkingProgress();
            // Check if credentials are valid.
            string machine = string.Empty;
            string instance = string.Empty;
            string connection = string.Empty;

            // Check email settings
            if (checkBoxEmailForCollectionStatus.Checked || checkBoxEmailFindings.Checked)
            {
                if (string.IsNullOrEmpty(Program.gController.Repository.NotificationProvider.ServerName))
                {
                    MsgBox.ShowWarning(ErrorMsgs.WarningEmailNoConfiguredTitle, ErrorMsgs.WarningEmailNoConfiguredMsg);
                }
            }

            // If the dirty flag is set, then update the repository.
            if (m_IsDirty)
            {
                // Change cursor to busy.
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                // Error message string for final display.
                StringBuilder msgBldr = new StringBuilder();

                // Process SQL credentials.
                // There are two cases to consider.   Checkbox is set or the check box is cleared.
                // If the checkbox is set, then compare the credentials to detect if there is a change.
                // If the checkbox is clear, then check if the configured value has SQL login.
                bool isCredentials = false;
                bool sqlCredentialsChanged = false;
                bool winCredentialsChanged = false;
                bool osCredentialsChanged = false;
                string sqlLogin = m_RegisteredServer.SqlLogin,
                       sqlPwd = m_RegisteredServer.SqlPassword,
                       sqlLoginAuthType = m_RegisteredServer.SQLServerAuthType,
                       wUser = m_RegisteredServer.WindowsUser,
                       wPwd = m_RegisteredServer.WindowsPassword;

                string authType = radioButton_SQLServerAuth.Checked ? "S" : "W";
                string sUser = radioButton_SQLServerAuth.Checked ? textbox_SqlLogin.Text : textBox_SQLWindowsUser.Text;
                string sPwd = radioButton_SQLServerAuth.Checked
                                  ? textbox_SqlLoginPassword.Text
                                  : textBox_SQLWindowsPassword.Text;
                if (sqlLoginAuthType != authType
                    || sqlLogin != sUser
                    || sqlPwd != sPwd
                    || wUser != textbox_WindowsUser.Text
                    || wPwd != textbox_WindowsPassword.Text)
                {
                    isCredentials = true;
                    // If no credential is specified, flag an error.
                    // If sql server credentials, check to see if they have been specified.
                    if (radioButton_SQLServerAuth.Checked)
                    {
                        if (string.IsNullOrEmpty(textbox_SqlLogin.Text))
                        {
                            msgBldr.Append(Utility.ErrorMsgs.SqlLoginNotSpecifiedMsg);
                            isOk = false;
                            allowRegisterAnyway = false;
                        }
                        else
                        {
                            sqlCredentialsChanged = (sqlLogin != sUser || sqlPwd != sPwd);
                        }
                    }
                    // If windows credentials, check to see if they have been specified.
                    else
                    {
                        if (string.IsNullOrEmpty(textBox_SQLWindowsUser.Text) ||
                            string.IsNullOrEmpty(textBox_SQLWindowsPassword.Text))
                        {
                            if (msgBldr.Length > 0)
                            {
                                msgBldr.Append("\n\n");
                            }
                            msgBldr.Append(Utility.ErrorMsgs.SqlLoginWindowsUserNotSpecifiedMsg);
                            isOk = false;
                            allowRegisterAnyway = false;
                        }
                        else
                        {
                            winCredentialsChanged = (wUser != textbox_WindowsUser.Text || wPwd != textbox_WindowsPassword.Text);
                        }

                        // Check if the account format is correct.
                        if (isOk)
                        {
                            string domain = string.Empty;
                            string user = string.Empty;
                            Path.SplitSamPath(textBox_SQLWindowsUser.Text, out domain, out user);
                            if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(user))
                            {
                                if (msgBldr.Length > 0)
                                {
                                    msgBldr.Append("\n\n");
                                }
                                msgBldr.Append(Utility.ErrorMsgs.SqlLoginWindowsUserNotSpecifiedMsg);
                                isOk = false;
                                allowRegisterAnyway = false;
                            }
                        }
                    }

                    bool isPasswordLengthValid = radioButton_SQLServerAuth.Checked
                        ? PasswordValidator.ValidatePasswordLength(textbox_SqlLoginPassword.Text)
                        : PasswordValidator.ValidatePasswordLength(textBox_SQLWindowsPassword.Text);

                    if (!isPasswordLengthValid)
                    {
                        isOk = false;
                        allowRegisterAnyway = false;
                        msgBldr.AppendFormat(Utility.Constants.PASSWORD_LENGTH_MESSAGE_FORMAT, Utility.Constants.MINIMUM_PASSWORD_LENGTH);
                    }

                    if (allowRegisterAnyway)
                    {
                        // Operating System and AD User 
                        if (string.IsNullOrEmpty(textbox_WindowsUser.Text) ||
                            string.IsNullOrEmpty(textbox_WindowsPassword.Text))
                        {
                            if (msgBldr.Length > 0)
                            {
                                msgBldr.Append("\n\n");
                            }
                            msgBldr.Append(Utility.ErrorMsgs.WindowsUserNotSpecifiedMsg);
                            isOk = false;
                            allowRegisterAnyway = true;
                            isWindowsCredentials = false;
                        }
                    }

                    // Check if the account format is correct.
                    if (allowRegisterAnyway && isWindowsCredentials)
                    {
                        string domain = string.Empty;
                        string user = string.Empty;
                        Path.SplitSamPath(textbox_WindowsUser.Text, out domain, out user);
                        if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(user))
                        {
                            if (msgBldr.Length > 0)
                            {
                                msgBldr.Append("\n\n");
                            }
                            msgBldr.Append(Utility.ErrorMsgs.WindowsUserNotSpecifiedMsg);
                            isOk = false;
                            allowRegisterAnyway = true;
                            isWindowsCredentials = false;
                        }
                        else
                        {
                            osCredentialsChanged = wUser != textbox_WindowsUser.Text || wPwd != textbox_WindowsPassword.Text;
                        }
                    }


                    // Get SQL Server properties and validate them.
                    if (allowRegisterAnyway)
                    {
                        try
                        {
                            showWorking.Show("Verifying SQL Server Credentials...", this);
                            if (radioButton_SQLServerAuth.Checked)
                            {
                                login = textbox_SqlLogin.Text;
                                password = textbox_SqlLoginPassword.Text;
                            }
                            else
                            {
                                // Impersonate ...
                                try
                                {
                                    WindowsIdentity wi =
                                        Impersonation.GetCurrentIdentity(textBox_SQLWindowsUser.Text,
                                                                         textBox_SQLWindowsPassword.Text);
                                    targetImpersonationContext = wi.Impersonate();
                                }
                                catch (Exception ex)
                                {
                                    if (msgBldr.Length > 0) { msgBldr.Remove(0, msgBldr.Length); }
                                    msgBldr.AppendFormat(
                                        "Could not validate the windows authentication credentials for connecting to SQL Server {0}.",
                                        m_RegisteredServer.ServerName.ToUpper());
                                    msgBldr.AppendFormat("\r\n\r\nError: {0}", ex.Message);
                                    logX.loggerX.Error(
                                        string.Format("Error Impersonating SQL Server Windows Login User {0}: {1}",
                                                      textBox_SQLWindowsUser.Text, ex));
                                    isOk = false;
                                    allowRegisterAnyway = false;
                                }
                            }
                            if (allowRegisterAnyway)
                            {
                                // Try connecting to SQLserver...
                                try
                                {
                                    showWorking.UpdateText(
                                        string.Format("Connecting to SQL Server {0}...", m_RegisteredServer.ServerName.ToUpper()));
                                    Sql.SqlServer.GetSqlServerProperties(m_RegisteredServer.FullConnectionName, login, password,
                                                                         out version, out machine, out instance,
                                                                         out connection, Utility.Activity.TypeServerOnPremise);
                                    if (targetImpersonationContext != null)
                                    {
                                        targetImpersonationContext.Undo();
                                        targetImpersonationContext.Dispose();
                                        targetImpersonationContext = null;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (msgBldr.Length > 0) { msgBldr.Remove(0, msgBldr.Length); }
                                    msgBldr.AppendFormat("Could not establish a connection with SQL Server {0}.",
                                                         m_RegisteredServer.ServerName.ToUpper());
                                    msgBldr.AppendFormat("\r\n\r\nError: {0}", ex.Message);
                                    machine = Path.GetComputerFromSQLServerInstance(m_RegisteredServer.ServerName);
                                    instance = Path.GetInstanceFromSQLServerInstance(m_RegisteredServer.ServerName);
                                    connection = m_RegisteredServer.ServerName;
                                    isOk = false;
                                    allowRegisterAnyway = false;
                                }
                            }
                            // Undo Impersonation
                            if (targetImpersonationContext != null)
                            {
                                targetImpersonationContext.Undo();
                                targetImpersonationContext.Dispose();
                                targetImpersonationContext = null;
                            }


                            if (isOk && allowRegisterAnyway && isWindowsCredentials)
                            {
                                string errorMsg;
                                Server.ServerAccess sa = Server.CheckServerAccess(machine, textbox_WindowsUser.Text, textbox_WindowsPassword.Text,
                                                         out errorMsg);
                                if(sa != Server.ServerAccess.OK)
                                {
                                    isOk = false;
                                    if (msgBldr.Length > 0) { msgBldr.Remove(0, msgBldr.Length); }
                                    msgBldr.Append("Warning:");
                                    msgBldr.AppendFormat("\r\nUnable to verify if account '{0}' has admin rights to computer {1}", textbox_WindowsUser.Text, machine);
                                    msgBldr.Append("\r\n\r\nDetails:\r\n");
                                    msgBldr.Append(errorMsg);
                                    msgBldr.Append("\r\n\r\nRecommendations:");
                                    msgBldr.AppendFormat("\r\nVerify account '{0}' has admin rights to computer {1}", textbox_WindowsUser.Text, machine);
                                    msgBldr.Append("\r\nVerify Firewall settings");
                                    msgBldr.AppendFormat("\r\nVerify WMI settings on computer {0}", machine);
                                    msgBldr.AppendFormat("\r\nVerify DCOM settings on computer {0}", machine);
                                    logX.loggerX.Error(
                                        string.Format("Error Connecting to Target Server with supplied Windows User {0}\r\n{1}",
                                        textbox_WindowsUser.Text, errorMsg));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (Utility.ExceptionHelper.IsSqlLoginFailed(ex))
                            {
                                msgBldr.Append("\n\n");
                                msgBldr.Append(Utility.ErrorMsgs.SqlLoginFailureMsg);
                                msgBldr.AppendFormat("\r\n\r\nError: {0}", ex.Message);
                                isOk = false;
                            }
                            else
                            {
                                msgBldr.Append("\n\n");
                                msgBldr.Append(Utility.ErrorMsgs.RetrieveServerPropertiesFailedMsg + " " + ex.Message);
                                msgBldr.AppendFormat("\r\n\r\nError: {0}", ex.Message);
                                isOk = false;
                            }
                        }
                    }

                    if (!isOk)
                    {
                        showWorking.Close();
                        Activate();
                        if (allowRegisterAnyway)
                        {
                            msgBldr.Append("\n\n");
                            msgBldr.Append("Register Anyway?");
                            System.Windows.Forms.DialogResult dr = MsgBox.ShowWarningConfirm(ErrorMsgs.RegisterSqlServerCaption, msgBldr.ToString());
                            if (dr == DialogResult.Yes)
                            {
                                isOk = true;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(msgBldr.ToString()))
                            {
                                MsgBox.ShowError(ErrorMsgs.RegisterSqlServerCaption, msgBldr.ToString());
                            }
                        }
                    }

                    showWorking.Close();
                }

                // Update the repository if validation was okay.
                if (isOk)
                {
                    m_scheduleData.snapshotretentionPeriod = (int)numericUpDown_KeepSnapshotDays.Value;
                    if (m_scheduleData.snapshotretentionPeriod != m_RegisteredServer.SnapshotRetentionPeriod)
                    {
                        try
                        {
                            Sql.RegisteredServer.UpdateRetentionPeriod(Program.gController.Repository.ConnectionString,
                                                    m_RegisteredServer.ConnectionName,
                                                    m_scheduleData.snapshotretentionPeriod);
                        }
                        catch (Exception ex)
                        {
                            MsgBox.ShowError(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.UpdateRetentionPeriodFailedMsg, ex);
                        }
                    }
                    if (isCredentials)
                    {
                        sqlLogin = radioButton_SQLServerAuth.Checked ? textbox_SqlLogin.Text : textBox_SQLWindowsUser.Text;
                        sqlPwd = radioButton_SQLServerAuth.Checked
                                          ? textbox_SqlLoginPassword.Text
                                          : textBox_SQLWindowsPassword.Text;
                        sqlLoginAuthType = radioButton_SQLServerAuth.Checked ? "S" : "W";
                        wUser = textbox_WindowsUser.Text;
                        wPwd = textbox_WindowsPassword.Text;
                        try
                        {
                            Sql.RegisteredServer.UpdateCredentials(Program.gController.Repository.ConnectionString, m_RegisteredServer.ConnectionName,
                                                                        sqlLogin, sqlPwd, sqlLoginAuthType, wUser, wPwd);
                        }
                        catch (Exception ex)
                        {
                            MsgBox.ShowError(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.UpdateCredentialsFailedMsg, ex);
                        }
                    }

                    //Update Audit Folders
                    try
                    {
                        string[] updatedFolders = addEditFoldersControl.GetFolders();
                        RegisteredServer.UpdateFolders(Program.gController.Repository.ConnectionString, m_RegisteredServer.ConnectionName, updatedFolders);
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowError(ErrorMsgs.UpdateAuditFoldersFailedCaption, ErrorMsgs.UpdateAuditFoldersFailedMsg, ex);
                    }

                    // Update filters.
                    Sql.DataCollectionFilter.UpdateFilters(Program.gController.Repository.ConnectionString,
                                                                m_RegisteredServer.ConnectionName, m_Filters);

                    // Update Policy
                    foreach (UltraListViewItem item in ultraListView_Policies.Items)
                    {
                        Policy p = (Policy)item.Tag;
                        if (item.CheckState == CheckState.Checked)
                        {
                            // If server wasn't already in policy add it
                            if (!p.IsDynamic && !m_OrginalPolicyList.Contains(p.PolicyId))
                            {
                                Sql.RegisteredServer.AddRegisteredServerToPolicy(m_RegisteredServer.RegisteredServerId, p.PolicyId, p.AssessmentId);
                            }
                        }
                        else if (item.CheckState == CheckState.Unchecked)
                        {
                            // If Server was in policy remove it
                            if (!p.IsDynamic && m_OrginalPolicyList.Contains(p.PolicyId))
                            {
                                Sql.RegisteredServer.RemoveRegisteredServerFromPolicy(m_RegisteredServer.RegisteredServerId, p.PolicyId, p.AssessmentId);
                            }
                        }

                    }

                    // Update Notifications
                    rSN.Recipients = textBox_Recipients.Text;
                    if (checkBoxEmailForCollectionStatus.Checked)
                    {
                        if (radioButtonAlways.Checked)
                        {
                            rSN.SnapshotStatus = RegisteredServerNotification.SnapshotStatusNotification_Always;
                        }
                        else if (radioButton_SendEmailWarningOrError.Checked)
                        {
                            rSN.SnapshotStatus = RegisteredServerNotification.SnapshotStatusNotification_OnWarningOrError;
                        }
                        else if (radioButton_SendEmailOnError.Checked)
                        {
                            rSN.SnapshotStatus = RegisteredServerNotification.SnapshotStatusNotification_OnlyOnError;
                        }
                    }
                    else
                    {
                        rSN.SnapshotStatus = RegisteredServerNotification.SnapshotStatusNotification_Never;
                    }
                    if (checkBoxEmailFindings.Checked)
                    {
                        if (radioButtonSendEmailFindingAny.Checked)
                        {
                            rSN.PolicyMetricSeverity = (int)RegisteredServerNotification.MetricSeverityNotificaiton.Any;
                        }
                        else if (radioButtonSendEmailFindingHighMedium.Checked)
                        {
                            rSN.PolicyMetricSeverity = (int)RegisteredServerNotification.MetricSeverityNotificaiton.OnWarningAndError;
                        }
                        else if (radioButtonSendEmailFindingHigh.Checked)
                        {
                            rSN.PolicyMetricSeverity = (int)RegisteredServerNotification.MetricSeverityNotificaiton.OnlyOnError;
                        }
                    }
                    else
                    {
                        rSN.PolicyMetricSeverity = (int)RegisteredServerNotification.MetricSeverityNotificaiton.Never;
                    }
                    if (m_newNotification)
                    {
                        rSN.RegisteredServerId = m_RegisteredServer.RegisteredServerId;
                        rSN.ProviderId = Program.gController.Repository.NotificationProvider.ProviderId;
                        rSN.AddNotificationProvider(Program.gController.Repository.ConnectionString);
                    }
                    else
                    {
                        rSN.UpdateNotificationProvider(Program.gController.Repository.ConnectionString);
                    }


                    // Update Job
                    if (m_scheduleChanged)
                    {
                        Guid jobID = Sql.ScheduleJob.AddJob(Program.gController.Repository.ConnectionString,
                                          m_RegisteredServer.ConnectionName,
                                          Program.gController.Repository.Instance,
                                          m_scheduleData);
                        //Get cached Registered Server from Repository
                        RegisteredServer rs = Program.gController.Repository.GetServer(m_RegisteredServer.ConnectionName);
                        rs.SetJobId(jobID);
                    }

                }

                // Change cursor to default.
                Cursor = System.Windows.Forms.Cursors.Default;

                // If everything is okay, clear the dirty flag.
                // else set the dialog result to none.
                if (isOk)
                {
                    m_IsDirty = false;

                    // If the credentials changed, ask if they want to update all matching credentials
                    if (isCredentials)
                    {
                        if (sqlCredentialsChanged)
                        {
                            List<RegisteredServer> updateServers = new List<RegisteredServer>();
                            Program.gController.Repository.RefreshRegisteredServers();
                            foreach (RegisteredServer server in Program.gController.Repository.RegisteredServers)
                            {
                                if (server.ConnectionName != m_RegisteredServer.ConnectionName
                                    && server.SQLServerAuthType == "S"
                                    && ((server.CaseSensitive && server.SqlLogin == sqlLogin)
                                            || server.SqlLogin.Equals(sqlLogin, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    updateServers.Add(server);
                                }
                            }

                            if (updateServers.Count > 0)
                            {
                                StringBuilder serverList = new StringBuilder();
                                foreach (RegisteredServer server in updateServers)
                                {
                                    if (serverList.Length > 0)
                                        serverList.Append(", ");
                                    serverList.Append(server.ConnectionName);
                                }

                                if (DialogResult.Yes == MsgBox.ShowConfirm("Update all matching Sql Server credentials?",
                                                                            string.Format("You have changed the SQL Server login credentials for this audited server. SQL login '{0}' is also used by the following servers: {1}\r\n\r\nDo you want to update all of these servers to use the same password?", sqlLogin, serverList.ToString())))
                                {
                                    try
                                    {
                                        foreach (RegisteredServer server in updateServers)
                                        {
                                            Sql.RegisteredServer.UpdateCredentials(Program.gController.Repository.ConnectionString, server.ConnectionName,
                                                        sqlLogin, sqlPwd, sqlLoginAuthType, server.WindowsUser, server.WindowsPassword);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MsgBox.ShowError(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.UpdateCredentialsFailedMsg, ex);
                                    }
                                    Program.gController.Repository.RefreshRegisteredServers();
                                }
                            }
                        }

                        if (winCredentialsChanged || osCredentialsChanged)
                        {
                            bool found = false;
                            foreach (RegisteredServer server in Program.gController.Repository.RegisteredServers)
                            {
                                if ((server.SQLServerAuthType == @"W"
                                            && server.SqlLogin.Equals(sqlLogin, StringComparison.CurrentCultureIgnoreCase))
                                    || server.WindowsUser.Equals(wUser, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (found && DialogResult.Yes == MsgBox.ShowConfirm("Update all matching Windows credentials?",
                                                                        "You have changed Windows credentials for this audited server.\r\n\r\nDo you want to update the passwords for all servers that use these Windows credentials?"))
                            {
                                try
                                {
                                    Program.gController.Repository.RefreshRegisteredServers();
                                    foreach (RegisteredServer server in Program.gController.Repository.RegisteredServers)
                                    {
                                        string updateSqlPassword;
                                        if (server.SQLServerAuthType == @"W"
                                            && server.SqlLogin.Equals(sqlLogin, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            updateSqlPassword = sqlPwd;
                                        }
                                        else
                                        {
                                            updateSqlPassword = server.SqlPassword;
                                        }

                                        string updateWindowsPassword = server.WindowsUser.Equals(wUser, StringComparison.CurrentCultureIgnoreCase) ? wPwd : server.WindowsPassword;

                                        Sql.RegisteredServer.UpdateCredentials(Program.gController.Repository.ConnectionString, server.ConnectionName,
                                                    sqlLogin, updateSqlPassword, sqlLoginAuthType, server.WindowsUser, updateWindowsPassword);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MsgBox.ShowError(ErrorMsgs.SqlServerPropertiesCaption, ErrorMsgs.UpdateCredentialsFailedMsg, ex);
                                }
                                Program.gController.Repository.RefreshRegisteredServers();
                            }
                        }
                    }
                }
                else
                {
                    DialogResult = DialogResult.None;
                }
            }
        }

        private void _btn_Help_Click(object sender, EventArgs e)
        {
            string helpTopic;

            switch (ultraTabControl_ServerProperties.SelectedTab.Index)
            {
                case (int)FormTabs.General:
                helpTopic = Utility.Help.ServerGeneralHelpTopic;
                    break;
                case (int)FormTabs.Credentials:
                helpTopic = Utility.Help.ServerCredentialsHelpTopic;
                    break;
                case (int)FormTabs.AuditFolders:
                    helpTopic = Utility.Help.ServerAuditFoldersHelpTopic;
                    break;
                case (int)FormTabs.Filters:
                helpTopic = Utility.Help.ServerFiltersHelpTopic;
                    break;
                case (int)FormTabs.Schedule:
                helpTopic = Utility.Help.ServerScheduleHelpTopic;
                    break;
                case (int)FormTabs.Email:
                helpTopic = Utility.Help.ServerEmailHelpTopic;
                    break;
                case (int)FormTabs.Policies:
                helpTopic = Utility.Help.ServerPoliciesHelpTopic;
                    break;
                default:
                    helpTopic = Utility.Help.ServerGeneralHelpTopic;
                    break;
            }
            Program.gController.ShowTopic(helpTopic);
        }

        private void Form_SqlServerProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            _btn_Help_Click(sender, hlpevent);
        }

        private void button_EmailProvider_Click(object sender, EventArgs e)
        {
            Form_SmtpEmailProviderConfig.Process(m_IsEdit);
        }

        private void checkBoxEmailFindings_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = checkBoxEmailFindings.Checked;

            radioButtonSendEmailFindingAny.Enabled = enable;
            radioButtonSendEmailFindingHigh.Enabled = enable;
            radioButtonSendEmailFindingHighMedium.Enabled = enable;
            m_IsDirty = true;
        }

        private void checkBoxEmailForCollectionStatus_CheckedChanged(object sender, EventArgs e)
        {
            bool enable = checkBoxEmailForCollectionStatus.Checked;

            radioButtonAlways.Enabled = enable;
            radioButton_SendEmailOnError.Enabled = enable;
            radioButton_SendEmailWarningOrError.Enabled = enable;
            m_IsDirty = true;
        }

        private void textBox_Recipients_TextChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        private void RadioButtonChanged(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            switch (ultraTabControl_ServerProperties.SelectedTab.Index)
            {
                case (int)FormTabs.General:
                    Description = DESCR_GENERAL;
                    break;
                case (int)FormTabs.Credentials:
                    Description = DESCR_CREDENTIALS;
                    break;
                case (int)FormTabs.Filters:
                    Description = DESCR_FILTERS;
                    break;
                case (int)FormTabs.Schedule:
                    Description = DESCR_SCHEDULE;
                    break;
                case (int)FormTabs.Email:
                    Description = DESCR_EMAIL;
                    break;
                case (int)FormTabs.Policies:
                    Description = DESCR_INTERVIEW;
                    break;
            }

        }

        private void ultraListView_Policies_ItemCheckStateChanged(object sender, ItemCheckStateChangedEventArgs e)
        {
            m_IsDirty = true;
        }

        private void addEditFoldersControl_FoldersUpdated(object sender, EventArgs e)
        {
            m_IsDirty = true;
        }

        #endregion
    }
}