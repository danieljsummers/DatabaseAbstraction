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
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        IDataReader Select(string queryName);

        /// <summary>
        /// Select a result set
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        IDataReader Select(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Select a result set
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        IDataReader Select(string queryName, IParameterProvider model);

        /// <summary>
        /// Select a single result
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        IDataReader SelectOne(string queryName);

        /// <summary>
        /// Select a single result
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        IDataReader SelectOne(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Select a single result
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        IDataReader SelectOne(string queryName, IParameterProvider model);

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        void Insert(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        void Insert(string queryName, IParameterProvider model);

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        void Update(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        void Update(string queryName, IParameterProvider model);

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        void Delete(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        void Delete(string queryName, IParameterProvider model);

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