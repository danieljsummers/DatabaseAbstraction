namespace DatabaseAbstraction.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This contains queries to support the database.
    /// It uses the "database" query namespace by default.
    /// </summary>
    public class DatabaseQueryProvider : IDatabaseQueryProvider
    {
        /// <summary>
        /// The prefix to use for the queries in this file
        /// </summary>
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }
        private string _prefix = "database.";

        #region Main

        /// <summary>
        /// Fill the query library with queries from this class
        /// </summary>
        /// <param name="queryLibrary">
        /// The query library being built
        /// </param>
        public void Queries(IDictionary<string, DatabaseQuery> queries)
        {
            // Select
            queries.Add(Prefix + "sequence.postgres", SequencePostgres());
            queries.Add(Prefix + "sequence.sqlserver", SequenceSqlServer());
            queries.Add(Prefix + "sequence.mysql", SequenceMySql());
            queries.Add(Prefix + "sequence.generic", SequenceGeneric());
            queries.Add(Prefix + "identity.sqlserver", IdentitySqlServer());
            queries.Add(Prefix + "identity.mysql", IdentityMySql());
            queries.Add(Prefix + "identity.sqlite", IdentitySQLite());
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