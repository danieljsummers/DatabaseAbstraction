namespace DatabaseAbstraction
{
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;
    using MySql.Data.MySqlClient;
    using System;

    /// <summary>
    /// A MySQL implementation of a database service
    /// </summary>
    public class MySqlDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the MySQL database service
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the database connection
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public MySqlDatabaseService(string connectionString, params Type[] providers)
            : base(providers)
        {
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence (int)
        /// </summary>
        /// <remarks>
        /// This uses the SHOW TABLE STATUS command in MySQL.  MySQL does not support traditional sequences like
        /// PostgreSQL or Oracle; this will return the next value that will be assigned, but does not reserve it.
        /// </remarks>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The next AUTO_INCREMENT value for the table specified
        /// </returns>
        public override int Sequence(string sequenceName)
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.mysql",
                    DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return IntValue(reader, reader.GetOrdinal("auto_increment"));
        }

        /// <summary>
        /// Get the next value in a database sequence (long)
        /// </summary>
        /// <remarks>
        /// This uses the SHOW TABLE STATUS command in MySQL.  MySQL does not support traditional sequences like
        /// PostgreSQL or Oracle; this will return the next value that will be assigned, but does not reserve it.
        /// </remarks>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The next AUTO_INCREMENT value for the table specified
        /// </returns>
        public override long LongSequence(string sequenceName)
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.mysql",
                    DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return LongValue(reader, reader.GetOrdinal("auto_increment"));
        }

        /// <summary>
        /// Get the last AUTO_INCREMENT inserted value (int)
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public override int LastIdentity()
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.mysql"))
                return IntValue(reader, 0);
        }

        /// <summary>
        /// Get the last AUTO_INCREMENT inserted value (long)
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public override long LongLastIdentity()
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.mysql"))
                return LongValue(reader, 0);
        }
    }
}