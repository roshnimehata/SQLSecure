using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Idera.SQLsecure.Core.Logger;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_Working : Form
    {
        private delegate void SetTextDelegate(string text);
        private delegate void CloseFormDelegate();
        private delegate void ShowFormDelegate();

        private Point m_ParentDesktopLocation;
        private Rectangle m_parentClientRect;


        public Form_Working(Point parentDeskTopLocation, Rectangle parentClientRect)
        {
            m_ParentDesktopLocation = parentDeskTopLocation;
            m_parentClientRect = parentClientRect;
            InitializeComponent();
        }

        public string WorkingText
        {
            set { label_text.Text = value;}
        }

        public void CloseForm()
        {
            if (this.InvokeRequired)
            {
                CloseFormDelegate d = new CloseFormDelegate(CloseForm);
                this.Invoke(d);
            }
            else
            {
                Close();                    
            }
        }

        public void SetText(string text)
        {
            if (this.InvokeRequired)
            {
                SetTextDelegate d = new SetTextDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                label_text.Text = text;
            }
        }

        public void ShowForm()
        {
            if (this.InvokeRequired)
            {
                ShowFormDelegate d = new ShowFormDelegate(ShowForm);
                this.Invoke(d);
            }
            else
            {
                this.ShowDialog();
            }            
        }

        private void Form_Working_Load(object sender, EventArgs e)
        {

            int x = m_ParentDesktopLocation.X + (m_parentClientRect.Width - ClientRectangle.Width) / 2;
            int y = m_ParentDesktopLocation.Y + (m_parentClientRect.Height - ClientRectangle.Height) / 2;
            this.SetDesktopLocation(x, y);    
            this.Activate();
        }           


    }

    public class ShowWorkingProgress
    {
        private Form_Working m_workingForm = null;
        private System.Threading.Thread progressThread = null;
        private static LogX logX = new LogX("Idera.SQLsecure.UI.Console.Forms.ShowWorkingProgress");


        public void Show(string text, Form parent)
        {
            Point parentDesktopLocation = parent.DesktopLocation;
            Rectangle parentClientRect = parent.ClientRectangle;
            if (progressThread == null)
            {
                progressThread =
                    new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(ShowWorkingDialog));
                progressThread.Start((object)new object[] { text,
                                                    parentDesktopLocation,
                                                    parentClientRect } );
            }
        }

        public void Close()
        {
            // ReTry up to 2 secounds 
            // just in case close is called before other thread initialized m_workingForm
            for (int x = 0; x < 20; x++)
            {
                if (m_workingForm != null)
                {
                    m_workingForm.CloseForm();
                    System.Threading.Thread.Sleep(100);
                    break;
                }
                System.Threading.Thread.Sleep(100);
            }
        }


        public void UpdateText(string text)
        {
            try
            {
                if(m_workingForm != null)
                {
                    m_workingForm.SetText(text);
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Updating Working Progress Dialog Text: {0}", ex.Message);
            }
            
        }

        private void ShowWorkingDialog(object value)
        {        
            try
            {
                object[] values = (object[]) value;
                string text = (string)values[0];
                Point parentDesktopLocation = (Point)values[1];
                Rectangle parentClientRect = (Rectangle)values[2];
                if (m_workingForm == null)
                {
                    m_workingForm = new Form_Working(parentDesktopLocation, parentClientRect);
                    m_workingForm.SetText(text);
                    m_workingForm.ShowForm();
                }
            }
            catch (Exception ex)
            {
                logX.loggerX.Error("Error Showing Progress Dialog: {0}", ex.Message);
            }
        }

    }
    
}