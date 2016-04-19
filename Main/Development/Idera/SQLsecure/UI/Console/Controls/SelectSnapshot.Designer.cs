namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class SelectSnapshot
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
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn1 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Status");
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn2 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Icon");
            Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn ultraListViewSubItemColumn3 = new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn("Baseline");
            this.ultraListViewSnapshots = new Infragistics.Win.UltraWinListView.UltraListView();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewSnapshots)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraListViewSnapshots
            // 
            this.ultraListViewSnapshots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListViewSnapshots.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListViewSnapshots.Location = new System.Drawing.Point(0, 0);
            this.ultraListViewSnapshots.MainColumn.Key = "Snapshot";
            this.ultraListViewSnapshots.MainColumn.Text = "Snapshot";
            this.ultraListViewSnapshots.MainColumn.VisiblePositionInDetailsView = 1;
            this.ultraListViewSnapshots.MainColumn.Width = 150;
            this.ultraListViewSnapshots.Name = "ultraListViewSnapshots";
            this.ultraListViewSnapshots.Size = new System.Drawing.Size(359, 253);
            ultraListViewSubItemColumn1.Key = "Status";
            ultraListViewSubItemColumn1.Text = "Status";
            ultraListViewSubItemColumn1.VisiblePositionInDetailsView = 2;
            ultraListViewSubItemColumn1.Width = 100;
            ultraListViewSubItemColumn2.DataType = typeof(System.Drawing.Bitmap);
            ultraListViewSubItemColumn2.Key = "Icon";
            ultraListViewSubItemColumn2.Text = " ";
            ultraListViewSubItemColumn2.VisiblePositionInDetailsView = 0;
            ultraListViewSubItemColumn2.Width = 30;
            ultraListViewSubItemColumn3.Key = "Baseline";
            ultraListViewSubItemColumn3.Text = "Baseline";
            ultraListViewSubItemColumn3.VisiblePositionInDetailsView = 3;
            ultraListViewSubItemColumn3.Width = 60;
            this.ultraListViewSnapshots.SubItemColumns.AddRange(new Infragistics.Win.UltraWinListView.UltraListViewSubItemColumn[] {
            ultraListViewSubItemColumn1,
            ultraListViewSubItemColumn2,
            ultraListViewSubItemColumn3});
            this.ultraListViewSnapshots.TabIndex = 3;
            this.ultraListViewSnapshots.Text = "ultraListView1";
            this.ultraListViewSnapshots.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListViewSnapshots.ViewSettingsDetails.FullRowSelect = true;
            this.ultraListViewSnapshots.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.ultraListViewSnapshots_ItemSelectionChanged);
            // 
            // SelectSnapshot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraListViewSnapshots);
            this.Name = "SelectSnapshot";
            this.Size = new System.Drawing.Size(359, 253);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewSnapshots)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinListView.UltraListView ultraListViewSnapshots;
    }
}
