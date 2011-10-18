namespace DatabaseAbstraction
{
    using System.Data.SQLite;
    using DatabaseAbstraction.Interfaces;

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
    }
}