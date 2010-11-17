namespace DatabaseAbstraction
{
    using System;
    using DatabaseAbstraction.Interfaces;
    using System.Data.SQLite;

    /// <summary>
    /// A SQL Server implementation of a database service.
    /// </summary>
    public class SQLiteDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the SQL Server database service.
        /// </summary>
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service.
        /// </param>
        public SQLiteDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : base(classes)
        {
            // Connect to the database.
            Connection = new SQLiteConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence 
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// An exception; SQL Server does not support sequences
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// SQL Server does not support sequences
        /// </exception>
        // FIXME: wrap sqlite_insert_id()
        public int Sequence(string sequenceName)
        {
            throw new InvalidOperationException("SQLite does not support sequences");
        }
    }
}