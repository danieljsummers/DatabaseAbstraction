namespace DatabaseAbstraction.Tests
{
    using System;
    using System.Collections.Generic;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using NUnit.Framework;

    /// <summary>
    /// Unit Tests for the DatabaseService class
    /// </summary>
    [TestFixture]
    public class DatabaseServiceTest
    {
        #region Tests

        /// <summary>
        /// Clear the static query library (removes database queries)
        /// </summary>
        [SetUp]
        [TearDown]
        public void ClearStaticQueryLibrary()
        {
            TestDatabaseService.StaticQueries.Clear();
        }

        /// <summary>
        /// Test the FillStaticQueryLibrary() method
        /// </summary>
        [Test]
        public void FillStaticQueryLibrary()
        {
            // Query Library Only
            TestDatabaseService.FillStaticQueryLibrary(new TestQueryLibrary());

            Assert.IsNotNull(TestDatabaseService.StaticQueries, "The static queries should not have been null");
            Assert.AreEqual(2, TestDatabaseService.StaticQueries.Count, "There should be two queries in the library");
            Assert.IsTrue(TestDatabaseService.StaticQueries.ContainsKey("test1.query1"), "Query 1 not found");
            Assert.IsTrue(TestDatabaseService.StaticQueries.ContainsKey("test1.query2"), "Query 2 not found");

            // Query Library / Fragment Library specified
            ClearStaticQueryLibrary();

            var fragments = new List<IQueryFragmentProvider>();
            fragments.Add(new TestFragmentLibrary());

            // This call will do nothing with the fragments; we're looking for NPEs and such
            TestDatabaseService.FillStaticQueryLibrary(fragments, new TestQueryLibrary());

            // Query/Fragment Library combined
            ClearStaticQueryLibrary();
            TestDatabaseService.FillStaticQueryLibrary(new TestQueryAndFragmentLibrary());

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
            public int LastIdentity()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Only query library
        /// </summary>
        private class TestQueryLibrary : IQueryLibrary
        {
            public string Prefix
            {
                get { return "test1."; }
                set { throw new NotImplementedException("Leave my prefix alone!"); }
            }

            public void GetQueries(Dictionary<string, DatabaseQuery> queries)
            {
                queries.Add(Prefix + "query1", new DatabaseQuery { SQL = "SELECT 3" });
                queries.Add(Prefix + "query2", new DatabaseQuery { SQL = "SELECT q" });
            }
        }

        /// <summary>
        /// Only fragment provider
        /// </summary>
        private class TestFragmentLibrary : IQueryFragmentProvider
        {
            public void Fragments(Dictionary<string, QueryFragment> fragments)
            {
                fragments.Add("test.fragment", new QueryFragment());
            }
        }

        /// <summary>
        /// Query and fragment provider
        /// </summary>
        private class TestQueryAndFragmentLibrary : IQueryLibrary, IQueryFragmentProvider
        {
            public string Prefix
            {
                get { return "test2."; }
                set { throw new NotImplementedException("Leave my prefix alone!"); }
            }

            public void GetQueries(Dictionary<string, DatabaseQuery> queries)
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

            public void Fragments(Dictionary<string, QueryFragment> fragments)
            {
                fragments.Add("test.from", new QueryFragment { SQL = "from some_table" });
            }
        }

        #endregion

    }
}