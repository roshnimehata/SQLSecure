namespace Idera.SQLsecure.UI.Console.Forms
{
   partial class Form_PasswordList
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose( bool disposing )
      {
         if ( disposing && (components != null) )
         {
            components.Dispose();
         }
         base.Dispose( disposing );
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_PasswordList));
          this.labelPasswordTitle = new System.Windows.Forms.Label();
          this.listPasswords = new System.Windows.Forms.ListView();
          this.columnPassword = new System.Windows.Forms.ColumnHeader();
          this.buttonClose = new System.Windows.Forms.Button();
          this.labelPasswordCount = new System.Windows.Forms.Label();
          this.SuspendLayout();
          // 
          // labelPasswordTitle
          // 
          this.labelPasswordTitle.AutoSize = true;
          this.labelPasswordTitle.Location = new System.Drawing.Point(13, 14);
          this.labelPasswordTitle.Name = "labelPasswordTitle";
          this.labelPasswordTitle.Size = new System.Drawing.Size(75, 13);
          this.labelPasswordTitle.TabIndex = 0;
          this.labelPasswordTitle.Text = "Password List:";
          // 
          // listPasswords
          // 
          this.listPasswords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                      | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.listPasswords.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPassword});
          this.listPasswords.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
          this.listPasswords.Location = new System.Drawing.Point(13, 41);
          this.listPasswords.Name = "listPasswords";
          this.listPasswords.Size = new System.Drawing.Size(366, 458);
          this.listPasswords.TabIndex = 2;
          this.listPasswords.UseCompatibleStateImageBehavior = false;
          this.listPasswords.View = System.Windows.Forms.View.Details;
          // 
          // columnPassword
          // 
          this.columnPassword.Text = "Password";
          this.columnPassword.Width = 346;
          // 
          // buttonClose
          // 
          this.buttonClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
          this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
          this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
          this.buttonClose.Location = new System.Drawing.Point(312, 507);
          this.buttonClose.Name = "buttonClose";
          this.buttonClose.Size = new System.Drawing.Size(66, 20);
          this.buttonClose.TabIndex = 3;
          this.buttonClose.Text = "&Close";
          // 
          // labelPasswordCount
          // 
          this.labelPasswordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.labelPasswordCount.AutoSize = true;
          this.labelPasswordCount.Location = new System.Drawing.Point(13, 510);
          this.labelPasswordCount.Name = "labelPasswordCount";
          this.labelPasswordCount.Size = new System.Drawing.Size(75, 13);
          this.labelPasswordCount.TabIndex = 4;
          this.labelPasswordCount.Text = "Password List:";
          // 
          // Form_PasswordList
          // 
          this.AcceptButton = this.buttonClose;
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.White;
          this.CancelButton = this.buttonClose;
          this.ClientSize = new System.Drawing.Size(390, 534);
          this.Controls.Add(this.labelPasswordCount);
          this.Controls.Add(this.buttonClose);
          this.Controls.Add(this.listPasswords);
          this.Controls.Add(this.labelPasswordTitle);
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.Name = "Form_PasswordList";
          this.ShowInTaskbar = false;
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.Text = "View Password List";
          this.ResumeLayout(false);
          this.PerformLayout();

      }

      #endregion

       private System.Windows.Forms.Label labelPasswordTitle;
      private System.Windows.Forms.ListView listPasswords;
      private System.Windows.Forms.Button buttonClose;
      private System.Windows.Forms.Label labelPasswordCount;
      private System.Windows.Forms.ColumnHeader columnPassword;
   }
}