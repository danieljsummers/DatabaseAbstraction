namespace DatabaseAbstraction.Interfaces {

    using System;
    using System.Collections.Generic;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// Defines the required methods for a query library
    /// </summary>
    public interface IQueryLibrary {

        /// <summary>
        /// Get the queries associated with the query library
        /// </summary>
        /// <param name="queries">
        /// The query library being built (queries will be appended)
        /// </param>
        void GetQueries(Dictionary<string, DatabaseQuery> queries);
    }
}