namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class BaseForm
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
            this._bf_MainPanel = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._bf_HeaderPanel = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._bf_label_Description = new System.Windows.Forms.Label();
            this._bf_pictureBox = new System.Windows.Forms.PictureBox();
            this._bf_HeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bf_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this._bf_MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bf_MainPanel.GradientColor = System.Drawing.Color.WhiteSmoke;
            this._bf_MainPanel.Location = new System.Drawing.Point(0, 53);
            this._bf_MainPanel.Name = "_bf_MainPanel";
            this._bf_MainPanel.Rotation = 270F;
            this._bf_MainPanel.Size = new System.Drawing.Size(570, 383);
            this._bf_MainPanel.TabIndex = 1;
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this._bf_HeaderPanel.Controls.Add(this._bf_label_Description);
            this._bf_HeaderPanel.Controls.Add(this._bf_pictureBox);
            this._bf_HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._bf_HeaderPanel.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._bf_HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this._bf_HeaderPanel.Margin = new System.Windows.Forms.Padding(0);
            this._bf_HeaderPanel.Name = "_bf_HeaderPanel";
            this._bf_HeaderPanel.Padding = new System.Windows.Forms.Padding(3);
            this._bf_HeaderPanel.Rotation = 270F;
            this._bf_HeaderPanel.Size = new System.Drawing.Size(570, 53);
            this._bf_HeaderPanel.TabIndex = 2;
            // 
            // _bf_label_Description
            // 
            this._bf_label_Description.BackColor = System.Drawing.Color.Transparent;
            this._bf_label_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this._bf_label_Description.ForeColor = System.Drawing.Color.Navy;
            this._bf_label_Description.Location = new System.Drawing.Point(52, 3);
            this._bf_label_Description.Name = "_bf_label_Description";
            this._bf_label_Description.Size = new System.Drawing.Size(515, 47);
            this._bf_label_Description.TabIndex = 1;
            this._bf_label_Description.Text = "Description of what dialog does.";
            this._bf_label_Description.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _bf_pictureBox
            // 
            this._bf_pictureBox.BackColor = System.Drawing.Color.Transparent;
            this._bf_pictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this._bf_pictureBox.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.SQLServerAudit;
            this._bf_pictureBox.Location = new System.Drawing.Point(3, 3);
            this._bf_pictureBox.Name = "_bf_pictureBox";
            this._bf_pictureBox.Size = new System.Drawing.Size(49, 47);
            this._bf_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._bf_pictureBox.TabIndex = 0;
            this._bf_pictureBox.TabStop = false;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 436);
            this.ControlBox = false;
            this.Controls.Add(this._bf_MainPanel);
            this.Controls.Add(this._bf_HeaderPanel);
            this.Name = "BaseForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BaseForm";
            this._bf_HeaderPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._bf_pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected GradientPanel _bf_MainPanel;
        protected GradientPanel _bf_HeaderPanel;
        private System.Windows.Forms.PictureBox _bf_pictureBox;
        private System.Windows.Forms.Label _bf_label_Description;
    }
}