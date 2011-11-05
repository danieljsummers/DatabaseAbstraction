namespace DatabaseAbstraction
{
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;
    using MySql.Data.MySqlClient;

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
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service
        /// </param>
        public MySqlDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : this(connectionString, null, classes) { }

        /// <summary>
        /// Constructor for the MySQL database service
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the database connection
        /// </param>
        /// <param name="fragments">
        /// The classes providing query fragments for instance-level queries
        /// </param>
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service
        /// </param>
        public MySqlDatabaseService(string connectionString, List<IQueryFragmentProvider> fragments,
            params IQueryLibrary[] classes)
            : base(fragments, classes)
        {
            // Connect to the database.
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence 
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
            using (IDataReader reader = SelectOne("database.sequence.mysql",
                                                  DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return (reader.Read()) ? reader.GetInt32(reader.GetOrdinal("auto_increment")) : 0;
        }

        /// <summary>
        /// Get the last AUTO_INCREMENT inserted value
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public int LastIdentity()
        {
            using (IDataReader reader = SelectOne("database.identity.mysql"))
                return (reader.Read()) ? reader.GetInt32(0) : 0;
        }
    }
}