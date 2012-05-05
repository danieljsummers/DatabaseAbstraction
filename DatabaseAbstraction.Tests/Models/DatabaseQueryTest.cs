namespace DatabaseAbstraction.Tests.Models
{
    using DatabaseAbstraction.Models;
    using NUnit.Framework;

    /// <summary>
    /// Tests for the <see cref="DatabaseQuery"/> class
    /// </summary>
    [TestFixture]
    public class DatabaseQueryTest
    {
        /// <summary>
        /// Test the getter for the parameter dictionary
        /// </summary>
        [Test]
        public void Getters()
        {
            var query = new DatabaseQuery();

            Assert.NotNull(query.Parameters);
            Assert.AreEqual(0, query.Parameters.Count);
        }
    }
}