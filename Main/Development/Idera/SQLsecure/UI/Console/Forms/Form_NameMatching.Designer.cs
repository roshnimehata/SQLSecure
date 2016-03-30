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
            this.gradientPanel_HeaderPanel = new Idera.SQLsecure.UI.Console.Controls.GradientPanel();
            this.label_HeaderLabel2 = new System.Windows.Forms.Label();
            this.label_HeaderLabel1 = new System.Windows.Forms.Label();
            this.listBox_AvailableObjects = new System.Windows.Forms.ListBox();
            this.listBox_SelectedObjects = new System.Windows.Forms.ListBox();
            this.label_Available = new System.Windows.Forms.Label();
            this.label_Selected = new System.Windows.Forms.Label();
            this.button_AddObject = new System.Windows.Forms.Button();
            this.button_RemoveObject = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.gradientPanel_HeaderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.listBox_MatchStrings);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button_Remove);
            this.groupBox1.Controls.Add(this.textBox_MatchString);
            this.groupBox1.Controls.Add(this.button_Add);
            this.groupBox1.Controls.Add(this.radioButton_Any);
            this.groupBox1.Controls.Add(this.radioButton_Like);
            this.groupBox1.Location = new System.Drawing.Point(12, 310);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 166);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Names matching";
            // 
            // listBox_MatchStrings
            // 
            this.listBox_MatchStrings.Location = new System.Drawing.Point(72, 81);
            this.listBox_MatchStrings.Name = "listBox_MatchStrings";
            this.listBox_MatchStrings.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_MatchStrings.Size = new System.Drawing.Size(287, 69);
            this.listBox_MatchStrings.TabIndex = 8;
            this.listBox_MatchStrings.EnabledChanged += new System.EventHandler(this.listBox_MatchStrings_EnabledChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "&Match strings:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(187, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "&Enter match string (wildcards allowed):";
            // 
            // button_Remove
            // 
            this.button_Remove.Location = new System.Drawing.Point(365, 81);
            this.button_Remove.Name = "button_Remove";
            this.button_Remove.Size = new System.Drawing.Size(75, 23);
            this.button_Remove.TabIndex = 7;
            this.button_Remove.Text = "&Remove";
            this.button_Remove.UseVisualStyleBackColor = true;
            this.button_Remove.Click += new System.EventHandler(this.button_Remove_Click);
            // 
            // textBox_MatchString
            // 
            this.textBox_MatchString.Location = new System.Drawing.Point(72, 42);
            this.textBox_MatchString.Name = "textBox_MatchString";
            this.textBox_MatchString.Size = new System.Drawing.Size(287, 20);
            this.textBox_MatchString.TabIndex = 3;
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(365, 42);
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
            this.radioButton_Any.Location = new System.Drawing.Point(10, 22);
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
            this.radioButton_Like.Location = new System.Drawing.Point(10, 45);
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
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(343, 492);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(424, 492);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 11;
            this.button_Cancel.Text = "&Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // gradientPanel_HeaderPanel
            // 
            this.gradientPanel_HeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gradientPanel_HeaderPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.gradientPanel_HeaderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gradientPanel_HeaderPanel.Controls.Add(this.label_HeaderLabel2);
            this.gradientPanel_HeaderPanel.Controls.Add(this.label_HeaderLabel1);
            this.gradientPanel_HeaderPanel.GradientColor = System.Drawing.Color.Empty;
            this.gradientPanel_HeaderPanel.Location = new System.Drawing.Point(-1, -1);
            this.gradientPanel_HeaderPanel.Name = "gradientPanel_HeaderPanel";
            this.gradientPanel_HeaderPanel.Rotation = 270F;
            this.gradientPanel_HeaderPanel.Size = new System.Drawing.Size(515, 62);
            this.gradientPanel_HeaderPanel.TabIndex = 14;
            // 
            // label_HeaderLabel2
            // 
            this.label_HeaderLabel2.AutoSize = true;
            this.label_HeaderLabel2.Location = new System.Drawing.Point(12, 33);
            this.label_HeaderLabel2.Name = "label_HeaderLabel2";
            this.label_HeaderLabel2.Size = new System.Drawing.Size(252, 13);
            this.label_HeaderLabel2.TabIndex = 15;
            this.label_HeaderLabel2.Text = "Specific Objects can be selested for data collection.";
            // 
            // label_HeaderLabel1
            // 
            this.label_HeaderLabel1.AutoSize = true;
            this.label_HeaderLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_HeaderLabel1.Location = new System.Drawing.Point(12, 9);
            this.label_HeaderLabel1.Name = "label_HeaderLabel1";
            this.label_HeaderLabel1.Size = new System.Drawing.Size(90, 13);
            this.label_HeaderLabel1.TabIndex = 14;
            this.label_HeaderLabel1.Text = "Select Objects";
            // 
            // listBox_AvailableObjects
            // 
            this.listBox_AvailableObjects.FormattingEnabled = true;
            this.listBox_AvailableObjects.Location = new System.Drawing.Point(13, 93);
            this.listBox_AvailableObjects.Name = "listBox_AvailableObjects";
            this.listBox_AvailableObjects.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_AvailableObjects.Size = new System.Drawing.Size(180, 199);
            this.listBox_AvailableObjects.TabIndex = 15;
            // 
            // listBox_SelectedObjects
            // 
            this.listBox_SelectedObjects.FormattingEnabled = true;
            this.listBox_SelectedObjects.Location = new System.Drawing.Point(319, 93);
            this.listBox_SelectedObjects.Name = "listBox_SelectedObjects";
            this.listBox_SelectedObjects.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_SelectedObjects.Size = new System.Drawing.Size(180, 199);
            this.listBox_SelectedObjects.TabIndex = 16;
            // 
            // label_Available
            // 
            this.label_Available.AutoSize = true;
            this.label_Available.Location = new System.Drawing.Point(13, 73);
            this.label_Available.Name = "label_Available";
            this.label_Available.Size = new System.Drawing.Size(92, 13);
            this.label_Available.TabIndex = 17;
            this.label_Available.Text = "Available Objects:";
            // 
            // label_Selected
            // 
            this.label_Selected.AutoSize = true;
            this.label_Selected.Location = new System.Drawing.Point(320, 73);
            this.label_Selected.Name = "label_Selected";
            this.label_Selected.Size = new System.Drawing.Size(91, 13);
            this.label_Selected.TabIndex = 18;
            this.label_Selected.Text = "Selected Objects:";
            // 
            // button_AddObject
            // 
            this.button_AddObject.Location = new System.Drawing.Point(216, 145);
            this.button_AddObject.Name = "button_AddObject";
            this.button_AddObject.Size = new System.Drawing.Size(80, 23);
            this.button_AddObject.TabIndex = 19;
            this.button_AddObject.Text = "A&dd >";
            this.button_AddObject.UseVisualStyleBackColor = true;
            this.button_AddObject.Click += new System.EventHandler(this.button_AddObject_Click);
            // 
            // button_RemoveObject
            // 
            this.button_RemoveObject.Location = new System.Drawing.Point(216, 174);
            this.button_RemoveObject.Name = "button_RemoveObject";
            this.button_RemoveObject.Size = new System.Drawing.Size(80, 23);
            this.button_RemoveObject.TabIndex = 20;
            this.button_RemoveObject.Text = "< &Remove";
            this.button_RemoveObject.UseVisualStyleBackColor = true;
            this.button_RemoveObject.Click += new System.EventHandler(this.button_RemoveObject_Click);
            // 
            // Form_NameMatching
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(511, 527);
            this.Controls.Add(this.button_RemoveObject);
            this.Controls.Add(this.button_AddObject);
            this.Controls.Add(this.label_Selected);
            this.Controls.Add(this.label_Available);
            this.Controls.Add(this.listBox_SelectedObjects);
            this.Controls.Add(this.listBox_AvailableObjects);
            this.Controls.Add(this.gradientPanel_HeaderPanel);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_NameMatching";
            this.Text = "ObjectName";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gradientPanel_HeaderPanel.ResumeLayout(false);
            this.gradientPanel_HeaderPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private Controls.GradientPanel gradientPanel_HeaderPanel;
        private System.Windows.Forms.Label label_HeaderLabel2;
        private System.Windows.Forms.Label label_HeaderLabel1;
        private System.Windows.Forms.ListBox listBox_AvailableObjects;
        private System.Windows.Forms.ListBox listBox_SelectedObjects;
        private System.Windows.Forms.Label label_Available;
        private System.Windows.Forms.Label label_Selected;
        private System.Windows.Forms.Button button_AddObject;
        private System.Windows.Forms.Button button_RemoveObject;

    }
}
