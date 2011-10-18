namespace DatabaseAbstraction
{
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
    }
}