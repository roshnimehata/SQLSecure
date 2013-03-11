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
        List<string> m_matchNames;

        #region CTOR

        public Form_NameMatching(string objectName, List<string> matchNames)
        {
            InitializeComponent();
            this.Text = objectName;

            if(matchNames != null && (matchNames.Count > 1 || (matchNames.Count > 0 && !string.IsNullOrEmpty(matchNames[0]))))
            {
                foreach (string str in matchNames)
                {
                    listBox_MatchStrings.Items.Add(str);
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
        }
        #endregion

        #region Properties

        public List<string> matchNames
        {
            get { return m_matchNames; }
        }

        #endregion

        #region Public

        public static List<string> Process(string objectName, List<string> matchNames, out bool isDirty)
        {
            isDirty = false;
            List<string> returnMatchNames = null;

            Forms.Form_NameMatching form = new Forms.Form_NameMatching(objectName, matchNames);

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.m_matchNames.Count == 0)
                {
                    form.m_matchNames.Add(string.Empty);
                }                
                returnMatchNames = form.m_matchNames;
                isDirty = true;
            }
            else
            {
                returnMatchNames = matchNames;
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
            m_matchNames = new List<string>();

            foreach (string str in listBox_MatchStrings.Items)
            {
                m_matchNames.Add(str);
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

    }
}

