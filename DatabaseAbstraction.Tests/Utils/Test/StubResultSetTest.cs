namespace DatabaseAbstraction.Tests.Utils.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using DatabaseAbstraction.Utils.UnitTest;

    /// <summary>
    /// Unit Tests for the StubResultSet class
    /// </summary>
    [TestFixture]
    public class StubResultSetTest
    {
        /// <summary>
        /// Test the CurrentRow property and the Read() method
        /// </summary>
        [Test]
        public void CurrentRowAndRead()
        {
            var result = new StubResultSet();

            // Current Row before Read() = exception
            try
            {
                var row = result.CurrentRow;
                Assert.Fail("Attempt to get current should have failed because Read() has not been called");
            }
            catch (ArgumentOutOfRangeException) { }

            // Current Row after Read()
            result = new StubResultSet("a", "b", "c");
            result.AddRow(1, 2, 3);
            result.AddRow(4, 5, 6);

            result.Read();
            Assert.AreEqual(1, result.CurrentRow[0], "First row was not returned after first Read()");
            Assert.AreEqual(1, result.CurrentRow[0], "Subsequent call to CurrentRow did not return the same results");

            result.Read();
            Assert.AreEqual(4, result.CurrentRow[0], "Second row was not returned after second Read()");

            // Final Read() should return false
            Assert.False(result.Read(), "Read() should have encountered the end of the set");
        }

        /// <summary>
        /// Test the GetFieldNames() method
        /// </summary>
        [Test]
        public void GetFieldNames()
        {
            var result = new StubResultSet("Q", "G", "yea");
            var fields = result.GetFieldNames();

            Assert.NotNull(fields, "Field names should not have been null");
            Assert.AreEqual(3, fields.Length, "There should have been 3 field names");
            Assert.AreEqual("Q", fields[0], "The first field name was incorrect");
            Assert.AreEqual("G", fields[1], "The second field name was incorrect");
            Assert.AreEqual("yea", fields[2], "The third field name was incorrect");
        }

        /// <summary>
        /// Tet the GetFieldName() method
        /// </summary>
        [Test]
        public void GetFieldName()
        {
            var result = new StubResultSet("x", "y", "d");

            Assert.AreEqual("x", result.GetFieldName(0), "Field 1 was incorrect");

            try
            {
                var field = result.GetFieldName(7);
                Assert.Fail("Call to invalid index should have thrown an exception");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.True(exception.Message.StartsWith("Index can only be between 0 and 2"),
                    "Unexpected exception message:\n" + exception.Message);
                Assert.True(exception.Message.EndsWith("Parameter name: index"),
                    "Unexpected exception message:\n" + exception.Message);
            }
        }

        /// <summary>
        /// Test the AddRow() method
        /// </summary>
        [Test]
        public void AddRow()
        {
            var result = new StubResultSet("e", "f", "g");
            result.AddRow(3, 4, 5);
            result.Read();

            Assert.AreEqual(3, result.CurrentRow[0], "The first value given to AddRow() was not preserved");
            Assert.AreEqual(4, result.CurrentRow[1], "The second value given to AddRow() was not preserved");
            Assert.AreEqual(5, result.CurrentRow[2], "The third value given to AddRow() was not preserved");

            try
            {
                result.AddRow(3, 2);
                Assert.Fail("AddRow() with wrong number of parameters should have thrown an exception");
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Assert.True(exception.Message.StartsWith("The row must contain 3 items"),
                    "Unexpected exception message:\n" + exception.Message);
                Assert.True(exception.Message.EndsWith("Parameter name: values"),
                    "Unexpected exception message:\n" + exception.Message);
            }
        }

        /// <summary>
        /// Test the GetIndexFieldFromName() method
        /// </summary>
        [Test]
        public void GetIndexFieldFromName()
        {
            var result = new StubResultSet("three", "six", "five");
            Assert.AreEqual(1, result.GetIndexFromFieldName("six"), "Field 2 was not retrieved correctly");

            try
            {
                var index = result.GetIndexFromFieldName("seven");
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