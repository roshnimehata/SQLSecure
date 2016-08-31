using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_Credentials : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        public Form_Credentials()
        {
            InitializeComponent();
        }
        public Form_Credentials(string Login, string Password)
        {
            InitializeComponent();

            textBox_Login.Text = Login;
            textBox_Password.Text = Password;
        }

        public string Login
        {
            get { return textBox_Login.Text; }
        }

        public string Password
        {
            get { return textBox_Password.Text; }
        }
    }
}

