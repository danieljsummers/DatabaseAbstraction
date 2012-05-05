﻿namespace DatabaseAbstraction.Tests.Utils
{
    using System;
    using DatabaseAbstraction.Utils;
    using DatabaseAbstraction.Utils.UnitTest;
    using NUnit.Framework;

    [TestFixture]
    public class NullUtilsTest
    {
        #region Data Reader Get Method Tests

        /// <summary>
        /// Test the GetBoolean() method
        /// </summary>
        [Test]
        public void GetBoolean()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(true);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.False(NullUtils.GetBoolean(reader, "unit_test"), "GetBoolean(null) should have returned false");

            reader.Read();
            Assert.True(NullUtils.GetBoolean(reader, "unit_test"), "GetBoolean(true) should have returned true");
        }

        /// <summary>
        /// Test the GetBooleanOrNull() method
        /// </summary>
        [Test]
        public void GetBooleanOrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(true);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetBooleanOrNull(reader, "unit_test"),
                "GetBooleanOrNull(null) should have returned false");

            reader.Read();
            Assert.True(NullUtils.GetBooleanOrNull(reader, "unit_test").Value,
                "GetBooleanOrNull(true) should have returned true");
        }

        /// <summary>
        /// Test the GetDateTime() method
        /// </summary>
        [Test]
        public void GetDateTime()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(new DateTime(2004, 12, 13));
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.NotNull(NullUtils.GetDateTime(reader, "unit_test"),
                "GetDateTime(null) should not have returned null");

            reader.Read();
            Assert.AreEqual(new DateTime(2004, 12, 13), NullUtils.GetDateTime(reader, "unit_test"),
                "GetDateTime(value) should have returned a value");
        }

        /// <summary>
        /// Test the GetDateTimeOrNull() method
        /// </summary>
        [Test]
        public void GetDateTimeOrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(new DateTime(2004, 12, 13));
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetDateTimeOrNull(reader, "unit_test"),
                "GetDateTime(null) should have returned null");

            reader.Read();
            Assert.AreEqual(new DateTime(2004, 12, 13), NullUtils.GetDateTimeOrNull(reader, "unit_test").Value,
                "GetDateTimeOrNull(value) should have returned a value");
        }

        /// <summary>
        /// Test the GetDecimal() method
        /// </summary>
        [Test]
        public void GetDecimal()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(17M);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual(0M, NullUtils.GetDecimal(reader, "unit_test"),
                "GetDecimal(null) did not return the expected value");

            reader.Read();
            Assert.AreEqual(17M, NullUtils.GetDecimal(reader, "unit_test"),
                "GetDecimal(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetDecimalOrNull() method
        /// </summary>
        [Test]
        public void GetDecimalOrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(22M);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetDecimalOrNull(reader, "unit_test"),
                "GetDecimalOrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual(22M, NullUtils.GetDecimalOrNull(reader, "unit_test").Value,
                "GetDecimalOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetDouble() method
        /// </summary>
        [Test]
        public void GetDouble()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(4D);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual(0D, NullUtils.GetDouble(reader, "unit_test"),
                "GetDouble(null) did not return the expected value");

            reader.Read();
            Assert.AreEqual(4D, NullUtils.GetDouble(reader, "unit_test"),
                "GetDouble(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetDoubleOrNull() method
        /// </summary>
        [Test]
        public void GetDoubleOrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(9D);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetDoubleOrNull(reader, "unit_test"),
                "GetDoubleOrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual(9D, NullUtils.GetDoubleOrNull(reader, "unit_test").Value,
                "GetDoubleOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetFloat() method
        /// </summary>
        [Test]
        public void GetFloat()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(6F);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual(0F, NullUtils.GetFloat(reader, "unit_test"),
                "GetFloat(null) did not return the expected value");

            reader.Read();
            Assert.AreEqual(6F, NullUtils.GetFloat(reader, "unit_test"),
                "GetFloat(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetFloatOrNull() method
        /// </summary>
        [Test]
        public void GetFloatOrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(2F);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetFloatOrNull(reader, "unit_test"),
                "GetFloatOrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual(2F, NullUtils.GetFloatOrNull(reader, "unit_test").Value,
                "GetFloatOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetGuid() method
        /// </summary>
        [Test]
        public void GetGuid()
        {
            var guid = Guid.NewGuid();
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(guid);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual(Guid.Empty, NullUtils.GetGuid(reader, "unit_test"),
                "GetGuid(null) should not have returned null");

            reader.Read();
            Assert.AreEqual(guid, NullUtils.GetGuid(reader, "unit_test"),
                "GetGuid(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetGuidOrNull() method
        /// </summary>
        [Test]
        public void GetGuidOrNull()
        {
            var guid = Guid.NewGuid();
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(guid);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetGuidOrNull(reader, "unit_test"),
                "GetGuidOrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual(guid, NullUtils.GetGuidOrNull(reader, "unit_test").Value,
                "GetGuidOrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt16() method
        /// </summary>
        [Test]
        public void GetInt16()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow((short)78);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual((short)0, NullUtils.GetInt16(reader, "unit_test"),
                "GetInt16(null) did not return the expected value");

            reader.Read();
            Assert.AreEqual((short)78, NullUtils.GetInt16(reader, "unit_test"),
                "GetInt16(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt16OrNull() method
        /// </summary>
        [Test]
        public void GetInt16OrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow((short)17);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetInt16OrNull(reader, "unit_test"),
                "GetInt16OrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual((short)17, NullUtils.GetInt16OrNull(reader, "unit_test").Value,
                "GetInt16OrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt32() method
        /// </summary>
        [Test]
        public void GetInt32()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(24);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual(0, NullUtils.GetInt32(reader, "unit_test"),
                "GetInt32(null) did not return the expected value");

            reader.Read();
            Assert.AreEqual(24, NullUtils.GetInt32(reader, "unit_test"),
                "GetInt32(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt32OrNull() method
        /// </summary>
        [Test]
        public void GetInt32OrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(7);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetInt32OrNull(reader, "unit_test"),
                "GetInt32OrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual(7, NullUtils.GetInt32OrNull(reader, "unit_test").Value,
                "GetInt32OrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt64() method
        /// </summary>
        [Test]
        public void GetInt64()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(5L);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual(0L, NullUtils.GetInt64(reader, "unit_test"),
                "GetInt64(null) did not return the expected value");

            reader.Read();
            Assert.AreEqual(5L, NullUtils.GetInt64(reader, "unit_test"),
                "GetInt64(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetInt64OrNull() method
        /// </summary>
        [Test]
        public void GetInt64OrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow(64L);
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetInt64OrNull(reader, "unit_test"),
                "GetInt64OrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual(64L, NullUtils.GetInt64OrNull(reader, "unit_test").Value,
                "GetInt64OrNull(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetString() method
        /// </summary>
        [Test]
        public void GetString()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow("ABC");
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.AreEqual(String.Empty, NullUtils.GetString(reader, "unit_test"),
                "GetString(null) did not return the expected value");

            reader.Read();
            Assert.AreEqual("ABC", NullUtils.GetString(reader, "unit_test"),
                "GetString(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetStringOrNull() method
        /// </summary>
        [Test]
        public void GetStringOrNull()
        {
            var result = new StubResultSet("unit_test");
            result.AddRow(DBNull.Value);
            result.AddRow("123");
            var reader = new StubDataReader(result);

            reader.Read();
            Assert.Null(NullUtils.GetStringOrNull(reader, "unit_test"),
                "GetStringOrNull(null) should have returned null");

            reader.Read();
            Assert.AreEqual("123", NullUtils.GetStringOrNull(reader, "unit_test"),
                "GetStringOrNull(value) did not return the expected value");
        }

        #endregion

        #region De-coalesce Nullable<?> to null Method Tests

        /// <summary>
        /// Test the GetNullOrBoolean() method.
        /// </summary>
        [Test]
        public void GetNullOrBoolean()
        {
            bool? value = null;
            Assert.Null(NullUtils.GetNullOrBoolean(value), "GetNullOrBoolean(null) should have returned null");

            value = true;
            Assert.True((bool)NullUtils.GetNullOrBoolean(value), "GetNullOrBoolean(value) should have returned true");
        }

        /// <summary>
        /// Test the GetNullOrDateTime() method.
        /// </summary>
        [Test]
        public void GetNullOrDateTime()
        {
            DateTime? value = null;
            Assert.Null(NullUtils.GetNullOrDateTime(value), "GetNullOrDateTime(null) should have returned null");

            value = new DateTime(1981, 1, 21);
            Assert.AreEqual(new DateTime(1981, 1, 21), NullUtils.GetNullOrDateTime(value),
                "GetNullOrDateTime(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrDecimal() method.
        /// </summary>
        [Test]
        public void GetNullOrDecimal()
        {
            decimal? value = null;
            Assert.Null(NullUtils.GetNullOrDecimal(value), "GetNullOrDecimal(null) should have returned null");

            value = 444M;
            Assert.AreEqual(444M, NullUtils.GetNullOrDecimal(value),
                "GetNullOrDecimal(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrDouble() method.
        /// </summary>
        [Test]
        public void GetNullOrDouble()
        {
            double? value = null;
            Assert.Null(NullUtils.GetNullOrDouble(value), "GetNullOrDouble(null) should have returned null");

            value = 321D;
            Assert.AreEqual(321D, NullUtils.GetNullOrDouble(value),
                "GetNullOrDouble(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrFloat() method.
        /// </summary>
        [Test]
        public void GetNullOrFloat()
        {
            float? value = null;
            Assert.Null(NullUtils.GetNullOrFloat(value), "GetNullOrFloat(null) should have returned null");

            value = 291F;
            Assert.AreEqual(291F, NullUtils.GetNullOrFloat(value),
                "GetNullOrFloat(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrGuid() method.
        /// </summary>
        [Test]
        public void GetNullOrGuid()
        {
            Guid? value = null;
            Assert.Null(NullUtils.GetNullOrGuid(value), "GetNullOrGuid(null) should have returned null");

            var guid = Guid.NewGuid();
            value = guid;
            Assert.AreEqual(guid, NullUtils.GetNullOrGuid(value),
                "GetNullOrGuid(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrInt16() method.
        /// </summary>
        [Test]
        public void GetNullOrInt16()
        {
            Int16? value = null;
            Assert.Null(NullUtils.GetNullOrInt16(value), "GetNullOrInt16(null) should have returned null");

            value = (short)12;
            Assert.AreEqual((short)12, NullUtils.GetNullOrInt16(value),
                "GetNullOrInt16(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrInt32() method.
        /// </summary>
        [Test]
        public void GetNullOrInt32()
        {
            Int32? value = null;
            Assert.Null(NullUtils.GetNullOrInt32(value), "GetNullOrInt32(null) should have returned null");

            value = 27;
            Assert.AreEqual(27, NullUtils.GetNullOrInt32(value),
                "GetNullOrInt32(value) did not return the expected value");
        }

        /// <summary>
        /// Test the GetNullOrInt64() method.
        /// </summary>
        [Test]
        public void GetNullOrInt64()
        {
            Int64? value = null;
            Assert.Null(NullUtils.GetNullOrInt64(value), "GetNullOrInt64(null) should have returned null");

            value = 5L;
            Assert.AreEqual(5L, NullUtils.GetNullOrInt64(value),
                "GetNullOrInt64(value) did not return the expected value");
        }

        #endregion

    }
}