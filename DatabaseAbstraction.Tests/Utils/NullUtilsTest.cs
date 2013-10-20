namespace DatabaseAbstraction.Tests.Utils
{
    using DatabaseAbstraction.Utils;
    using DatabaseAbstraction.Utils.UnitTest;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Unit Tests for the NullUtils class
    /// </summary>
    [TestClass]
    public class NullUtilsTest
    {
        #region Data Reader Get Method Tests

        /// <summary>
        /// Test the GetBoolean() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetBoolean_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(bool), NullUtils.GetBoolean(reader, "unit_test"),
                "GetBoolean(null) should have returned the default boolean value");
        }

        /// <summary>
        /// Test the GetBoolean() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetBoolean_NotNull_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(true));

            reader.Read();
            Assert.IsTrue(NullUtils.GetBoolean(reader, "unit_test"), "GetBoolean(true) should have returned true");
        }

        /// <summary>
        /// Test the GetBooleanOrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetBooleanOrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetBooleanOrNull(reader, "unit_test"),
                "GetBooleanOrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetBooleanOrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetBooleanOrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(true));

            reader.Read();
            Assert.IsTrue(NullUtils.GetBooleanOrNull(reader, "unit_test").Value,
                "GetBooleanOrNull(true) should have returned true");
        }

        /// <summary>
        /// Test the GetDateTime() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDateTime_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(DateTime), NullUtils.GetDateTime(reader, "unit_test"),
                "GetDateTime(null) should not have returned a default DateTime");
        }

        /// <summary>
        /// Test the GetDateTime() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDateTime_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(new DateTime(2004, 12, 13)));

            reader.Read();
            Assert.AreEqual(new DateTime(2004, 12, 13), NullUtils.GetDateTime(reader, "unit_test"),
                "GetDateTime(value) should have returned a value");
        }

        /// <summary>
        /// Test the GetDateTimeOrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDateTimeOrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetDateTimeOrNull(reader, "unit_test"),
                "GetDateTime(null) should have returned null");
        }

        /// <summary>
        /// Test the GetDateTimeOrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDateTimeOrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(new DateTime(2004, 12, 13)));

            reader.Read();
            Assert.AreEqual(new DateTime(2004, 12, 13), NullUtils.GetDateTimeOrNull(reader, "unit_test").Value,
                "GetDateTimeOrNull(value) should have returned a value");
        }

        /// <summary>
        /// Test the GetDecimal() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDecimal_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(decimal), NullUtils.GetDecimal(reader, "unit_test"),
                "GetDecimal(null) should have returned the default decimal value");
        }

        /// <summary>
        /// Test the GetDecimal() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDecimal_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(17M));

            reader.Read();
            Assert.AreEqual(17M, NullUtils.GetDecimal(reader, "unit_test"),
                "GetDecimal(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetDecimalOrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDecimalOrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetDecimalOrNull(reader, "unit_test"),
                "GetDecimalOrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetDecimalOrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDecimalOrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(22M));

            reader.Read();
            Assert.AreEqual(22M, NullUtils.GetDecimalOrNull(reader, "unit_test").Value,
                "GetDecimalOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetDouble() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDouble_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(double), NullUtils.GetDouble(reader, "unit_test"),
                "GetDouble(null) should have returned the default double value");
        }

        /// <summary>
        /// Test the GetDouble() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDouble_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(4D));

            reader.Read();
            Assert.AreEqual(4D, NullUtils.GetDouble(reader, "unit_test"),
                "GetDouble(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetDoubleOrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDoubleOrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetDoubleOrNull(reader, "unit_test"),
                "GetDoubleOrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetDoubleOrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetDoubleOrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(9D));

            reader.Read();
            Assert.AreEqual(9D, NullUtils.GetDoubleOrNull(reader, "unit_test").Value,
                "GetDoubleOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetFloat() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetFloat_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(float), NullUtils.GetFloat(reader, "unit_test"),
                "GetFloat(null) should have returned the default float value");
        }

        /// <summary>
        /// Test the GetFloat() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetFloat_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(6F));

            reader.Read();
            Assert.AreEqual(6F, NullUtils.GetFloat(reader, "unit_test"),
                "GetFloat(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetFloatOrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetFloatOrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetFloatOrNull(reader, "unit_test"),
                "GetFloatOrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetFloatOrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetFloatOrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(2F));

            reader.Read();
            Assert.AreEqual(2F, NullUtils.GetFloatOrNull(reader, "unit_test").Value,
                "GetFloatOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetGuid() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetGuid_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(Guid), NullUtils.GetGuid(reader, "unit_test"),
                "GetGuid(null) should have returned the default Guid value");
        }

        /// <summary>
        /// Test the GetGuid() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetGuid_Value_Success()
        {
            var guid = Guid.NewGuid();
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(guid));

            reader.Read();
            Assert.AreEqual(guid, NullUtils.GetGuid(reader, "unit_test"),
                "GetGuid(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetGuidOrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetGuidOrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetGuidOrNull(reader, "unit_test"),
                "GetGuidOrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetGuidOrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetGuidOrNull_Value_Success()
        {
            var guid = Guid.NewGuid();
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(guid));

            reader.Read();
            Assert.AreEqual(guid, NullUtils.GetGuidOrNull(reader, "unit_test").Value,
                "GetGuidOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt16() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt16_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(short), NullUtils.GetInt16(reader, "unit_test"),
                "GetInt16(null) should have returned the default short value");
        }

        /// <summary>
        /// Test the GetInt16() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt16_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow((short)78));

            reader.Read();
            Assert.AreEqual((short)78, NullUtils.GetInt16(reader, "unit_test"),
                "GetInt16(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt16OrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt16OrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetInt16OrNull(reader, "unit_test"),
                "GetInt16OrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetInt16OrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt16OrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow((short)17));

            reader.Read();
            Assert.AreEqual((short)17, NullUtils.GetInt16OrNull(reader, "unit_test").Value,
                "GetInt16OrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt32() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt32_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(int), NullUtils.GetInt32(reader, "unit_test"),
                "GetInt32(null) should have returned the default int value");
        }

        /// <summary>
        /// Test the GetInt32() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt32_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(24));

            reader.Read();
            Assert.AreEqual(24, NullUtils.GetInt32(reader, "unit_test"),
                "GetInt32(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt32OrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt32OrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetInt32OrNull(reader, "unit_test"),
                "GetInt32OrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetInt32OrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt32OrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(7));

            reader.Read();
            Assert.AreEqual(7, NullUtils.GetInt32OrNull(reader, "unit_test").Value,
                "GetInt32OrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt64() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt64_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(default(long), NullUtils.GetInt64(reader, "unit_test"),
                "GetInt64(null) should have returned the default long value");
        }

        /// <summary>
        /// Test the GetInt64() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt64_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(5L));

            reader.Read();
            Assert.AreEqual(5L, NullUtils.GetInt64(reader, "unit_test"),
                "GetInt64(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt64OrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt64OrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetInt64OrNull(reader, "unit_test"),
                "GetInt64OrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetInt64OrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetInt64OrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(64L));

            reader.Read();
            Assert.AreEqual(64L, NullUtils.GetInt64OrNull(reader, "unit_test").Value,
                "GetInt64OrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetString() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetString_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.AreEqual(String.Empty, NullUtils.GetString(reader, "unit_test"),
                "GetString(null) should have returned an empty string");
        }

        /// <summary>
        /// Test the GetString() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetString_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow("ABC"));

            reader.Read();
            Assert.AreEqual("ABC", NullUtils.GetString(reader, "unit_test"),
                "GetString(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetStringOrNull() method on a null column
        /// </summary>
        [TestMethod]
        public void NullUtils_GetStringOrNull_Null_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow(DBNull.Value));

            reader.Read();
            Assert.IsNull(NullUtils.GetStringOrNull(reader, "unit_test"),
                "GetStringOrNull(null) should have returned null");
        }

        /// <summary>
        /// Test the GetStringOrNull() method on a column with a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetStringOrNull_Value_Success()
        {
            var reader = new StubDataReader(new StubResultSet("unit_test").AddRow("123"));

            reader.Read();
            Assert.AreEqual("123", NullUtils.GetStringOrNull(reader, "unit_test"),
                "GetStringOrNull(value) did not return the expected value");
        }

        #endregion

        #region De-coalesce Nullable<?> to null Method Tests

        /// <summary>
        /// Test the GetNullOrBoolean() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrBoolean_Null_Success()
        {
            bool? value = null;
            Assert.IsNull(NullUtils.GetNullOrBoolean(value), "GetNullOrBoolean(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrBoolean() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrBoolean_Value_Success()
        {
            bool? value = true;
            Assert.IsTrue((bool)NullUtils.GetNullOrBoolean(value), "GetNullOrBoolean(value) should have returned true");
        }

        /// <summary>
        /// Test the GetNullOrDateTime() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrDateTime_Null_Success()
        {
            DateTime? value = null;
            Assert.IsNull(NullUtils.GetNullOrDateTime(value), "GetNullOrDateTime(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrDateTime() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrDateTime_Value_Success()
        {
            DateTime? value = new DateTime(1981, 1, 21);
            Assert.AreEqual(new DateTime(1981, 1, 21), NullUtils.GetNullOrDateTime(value),
                "GetNullOrDateTime(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrDecimal() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrDecimal_Null_Success()
        {
            decimal? value = null;
            Assert.IsNull(NullUtils.GetNullOrDecimal(value), "GetNullOrDecimal(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrDecimal() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrDecimal_Value_Success()
        {
            decimal? value = 444M;
            Assert.AreEqual(444M, NullUtils.GetNullOrDecimal(value),
                "GetNullOrDecimal(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrDouble() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrDouble_Null_Success()
        {
            double? value = null;
            Assert.IsNull(NullUtils.GetNullOrDouble(value), "GetNullOrDouble(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrDouble() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrDouble_Value_Success()
        {
            double? value = 321D;
            Assert.AreEqual(321D, NullUtils.GetNullOrDouble(value),
                "GetNullOrDouble(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrFloat() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrFloat_Null_Success()
        {
            float? value = null;
            Assert.IsNull(NullUtils.GetNullOrFloat(value), "GetNullOrFloat(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrFloat() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrFloat_Value_Success()
        {
            float? value = 291F;
            Assert.AreEqual(291F, NullUtils.GetNullOrFloat(value),
                "GetNullOrFloat(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrGuid() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrGuid_Null_Success()
        {
            Guid? value = null;
            Assert.IsNull(NullUtils.GetNullOrGuid(value), "GetNullOrGuid(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrGuid() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrGuid_Value_Success()
        {
            var guid = Guid.NewGuid();
            Guid? value = guid;
            Assert.AreEqual(guid, NullUtils.GetNullOrGuid(value),
                "GetNullOrGuid(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrInt16() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrInt16_Null_Success()
        {
            Int16? value = null;
            Assert.IsNull(NullUtils.GetNullOrInt16(value), "GetNullOrInt16(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrInt16() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrInt16_Value_Success()
        {
            Int16? value = (short)12;
            Assert.AreEqual((short)12, NullUtils.GetNullOrInt16(value),
                "GetNullOrInt16(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrInt32() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrInt32_Null_Success()
        {
            Int32? value = null;
            Assert.IsNull(NullUtils.GetNullOrInt32(value), "GetNullOrInt32(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrInt32() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrInt32_Value_Success()
        {
            Int32? value = 27;
            Assert.AreEqual(27, NullUtils.GetNullOrInt32(value),
                "GetNullOrInt32(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrInt64() method, expecting null
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrInt64_Null_Success()
        {
            Int64? value = null;
            Assert.IsNull(NullUtils.GetNullOrInt64(value), "GetNullOrInt64(null) should have returned null");
        }

        /// <summary>
        /// Test the GetNullOrInt64() method, expecting a value
        /// </summary>
        [TestMethod]
        public void NullUtils_GetNullOrInt64_Value_Success()
        {
            Int64? value = 5L;
            Assert.AreEqual(5L, NullUtils.GetNullOrInt64(value),
                "GetNullOrInt64(value) did not return the expected value");
        }

        #endregion

    }
}