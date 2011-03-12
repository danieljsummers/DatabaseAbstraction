namespace com.codeplex.dbabstraction.DatabaseAbstraction.Models {

    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// A database query.
    /// </summary>
    public class DatabaseQuery {

        /// <summary>
        /// The name by which the query is known.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The SQL for the query.
        /// </summary>
        public string SQL { get; set; }

        /// <summary>
        /// The parameters for the query.
        /// </summary>
        public Dictionary<string, DbType> Parameters { get; private set; }

        /// <summary>
        /// Constructor for the query object.
        /// </summary>
        public DatabaseQuery() {
            Parameters = new Dictionary<string, DbType>();
        }
    }
}