using System ;
using System.ComponentModel ;
using System.Drawing ;
using System.Windows.Forms ;
using File=System.IO.File ;

namespace Idera.Common.ReportsInstaller.View.HelperForms
{
	/// <summary>
	/// Summary description for SQLInstanceForm.
	/// </summary>
	public class SQLInstanceForm : Form
	{
		private Button buttonCancel;
		private Button buttonOk;
		private ListBox listBoxServer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private event SQLInstanceFormEventHandler okButtonEvent;
		public SQLInstanceFormEventHandler OkButtonEvent
		{
			get
			{
				return okButtonEvent;
			}
			set
			{
				okButtonEvent += value;
			}
		}

		public SQLInstanceForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SQLInstanceForm));
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.listBoxServer = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(208, 240);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(120, 240);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "&OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// listBoxServer
			// 
			this.listBoxServer.HorizontalScrollbar = true;
			this.listBoxServer.Location = new System.Drawing.Point(8, 8);
			this.listBoxServer.Name = "listBoxServer";
			this.listBoxServer.Size = new System.Drawing.Size(272, 225);
			this.listBoxServer.TabIndex = 2;
			// 
			// SQLInstanceForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.listBoxServer);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SQLInstanceForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select SQL Server Instance";
			this.Closed += new System.EventHandler(this.SQLInstanceForm_Close);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOk_Click(object sender, EventArgs e)
		{
			if(listBoxServer.SelectedIndex != -1)
			{
				okButtonEvent();
				listBoxServer.Items.Clear();
				this.Close();
			}
		
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			listBoxServer.Items.Clear();
			this.Close();
		}

		private delegate string SelectedInstanceInBoxDelegate();
		/// <summary>
		/// Retrieves the selected instance of SQL server.
		/// </summary>
		/// <returns>the selected instance</returns>
		public string SelectedInstanceInBox()
		{
			if (this.InvokeRequired)
			{
				return (string)(this.Invoke(new SelectedInstanceInBoxDelegate(SelectedInstanceInBox), 
					null)); 
			}
			return listBoxServer.SelectedItem.ToString();
		}

		private delegate void AddItemToSQLInstanceListDelegate(string data);
		/// <summary>
		/// Adds an item to the list box in the SQL Instance Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		public void AddItemToSQLInstanceList(string data)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new AddItemToSQLInstanceListDelegate(AddItemToSQLInstanceList), 
					new object[] {data});
				return;
			}
			listBoxServer.Items.Add(data);
		}

		private void SQLInstanceForm_Close(object sender, EventArgs e)
		{
			listBoxServer.Items.Clear();
		}

		private delegate void InitializeDelegate(string icon);
		public void Initialize(string icon)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new InitializeDelegate(Initialize), new object[] {icon});
				return;
			}
			if (File.Exists(icon))
			{
				try
				{
					this.Icon = new Icon(icon);
				}
				catch (Exception)
				{
				}
			}
		}
	}
	public delegate void SQLInstanceFormEventHandler();

}
