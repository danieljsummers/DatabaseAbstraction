namespace DatabaseAbstraction
{
    using System;
    using DatabaseAbstraction.Interfaces;
    using MySql.Data.MySqlClient;

    /// <summary>
    /// A MySQL implementation of a database service.
    /// </summary>
    public class MySqlDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the MySQL database service.
        /// </summary>
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service.
        /// </param>
        public MySqlDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : base(classes)
        {
            // Connect to the database.
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence 
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// An exception; MySQL does not support sequences
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// MySQL does not support sequences
        /// </exception>
        // FIME: wrap mysql_last_insert_id
        public int Sequence(string sequenceName)
        {
            throw new InvalidOperationException("MySQL does not support sequences");
        }
    }
}