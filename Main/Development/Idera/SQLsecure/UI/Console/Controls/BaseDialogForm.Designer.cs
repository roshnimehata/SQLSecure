namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class BaseDialogForm
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
            this._bfd_ButtonPanel = new System.Windows.Forms.Panel();
            this._bfd_labelDivider = new System.Windows.Forms.Label();
            this._bfd_ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.ForeColor = System.Drawing.Color.Navy;
            this._bf_MainPanel.Size = new System.Drawing.Size(570, 343);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            // 
            // _bfd_ButtonPanel
            // 
            this._bfd_ButtonPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this._bfd_ButtonPanel.Controls.Add(this._bfd_labelDivider);
            this._bfd_ButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._bfd_ButtonPanel.Location = new System.Drawing.Point(0, 396);
            this._bfd_ButtonPanel.Name = "_bfd_ButtonPanel";
            this._bfd_ButtonPanel.Size = new System.Drawing.Size(570, 40);
            this._bfd_ButtonPanel.TabIndex = 3;
            // 
            // _bfd_labelDivider
            // 
            this._bfd_labelDivider.BackColor = System.Drawing.Color.Gainsboro;
            this._bfd_labelDivider.Dock = System.Windows.Forms.DockStyle.Top;
            this._bfd_labelDivider.Location = new System.Drawing.Point(0, 0);
            this._bfd_labelDivider.Name = "_bfd_labelDivider";
            this._bfd_labelDivider.Size = new System.Drawing.Size(570, 1);
            this._bfd_labelDivider.TabIndex = 0;
            // 
            // BaseDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(570, 436);
            this.ControlBox = true;
            this.Controls.Add(this._bfd_ButtonPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseDialogForm";
            this.ShowIcon = false;
            this.Text = "BaseDialogForm";
            this.Controls.SetChildIndex(this._bf_HeaderPanel, 0);
            this.Controls.SetChildIndex(this._bfd_ButtonPanel, 0);
            this.Controls.SetChildIndex(this._bf_MainPanel, 0);
            this._bfd_ButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel _bfd_ButtonPanel;
        private System.Windows.Forms.Label _bfd_labelDivider;
    }
}
