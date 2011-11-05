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
            queries.Add(PREFIX + "sequence.postgres", SequencePostgres());
            queries.Add(PREFIX + "sequence.sqlserver", SequenceSqlServer());
            queries.Add(PREFIX + "sequence.mysql", SequenceMySql());
            queries.Add(PREFIX + "sequence.generic", SequenceGeneric());
            queries.Add(PREFIX + "identity.sqlserver", IdentitySqlServer());
            queries.Add(PREFIX + "identity.mysql", IdentityMySql());
            queries.Add(PREFIX + "identity.sqlite", IdentitySQLite());
        }

        #endregion

        #region Select

        /// <summary>
        /// database.sequence.postgres
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery SequencePostgres()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = "SELECT currval('[]sequence_name_seq') AS sequence_value"
            };
            query.Parameters.Add("[]sequence_name", DbType.String);

            return query;
        }

        /// <summary>
        /// database.sequence.sqlserver
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery SequenceSqlServer()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = "SELECT IDENT_CURRENT('[]sequence_name') AS sequence_value"
            };
            query.Parameters.Add("[]sequence_name", DbType.String);

            return query;
        }

        /// <summary>
        /// database.sequence.mysql
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery SequenceMySql()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = "SHOW TABLE STATUS LIKE '[]sequence_name'"
            };
            query.Parameters.Add("[]sequence_name", DbType.String);

            return query;
        }

        /// <summary>
        /// database.sequence.generic
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery SequenceGeneric()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT MAX([]primary_key_name) AS max_pk
                FROM []table_name"
            };
            query.Parameters.Add("[]primary_key_name", DbType.String);
            query.Parameters.Add("[]table_name", DbType.String);

            return query;
        }

        /// <summary>
        /// database.identity.sqlserver
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery IdentitySqlServer()
        {
            return new DatabaseQuery
            {
                SQL = "SELECT SCOPE_IDENTITY()"
            };
        }

        /// <summary>
        /// database.identity.mysql
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery IdentityMySql()
        {
            return new DatabaseQuery
            {
                SQL = "SELECT LAST_INSERT_ID()"
            };
        }

        /// <summary>
        /// database.identity.sqlite
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery IdentitySQLite()
        {
            return new DatabaseQuery
            {
                SQL = "SELECT last_insert_rowid()"
            };
        }

        #endregion
    }
}