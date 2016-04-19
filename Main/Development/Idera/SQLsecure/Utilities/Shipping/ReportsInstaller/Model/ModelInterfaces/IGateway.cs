using System;
using System.Data.SqlClient;

namespace Idera.Common.ReportsInstaller.Model.ModelInterfaces
{
	/// <summary>
	/// Summary description for IGateway.
	/// </summary>
	public interface IGateway
	{
		/// <summary>
		/// The connection string associated with a particular database.
		/// </summary>
		string ConnectionString
		{
			get;
		}

		/// <summary>
		/// The actual connection to a particular database.
		/// </summary>
		SqlConnection SQLConnection
		{
			get;
		}

		/// <summary>
		/// The name of a table in a database.
		/// </summary>
		string TableName
		{
			get;
		}

		/// <summary>
		/// Tests the connection to this gateway's specified database.
		/// Must be placed in a try-catch in case an exception is thrown.
		/// </summary>
		void TestConnection();

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
		object ExecuteQuery(IQuery query, string[] columns, Type[] types, 
			object[] values);
	}
}
