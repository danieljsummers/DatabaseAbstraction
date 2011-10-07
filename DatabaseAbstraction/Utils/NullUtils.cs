namespace DatabaseAbstraction.Utils {

    using System;
    using System.Data;

    /// <summary>
    /// Convenience methods for coalescing DbNull to null when dealing with data readers
    /// </summary>
    public sealed class NullUtils {

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
        public static bool GetBoolean(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return false;
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
        public static bool? GetBooleanOrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static DateTime GetDateTime(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return new DateTime();
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
        public static DateTime? GetDateTimeOrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static decimal GetDecimal(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return 0M;
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
        public static decimal? GetDecimalOrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static double GetDouble(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return 0D;
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
        public static double? GetDoubleOrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static float GetFloat(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return 0F;
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
        public static float? GetFloatOrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static Guid GetGuid(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return Guid.Empty;
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
        public static Guid? GetGuidOrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static Int16 GetInt16(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return 0;
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
        public static Int16? GetInt16OrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static Int32 GetInt32(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return 0;
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
        public static Int32? GetInt32OrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static Int64 GetInt64(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return 0L;
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
        public static Int64? GetInt64OrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
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
        public static string GetString(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return String.Empty;
            return reader.GetString(reader.GetOrdinal(columnName));
        }

        /// <summary>
        /// Get a possibly-null String value
        /// </summary>
        /// <param name="reader">
        /// The data reader with the data
        /// </param>
        /// <param name="columnName">
        /// The name of the column
        /// </param>
        /// <returns>
        /// null if the column is null, the String value if not
        /// </returns>
        public static string GetStringOrNull(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetString(reader.GetOrdinal(columnName));
        }
    }
}