namespace DatabaseAbstraction
{
    using DatabaseAbstraction.Interfaces;
    using System;
    using System.Data.SQLite;

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
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public SQLiteDatabaseService(string connectionString, params Type[] providers)
            : base(providers)
        {
            Connection = new SQLiteConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the last INTEGER PRIMARY KEY auto-incrementing inserted value (int)
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public override int LastIdentity()
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.sqlite"))
                return IntValue(reader, 0);
        }

        /// <summary>
        /// Get the last INTEGER PRIMARY KEY auto-incrementing inserted value (long)
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public override long LongLastIdentity()
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.sqlite"))
                return LongValue(reader, 0);
        }
    }
}