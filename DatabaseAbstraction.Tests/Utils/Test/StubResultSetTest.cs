namespace DatabaseAbstraction.Tests.Utils.Test
{
    using DatabaseAbstraction.Utils.UnitTest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Unit Tests for the StubResultSet class
    /// </summary>
    [TestClass]
    public class StubResultSetTest
    {
        /// <summary>
        /// Test the CurrentRow property on a set that is pointing to Beginning of File (BOF)
        /// </summary>
        [TestMethod]
        public void StubResultSet_CurrentRow_BOF_Failure()
        {
            try
            {
                var row = new StubResultSet().CurrentRow;
                Assert.Fail("Attempt to get current row should have thrown an exception (BOF)");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Current ResultSet is at BOF", exception.Message, "Unexpected BOF exception message");
            }
        }

        /// <summary>
        /// Test the CurrentRow property and the Read() method
        /// </summary>
        [TestMethod]
        public void StubResultSet_CurrentRow_Read_Success()
        {
            var result = new StubResultSet("a", "b", "c")
                .AddRow(1, 2, 3)
                .AddRow(4, 5, 6);

            Assert.IsTrue(result.Read(), "Read() should have returned true (row 1)");
            Assert.AreEqual(1, result.CurrentRow[0], "First row was not returned after first Read()");
            Assert.AreEqual(1, result.CurrentRow[0], "Subsequent call to CurrentRow did not return the same results");

            Assert.IsTrue(result.Read(), "Read() should have returned true (row 2)");
            Assert.AreEqual(4, result.CurrentRow[0], "Second row was not returned after second Read()");

            Assert.IsFalse(result.Read(), "Read() should have encountered the end of the set");
        }

        /// <summary>
        /// Test the CurrentRow property on a set that is pointing to End of File (EOF)
        /// </summary>
        [TestMethod]
        public void StubResultSet_CurrentRow_EOF_Failure()
        {
            var result = new StubResultSet();

            Assert.IsFalse(result.Read(), "Read() should have encountered the end of the set");

            try
            {
                var row = result.CurrentRow;
                Assert.Fail("Attempt to get current row should have thrown an exception (EOF)");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Current ResultSet is at EOF", exception.Message, "Unexpected EOF exception message");
            }
        }

        /// <summary>
        /// Test the GetFieldNames() method
        /// </summary>
        [TestMethod]
        public void StubResultSet_GetFieldNames_Success()
        {
            var fields = new StubResultSet("Q", "G", "yea").GetFieldNames();

            Assert.IsNotNull(fields, "Field names should not have been null");
            Assert.AreEqual(3, fields.Length, "There should have been 3 field names");
            Assert.AreEqual("Q", fields[0], "The first field name was incorrect");
            Assert.AreEqual("G", fields[1], "The second field name was incorrect");
            Assert.AreEqual("yea", fields[2], "The third field name was incorrect");
        }

        /// <summary>
        /// Tet the GetFieldName() method on an index that exists
        /// </summary>
        [TestMethod]
        public void StubResultSet_GetFieldName_Success()
        {
            Assert.AreEqual("x", new StubResultSet("x", "y", "d").GetFieldName(0), "Field 1 was incorrect");
        }

        /// <summary>
        /// Tet the GetFieldName() method on an index that does not exist
        /// </summary>
        [TestMethod]
        public void StubResultSet_GetFieldName_Failure()
        {
            try
            {
                var field = new StubResultSet("x", "y", "d").GetFieldName(7);
                Assert.Fail("Call to invalid index should have thrown an exception");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Index can only be between 0 and 2"),
                    "Unexpected exception message:\n" + exception.Message);
                Assert.IsTrue(exception.Message.EndsWith("Parameter name: index"),
                    "Unexpected exception message:\n" + exception.Message);
            }
        }

        /// <summary>
        /// Test the AddRow() method with data that fits
        /// </summary>
        [TestMethod]
        public void StubResultSet_AddRow_Success()
        {
            var result = new StubResultSet("e", "f", "g");
            var addRowResult = result.AddRow(3, 4, 5);

            Assert.AreSame(result, addRowResult, "AddRow() should have returned a reference to its owning result set");

            result.Read();

            Assert.AreEqual(3, result.CurrentRow[0], "The first value given to AddRow() was not preserved");
            Assert.AreEqual(4, result.CurrentRow[1], "The second value given to AddRow() was not preserved");
            Assert.AreEqual(5, result.CurrentRow[2], "The third value given to AddRow() was not preserved");
        }

        /// <summary>
        /// Test the AddRow() method with the wrong number of values
        /// </summary>
        [TestMethod]
        public void StubResultSet_AddRow_Failure()
        {
            try
            {
                new StubResultSet("e", "f", "g").AddRow(3, 2);
                Assert.Fail("AddRow() with wrong number of parameters should have thrown an exception");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("The row must contain 3 items"),
                    "Unexpected exception message:\n" + exception.Message);
                Assert.IsTrue(exception.Message.EndsWith("Parameter name: values"),
                    "Unexpected exception message:\n" + exception.Message);
            }
        }

        /// <summary>
        /// Test the GetIndexFieldFromName() method on a field name that exists
        /// </summary>
        [TestMethod]
        public void StubResultSet_GetIndexFieldFromName_Success()
        {
            Assert.AreEqual(1, new StubResultSet("three", "six", "five").GetIndexFromFieldName("six"),
                "Field 2 was not retrieved correctly");
        }

        /// <summary>
        /// Test the GetIndexFieldFromName() method on a field name that does not exist
        /// </summary>
        [TestMethod]
        public void StubResultSet_GetIndexFieldFromName_Failure()
        {
            try
            {
                var index = new StubResultSet("three", "six", "five").GetIndexFromFieldName("seven");
                Assert.Fail("GetIndexFromFieldName() with non-existent name should have thrown an exception");
            }
            catch (IndexOutOfRangeException exception)
            {
                Assert.AreEqual("The key 'seven' was not found in this data reader", exception.Message,
                    "Unexpected exception message");
            }
        }
    }
}