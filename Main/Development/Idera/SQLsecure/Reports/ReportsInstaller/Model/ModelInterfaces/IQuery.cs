using System;

namespace Idera.Common.ReportsInstaller.Model.ModelInterfaces
{
	/// <summary>
	/// The abstraction of a query to the databasee.
	/// </summary>
	public interface IQuery
	{
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
		/// <returns>the execution of the query</returns>
		object Execute(IGateway host, string[] columns, Type[] types, 
			object[] values);
	}
}
