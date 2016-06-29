using System;
using System.Data.SqlClient;
using Idera.Common.ReportsInstaller.Model.ModelInterfaces;

namespace Idera.Common.ReportsInstaller.Model
{
	/// <summary>
	/// The abstraction of a connection to a table in a database.
	/// </summary>
	public class Gateway : IGateway
	{
		private string _connectionString;
		/// <summary>
		/// The connection string associated with a particular database.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}
		}

		private SqlConnection _sqlConnection;
		/// <summary>
		/// The actual connection to a particular database.
		/// </summary>
		public SqlConnection SQLConnection
		{
			get
			{
				return _sqlConnection;
			}
		}

		private string _tableName;
		/// <summary>
		/// The name of a table in a database.
		/// </summary>
		public string TableName
		{
			get
			{
				return _tableName;
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="serverName">The name of the server.</param>
		/// <param name="databaseName">The name of the database.</param>
		/// <param name="tableName">The name of the table.</param>
		public Gateway(string serverName, string databaseName, string tableName)
		{
			string conStr = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog={0};Data Source={1}";
			_connectionString = String.Format(conStr,databaseName,serverName);
			_sqlConnection = new SqlConnection(_connectionString);
			_tableName = tableName;
		}

		/// <summary>
		/// Tests the connection to this gateway's specified database.
		/// Must be placed in a try-catch in case an exception is thrown.
		/// </summary>
		public void TestConnection()
		{
			using (SqlConnection cn = _sqlConnection)
			{
				cn.Open();
			}
//			_sqlConnection.Close();
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
		/// <param name="query">The query (or visitor) that is executed.</param>
		/// <param name="columns">The columns in the table that the query affects.</param>
		/// <param name="types">The datatypes associated with the columns.</param>
		/// <param name="values">The values associated with the columns.</param>
		/// <returns>the execution of the query</returns>
		public object ExecuteQuery(IQuery query, string[] columns, Type[] types, 
			object[] values)
		{
			return query.Execute(this, columns, types, values);
		}
	}
}
