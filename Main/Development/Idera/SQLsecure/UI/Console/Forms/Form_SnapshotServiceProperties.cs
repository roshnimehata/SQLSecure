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
    public partial class Form_SnapshotServiceProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private Sql.ServerVersion m_Version = Sql.ServerVersion.Unsupported;
        private Sql.ObjectTag m_Tag = null;

        #endregion

        #region Helpers

        #endregion

        #region Ctors

        private Form_SnapshotServiceProperties(
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
            Sql.Service s = Sql.Service.GetSnapshotService(m_Tag.SnapshotId, m_Tag.ObjectName);

            // Set display text.
            Text = "Service Properties - " + m_Tag.ObjectName;

            // Set minimum size.
            MinimumSize = Size;

            // Display labels.
            _lbl_Name.Text = s.Name;
            _lbl_DisplayName.Text = s.DisplayName;
            _lbl_Path.Text = s.Path;
            _lbl_StartupType.Text = s.StartupType;
            _lbl_LoginName.Text = s.LoginName;
            _lbl_State.Text = s.State;
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

            Form_SnapshotServiceProperties form = new Form_SnapshotServiceProperties(version, tag);
            form.ShowDialog();
        }

        #endregion
    }
}

