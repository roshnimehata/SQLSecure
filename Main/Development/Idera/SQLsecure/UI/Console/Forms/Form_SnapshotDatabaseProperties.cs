using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SnapshotDatabaseProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {

        #region Ctors

        public Form_SnapshotDatabaseProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            InitializeComponent();

            Sql.Database db = Sql.Database.GetSnapshotDatabase(tag.SnapshotId, tag.DatabaseId);
            if (db != null)
            {

                // Set the properties.
                _lbl_Name.Text = db.Name;
                _lbl_Owner.Text = db.Owner;
                _lbl_GuestEnabled.Text = db.IsGuestEnabled ? "Yes" : "No";
                _lbl_Trustworthy.Text = db.IsTrustworthy;
                _lbl_Status.Text = db.Status;

                // Set title based on type.
                this.Text = tag.TypeName + " Properties - " + db.Name;

                // If database is available, fill explicit permissions
                // Else disable the grid.
                if (db.IsAvailable)
                {
                    _permissionsGrid.Initialize(version, tag);
                }
                else
                {
                    _permissionsGrid.Enabled = false;
                }
            }
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag)
        {
            Debug.Assert(tag != null);

            Form_SnapshotDatabaseProperties form = new Form_SnapshotDatabaseProperties(version, tag);
            form.ShowDialog();
        }

        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            ShowHelpTopic();
        }

        private void _btn_Help_Click(object sender, EventArgs e)
        {
            ShowHelpTopic();
        }

        private void ShowHelpTopic()
        {
            Program.gController.ShowTopic(Utility.Help.DatabasePropertiesHelpTopic);
        }

        #endregion
    }
}