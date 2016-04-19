namespace Idera.SQLsecure.UI.Console.Controls
{
    partial class PolicyInterview
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
            this.components = new System.ComponentModel.Container();
            this._ultraButton_EditInterview = new Infragistics.Win.Misc.UltraButton();
            this._ultraFormattedTextEditor_Interview = new Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor();
            this._ultraSpellChecker = new Infragistics.Win.UltraWinSpellChecker.UltraSpellChecker(this.components);
            this._label_Interview = new System.Windows.Forms.Label();
            this._textBox_InterviewName = new System.Windows.Forms.TextBox();
            this._label_InterviewName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._ultraSpellChecker)).BeginInit();
            this.SuspendLayout();
            // 
            // _ultraButton_EditInterview
            // 
            this._ultraButton_EditInterview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._ultraButton_EditInterview.Location = new System.Drawing.Point(5, 454);
            this._ultraButton_EditInterview.Name = "_ultraButton_EditInterview";
            this._ultraButton_EditInterview.Size = new System.Drawing.Size(108, 26);
            this._ultraButton_EditInterview.TabIndex = 2;
            this._ultraButton_EditInterview.Text = "Check &Spelling";
            this._ultraButton_EditInterview.Click += new System.EventHandler(this._ultraButton_EditInterview_Click);
            // 
            // _ultraFormattedTextEditor_Interview
            // 
            this._ultraFormattedTextEditor_Interview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._ultraFormattedTextEditor_Interview.ContextMenuItems = ((Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems)((((((((Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.Cut | Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.Copy)
                        | Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.Paste)
                        | Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.Delete)
                        | Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.Undo)
                        | Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.Redo)
                        | Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.SelectAll)
                        | Infragistics.Win.FormattedLinkLabel.FormattedTextMenuItems.SpellingSuggestions)));
            this._ultraFormattedTextEditor_Interview.Location = new System.Drawing.Point(5, 128);
            this._ultraFormattedTextEditor_Interview.Name = "_ultraFormattedTextEditor_Interview";
            this._ultraFormattedTextEditor_Interview.Size = new System.Drawing.Size(613, 318);
            this._ultraFormattedTextEditor_Interview.SpellChecker = this._ultraSpellChecker;
            this._ultraFormattedTextEditor_Interview.TabIndex = 1;
            this._ultraFormattedTextEditor_Interview.UseAppStyling = false;
            this._ultraFormattedTextEditor_Interview.Value = "<Enter your text here>";
            this._ultraFormattedTextEditor_Interview.TextChanged += new System.EventHandler(this._ultraFormattedTextEditor_Interview_TextChanged);
            this._ultraFormattedTextEditor_Interview.Leave += new System.EventHandler(this._ultraFormattedTextEditor_Interview_Leave);
            this._ultraFormattedTextEditor_Interview.Enter += new System.EventHandler(this._ultraFormattedTextEditor_Interview_Enter);
            this._ultraFormattedTextEditor_Interview.KeyDown += new System.Windows.Forms.KeyEventHandler(this._ultraFormattedTextEditor_Interview_KeyDown);
            // 
            // _ultraSpellChecker
            // 
            this._ultraSpellChecker.ContainingControl = this;
            this._ultraSpellChecker.Mode = Infragistics.Win.UltraWinSpellChecker.SpellCheckingMode.AsYouType;
            // 
            // _label_Interview
            // 
            this._label_Interview.Dock = System.Windows.Forms.DockStyle.Top;
            this._label_Interview.Location = new System.Drawing.Point(6, 6);
            this._label_Interview.Name = "_label_Interview";
            this._label_Interview.Size = new System.Drawing.Size(613, 55);
            this._label_Interview.TabIndex = 0;
            this._label_Interview.Text = "Text can be added to your security assessment report to enable manually gathering" +
                " data and reporting it in one comprehensive place. Enter an optional title and a" +
                "dditional text for your report here.";
            // 
            // _textBox_InterviewName
            // 
            this._textBox_InterviewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox_InterviewName.Location = new System.Drawing.Point(5, 89);
            this._textBox_InterviewName.MaxLength = 256;
            this._textBox_InterviewName.Name = "_textBox_InterviewName";
            this._textBox_InterviewName.Size = new System.Drawing.Size(613, 20);
            this._textBox_InterviewName.TabIndex = 0;
            this._textBox_InterviewName.Leave += new System.EventHandler(this._textBox_InterviewName_Leave);
            this._textBox_InterviewName.Enter += new System.EventHandler(this._textBox_InterviewName_Enter);
            // 
            // _label_InterviewName
            // 
            this._label_InterviewName.AutoSize = true;
            this._label_InterviewName.Location = new System.Drawing.Point(4, 73);
            this._label_InterviewName.Name = "_label_InterviewName";
            this._label_InterviewName.Size = new System.Drawing.Size(27, 13);
            this._label_InterviewName.TabIndex = 4;
            this._label_InterviewName.Text = "Title";
            // 
            // PolicyInterview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this._label_InterviewName);
            this.Controls.Add(this._textBox_InterviewName);
            this.Controls.Add(this._ultraButton_EditInterview);
            this.Controls.Add(this._ultraFormattedTextEditor_Interview);
            this.Controls.Add(this._label_Interview);
            this.Name = "PolicyInterview";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(625, 487);
            ((System.ComponentModel.ISupportInitialize)(this._ultraSpellChecker)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton _ultraButton_EditInterview;
        private Infragistics.Win.FormattedLinkLabel.UltraFormattedTextEditor _ultraFormattedTextEditor_Interview;
        private System.Windows.Forms.Label _label_Interview;
        private Infragistics.Win.UltraWinSpellChecker.UltraSpellChecker _ultraSpellChecker;
        private System.Windows.Forms.Label _label_InterviewName;
        private System.Windows.Forms.TextBox _textBox_InterviewName;
    }
}
