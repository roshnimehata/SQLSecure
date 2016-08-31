using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        public string Description
        {
            get { return _bf_label_Description.Text;  }
            set { _bf_label_Description.Text = value; }
        }

        public Image Picture
        {
            get { return _bf_pictureBox.Image; }
            set 
            {
                if (value == null)
                {
                    _bf_pictureBox.Visible = false;
                }
                else
                {
                    _bf_pictureBox.Visible = true;
                    _bf_pictureBox.Image = value;
                }
            }
        }
    }
}