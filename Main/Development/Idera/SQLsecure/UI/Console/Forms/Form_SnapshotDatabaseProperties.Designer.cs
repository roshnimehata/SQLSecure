namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SnapshotDatabaseProperties
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
            this._grpbx_ObjProperties = new System.Windows.Forms.GroupBox();
            this._lbl_Trustworthy = new System.Windows.Forms.Label();
            this._lbl_Trust = new System.Windows.Forms.Label();
            this._lbl_Status = new System.Windows.Forms.Label();
            this._lbl_S = new System.Windows.Forms.Label();
            this._lbl_Name = new System.Windows.Forms.Label();
            this._lbl_N = new System.Windows.Forms.Label();
            this._lbl_GuestEnabled = new System.Windows.Forms.Label();
            this._lbl_G = new System.Windows.Forms.Label();
            this._lbl_Owner = new System.Windows.Forms.Label();
            this._lbl_O = new System.Windows.Forms.Label();
            this._pnl_Permissions = new System.Windows.Forms.GroupBox();
            this._permissionsGrid = new Idera.SQLsecure.UI.Console.Controls.ObjectPermissionsGrid();
            this._btn_Help = new Infragistics.Win.Misc.UltraButton();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this._pnl_Properties.SuspendLayout();
            this._grpbx_ObjProperties.SuspendLayout();
            this._pnl_Permissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._btn_Help);
            this._bfd_ButtonPanel.Controls.Add(this._btn_Close);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 456);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(567, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Close, 0);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Help, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._pnl_Properties);
            this._bf_MainPanel.Size = new System.Drawing.Size(567, 403);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(567, 53);
            // 
            // _btn_Close
            // 
            this._btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btn_Close.Location = new System.Drawing.Point(402, 10);
            this._btn_Close.Name = "_btn_Close";
            this._btn_Close.Size = new System.Drawing.Size(75, 23);
            this._btn_Close.TabIndex = 1;
            this._btn_Close.Text = "&Close";
            // 
            // _pnl_Properties
            // 
            this._pnl_Properties.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Properties.Controls.Add(this._grpbx_ObjProperties);
            this._pnl_Properties.Controls.Add(this._pnl_Permissions);
            this._pnl_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnl_Properties.Location = new System.Drawing.Point(0, 0);
            this._pnl_Properties.Name = "_pnl_Properties";
            this._pnl_Properties.Size = new System.Drawing.Size(567, 403);
            this._pnl_Properties.TabIndex = 23;
            // 
            // _grpbx_ObjProperties
            // 
            this._grpbx_ObjProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Trustworthy);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Trust);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Status);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_S);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Name);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_N);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_GuestEnabled);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_G);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Owner);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_O);
            this._grpbx_ObjProperties.Location = new System.Drawing.Point(3, 3);
            this._grpbx_ObjProperties.Name = "_grpbx_ObjProperties";
            this._grpbx_ObjProperties.Size = new System.Drawing.Size(561, 79);
            this._grpbx_ObjProperties.TabIndex = 5;
            this._grpbx_ObjProperties.TabStop = false;
            this._grpbx_ObjProperties.Text = "General";
            // 
            // _lbl_Trustworthy
            // 
            this._lbl_Trustworthy.AutoEllipsis = true;
            this._lbl_Trustworthy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Trustworthy.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Trustworthy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Trustworthy.Location = new System.Drawing.Point(495, 46);
            this._lbl_Trustworthy.Name = "_lbl_Trustworthy";
            this._lbl_Trustworthy.Size = new System.Drawing.Size(60, 20);
            this._lbl_Trustworthy.TabIndex = 9;
            this._lbl_Trustworthy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_Trust
            // 
            this._lbl_Trust.AutoSize = true;
            this._lbl_Trust.Location = new System.Drawing.Point(424, 50);
            this._lbl_Trust.Name = "_lbl_Trust";
            this._lbl_Trust.Size = new System.Drawing.Size(65, 13);
            this._lbl_Trust.TabIndex = 8;
            this._lbl_Trust.Text = "Trustworthy:";
            // 
            // _lbl_Status
            // 
            this._lbl_Status.AutoEllipsis = true;
            this._lbl_Status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Status.Location = new System.Drawing.Point(50, 46);
            this._lbl_Status.Name = "_lbl_Status";
            this._lbl_Status.Size = new System.Drawing.Size(212, 20);
            this._lbl_Status.TabIndex = 12;
            this._lbl_Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_S
            // 
            this._lbl_S.AutoSize = true;
            this._lbl_S.Location = new System.Drawing.Point(6, 50);
            this._lbl_S.Name = "_lbl_S";
            this._lbl_S.Size = new System.Drawing.Size(40, 13);
            this._lbl_S.TabIndex = 11;
            this._lbl_S.Text = "Status:";
            // 
            // _lbl_Name
            // 
            this._lbl_Name.AutoEllipsis = true;
            this._lbl_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Name.Location = new System.Drawing.Point(50, 16);
            this._lbl_Name.Name = "_lbl_Name";
            this._lbl_Name.Size = new System.Drawing.Size(212, 20);
            this._lbl_Name.TabIndex = 8;
            this._lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_N
            // 
            this._lbl_N.AutoSize = true;
            this._lbl_N.Location = new System.Drawing.Point(6, 20);
            this._lbl_N.Name = "_lbl_N";
            this._lbl_N.Size = new System.Drawing.Size(38, 13);
            this._lbl_N.TabIndex = 5;
            this._lbl_N.Text = "Name:";
            // 
            // _lbl_GuestEnabled
            // 
            this._lbl_GuestEnabled.AutoEllipsis = true;
            this._lbl_GuestEnabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_GuestEnabled.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_GuestEnabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_GuestEnabled.Location = new System.Drawing.Point(354, 46);
            this._lbl_GuestEnabled.Name = "_lbl_GuestEnabled";
            this._lbl_GuestEnabled.Size = new System.Drawing.Size(60, 20);
            this._lbl_GuestEnabled.TabIndex = 7;
            this._lbl_GuestEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_G
            // 
            this._lbl_G.AutoSize = true;
            this._lbl_G.Location = new System.Drawing.Point(268, 50);
            this._lbl_G.Name = "_lbl_G";
            this._lbl_G.Size = new System.Drawing.Size(80, 13);
            this._lbl_G.TabIndex = 6;
            this._lbl_G.Text = "Guest Enabled:";
            // 
            // _lbl_Owner
            // 
            this._lbl_Owner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Owner.AutoEllipsis = true;
            this._lbl_Owner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Owner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Owner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Owner.Location = new System.Drawing.Point(315, 16);
            this._lbl_Owner.Name = "_lbl_Owner";
            this._lbl_Owner.Size = new System.Drawing.Size(240, 20);
            this._lbl_Owner.TabIndex = 4;
            this._lbl_Owner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_O
            // 
            this._lbl_O.AutoSize = true;
            this._lbl_O.Location = new System.Drawing.Point(268, 20);
            this._lbl_O.Name = "_lbl_O";
            this._lbl_O.Size = new System.Drawing.Size(41, 13);
            this._lbl_O.TabIndex = 1;
            this._lbl_O.Text = "Owner:";
            // 
            // _pnl_Permissions
            // 
            this._pnl_Permissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pnl_Permissions.Controls.Add(this._permissionsGrid);
            this._pnl_Permissions.Location = new System.Drawing.Point(3, 88);
            this._pnl_Permissions.Name = "_pnl_Permissions";
            this._pnl_Permissions.Size = new System.Drawing.Size(561, 312);
            this._pnl_Permissions.TabIndex = 2;
            this._pnl_Permissions.TabStop = false;
            this._pnl_Permissions.Text = "Permissions";
            // 
            // _permissionsGrid
            // 
            this._permissionsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._permissionsGrid.Location = new System.Drawing.Point(9, 19);
            this._permissionsGrid.Name = "_permissionsGrid";
            this._permissionsGrid.Size = new System.Drawing.Size(546, 287);
            this._permissionsGrid.TabIndex = 0;
            // 
            // _btn_Help
            // 
            this._btn_Help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Help.Location = new System.Drawing.Point(483, 10);
            this._btn_Help.Name = "_btn_Help";
            this._btn_Help.Size = new System.Drawing.Size(75, 23);
            this._btn_Help.TabIndex = 2;
            this._btn_Help.Text = "&Help";
            this._btn_Help.Click += new System.EventHandler(this._btn_Help_Click);
            // 
            // Form_SnapshotDatabaseProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 496);
            this.Description = "View properties of this SQL Server Database.";
            this.MinimumSize = new System.Drawing.Size(470, 380);
            this.Name = "Form_SnapshotDatabaseProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.DB_48;
            this.Text = "Form_SnapshotDatabaseProperties";
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._pnl_Properties.ResumeLayout(false);
            this._grpbx_ObjProperties.ResumeLayout(false);
            this._grpbx_ObjProperties.PerformLayout();
            this._pnl_Permissions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _btn_Close;
        private System.Windows.Forms.Panel _pnl_Properties;
        private System.Windows.Forms.GroupBox _pnl_Permissions;
        private Idera.SQLsecure.UI.Console.Controls.ObjectPermissionsGrid _permissionsGrid;
        private System.Windows.Forms.GroupBox _grpbx_ObjProperties;
        private System.Windows.Forms.Label _lbl_Name;
        private System.Windows.Forms.Label _lbl_N;
        private System.Windows.Forms.Label _lbl_GuestEnabled;
        private System.Windows.Forms.Label _lbl_G;
        private System.Windows.Forms.Label _lbl_Owner;
        private System.Windows.Forms.Label _lbl_O;
        private System.Windows.Forms.Label _lbl_Status;
        private System.Windows.Forms.Label _lbl_S;
        private Infragistics.Win.Misc.UltraButton _btn_Help;
        private System.Windows.Forms.Label _lbl_Trustworthy;
        private System.Windows.Forms.Label _lbl_Trust;
    }
}