using System;
using Microsoft.Win32;

namespace Idera.Common.ReportsInstaller.Model
{
	/// <summary>
	/// Read and write the preferences from the local registry.
	/// </summary>
	public class Preferences
	{
		// Singleton Pattern
		private Preferences(){}
		public static Preferences Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly Preferences ONLY = new Preferences();
		}
      
		/// <summary>
		/// Loads perferences from local registry.
		/// </summary>
		/// <param name="regkey">The name of a subkey in the registry.</param>
		/// <param name="name">The name of the value to retrieve.</param>
		/// <param name="defaultValue">The value to return if the name does not
		/// exist in the registry.</param>
		/// <returns>retrieved preference</returns>
		public string ReadPreferences(string regkey, string name, object defaultValue)
		{
			string regValue = "";

			// I decided to place the try-catch here instead of in RsModel because if the
			// registry operation fails, the user will still be able to successfully
			// complete the installation.  It just means that the fields will not have the
			// user's previous settings.
			try
			{
				using (RegistryKey rk  = Registry.CurrentUser)
				{
					using (RegistryKey rks = rk.CreateSubKey(regkey))
					{
						// name of the value to retrieve
						// value to return if name does not exist
						regValue = (string)rks.GetValue(name, defaultValue);
					}
				}
			}
			catch (Exception)
			{
			}
			return regValue;
		}

		/// <summary>
		/// Writes perferences from local registry.
		/// </summary>
		/// <param name="regkey">The name of a subkey in the registry.</param>
		/// <param name="name">The name of the value to store the data in.</param>
		/// <param name="newValue">The data to store.</param>
		public void WritePreferences(string regkey, string name, object newValue)
		{
			// I decided to place the try-catch here instead of in RsModel because if the
			// registry operation fails, the user will still be able to successfully
			// complete the installation.  It just means that the fields will not have the
			// user's previous settings.
			try
			{
				using (RegistryKey rk  = Registry.CurrentUser)
				{
					using (RegistryKey rks = rk.CreateSubKey(regkey))
					{
						// name of the value to store data in
						// data to store
						rks.SetValue(name, newValue);
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}