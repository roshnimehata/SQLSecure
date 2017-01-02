using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteSQLScript
{
    class Program
    {
        private static string GetFilePath
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        public enum ExitCode
        {
            Success = 0,
            Script_not_present = -999,
            Script_failure = -1000
        }

        static int Main(string[] args)
        {
            const string SaveCollectorInfo = @"SQLsecure.dbo.isp_sqlsecure_addcollectorinfo";
            string[] sql_script_list = { "sqlsecure_ddl.sql", "merge_fn.sql", "merge_vw.sql", "merge_sp.sql", "sql_postscript.sql", "Sqlsecure_version.sql" };
            string server_name = args[0];
            System.IO.File.AppendAllText(@"D:\path.txt", "SERVER NAME : " + args[0] + "\n\n");
            bool areScriptsPresent = true;
            foreach(string file in sql_script_list)
            {
                if(!File.Exists(@"D:\idera-SQLSecure\SQLSecure\Main\Development\Idera\SQLsecure\bin\Debug" + "\\" + file))
                {
                    areScriptsPresent = false;
                    return (int)ExitCode.Script_not_present;
                }
            }
            if (areScriptsPresent)
            {
                try
                {
                    foreach (string sql_script in sql_script_list) {
                        string sqlConnectionString = @"Data Source=" + server_name + ";Initial Catalog=master ;Integrated Security=True";
                        System.IO.File.AppendAllText(@"D:\path.txt", "Connection String : " + sqlConnectionString + "\n\n");
                        FileInfo file = new FileInfo(@"D:\idera-SQLSecure\SQLSecure\Main\Development\Idera\SQLsecure\bin\Debug" + "\\" + sql_script);
                        string script = file.OpenText().ReadToEnd();
                        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(@"Data Source=" + server_name + ";Initial Catalog=master ;Integrated Security=True");
                        Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new ServerConnection(conn));
                        System.IO.File.AppendAllText(@"D:\path.txt", "Before executing non query");
                        int success = server.ConnectionContext.ExecuteNonQuery(script);
                         System.IO.File.AppendAllText(@"D:\path.txt", "After executing query");
                        ////                       
                        ////
                        if (success == 0)
                        {
                            //script did not execute successfully
                            Console.WriteLine("Script could not be executed.");
                                System.IO.File.AppendAllText(@"D:\path.txt", "Script could not be executed.");
                            return (int)ExitCode.Script_failure;
                        }
                        else
                        {
                            //script executed successfully
                            Console.WriteLine("Script executed successfully. Total queries executed : " + success);
                                  System.IO.File.AppendAllText(@"D:\path.txt", "Script executed successfully. Total queries executed : " + success);
                        }

                    }
                    System.Data.SqlClient.SqlConnection conn1 = new System.Data.SqlClient.SqlConnection(@"Data Source=" + server_name + ";Initial Catalog=master ;Integrated Security=True");
                    SqlCommand cmd = new SqlCommand(SaveCollectorInfo, conn1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@infoname", SqlDbType.VarChar, 64).Value = "FILEPATH";
                    cmd.Parameters.Add("@infovalue", SqlDbType.VarChar, 1000).Value = GetFilePath + "\\Idera.SQLsecure.Collector.exe";
                    conn1.Open();
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error occured! " + e);
                    System.IO.File.AppendAllText(@"D:\path.txt", "Exception " + e);
                }

            }
            return (int)ExitCode.Success;
        }
    }
}
