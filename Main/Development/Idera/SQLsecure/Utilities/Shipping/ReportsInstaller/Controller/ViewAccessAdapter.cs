using Idera.Common.ReportsInstaller.Model ;
using Idera.Common.ReportsInstaller.View ;

namespace Idera.Common.ReportsInstaller.Controller
{
	/// <summary>
	/// Gives the model access to certain methods in the view.
	/// </summary>
	public class ViewAccessAdapter : IViewAccessAdapter
	{
		private IInstallerGUI _view;
		/// <summary>
		/// The view that the adapter can decide to expose.
		/// </summary>
		public IInstallerGUI View
		{
			get
			{
				return _view;
			}
			set
			{
				_view = value;
			}
		}

		/// <summary>
		/// Displays a message box with the specified text and specified
		/// title bar text.
		/// </summary>
		/// <param name="message">The text that appears in the message box.</param>
		/// <param name="titleBar">The text that appears in the title bar
		/// of the message box.</param>
		public void DisplayMessageBox(string message, string titleBar)
		{
			_view.DisplayMessageBox(message, titleBar);
		}

		/// <summary>
		/// Displays a message box with the specified text and specified
		/// title bar text.  Also includes YesNo buttons.
		/// </summary>
		/// <param name="message">The text that appears in the message box.</param>
		/// <param name="titleBar">The text that appears in the title bar
		/// of the message box.</param>
		/// <returns>True if Yes is chosen; False if No is chosen</returns>
		public bool DisplayMessageBoxYesNo(string message, string titleBar)
		{
			return _view.DisplayMessageBoxYesNo(message, titleBar);
		}

		/// <summary>
		/// Adds text to the installer log (txtInstallerLog).
		/// </summary>
		/// <param name="message">The text that appears in the rich text box 
		/// acting as an installer log.</param>
		public void UpdateInstallerLog(string message)
		{
			_view.UpdateInstallerLog(message);
		}

		/// <summary>
		/// Adds a report to the checked list box (checkedListReports).
		/// </summary>
		/// <param name="data">The item added to the checked list box</param>
		public void AddToReportsList(string data)
		{
			_view.AddToReportsList(data);
		}

		/// <summary>
		/// Adds a report to the already installed list box (listBoxReports).
		/// </summary>
		/// <param name="data">The item added to the list box</param>
		public void AddToAlreadyInstalledList(string data)
		{
			_view.AddToAlreadyInstalledList(data);
		}

		/// <summary>
		/// Adds a report to the completed list box (listBoxComplete).
		/// </summary>
		/// <param name="data">The item added to the list box</param>
		public void AddToInstalledList(string data)
		{
			_view.AddToInstalledList(data);
		}

		/// <summary>
		/// Adds an item to the list box in the SQL Instance Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		public void AddItemToSQLInstanceList(string data)
		{
			_view.AddItemToSQLInstanceList(data);
		}

		/// <summary>
		/// Adds an item to the list box in the folder Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		public void AddItemToFolderList(string data)
		{
			_view.AddItemToFolderList(data);
		}

	}
}
