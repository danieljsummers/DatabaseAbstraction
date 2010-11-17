using System;
using System.Collections.Generic;
using System.Data;
using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;
using com.codeplex.dbabstraction.DatabaseAbstraction.Models;
using com.codeplex.dbabstraction.DatabaseAbstraction.Queries;

namespace com.codeplex.dbabstraction.DatabaseAbstraction {
	/// <summary>
	/// Mock implementation of a database service
	/// </summary>
	/// <remarks>
	/// This is designed to be used for unit tests.  Assuming the code uses
	/// dependency injection for the IDatabaseService interface, this
	/// implementation can be substituted for the actual database.  Rather
	/// than actually storing data, it accumulates the query names and
	/// parameters passed to it, and provides a means to retrieve these for
	/// verification.  Remember that, unless instantiated statically, each
	/// test run will obtain a new version of this service.
	/// 
	/// It will create the query library the same way that the actual
	/// implementation do, and check for validity; a missing query will throw
	/// a KeyNotFoundException, and a mismatch (ex. a "delete" query that
	/// starts with "SELECT") will throw a NotSupportedException.
	/// 
	/// (This methodology differs from the "expect" functionality of some other
	/// mock frameworks; instead of setting up expectations and then executing
	/// the test, the workflow is to execute the test, then check that the
	/// right quer(y|ies) were executed and the correct parameters were passed.
	/// See the #Assertions region for convenience methods to do this.)
	/// </remarks>
	public class MockDatabaseService : IDatabaseService {
		
		#region Properties
		
		/// <summary>
		/// Query library
		/// </summary>
		private Dictionary<string, DatabaseQuery> Queries { get; set; }
		
		/// <summary>
		/// Executed queries and their parameters
		/// </summary>
		private List<ExecutedQuery> ExecutedQueries { get; set; }
		
		#endregion
		
		#region Constructor
		
		/// <summary>
		/// Construct the database service
		/// </summary>
		/// <param name="classes">
		/// The <see cref="IQueryLibrary[]"/> classes to use when building the query library
		/// </param>
		public MockDatabaseService(params IQueryLibrary[] classes) {
			
			// Fill the query library.
			Queries = new Dictionary<string, DatabaseQuery>();
			foreach (IQueryLibrary library in classes) library.GetQueries(Queries);
			
			// Add database queries.
			(new DatabaseQueryLibrary()).GetQueries(Queries);
			
			// Set the name property in every query.
			foreach (KeyValuePair<string, DatabaseQuery> query in Queries) query.Value.Name = query.Key;
		}
		
		#endregion
		
		#region Interface Implementation
		
		public IDataReader Select (string queryName) {
			return Select(queryName, new Dictionary<string, object>());
		}

		public IDataReader Select (string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("SELECT")) {
				throw new NotSupportedException("Query " + queryName + " is not a select statement");
			}
			
			RecordQuery(queryName, "select", parameters);
			
			return null;
		}

		public IDataReader Select (string queryName, IDatabaseModel model) {
			return Select(queryName, model.DataParameters());
		}

		public IDataReader SelectOne (string queryName) {
			return Select(queryName, new Dictionary<string, object>());
		}

		public IDataReader SelectOne (string queryName, Dictionary<string, object> parameters) {
			return Select(queryName, parameters);
		}

		public IDataReader SelectOne (string queryName, IDatabaseModel model) {
			return Select(queryName, model.DataParameters());
		}

		public void Insert (string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("INSERT")) {
				throw new NotSupportedException("Query " + queryName + " is not an insert statement");
			}
			
			RecordQuery(queryName, "insert", parameters);
		}

		public void Insert (string queryName, IDatabaseModel model) {
			Insert(queryName, model.DataParameters());
		}

		public void Update (string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("UPDATE")) {
				throw new NotSupportedException("Query " + queryName + " is not an update statement");
			}
			
			RecordQuery(queryName, "update", parameters);
		}

		public void Update (string queryName, IDatabaseModel model) {
			Update(queryName, model.DataParameters());
		}

		public void Delete (string queryName, Dictionary<string, object> parameters) {
			
			DatabaseQuery query = GetQuery(queryName);
			
			if (!query.SQL.ToUpper().StartsWith("DELETE")) {
				throw new NotSupportedException("Query " + queryName + " is not a delete statement");
			}
			
			RecordQuery(queryName, "delete", parameters);
		}

		public void Delete (string queryName, IDatabaseModel model) {
			Delete(queryName, model.DataParameters());
		}

		public int Sequence (string sequenceName) {
			RecordQuery(sequenceName, "sequence", null);
			return -1;
		}
		
		private void RecordQuery(string queryName, string queryType, Dictionary<string, object> parameters) {
			ExecutedQueries.Add(new ExecutedQuery {
				QueryName = queryName, QueryType = queryType, Parameters = parameters });
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

		#endregion
		
		#region Assertions
		
		public void AssertPerformed(string queryName) {
			// TODO: stopped work here...
		}
		
		#endregion
		
		#region Structs
		
		/// <summary>
		/// This represents an executed query.
		/// </summary>
		private struct ExecutedQuery {
			
			/// <summary>
			/// The name of the executed query
			/// </summary>
			public string QueryName { get; set; }
			
			/// <summary>
			/// The type of the executed query (select|insert|update|delete|sequence)
			/// </summary>
			public string QueryType { get; set; }
			
			/// <summary>
			/// The parameters passed with the executed query
			/// </summary>
			public Dictionary<string, object> Parameters { get; set; }
		}
		
		#endregion
	}
}