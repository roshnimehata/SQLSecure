namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class ViewSection
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
            this._gradientPanel_Body = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._gradientPanel_Header = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this._label_Title = new System.Windows.Forms.Label();
            this._gradientPanel_Header.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gradientPanel_Body
            // 
            this._gradientPanel_Body.BackColor = System.Drawing.Color.Transparent;
            this._gradientPanel_Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gradientPanel_Body.GradientColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this._gradientPanel_Body.Location = new System.Drawing.Point(0, 20);
            this._gradientPanel_Body.Name = "_gradientPanel_Body";
            this._gradientPanel_Body.Rotation = 270F;
            this._gradientPanel_Body.Size = new System.Drawing.Size(652, 230);
            this._gradientPanel_Body.TabIndex = 2;
            // 
            // _gradientPanel_Header
            // 
            this._gradientPanel_Header.BackColor = System.Drawing.Color.White;
            this._gradientPanel_Header.Controls.Add(this._label_Title);
            this._gradientPanel_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this._gradientPanel_Header.GradientBorderMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientBorderStyle.Fixed3DOut;
            this._gradientPanel_Header.GradientColor = System.Drawing.Color.DarkGray;
            this._gradientPanel_Header.GradientCornerMode = Idera.SQLsecure.UI.Console.Controls.GradientPanel.GradientCornerStyle.RoundTop;
            this._gradientPanel_Header.Location = new System.Drawing.Point(0, 0);
            this._gradientPanel_Header.Name = "_gradientPanel_Header";
            this._gradientPanel_Header.Rotation = 90F;
            this._gradientPanel_Header.Size = new System.Drawing.Size(652, 20);
            this._gradientPanel_Header.TabIndex = 0;
            // 
            // _label_Title
            // 
            this._label_Title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label_Title.AutoEllipsis = true;
            this._label_Title.BackColor = System.Drawing.Color.Transparent;
            this._label_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this._label_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this._label_Title.Location = new System.Drawing.Point(5, 1);
            this._label_Title.Name = "_label_Title";
            this._label_Title.Size = new System.Drawing.Size(644, 17);
            this._label_Title.TabIndex = 1;
            this._label_Title.Text = "Section Title";
            // 
            // ViewSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._gradientPanel_Body);
            this.Controls.Add(this._gradientPanel_Header);
            this.DoubleBuffered = true;
            this.Name = "ViewSection";
            this.Size = new System.Drawing.Size(652, 250);
            this._gradientPanel_Header.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public GradientPanel _gradientPanel_Body;
        private GradientPanel _gradientPanel_Header;
        private System.Windows.Forms.Label _label_Title;



    }
}
