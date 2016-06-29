using System;
using System.IO;
using Idera.Common.ReportsInstaller.rs;
using Idera.Common.ReportsInstaller.Model.ModelInterfaces;

namespace Idera.Common.ReportsInstaller.Model.RsAdapter
{
	/// <summary>
	/// Adds a new report to the report server database.
	/// </summary>
	public class DeployReportAlgo : IRsAlgo
	{
		// Singleton Pattern
		private DeployReportAlgo(){}
		public static IRsAlgo Singleton { get { return Nested.ONLY;}}
		class Nested
		{
			static Nested() {}
			internal static readonly DeployReportAlgo ONLY = new DeployReportAlgo();
		}

		/// <summary>
		/// Adds a new report to the report server database.
		/// </summary>
		/// <param name="host">The IProxyFactory that the algorithm or 'visitor' is executed on.</param>
		/// <param name="param[0]">The name of the server that has Reporting Service.</param>
		/// <param name="param[1]">The virtual directory in the server.</param>
		/// <param name="param[2]">The full path name of the folder.</param>
		/// <param name="param[3]">The full path name of the RDL.</param>
		/// <param name="param[4]">The name of the new report.</param>
		/// <param name="param[5]">The value of a property for a report.</param>
		/// <returns>True for successful; False for otherwise</returns>
		public object Execute(IProxyFactory host, params object[] param)
		{
			string server = (string)param[0];
			string virtualRoot = (string)param[1];
			string folderPath = (string)param[2];
			string rdlPath = (string)param[3];
			string reportName = (string)param[4];
			string reportDescription = (string)param[5];

			Byte[] definition;

			// Read the RDL file and convert to byte array
			using (FileStream stream = File.OpenRead(rdlPath))
			{
				definition = new Byte[stream.Length];
				stream.Read(definition, 0, (int) stream.Length);
			}
			//stream.Close();

			ReportingService rs =  host.Proxy(server, virtualRoot);	
			Property setProp = new Property();
			setProp.Name="Description";
			setProp.Value=reportDescription;	
			Property[] props = new Property[1];
			props[0]=setProp;
				
			// Upload the report
			rs.CreateReport(reportName,
				folderPath,
				true,
				definition,
				props);

			return true;
		}
	}
}

