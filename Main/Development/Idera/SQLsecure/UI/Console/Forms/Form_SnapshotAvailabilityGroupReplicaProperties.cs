using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.UI.Console.SQL;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SnapshotAvailabilityGroupReplicaProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
       

        #region Ctors

        public Form_SnapshotAvailabilityGroupReplicaProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            InitializeComponent();

            // Init size, icon & text.
            this.MinimumSize = this.Size;
            this.Text = "Availability Group Replica Properties - " + tag.ObjectName;
            // Get endpoint object.
            this.Description = "View properties of this Always On Availability Group Replica";

            AvailabilityGroupReplica gGroup = tag.Tag as AvailabilityGroupReplica;
            if (gGroup != null)
            {
                _lbl_Name.Text = gGroup.ReplicaServerName;
                _lbl_FailMode.Text = gGroup.FailoverModeDesc;
                _lbl_CreateDate.Text = gGroup.CreateDate.ToShortDateString();
                _lb_ModifyDate.Text = gGroup.ModifyDate.ToShortDateString();
                _lb_AvMode.Text = gGroup.AvailabilityModeDesc;
                _lbl_EndUrl .Text = gGroup.EndpointUrl;


            }
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Form_SnapshotAvailabilityGroupReplicaProperties form = new Form_SnapshotAvailabilityGroupReplicaProperties(version, tag);
            form.ShowDialog();
        }

        #endregion

      
    }
}