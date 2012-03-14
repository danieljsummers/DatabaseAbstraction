namespace DatabaseAbstraction.Tests.Utils.Test
{
    using DatabaseAbstraction.Utils.UnitTest;
    using NUnit.Framework;

    /// <summary>
    /// Unit Test for the StubDataRow class
    /// </summary>
    [TestFixture]
    public class StubDataRowTest
    {
        [Test]
        public void Test()
        {
            var row = new StubDataRow(1, "E", false);

            Assert.NotNull(row, "The row should not have been null");
            Assert.AreEqual(3, row.Length, "There should have been 3 values in the row");
            Assert.AreEqual(1, row[0], "Value 1 should have been the number 1");
            Assert.AreEqual("E", row[1], "Value 2 should have been the string E");
            Assert.False((bool)row[2], "Value 3 should have been boolean false");
        }
    }
}