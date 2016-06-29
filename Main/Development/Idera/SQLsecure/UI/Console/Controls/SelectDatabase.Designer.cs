namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class SelectDatabase
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
            this.radioButton_ServerOnly = new System.Windows.Forms.RadioButton();
            this.radioButton_ServerAndDatabase = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraListViewDatabases = new Infragistics.Win.UltraWinListView.UltraListView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewDatabases)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton_ServerOnly
            // 
            this.radioButton_ServerOnly.AutoSize = true;
            this.radioButton_ServerOnly.Location = new System.Drawing.Point(3, 12);
            this.radioButton_ServerOnly.Name = "radioButton_ServerOnly";
            this.radioButton_ServerOnly.Size = new System.Drawing.Size(80, 17);
            this.radioButton_ServerOnly.TabIndex = 2;
            this.radioButton_ServerOnly.TabStop = true;
            this.radioButton_ServerOnly.Text = "Server Only";
            this.radioButton_ServerOnly.UseVisualStyleBackColor = true;
            this.radioButton_ServerOnly.CheckedChanged += new System.EventHandler(this.radioButton_ServerOnly_CheckedChanged);
            // 
            // radioButton_ServerAndDatabase
            // 
            this.radioButton_ServerAndDatabase.AutoSize = true;
            this.radioButton_ServerAndDatabase.Location = new System.Drawing.Point(148, 12);
            this.radioButton_ServerAndDatabase.Name = "radioButton_ServerAndDatabase";
            this.radioButton_ServerAndDatabase.Size = new System.Drawing.Size(126, 17);
            this.radioButton_ServerAndDatabase.TabIndex = 3;
            this.radioButton_ServerAndDatabase.TabStop = true;
            this.radioButton_ServerAndDatabase.Text = "Server and Database";
            this.radioButton_ServerAndDatabase.UseVisualStyleBackColor = true;
            this.radioButton_ServerAndDatabase.CheckedChanged += new System.EventHandler(this.radioButton_ServerAndDatabase_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton_ServerOnly);
            this.panel1.Controls.Add(this.radioButton_ServerAndDatabase);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(310, 40);
            this.panel1.TabIndex = 4;
            // 
            // ultraListViewDatabases
            // 
            this.ultraListViewDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraListViewDatabases.ItemSettings.AllowEdit = Infragistics.Win.DefaultableBoolean.False;
            this.ultraListViewDatabases.Location = new System.Drawing.Point(0, 40);
            this.ultraListViewDatabases.MainColumn.Text = "Database";
            this.ultraListViewDatabases.Name = "ultraListViewDatabases";
            this.ultraListViewDatabases.Size = new System.Drawing.Size(310, 213);
            this.ultraListViewDatabases.TabIndex = 5;
            this.ultraListViewDatabases.Text = "ultraListView1";
            this.ultraListViewDatabases.View = Infragistics.Win.UltraWinListView.UltraListViewStyle.Details;
            this.ultraListViewDatabases.ItemSelectionChanged += new Infragistics.Win.UltraWinListView.ItemSelectionChangedEventHandler(this.ultraListViewDatabases_ItemSelectionChanged);
            // 
            // SelectDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ultraListViewDatabases);
            this.Controls.Add(this.panel1);
            this.Name = "SelectDatabase";
            this.Size = new System.Drawing.Size(310, 253);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraListViewDatabases)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_ServerOnly;
        private System.Windows.Forms.RadioButton radioButton_ServerAndDatabase;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.UltraWinListView.UltraListView ultraListViewDatabases;
    }
}
