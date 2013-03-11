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

            // Create properties.
            Property[] props = null;
			Property descriptionProp = new Property();
            descriptionProp.Name = "Description";
            descriptionProp.Value = reportDescription;

            const string InternalReport = @"SubReport_";
            // Hide the internal sub-report.
            if (reportName.Length >= InternalReport.Length && string.Compare(reportName.Substring(0,InternalReport.Length), InternalReport, true) == 0)
            {
                // Hide internal view report.
                Property hideProp = new Property();
                hideProp.Name = "Hidden";
                hideProp.Value = "true";

                props = new Property[2];
                props[0] = descriptionProp;
                props[1] = hideProp;
            }
            else
            {
                props = new Property[1];
                props[0] = descriptionProp;
            }

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

