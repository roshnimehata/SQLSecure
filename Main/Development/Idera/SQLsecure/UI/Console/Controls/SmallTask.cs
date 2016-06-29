using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Controls
{
    public partial class SmallTask : UserControl
    {
        #region Task Event Handler

        private event EventHandler m_TaskHandler;

        private void _st_Image_Click(object sender, EventArgs e)
        {
            if (m_TaskHandler != null) { m_TaskHandler(sender, e); }
        }

        private void _st_Link_Click(object sender, EventArgs e)
        {
            if (m_TaskHandler != null) { m_TaskHandler(sender, e); }
        }
        #endregion

        #region Ctors
        public SmallTask()
        {
            InitializeComponent();

            _st_Label.Text = @"";
        }
        #endregion

        #region Properties

        public String TaskText
        {
            get { return _st_Link.Text; }
            set { _st_Link.Text = value; }
        }

        public Image TaskImage
        {
            get { return _st_Image.Image; }
            set { _st_Image.Image = value; }
        }

        public String TaskDescription
        {
            get { return _st_Label.Text; }
            set 
            { 
                _st_Label.Text = value;
                if (_st_Label.Text.Length > 0)
                {
                    _st_Label.Visible = true;
                    this.Height = 34;
                }
                else
                {
                    _st_Label.Visible = false;
                    this.Height = 34;
                }
            }
        }

        public event EventHandler TaskHandler
        {
            add { m_TaskHandler += value; }
            remove { m_TaskHandler -= value; }
        }

        #endregion
    }
}
