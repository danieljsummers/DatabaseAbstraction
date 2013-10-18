namespace DatabaseAbstraction.Tests.Utils.Test
{
    using DatabaseAbstraction.Utils.UnitTest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit Test for the StubDataRow class
    /// </summary>
    [TestClass]
    public class StubDataRowTest
    {
        /// <summary>
        /// Test the constructor
        /// </summary>
        [TestMethod]
        public void StubDataRow_Constructor_Success()
        {
            var row = new StubDataRow(1, "E", false);

            Assert.IsNotNull(row, "The row should not have been null");
            Assert.AreEqual(3, row.Length, "There should have been 3 values in the row");
            Assert.AreEqual(1, row[0], "Value 1 should have been the number 1");
            Assert.AreEqual("E", row[1], "Value 2 should have been the string E");
            Assert.IsFalse((bool)row[2], "Value 3 should have been boolean false");
        }
    }
}