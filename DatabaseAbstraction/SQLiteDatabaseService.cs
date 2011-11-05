namespace DatabaseAbstraction
{
    using System.Data;
    using System.Data.SQLite;
    using DatabaseAbstraction.Interfaces;
    using System.Collections.Generic;

    /// <summary>
    /// A SQLite implementation of a database service
    /// </summary>
    public class SQLiteDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the SQLite database service
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the database connection
        /// </param>
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service
        /// </param>
        public SQLiteDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : this(connectionString, null, classes) { }

        /// <summary>
        /// Constructor for the SQLite database service
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
        public SQLiteDatabaseService(string connectionString, List<IQueryFragmentProvider> fragments,
            params IQueryLibrary[] classes)
            : base(fragments, classes)
        {
            // Connect to the database.
            Connection = new SQLiteConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the last INTEGER PRIMARY KEY auto-incrementing inserted value
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public int LastIdentity()
        {
            using (IDataReader reader = SelectOne("database.identity.sqlite"))
                return (reader.Read()) ? reader.GetInt32(0) : 0;
        }
    }
}