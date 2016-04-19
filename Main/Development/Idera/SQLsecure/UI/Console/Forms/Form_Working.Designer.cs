namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_Working
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
            this.label_text = new System.Windows.Forms.Label();
            this.pictureBox_Step6 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step6)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_text
            // 
            this.label_text.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_text.Location = new System.Drawing.Point(0, 0);
            this.label_text.Name = "label_text";
            this.label_text.Size = new System.Drawing.Size(323, 35);
            this.label_text.TabIndex = 0;
            this.label_text.Text = "Working...";
            this.label_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Step6
            // 
            this.pictureBox_Step6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Step6.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.Working;
            this.pictureBox_Step6.Location = new System.Drawing.Point(139, 48);
            this.pictureBox_Step6.Name = "pictureBox_Step6";
            this.pictureBox_Step6.Size = new System.Drawing.Size(44, 42);
            this.pictureBox_Step6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Step6.TabIndex = 15;
            this.pictureBox_Step6.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.pictureBox_Step6);
            this.panel1.Controls.Add(this.label_text);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(323, 104);
            this.panel1.TabIndex = 16;
            // 
            // Form_Working
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ClientSize = new System.Drawing.Size(329, 110);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.Navy;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Working";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form_Working_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Step6)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_text;
        private System.Windows.Forms.PictureBox pictureBox_Step6;
        private System.Windows.Forms.Panel panel1;
    }
}