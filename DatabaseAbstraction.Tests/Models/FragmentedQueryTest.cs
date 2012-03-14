namespace DatabaseAbstraction.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using DatabaseAbstraction.Models;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the <see cref="FragmentedQuery"/> class
    /// </summary>
    [TestFixture]
    public class FragmentedQueryTest
    {
        /// <summary>
        /// Test the getters for the fragment and after-fragment dictionaries
        /// </summary>
        [Test]
        public void Getters()
        {
            FragmentedQuery query = new FragmentedQuery();

            Assert.NotNull(query.Fragments, "Fragment dictionary should not be null");
            Assert.NotNull(query.AfterFragment, "After Fragment dictionary should not be null");

            Assert.AreEqual(0, query.Fragments.Count, "Fragment dictionary should be empty");
            Assert.AreEqual(0, query.AfterFragment.Count, "After Fragment dictionary should be empty");
        }

        /// <summary>
        /// Test the AppendFragment() method
        /// </summary>
        [Test]
        public void AppendFragment()
        {
            FragmentedQuery query = new FragmentedQuery();
            query.Name = "unit.test.query";

            // Set up the fragment
            query.SQL = "before";
            query.Fragments.Add(QueryFragmentType.Where, "unit.test.fragment");
            query.AfterFragment.Add(QueryFragmentType.Where, "afterwards");

            Dictionary<string, QueryFragment> fragments = new Dictionary<string, QueryFragment>();
            fragments.Add("unit.test.fragment", new QueryFragment { SQL = "the fragment" });
            fragments["unit.test.fragment"].Parameters.Add("unit.test.parameter", DbType.String);

            StringBuilder sql = new StringBuilder();

            // Assemble this fragment
            query.AppendFragment(QueryFragmentType.Where, sql, fragments);

            // Check the results
            Assert.AreEqual(" the fragment afterwards", sql.ToString());
            Assert.AreEqual(1, query.Parameters.Count);
            Assert.True(query.Parameters.ContainsKey("unit.test.parameter"));
            Assert.AreEqual(DbType.String, query.Parameters["unit.test.parameter"]);

            // Test it with no after-fragment
            sql.Clear();
            query.AfterFragment.Clear();
            query.Parameters.Clear();

            query.AppendFragment(QueryFragmentType.Where, sql, fragments);

            // Check the results
            Assert.AreEqual(" the fragment", sql.ToString());
            Assert.AreEqual(1, query.Parameters.Count);

            // Test it with no fragments defined
            sql.Clear();
            query.Parameters.Clear();

            query.AppendFragment(QueryFragmentType.From, sql, fragments);

            // Check the results
            Assert.AreEqual(String.Empty, sql.ToString());
            Assert.AreEqual(0, query.Parameters.Count);

            // Test with a non-existent fragment
            query.Fragments.Add(QueryFragmentType.OrderBy, "non.existent.fragment");

            try
            {
                query.AppendFragment(QueryFragmentType.OrderBy, sql, fragments);
                Assert.Fail("AppendFragment should have thrown an exeception for a non-existent fragment");
            }
            catch (KeyNotFoundException exception)
            {
                Assert.AreEqual(
                    "Unable to find OrderBy query fragment non.existent.fragment defined in query unit.test.query",
                    exception.Message);
            }
        }

        /// <summary>
        /// Test the Assemble() method
        /// </summary>
        [Test]
        public void Assemble()
        {
            // Set up a query with SQL and fragments/after-fragments on every type
            var query = AllFragmentsUsed();
            var fragments = AllFragments();

            query.SQL = "original";

            // Put it all together
            query.Assemble(fragments);

            // Check the results
            Assert.AreEqual(
                "original select after-select insert after-insert update after-update delete after-delete from after-from where after-where orderby after-orderby",
                query.SQL, "Fragmented query was not generated correctly");
            Assert.AreEqual(0, query.Fragments.Count, "Query fragments were not cleared");
            Assert.AreEqual(0, query.AfterFragment.Count, "Query after-fragments were not cleared");

            // Same as above, but this time without SQL
            query = AllFragmentsUsed();

            query.Assemble(fragments);

            Assert.AreEqual(
                "select after-select insert after-insert update after-update delete after-delete from after-from where after-where orderby after-orderby",
                query.SQL, "Fragmented query with no prior SQL was not generated correctly");
        }

        /// <summary>
        /// Get a fragmented query with all fragments used
        /// </summary>
        /// <returns>
        /// A fragmented query with all fragments used
        /// </returns>
        private FragmentedQuery AllFragmentsUsed()
        {
            var query = new FragmentedQuery();

            query.Fragments.Add(QueryFragmentType.Select, "unit.test.select");
            query.Fragments.Add(QueryFragmentType.From, "unit.test.from");
            query.Fragments.Add(QueryFragmentType.Where, "unit.test.where");
            query.Fragments.Add(QueryFragmentType.OrderBy, "unit.test.orderby");
            query.Fragments.Add(QueryFragmentType.Insert, "unit.test.insert");
            query.Fragments.Add(QueryFragmentType.Update, "unit.test.update");
            query.Fragments.Add(QueryFragmentType.Delete, "unit.test.delete");

            query.AfterFragment.Add(QueryFragmentType.Select, "after-select");
            query.AfterFragment.Add(QueryFragmentType.Insert, "after-insert");
            query.AfterFragment.Add(QueryFragmentType.Update, "after-update");
            query.AfterFragment.Add(QueryFragmentType.Delete, "after-delete");
            query.AfterFragment.Add(QueryFragmentType.From, "after-from");
            query.AfterFragment.Add(QueryFragmentType.Where, "after-where");
            query.AfterFragment.Add(QueryFragmentType.OrderBy, "after-orderby");

            return query;
        }

        /// <summary>
        /// Get a list of fragments with their type name as the SQL for the fragment
        /// </summary>
        /// <returns>
        /// A list of fragments
        /// </returns>
        private Dictionary<string, QueryFragment> AllFragments()
        {
            var fragments = new Dictionary<string, QueryFragment>();

            fragments.Add("unit.test.select", new QueryFragment { SQL = "select" });
            fragments.Add("unit.test.insert", new QueryFragment { SQL = "insert" });
            fragments.Add("unit.test.update", new QueryFragment { SQL = "update" });
            fragments.Add("unit.test.delete", new QueryFragment { SQL = "delete" });
            fragments.Add("unit.test.from", new QueryFragment { SQL = "from" });
            fragments.Add("unit.test.where", new QueryFragment { SQL = "where" });
            fragments.Add("unit.test.orderby", new QueryFragment { SQL = "orderby" });

            return fragments;
        }
    }
}