using System;
using System.Collections.Generic;
using System.Data;
using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;
using com.codeplex.dbabstraction.DatabaseAbstraction.Models;
using com.codeplex.dbabstraction.DatabaseAbstraction.Queries;

namespace com.codeplex.dbabstraction.DatabaseAbstraction {
	/// <summary>
	/// This abstract class contains the majority of the implementation of the database abstraction.  The specific
	/// implementations only need to call the constructor with the list of classes, then fill the connection property
	/// with a concrete database connection.
	/// </summary>
	public abstract class DatabaseService {
		
		/// <summary>
		/// The query library. 
		/// </summary>
		private Dictionary<string, DatabaseQuery> Queries { get; set; }
		
		/// <summary>
		/// The database connection for this service.
		/// </summary>
		protected IDbConnection Connection { get; set; }

		/// <summary>
		/// Constructor for this class.
		/// </summary>
		/// <param name="classes">
		/// The <see cref="IQueryLibrary[]"/> classes to use when building the query library.
		/// </param>
		public DatabaseService(params IQueryLibrary[] classes) {
			
			// Fill the query library.
			Queries = new Dictionary<string, DatabaseQuery>();
			foreach (IQueryLibrary library in classes) library.GetQueries(Queries);
			
			// Add database queries.
			(new DatabaseQueryLibrary()).GetQueries(Queries);
			
			// Set the name property in every query.
			foreach (KeyValuePair<string, DatabaseQuery> query in Queries) query.Value.Name = query.Key;
		}
		
		/// <summary>
		/// Get a set of data from the database
		/// </summary>
		/// <param name="queryName">
		/// The name of the query
		/// </param>
		/// <returns>
		/// A data reader with the data
		/// </returns>
		public IDataReader Select(string queryName) {
			return Select(queryName, new Dictionary<string, object>());
		}
		
		/// <summary>
		/// Get a set of data from the database
		/// </summary>
		/// <param name="queryName">
		/// The name of the query
		/// </param>
		/// <param name="parameters">
		/// Parameters to use when constructing the query
		/// </param>
		/// <returns>
		/// A data reader with the data
		/// </returns>
		public IDataReader Select(string queryName, Dictionary<string, object> parameters) {
			
			using (IDbCommand command = GetCommandForSelect(queryName, parameters)) {
				return command.ExecuteReader();
			}
		}
		
		/// <summary>
		/// Get a set of data from the database
		/// </summary>
		/// <param name="queryName">
		/// The name of the query
		/// </param>
		/// <param name="model">
		/// The <see cref="com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces.IDatabaseModel"/> model object
		/// to use for the query.
		/// </param>
		/// <returns>
		/// A data reader with the data
		/// </returns>
		public IDataReader Select(string queryName, IDatabaseModel model) {
			return Select(queryName, model.DataParameters());
		}
		
		/// <summary>
		/// Get a single result from the database
		/// </summary>
		/// <param name="queryName">
		/// The name of the query
		/// </param>
		/// <returns>
		/// A data reader with the data
		/// </returns>
		public IDataReader SelectOne(string queryName) {
			return SelectOne(queryName, new Dictionary<string, object>());
		}
		
		/// <summary>
		/// Get a single result from the database
		/// </summary>
		/// <param name="queryName">
		/// The name of the query
		/// </param>
		/// <param name="parameters">
		/// Parameters to use when constructing the query
		/// </param>
		/// <returns>
		/// A data reader with the data
		/// </returns>
		public IDataReader SelectOne(string queryName, Dictionary<string, object> parameters) {
			
			using (IDbCommand command = GetCommandForSelect(queryName, parameters)) {
				return command.ExecuteReader(CommandBehavior.SingleRow);
			}
		}
		
		/// <summary>
		/// Get a single result from the database
		/// </summary>
		/// <param name="queryName">
		/// The name of the query
		/// </param>
		/// <param name="model">
		/// The <see cref="com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces.IDatabaseModel"/> model object
		/// to use for the query.
		/// </param>
		/// <returns>
		/// A data reader with the data
		/// </returns>
		public IDataReader SelectOne(string queryName, IDatabaseModel model) {
			return SelectOne(queryName, model.DataParameters());
		}
		
		/// <summary>
		/// Create a command for a select query. 
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to retrieve
		/// </param>
		/// <param name="parameters">
		/// The parameters to use for the command
		/// </param>
		/// <returns>
		/// A command ready to execute
		/// </returns>
		/// <exception cref="System.NotSupportedException">
		/// If the query is not a SELECT statement
		/// </exception>
		private IDbCommand GetCommandForSelect(string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("SELECT")) {
				throw new NotSupportedException("Query " + queryName + " is not a select statement");
			}
			
