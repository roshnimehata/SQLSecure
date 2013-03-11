namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class FilterSelection
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Scope");
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn2 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Name");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterSelection));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ultraListViewFilters = new Infragistics.Win.UltraWinListView.UltraListView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.contextMenuStripType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Any = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_LikeDialog = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_LikeBelow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gradientPanel1 = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewFilters)).BeginInit();
            this.contextMenuStripType.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(200)))), ((int)(((byte)(206)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ultraListViewFilters);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(486, 392);
            this.splitContainer1.SplitterDistance = 241;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // ultraListViewFilters
            // 
            this.ultraListViewFilters.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraListViewFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListViewFilters.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.data_gear;
            this.ultraListViewFilters.ItemSettings.Appearance = appearance1;
            this.ultraListViewFilters.Location = new System.Drawing.Point(0, 0);
            this.ultraListViewFilters.MainColumn.Key = "Object";
            this.ultraListViewFilters.MainColumn.Text = "Object";
            this.ultraListViewFilters.MainColumn.VisiblePositionInDetailsView = 0;
            this.ultraListViewFilters.MainColumn.Width = 220;
            this.ultraListViewFilters.Name = "ultraListViewFilters";
            this.ultraListViewFilters.Size = new System.Drawing.Size(486, 241);
            ultraListViewSubItemColumn1.Key = "Scope";
            ultraListViewSubItemColumn1.Text = "Scope matches";
            ultraListViewSubItemColumn1.VisiblePositionInDetailsView = 1;
            ultraListViewSubItemColumn1.Width = 120;
            ultraListViewSubItemColumn2.Key = "Name";
            ultraListViewSubItemColumn2.Text = "Name matches";
            ultraListViewSubItemColumn2.VisiblePositionInDetailsView = 2;
            ultraListViewSubItemColumn2.Width = 120;
            this.ultraListViewFilters.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1,
            ultraListViewSubItemColumn2});
            this.ultraListViewFilters.TabIndex = 2;
            this.ultraListViewFilters.Text = "ultraListView1";
            this.ultraListViewFilters.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListViewFilters.ViewSettingsDetails.CheckBoxStyle = Infragistics.Win.UltraWinListView.CheckBoxStyle.CheckBox;
            this.ultraListViewFilters.Enter += new System.EventHandler(this.ultraListViewFilters_Enter);
            this.ultraListViewFilters.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ultraListViewFilters_MouseMove);
            this.ultraListViewFilters.ItemCheckStateChanged += new Infragistics.Win.UltraWinListView.ItemCheckStateChangedEventHandler(this.ultraListViewFilters_ItemCheckStateChanged);
            this.ultraListViewFilters.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ultraListViewFilters_MouseDown);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(486, 149);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // contextMenuStripType
            // 
            this.contextMenuStripType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userToolStripMenuItem,
            this.systemToolStripMenuItem,
            this.allToolStripMenuItem});
            this.contextMenuStripType.Name = "contextMenuStripType";
            this.contextMenuStripType.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStripType.ShowImageMargin = false;
            this.contextMenuStripType.Size = new System.Drawing.Size(134, 70);
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.userToolStripMenuItem.Text = "User";
            this.userToolStripMenuItem.Click += new System.EventHandler(this.userToolStripMenuItem_Click);
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.systemToolStripMenuItem.Text = "System";
            this.systemToolStripMenuItem.Click += new System.EventHandler(this.systemToolStripMenuItem_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.allToolStripMenuItem.Text = "System or User";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_Any
            // 
            this.toolStripMenuItem_Any.Name = "toolStripMenuItem_Any";
            this.toolStripMenuItem_Any.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem_LikeDialog
            // 
            this.toolStripMenuItem_LikeDialog.Name = "toolStripMenuItem_LikeDialog";
            this.toolStripMenuItem_LikeDialog.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripMenuItem_LikeBelow
            // 
            this.toolStripMenuItem_LikeBelow.Name = "toolStripMenuItem_LikeBelow";
            this.toolStripMenuItem_LikeBelow.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 21);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.gradientPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(490, 420);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(486, 392);
            this.panel3.TabIndex = 1;
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gradientPanel1.Controls.Add(this.label7);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanel1.GradientBorderMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DOut;
            this.gradientPanel1.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Rotation = 270F;
            this.gradientPanel1.Size = new System.Drawing.Size(486, 24);
            this.gradientPanel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(5, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(467, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Select filter for databases and database objects to collect. Click on underlined " +
                "text to edit settings.";
            // 
            // FilterSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel2);
            this.Name = "FilterSelection";
            this.Size = new System.Drawing.Size(490, 420);
            this.Load += new System.EventHandler(this.FilterSelection_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewFilters)).EndInit();
            this.contextMenuStripType.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripType;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Any;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_LikeDialog;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_LikeBelow;
        private GradientPanel gradientPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListViewFilters;
    }
}
