namespace DatabaseAbstraction.Queries {

    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This contains queries to support the database.
    /// It uses the "database" query namespace.
    /// </summary>
    public sealed class DatabaseQueryLibrary : IQueryLibrary {

        private static string PREFIX = "database.";

        #region Main

        public void GetQueries(Dictionary<string, DatabaseQuery> queries) {

            // Select.
            addSequencePostgres(queries);
        }

        #endregion

        #region Select

        /// <summary>
        /// database.sequence.postgres
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addSequencePostgres(Dictionary<string, DatabaseQuery> queries) {

            string name = PREFIX + "sequence.postgres";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SELECT currval('[]sequence_name_seq') AS sequence_value";

            queries[name].Parameters.Add("[]sequence_name", DbType.String);
        }

        /// <summary>
        /// database.sequence.oracle
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addSequenceOracle(Dictionary<string, DatabaseQuery> queries) {

            string name = PREFIX + "sequence.oracle";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = "SELECT []sequence_name.NEXTVAL AS sequence_value FROM DUAL";

            queries[name].Parameters.Add("[]sequence_name", DbType.String);
        }

        #endregion
    }
}