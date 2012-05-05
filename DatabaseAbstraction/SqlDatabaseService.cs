namespace DatabaseAbstraction
{
    using System;
    using System.Data.SqlClient;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Utils;

    /// <summary>
    /// A SQL Server implementation of a database service.
    /// </summary>
    public class SqlDatabaseService : DatabaseService, IDatabaseService
    {
        /// <summary>
        /// Constructor for the SQL Server database service
        /// </summary>
        /// <param name="connectionString">
        /// The string to use when creating the database connection
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public SqlDatabaseService(string connectionString, params Type[] providers)
            : base(providers)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Get the next value in a database sequence 
        /// </summary>
        /// <remarks>
        /// Technically, an IDENTITY column in SQL Server has its values assigned by the database.  This implementation
        /// retrieves the current IDENTITY value and increments it by 1.  Note that SQL Server IDENTITY values can have
        /// a different increment; however, this will mimic the traditional implementation of sequences in other
        /// databases.
        /// </remarks>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The current identity value plus one; zero if nothing found
        /// </returns>
        public override int Sequence(string sequenceName)
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.sqlserver",
                                          DbUtils.SingleParameter("[]sequence_name", sequenceName)))
                return (reader.Read()) ? reader.GetInt32(reader.GetOrdinal("sequence_value")) + 1 : 0;
        }

        /// <summary>
        /// Get the current SCOPE_IDENTITY value
        /// </summary>
        /// <returns>
        /// The last value, or 0 if not found
        /// </returns>
        public int LastIdentity()
        {
            using (var reader = SelectOne(DatabaseQueryPrefix + "identity.sqlserver"))
                return (reader.Read()) ? reader.GetInt32(0) : 0;
        }
    }
}