namespace DatabaseAbstraction
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using DatabaseAbstraction.Utils.UnitTest;
    using NUnit.Framework;

    #endregion

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
    /// It will create the query library the same way that the actual implementations do, and check for validity; a
    /// missing query will throw a KeyNotFoundException, and a mismatch (ex. a "delete" query that starts with
    /// "SELECT") will throw a NotSupportedException.
    /// 
    /// Thanks to Phil Haack and the Subtext project, this now has the ability to return data.  Set up a StubDataReader
    /// with one or more StubResultSet objects; each call to either Select() or SelectOne() will advance this to the
    /// next result set.  Keep in mind that there is only one instance per class, so once the reader is advanced to the
    /// next result set, the data in the previous ones is gone.
    /// 
    /// (This methodology differs from the "expect" functionality of some other mock frameworks; instead of setting up
    /// expectations and then executing the test, the workflow is to execute the test, then check that the right
    /// quer(y|ies) were executed and the correct parameters were passed.  See the Assertions region for convenience
    /// methods to do this.)
    /// 
    /// See the Documentation wiki at http://dbabstraction.codeplex.com for full documentation on unit testing. (soon)
    /// </remarks>
    public class MockDatabaseService : IDatabaseService
    {
        #region Properties

        /// <summary>
        /// Query library
        /// </summary>
        private Dictionary<string, DatabaseQuery> Queries { get; set; }

        /// <summary>
        /// Executed queries and their parameters
        /// </summary>
        private List<ExecutedQuery> ExecutedQueries { get; set; }

        /// <summary>
        /// The data returned by this instance
        /// </summary>
        private StubDataReader Data { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Construct the database service
        /// </summary>
        /// <param name="classes">
        /// The <see cref="IQueryLibrary"/> classes to use when building the query library
        /// </param>
        public MockDatabaseService(StubDataReader data, params IQueryLibrary[] classes)
        {
            Data = data;

            // Fill the query library
            Queries = new Dictionary<string, DatabaseQuery>();
            foreach (IQueryLibrary library in classes) library.GetQueries(Queries);

            // Add database queries
            (new DatabaseQueryLibrary()).GetQueries(Queries);

            // Set the name property in every query
            foreach (KeyValuePair<string, DatabaseQuery> query in Queries) query.Value.Name = query.Key;

            // Initialize the executed query list
            ExecutedQueries = new List<ExecutedQuery>();
        }

        #endregion

        #region Interface Implementation

        public IDataReader Select(string queryName)
        {
            return Select(queryName, new Dictionary<string, object>());
        }

        public IDataReader Select(string queryName, Dictionary<string, object> parameters)
        {
            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("SELECT"))
                throw new NotSupportedException(String.Format("Query {0} is not a select statement", queryName));

            RecordQuery(queryName, "select", parameters);

            if (Data.NextResult())
                return Data;

            return null;
        }

        public IDataReader Select(string queryName, IDatabaseModel model)
        {
            return Select(queryName, model.DataParameters());
        }

        public IDataReader SelectOne(string queryName)
        {
            return Select(queryName, new Dictionary<string, object>());
        }

        public IDataReader SelectOne(string queryName, Dictionary<string, object> parameters)
        {
            return Select(queryName, parameters);
        }

        public IDataReader SelectOne(string queryName, IDatabaseModel model)
        {
            return Select(queryName, model.DataParameters());
        }

        public void Insert(string queryName, Dictionary<string, object> parameters)
        {
            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("INSERT"))
                throw new NotSupportedException(String.Format("Query {0} is not an insert statement", queryName));

            RecordQuery(queryName, "insert", parameters);
        }

        public void Insert(string queryName, IDatabaseModel model)
        {
            Insert(queryName, model.DataParameters());
        }

        public void Update(string queryName, Dictionary<string, object> parameters)
        {
            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("UPDATE"))
                throw new NotSupportedException(String.Format("Query {0} is not an update statement", queryName));

            RecordQuery(queryName, "update", parameters);
        }

        public void Update(string queryName, IDatabaseModel model)
        {
            Update(queryName, model.DataParameters());
        }

        public void Delete(string queryName, Dictionary<string, object> parameters)
        {
            DatabaseQuery query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("DELETE"))
                throw new NotSupportedException(String.Format("Query {0} is not a delete statement", queryName));

            RecordQuery(queryName, "delete", parameters);
        }

        public void Delete(string queryName, IDatabaseModel model)
        {
            Delete(queryName, model.DataParameters());
        }

        public int Sequence(string sequenceName)
        {
            RecordQuery(sequenceName, "sequence", null);
            return -1;
        }

        private void RecordQuery(string queryName, string queryType, Dictionary<string, object> parameters)
        {
            ExecutedQueries.Add(new ExecutedQuery
            {
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
        private DatabaseQuery GetQuery(string queryName)
        {
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
        public void AssertPerformed(string queryName)
        {
            if (0 == GetExecutedQueries(queryName).Count())
                Assert.Fail(String.Format("Query {0} was not performed", queryName));
        }

        /// <summary>
        /// Assert that the given SELECT query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedSelect(string queryName)
        {
            AssertPerformedType(queryName, "select");
        }

        /// <summary>
        /// Assert that the given INSERT query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedInsert(string queryName)
        {
            AssertPerformedType(queryName, "insert");
        }

        /// <summary>
        /// Assert that the given UPDATE query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedUpdate(string queryName)
        {
            AssertPerformedType(queryName, "update");
        }

        /// <summary>
        /// Assert that the given DELETE query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedDelete(string queryName)
        {
            AssertPerformedType(queryName, "delete");
        }

        /// <summary>
        /// Assert that the given sequence query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedSequence(string queryName)
        {
            AssertPerformedType(queryName, "sequence");
        }

        /// <summary>
        /// Assert that the given query of the given type has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="type">
        /// The type of the query that should have been executed
        /// </param>
        private void AssertPerformedType(string queryName, string type)
        {
            if (0 == GetExecutedQueries(queryName, type).Count())
                Assert.Fail(String.Format("{0} Query {1} was not performed", Capitalize(type), queryName));
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
        public void AssertPerformed(string queryName, int times)
        {
            IEnumerable<ExecutedQuery> queries = GetExecutedQueries(queryName);
            if (times != queries.Count())
                Assert.Fail(String.Format("Query {0} executed {1} time(s), but should have been executed {2} time(s)",
                    queryName, queries.Count(), times));
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
        public void AssertPerformedSelect(string queryName, int times)
        {
            AssertPerformedType(queryName, "select", times);
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
        public void AssertPerformedInsert(string queryName, int times)
        {
            AssertPerformedType(queryName, "insert", times);
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
        public void AssertPerformedUpdate(string queryName, int times)
        {
            AssertPerformedType(queryName, "update", times);
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
        public void AssertPerformedDelete(string queryName, int times)
        {
            AssertPerformedType(queryName, "delete", times);
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
        public void AssertPerformedSequence(string queryName, int times)
        {
            AssertPerformedType(queryName, "sequence", times);
        }

        /// <summary>
        /// Assert that the given query of the given type has been executed a specific number of times
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="type">
        /// The type of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        private void AssertPerformedType(string queryName, string type, int times)
        {
            var queries = GetExecutedQueries(queryName, type);
            if (times != queries.Count())
                Assert.Fail(String.Format("{0} Query {1} executed {2} time(s), but should have been executed {3} time(s)",
                    Capitalize(type), queryName, queries.Count(), times));
        }

        /// <summary>
        /// Assert that a query was performed with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformed(string queryName, Dictionary<string, object> parameters)
        {
            var queries = GetExecutedQueries(queryName);
            if (!FindQueryWithParameters(parameters, queries))
                Assert.Fail(String.Format(
                    "The passed query parameters for query {0} were not found in any of the {1} execution(s) of that query.",
                    queryName, queries.Count()));
        }

        /// <summary>
        /// Assert that a SELECT query was performed with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedSelect(string queryName, Dictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, "select", parameters);
        }

        /// <summary>
        /// Assert that a INSERT query was performed with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedInsert(string queryName, Dictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, "insert", parameters);
        }

        /// <summary>
        /// Assert that a UPDATE query was performed with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedUpdate(string queryName, Dictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, "update", parameters);
        }

        /// <summary>
        /// Assert that a DELETE query was performed with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedDelete(string queryName, Dictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, "delete", parameters);
        }

        /// <summary>
        /// Assert that a sequence query was performed with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedSequence(string queryName, Dictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, "sequence", parameters);
        }

        /// <summary>
        /// Assert that a given query of a given type has been execute with the given parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="type">
        /// The type of the query that should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters that should have been passed to the query
        /// </param>
        private void AssertPerformedType(string queryName, string type, Dictionary<string, object> parameters)
        {
            var queries = GetExecutedQueries(queryName, type);
            if (!FindQueryWithParameters(parameters, queries))
                Assert.Fail(String.Format(
                    "The passed query parameters for {0} query {1} were not found in any of the {2} execution(s) of that query.",
                    Capitalize(type), queryName, queries.Count()));
        }
        
        /// <summary>
        /// Check to see that a query contains the given parameters
        /// </summary>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        /// <param name="query">
        /// The executed query record
        /// </param>
        /// <returns>
        /// True if they match, false if not
        /// </returns>
        private bool FindQueryWithParameters(Dictionary<string, object> parameters, IEnumerable<ExecutedQuery> queries)
        {
            foreach (ExecutedQuery query in queries)
            {
                bool good = true;

                if (query.Parameters.Count != parameters.Count)
                    continue;

                for (int index = 0; index < parameters.Count; index++)
                {
                    if ((!query.Parameters.ElementAt(index).Key.Equals(parameters.ElementAt(index).Key))
                        || (!query.Parameters.ElementAt(index).Value.Equals(parameters.ElementAt(index).Value)))
                    {
                        good = false;
                        break;
                    }
                }

                if (good) return true;
            }

            return false;
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
        private IEnumerable<ExecutedQuery> GetExecutedQueries(string queryName)
        {
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
        private IEnumerable<ExecutedQuery> GetExecutedQueries(string queryName, string queryType)
        {
            return from query in ExecutedQueries
                   where query.QueryName == queryName && query.QueryType == queryType
                   select query;
        }

        #endregion

        #region Structs

        /// <summary>
        /// This represents an executed query.
        /// </summary>
        private struct ExecutedQuery
        {
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

        #region Util

        /// <summary>
        /// Capitalize the first letter in the given string
        /// </summary>
        /// <param name="theString">
        /// The string to capitalize
        /// </param>
        /// <returns>
        /// The capitalized string
        /// </returns>
        private string Capitalize(string theString)
        {
            char[] chars = theString.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }

        #endregion
    }
}