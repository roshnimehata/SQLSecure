using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Idera.SQLsecure.UI.Console.Utility;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SnapshotDbRoleProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        private Sql.ServerVersion m_Version = Sql.ServerVersion.Unsupported;
        private Sql.ObjectTag m_Tag = null;
        private bool m_IsGridFilled = false;

        #endregion

        #region Grid Stuff

        private const string colTypeIcon = "TypeIcon";
        private const string colName = "Name";
        private const string colType = "Type";
        private const string colTag = "Tag";

        #endregion

        #region Helpers

        #endregion

        #region Ctors

        public Form_SnapshotDbRoleProperties(
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

            // Set size and text.
            MinimumSize = Size;
            Text = "Role Properties - " + m_Tag.ObjectName;

            // Get the role object & its members.
            Sql.DatabasePrincipal role = Sql.DatabasePrincipal.GetSnapshotDbRole(m_Tag.SnapshotId, m_Tag.DatabaseId, m_Tag.ObjectId);
            if (role != null)
            {
                // Update fields.
                _lbl_Name.Text = role.Name;
                _lbl_Type.Text = role.TypeStr;
                _lbl_Owner.Text = role.Owner;

                // Fill role members grid based on the role type.
                DataTable dataTable = new DataTable();
                if (role.TypeEnum == Sql.ObjectType.TypeEnum.DatabaseRole)
                {
                    // Get role members.
                    List<Sql.DatabasePrincipal> members = Sql.DatabasePrincipal.GetSnapshotDbRoleMembers(m_Tag.SnapshotId, m_Tag.DatabaseId, m_Tag.ObjectId);

                    // Update members grid
                    dataTable.Columns.Add(colTypeIcon, typeof(Image));
                    dataTable.Columns.Add(colName, typeof(string));
                    dataTable.Columns.Add(colType, typeof(string));
                    foreach (Sql.DatabasePrincipal dbp in members)
                    {
                        dataTable.Rows.Add(Sql.ObjectType.TypeImage16(dbp.TypeEnum), dbp.Name, dbp.TypeStr);
                    }
                }
                else if (role.TypeEnum == Sql.ObjectType.TypeEnum.ApplicationRole)
                {
                    dataTable.Columns.Add(colTypeIcon, typeof(Image));
                    dataTable.Columns.Add(colName, typeof(string));
                    dataTable.Columns.Add(colType, typeof(string));
                    dataTable.Rows.Add(null, "Application roles do not contain members", string.Empty);
                }
                else
                {
                    Debug.Assert(false, "Unknown role type");
                }

                _ultraGrid.BeginUpdate();
                _ultraGrid.DataSource = dataTable;
                _ultraGrid.DataMember = "";
                _ultraGrid.EndUpdate();

                ultraTabControl1.DrawFilter = new HideFocusRectangleDrawFilter();
            }
        }

        #endregion

        #region Event Handlers

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Key == "Permissions" && !m_IsGridFilled)
            {
                this.Cursor = Cursors.WaitCursor;
                _dbPrincipalPermissionsGrid.Initialize(m_Version, m_Tag);
                m_IsGridFilled = true;
                this.Cursor = Cursors.Default;                
            }
        }

        private void _ultraGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Override.CellAppearance.BorderAlpha = Alpha.Transparent;
            //e.Layout.Override.RowAppearance.BorderColor = Color.White;

            e.Layout.Bands[0].Columns[colTypeIcon].Header.Caption = "";
            e.Layout.Bands[0].Columns[colTypeIcon].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
            e.Layout.Bands[0].Columns[colTypeIcon].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colTypeIcon].Width = 22;

            e.Layout.Bands[0].Columns[colName].Header.Caption = "Name";
            e.Layout.Bands[0].Columns[colName].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colName].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colName].Width = 350;

            e.Layout.Bands[0].Columns[colType].Header.Caption = "Type";
            e.Layout.Bands[0].Columns[colType].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colType].AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Columns[colType].Width = 20;
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Form_SnapshotDbRoleProperties form = new Form_SnapshotDbRoleProperties(version, tag);
            form.ShowDialog();
        }

        #endregion

       
    }
}