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
    public partial class Form_SnapshotFileProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private Sql.ServerVersion m_Version = Sql.ServerVersion.Unsupported;
        private Sql.ObjectTag m_Tag = null;

        #endregion

        #region Helpers

        #endregion

        #region Ctors

        private Form_SnapshotFileProperties(
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
            Sql.FileSystemObject f = Sql.FileSystemObject.GetSnapshotObject(m_Tag.SnapshotId, m_Tag.ObjectId);

            // Set display text.
            Text = "File Properties - " + m_Tag.ObjectName;

            // Set minimum size.
            MinimumSize = Size;

            // Display labels.
            _lbl_Name.Text = f.Name;
            _lbl_Type.Text = f.TypeDescription;
            _lbl_DiskType.Text = f.DiskType;
            _lbl_Owner.Text = f.Owner;
            _lbl_D.Visible = _lbl_Database.Visible = (f.ObjectType == Utility.OsObjectType.DB);
            _lbl_Database.Text = f.Database;

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

            Form_SnapshotFileProperties form = new Form_SnapshotFileProperties(version, tag);
            form.ShowDialog();
        }

        #endregion
    }
}

