namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_SnapshotDbObjProperties
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
            this._lbl_Schema = new System.Windows.Forms.Label();
            this._lbl_S = new System.Windows.Forms.Label();
            this._lbl_Owner = new System.Windows.Forms.Label();
            this._lbl_O = new System.Windows.Forms.Label();
            this._lbl_SchemaOwner = new System.Windows.Forms.Label();
            this._lbl_SO = new System.Windows.Forms.Label();
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
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 454);
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(567, 40);
            this._bfd_ButtonPanel.Controls.SetChildIndex(this._btn_Close, 0);
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Controls.Add(this._pnl_Properties);
            this._bf_MainPanel.Size = new System.Drawing.Size(567, 401);
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
            this._btn_Close.TabIndex = 3;
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
            this._pnl_Properties.Size = new System.Drawing.Size(567, 401);
            this._pnl_Properties.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this._permissionsGrid);
            this.groupBox1.Location = new System.Drawing.Point(3, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 315);
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
            this._permissionsGrid.Size = new System.Drawing.Size(546, 290);
            this._permissionsGrid.TabIndex = 0;
            // 
            // _pnl_Object
            // 
            this._pnl_Object.BackColor = System.Drawing.Color.Transparent;
            this._pnl_Object.Controls.Add(this._grpbx_ObjProperties);
            this._pnl_Object.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnl_Object.Location = new System.Drawing.Point(0, 0);
            this._pnl_Object.Name = "_pnl_Object";
            this._pnl_Object.Size = new System.Drawing.Size(567, 77);
            this._pnl_Object.TabIndex = 1;
            // 
            // _grpbx_ObjProperties
            // 
            this._grpbx_ObjProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Name);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_N);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Schema);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_S);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_Owner);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_O);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_SchemaOwner);
            this._grpbx_ObjProperties.Controls.Add(this._lbl_SO);
            this._grpbx_ObjProperties.Location = new System.Drawing.Point(3, 3);
            this._grpbx_ObjProperties.Name = "_grpbx_ObjProperties";
            this._grpbx_ObjProperties.Size = new System.Drawing.Size(561, 68);
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
            // _lbl_Schema
            // 
            this._lbl_Schema.AutoEllipsis = true;
            this._lbl_Schema.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Schema.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Schema.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Schema.Location = new System.Drawing.Point(61, 40);
            this._lbl_Schema.Name = "_lbl_Schema";
            this._lbl_Schema.Size = new System.Drawing.Size(212, 15);
            this._lbl_Schema.TabIndex = 7;
            // 
            // _lbl_S
            // 
            this._lbl_S.AutoSize = true;
            this._lbl_S.Location = new System.Drawing.Point(6, 40);
            this._lbl_S.Name = "_lbl_S";
            this._lbl_S.Size = new System.Drawing.Size(49, 13);
            this._lbl_S.TabIndex = 6;
            this._lbl_S.Text = "Schema:";
            // 
            // _lbl_Owner
            // 
            this._lbl_Owner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_Owner.AutoEllipsis = true;
            this._lbl_Owner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_Owner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_Owner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_Owner.Location = new System.Drawing.Point(368, 15);
            this._lbl_Owner.Name = "_lbl_Owner";
            this._lbl_Owner.Size = new System.Drawing.Size(187, 15);
            this._lbl_Owner.TabIndex = 4;
            // 
            // _lbl_O
            // 
            this._lbl_O.AutoSize = true;
            this._lbl_O.Location = new System.Drawing.Point(279, 16);
            this._lbl_O.Name = "_lbl_O";
            this._lbl_O.Size = new System.Drawing.Size(41, 13);
            this._lbl_O.TabIndex = 1;
            this._lbl_O.Text = "Owner:";
            // 
            // _lbl_SchemaOwner
            // 
            this._lbl_SchemaOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._lbl_SchemaOwner.AutoEllipsis = true;
            this._lbl_SchemaOwner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this._lbl_SchemaOwner.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._lbl_SchemaOwner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this._lbl_SchemaOwner.Location = new System.Drawing.Point(368, 40);
            this._lbl_SchemaOwner.Name = "_lbl_SchemaOwner";
            this._lbl_SchemaOwner.Size = new System.Drawing.Size(187, 15);
            this._lbl_SchemaOwner.TabIndex = 3;
            // 
            // _lbl_SO
            // 
            this._lbl_SO.AutoSize = true;
            this._lbl_SO.Location = new System.Drawing.Point(279, 40);
            this._lbl_SO.Name = "_lbl_SO";
            this._lbl_SO.Size = new System.Drawing.Size(83, 13);
            this._lbl_SO.TabIndex = 2;
            this._lbl_SO.Text = "Schema Owner:";
            // 
            // Form_SnapshotDbObjProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 494);
            this.Description = "View properties for the SQL Server Database Object.";
            this.MinimumSize = new System.Drawing.Size(470, 370);
            this.Name = "Form_SnapshotDbObjProperties";
            this.Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.UserDefinedData_48;
            this.Text = "Forms_SnapshotDbObjProperties";
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
        private Idera.SQLsecure.UI.Console.Controls.ObjectPermissionsGrid _permissionsGrid;
        private System.Windows.Forms.Panel _pnl_Properties;
        private System.Windows.Forms.Panel _pnl_Object;
        private System.Windows.Forms.Label _lbl_SO;
        private System.Windows.Forms.Label _lbl_O;
        private System.Windows.Forms.Label _lbl_SchemaOwner;
        private System.Windows.Forms.Label _lbl_Owner;
        private System.Windows.Forms.GroupBox _grpbx_ObjProperties;
        private System.Windows.Forms.Label _lbl_Name;
        private System.Windows.Forms.Label _lbl_N;
        private System.Windows.Forms.Label _lbl_Schema;
        private System.Windows.Forms.Label _lbl_S;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}