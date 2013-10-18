namespace DatabaseAbstraction.Tests.Queries
{
    using System.Collections.Generic;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.AreEqual(7, queries.Count);
            Assert.IsTrue(queries.ContainsKey("database.sequence.postgres"));
            Assert.IsTrue(queries.ContainsKey("database.sequence.sqlserver"));
            Assert.IsTrue(queries.ContainsKey("database.sequence.mysql"));
            Assert.IsTrue(queries.ContainsKey("database.sequence.generic"));
            Assert.IsTrue(queries.ContainsKey("database.identity.sqlserver"));
            Assert.IsTrue(queries.ContainsKey("database.identity.mysql"));
            Assert.IsTrue(queries.ContainsKey("database.identity.sqlite"));

            Assert.IsNotNull(queries["database.sequence.postgres"]);
            Assert.IsNotNull(queries["database.sequence.sqlserver"]);
            Assert.IsNotNull(queries["database.sequence.mysql"]);
            Assert.IsNotNull(queries["database.sequence.generic"]);
            Assert.IsNotNull(queries["database.identity.sqlserver"]);
            Assert.IsNotNull(queries["database.identity.mysql"]);
            Assert.IsNotNull(queries["database.identity.sqlite"]);
        }
    }
}