using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Idera.SQLsecure.UI.Console.Controls;

namespace Idera.SQLsecure.Controls
{
    public partial class CommonTask : UserControl
    {
        #region Task Event Handler

        private event EventHandler m_TaskHandler;      

        #endregion

        #region Fields

        private Image m_HoverImage = null;
        private Image m_TempImage = null;

        #endregion

        #region Ctors

        public CommonTask()
        {
            InitializeComponent();
            gradientPanel1.GradientBorderMode = GradientPanel.GradientBorderStyle.Fixed3DOut;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The title of the task
        /// </summary>
        public String TaskText
        {
            get { return _label_Title.Text; }
            set { _label_Title.Text = value; }
        }

        /// <summary>
        /// The image to display when the button is not hilighted via mouse over
        /// </summary>
        public Image TaskImage
        {
            get { return _pictureBox_TaskImage.Image; }
            set { _pictureBox_TaskImage.Image = value; }
        }

        /// <summary>
        /// The image to display when the button is hilighted via mouse over
        /// </summary>
        public Image HoverTaskImage
        {
            get { return m_HoverImage; }
            set { m_HoverImage = value; }
        }

        /// <summary>
        /// An additional description of the task 
        /// </summary>
        public String TaskDescription
        {
            get { return _label_Description.Text; }
            set { _label_Description.Text = value; }
        }

        /// <summary>
        /// The event to fire when the task is clicked
        /// </summary>
        public event EventHandler TaskHandler
        {
            add { m_TaskHandler += value; }
            remove { m_TaskHandler -= value; }
        }

        #endregion

        #region Events

        private void _pictureBox_TaskImage_MouseEnter(object sender, EventArgs e)
        {
            GenericMouseEnter();
        }

        private void _label_Description_MouseEnter(object sender, EventArgs e)
        {
            GenericMouseEnter();
        }

        private void _label_Title_MouseEnter(object sender, EventArgs e)
        {
            GenericMouseEnter();
        }

        private void gradientPanel1_MouseEnter(object sender, EventArgs e)
        {
            GenericMouseEnter();
        }

        private void _pictureBox_TaskImage_MouseLeave(object sender, EventArgs e)
        {
            GenericMouseLeave();
        }

        private void _label_Description_MouseLeave(object sender, EventArgs e)
        {
            GenericMouseLeave();
        }

        private void _label_Title_MouseLeave(object sender, EventArgs e)
        {
            GenericMouseLeave();
        }

        private void gradientPanel1_MouseLeave(object sender, EventArgs e)
        {
            GenericMouseLeave();
        }

        private void gradientPanel1_Click(object sender, EventArgs e)
        {
            GenericMouseClick(sender, e);
        }

        private void _label_Description_Click(object sender, EventArgs e)
        {
            GenericMouseClick(sender, e);
        }

        private void _pictureBox_TaskImage_Click(object sender, EventArgs e)
        {
            GenericMouseClick(sender, e);
        }

        private void _label_Title_Click(object sender, EventArgs e)
        {
            GenericMouseClick(sender, e);
        }

        private void CommonTask_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                if (Cursor == Cursors.Hand)
                {
                    gradientPanel1.GradientColor = Color.NavajoWhite;
                }
                else
                {
                    gradientPanel1.GradientColor = Color.FromArgb(205, 205, 205);
                }
            }
            else
            {
                gradientPanel1.GradientColor = Color.LightGray;
            }
        }
        
        #endregion

        #region Helper

        private void GenericMouseEnter()
        {
            if (Cursor == Cursors.Hand) return;            
            
            gradientPanel1.SuspendLayout();

            gradientPanel1.GradientColor = Color.NavajoWhite;
            _label_Title.ForeColor = Color.FromArgb(75, 75, 75);
            _label_Description.ForeColor = Color.FromArgb(95, 95, 95);

            Cursor = Cursors.Hand;

            // swap out to the hover image
            if (m_HoverImage != null)
            {
                _pictureBox_TaskImage.SuspendLayout();
                m_TempImage = _pictureBox_TaskImage.Image;
                _pictureBox_TaskImage.Image = m_HoverImage;
                _pictureBox_TaskImage.ResumeLayout();
                _pictureBox_TaskImage.Refresh();
            }

            gradientPanel1.ResumeLayout();
            gradientPanel1.Refresh();
        }

        private void GenericMouseLeave()
        {
            if (gradientPanel1.ClientRectangle.Contains(gradientPanel1.PointToClient(new Point(MousePosition.X, MousePosition.Y)))) return;
            
            gradientPanel1.SuspendLayout();
            
            gradientPanel1.GradientBorderMode = GradientPanel.GradientBorderStyle.Fixed3DOut;
            gradientPanel1.GradientColor = Color.FromArgb(205, 205, 205);
            _label_Title.ForeColor = Color.FromArgb(75, 75, 75);
            _label_Description.ForeColor = Color.FromArgb(95, 95, 95);

            // swap back to the smaller image
            if (m_HoverImage != null && m_TempImage != null)
            {
                _pictureBox_TaskImage.SuspendLayout();
                _pictureBox_TaskImage.Image = m_TempImage;
                _pictureBox_TaskImage.ResumeLayout();
                _pictureBox_TaskImage.Refresh();
            }

            Cursor = Cursors.Default;

            gradientPanel1.ResumeLayout();
            gradientPanel1.Refresh();             
        }

        private void GenericMouseClick(object sender, EventArgs e)
        {
            gradientPanel1.SuspendLayout();
            
            gradientPanel1.GradientBorderMode = GradientPanel.GradientBorderStyle.Fixed3DOut;
            gradientPanel1.GradientColor = Color.FromArgb(205, 205, 205);
            _label_Title.ForeColor = Color.FromArgb(75, 75, 75);
            _label_Description.ForeColor = Color.FromArgb(95, 95, 95);

            // swap back to the original image
            if (m_HoverImage != null && m_TempImage != null)
            {
                _pictureBox_TaskImage.SuspendLayout();
                _pictureBox_TaskImage.Image = m_TempImage;
                _pictureBox_TaskImage.ResumeLayout();
                _pictureBox_TaskImage.Refresh();
            }

            Cursor = Cursors.Default;

            gradientPanel1.ResumeLayout();
            gradientPanel1.Refresh();

            if (m_TaskHandler != null) { m_TaskHandler(sender, e); }
        }

        #endregion
    }
}