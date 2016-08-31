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
    public partial class Form_SnapshotUserProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
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

        private static string memberOf(List<string> roles)
        {
            string ret = string.Empty;
            for (int i = 0; i < roles.Count; ++i)
            {
                ret += roles[i];
                if (i != (roles.Count - 1)) { ret += ", "; }
            }
            return ret;
        }

        #endregion

        #region Ctors

        public Form_SnapshotUserProperties(
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

            // Set size and form text.
            MinimumSize = Size;
            Text = "User Properties - " + m_Tag.ObjectName;

            // Get user associated with the tag.
            Sql.DatabasePrincipal user = Sql.DatabasePrincipal.GetSnapshotUser(m_Tag.SnapshotId, m_Tag.DatabaseId, m_Tag.ObjectId);     

            // If user is found get roles and update fields.
            if (user != null)
            {
                // Fill fields.
                _lbl_Name.Text = user.Name;
                _lbl_Type.Text = user.TypeStr;
                _lbl_Login.Text = user.Login;
                _lbl_HasAccess.Text = user.HasAccessStr;
                _lbl_IsAliased.Text = user.IsAliasStr;
                _lb_ContainedType.Text = user.AuthenticationType;
                if (user.IsAlias) { _lbl_AliasedTo.Text = user.AltName; }

                if (version == Sql.ServerVersion.SQL2000)
                {
                    _lbl_DSN.Enabled = _lbl_DefaultSchemaName.Enabled = false;
                }
                else
                {
                    _lbl_DefaultSchemaName.Text = user.DefaultSchemaName;
                }

                // Get roles.
                List<Sql.DatabasePrincipal> roles = Sql.DatabasePrincipal.GetSnapshotUserRoles(m_Tag.SnapshotId, m_Tag.DatabaseId, m_Tag.ObjectId);

                // Update memberof grid
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(colTypeIcon, typeof(Image));
                dataTable.Columns.Add(colName, typeof(string));
                dataTable.Columns.Add(colType, typeof(string));
                foreach (Sql.DatabasePrincipal dbp in roles)
                {
                    dataTable.Rows.Add(Sql.ObjectType.TypeImage16(dbp.TypeEnum), dbp.Name, dbp.TypeStr);
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
            Form_SnapshotUserProperties form = new Form_SnapshotUserProperties(version, tag);
            form.ShowDialog();
        }

        #endregion

    }
}