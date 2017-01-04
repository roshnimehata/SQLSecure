using Microsoft.SqlServer.Management.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DeployRepository
{
    class Program
    {

        public static string ServerName;
        public static string UserName;
        public static string Password;
        public static string UpgradeRepository;
        public static string sqlConnectionString;
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
            ScriptNotExist = -999,
            ScriptFailure = -1000
        }

        static int Main(string[] args)
        {
            int status = 0;
            ServerName = args[0];
            UpgradeRepository = args[1];
            if(args.Length == 4)
            {
                UserName = args[2];
                Password = args[3];
                sqlConnectionString = @"Data Source=" + ServerName + ";Initial Catalog=master ;User ID= " + UserName + ";Password=" + Password + ";";
            }else
            {
                sqlConnectionString = @"Data Source=" + ServerName + ";Initial Catalog=master ;Integrated Security=True";
            }
            if (UpgradeRepository.Equals("true"))
            {
                status = ExecuteUpdate();
            }else
            {
                status = DeployRepositoryQuery();
            }
            return status;
        }

        public static int ExecuteUpdate()
        {
            string sql_update_script = "sqlsecure_ddl_update.sql";
            if (!File.Exists(GetFilePath + "\\" + sql_update_script))
            {
                return (int)ExitCode.ScriptNotExist;
            }
            try
            {
                System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Connection String : " + sqlConnectionString + "\n\n");
                FileInfo file = new FileInfo(GetFilePath + "\\" + sql_update_script);
                string script = file.OpenText().ReadToEnd();
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(sqlConnectionString);
                Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new ServerConnection(conn));
                System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Before executing non query");
                int success = server.ConnectionContext.ExecuteNonQuery(script);
                System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "After executing query");
                if (success == 0)
                {
                    //script did not execute successfully
                    Console.WriteLine("Script could not be executed.");
                    System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Script could not be executed.");
                    return (int)ExitCode.ScriptFailure;
                }
                else
                {
                    //script executed successfully
                    Console.WriteLine("Script executed successfully. Total queries executed : " + success);
                    System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Script executed successfully. Total queries executed : " + success);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured! " + e);
                System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Exception " + e);
            }
            return (int)ExitCode.Success;
        }

        public static int DeployRepositoryQuery()
        {

            const string SaveCollectorInfo = @"SQLsecure.dbo.isp_sqlsecure_addcollectorinfo";
            string[] sql_script_list = { "sqlsecure_ddl.sql", "merge_fn.sql", "merge_vw.sql", "merge_sp.sql", "sql_postscript.sql", "Sqlsecure_version.sql" };

            System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "SERVER NAME : " + ServerName + "\n\n");
            bool DoScriptsExist = true;
            foreach (string file in sql_script_list)
            {
                if (!File.Exists(GetFilePath + "\\" + file))
                {
                    DoScriptsExist = false;
                    return (int)ExitCode.ScriptNotExist;
                }
            }
            if (DoScriptsExist)
            {
                try
                {
                    foreach (string sql_script in sql_script_list)
                    {
                        System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Connection String : " + sqlConnectionString + "\n\n");
                        FileInfo file = new FileInfo(GetFilePath + "\\" + sql_script);
                        string script = file.OpenText().ReadToEnd();
                        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(sqlConnectionString);
                        Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new ServerConnection(conn));
                        System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Before executing non query");
                        int success = server.ConnectionContext.ExecuteNonQuery(script);
                        System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "After executing query");
                        if (success == 0)
                        {
                            //script did not execute successfully
                            Console.WriteLine("Script could not be executed.");
                            System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Script could not be executed.");
                            return (int)ExitCode.ScriptFailure;
                        }
                        else
                        {
                            //script executed successfully
                            Console.WriteLine("Script executed successfully. Total queries executed : " + success);
                            System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Script executed successfully. Total queries executed : " + success);
                        }

                    }
                    System.Data.SqlClient.SqlConnection conn1 = new System.Data.SqlClient.SqlConnection(sqlConnectionString);
                    SqlCommand cmd = new SqlCommand(SaveCollectorInfo, conn1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@infoname", SqlDbType.VarChar, 64).Value = "FILEPATH";
                    cmd.Parameters.Add("@infovalue", SqlDbType.VarChar, 1000).Value = GetFilePath + "\\Idera.SQLsecure.Collector.exe";
                    conn1.Open();
                    cmd.ExecuteNonQuery();
                    conn1.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occured! " + e);
                    System.IO.File.AppendAllText(GetFilePath + "\\" + "path.txt", "Exception " + e);
                }

            }
            return (int)ExitCode.Success;

        }
    }
}
