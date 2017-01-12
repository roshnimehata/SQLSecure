using Idera.SQLsecure.Core.Accounts;
using Idera.SQLsecure.UI.Console.Controls;
using Idera.SQLsecure.UI.Console.Data;
using Idera.SQLsecure.UI.Console.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_NameMatching : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        ServerInfo m_serverInfo;
        FilterObject m_filterObject;
        FilterObject m_databaseFilterObject;

        #region CTOR

        public Form_NameMatching(FilterObject filterObject, FilterObject databaseFilterObject, ServerInfo serverinfo)
        {
            InitializeComponent();
            this.Text = filterObject.ObjectTypeDisplay;

            if (filterObject.ObjectTypeDisplay.Contains(" "))
            {
                string name = filterObject.ObjectTypeDisplay.Split(' ')[0];
                label_HeaderLabel1.Text = "Select " + name;
                label_HeaderLabel2.Text = "Specific " + name + " can be selected for data collection.";
                label_Available.Text = "Available " + name + ":";
                label_Selected.Text = "Selected " + name + ":";
            }

            m_serverInfo = serverinfo;
            m_filterObject = filterObject;
            m_databaseFilterObject = databaseFilterObject;
            listBox_AvailableObjects.Items.Clear();
            listBox_SelectedObjects.Items.Clear();
            listBox_MatchStrings.Items.Clear();
            
            List<string> itemsList = GetItems();
            if (m_filterObject.MatchStringList != null && 
                (m_filterObject.MatchStringList.Count > 1 || (m_filterObject.MatchStringList.Count > 0 && !string.IsNullOrEmpty(m_filterObject.MatchStringList[0]))))
            {
                foreach (string str in m_filterObject.MatchStringList)
                {
                    if (itemsList.Contains(str))
                    {
                        listBox_SelectedObjects.Items.Add(str);
                        itemsList.Remove(str);
                    }
                    else
                {
                    listBox_MatchStrings.Items.Add(str);
                }
            }
            }
            if (listBox_MatchStrings.Items.Count == 0)
            {
                radioButton_Any.Checked = true;
            }
            else
            {
                radioButton_Like.Checked = true;
            }

            foreach (string item in itemsList)
            {
                listBox_AvailableObjects.Items.Add(item);
            }
            CheckState();
        }
        #endregion

        private List<string> GetItems()
        {
            System.Security.Principal.WindowsImpersonationContext targetImpersonationContext = null;
            List<string> list = null;
            bool res = false;
            System.Data.SqlClient.SqlConnectionStringBuilder bldr;

            if (m_serverInfo.windowsAuth)
        {
                try
                {
                    System.Security.Principal.WindowsIdentity wi =
                                        Impersonation.GetCurrentIdentity(m_serverInfo.login, m_serverInfo.password);
                    targetImpersonationContext = wi.Impersonate();
                }
                catch (Exception ex)
                {
                    Idera.SQLsecure.Core.Logger.LogX logX = new Idera.SQLsecure.Core.Logger.LogX("Idera.SQLsecure.UI.Console.Sql.Database");
                    logX.loggerX.Error("Error Processing Impersonation for retrieving Database objects list (" + m_serverInfo.login + ")", ex);
                }
                bldr = Sql.SqlHelper.ConstructConnectionString(m_serverInfo.connectionName, null, null,Utility.Activity.TypeServerOnPremise);
            }
            else
            {
                bldr = Sql.SqlHelper.ConstructConnectionString(m_serverInfo.connectionName, m_serverInfo.login, m_serverInfo.password, Utility.Activity.TypeServerOnPremise);
        }

            switch (m_filterObject.ObjectType)
            {
                case RuleObjectType.Database:
                    res = Idera.SQLsecure.UI.Console.Sql.Database.GetTargetDatabases(m_serverInfo.version, m_filterObject.ObjectScope, bldr.ConnectionString, out list);
                    break;
                case RuleObjectType.Table:
                    res = Idera.SQLsecure.UI.Console.Sql.Database.GetTargetTables(m_serverInfo.version, m_filterObject.ObjectScope, m_databaseFilterObject, bldr.ConnectionString, out list);
                    break;
                case RuleObjectType.View:
                    res = Idera.SQLsecure.UI.Console.Sql.Database.GetTargetViews(m_serverInfo.version, m_filterObject.ObjectScope, m_databaseFilterObject, bldr.ConnectionString, out list);
                    break;
                case RuleObjectType.Function:
                    res = Idera.SQLsecure.UI.Console.Sql.Database.GetTargetFunctions(m_serverInfo.version, m_filterObject.ObjectScope, m_databaseFilterObject, bldr.ConnectionString, out list);
                    break;
            }

            if (targetImpersonationContext != null)
            {
                targetImpersonationContext.Undo();
                targetImpersonationContext.Dispose();
                targetImpersonationContext = null;
            }

            return res ? list : new List<string>();
        }

        #region Public

        public static List<string> Process(FilterObject filterObject, FilterObject databaseFilterObject, ServerInfo serverinfo, out bool isDirty)
        {
            isDirty = false;
            List<string> returnMatchNames = null;

            Forms.Form_NameMatching form = new Forms.Form_NameMatching(filterObject, databaseFilterObject, serverinfo);

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.m_filterObject.MatchStringList.Count == 0)
                {
                    form.m_filterObject.MatchStringList.Add(string.Empty);
                }                
                returnMatchNames = form.m_filterObject.MatchStringList;
                isDirty = true;
            }
            else
            {
                returnMatchNames = filterObject.MatchStringList;
            }

            return returnMatchNames;

        }

        #endregion

        #region Events

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox_MatchString.Text))
            {
                if (Sql.SqlHelper.SqlInjectionChars(textBox_MatchString.Text))
                {
                    Utility.MsgBox.ShowError(this.Text, Utility.ErrorMsgs.NameMatchInvalidCharsMsg);
                    return;
                }                

                if (!listBox_MatchStrings.Items.Contains(textBox_MatchString.Text))
                {
                    listBox_MatchStrings.Items.Add(textBox_MatchString.Text);
                }
                textBox_MatchString.Text = string.Empty;
            }
        }

        private void button_Remove_Click(object sender, EventArgs e)
        {
            List<string> strs = new List<string>();

            foreach (string str in listBox_MatchStrings.SelectedItems)
            {
                strs.Add(str);
            }
            foreach (string str in strs)
            {
                listBox_MatchStrings.Items.Remove(str);
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_filterObject.MatchStringList = new List<string>();

            foreach (string str in listBox_SelectedObjects.Items)
            {
                m_filterObject.MatchStringList.Add(str);
            }
            foreach (string str in listBox_MatchStrings.Items)
            {
                if (m_filterObject.MatchStringList.LastIndexOf(str) < 0)
                {
                    m_filterObject.MatchStringList.Add(str);
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void radioButton_Any_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Any.Checked)
            {
                listBox_MatchStrings.Items.Clear();
                listBox_MatchStrings.Items.Add("");
                listBox_MatchStrings.Enabled = false;
                textBox_MatchString.Enabled = false;
                button_Add.Enabled = false;
                button_Remove.Enabled = false;
            }
        }

        private void radioButton_Like_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Like.Checked)
            {
                listBox_MatchStrings.Items.Clear();
                listBox_MatchStrings.Enabled = true;
                textBox_MatchString.Enabled = true;
                button_Add.Enabled = true;
                button_Remove.Enabled = true;
            }
        }

        private void listBox_MatchStrings_EnabledChanged(object sender, EventArgs e)
        {
            listBox_MatchStrings.BackColor = listBox_MatchStrings.Enabled ? SystemColors.Window : SystemColors.Control;
        }

        


        #endregion

        private void button_AddObject_Click(object sender, EventArgs e)
        {
            for (int i = 0; i< listBox_AvailableObjects.Items.Count;)
            {
                if (listBox_AvailableObjects.GetSelected(i))
                {
                    listBox_SelectedObjects.Items.Add(listBox_AvailableObjects.Items[i].ToString());
                    listBox_AvailableObjects.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            CheckState();
        }

        private void button_RemoveObject_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox_SelectedObjects.Items.Count; )
            {
                if (listBox_SelectedObjects.GetSelected(i))
                {
                    listBox_AvailableObjects.Items.Add(listBox_SelectedObjects.Items[i].ToString());
                    listBox_SelectedObjects.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            CheckState();
        }

        private void CheckState()
        {
            if (listBox_SelectedObjects.Items.Count > 0)
            {
                if (radioButton_Any.Checked)
                {
                    radioButton_Like.Checked = true;
                }
                radioButton_Any.Enabled = false;
            }
            else
            {
                radioButton_Any.Enabled = true;
                if (listBox_MatchStrings.Items.Count == 0)
                {
                    radioButton_Any.Checked = true;
                }
            }
        }

    }
}

