namespace DatabaseAbstraction.Tests.Utils.Test
{
    using DatabaseAbstraction.Utils.UnitTest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Unit Tests for the StubDataReader class
    /// </summary>
    [TestClass]
    public class StubDataReaderTest
    {
        /// <summary>
        /// Test the CurrentResultSet property and the NextResult() method
        /// </summary>
        [TestMethod]
        public void StubDataReader_CurrentResultSet_NextResult_Success()
        {
            var result1 = new StubResultSet("col1", "col2", "col3");
            var result2 = new StubResultSet("col4", "col5");

            using (var reader = new StubDataReader(result1, result2))
            {
                Assert.IsNotNull(reader, "The StubDataReader object should not have been null");

                var currentSet = reader.CurrentResultSet;
                Assert.IsNotNull(currentSet, "First call to CurrentResultSet should not have returned null");
                Assert.AreEqual(result1, currentSet, "First call to CurrentResultSet did not return the first set");

                Assert.IsTrue(reader.NextResult(), "First call to NextResult() should have had data");

                currentSet = reader.CurrentResultSet;
                Assert.IsNotNull(currentSet, "Second call to CurrentResultSet should not have returned null");
                Assert.AreEqual(result2, currentSet, "Second call to CurrentResultSet did not return the second set");

                Assert.IsFalse(reader.NextResult(), "Second call to NextResult() should not have had data");

                try
                {
                    currentSet = reader.CurrentResultSet;
                    Assert.Fail("Call to CurrentResultSet should have thrown an exception");
                }
                catch (InvalidOperationException exception)
                {
                    Assert.AreEqual("Current ResultSet is at EOF", exception.Message,
                        "Unexpected EOF exception message");
                }
            }
        }

        /// <summary>
        /// Test the FieldCount property
        /// </summary>
        [TestMethod]
        public void StubDataReader_FieldCount_Success()
        {
            var result1 = new StubResultSet("col6", "col7", "col8");
            var result2 = new StubResultSet("col9", "col0");

            using (var reader = new StubDataReader(result1, result2))
            {
                Assert.IsNotNull(reader, "The StubDataReader object should not have been null");
                Assert.AreEqual(3, reader.FieldCount, "The field count for the first result set was incorrect");

                reader.NextResult();
                Assert.AreEqual(2, reader.FieldCount, "The field count for the second result was incorrect");
            }
        }

        /// <summary>
        /// Test the index accessors and the Read() method
        /// </summary>
        [TestMethod]
        public void StubDataReader_IndexAccessors_Read_Success()
        {
            var result = new StubResultSet("abc", "def");
            result.AddRow(3, "nice");

            using (var reader = new StubDataReader(result))
            {
                try
                {
                    var column = reader[0];
                    Assert.Fail("Index accessor should have thrown an exception (BOF)");
                }
                catch (InvalidOperationException) { }

                Assert.IsTrue(reader.Read(), "First call to Read() should have had data");
                Assert.AreEqual(3, reader[0], "The first column (by index) was not correct");
                Assert.AreEqual("nice", reader["def"], "The second column (by name) was not correct");

                Assert.IsFalse(reader.Read(), "Second call to Read() should not have had data");

                try
                {
                    var column = reader[0];
                    Assert.Fail("Index accessor should have thrown an exception (EOF)");
                }
                catch (InvalidOperationException) { }
            }
        }

        /// <summary>
        /// Test the GetName() and GetDataTypeName() methods
        /// </summary>
        [TestMethod]
        public void StubDataReader_GetName_GetDataTypeName_Success()
        {
            var result = new StubResultSet("unit", "test");

            using (var reader = new StubDataReader(result))
            {
                Assert.AreEqual("test", reader.GetName(1));
                Assert.AreEqual("unit", reader.GetDataTypeName(0));
            }
        }

        /// <summary>
        /// Test the GetFieldType() method
        /// </summary>
        [TestMethod]
        public void StubDataReader_GetFieldType_Success()
        {
            var result = new StubResultSet("more", "fun");
            result.AddRow(8, "yowza");

            using (var reader = new StubDataReader(result))
            {
                reader.Read();
                Assert.AreEqual(typeof(int), reader.GetFieldType(0));
                Assert.AreEqual(typeof(string), reader.GetFieldType(1));
            }
        }
    }
}