namespace DatabaseAbstraction
{
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;
    using Npgsql;
    using System;

    /// <summary>
    /// A PostgreSQL implementation of a database service
    /// </summary>
    public class PostgresDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the PostgreSQL database service
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the database connection
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public PostgresDatabaseService(string connectionString, params Type[] providers)
            : base(providers)
        {
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence (int)
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The value of the sequence
        /// </returns>
        public override int Sequence(string sequenceName)
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.postgres",
                    DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return IntValue(reader, reader.GetOrdinal("sequence_value"));
        }

        /// <summary>
        /// Get the next value in a database sequence (long)
        /// </summary>
        /// <remarks>
        /// Newer versions of PostgreSQL are returning SERIAL columns as longs, so we try long first
        /// </remarks>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The value of the sequence
        /// </returns>
        public override long LongSequence(string sequenceName)
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.postgres",
                    DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return LongValue(reader, reader.GetOrdinal("sequence_value"));
        }

        /// <summary>
        /// Get the last inserted identity value (int - N/A for PostgreSQL)
        /// </summary>
        /// <returns>
        /// Nothing but an exception
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// PostgreSQL does not support last identity; use Sequence() with the sequence name instead
        /// </exception>
        public override int LastIdentity()
        {
            throw new InvalidOperationException(
                "PostgreSQL does not support last identity; use Sequence() with the sequence name instead");
        }

        /// <summary>
        /// Get the last inserted identity value (long - N/A for PostgreSQL)
        /// </summary>
        /// <returns>
        /// Nothing but an exception
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// PostgreSQL does not support last identity; use Sequence() with the sequence name instead
        /// </exception>
        public override long LongLastIdentity()
        {
            return Convert.ToInt64(LastIdentity());
        }
    }
}