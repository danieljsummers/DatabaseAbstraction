namespace DatabaseAbstraction.Tests.Queries
{
    using System.Collections.Generic;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using NUnit.Framework;

    /// <summary>
    /// Unit Test for the <see cref="DatabaseQueryProvider"/> class
    /// </summary>
    [TestFixture]
    public class DatabaseQueryProviderTest
    {
        /// <summary>
        /// Test the queries being provided
        /// </summary>
        [Test]
        public void Queries()
        {
            // Get the queries
            var queries = new Dictionary<string, DatabaseQuery>();
            new DatabaseQueryProvider().Queries(queries);

            // Ensure they all exist
            Assert.AreEqual(7, queries.Count);
            Assert.True(queries.ContainsKey("database.sequence.postgres"));
            Assert.True(queries.ContainsKey("database.sequence.sqlserver"));
            Assert.True(queries.ContainsKey("database.sequence.mysql"));
            Assert.True(queries.ContainsKey("database.sequence.generic"));
            Assert.True(queries.ContainsKey("database.identity.sqlserver"));
            Assert.True(queries.ContainsKey("database.identity.mysql"));
            Assert.True(queries.ContainsKey("database.identity.sqlite"));

            Assert.NotNull(queries["database.sequence.postgres"]);
            Assert.NotNull(queries["database.sequence.sqlserver"]);
            Assert.NotNull(queries["database.sequence.mysql"]);
            Assert.NotNull(queries["database.sequence.generic"]);
            Assert.NotNull(queries["database.identity.sqlserver"]);
            Assert.NotNull(queries["database.identity.mysql"]);
            Assert.NotNull(queries["database.identity.sqlite"]);
        }
    }
}