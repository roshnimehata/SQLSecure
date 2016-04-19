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
    public partial class Form_SnapshotAvailabilityGroupProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
   

        #region Ctors

        public Form_SnapshotAvailabilityGroupProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            InitializeComponent();

            // Init size, icon & text.
            this.MinimumSize = this.Size;
            this.Text = "Availability Group Properties - " + tag.ObjectName;
            // Get endpoint object.
            

            AvailabilityGroup gGroup = tag.Tag as AvailabilityGroup;
            if (gGroup != null)
            {
                _lbl_Name.Text = gGroup.Name;
                _lbl_Type.Text = tag.TypeName;
                _lbl_ResId.Text = gGroup.ResourceGroupId;
                _lbl_Failure.Text = gGroup.FailureConditionLevel.ToString();
                _lb_BuckupRef.Text = gGroup.AutomatedBackuppReferenceDesc;
                _lb_TimeOut.Text = gGroup.HealthCheckTimeout.ToString();

                _permissionsGrid.Initialize(version, tag);
            }
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            Form_SnapshotAvailabilityGroupProperties form = new Form_SnapshotAvailabilityGroupProperties(version, tag);
            form.ShowDialog();
        }

        #endregion
    }
}