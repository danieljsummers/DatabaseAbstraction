namespace DatabaseAbstraction.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This contains queries to support the database.
    /// It uses the "database" query namespace.
    /// </summary>
    public sealed class DatabaseQueryLibrary : IQueryLibrary
    {
        private static string PREFIX = "database.";

        #region Main

        /// <summary>
        /// Fill the query library with queries from this class
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        public void GetQueries(Dictionary<string, DatabaseQuery> queries)
        {
            // Select
            addSequencePostgres(queries);
            addSequenceSqlServer(queries);
            addSequenceMySql(queries);
            addSequenceGeneric(queries);
            addIdentitySqlServer(queries);
            addIdentityMySql(queries);
            addIdentitySQLite(queries);
        }

        #endregion

        #region Select

        /// <summary>
        /// database.sequence.postgres
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addSequencePostgres(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "sequence.postgres";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SELECT currval('[]sequence_name_seq') AS sequence_value";

            queries[name].Parameters.Add("[]sequence_name", DbType.String);
        }

        /// <summary>
        /// database.sequence.sqlserver
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addSequenceSqlServer(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "sequence.sqlserver";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SELECT IDENT_CURRENT('[]sequence_name') AS sequence_value";

            queries[name].Parameters.Add("[]sequence_name", DbType.String);
        }

        /// <summary>
        /// database.sequence.mysql
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addSequenceMySql(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "sequence.mysql";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SHOW TABLE STATUS LIKE '[]sequence_name'";

            queries[name].Parameters.Add("[]sequence_name", DbType.String);
        }

        /// <summary>
        /// database.sequence.generic
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addSequenceGeneric(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "sequence.generic";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT MAX([]primary_key_name) AS max_pk
                FROM []table_name";

            queries[name].Parameters.Add("[]primary_key_name", DbType.String);
            queries[name].Parameters.Add("[]table_name", DbType.String);
        }

        /// <summary>
        /// database.identity.sqlserver
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addIdentitySqlServer(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "identity.sqlserver";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SELECT SCOPE_IDENTITY()";
        }

        /// <summary>
        /// database.identity.mysql
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addIdentityMySql(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "identity.mysql";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SELECT LAST_INSERT_ID()";
        }

        /// <summary>
        /// database.identity.sqlite
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addIdentitySQLite(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "identity.sqlite";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SELECT last_insert_rowid()";
        }

        #endregion
    }
}