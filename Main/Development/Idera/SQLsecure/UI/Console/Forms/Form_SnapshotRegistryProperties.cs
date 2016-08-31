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
    public partial class Form_SnapshotRegistryProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private Sql.ServerVersion m_Version = Sql.ServerVersion.Unsupported;
        private Sql.ObjectTag m_Tag = null;

        #endregion

        #region Helpers

        #endregion

        #region Ctors

        private Form_SnapshotRegistryProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Debug.Assert(version != Sql.ServerVersion.Unsupported);
            Debug.Assert(tag != null);

            InitializeComponent();

            // Initialize fields.
            m_Version = version;
            m_Tag = tag;

            // Get properties.
            Sql.RegistryKey r = Sql.RegistryKey.GetSnapshotKey(m_Tag.SnapshotId, m_Tag.ObjectId);

            // Set display text.
            Text = "Registry Key Properties - " + m_Tag.ObjectName;

            // Set minimum size.
            MinimumSize = Size;

            // Display labels.
            _lbl_Name.Text = r.Name;
            _lbl_Owner.Text = r.Owner;

            // Fill explicit permissions.
            _permissionsGrid.Initialize(m_Version, m_Tag);
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Debug.Assert(version != Sql.ServerVersion.Unsupported);
            Debug.Assert(tag != null);

            Form_SnapshotRegistryProperties form = new Form_SnapshotRegistryProperties(version, tag);
            form.ShowDialog();
        }

        #endregion
    }
}

