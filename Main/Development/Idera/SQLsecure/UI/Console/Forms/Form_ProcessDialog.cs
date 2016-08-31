using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_ProcessDialog : Form
    {
        public Form_ProcessDialog()
        {
            InitializeComponent();
        }

        public Form_ProcessDialog(string title, string message, EventHandler onCancel, Image dialogImage) : this()
        {
            Title = title;
            Message = message;
            OnCancel = onCancel;
            pbDialogIcon.Image = dialogImage;
            
        }
        public event EventHandler OnCancel;
        public string Title
        {
            get { return Text; }
            set { Text = value; }
        }

        public string Message
        {
            get { return lbMessage.Text; }
            set { lbMessage.Text = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Text = "Canceling...";
            btnCancel.Enabled = false;
            if (OnCancel != null) OnCancel(sender, e);
            DialogResult = DialogResult.Cancel;
        }



       
    }
}
