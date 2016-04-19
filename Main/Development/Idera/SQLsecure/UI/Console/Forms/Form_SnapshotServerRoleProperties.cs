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
    public partial class Form_SnapshotServerRoleProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
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

        private Form_SnapshotServerRoleProperties(
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

            // Set min size to size.
            MinimumSize = Size;

            // Set form text.
            Text = "Server Role Properties - " + tag.ObjectName;

            // Fill name label.
            _lbl_Name.Text = tag.ObjectName;


            // Update members grid
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(colTypeIcon, typeof(Image));
            dataTable.Columns.Add(colName, typeof(string));
            dataTable.Columns.Add(colType, typeof(string));
            List<Sql.Login> members = Sql.ServerRole.GetSnapshotServerRoleMembers(tag.SnapshotId, tag.ObjectId);
            foreach (Sql.Login l in members)
            {
                dataTable.Rows.Add(Sql.ObjectType.TypeImage16(l.Type), l.Name, l.TypeStr);
            }
            _ultraGrid.BeginUpdate();
            _ultraGrid.DataSource = dataTable;
            _ultraGrid.DataMember = "";
            _ultraGrid.EndUpdate();

            ultraTabControl1.DrawFilter = new HideFocusRectangleDrawFilter();
        }

        #endregion

        #region Event Handlers

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab.Key == "Permissions" && !m_IsGridFilled)
            {
                this.Cursor = Cursors.WaitCursor;
                _serverPrincipalPermissionsGrid.Initialize(m_Version, m_Tag);
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
            Debug.Assert(version != Sql.ServerVersion.Unsupported);
            Debug.Assert(tag != null);

            Form_SnapshotServerRoleProperties form = new Form_SnapshotServerRoleProperties(version, tag);
            form.ShowDialog();
        }

        #endregion

    }
}