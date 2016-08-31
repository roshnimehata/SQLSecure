namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class SmallTask
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
            this._st_Link = new System.Windows.Forms.LinkLabel();
            this._st_Label = new System.Windows.Forms.Label();
            this._st_Image = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this._st_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // _st_Link
            // 
            this._st_Link.AutoSize = true;
            this._st_Link.LinkColor = System.Drawing.Color.Navy;
            this._st_Link.Location = new System.Drawing.Point(40, 2);
            this._st_Link.Name = "_st_Link";
            this._st_Link.Size = new System.Drawing.Size(55, 13);
            this._st_Link.TabIndex = 1;
            this._st_Link.TabStop = true;
            this._st_Link.Text = "linkLabel1";
            this._st_Link.Click += new System.EventHandler(this._st_Link_Click);
            // 
            // _st_Label
            // 
            this._st_Label.Location = new System.Drawing.Point(40, 15);
            this._st_Label.Name = "_st_Label";
            this._st_Label.Size = new System.Drawing.Size(194, 23);
            this._st_Label.TabIndex = 2;
            this._st_Label.Visible = false;
            // 
            // _st_Image
            // 
            this._st_Image.Cursor = System.Windows.Forms.Cursors.Hand;
            this._st_Image.Location = new System.Drawing.Point(1, 1);
            this._st_Image.Name = "_st_Image";
            this._st_Image.Size = new System.Drawing.Size(32, 32);
            this._st_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._st_Image.TabIndex = 0;
            this._st_Image.TabStop = false;
            this._st_Image.Click += new System.EventHandler(this._st_Image_Click);
            // 
            // SmallTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._st_Label);
            this.Controls.Add(this._st_Link);
            this.Controls.Add(this._st_Image);
            this.Name = "SmallTask";
            this.Size = new System.Drawing.Size(234, 34);
            ((System.ComponentModel.ISupportInitialize)(this._st_Image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _st_Image;
        private System.Windows.Forms.LinkLabel _st_Link;
        private System.Windows.Forms.Label _st_Label;

    }
}
