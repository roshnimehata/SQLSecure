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
    public partial class Form_SnapshotSchemaProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        Sql.ServerVersion m_Version;
        Sql.ObjectTag m_ObjectTag;

        #endregion

        #region Helpers

        private void getNames(
                ref string name,
                ref string owner
            )
        {
            Sql.Schema s = Sql.Schema.GetSnapshotSchema(m_ObjectTag.SnapshotId, m_ObjectTag.DatabaseId, m_ObjectTag.ObjectId);
            name = m_ObjectTag.DatabaseName + "." + m_ObjectTag.ObjectName;
            owner = s.OwnerName;
        }

        #endregion

        #region Ctors

        public Form_SnapshotSchemaProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            InitializeComponent();

            // Update fields.
            m_Version = version;
            m_ObjectTag = tag;

            // Retrieve properties.
            string name = string.Empty, 
                   owner = string.Empty;
            getNames(ref name, ref owner);

            // Set the labels.
            _lbl_Name.Text = name;
            _lbl_Owner.Text = owner;

            // Set title based on type.
            this.Text = tag.TypeName + " Properties - " + name;

            // Fill explicit permissions.
            _permissionsGrid.Initialize(m_Version, m_ObjectTag);
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag)
        {
            Debug.Assert(tag != null);

            Form_SnapshotSchemaProperties form = new Form_SnapshotSchemaProperties(version, tag);
            form.ShowDialog();
        }

        #endregion
    }
}