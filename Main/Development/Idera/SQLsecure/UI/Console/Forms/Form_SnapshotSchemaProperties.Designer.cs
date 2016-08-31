namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SnapshotSchemaProperties
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
            this._pnl_Permissions = new System.Windows.Forms.GroupBox();
            this._permissionsGrid = new Idera.SQLsecure.UI.Console.Controls.ObjectPermissionsGrid();
            this._pnl_Object = new System.Windows.Forms.Panel();
            this._grpbx_ObjProperties = new System.Windows.Forms.GroupBox();
            this._lbl_Name = new System.Windows.Forms.Label();
            this._lbl_N = new System.Windows.Forms.Label();
            this._lbl_Owner = new System.Windows.Forms.Label();
            this._lbl_O = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this._bf_MainPanel.SuspendLayout();
            this._pnl_Properties.SuspendLayout();
            this._pnl_Permissions.SuspendLayout();
            this._pnl_Object.SuspendLayout();
            this._grpbx_ObjProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.Controls.Add(this._btn_Close);
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 397);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(567, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Close, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._pnl_Properties);
            this._bf_MainPanel.Size = new System.Drawing.Size(567, 344);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.Size = new System.Drawing.Size(567, 53);
            // 
            // _btn_Close
            // 
            this._btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btn_Close.Location = new System.Drawing.Point(471, 9);
            this._btn_Close.Name = "_btn_Close";
            this._btn_Close.Size = new System.Drawing.Size(75, 23);
            this._btn_Close.TabIndex = 22;
            this._btn_Close.Text = "&Close";
            // 
            // _pnl_Properties
            // 
            this._pnl_Properties.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Properties.Controls.Add(this._pnl_Permissions);
            this._pnl_Properties.Controls.Add(this._pnl_Object);
            this._pnl_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnl_Properties.Location = new System.Drawing.Point(0, 0);
            this._pnl_Properties.Name = "_pnl_Properties";
            this._pnl_Properties.Size = new System.Drawing.Size(567, 344);
            this._pnl_Properties.TabIndex = 23;
            // 
            // _pnl_Permissions
            // 
            this._pnl_Permissions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._pnl_Permissions.Controls.Add(this._permissionsGrid);
            this._pnl_Permissions.Location = new System.Drawing.Point(3, 52);
            this._pnl_Permissions.Name = "_pnl_Permissions";
            this._pnl_Permissions.Size = new System.Drawing.Size(561, 286);
            this._pnl_Permissions.TabIndex = 2;
            this._pnl_Permissions.TabStop = false;
            this._pnl_Permissions.Text = "Permissions";
            // 
            // _permissionsGrid
            // 
            this._permissionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._permissionsGrid.Location = new System.Drawing.Point(3, 16);
            this._permissionsGrid.Name = "_permissionsGrid";
            this._permissionsGrid.Size = new System.Drawing.Size(555, 267);
            this._permissionsGrid.TabIndex = 0;
            // 
            // _pnl_Object
            // 
            this._pnl_Object.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Object.Controls.Add(this._grpbx_ObjProperties);
            this._pnl_Object.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_Object.Location = new System.Drawing.Point(0, 0);
            this._pnl_Object.Name = "_pnl_Object";
            this._pnl_Object.Size = new System.Drawing.Size(567, 63);
            this._pnl_Object.TabIndex = 1;
            // 
            // _grpbx_ObjProperties
            // 
            this._grpbx_ObjProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Name);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_N);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Owner);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_O);
            this._grpbx_ObjProperties.Location = new System.Drawing.Point(3, 3);
            this._grpbx_ObjProperties.Name = "_grpbx_ObjProperties";
            this._grpbx_ObjProperties.Size = new System.Drawing.Size(534, 43);
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
            this._lbl_Name.Location = new System.Drawing.Point(50, 16);
            this._lbl_Name.Name = "_lbl_Name";
            this._lbl_Name.Size = new System.Drawing.Size(243, 15);
            this._lbl_Name.TabIndex = 8;
            this._lbl_Name.Text = "label2";
            // 
            // _lbl_N
            // 
            this._lbl_N.AutoSize = true;
            this._lbl_N.Location = new System.Drawing.Point(6, 17);
            this._lbl_N.Name = "_lbl_N";
            this._lbl_N.Size = new System.Drawing.Size(38, 13);
            this._lbl_N.TabIndex = 5;
            this._lbl_N.Text = "Name:";
            // 
            // _lbl_Owner
            // 
            this._lbl_Owner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Owner.AutoEllipsis = true;
            this._lbl_Owner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Owner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Owner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Owner.Location = new System.Drawing.Point(349, 16);
            this._lbl_Owner.Name = "_lbl_Owner";
            this._lbl_Owner.Size = new System.Drawing.Size(152, 15);
            this._lbl_Owner.TabIndex = 4;
            this._lbl_Owner.Text = "label2";
            // 
            // _lbl_O
            // 
            this._lbl_O.AutoSize = true;
            this._lbl_O.Location = new System.Drawing.Point(302, 17);
            this._lbl_O.Name = "_lbl_O";
            this._lbl_O.Size = new System.Drawing.Size(41, 13);
            this._lbl_O.TabIndex = 1;
            this._lbl_O.Text = "Owner:";
            // 
            // Form_SnapshotSchemaProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 437);
            this.Description = "View properties of this SQL Server Schema";
            this.Name = "Form_SnapshotSchemaProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Schema_48;
            this.Text = "Form_SnapshotSchemaProperties";
            this._bfd_ButtonPanel.ResumeLayout(false);
            this._bf_MainPanel.ResumeLayout(false);
            this._pnl_Properties.ResumeLayout(false);
            this._pnl_Permissions.ResumeLayout(false);
            this._pnl_Object.ResumeLayout(false);
            this._grpbx_ObjProperties.ResumeLayout(false);
            this._grpbx_ObjProperties.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _btn_Close;
        private System.Windows.Forms.Panel _pnl_Properties;
        private System.Windows.Forms.GroupBox _pnl_Permissions;
        private Idera.SQLsecure.UI.Console.Controls.ObjectPermissionsGrid _permissionsGrid;
        private System.Windows.Forms.Panel _pnl_Object;
        private System.Windows.Forms.GroupBox _grpbx_ObjProperties;
        private System.Windows.Forms.Label _lbl_Name;
        private System.Windows.Forms.Label _lbl_N;
        private System.Windows.Forms.Label _lbl_Owner;
        private System.Windows.Forms.Label _lbl_O;
    }
}