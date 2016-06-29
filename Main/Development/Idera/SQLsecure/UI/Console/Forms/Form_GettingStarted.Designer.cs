namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_GettingStarted
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_GettingStarted));
            this._panel_Header = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.controlGettingStarted1 = new Idera.SQLsecure.UI.Console.Controls.ControlGettingStarted();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraButtonClose = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_Next = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_Back = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton_Help = new Infragistics.Win.Misc.UltraButton();
            this.gradientPanelDescription = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this.label_GettingStarted = new System.Windows.Forms.Label();
            this._panel_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.gradientPanelDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel_Header
            // 
            this._panel_Header.BackgroundImage = global::Idera.SQLsecure.UI.Console.Properties.Resources.UI_header;
            this._panel_Header.Controls.Add(this.pictureBox1);
            this._panel_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this._panel_Header.Location = new System.Drawing.Point(0, 0);
            this._panel_Header.Name = "_panel_Header";
            this._panel_Header.Size = new System.Drawing.Size(757, 50);
            this._panel_Header.TabIndex = 8;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Idera.SQLsecure.UI.Console.Properties.Resources.UI_header_tagline;
            this.pictureBox1.Location = new System.Drawing.Point(420, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(337, 50);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // controlGettingStarted1
            // 
            this.controlGettingStarted1.BackColor = System.Drawing.Color.Transparent;
            this.controlGettingStarted1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlGettingStarted1.Location = new System.Drawing.Point(0, 113);
            this.controlGettingStarted1.Name = "controlGettingStarted1";
            this.controlGettingStarted1.Size = new System.Drawing.Size(757, 330);
            this.controlGettingStarted1.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.ultraButtonClose);
            this.panel1.Controls.Add(this.ultraButton_Next);
            this.panel1.Controls.Add(this.ultraButton_Back);
            this.panel1.Controls.Add(this.ultraButton_Help);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 443);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(757, 40);
            this.panel1.TabIndex = 10;
            // 
            // ultraButtonClose
            // 
            this.ultraButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButtonClose.Location = new System.Drawing.Point(573, 9);
            this.ultraButtonClose.Name = "ultraButtonClose";
            this.ultraButtonClose.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonClose.TabIndex = 3;
            this.ultraButtonClose.Text = "&Close";
            // 
            // ultraButton_Next
            // 
            this.ultraButton_Next.Location = new System.Drawing.Point(491, 9);
            this.ultraButton_Next.Name = "ultraButton_Next";
            this.ultraButton_Next.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Next.TabIndex = 2;
            this.ultraButton_Next.Text = "&Next >>";
            this.ultraButton_Next.Click += new System.EventHandler(this.ultraButton_Next_Click);
            // 
            // ultraButton_Back
            // 
            this.ultraButton_Back.Location = new System.Drawing.Point(409, 9);
            this.ultraButton_Back.Name = "ultraButton_Back";
            this.ultraButton_Back.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Back.TabIndex = 1;
            this.ultraButton_Back.Text = "<< &Back";
            this.ultraButton_Back.Click += new System.EventHandler(this.ultraButton_Back_Click);
            // 
            // ultraButton_Help
            // 
            this.ultraButton_Help.Location = new System.Drawing.Point(655, 9);
            this.ultraButton_Help.Name = "ultraButton_Help";
            this.ultraButton_Help.Size = new System.Drawing.Size(75, 23);
            this.ultraButton_Help.TabIndex = 0;
            this.ultraButton_Help.Text = "&Help";
            this.ultraButton_Help.Click += new System.EventHandler(this.ultraButton_Help_Click);
            // 
            // gradientPanelDescription
            // 
            this.gradientPanelDescription.BackColor = System.Drawing.Color.White;
            this.gradientPanelDescription.Controls.Add(this.label_GettingStarted);
            this.gradientPanelDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.gradientPanelDescription.GradientColor = System.Drawing.Color.WhiteSmoke;
            this.gradientPanelDescription.Location = new System.Drawing.Point(0, 50);
            this.gradientPanelDescription.Name = "gradientPanelDescription";
            this.gradientPanelDescription.Rotation = 270F;
            this.gradientPanelDescription.Size = new System.Drawing.Size(757, 63);
            this.gradientPanelDescription.TabIndex = 11;
            // 
            // label_GettingStarted
            // 
            this.label_GettingStarted.BackColor = System.Drawing.Color.Transparent;
            this.label_GettingStarted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_GettingStarted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label_GettingStarted.ForeColor = System.Drawing.Color.Navy;
            this.label_GettingStarted.Location = new System.Drawing.Point(0, 0);
            this.label_GettingStarted.Name = "label_GettingStarted";
            this.label_GettingStarted.Size = new System.Drawing.Size(757, 63);
            this.label_GettingStarted.TabIndex = 1;
            this.label_GettingStarted.Text = resources.GetString("label_GettingStarted.Text");
            this.label_GettingStarted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Form_GettingStarted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(757, 483);
            this.Controls.Add(this.controlGettingStarted1);
            this.Controls.Add(this.gradientPanelDescription);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._panel_Header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "Form_GettingStarted";
            this.Text = "Getting Started";
            this._panel_Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.gradientPanelDescription.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel _panel_Header;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Idera.SQLsecure.UI.Console.Controls.ControlGettingStarted controlGettingStarted1;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton ultraButton_Next;
        private Infragistics.Win.Misc.UltraButton ultraButton_Back;
        private Infragistics.Win.Misc.UltraButton ultraButton_Help;
        private Infragistics.Win.Misc.UltraButton ultraButtonClose;
        private Idera.SQLsecure.UI.Console.Controls.GradientPanel gradientPanelDescription;
        private System.Windows.Forms.Label label_GettingStarted;

    }
}
