namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SnapshotAvailabilityGroupReplicaProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SnapshotAvailabilityGroupReplicaProperties));
            this._btn_Close = new Infragistics.Win.Misc.UltraButton();
            this._pnl_Properties = new System.Windows.Forms.Panel();
            this._pnl_Object = new System.Windows.Forms.Panel();
            this._grpbx_ObjProperties = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._lbl_Name = new System.Windows.Forms.Label();
            this._lbl_N = new System.Windows.Forms.Label();
            this._lb_ModifyDate = new System.Windows.Forms.Label();
            this._lb_AvMode = new System.Windows.Forms.Label();
            this._lbl_EndUrl = new System.Windows.Forms.Label();
            this._lbl_P = new System.Windows.Forms.Label();
            this._lbl_FailMode = new System.Windows.Forms.Label();
            this._lbl_T = new System.Windows.Forms.Label();
            this._lbl_CreateDate = new System.Windows.Forms.Label();
            this._lbl_IAE = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this._pnl_Properties.SuspendLayout();
            this._pnl_Object.SuspendLayout();
            this._grpbx_ObjProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._btn_Close);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 236);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(551, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Close, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._pnl_Properties);
            this._bf_MainPanel.Size = new System.Drawing.Size(551, 183);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(551, 53);
            // 
            // _btn_Close
            // 
            this._btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btn_Close.Location = new System.Drawing.Point(464, 9);
            this._btn_Close.Name = "_btn_Close";
            this._btn_Close.Size = new System.Drawing.Size(75, 23);
            this._btn_Close.TabIndex = 22;
            this._btn_Close.Text = "&Close";
            // 
            // _pnl_Properties
            // 
            this._pnl_Properties.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Properties.Controls.Add(this._pnl_Object);
            this._pnl_Properties.Controls.Add(this.label1);
            this._pnl_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnl_Properties.Location = new System.Drawing.Point(0, 0);
            this._pnl_Properties.Name = "_pnl_Properties";
            this._pnl_Properties.Size = new System.Drawing.Size(551, 183);
            this._pnl_Properties.TabIndex = 23;
            // 
            // _pnl_Object
            // 
            this._pnl_Object.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Object.Controls.Add(this._grpbx_ObjProperties);
            this._pnl_Object.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_Object.Location = new System.Drawing.Point(0, 0);
            this._pnl_Object.Name = "_pnl_Object";
            this._pnl_Object.Size = new System.Drawing.Size(551, 105);
            this._pnl_Object.TabIndex = 1;
            // 
            // _grpbx_ObjProperties
            // 
            this._grpbx_ObjProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_ObjProperties.Controls.Add(this.label5);
            this._grpbx_ObjProperties.Controls.Add(this.label3);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Name);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_N);
            this._grpbx_ObjProperties.Controls.Add(this._lb_ModifyDate);
            this._grpbx_ObjProperties.Controls.Add(this._lb_AvMode);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_EndUrl);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_P);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_FailMode);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_T);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_CreateDate);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_IAE);
            this._grpbx_ObjProperties.Location = new System.Drawing.Point(3, 3);
            this._grpbx_ObjProperties.Name = "_grpbx_ObjProperties";
            this._grpbx_ObjProperties.Size = new System.Drawing.Size(545, 183);
            this._grpbx_ObjProperties.TabIndex = 5;
            this._grpbx_ObjProperties.TabStop = false;
            this._grpbx_ObjProperties.Text = "General";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(283, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Modify Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Availability Mode ";
            // 
            // _lbl_Name
            // 
            this._lbl_Name.AutoEllipsis = true;
            this._lbl_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Name.Location = new System.Drawing.Point(128, 22);
            this._lbl_Name.Name = "_lbl_Name";
            this._lbl_Name.Size = new System.Drawing.Size(149, 15);
            this._lbl_Name.TabIndex = 8;
            // 
            // _lbl_N
            // 
            this._lbl_N.AutoSize = true;
            this._lbl_N.Location = new System.Drawing.Point(6, 23);
            this._lbl_N.Name = "_lbl_N";
            this._lbl_N.Size = new System.Drawing.Size(111, 13);
            this._lbl_N.TabIndex = 5;
            this._lbl_N.Text = "Replica Server Name:";
            // 
            // _lb_ModifyDate
            // 
            this._lb_ModifyDate.AutoEllipsis = true;
            this._lb_ModifyDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lb_ModifyDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lb_ModifyDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lb_ModifyDate.Location = new System.Drawing.Point(379, 89);
            this._lb_ModifyDate.Name = "_lb_ModifyDate";
            this._lb_ModifyDate.Size = new System.Drawing.Size(160, 15);
            this._lb_ModifyDate.TabIndex = 7;
            // 
            // _lb_AvMode
            // 
            this._lb_AvMode.AutoEllipsis = true;
            this._lb_AvMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lb_AvMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lb_AvMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lb_AvMode.Location = new System.Drawing.Point(128, 87);
            this._lb_AvMode.Name = "_lb_AvMode";
            this._lb_AvMode.Size = new System.Drawing.Size(149, 15);
            this._lb_AvMode.TabIndex = 7;
            // 
            // _lbl_EndUrl
            // 
            this._lbl_EndUrl.AutoEllipsis = true;
            this._lbl_EndUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_EndUrl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_EndUrl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_EndUrl.Location = new System.Drawing.Point(128, 56);
            this._lbl_EndUrl.Name = "_lbl_EndUrl";
            this._lbl_EndUrl.Size = new System.Drawing.Size(149, 15);
            this._lbl_EndUrl.TabIndex = 7;
            // 
            // _lbl_P
            // 
            this._lbl_P.AutoSize = true;
            this._lbl_P.Location = new System.Drawing.Point(6, 56);
            this._lbl_P.Name = "_lbl_P";
            this._lbl_P.Size = new System.Drawing.Size(71, 13);
            this._lbl_P.TabIndex = 6;
            this._lbl_P.Text = "Endpoin URL";
            // 
            // _lbl_FailMode
            // 
            this._lbl_FailMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_FailMode.AutoEllipsis = true;
            this._lbl_FailMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_FailMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_FailMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_FailMode.Location = new System.Drawing.Point(379, 22);
            this._lbl_FailMode.Name = "_lbl_FailMode";
            this._lbl_FailMode.Size = new System.Drawing.Size(160, 15);
            this._lbl_FailMode.TabIndex = 4;
            // 
            // _lbl_T
            // 
            this._lbl_T.AutoSize = true;
            this._lbl_T.Location = new System.Drawing.Point(283, 23);
            this._lbl_T.Name = "_lbl_T";
            this._lbl_T.Size = new System.Drawing.Size(74, 13);
            this._lbl_T.TabIndex = 1;
            this._lbl_T.Text = "Failover Mode";
            // 
            // _lbl_CreateDate
            // 
            this._lbl_CreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_CreateDate.AutoEllipsis = true;
            this._lbl_CreateDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_CreateDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_CreateDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_CreateDate.Location = new System.Drawing.Point(379, 56);
            this._lbl_CreateDate.Name = "_lbl_CreateDate";
            this._lbl_CreateDate.Size = new System.Drawing.Size(160, 15);
            this._lbl_CreateDate.TabIndex = 3;
            // 
            // _lbl_IAE
            // 
            this._lbl_IAE.AutoSize = true;
            this._lbl_IAE.ForeColor = System.Drawing.Color.MidnightBlue;
            this._lbl_IAE.Location = new System.Drawing.Point(283, 56);
            this._lbl_IAE.Name = "_lbl_IAE";
            this._lbl_IAE.Size = new System.Drawing.Size(64, 13);
            this._lbl_IAE.TabIndex = 2;
            this._lbl_IAE.Text = "Create Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Health Check Timeout:";
            // 
            // Form_SnapshotAvailabilityGroupReplicaProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 276);
            this.Description = "View properties of this SQL Server Availability Group Replica";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_SnapshotAvailabilityGroupReplicaProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.replica_48;
            this.Text = "Form_SnapshotAvailabilityGroupReplicaProperties";
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._pnl_Properties.ResumeLayout(false);
            this._pnl_Properties.PerformLayout();
            this._pnl_Object.ResumeLayout(false);
            this._grpbx_ObjProperties.ResumeLayout(false);
            this._grpbx_ObjProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _btn_Close;
        private System.Windows.Forms.Panel _pnl_Properties;
        private System.Windows.Forms.Panel _pnl_Object;
        private System.Windows.Forms.GroupBox _grpbx_ObjProperties;
        private System.Windows.Forms.Label _lbl_Name;
        private System.Windows.Forms.Label _lbl_N;
        private System.Windows.Forms.Label _lbl_EndUrl;
        private System.Windows.Forms.Label _lbl_P;
        private System.Windows.Forms.Label _lbl_FailMode;
        private System.Windows.Forms.Label _lbl_T;
        private System.Windows.Forms.Label _lbl_CreateDate;
        private System.Windows.Forms.Label _lbl_IAE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _lb_ModifyDate;
        private System.Windows.Forms.Label _lb_AvMode;
        private System.Windows.Forms.Label label1;
    }
}