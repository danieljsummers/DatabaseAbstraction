namespace DatabaseAbstraction
{
    using System;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;
    using Npgsql;

    /// <summary>
    /// A PostgreSQL implementation of a database service
    /// </summary>
    public class PostgresDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the PostgreSQL database service
        /// </summary>
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service
        /// </param>
        public PostgresDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : base(classes)
        {
            // Connect to the database.
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence 
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The value of the sequence
        /// </returns>
        public override int Sequence(string sequenceName)
        {
            using (IDataReader reader = SelectOne("database.sequence.postgres",
                                                  DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return (reader.Read()) ? reader.GetInt32(reader.GetOrdinal("sequence_value")) : 0;
        }

        /// <summary>
        /// Get the last inserted identity value (N/A for PostgreSQL)
        /// </summary>
        /// <returns>
        /// Nothing but an exception
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// PostgreSQL does not support last identity; use Sequence() with the sequence name instead
        /// </exception>
        public int LastIdentity()
        {
            throw new InvalidOperationException(
                "PostgreSQL does not support last identity; use Sequence() with the sequence name instead");
        }
    }
}