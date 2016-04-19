using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_Welcome : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        public Form_Welcome()
        {
            InitializeComponent();

            richTextBox1.Rtf =
                @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fprq2\fcharset0 Arial;}{\f1\fswiss\fcharset0 Arial;}}
\viewkind4\uc1\pard\f0\fs18 SQLsecure collects and evaluates key security settings from SQL Server and Active Directory and then provides proactive recommendations on how to improve your server security. Additionally, SQLsecure provides analysis and reporting on effective user permissions as well as access rights for SQL Server objects and related Windows components across your enterprise.\par
\par
\f0\fs20
\b To get started:\par
\b0\f0\fs8\par
\f0\fs20
\b 1. \b0 Register your SQL Server instances\par
\b 2. \b0 Take snapshots to collect audit data\par
\b 3. \b0 Create policies to set up security checks\par
\b 4. \b0 Use Report Cards to analyze your security status\par
\f1\par
}";
        }

        static public void Process()
        {
            Forms.Form_Welcome formWelcome = new Form_Welcome();
            if (formWelcome.ShowDialog() == DialogResult.Yes)
            {
                Form_WizardRegisterSQLServer.Process();
            }
        }

        private void button_Yes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void button_No_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}

