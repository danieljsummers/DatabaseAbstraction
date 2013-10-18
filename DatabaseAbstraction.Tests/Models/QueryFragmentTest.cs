namespace DatabaseAbstraction.Tests.Models
{
    using DatabaseAbstraction.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit Test for the QueryFragment class
    /// </summary>
    [TestClass]
    public class QueryFragmentTest
    {
        /// <summary>
        /// Test the getter for the parameter dictionary
        /// </summary>
        [TestMethod]
        public void QueryFragment_Parameters_Empty_Success()
        {
            var fragment = new QueryFragment();

            Assert.IsNotNull(fragment.Parameters);
            Assert.AreEqual(0, fragment.Parameters.Count);
        }
    }
}