			return MakeCommand(query, parameters);
		}

		/// <summary>
		/// Insert data
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to execute
		/// </param>
		/// <param name="parameters">
		/// The parameters to use for the command
		/// </param>
		/// <exception cref="System.NotSupportedException">
		/// If the query is not an INSERT statement
		/// </exception>
		public void Insert(string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("INSERT")) {
				throw new NotSupportedException("Query " + queryName + " is not an insert statement");
			}
			
			using (IDbCommand command = MakeCommand(query, parameters)) {
				command.ExecuteNonQuery();
			}
		}
		
		/// <summary>
		/// Insert data
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to execute
		/// </param>
		/// <param name="model">
		/// The <see cref="com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces.IDatabaseModel"/> model object
		/// to use for the query.
		/// </param>
		public void Insert(string queryName, IDatabaseModel model) {
			Insert(queryName, model.DataParameters());
		}
		
		/// <summary>
		/// Update data
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to execute
		/// </param>
		/// <param name="parameters">
		/// The parameters to use for the command
		/// </param>
		/// <exception cref="System.NotSupportedException">
		/// If the query is not an UPDATE statement
		/// </exception>
		public void Update(string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("UPDATE")) {
				throw new NotSupportedException("Query " + queryName + " is not an update statement");
			}
			
			using (IDbCommand command = MakeCommand(query, parameters)) {
				command.ExecuteNonQuery();
			}
		}
		
		/// <summary>
		/// Update data
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to execute
		/// </param>
		/// <param name="model">
		/// The <see cref="com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces.IDatabaseModel"/> model object
		/// to use for the query.
		/// </param>
		public void Update(string queryName, IDatabaseModel model) {
			Update(queryName, model.DataParameters());
		}
		
		/// <summary>
		/// Delete data
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to execute
		/// </param>
		/// <param name="parameters">
		/// The parameters to use for the command
		/// </param>
		/// <exception cref="System.NotSupportedException">
		/// If the query is not an DELETE statement
		/// </exception>
		public void Delete(string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("DELETE")) {
				throw new NotSupportedException("Query " + queryName + " is not a delete statement");
			}
			
			using (IDbCommand command = MakeCommand(query, parameters)) {
				command.ExecuteNonQuery();
			}
		}
		
		/// <summary>
		/// Delete data
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to execute
		/// </param>
		/// <param name="model">
		/// The <see cref="com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces.IDatabaseModel"/> model object
		/// to use for the query.
		/// </param>
		public void Delete(string queryName, IDatabaseModel model) {
			Delete(queryName, model.DataParameters());
		}
		
		/// <summary>
		/// Get a query from the library 
		/// </summary>
		/// <param name="queryName">
		/// The name of the query to retrieve
		/// </param>
		/// <returns>
		/// The database query
		/// </returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">
		/// Thrown when the query name is not found in the query library
		/// </exception>
		private DatabaseQuery GetQuery(string queryName) {
			
			if (Queries.ContainsKey(queryName)) return Queries[queryName];
			throw new KeyNotFoundException("Unable to find query " + queryName);
		}
		
		/// <summary>
		/// Create a command that is ready to be run 
		/// </summary>
		/// <param name="query">
		/// The database query to be executed
		/// </param>
		/// <param name="parameters">
		/// The parameters to use for this query
		/// </param>
		/// <returns>
		/// A command ready to execute
		/// </returns>
		private IDbCommand MakeCommand(DatabaseQuery query, Dictionary<string, object> parameters) {
			
			IDbCommand command = Connection.CreateCommand();
			command.CommandText = String.Copy(query.SQL);
			command.CommandType = CommandType.Text;
			
			if (null == parameters) return command;
			
			foreach (KeyValuePair<string, DbType> queryParameter in query.Parameters) {
				if (parameters.ContainsKey(queryParameter.Key)) {
					if (queryParameter.Key.StartsWith("[]")) {
						// Do a straight string replacement on this parameter.
						command.CommandText = command.CommandText.Replace(queryParameter.Key,
								parameters[queryParameter.Key].ToString());
					}
					else {
						// Bind the parameter.
						IDbDataParameter param = command.CreateParameter();
						param.ParameterName = queryParameter.Key;
						param.DbType = queryParameter.Value;
						param.Value = parameters[queryParameter.Key];
						command.Parameters.Add(param);
					}
				}
			}
			
			return command;
		}
	}
}