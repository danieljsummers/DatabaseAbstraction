namespace DatabaseAbstraction.Utils
{
    using System;
    using System.Data;

    /// <summary>
    /// Convenience methods for coalescing DbNull to null when dealing with data readers
    /// </summary>
    public sealed class NullUtils
    {
        #region Data Reader Get Methods

        /// <summary>
        /// Get a possibly-null Boolean value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// false if the column is null, the Boolean value if not
        /// </returns>
        public static bool GetBoolean(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return false;
            return reader.GetBoolean(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Boolean value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Boolean value if not
        /// </returns>
        public static bool? GetBooleanOrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetBoolean(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null DateTime value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// A default DateTime if the column is null, the DateTime value if not
        /// </returns>
        public static DateTime GetDateTime(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return new DateTime();
            return reader.GetDateTime(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null DateTime value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the DateTime value if not
        /// </returns>
        public static DateTime? GetDateTimeOrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetDateTime(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Decimal value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// 0.0 if the column is null, the Decimal value if not
        /// </returns>
        public static decimal GetDecimal(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return 0M;
            return reader.GetDecimal(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Decimal value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Decimal value if not
        /// </returns>
        public static decimal? GetDecimalOrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetDecimal(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Double value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// 0.0 if the column is null, the Double value if not
        /// </returns>
        public static double GetDouble(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return 0D;
            return reader.GetDouble(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Double value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Double value if not
        /// </returns>
        public static double? GetDoubleOrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetDouble(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Float value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// 0.0 if the column is null, the Float value if not
        /// </returns>
        public static float GetFloat(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return 0F;
            return reader.GetFloat(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Float value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Float value if not
        /// </returns>
        public static float? GetFloatOrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetFloat(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Guid value, coaleseced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// A Guid with all zeroes if the column is null, the Guid value if not
        /// </returns>
        public static Guid GetGuid(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return Guid.Empty;
            return reader.GetGuid(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Guid value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Guid value if not
        /// </returns>
        public static Guid? GetGuidOrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetGuid(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Int16 value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// 0 if the column is null, the Int16 value if not
        /// </returns>
        public static Int16 GetInt16(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return (short)0;
            return reader.GetInt16(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Int16 value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Int16 value if not
        /// </returns>
        public static Int16? GetInt16OrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetInt16(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Int32 value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// 0 if the column is null, the Int32 value if not
        /// </returns>
        public static Int32 GetInt32(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return 0;
            return reader.GetInt32(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Int32 value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Int32 value if not
        /// </returns>
        public static Int32? GetInt32OrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetInt32(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Int64 value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// 0 if the column is null, the Int64 value if not
        /// </returns>
        public static Int64 GetInt64(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return 0L;
            return reader.GetInt64(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null Int64 value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the Int64 value if not
        /// </returns>
        public static Int64? GetInt64OrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetInt64(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null String value, coalesced to a non-null value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// An empty string if the column is null, the String value if not
        /// </returns>
        public static string GetString(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return String.Empty;
            return reader.GetString(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null String value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// 
        /// 
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the String value if not
        /// </returns>
        public static string GetStringOrNull(IDataReader reader, string columnName)
        {
            if (DBNull.Value.Equals(reader.GetValue(reader.GetOrdinal(columnName))))
                return null;
            return reader.GetString(reader.GetOrdinal(columnName));
        }

        #endregion

        #region De-coalese Nullable<?> to null Methods

        /// <summary>
        /// Get a boolean or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<bool> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrBoolean(bool? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get a DateTime or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<DateTime> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrDateTime(DateTime? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get a decimal or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<decimal> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrDecimal(decimal? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get a double or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<double> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrDouble(double? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get a float or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<float> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrFloat(float? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get a GUID or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<Guid> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrGuid(Guid? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get am Int16 or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<Int16> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrInt16(Int16? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get an Int32 or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<Int32> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrInt32(Int32? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        /// <summary>
        /// Get an Int64 or null value
        /// </summary>
        /// <param name="value">
        /// The Nullable<Int64> object
        /// </param>
        /// <returns>
        /// The value, or null if no value
        /// </returns>
        public static object GetNullOrInt64(Int64? value)
        {
            if (value.HasValue) return value;
            return null;
        }

        // string is an object - straight assignment will work for that data type

        #endregion
    }
}