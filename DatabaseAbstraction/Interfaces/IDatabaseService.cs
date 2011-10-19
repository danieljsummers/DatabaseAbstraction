namespace DatabaseAbstraction.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// This interface describes the functions that must exist in a DatabaseService implementation.
    /// </summary>
    public interface IDatabaseService : IDisposable
    {
        /// <summary>
        /// Select a result set
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <returns>
        /// A <see cref="System.Data.IDataReader"/> with the results
        /// </returns>
        IDataReader Select(string queryName);

        /// <summary>
        /// Select a result set
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        /// <returns>
        /// A <see cref="System.Data.IDataReader"/> with the results
        /// </returns>
        IDataReader Select(string queryName, Dictionary<string, object> parameters);

        /// <summary>
        /// Select a result set
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        /// <returns>
        /// A <see cref="System.Data.IDataReader"/> with the results
        /// </returns>
        IDataReader Select(string queryName, IDatabaseModel model);

        /// <summary>
        /// Select a single result
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <returns>
        /// A <see cref="System.Data.IDataReader"/> with the results
        /// </returns>
        IDataReader SelectOne(string queryName);

        /// <summary>
        /// Select a single result
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        /// <returns>
        /// A <see cref="System.Data.IDataReader"/> with the results
        /// </returns>
        IDataReader SelectOne(string queryName, Dictionary<string, object> parameters);

        /// <summary>
        /// Select a single result
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        /// <returns>
        /// A <see cref="System.Data.IDataReader"/> with the results
        /// </returns>
        IDataReader SelectOne(string queryName, IDatabaseModel model);

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        void Insert(string queryName, Dictionary<string, object> parameters);

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="model">
        /// The <see cref=""/> model from which parameters may be obtained
        /// </param>
        void Insert(string queryName, IDatabaseModel model);

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        void Update(string queryName, Dictionary<string, object> parameters);

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        void Update(string queryName, IDatabaseModel model);

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        void Delete(string queryName, Dictionary<string, object> parameters);

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IQueryLibrary"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        void Delete(string queryName, IDatabaseModel model);

        /// <summary>
        /// Get a sequence from the database
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence to query
        /// </param>
        /// <returns>
        /// The sequence value
        /// </returns>
        int Sequence(string sequenceName);

        /// <summary>
        /// Get the last auto-incremented / auto-numbered / identity value
        /// </summary>
        /// <returns>
        /// The last identity value
        /// </returns>
        int LastIdentity();
    }
}