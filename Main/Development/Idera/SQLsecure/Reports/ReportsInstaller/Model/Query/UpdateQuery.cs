using System;
using System.Data.SqlClient;
using System.ComponentModel;
using Idera.Common.ReportsInstaller.Model.ModelInterfaces;

namespace Idera.Common.ReportsInstaller.Model.Query
{
	/// <summary>
	/// An UPDATE query to a table in a SQL database.
	/// </summary>
	public class UpdateQuery : IQuery
	{
		// Singleton Pattern
		private UpdateQuery(){}
		public static IQuery Singleton { get { return Nested.Singleton;}}
		class Nested
		{
			static Nested() {}
			internal static readonly UpdateQuery Singleton = new UpdateQuery();
		}

		/// <summary>
		/// Executes a query on a table in the SqlConnection.
		/// The columns array, the types array, and the values array must 
		/// be of the same length.
		/// For example, if you want the 'Name' column (string) to be set to 'Bob'
		/// and the 'Age' column (int) to be set to 30, then the arrays must contain
		/// the following:
		/// columns = {Name, Age}
		/// types = {typeof(string), typeof(int)}
		/// values = {"Bob", 30} 
		/// Uses the visitor design pattern.
		/// </summary>
		/// <param name="host">The gateway that the query affects.</param>
		/// <param name="columns">The columns in the table that the query affects.</param>
		/// <param name="types">The datatypes associated with the columns.</param>
		/// <param name="values">The values associated with the columns.</param>
		/// <returns>number of rows affected by update</returns>
		public object Execute(IGateway host, string[] columns, Type[] types, 
			object[] values)
		{
			// build query example
			// string SQL_UPDATE_REPORT =" UPDATE "+TABLE_NAME+" SET reportServer=@reportServer,reportFolder=@reportFolder";
			string update =" UPDATE "+host.TableName+" SET ";
			if (columns.Length > 0)
			{
				update += columns[0]+"=@"+columns[0];
				for (int i=1; i < columns.Length; i++)
				{
					update += ","+columns[i]+"=@"+columns[i];
				}
			}
			string query = update; 

			int rowUpdateCount = 0;
			using (SqlConnection cn = host.SQLConnection)
			{
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
			
//					SqlCommand cmd = new SqlCommand(query, host.SQLConnection);
					// build params
					if (values.Length > 0)
					{
						TypeConverter converter = new TypeConverter();
						for (int i=0; i < values.Length; i++)
						{
							cmd.Parameters.Add("@"+columns[i],converter.ConvertTo(values[i], types[i]));
						}
					}

					// execute query
					cn.Open();
//					host.SQLConnection.Open();
					rowUpdateCount = cmd.ExecuteNonQuery();
//					host.SQLConnection.Close();
				}
			}
			return rowUpdateCount;
		}
	}
}
