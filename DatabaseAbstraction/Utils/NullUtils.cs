namespace DatabaseAbstraction.Utils {

    using System;
    using System.Data;

    /// <summary>
    /// Convenience methods for coalescing DbNull to null when dealing with data readers
    /// </summary>
    public sealed class NullUtils {

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
        public static bool? GetBoolean(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetBoolean(reader.GetOrdinal(columnName));
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
        public static DateTime? GetDateTime(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetDateTime(reader.GetOrdinal(columnName));
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
        public static decimal? GetDecimal(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetDecimal(reader.GetOrdinal(columnName));
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
        public static double? GetDouble(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetDouble(reader.GetOrdinal(columnName));
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
        public static float? GetFloat(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetFloat(reader.GetOrdinal(columnName));
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
        public static Guid? GetGuid(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetGuid(reader.GetOrdinal(columnName));
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
        public static Int16? GetInt16(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetInt16(reader.GetOrdinal(columnName));
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
        public static Int32? GetInt32(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetInt32(reader.GetOrdinal(columnName));
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
        public static Int64? GetInt64(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetInt64(reader.GetOrdinal(columnName));
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
        public static string GetString(IDataReader reader, string columnName) {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetString(reader.GetOrdinal(columnName));
        }
    }
}