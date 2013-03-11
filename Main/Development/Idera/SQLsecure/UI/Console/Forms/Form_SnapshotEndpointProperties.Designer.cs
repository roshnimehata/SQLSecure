namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SnapshotEndpointProperties
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
            this._btn_Close = new Infragistics.Win.Misc.UltraButton();
            this._pnl_Properties = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._permissionsGrid = new Idera.SQLsecure.UI.Console.Controls.ObjectPermissionsGrid();
            this._pnl_Object = new System.Windows.Forms.Panel();
            this._grpbx_ObjProperties = new System.Windows.Forms.GroupBox();
            this._lbl_Name = new System.Windows.Forms.Label();
            this._lbl_N = new System.Windows.Forms.Label();
            this._lbl_Protocol = new System.Windows.Forms.Label();
            this._lbl_P = new System.Windows.Forms.Label();
            this._lbl_Type = new System.Windows.Forms.Label();
            this._lbl_T = new System.Windows.Forms.Label();
            this._lbl_IsAdminEndpoint = new System.Windows.Forms.Label();
            this._lbl_IAE = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this._pnl_Properties.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._pnl_Object.SuspendLayout();
            this._grpbx_ObjProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._btn_Close);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 415);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(551, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Close, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._pnl_Properties);
            this._bf_MainPanel.Size = new System.Drawing.Size(551, 362);
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
            this._pnl_Properties.Controls.Add(this.groupBox1);
            this._pnl_Properties.Controls.Add(this._pnl_Object);
            this._pnl_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnl_Properties.Location = new System.Drawing.Point(0, 0);
            this._pnl_Properties.Name = "_pnl_Properties";
            this._pnl_Properties.Size = new System.Drawing.Size(551, 362);
            this._pnl_Properties.TabIndex = 23;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._permissionsGrid);
            this.groupBox1.Location = new System.Drawing.Point(3, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(545, 276);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Permissions";
            // 
            // _permissionsGrid
            // 
            this._permissionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._permissionsGrid.Location = new System.Drawing.Point(9, 19);
            this._permissionsGrid.Name = "_permissionsGrid";
            this._permissionsGrid.Size = new System.Drawing.Size(530, 251);
            this._permissionsGrid.TabIndex = 0;
            // 
            // _pnl_Object
            // 
            this._pnl_Object.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Object.Controls.Add(this._grpbx_ObjProperties);
            this._pnl_Object.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_Object.Location = new System.Drawing.Point(0, 0);
            this._pnl_Object.Name = "_pnl_Object";
            this._pnl_Object.Size = new System.Drawing.Size(551, 77);
            this._pnl_Object.TabIndex = 1;
            // 
            // _grpbx_ObjProperties
            // 
            this._grpbx_ObjProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Name);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_N);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Protocol);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_P);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Type);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_T);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_IsAdminEndpoint);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_IAE);
            this._grpbx_ObjProperties.Location = new System.Drawing.Point(3, 3);
            this._grpbx_ObjProperties.Name = "_grpbx_ObjProperties";
            this._grpbx_ObjProperties.Size = new System.Drawing.Size(545, 68);
            this._grpbx_ObjProperties.TabIndex = 5;
            this._grpbx_ObjProperties.TabStop = false;
            this._grpbx_ObjProperties.Text = "General";
            // 
            // _lbl_Name
            // 
            this._lbl_Name.AutoEllipsis = true;
            this._lbl_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Name.Location = new System.Drawing.Point(61, 15);
            this._lbl_Name.Name = "_lbl_Name";
            this._lbl_Name.Size = new System.Drawing.Size(212, 15);
            this._lbl_Name.TabIndex = 8;
            // 
            // _lbl_N
            // 
            this._lbl_N.AutoSize = true;
            this._lbl_N.Location = new System.Drawing.Point(6, 16);
            this._lbl_N.Name = "_lbl_N";
            this._lbl_N.Size = new System.Drawing.Size(38, 13);
            this._lbl_N.TabIndex = 5;
            this._lbl_N.Text = "Name:";
            // 
            // _lbl_Protocol
            // 
            this._lbl_Protocol.AutoEllipsis = true;
            this._lbl_Protocol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Protocol.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Protocol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Protocol.Location = new System.Drawing.Point(61, 40);
            this._lbl_Protocol.Name = "_lbl_Protocol";
            this._lbl_Protocol.Size = new System.Drawing.Size(212, 15);
            this._lbl_Protocol.TabIndex = 7;
            // 
            // _lbl_P
            // 
            this._lbl_P.AutoSize = true;
            this._lbl_P.Location = new System.Drawing.Point(6, 40);
            this._lbl_P.Name = "_lbl_P";
            this._lbl_P.Size = new System.Drawing.Size(49, 13);
            this._lbl_P.TabIndex = 6;
            this._lbl_P.Text = "Protocol:";
            // 
            // _lbl_Type
            // 
            this._lbl_Type.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Type.AutoEllipsis = true;
            this._lbl_Type.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Type.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Type.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Type.Location = new System.Drawing.Point(380, 15);
            this._lbl_Type.Name = "_lbl_Type";
            this._lbl_Type.Size = new System.Drawing.Size(159, 15);
            this._lbl_Type.TabIndex = 4;
            // 
            // _lbl_T
            // 
            this._lbl_T.AutoSize = true;
            this._lbl_T.Location = new System.Drawing.Point(279, 16);
            this._lbl_T.Name = "_lbl_T";
            this._lbl_T.Size = new System.Drawing.Size(34, 13);
            this._lbl_T.TabIndex = 1;
            this._lbl_T.Text = "Type:";
            // 
            // _lbl_IsAdminEndpoint
            // 
            this._lbl_IsAdminEndpoint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_IsAdminEndpoint.AutoEllipsis = true;
            this._lbl_IsAdminEndpoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_IsAdminEndpoint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_IsAdminEndpoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_IsAdminEndpoint.Location = new System.Drawing.Point(380, 40);
            this._lbl_IsAdminEndpoint.Name = "_lbl_IsAdminEndpoint";
            this._lbl_IsAdminEndpoint.Size = new System.Drawing.Size(159, 15);
            this._lbl_IsAdminEndpoint.TabIndex = 3;
            // 
            // _lbl_IAE
            // 
            this._lbl_IAE.AutoSize = true;
            this._lbl_IAE.Location = new System.Drawing.Point(279, 40);
            this._lbl_IAE.Name = "_lbl_IAE";
            this._lbl_IAE.Size = new System.Drawing.Size(95, 13);
            this._lbl_IAE.TabIndex = 2;
            this._lbl_IAE.Text = "Is Admin Endpoint:";
            // 
            // Form_SnapshotEndpointProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 455);
            this.Description = "View properties of this SQL Server Endpoint";
            this.Name = "Form_SnapshotEndpointProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Endpoint_48;
            this.Text = "Form_SnapshotEndpointProperties";
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._pnl_Properties.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this._pnl_Object.ResumeLayout(false);
            this._grpbx_ObjProperties.ResumeLayout(false);
            this._grpbx_ObjProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _btn_Close;
        private System.Windows.Forms.Panel _pnl_Properties;
        private System.Windows.Forms.GroupBox groupBox1;
        private Idera.SQLsecure.UI.Console.Controls.ObjectPermissionsGrid _permissionsGrid;
        private System.Windows.Forms.Panel _pnl_Object;
        private System.Windows.Forms.GroupBox _grpbx_ObjProperties;
        private System.Windows.Forms.Label _lbl_Name;
        private System.Windows.Forms.Label _lbl_N;
        private System.Windows.Forms.Label _lbl_Protocol;
        private System.Windows.Forms.Label _lbl_P;
        private System.Windows.Forms.Label _lbl_Type;
        private System.Windows.Forms.Label _lbl_T;
        private System.Windows.Forms.Label _lbl_IsAdminEndpoint;
        private System.Windows.Forms.Label _lbl_IAE;
    }
}