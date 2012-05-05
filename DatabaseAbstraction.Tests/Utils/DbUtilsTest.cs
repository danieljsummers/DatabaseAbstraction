namespace DatabaseAbstraction.Tests.Utils
{
    using System;
    using DatabaseAbstraction.Utils;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the DbUtils class
    /// </summary>
    [TestFixture]
    public class DbUtilsTest
    {
        /// <summary>
        /// Test the SingleParameter() method
        /// </summary>
        [Test]
        public void SingleParameter()
        {
            var parameter = DbUtils.SingleParameter("unit_test", "hooray");

            Assert.NotNull(parameter, "SingleParameter() should not have returned null");
            Assert.True(parameter.ContainsKey("unit_test"), "Parameter key not found");
            Assert.AreEqual("hooray", parameter["unit_test"], "Parameter value not correct");
        }

        /// <summary>
        /// Test the CreateDatabaseService() method
        /// </summary>
        /// <remarks>
        /// We can't exhaustively test this method without having one of each type of supported connection set up, and
        /// also incurring a dependency on the driver.  So, while cheesy, this test only checks the negative condition.
        /// </remarks>
        [Test]
        public void CreateDatabaseService()
        {
            var service = DbUtils.CreateDatabaseService(null, "NonExistentDriver", (Type)null);
            Assert.Null(service, "CreateDatabaseService() should have returned null");
        }
    }
}