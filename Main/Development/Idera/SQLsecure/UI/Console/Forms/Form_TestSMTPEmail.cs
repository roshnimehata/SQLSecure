using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_TestSMTPEmail : Form
    {
        public Form_TestSMTPEmail()
        {
            InitializeComponent();
        }

        public string Recepient
        {
            get { return _tbRecepient.Text; }
            set { _tbRecepient.Text = value; }
        }
    }
}