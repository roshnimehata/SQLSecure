namespace Idera.Common.ReportsInstaller.View
{
	/// <summary>
	/// The abstraction of the installer display for the application.
	/// The view of the model-view-controller design pattern (MVC pattern).
	/// </summary>
	public interface IInstallerGUI
	{
		/// <summary>
		/// Gives the view access to certain methods in the model.
		/// </summary>
		IModelAccessAdapter ModelAdapter
		{
			get;
		}

		/// <summary>
		/// Starts the GUI
		/// </summary>
		void Run();

		/// <summary>
		/// Displays a message box with the specified text and specified
		/// title bar text.
		/// </summary>
		/// <param name="message">The text that appears in the message box.</param>
		/// <param name="titleBar">The text that appears in the title bar
		/// of the message box.</param>
		void DisplayMessageBox(string message, string titleBar);

		/// <summary>
		/// Displays a message box with the specified text and specified
		/// title bar text.  Also includes YesNo buttons.
		/// </summary>
		/// <param name="message">The text that appears in the message box.</param>
		/// <param name="titleBar">The text that appears in the title bar
		/// of the message box.</param>
		/// <returns>True if Yes is chosen; False if No is chosen</returns>
		bool DisplayMessageBoxYesNo(string message, string titleBar);

		/// <summary>
		/// Adds text to the installer log (txtInstallerLog).
		/// </summary>
		/// <param name="message">The text that appears in the rich text box 
		/// acting as an installer log.</param>
		void UpdateInstallerLog(string message);

		/// <summary>
		/// Adds a report to the checked list box (checkedListReports).
		/// </summary>
		/// <param name="data">The item added to the checked list box</param>
		void AddToReportsList(string data);

		/// <summary>
		/// Adds a report to the already installed list box (listBoxReports).
		/// </summary>
		/// <param name="data">The item added to the list box</param>
		void AddToAlreadyInstalledList(string data);

		/// <summary>
		/// Adds a report to the completed list box (listBoxComplete).
		/// </summary>
		/// <param name="data">The item added to the list box</param>
		void AddToInstalledList(string data);

		/// <summary>
		/// Adds an item to the list box in the SQL Instance Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		void AddItemToSQLInstanceList(string data);

		/// <summary>
		/// Adds an item to the list box in the folder Form.
		/// </summary>
		/// <param name="data">The item being added</param>
		void AddItemToFolderList(string data);
	}
}
