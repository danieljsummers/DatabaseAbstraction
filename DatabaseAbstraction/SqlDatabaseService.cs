namespace DatabaseAbstraction {

    using System;
    using System.Data.SqlClient;
    using DatabaseAbstraction.Interfaces;

    /// <summary>
    /// A SQL Server implementation of a database service.
    /// </summary>
    public class SqlDatabaseService : DatabaseService, IDatabaseService {

        /// <summary>
        /// Constructor for the SQL Server database service.
        /// </summary>
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service.
        /// </param>
        public SqlDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : base(classes) {

            // Connect to the database.
            Connection = new SqlConnection(connectionString);
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
        public int Sequence(string sequenceName) {
            throw new InvalidOperationException("SQL Server does not support sequences");
        }
    }
}