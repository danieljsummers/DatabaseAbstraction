namespace DatabaseAbstraction.Tests.Models
{
    using DatabaseAbstraction.Models;
    using NUnit.Framework;

    [TestFixture]
    public class QueryFragmentTest
    {
        /// <summary>
        /// Test the getter for the parameter dictionary
        /// </summary>
        [Test]
        public void Getters()
        {
            QueryFragment fragment = new QueryFragment();

            Assert.NotNull(fragment.Parameters);
            Assert.AreEqual(0, fragment.Parameters.Count);
        }
    }
}