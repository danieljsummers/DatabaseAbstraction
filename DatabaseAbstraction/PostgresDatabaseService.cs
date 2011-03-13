namespace com.codeplex.dbabstraction.DatabaseAbstraction {

    using System;
    using System.Data;
    using com.codeplex.dbabstraction.DatabaseAbstraction.Interfaces;
    using com.codeplex.dbabstraction.DatabaseAbstraction.Utils;
    using Npgsql;

    /// <summary>
    /// A PostgreSQL implementation of a database service.
    /// </summary>
    public class PostgresDatabaseService : DatabaseService, IDatabaseService, IDisposable {

        /// <summary>
        /// Constructor for the PostgreSQL database service.
        /// </summary>
        /// <param name="classes">
        /// Classes that contain query libraries to use when initializing the service.
        /// </param>
        public PostgresDatabaseService(string connectionString, params IQueryLibrary[] classes)
            : base(classes) {

            // Connect to the database.
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
        }


        /// <summary>
        /// Get the next value in a database sequence 
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence
        /// </param>
        /// <returns>
        /// The value of the sequence
        /// </returns>
        public int Sequence(string sequenceName) {

            using (IDataReader reader = SelectOne("database.sequence.postgres",
                                                  DbUtils.SingleParameter("[]sequence_name", sequenceName))) {
                if (reader.NextResult()) {
                    reader.Read();
                    return Convert.ToInt32(reader["sequence_value"]);
                }
                else {
                    return 0;
                }
            }
        }
    }
}