using System ;
using System.ComponentModel ;
using System.Drawing ;
using System.Windows.Forms ;
using File=System.IO.File ;

namespace Idera.Common.ReportsInstaller.View.HelperForms
{
	/// <summary>
	/// Summary description for FolderForm.
	/// </summary>
	public class FolderForm : Form
	{
		private Button buttonOk;
		private Button buttonCancel;
		private ListBox listBoxFolder;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private event FolderFormEventHandler okButtonEvent;
		public FolderFormEventHandler OkButtonEvent
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

		public FolderForm()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FolderForm));
			this.listBoxFolder = new System.Windows.Forms.ListBox();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listBoxFolder
			// 
			this.listBoxFolder.HorizontalScrollbar = true;
			this.listBoxFolder.Location = new System.Drawing.Point(8, 8);
			this.listBoxFolder.Name = "listBoxFolder";
			this.listBoxFolder.Size = new System.Drawing.Size(272, 225);
			this.listBoxFolder.TabIndex = 0;
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(64, 240);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "&OK";
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(152, 240);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// FolderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.listBoxFolder);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FolderForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Report Folder";
			this.Closed += new System.EventHandler(this.FolderForm_Close);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonOk_Click(object sender, EventArgs e)
		{
			if(listBoxFolder.SelectedIndex != -1)
			{
				okButtonEvent();
				listBoxFolder.Items.Clear();
				this.Close();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			listBoxFolder.Items.Clear();
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
			return listBoxFolder.SelectedItem.ToString();
		}

		private delegate void AddItemToFolderListDelegate(string data);
		/// <summary>
		/// Adds an item to the list box in the SQL Instance Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		public void AddItemToFolderList(string data)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new AddItemToFolderListDelegate(AddItemToFolderList),
					new object[] {data});
				return;
			}
			listBoxFolder.Items.Add(data);
		}

		private void FolderForm_Close(object sender, EventArgs e)
		{
			listBoxFolder.Items.Clear();
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
	public delegate void FolderFormEventHandler();
}
