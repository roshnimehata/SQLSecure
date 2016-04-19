namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class SelectServer
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
            this.ultraListViewSelectServer = new Infragistics.Win.UltraWinListView.UltraListView();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewSelectServer)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraListViewSelectServer
            // 
            this.ultraListViewSelectServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListViewSelectServer.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListViewSelectServer.Location = new System.Drawing.Point(0, 0);
            this.ultraListViewSelectServer.MainColumn.Text = "Server";
            this.ultraListViewSelectServer.Name = "ultraListViewSelectServer";
            this.ultraListViewSelectServer.Size = new System.Drawing.Size(288, 253);
            this.ultraListViewSelectServer.TabIndex = 4;
            this.ultraListViewSelectServer.Text = "ultraListView1";
            this.ultraListViewSelectServer.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListViewSelectServer.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.ultraListViewSelectServer_ItemSelectionChanged);
            // 
            // SelectServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraListViewSelectServer);
            this.Name = "SelectServer";
            this.Size = new System.Drawing.Size(288, 253);
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewSelectServer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinListView.UltraListView ultraListViewSelectServer;

    }
}
