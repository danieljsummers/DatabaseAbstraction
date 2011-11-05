namespace DatabaseAbstraction
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Odbc;
    using DatabaseAbstraction.Interfaces;

    /// <summary>
    /// An ODBC implementation of a database service
    /// </summary>
    public class OdbcDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the connection
        /// </param>
        /// <param name="classes">
        /// Zero or more <see cref="IQueryLibrary"/> classes with queries for this instance
        /// </param>
        public OdbcDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : this(connectionString, null, classes) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the connection
        /// </param>
        /// <param name="fragments">
        /// The classes providing query fragments for instance-level queries
        /// </param>
        /// <param name="classes">
        /// Zero or more <see cref="IQueryLibrary"/> classes with queries for this instance
        /// </param>
        public OdbcDatabaseService(string connectionString, List<IQueryFragmentProvider> fragments,
            params IQueryLibrary[] classes)
            : base(fragments, classes)
        {
            // Connect to the database
            Connection = new OdbcConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the last inserted identity value; ODBC users must provide their own implementation
        /// </summary>
        /// <returns>
        /// The identity if the query is defined; an exception if not.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// If there is no query defined
        /// </exception>
        public int LastIdentity()
        {
            if (!StaticQueries.ContainsKey("database.identity.odbc"))
                if (!Queries.ContainsKey("database.identity.odbc"))
                    throw new InvalidOperationException(
                        "There is no way for Database Abstraction to determine proper identity SQL syntax for ODBC "
                        + "connections.  If your data store supports it, define a \"database.identity.odbc\" query and "
                        + "provide it to the database service, and it will be used.");

            using (IDataReader reader = SelectOne("database.identity.odbc"))
                return (reader.Read()) ? reader.GetInt32(0) : 0;
        }
    }
}