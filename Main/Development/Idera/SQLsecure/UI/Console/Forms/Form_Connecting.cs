using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Idera.SQLsecure.UI.Console.Forms
{
	 public partial class Form_Connecting : Form
	 {
		  public Form_Connecting(string serverName)
		  {
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				m_server = serverName;
				labelServer.Text = serverName;
				this.Invalidate();

		  }

		  private string m_server = "";
	 }
}