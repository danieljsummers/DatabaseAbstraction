namespace DatabaseAbstraction.Tests.Models
{
    using DatabaseAbstraction.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="DatabaseQuery"/> class
    /// </summary>
    [TestClass]
    public class DatabaseQueryTest
    {
        /// <summary>
        /// Test the getter for the parameter dictionary
        /// </summary>
        [TestMethod]
        public void DatabaseQuery_Parameters_Empty_Success()
        {
            var query = new DatabaseQuery();

            Assert.IsNotNull(query.Parameters);
            Assert.AreEqual(0, query.Parameters.Count);
        }
    }
}