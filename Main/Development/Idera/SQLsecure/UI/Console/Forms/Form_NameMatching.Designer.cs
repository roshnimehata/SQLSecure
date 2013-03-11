namespace Idera.SQLsecure.UI.Console.Forms
{
    partial class Form_NameMatching
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox_MatchStrings = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_Remove = new System.Windows.Forms.Button();
            this.textBox_MatchString = new System.Windows.Forms.TextBox();
            this.button_Add = new System.Windows.Forms.Button();
            this.radioButton_Any = new System.Windows.Forms.RadioButton();
            this.radioButton_Like = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox_MatchStrings);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button_Remove);
            this.groupBox1.Controls.Add(this.textBox_MatchString);
            this.groupBox1.Controls.Add(this.button_Add);
            this.groupBox1.Controls.Add(this.radioButton_Any);
            this.groupBox1.Controls.Add(this.radioButton_Like);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 193);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Names matching";
            // 
            // listBox_MatchStrings
            // 
            this.listBox_MatchStrings.Location = new System.Drawing.Point(60, 100);
            this.listBox_MatchStrings.Name = "listBox_MatchStrings";
            this.listBox_MatchStrings.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_MatchStrings.Size = new System.Drawing.Size(217, 82);
            this.listBox_MatchStrings.TabIndex = 8;
            this.listBox_MatchStrings.EnabledChanged += new System.EventHandler(this.listBox_MatchStrings_EnabledChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Match strings:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(187, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Enter match string (wildcards allowed):";
            // 
            // button_Remove
            // 
            this.button_Remove.Location = new System.Drawing.Point(283, 100);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(75, 23);
            this.button_Remove.TabIndex = 7;
            this.button_Remove.Text = "&Remove";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
            // 
            // textBox_MatchString
            // 
            this.textBox_MatchString.Location = new System.Drawing.Point(60, 61);
            this.textBox_MatchString.Name = "textBox_MatchString";
            this.textBox_MatchString.Size = new System.Drawing.Size(217, 20);
            this.textBox_MatchString.TabIndex = 3;
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(283, 61);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(75, 23);
            this.button_Add.TabIndex = 4;
            this.button_Add.Text = "A&dd";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // radioButton_Any
            // 
            this.radioButton_Any.AutoSize = true;
            this.radioButton_Any.Location = new System.Drawing.Point(9, 19);
            this.radioButton_Any.Name = "radioButton_Any";
            this.radioButton_Any.Size = new System.Drawing.Size(43, 17);
            this.radioButton_Any.TabIndex = 0;
            this.radioButton_Any.Text = "&Any";
            this.radioButton_Any.UseVisualStyleBackColor = true;
            this.radioButton_Any.CheckedChanged += new System.EventHandler(this.radioButton_Any_CheckedChanged);
            // 
            // radioButton_Like
            // 
            this.radioButton_Like.AutoSize = true;
            this.radioButton_Like.Checked = true;
            this.radioButton_Like.Location = new System.Drawing.Point(60, 19);
            this.radioButton_Like.Name = "radioButton_Like";
            this.radioButton_Like.Size = new System.Drawing.Size(45, 17);
            this.radioButton_Like.TabIndex = 1;
            this.radioButton_Like.TabStop = true;
            this.radioButton_Like.Text = "&Like";
            this.radioButton_Like.UseVisualStyleBackColor = true;
            this.radioButton_Like.CheckedChanged += new System.EventHandler(this.radioButton_Like_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(227, 222);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(308, 222);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 11;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // Form_NameMatching
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(395, 256);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_NameMatching";
            this.Text = "ObjectName";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_Remove;
        private System.Windows.Forms.TextBox textBox_MatchString;
        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.RadioButton radioButton_Any;
        private System.Windows.Forms.RadioButton radioButton_Like;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.ListBox listBox_MatchStrings;

    }
}
