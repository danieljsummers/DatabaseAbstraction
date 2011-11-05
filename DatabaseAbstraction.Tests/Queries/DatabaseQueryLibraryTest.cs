namespace DatabaseAbstraction.Tests.Queries
{
    using System.Collections.Generic;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the <see cref="DatabaseQueryLibrary"/> class
    /// </summary>
    [TestFixture]
    public class DatabaseQueryLibraryTest
    {
        /// <summary>
        /// Test the database queries
        /// </summary>
        [Test]
        public void Library()
        {
            // Get the queries
            Dictionary<string, DatabaseQuery> queries = new Dictionary<string, DatabaseQuery>();
            DatabaseQueryLibrary library = new DatabaseQueryLibrary();
            
            library.GetQueries(queries);

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