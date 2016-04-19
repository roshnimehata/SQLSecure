namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class BasePropertyForm
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
            this._bpf_CloseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _bf_MainPanel
            // 
            this._bf_MainPanel.Size = new System.Drawing.Size(569, 388);
            // 
            // _bf_HeaderPanel
            // 
            this._bf_HeaderPanel.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._bf_HeaderPanel.Size = new System.Drawing.Size(569, 53);
            // 
            // _bpf_CloseBtn
            // 
            this._bpf_CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._bpf_CloseBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._bpf_CloseBtn.Location = new System.Drawing.Point(482, 16);
            this._bpf_CloseBtn.Name = "_bpf_CloseBtn";
            this._bpf_CloseBtn.Size = new System.Drawing.Size(75, 23);
            this._bpf_CloseBtn.TabIndex = 0;
            this._bpf_CloseBtn.Text = "Close";
            this._bpf_CloseBtn.UseVisualStyleBackColor = true;
            // 
            // BasePropertyForm
            // 
            this.AcceptButton = this._bpf_CloseBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._bpf_CloseBtn;
            this.ClientSize = new System.Drawing.Size(569, 441);
            this.ControlBox = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BasePropertyForm";
            this.Text = "BasePropertyForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _bpf_CloseBtn;
    }
}
