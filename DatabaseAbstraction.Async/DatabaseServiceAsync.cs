namespace DatabaseAbstraction.Async
{
    using DatabaseAbstraction.Async.Interfaces;
    using DatabaseAbstraction.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// This class wraps a <see cref="IDatabaseService" /> instance and makes the calls conform to the .NET Framework
    /// version 4.5's new async/await paradigm.
    /// </summary>
    /// <remarks>
    /// This also implements IDisposable, and disposes the database service instance when this instance is disposed.
    /// </remarks>
    public class DatabaseServiceAsync : IDatabaseServiceAsync
    {
        /// <summary>
        /// The concrete (non-async) data service
        /// </summary>
        public IDatabaseService Service { get; set; }

        #region Constructors

        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="service">
        /// The concrete, synchronous database service implementation which this will wrap
        /// </param>
        public DatabaseServiceAsync(IDatabaseService service)
        {
            if (null == service)
                throw new ArgumentNullException("service", "The database service cannot be null");

            Service = service;
        }

        #endregion

        #region IDatabaseServiceAsync Implementation

        /// <summary>
        /// Get a set of data from the database
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <returns>
        /// A data reader with the data
        /// </returns>
        public virtual Task<IDataReader> SelectAsync(string queryName)
        {
            return Task.FromResult<IDataReader>(Service.Select(queryName));
        }

        /// <summary>
        /// Get a set of data from the database
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="parameters">
        /// Parameters to use when constructing the query
        /// </param>
        /// <returns>
        /// A data reader with the data
        /// </returns>
        public virtual Task<IDataReader> SelectAsync(string queryName, IDictionary<string, object> parameters)
        {
            return Task.FromResult<IDataReader>(Service.Select(queryName, parameters));
        }

        /// <summary>
        /// Get a set of data from the database
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        /// <returns>
        /// A data reader with the data
        /// </returns>
        public virtual Task<IDataReader> SelectAsync(string queryName, IParameterProvider model)
        {
            return Task.FromResult<IDataReader>(Service.Select(queryName, model));
        }

        /// <summary>
        /// Get a single result from the database
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <returns>
        /// A data reader with the data
        /// </returns>
        public virtual Task<IDataReader> SelectOneAsync(string queryName)
        {
            return Task.FromResult<IDataReader>(Service.SelectOne(queryName));
        }

        /// <summary>
        /// Get a single result from the database
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="parameters">
        /// Parameters to use when constructing the query
        /// </param>
        /// <returns>
        /// A data reader with the data
        /// </returns>
        public virtual Task<IDataReader> SelectOneAsync(string queryName, IDictionary<string, object> parameters)
        {
            return Task.FromResult<IDataReader>(Service.SelectOne(queryName, parameters));
        }

        /// <summary>
        /// Get a single result from the database
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        /// <returns>
        /// A data reader with the data
        /// </returns>
        public virtual Task<IDataReader> SelectOneAsync(string queryName, IParameterProvider model)
        {
            return Task.FromResult<IDataReader>(Service.SelectOne(queryName, model));
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="parameters">
        /// The parameters to use for the command
        /// </param>
        /// <exception cref="System.NotSupportedException">
        /// If the query is not an INSERT statement
        /// </exception>
        public virtual Task InsertAsync(string queryName, IDictionary<string, object> parameters)
        {
            Service.Insert(queryName, parameters);
            
            return TaskOK();
        }

        /// <summary>
        /// Insert data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        public virtual Task InsertAsync(string queryName, IParameterProvider model)
        {
            Service.Insert(queryName, model);

            return TaskOK();
        }

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="parameters">
        /// The parameters to use for the command
        /// </param>
        /// <exception cref="System.NotSupportedException">
        /// If the query is not an UPDATE statement
        /// </exception>
        public virtual Task UpdateAsync(string queryName, IDictionary<string, object> parameters)
        {
            Service.Update(queryName, parameters);

            return TaskOK();
        }

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        public virtual Task UpdateAsync(string queryName, IParameterProvider model)
        {
            Service.Update(queryName, model);

            return TaskOK();
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="parameters">
        /// The parameters to use for the command
        /// </param>
        /// <exception cref="System.NotSupportedException">
        /// If the query is not an DELETE statement
        /// </exception>
        public virtual Task DeleteAsync(string queryName, IDictionary<string, object> parameters)
        {
            Service.Delete(queryName, parameters);

            return TaskOK();
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        public virtual Task DeleteAsync(string queryName, IParameterProvider model)
        {
            Service.Delete(queryName, model);

            return TaskOK();
        }

        /// <summary>
        /// Get a sequence (int - async)
        /// </summary>
        /// <param name="sequenceName">
        /// The input required for the current implementation's sequence command
        /// </param>
        /// <returns>
        /// The next value in sequence for the given primary key
        /// </returns>
        public virtual Task<int> SequenceAsync(string sequenceName)
        {
            return Task.FromResult<int>(Service.Sequence(sequenceName));
        }

        /// <summary>
        /// Get a sequence (long - async)
        /// </summary>
        /// <param name="sequenceName">
        /// The input required for the current implementation's sequence command
        /// </param>
        /// <returns>
        /// The next value in sequence for the given primary key
        /// </returns>
        public virtual Task<long> LongSequenceAsync(string sequenceName)
        {
            return Task.FromResult<long>(Service.LongSequence(sequenceName));
        }

        /// <summary>
        /// Get the last "identity" value (int - async)
        /// </summary>
        /// <returns>
        /// The identity value
        /// </returns>
        public virtual Task<int> LastIdentityAsync()
        {
            return Task.FromResult<int>(Service.LastIdentity());
        }

        /// <summary>
        /// Get the last "identity" value (long - async)
        /// </summary>
        /// <returns>
        /// The identity value
        /// </returns>
        public virtual Task<long> LongLastIdentityAsync()
        {
            return Task.FromResult<long>(Service.LongLastIdentity());
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Cleanliness is next to Godliness!
        /// </summary>
        public void Dispose()
        {
            Service.Dispose();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// A "void" return value
        /// </summary>
        /// <returns>
        /// A task indicating that the database method was successful
        /// </returns>
        private Task TaskOK()
        {
            return Task.FromResult<bool>(true);
        }

        #endregion
    }
}