namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SnapshotFileProperties
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
            this._lbl_DiskType = new System.Windows.Forms.Label();
            this._lbl_DT = new System.Windows.Forms.Label();
            this._lbl_Type = new System.Windows.Forms.Label();
            this._lbl_T = new System.Windows.Forms.Label();
            this._lbl_Name = new System.Windows.Forms.Label();
            this._lbl_N = new System.Windows.Forms.Label();
            this._lbl_Database = new System.Windows.Forms.Label();
            this._lbl_D = new System.Windows.Forms.Label();
            this._lbl_Owner = new System.Windows.Forms.Label();
            this._lbl_O = new System.Windows.Forms.Label();
            this._pnl_Permissions = new System.Windows.Forms.GroupBox();
            this._permissionsGrid = new Idera.SQLsecure.UI.Console.Controls.OsObjectPermissionsGrid();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this._pnl_Properties.SuspendLayout();
            this._grpbx_ObjProperties.SuspendLayout();
            this._pnl_Permissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._btn_Close);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 429);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(567, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Close, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._pnl_Properties);
            this._bf_MainPanel.Size = new System.Drawing.Size(567, 376);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(567, 53);
            // 
            // _btn_Close
            // 
            this._btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btn_Close.Location = new System.Drawing.Point(480, 9);
            this._btn_Close.Name = "_btn_Close";
            this._btn_Close.Size = new System.Drawing.Size(75, 23);
            this._btn_Close.TabIndex = 24;
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
            this._pnl_Properties.Size = new System.Drawing.Size(567, 376);
            this._pnl_Properties.TabIndex = 25;
            // 
            // _grpbx_ObjProperties
            // 
            this._grpbx_ObjProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_ObjProperties.Controls.Add(this._lbl_DiskType);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_DT);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Type);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_T);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Name);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_N);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Database);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_D);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Owner);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_O);
            this._grpbx_ObjProperties.Location = new System.Drawing.Point(3, 3);
            this._grpbx_ObjProperties.Name = "_grpbx_ObjProperties";
            this._grpbx_ObjProperties.Size = new System.Drawing.Size(561, 100);
            this._grpbx_ObjProperties.TabIndex = 5;
            this._grpbx_ObjProperties.TabStop = false;
            this._grpbx_ObjProperties.Text = "General";
            // 
            // _lbl_DiskType
            // 
            this._lbl_DiskType.AutoEllipsis = true;
            this._lbl_DiskType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_DiskType.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_DiskType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_DiskType.Location = new System.Drawing.Point(66, 68);
            this._lbl_DiskType.Name = "_lbl_DiskType";
            this._lbl_DiskType.Size = new System.Drawing.Size(111, 20);
            this._lbl_DiskType.TabIndex = 14;
            this._lbl_DiskType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_DT
            // 
            this._lbl_DT.AutoSize = true;
            this._lbl_DT.Location = new System.Drawing.Point(6, 72);
            this._lbl_DT.Name = "_lbl_DT";
            this._lbl_DT.Size = new System.Drawing.Size(58, 13);
            this._lbl_DT.TabIndex = 13;
            this._lbl_DT.Text = "Disk Type:";
            // 
            // _lbl_Type
            // 
            this._lbl_Type.AutoEllipsis = true;
            this._lbl_Type.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Type.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Type.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Type.Location = new System.Drawing.Point(66, 42);
            this._lbl_Type.Name = "_lbl_Type";
            this._lbl_Type.Size = new System.Drawing.Size(111, 20);
            this._lbl_Type.TabIndex = 12;
            this._lbl_Type.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_T
            // 
            this._lbl_T.AutoSize = true;
            this._lbl_T.Location = new System.Drawing.Point(6, 46);
            this._lbl_T.Name = "_lbl_T";
            this._lbl_T.Size = new System.Drawing.Size(34, 13);
            this._lbl_T.TabIndex = 11;
            this._lbl_T.Text = "Type:";
            // 
            // _lbl_Name
            // 
            this._lbl_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Name.AutoEllipsis = true;
            this._lbl_Name.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Name.Location = new System.Drawing.Point(66, 16);
            this._lbl_Name.Name = "_lbl_Name";
            this._lbl_Name.Size = new System.Drawing.Size(486, 20);
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
            // _lbl_Database
            // 
            this._lbl_Database.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Database.AutoEllipsis = true;
            this._lbl_Database.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Database.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Database.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Database.Location = new System.Drawing.Point(244, 68);
            this._lbl_Database.Name = "_lbl_Database";
            this._lbl_Database.Size = new System.Drawing.Size(308, 20);
            this._lbl_Database.TabIndex = 7;
            this._lbl_Database.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._lbl_Database.Visible = false;
            // 
            // _lbl_D
            // 
            this._lbl_D.AutoSize = true;
            this._lbl_D.Location = new System.Drawing.Point(183, 72);
            this._lbl_D.Name = "_lbl_D";
            this._lbl_D.Size = new System.Drawing.Size(56, 13);
            this._lbl_D.TabIndex = 6;
            this._lbl_D.Text = "Database:";
            this._lbl_D.Visible = false;
            // 
            // _lbl_Owner
            // 
            this._lbl_Owner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Owner.AutoEllipsis = true;
            this._lbl_Owner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Owner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Owner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Owner.Location = new System.Drawing.Point(244, 42);
            this._lbl_Owner.Name = "_lbl_Owner";
            this._lbl_Owner.Size = new System.Drawing.Size(308, 20);
            this._lbl_Owner.TabIndex = 4;
            this._lbl_Owner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _lbl_O
            // 
            this._lbl_O.AutoSize = true;
            this._lbl_O.Location = new System.Drawing.Point(183, 46);
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
            this._pnl_Permissions.Location = new System.Drawing.Point(3, 109);
            this._pnl_Permissions.Name = "_pnl_Permissions";
            this._pnl_Permissions.Size = new System.Drawing.Size(561, 264);
            this._pnl_Permissions.TabIndex = 2;
            this._pnl_Permissions.TabStop = false;
            this._pnl_Permissions.Text = "Rights";
            // 
            // _permissionsGrid
            // 
            this._permissionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._permissionsGrid.Location = new System.Drawing.Point(3, 16);
            this._permissionsGrid.Name = "_permissionsGrid";
            this._permissionsGrid.Size = new System.Drawing.Size(555, 245);
            this._permissionsGrid.TabIndex = 0;
            // 
            // Form_SnapshotFileProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 469);
            this.Description = "View properties of this SQL Server file";
            this.Name = "Form_SnapshotFileProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.File_48;
            this.Text = "Form_SnapshotFileProperties";
            this.TopMost = true;
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
        private System.Windows.Forms.GroupBox _grpbx_ObjProperties;
        private System.Windows.Forms.Label _lbl_Type;
        private System.Windows.Forms.Label _lbl_T;
        private System.Windows.Forms.Label _lbl_Name;
        private System.Windows.Forms.Label _lbl_N;
        private System.Windows.Forms.Label _lbl_Database;
        private System.Windows.Forms.Label _lbl_D;
        private System.Windows.Forms.Label _lbl_Owner;
        private System.Windows.Forms.Label _lbl_O;
        private System.Windows.Forms.GroupBox _pnl_Permissions;
        private System.Windows.Forms.Label _lbl_DiskType;
        private System.Windows.Forms.Label _lbl_DT;
        private Idera.SQLsecure.UI.Console.Controls.OsObjectPermissionsGrid _permissionsGrid;

    }
}