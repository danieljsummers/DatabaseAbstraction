namespace DatabaseAbstraction.Interfaces
{
    using System.Collections.Generic;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This defines methods required for classes that provide query fragments to use in combination with
    /// <see cref="IDatabaseQueryProvider"/>s to create queries
    /// </summary>
    public interface IQueryFragmentProvider
    {
        /// <summary>
        /// The query fragments provided by this class
        /// </summary>
        /// <param name="fragments">
        /// The fragment collection being built
        /// </param>
        void Fragments(IDictionary<string, QueryFragment> fragments);
    }
}