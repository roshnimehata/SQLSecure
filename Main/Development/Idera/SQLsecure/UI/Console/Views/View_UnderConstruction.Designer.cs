namespace Idera.SQLsecure.UI.Console.Views
{
    partial class View_UnderConstruction
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
            this.label1 = new System.Windows.Forms.Label();
            this._vw_MainPanel.SuspendLayout();
            this._vw_TasksPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _vw_MainPanel
            // 
            this._vw_MainPanel.Controls.Add(this.label1);
            this._vw_MainPanel.Controls.SetChildIndex(this.label1, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(257, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Under Construction";
            // 
            // View_UnderConstruction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "View_UnderConstruction";
            this.Title = "Under Construction";
            this._vw_MainPanel.ResumeLayout(false);
            this._vw_MainPanel.PerformLayout();
            this._vw_TasksPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;

    }
}
