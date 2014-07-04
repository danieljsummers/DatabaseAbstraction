namespace DatabaseAbstraction
{
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using DatabaseAbstraction.Utils.UnitTest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    /// <summary>
    /// Mock implementation of a database service
    /// </summary>
    /// <remarks>
    /// This is designed to be used for Microsoft-based unit tests.  Assuming the code uses dependency injection for the
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
    public class MockDatabaseService : DatabaseService, IDatabaseService
    {
        #region Properties

        /// <summary>
        /// Executed queries and their parameters
        /// </summary>
        private IList<ExecutedQuery> ExecutedQueries { get; set; }

        /// <summary>
        /// The data returned by this instance
        /// </summary>
        public StubDataReader Data { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct the database service
        /// </summary>
        /// <param name="data">
        /// The <see cref="StubDataReader"/> with data to use for query executions
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public MockDatabaseService(StubDataReader data, params Type[] providers)
        {
            Data = data;
            Queries = new Dictionary<string, DatabaseQuery>();
            ExecutedQueries = new List<ExecutedQuery>();

            DatabaseService.FillQueryLibrary(Queries, providers);

            if (!Queries.ContainsKey("database.sequence.generic"))
            {
                DatabaseService.FillQueryLibrary(Queries, typeof(DatabaseQueryProvider));
            }
        }

        #endregion

        #region Interface Implementation

        public override IDataReader Select(string queryName)
        {
            return Select(queryName, new Dictionary<string, object>());
        }

        public override IDataReader Select(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("SELECT"))
            {
                throw new NotSupportedException(String.Format("Query {0} is not a select statement", queryName));
            }

            RecordQuery(queryName, QueryType.Select, parameters);

            if (Data.NextResult()) { return Data; }

            return new StubDataReader(new StubResultSet());
        }

        public override IDataReader Select(string queryName, IParameterProvider model)
        {
            return Select(queryName, model.Parameters());
        }

        public override IDataReader SelectOne(string queryName)
        {
            return Select(queryName, new Dictionary<string, object>());
        }

        public override IDataReader SelectOne(string queryName, IDictionary<string, object> parameters)
        {
            return Select(queryName, parameters);
        }

        public override IDataReader SelectOne(string queryName, IParameterProvider model)
        {
            return Select(queryName, model.Parameters());
        }

        public override void Insert(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("INSERT"))
            {
                throw new NotSupportedException(String.Format("Query {0} is not an insert statement", queryName));
            }

            RecordQuery(queryName, QueryType.Insert, parameters);
        }

        public override void Insert(string queryName, IParameterProvider model)
        {
            Insert(queryName, model.Parameters());
        }

        public override void Update(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("UPDATE"))
            {
                throw new NotSupportedException(String.Format("Query {0} is not an update statement", queryName));
            }

            RecordQuery(queryName, QueryType.Update, parameters);
        }

        public override void Update(string queryName, IParameterProvider model)
        {
            Update(queryName, model.Parameters());
        }

        public override void Delete(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("DELETE"))
            {
                throw new NotSupportedException(String.Format("Query {0} is not a delete statement", queryName));
            }

            RecordQuery(queryName, QueryType.Delete, parameters);
        }

        public override void Delete(string queryName, IParameterProvider model)
        {
            Delete(queryName, model.Parameters());
        }

        public override int Sequence(string sequenceName)
        {
            RecordQuery(sequenceName, QueryType.Sequence, null);
            return -1;
        }

        public override long LongSequence(string sequenceName)
        {
            RecordQuery(sequenceName, QueryType.Sequence, null);
            return -1L;
        }

        public override int LastIdentity()
        {
            RecordQuery("", QueryType.Identity, null);
            return -1;
        }

        public override long LongLastIdentity()
        {
            RecordQuery("", QueryType.Identity, null);
            return -1L;
        }

        /// <summary>
        /// Record a query execution
        /// </summary>
        /// <param name="queryName">
        /// The name of the query being executed
        /// </param>
        /// <param name="queryType">
        /// The type of the query being executed
        /// </param>
        /// <param name="parameters">
        /// The parameter with which the query was executed
        /// </param>
        private void RecordQuery(string queryName, QueryType queryType, IDictionary<string, object> parameters)
        {
            ExecutedQueries.Add(new ExecutedQuery
            {
                QueryName = queryName,
                QueryType = queryType,
                Parameters = parameters
            });
        }

        /// <summary>
        /// Get the currently-defined queries
        /// </summary>
        /// <returns>
        /// The currently-defined queries
        /// </returns>
        public IDictionary<string, DatabaseQuery> GetQueries()
        {
            return Queries;
        }

        /// <summary>
        /// Implementation of the dispose method; nothing to dispose since there is no real connection
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

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
            AssertPerformedType(queryName, QueryType.Select);
        }

        /// <summary>
        /// Assert that the given INSERT query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedInsert(string queryName)
        {
            AssertPerformedType(queryName, QueryType.Insert);
        }

        /// <summary>
        /// Assert that the given UPDATE query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedUpdate(string queryName)
        {
            AssertPerformedType(queryName, QueryType.Update);
        }

        /// <summary>
        /// Assert that the given DELETE query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedDelete(string queryName)
        {
            AssertPerformedType(queryName, QueryType.Delete);
        }

        /// <summary>
        /// Assert that the given sequence query has been performed at least once
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        public void AssertPerformedSequence(string queryName)
        {
            AssertPerformedType(queryName, QueryType.Sequence);
        }

        /// <summary>
        /// Assert that an identity query has been performed at least once
        /// </summary>
        public void AssertPerformedIdentity()
        {
            AssertPerformedType("", QueryType.Identity);
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
        private void AssertPerformedType(string queryName, QueryType type)
        {
            if (0 == GetExecutedQueries(queryName, type).Count())
                Assert.Fail(String.Format("{0} Query {1} was not performed", Enum.GetName(typeof(QueryType), type),
                    queryName));
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
            var queries = GetExecutedQueries(queryName);
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
            AssertPerformedType(queryName, QueryType.Select, times);
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
            AssertPerformedType(queryName, QueryType.Insert, times);
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
            AssertPerformedType(queryName, QueryType.Update, times);
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
            AssertPerformedType(queryName, QueryType.Delete, times);
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
            AssertPerformedType(queryName, QueryType.Sequence, times);
        }

        /// <summary>
        /// Assert that an identity query has been executed a specific number of times
        /// </summary>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        public void AssertPerformedIdentity(int times)
        {
            AssertPerformedType("", QueryType.Identity, times);
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
        private void AssertPerformedType(string queryName, QueryType type, int times)
        {
            var queries = GetExecutedQueries(queryName, type);
            if (times != queries.Count())
                Assert.Fail(String.Format("{0} Query {1} executed {2} time(s), but should have been executed {3} time(s)",
                    Enum.GetName(typeof(QueryType), type), queryName, queries.Count(), times));
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
        public void AssertPerformed(string queryName, IDictionary<string, object> parameters)
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
        public void AssertPerformedSelect(string queryName, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Select, parameters);
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
        public void AssertPerformedInsert(string queryName, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Insert, parameters);
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
        public void AssertPerformedUpdate(string queryName, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Update, parameters);
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
        public void AssertPerformedDelete(string queryName, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Delete, parameters);
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
        private void AssertPerformedType(string queryName, QueryType type, IDictionary<string, object> parameters)
        {
            var queries = GetExecutedQueries(queryName, type);
            if (!FindQueryWithParameters(parameters, queries))
                Assert.Fail(String.Format(
                    "The passed query parameters for {0} query {1} were not found in any of the {2} execution(s) of that query.",
                    Enum.GetName(typeof(QueryType), type), queryName, queries.Count()));
        }

        /// <summary>
        /// Assert that a given query has been executed a given number of times with the given parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformed(string queryName, int times, IDictionary<string, object> parameters)
        {
            var matches = CountQueryWithParameters(parameters, GetExecutedQueries(queryName));
            if (times != matches)
                Assert.Fail(String.Format(
                    "Query {0} with parameters executed {1} time(s), but should have been executed {2} time(s)",
                    queryName, matches, times));
        }

        /// <summary>
        /// Assert that a SELECT query was performed a given number of times with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedSelect(string queryName, int times, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Select, times, parameters);
        }

        /// <summary>
        /// Assert that a INSERT query was performed a given number of times with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedInsert(string queryName, int times, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Insert, times, parameters);
        }

        /// <summary>
        /// Assert that a UPDATE query was performed a given number of times with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedUpdate(string queryName, int times, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Update, times, parameters);
        }

        /// <summary>
        /// Assert that a DELETE query was performed a given number of times with the specified parameters
        /// </summary>
        /// <param name="queryName">
        /// The name of the query that should have been executed
        /// </param>
        /// <param name="times">
        /// The number of times the query should have been executed
        /// </param>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        public void AssertPerformedDelete(string queryName, int times, IDictionary<string, object> parameters)
        {
            AssertPerformedType(queryName, QueryType.Delete, times, parameters);
        }

        /// <summary>
        /// Assert that a given query of a given type has been executed a given number of times with the given
        /// parameters
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
        /// <param name="parameters">
        /// The parameters that should have been passed to the query
        /// </param>
        private void AssertPerformedType(string queryName, QueryType type, int times,
            IDictionary<string, object> parameters)
        {
            var matches = CountQueryWithParameters(parameters, GetExecutedQueries(queryName, type));
            if (times != matches)
                Assert.Fail(String.Format(
                    "{0} Query {1} with parameters executed {2} time(s), but should have been executed {3} time(s)",
                    Enum.GetName(typeof(QueryType), type), queryName, matches, times));
        }

        /// <summary>
        /// Check to see that a query contains the given parameters
        /// </summary>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        /// <param name="queries">
        /// The executed query records
        /// </param>
        /// <returns>
        /// True if they match, false if not
        /// </returns>
        private bool FindQueryWithParameters(IDictionary<string, object> parameters, IEnumerable<ExecutedQuery> queries)
        {
            return 0 < CountQueryWithParameters(parameters, queries);
        }

        /// <summary>
        /// Count the number of query executions with the given parameters
        /// </summary>
        /// <param name="parameters">
        /// The parameters with which the query should have been executed
        /// </param>
        /// <param name="queries">
        /// The executed query records
        /// </param>
        /// <returns>
        /// The number of matches found
        /// </returns>
        private int CountQueryWithParameters(IDictionary<string, object> parameters, IEnumerable<ExecutedQuery> queries)
        {
            int matches = 0;

            foreach (var query in queries)
            {
                bool good = true;

                if ((null == query.Parameters) || (query.Parameters.Count != parameters.Count))
                    continue;

                for (int index = 0; index < parameters.Count; index++)
                    if ((!String.Equals(parameters.ElementAt(index).Key, query.Parameters.ElementAt(index).Key))
                        || (!Object.Equals(parameters.ElementAt(index).Value, query.Parameters.ElementAt(index).Value)))
                    {
                        good = false;
                        break;
                    }

                if (good)
                    matches++;
            }

            return matches;
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
        private IEnumerable<ExecutedQuery> GetExecutedQueries(string queryName, QueryType queryType)
        {
            return from query in ExecutedQueries
                   where query.QueryName == queryName && query.QueryType == queryType
                   select query;
        }

        #endregion

        #region Structs and Enums

        /// <summary>
        /// The type of query that was executed
        /// </summary>
        private enum QueryType
        {
            Select,
            Insert,
            Update,
            Delete,
            Sequence,
            Identity
        };

        /// <summary>
        /// An executed query
        /// </summary>
        private struct ExecutedQuery
        {
            /// <summary>
            /// The name of the executed query
            /// </summary>
            public string QueryName { get; set; }

            /// <summary>
            /// The type of the executed query
            /// </summary>
            public QueryType QueryType { get; set; }

            /// <summary>
            /// The parameters passed with the executed query
            /// </summary>
            public IDictionary<string, object> Parameters { get; set; }
        }

        #endregion

    }
}