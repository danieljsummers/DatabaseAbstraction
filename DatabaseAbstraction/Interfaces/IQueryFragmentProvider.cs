namespace DatabaseAbstraction.Interfaces
{
    using System.Collections.Generic;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This defines methods required for models to provide query fragments for a
    /// <see cref="DatabaseAbstraction.Interfaces.IQueryFragmentProvider"/> implementation
    /// </summary>
    public interface IQueryFragmentProvider
    {
        /// <summary>
        /// The query fragments provided by this class
        /// </summary>
        /// <param name="fragments">
        /// The fragment collection being built
        /// </param>
        void Fragments(Dictionary<string, QueryFragment> fragments);
    }
}