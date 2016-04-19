using System;
using Idera.Common.ReportsInstaller.rs;
using Idera.Common.ReportsInstaller.Model.ModelInterfaces;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Determines whether the user has the requested permissions.
	/// </summary>
	public class PermissionsAlgo : IRsAlgo
	{
		// Singleton Pattern
		private PermissionsAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly PermissionsAlgo ONLY = new PermissionsAlgo();
		}

		/// <summary>
		/// Determines whether the user has the requested permissions.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param[0]">The name of the server that has Reporting Service.</param>
		/// <param name="param[1]">The virtual directory in the server.</param>
		/// <param name="param[2]">The item in the ReportServer database that the user 
		/// permissions are associated with.</param>
		/// <param name="param[3]">The required permissions in question.</param>
		/// <returns>True is has requested permissions; False if doesn't</returns>
		public object Execute(IProxyFactory host, params object[] param)
		{
			string server = (string)param[0];
			string virtualRoot = (string)param[1];
			string reportItem = (string)param[2];
			string[] requiredPermissions = (string[])param[3];

			String[] grantedPermissions ;
			ReportingService proxy = host.Proxy(server, virtualRoot);

			// GetPermissions returns a string array containing a list of user
			// permissions that are associated with a particular item in the
			// Reporting Server database.  The required input parameter is a
			// string representing the name of the item.
			grantedPermissions = proxy.GetPermissions(reportItem);

			int i = 0;
			bool hasPermissions = false;
			foreach ( string requiredPerm in requiredPermissions )
			{
				foreach (string grantedPerm in grantedPermissions)
				{
					if (grantedPerm.ToLower() == requiredPerm.ToLower() )
					{
						i++; break;
					}
				}
				if (i == requiredPermissions.Length) 
				{
					hasPermissions = true; break;
				}
			}
			
			return hasPermissions;
		}
	}
}
