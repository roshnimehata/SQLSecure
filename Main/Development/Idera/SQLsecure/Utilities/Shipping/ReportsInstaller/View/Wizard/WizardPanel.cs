using System.Windows.Forms ;

namespace Idera.Common.ReportsInstaller.View.Wizard
{
	/// <summary>
	/// Abstraction of a panel in wizard application.
	/// </summary>
	public class WizardPanel : Panel
	{
		/// <summary>
		/// Defines what happens when the user wishes to go to the previous panel.
		/// </summary>
		private event PreviousPanelEventHandler previousPanelEvent;
		public PreviousPanelEventHandler PreviousPanelEvent
		{
			get
			{
				return previousPanelEvent;
			}
			set
			{
				previousPanelEvent += value;
			}
		}

		/// <summary>
		/// Defines what happens when the user wishes to go to the next panel.
		/// </summary>
		private event NextPanelEventHandler nextPanelEvent;
		public NextPanelEventHandler NextPanelEvent
		{
			get
			{
				return nextPanelEvent;
			}
			set
			{
				nextPanelEvent += value;
			}
		}

		/// <summary>
		/// Performs the actions required to go to the previous panel.
		/// </summary>
		public void PreviousPanel()
		{
			previousPanelEvent();
		}

		/// <summary>
		/// Performs the actions required to go to the next panel.
		/// </summary>
		public void NextPanel()
		{
			nextPanelEvent();
		}
	}
}
