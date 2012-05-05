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
        /// <summary>
        /// Constructor for the ODBC database service
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the connection
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public OdbcDatabaseService(string connectionString, params Type[] providers)
            : base(providers)
        {
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
            if (!StaticQueries.ContainsKey(DatabaseQueryPrefix + "identity.odbc"))
                if (!Queries.ContainsKey(DatabaseQueryPrefix + "identity.odbc"))
                    throw new InvalidOperationException(String.Format(
                        "There is no way for Database Abstraction to determine proper identity SQL syntax for ODBC "
                        + "connections.  If your data store supports it, define a \"{0}identity.odbc\" query and "
                        + "provide it to the database service, and it will be used.", DatabaseQueryPrefix));

            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.odbc"))
                return (reader.Read()) ? reader.GetInt32(0) : 0;
        }
    }
}