using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_GettingStarted : Idera.SQLsecure.UI.Console.Controls.BaseDialog
    {
        private const string FinishText = "&Finish";
        private const string NextText = "&Next >>";

        public Form_GettingStarted()
        {
            InitializeComponent();

            controlGettingStarted1.RegisterTabPageChangedDelegate(TabPageChanged);
            controlGettingStarted1.LoadData();
        }

        static public void Process()
        {
            Form_GettingStarted dlg = new Form_GettingStarted();            
            dlg.ShowDialog();

        }

        private void ultraButton_Next_Click(object sender, EventArgs e)
        {

            if(ultraButton_Next.Text == FinishText)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                return;
            }

            bool pagesLeft = controlGettingStarted1.NextPage();

            if(!pagesLeft)
            {
                ultraButton_Next.Text = FinishText;
            }
            ultraButton_Back.Enabled = true;
        }

        private void ultraButton_Back_Click(object sender, EventArgs e)
        {
            bool pagesLeft = controlGettingStarted1.BackPage();
            if(!pagesLeft)
            {
                ultraButton_Back.Enabled = false;
            }
            ultraButton_Next.Text = NextText;
        }

        private void ultraButton_Help_Click(object sender, EventArgs e)
        {

        }


        private void TabPageChanged(int pagesToGoBack, int pagesToGoForward)
        {
            if(pagesToGoBack == 0)
            {
                ultraButton_Back.Enabled = false;
            }
            else
            {
                ultraButton_Back.Enabled = true;
            }

            if(pagesToGoForward == 0)
            {
                ultraButton_Next.Text = FinishText; 
            }
            else
            {
                ultraButton_Next.Text = NextText;
            }
        }
    }
}

