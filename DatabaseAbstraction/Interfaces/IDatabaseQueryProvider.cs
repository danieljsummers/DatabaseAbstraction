namespace DatabaseAbstraction.Interfaces
{
    using System.Collections.Generic;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// Defines methods required for a query provider
    /// </summary>
    public interface IDatabaseQueryProvider
    {
        /// <summary>
        /// The prefix to use for queries in this library
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        /// Get the queries provided by this class
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        void Queries(IDictionary<string, DatabaseQuery> queries);
    }
}