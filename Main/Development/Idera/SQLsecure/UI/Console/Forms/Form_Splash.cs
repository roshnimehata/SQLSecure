using System;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_Splash : Form
    {
        Double m_currentOpacity;
        Boolean m_bOpening;
        Boolean m_bCloseTime;


        public Form_Splash()
        {
            InitializeComponent();
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            m_bOpening = true;
            m_currentOpacity = 0;
            TimerToClose.Interval = 50;
            TimerToClose.Enabled = true;
            Opacity = m_currentOpacity;
            m_bCloseTime = false;
            
            string copyrightYear = DateTime.Now.Year.ToString();
            string productVersion = Application.ProductVersion;

            StringBuilder copyrightSB = new StringBuilder();
            copyrightSB.Append("Version ");
            copyrightSB.Append(productVersion);
            copyrightSB.Append(" Copyright © ");
            copyrightSB.Append(copyrightYear);
            copyrightSB.Append(" IDERA, Inc.");

            copyrightLabel.Text = copyrightSB.ToString();
            //copyrightLabel.Text = Constants.COPYRIGHT_SPLASH_MSG;
        }

        private void TimerToClose_Tick(object sender, EventArgs e)
        {
            if (m_bOpening)
            {
                m_currentOpacity = m_currentOpacity + 0.1;
                if (m_currentOpacity < 1)
                    Opacity = m_currentOpacity;
                else
                {
                    m_bOpening = false;
                    TimerToClose.Interval = 1000;
                }
            }
            else
            {
                if (m_bCloseTime)
                {
                    TimerToClose.Interval = 50;
                    m_currentOpacity = m_currentOpacity - 0.1;
                    if (m_currentOpacity > 0)
                        Opacity = m_currentOpacity;
                    else
                        Close();
                }
            }
        }

        public void ForceClose()
        {
            m_bCloseTime = true;
        }
    }
}