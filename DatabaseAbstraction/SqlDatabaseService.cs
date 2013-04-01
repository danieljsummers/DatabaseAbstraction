namespace DatabaseAbstraction
{
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;
    using System;
    using System.Data.SqlClient;

    /// <summary>
    /// A SQL Server implementation of a database service.
    /// </summary>
    public class SqlDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the SQL Server database service
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the database connection
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public SqlDatabaseService(string connectionString, params Type[] providers)
            : base(providers)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence (int)
        /// </summary>
        /// <remarks>
        /// Technically, an IDENTITY column in SQL Server has its values assigned by the database.  This implementation
        /// retrieves the current IDENTITY value and increments it by 1.  Note that SQL Server IDENTITY values can have
        /// a different increment; however, this will mimic the traditional implementation of sequences in other
        /// databases.
        /// </remarks>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The current identity value plus one; zero if nothing found
        /// </returns>
        public override int Sequence(string sequenceName)
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.sqlserver",
                    DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return IntValue(reader, reader.GetOrdinal("sequence_value")) + 1;
        }

        /// <summary>
        /// Get the next value in a database sequence (long)
        /// </summary>
        /// <remarks>
        /// Technically, an IDENTITY column in SQL Server has its values assigned by the database.  This implementation
        /// retrieves the current IDENTITY value and increments it by 1.  Note that SQL Server IDENTITY values can have
        /// a different increment; however, this will mimic the traditional implementation of sequences in other
        /// databases.
        /// </remarks>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The current identity value plus one; zero if nothing found
        /// </returns>
        public override long LongSequence(string sequenceName)
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.sqlserver",
                    DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return LongValue(reader, reader.GetOrdinal("sequence_value")) + 1;
        }

        /// <summary>
        /// Get the current SCOPE_IDENTITY value (int)
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public override int LastIdentity()
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.sqlserver"))
                return IntValue(reader, 0);
        }

        /// <summary>
        /// Get the current SCOPE_IDENTITY value (long)
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public override long LongLastIdentity()
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.sqlserver"))
                return LongValue(reader, 0);
        }
    }
}