namespace DatabaseAbstraction
{
    using System;
    using System.Data.Odbc;
    using DatabaseAbstraction.Interfaces;

    /// <summary>
    /// An ODBC implementation of a database service
    /// </summary>
    public class OdbcDatabaseService : DatabaseService, IDatabaseService
    {
        public OdbcDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : base(classes)
        {
            // Connect to the database
            Connection = new OdbcConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence 
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// An exception; ODBC does not support sequences
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// ODBC does not support sequences
        /// </exception>
        public int Sequence(string sequenceName)
        {
            throw new InvalidOperationException("ODBC does not support sequences");
        }
    }
}