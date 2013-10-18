namespace DatabaseAbstraction.Tests.Utils
{
    using DatabaseAbstraction.Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Unit tests for the DbUtils class
    /// </summary>
    [TestClass]
    public class DbUtilsTest
    {
        /// <summary>
        /// Test the SingleParameter() method
        /// </summary>
        [TestMethod]
        public void DbUtils_SingleParameter_Success()
        {
            var parameter = DbUtils.SingleParameter("unit_test", "hooray");

            Assert.IsNotNull(parameter, "SingleParameter() should not have returned null");
            Assert.IsTrue(parameter.ContainsKey("unit_test"), "Parameter key not found");
            Assert.AreEqual("hooray", parameter["unit_test"], "Parameter value not correct");
        }

        /// <summary>
        /// Test the CreateDatabaseService() method
        /// </summary>
        /// <remarks>
        /// We can't exhaustively test this method without having one of each type of supported connection set up, and
        /// also incurring a dependency on the driver.  So, while cheesy, this test only checks the negative condition.
        /// </remarks>
        [TestMethod]
        public void DbUtils_CreateDatabaseService_Failure()
        {
            var service = DbUtils.CreateDatabaseService(null, "NonExistentDriver", (Type)null);
            Assert.IsNull(service, "CreateDatabaseService() should have returned null");
        }
    }
}