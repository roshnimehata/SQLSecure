namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_ConnectionProperties
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
            this._button_OK = new System.Windows.Forms.Button();
            this._groupBox_Properties = new System.Windows.Forms.GroupBox();
            this._listView_Properties = new System.Windows.Forms.ListView();
            this.col1 = new System.Windows.Forms.ColumnHeader();
            this.col2 = new System.Windows.Forms.ColumnHeader();
            this._label_Properties = new System.Windows.Forms.Label();
            this._button_Help = new System.Windows.Forms.Button();
            this._groupBox_Properties.SuspendLayout();
            this.SuspendLayout();
            // 
            // _button_OK
            // 
            this._button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._button_OK.Location = new System.Drawing.Point(226, 233);
            this._button_OK.Name = "_button_OK";
            this._button_OK.Size = new System.Drawing.Size(75, 23);
            this._button_OK.TabIndex = 3;
            this._button_OK.Text = "&Close";
            this._button_OK.UseVisualStyleBackColor = true;
            // 
            // _groupBox_Properties
            // 
            this._groupBox_Properties.Controls.Add(this._listView_Properties);
            this._groupBox_Properties.Controls.Add(this._label_Properties);
            this._groupBox_Properties.Location = new System.Drawing.Point(12, 12);
            this._groupBox_Properties.Name = "_groupBox_Properties";
            this._groupBox_Properties.Size = new System.Drawing.Size(370, 203);
            this._groupBox_Properties.TabIndex = 0;
            this._groupBox_Properties.TabStop = false;
            this._groupBox_Properties.Text = "Repository";
            // 
            // _listView_Properties
            // 
            this._listView_Properties.BackColor = System.Drawing.SystemColors.Window;
            this._listView_Properties.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._listView_Properties.CausesValidation = false;
            this._listView_Properties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col1,
            this.col2});
            this._listView_Properties.Enabled = false;
            this._listView_Properties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._listView_Properties.Location = new System.Drawing.Point(21, 55);
            this._listView_Properties.Name = "_listView_Properties";
            this._listView_Properties.Scrollable = false;
            this._listView_Properties.ShowGroups = false;
            this._listView_Properties.Size = new System.Drawing.Size(334, 126);
            this._listView_Properties.TabIndex = 2;
            this._listView_Properties.TabStop = false;
            this._listView_Properties.UseCompatibleStateImageBehavior = false;
            this._listView_Properties.View = System.Windows.Forms.View.Details;
            // 
            // col1
            // 
            this.col1.Width = 130;
            // 
            // col2
            // 
            this.col2.Width = 200;
            // 
            // _label_Properties
            // 
            this._label_Properties.Location = new System.Drawing.Point(18, 28);
            this._label_Properties.Name = "_label_Properties";
            this._label_Properties.Size = new System.Drawing.Size(337, 13);
            this._label_Properties.TabIndex = 0;
            this._label_Properties.Text = "_label_Properties";
            // 
            // _button_Help
            // 
            this._button_Help.Location = new System.Drawing.Point(307, 233);
            this._button_Help.Name = "_button_Help";
            this._button_Help.Size = new System.Drawing.Size(75, 23);
            this._button_Help.TabIndex = 4;
            this._button_Help.Text = "&Help";
            this._button_Help.UseVisualStyleBackColor = true;
            this._button_Help.Click += new System.EventHandler(this._button_Help_Click);
            // 
            // Form_ConnectionProperties
            // 
            this.AcceptButton = this._button_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this._button_OK;
            this.ClientSize = new System.Drawing.Size(394, 268);
            this.Controls.Add(this._button_Help);
            this.Controls.Add(this._groupBox_Properties);
            this.Controls.Add(this._button_OK);
            this.Name = "Form_ConnectionProperties";
            this.Text = "Connection Properties";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.Form_ConnectionProperties_HelpRequested);
            this._groupBox_Properties.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _button_OK;
        private System.Windows.Forms.GroupBox _groupBox_Properties;
        private System.Windows.Forms.Label _label_Properties;
        private System.Windows.Forms.ListView _listView_Properties;
        private System.Windows.Forms.ColumnHeader col1;
        private System.Windows.Forms.ColumnHeader col2;
        private System.Windows.Forms.Button _button_Help;
    }
}
