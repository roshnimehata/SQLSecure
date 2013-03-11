using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    #region Ctors

    public partial class Form_ConnectionProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        public Form_ConnectionProperties()
        {
            InitializeComponent();

            Sql.Repository repository = Program.gController.Repository;
            if (repository.IsValid)
            {
                this._groupBox_Properties.Text = repository.Instance;
                this._label_Properties.Text = "Connected to Repository on " + repository.Instance;
                //if (repository.User.Length > 0)
                //{
                //    this._label_Properties.Text += "\n " + repository.Instance;
                //}
                this._listView_Properties.Items.Add("SQL Server:");
                this._listView_Properties.Items[_listView_Properties.Items.Count - 1].SubItems.Add(repository.SQLServerVersion + " (" + repository.SQLServerFullVersion + ")");
                this._listView_Properties.Items.Add("Schema Version:");
                this._listView_Properties.Items[_listView_Properties.Items.Count - 1].SubItems.Add(repository.SchemaVersion.ToString("n0"));
                this._listView_Properties.Items.Add("DAL Version:");
                this._listView_Properties.Items[_listView_Properties.Items.Count - 1].SubItems.Add(repository.DALVersion.ToString("n0"));
                this._listView_Properties.Items.Add("Access Level:");
                this._listView_Properties.Items[_listView_Properties.Items.Count - 1].SubItems.Add(Program.gController.Permissions.UserAccessLevel.ToString());
                this._listView_Properties.Items.Add("");
                this._listView_Properties.Items.Add("Registered Servers:");
                this._listView_Properties.Items[_listView_Properties.Items.Count - 1].SubItems.Add(repository.RegisteredServers.Count.ToString());
            }
            else
             {
                this._groupBox_Properties.Text = "Not Connected";
                this._label_Properties.Text = "Not currently connected to a Repository";
            }
        }
        private void showHelpTopic()
        {
//            Program.gController.ShowTopic(Utility.Help.ConnectionPropertiesHelpTopic);
        }

        private void _button_Help_Click(object sender, EventArgs e)
        {
            showHelpTopic();
        }

        private void Form_ConnectionProperties_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            showHelpTopic();
        }
    }

    #endregion
}

