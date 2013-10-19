namespace DatabaseAbstraction.Tests.Queries
{
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    /// <summary>
    /// Unit Test for the <see cref="DatabaseQueryProvider"/> class
    /// </summary>
    [TestClass]
    public class DatabaseQueryProviderTest
    {
        /// <summary>
        /// Test the queries being provided
        /// </summary>
        [TestMethod]
        public void DatabaseQueryProvider_Queries_Success()
        {
            // Get the queries
            var queries = new Dictionary<string, DatabaseQuery>();
            new DatabaseQueryProvider().Queries(queries);

            // Ensure they all exist
            Assert.AreEqual(7, queries.Count, "There should be 7 queries from the database query provider");
            Assert.IsTrue(queries.ContainsKey("database.sequence.postgres"),
                "The PostgreSQL sequence query was not found");
            Assert.IsTrue(queries.ContainsKey("database.sequence.sqlserver"),
                "The SQL Server sequence query was not found");
            Assert.IsTrue(queries.ContainsKey("database.sequence.mysql"), "The MySQL sequence query was not found");
            Assert.IsTrue(queries.ContainsKey("database.sequence.generic"),
                "The generic (MAX + 1) sequence query was not found");
            Assert.IsTrue(queries.ContainsKey("database.identity.sqlserver"),
                "The SQL Server identity query was not found");
            Assert.IsTrue(queries.ContainsKey("database.identity.mysql"), "The MySQL identity query was not found");
            Assert.IsTrue(queries.ContainsKey("database.identity.sqlite"), "The SQLite identity query was not found");

            Assert.IsNotNull(queries["database.sequence.postgres"], "The PostgreSQL sequence query should not be null");
            Assert.IsNotNull(queries["database.sequence.sqlserver"],
                "The SQL Server sequence query should not be null");
            Assert.IsNotNull(queries["database.sequence.mysql"], "The MySQL sequence query should not be null");
            Assert.IsNotNull(queries["database.sequence.generic"],
                "The generic (MAX + 1) sequence query should not be null");
            Assert.IsNotNull(queries["database.identity.sqlserver"],
                "The SQL Server identity query should not be null");
            Assert.IsNotNull(queries["database.identity.mysql"], "The MySQL identity query should not be null");
            Assert.IsNotNull(queries["database.identity.sqlite"], "The SQLite identity query should not be null");
        }
    }
}