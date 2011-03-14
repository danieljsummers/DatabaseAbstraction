namespace com.codeplex.dbabstraction.DatabaseAbstraction {

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;
    using com.codeplex.dbabstraction.DatabaseAbstraction.Models;
    using com.codeplex.dbabstraction.DatabaseAbstraction.Queries;
    using NUnit.Framework;

    /// <summary>
    /// Mock implementation of a database service
    /// </summary>
    /// <remarks>
    /// This is designed to be used for NUnit-based unit tests.  Assuming the code uses dependency injection for the
    /// IDatabaseService interface, this implementation can be substituted for the actual database.  Rather than
    /// actually storing data, it accumulates the query names and parameters passed to it, and provides a means to
    /// retrieve these for verification.  Remember that, unless instantiated statically, each test run will obtain a
    /// new version of this service.
    /// 
    /// It will create the query library the same way that the actual implementation do, and check for validity; a
    /// missing query will throw a KeyNotFoundException, and a mismatch (ex. a "delete" query that starts with
    /// "SELECT") will throw a NotSupportedException.
    /// 
    /// (This methodology differs from the "expect" functionality of some other mock frameworks; instead of setting up
    /// expectations and then executing the test, the workflow is to execute the test, then check that the right
    /// quer(y|ies) were executed and the correct parameters were passed.  See the Assertions region for convenience
    /// methods to do this.)
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

        public IDataReader Select(string queryName) {
            return Select(queryName, new Dictionary<string, object>());
        }

        public IDataReader Select(string queryName, Dictionary<string, object> parameters) {

            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("SELECT")) {
                throw new NotSupportedException("Query " + queryName + " is not a select statement");
            }

            RecordQuery(queryName, "select", parameters);

            return null;
        }

        public IDataReader Select(string queryName, IDatabaseModel model) {
            return Select(queryName, model.DataParameters());
        }

        public IDataReader SelectOne(string queryName) {
            return Select(queryName, new Dictionary<string, object>());
        }

        public IDataReader SelectOne(string queryName, Dictionary<string, object> parameters) {
            return Select(queryName, parameters);
        }

        public IDataReader SelectOne(string queryName, IDatabaseModel model) {
            return Select(queryName, model.DataParameters());
        }

        public void Insert(string queryName, Dictionary<string, object> parameters) {

            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("INSERT")) {
                throw new NotSupportedException("Query " + queryName + " is not an insert statement");
            }

            RecordQuery(queryName, "insert", parameters);
        }

        public void Insert(string queryName, IDatabaseModel model) {
            Insert(queryName, model.DataParameters());
        }

        public void Update(string queryName, Dictionary<string, object> parameters) {

            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("UPDATE")) {
                throw new NotSupportedException("Query " + queryName + " is not an update statement");
            }

            RecordQuery(queryName, "update", parameters);
        }

        public void Update(string queryName, IDatabaseModel model) {
            Update(queryName, model.DataParameters());
        }

        public void Delete(string queryName, Dictionary<string, object> parameters) {

            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("DELETE")) {
                throw new NotSupportedException("Query " + queryName + " is not a delete statement");
            }

            RecordQuery(queryName, "delete", parameters);
        }

        public void Delete(string queryName, IDatabaseModel model) {
            Delete(queryName, model.DataParameters());
        }

        public int Sequence(string sequenceName) {
            RecordQuery(sequenceName, "sequence", null);
            return -1;
        }

        private void RecordQuery(string queryName, string queryType, Dictionary<string, object> parameters) {
            ExecutedQueries.Add(new ExecutedQuery {
                QueryName = queryName,
                QueryType = queryType,
                Parameters = parameters
            });
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
        /// Implementation of the dispose method; nothing to dispose since there is no real connection
        /// </summary>
        public void Dispose() { }

        #endregion

        #region Assertions

        /// <summary>
        /// Assert that the given query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformed(string queryName) {
            if (0 == GetExecutedQueries(queryName).Count()) {
                Assert.Fail("Query " + queryName + " was not performed");
            }
        }

        /// <summary>
        /// Assert that the given SELECT query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedSelect(string queryName) {
            if (0 == GetExecutedQueries(queryName, "select").Count()) {
                Assert.Fail("SELECT Query " + queryName + " was not performed");
            }
        }

        /// <summary>
        /// Assert that the given INSERT query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedInsert(string queryName) {
            if (0 == GetExecutedQueries(queryName, "insert").Count()) {
                Assert.Fail("INSERT Query " + queryName + " was not performed");
            }
        }

        /// <summary>
        /// Assert that the given UPDATE query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedUpdate(string queryName) {
            if (0 == GetExecutedQueries(queryName, "update").Count()) {
                Assert.Fail("UPDATE Query " + queryName + " was not performed");
            }
        }

        /// <summary>
        /// Assert that the given DELETE query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedDelete(string queryName) {
            if (0 == GetExecutedQueries(queryName, "delete").Count()) {
                Assert.Fail("DELETE Query " + queryName + " was not performed");
            }
        }

        /// <summary>
        /// Assert that the given sequence query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedSequence(string queryName) {
            if (0 == GetExecutedQueries(queryName, "sequence").Count()) {
                Assert.Fail("Sequence Query " + queryName + " was not performed");
            }
        }

        /// <summary>
        /// Assert that the given query has been executed a specific number of times
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        public void AssertPerformed(string queryName, int times) {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName);
            if (times != queries.Count()) {
                Assert.Fail("Query " + queryName + " executed " + queries.Count()
                        + " times, but should have been executed " + times + " times");
            }
        }

        /// <summary>
        /// Assert that the given SELECT query has been executed a specific number of times
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        public void AssertPerformedSelect(string queryName, int times) {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName, "select");
            if (times != queries.Count()) {
                Assert.Fail("SELECT Query " + queryName + " executed " + queries.Count()
                        + " times, but should have been executed " + times + " times");
            }
        }

        /// <summary>
        /// Assert that the given INSERT query has been executed a specific number of times
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        public void AssertPerformedInsert(string queryName, int times) {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName, "insert");
            if (times != queries.Count()) {
                Assert.Fail("INSERT Query " + queryName + " executed " + queries.Count()
                        + " times, but should have been executed " + times + " times");
            }
        }

        /// <summary>
        /// Assert that the given UPDATE query has been executed a specific number of times
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        public void AssertPerformedUpdate(string queryName, int times) {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName, "update");
            if (times != queries.Count()) {
                Assert.Fail("UPDATE Query " + queryName + " executed " + queries.Count()
                        + " times, but should have been executed " + times + " times");
            }
        }

        /// <summary>
        /// Assert that the given DELETE query has been executed a specific number of times
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        public void AssertPerformedDelete(string queryName, int times) {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName, "delete");
            if (times != queries.Count()) {
                Assert.Fail("DELETE Query " + queryName + " executed " + queries.Count()
                        + " times, but should have been executed " + times + " times");
            }
        }

        /// <summary>
        /// Assert that the given sequence query has been executed a specific number of times
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        public void AssertPerformedSequence(string queryName, int times) {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName, "sequence");
            if (times != queries.Count()) {
                Assert.Fail("Sequence Query " + queryName + " executed " + queries.Count()
                        + " times, but should have been executed " + times + " times");
            }
        }

        public void AssertPerformed(string queryName, Dictionary<string, object> parameters) {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName);
            bool found = false;
            foreach (ExecutedQuery query in queries) {
                found = ParametersExist(parameters, query);
                if (found) break;
            }
            if (!found) {
                Assert.Fail("The passed query parameters for query " + queryName + " were not found in any of the "
                            + queries.Count() + " execution(s) of that query.");
            }
        }
        // TODO:stopped here
        /// <summary>
        /// Check to see that a query in the 
        /// </summary>
        /// <param name="queries">
        /// A <see cref="IEnumerable<ExecutedQueries>"/>
        /// </param>
        /// <param name="parameters">
        /// A <see cref="Dictionary<System.String, System.Object>"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        private bool ParametersExist(Dictionary<string, object> parameters, ExecutedQuery query) {
            // FIXME: fake
            return true;
        }

        /// <summary>
        /// Retrieve the executed queries for a given query name
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <returns>
        /// A set of executed queries
        /// </returns>
        private IEnumerable<ExecutedQuery> GetExecutedQueries(string queryName) {
            return from query in ExecutedQueries
                   where query.QueryName == queryName
                   select query;
        }

        /// <summary>
        /// Retrieve the executed queries for a given query name and type
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="queryType">
        /// The query type (select|insert|update|delete|sequence)
        /// </param>
        /// <returns>
        /// A set of executed queries
        /// </returns>
        private IEnumerable<ExecutedQuery> GetExecutedQueries(string queryName, string queryType) {
            return from query in ExecutedQueries
                   where query.QueryName == queryName && query.QueryType == queryType
                   select query;
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