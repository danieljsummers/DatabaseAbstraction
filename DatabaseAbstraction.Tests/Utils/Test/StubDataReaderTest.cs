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
        /// Test the CurrentResultSet property and the NextResult() method where data is returned
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
            }
        }

        /// <summary>
        /// Test the CurrentResultSet property and the NextResult() method where data was not found
        /// </summary>
        [TestMethod]
        public void StubDataReader_CurrentResultSet_NextResult_Failure()
        {
            using (var reader = new StubDataReader())
            {
                Assert.IsNotNull(reader, "The StubDataReader object should not have been null");
                Assert.IsFalse(reader.NextResult(), "NextResult() should not have had data");

                try
                {
                    var results = reader.CurrentResultSet;
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
        /// Test the index accessors on a set that is pointing to Beginning of File (BOF)
        /// </summary>
        [TestMethod]
        public void StubDataReader_IndexAccessors_BOF_Failure()
        {
            using (var reader = new StubDataReader(new StubResultSet()))
            {
                try
                {
                    var column = reader[0];
                    Assert.Fail("Index accessor should have thrown an exception (BOF)");
                }
                catch (InvalidOperationException) { }
            }
        }

        /// <summary>
        /// Test the index accessors and the Read() method
        /// </summary>
        [TestMethod]
        public void StubDataReader_IndexAccessors_Read_Success()
        {
            using (var reader = new StubDataReader(new StubResultSet("abc", "def").AddRow(3, "nice")))
            {
                Assert.IsTrue(reader.Read(), "First call to Read() should have had data");
                Assert.AreEqual(3, reader[0], "The first column (by index) was not correct");
                Assert.AreEqual("nice", reader["def"], "The second column (by name) was not correct");

                Assert.IsFalse(reader.Read(), "Second call to Read() should not have had data");
            }
        }

        /// <summary>
        /// Test the index accessors on a set that is pointing to End of File (EOF)
        /// </summary>
        [TestMethod]
        public void StubDataReader_IndexAccessors_EOF_Failure()
        {
            using (var reader = new StubDataReader(new StubResultSet()))
            {
                Assert.IsFalse(reader.Read(), "Read() should not have had data");

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
            using (var reader = new StubDataReader(new StubResultSet("unit", "test")))
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
            using (var reader = new StubDataReader(new StubResultSet("more", "fun").AddRow(8, "yowza")))
            {
                reader.Read();
                Assert.AreEqual(typeof(int), reader.GetFieldType(0));
                Assert.AreEqual(typeof(string), reader.GetFieldType(1));
            }
        }
    }
}