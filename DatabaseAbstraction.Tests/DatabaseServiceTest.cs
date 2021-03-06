﻿namespace DatabaseAbstraction.Tests
{
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Unit Tests for the DatabaseService class
    /// </summary>
    [TestClass]
    public class DatabaseServiceTest
    {
        #region Tests

        /// <summary>
        /// Clear the static query library (removes database queries)
        /// </summary>
        [TestInitialize]
        [TestCleanup]
        public void ClearStaticQueryLibrary()
        {
            TestDatabaseService.StaticQueries.Clear();
        }

        /// <summary>
        /// Test the FillStaticQueryLibrary() method
        /// </summary>
        [TestMethod]
        public void DatabaseService_FillStaticQueryLibrary_Success()
        {
            // Query Library Only
            TestDatabaseService.FillStaticQueryLibrary(typeof(TestDatabaseQueryProvider));

            Assert.IsNotNull(TestDatabaseService.StaticQueries, "The static queries should not have been null");
            Assert.AreEqual(2, TestDatabaseService.StaticQueries.Count, "There should be two queries in the library");
            Assert.IsTrue(TestDatabaseService.StaticQueries.ContainsKey("test1.query1"), "Query 1 not found");
            Assert.IsTrue(TestDatabaseService.StaticQueries.ContainsKey("test1.query2"), "Query 2 not found");

            // Query Library / Fragment Library specified
            ClearStaticQueryLibrary();

            // (note: this call will do nothing with the fragments; we're looking for NPEs and such)
            TestDatabaseService.FillStaticQueryLibrary(typeof(TestFragmentProvider), typeof(TestDatabaseQueryProvider));

            // Query/Fragment Library combined
            ClearStaticQueryLibrary();
            TestDatabaseService.FillStaticQueryLibrary(typeof(TestQueryAndFragmentProvider));

            Assert.AreEqual(1, TestDatabaseService.StaticQueries.Count, "There should be one query in the library");
            Assert.IsTrue(TestDatabaseService.StaticQueries.ContainsKey("test2.query1"), "Query 1 not found");
            Assert.AreEqual("SELECT from some_table after_from", TestDatabaseService.StaticQueries["test2.query1"].SQL,
                "Fragmented query was not assembled correctly");
        }

        #endregion

        #region Support Classes

        /// <summary>
        /// Concrete implementation to use for testing
        /// </summary>
        private class TestDatabaseService : DatabaseService, IDatabaseService
        {
            public override int LastIdentity()
            {
                throw new NotImplementedException();
            }
            public override long LongLastIdentity()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Only query library
        /// </summary>
        private class TestDatabaseQueryProvider : IDatabaseQueryProvider
        {
            public string Prefix
            {
                get { return "test1."; }
                set { throw new NotImplementedException("Leave my prefix alone!"); }
            }

            public void Queries(IDictionary<string, DatabaseQuery> queries)
            {
                queries.Add(Prefix + "query1", new DatabaseQuery { SQL = "SELECT 3" });
                queries.Add(Prefix + "query2", new DatabaseQuery { SQL = "SELECT q" });
            }
        }

        /// <summary>
        /// Only fragment provider
        /// </summary>
        private class TestFragmentProvider : IQueryFragmentProvider
        {
            public void Fragments(IDictionary<string, QueryFragment> fragments)
            {
                fragments.Add("test.fragment", new QueryFragment());
            }
        }

        /// <summary>
        /// Query and fragment provider
        /// </summary>
        private class TestQueryAndFragmentProvider : IDatabaseQueryProvider, IQueryFragmentProvider
        {
            public string Prefix
            {
                get { return "test2."; }
                set { throw new NotImplementedException("Leave my prefix alone!"); }
            }

            public void Queries(IDictionary<string, DatabaseQuery> queries)
            {
                queries.Add(Prefix + "query1", AFragmentedQuery());
            }

            private FragmentedQuery AFragmentedQuery()
            {
                var query = new FragmentedQuery { SQL = "SELECT" };
                query.Fragments.Add(QueryFragmentType.From, "test.from");
                query.AfterFragment.Add(QueryFragmentType.From, "after_from");

                return query;
            }

            public void Fragments(IDictionary<string, QueryFragment> fragments)
            {
                fragments.Add("test.from", new QueryFragment { SQL = "from some_table" });
            }
        }

        #endregion

    }
}