namespace Idera.SQLsecure.Controls
{
    partial class CommonTask
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
            this.gradientPanel1 = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._pictureBox_TaskImage = new System.Windows.Forms.PictureBox();
            this._label_Title = new System.Windows.Forms.Label();
            this._label_Description = new System.Windows.Forms.Label();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_TaskImage)).BeginInit();
            this.SuspendLayout();
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.gradientPanel1.Controls.Add(this._pictureBox_TaskImage);
            this.gradientPanel1.Controls.Add(this._label_Title);
            this.gradientPanel1.Controls.Add(this._label_Description);
            this.gradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel1.GradientBorderMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DIn;
            this.gradientPanel1.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.gradientPanel1.GradientCornerMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundCorners;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.gradientPanel1.Rotation = 90F;
            this.gradientPanel1.Size = new System.Drawing.Size(241, 73);
            this.gradientPanel1.TabIndex = 12;
            this.gradientPanel1.MouseLeave += new System.EventHandler(this.gradientPanel1_MouseLeave);
            this.gradientPanel1.Click += new System.EventHandler(this.gradientPanel1_Click);
            this.gradientPanel1.MouseEnter += new System.EventHandler(this.gradientPanel1_MouseEnter);
            // 
            // _pictureBox_TaskImage
            // 
            this._pictureBox_TaskImage.BackColor = System.Drawing.Color.Transparent;
            this._pictureBox_TaskImage.Dock = System.Windows.Forms.DockStyle.Left;
            this._pictureBox_TaskImage.ErrorImage = null;
            this._pictureBox_TaskImage.InitialImage = null;
            this._pictureBox_TaskImage.Location = new System.Drawing.Point(2, 0);
            this._pictureBox_TaskImage.Margin = new System.Windows.Forms.Padding(0);
            this._pictureBox_TaskImage.Name = "_pictureBox_TaskImage";
            this._pictureBox_TaskImage.Size = new System.Drawing.Size(50, 73);
            this._pictureBox_TaskImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._pictureBox_TaskImage.TabIndex = 10;
            this._pictureBox_TaskImage.TabStop = false;
            this._pictureBox_TaskImage.MouseLeave += new System.EventHandler(this._pictureBox_TaskImage_MouseLeave);
            this._pictureBox_TaskImage.Click += new System.EventHandler(this._pictureBox_TaskImage_Click);
            this._pictureBox_TaskImage.MouseEnter += new System.EventHandler(this._pictureBox_TaskImage_MouseEnter);
            // 
            // _label_Title
            // 
            this._label_Title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_Title.AutoEllipsis = true;
            this._label_Title.BackColor = System.Drawing.Color.Transparent;
            this._label_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this._label_Title.Location = new System.Drawing.Point(54, 5);
            this._label_Title.Name = "_label_Title";
            this._label_Title.Size = new System.Drawing.Size(184, 13);
            this._label_Title.TabIndex = 13;
            this._label_Title.Text = "Task Title";
            this._label_Title.MouseLeave += new System.EventHandler(this._label_Title_MouseLeave);
            this._label_Title.Click += new System.EventHandler(this._label_Title_Click);
            this._label_Title.MouseEnter += new System.EventHandler(this._label_Title_MouseEnter);
            // 
            // _label_Description
            // 
            this._label_Description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_Description.BackColor = System.Drawing.Color.Transparent;
            this._label_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._label_Description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(95)))), ((int)(((byte)(95)))));
            this._label_Description.Location = new System.Drawing.Point(54, 18);
            this._label_Description.Name = "_label_Description";
            this._label_Description.Size = new System.Drawing.Size(184, 55);
            this._label_Description.TabIndex = 11;
            this._label_Description.Text = "Task Description";
            this._label_Description.MouseLeave += new System.EventHandler(this._label_Description_MouseLeave);
            this._label_Description.Click += new System.EventHandler(this._label_Description_Click);
            this._label_Description.MouseEnter += new System.EventHandler(this._label_Description_MouseEnter);
            // 
            // CommonTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gradientPanel1);
            this.DoubleBuffered = true;
            this.Name = "CommonTask";
            this.Size = new System.Drawing.Size(241, 73);
            this.EnabledChanged += new System.EventHandler(this.CommonTask_EnabledChanged);
            this.gradientPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox_TaskImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _label_Description;
        private System.Windows.Forms.PictureBox _pictureBox_TaskImage;
        private Idera.SQLsecure.UI.Console.Controls.GradientPanel gradientPanel1;
        private System.Windows.Forms.Label _label_Title;
    }
}
