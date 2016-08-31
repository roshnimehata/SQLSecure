using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SnapshotEndpointProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Helpers

        #endregion

        #region Ctors

        public Form_SnapshotEndpointProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            InitializeComponent();

            // Init size, icon & text.
            this.MinimumSize = this.Size;
            this.Text = "Endpoint Properties - " + tag.ObjectName;

            // Get endpoint object.
            Sql.Endpoint endpoint = Sql.Endpoint.GetSnapshotEndpoint(tag.SnapshotId, tag.ObjectId);
            if (endpoint != null)
            {
                _lbl_Name.Text = endpoint.Name;
                _lbl_Type.Text = endpoint.EndpointType;
                _lbl_Protocol.Text = endpoint.Protocol;
                _lbl_IsAdminEndpoint.Text = endpoint.IsAdminEndpointStr;
                _permissionsGrid.Initialize(version, tag);
            }
            else
            {
            }
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Form_SnapshotEndpointProperties form = new Form_SnapshotEndpointProperties(version, tag);
            form.ShowDialog();
        }

        #endregion
    }
}