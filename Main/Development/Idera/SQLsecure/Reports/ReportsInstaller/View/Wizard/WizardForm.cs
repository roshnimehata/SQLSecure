using System ;
using System.ComponentModel ;
using System.Windows.Forms ;

namespace Idera.Common.ReportsInstaller.View.Wizard
{
	public delegate void PreviousPanelEventHandler();
	public delegate void NextPanelEventHandler();

	/// <summary>
	/// Abstraction of a wizard application.
	/// </summary>
	public class WizardForm : Form
	{
		protected Button buttonPrev;
		protected Button buttonNext;
		protected Button buttonFinish;
		protected Button buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public WizardForm()
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
			this.buttonPrev = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonFinish = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonPrev
			// 
			this.buttonPrev.Location = new System.Drawing.Point(216, 352);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.TabIndex = 0;
			this.buttonPrev.Text = "< &Prev";
			this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(288, 352);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(72, 23);
			this.buttonNext.TabIndex = 1;
			this.buttonNext.Text = "&Next >";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonFinish
			// 
			this.buttonFinish.Location = new System.Drawing.Point(392, 352);
			this.buttonFinish.Name = "buttonFinish";
			this.buttonFinish.TabIndex = 2;
			this.buttonFinish.Text = "&Finish";
			this.buttonFinish.Click += new System.EventHandler(this.buttonFinish_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(480, 352);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// WizardForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(568, 382);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonFinish);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.buttonPrev);
			this.Name = "WizardForm";
			this.Text = "WizardForm";
			this.ResumeLayout(false);

		}
		#endregion



		protected virtual void buttonPrev_Click(object sender, EventArgs e)
		{
			// delegate to panel
		}

		protected virtual void buttonNext_Click(object sender, EventArgs e)
		{
			// delegate to panel
		}

		protected virtual void buttonFinish_Click(object sender, EventArgs e)
		{
		
		}

		protected virtual void buttonCancel_Click(object sender, EventArgs e)
		{
		
		}

		private delegate void GoToPreviousPanelDelegate(WizardPanel currentPanel,
			WizardPanel previousPanel);
		public void GoToPreviousPanel(WizardPanel currentPanel, WizardPanel previousPanel)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new GoToPreviousPanelDelegate(GoToPreviousPanel),
					new object[] {currentPanel, previousPanel});
				return;
			}
			currentPanel.Enabled = false;
			previousPanel.Enabled = true;
			previousPanel.BringToFront();
		}

		private delegate void GoToNextPanelDelegate(WizardPanel currentPanel,
			WizardPanel nextPanel);
		public void GoToNextPanel(WizardPanel currentPanel, WizardPanel nextPanel)
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new GoToNextPanelDelegate(GoToNextPanel),
					new object[] {currentPanel, nextPanel});
				return;
			}
			currentPanel.Enabled = false;
			nextPanel.Enabled = true;
			nextPanel.BringToFront();
		}

	}
}
