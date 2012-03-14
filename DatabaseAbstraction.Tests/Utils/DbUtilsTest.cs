namespace DatabaseAbstraction.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using DatabaseAbstraction.Utils;
    using DatabaseAbstraction.Interfaces;

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
        /// also incurring a dependency on the drive.  So, while cheesy, this test only checks the negative condition.
        /// </remarks>
        [Test]
        public void CreateDatabaseService()
        {
            // No fragments
            var service = DbUtils.CreateDatabaseService(null, "NonExistentDriver", (IQueryLibrary)null);
            Assert.Null(service, "CreateDatabaseService() should have returned null");
            
            // Fragments
            service = DbUtils.CreateDatabaseService(null, "NonExistentDriver", (List<IQueryFragmentProvider>)null,
                (IQueryLibrary)null);
            Assert.Null(service, "CreateDatabaseService() should have returned null");
        }
    }
}