namespace DatabaseAbstraction.Async.Interfaces
{
    using DatabaseAbstraction.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// Methods required for an asynchronous database service implementation
    /// </summary>
    [Obsolete("This interface's usage is no longer necessary.  Adding a reference to DatabaseAbstraction.Async.dll will provide the extensions for the IDatabaseService interface without changing the name.")]
    public interface IDatabaseServiceAsync : IDisposable
    {
        /// <summary>
        /// The concrete database service which will be enabled for asynchronous retrieval
        /// </summary>
        IDatabaseService Service { get; }

        /// <summary>
        /// Select a result set (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        Task<DbDataReader> SelectAsync(string queryName);

        /// <summary>
        /// Select a result set (async)
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
        Task<DbDataReader> SelectAsync(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Select a result set (async)
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
        Task<DbDataReader> SelectAsync(string queryName, IParameterProvider model);

        /// <summary>
        /// Select a single result (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <returns>
        /// An <see cref="IDataReader"/> with the results
        /// </returns>
        Task<DbDataReader> SelectOneAsync(string queryName);

        /// <summary>
        /// Select a single result (async)
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
        Task<DbDataReader> SelectOneAsync(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Select a single result (async)
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
        Task<DbDataReader> SelectOneAsync(string queryName, IParameterProvider model);

        /// <summary>
        /// Insert data (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        Task InsertAsync(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Insert data (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        Task InsertAsync(string queryName, IParameterProvider model);

        /// <summary>
        /// Update data (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        Task UpdateAsync(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Update data (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        Task UpdateAsync(string queryName, IParameterProvider model);

        /// <summary>
        /// Delete data (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="parameters">
        /// The parameters to use in executing the query
        /// </param>
        Task DeleteAsync(string queryName, IDictionary<string, object> parameters);

        /// <summary>
        /// Delete data (async)
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute as defined in a <see cref="IDatabaseQueryProvider"/> instance
        /// </param>
        /// <param name="model">
        /// The model from which parameters may be obtained
        /// </param>
        Task DeleteAsync(string queryName, IParameterProvider model);

        /// <summary>
        /// Get a sequence (int) from the database (async)
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence to query
        /// </param>
        /// <returns>
        /// The sequence value
        /// </returns>
        Task<int> SequenceAsync(string sequenceName);

        /// <summary>
        /// Get a sequence (long) from the database (async)
        /// </summary>
        /// <param name="sequenceName">
        /// The name of the sequence to query
        /// </param>
        /// <returns>
        /// The sequence value
        /// </returns>
        Task<long> LongSequenceAsync(string sequenceName);

        /// <summary>
        /// Get the last auto-incremented / auto-numbered / identity value (async)
        /// </summary>
        /// <returns>
        /// The last identity value
        /// </returns>
        Task<int> LastIdentityAsync();

        /// <summary>
        /// Get the last auto-incremented / auto-numbered / identity value (async)
        /// </summary>
        /// <returns>
        /// The last identity value
        /// </returns>
        Task<long> LongLastIdentityAsync();
    }
}