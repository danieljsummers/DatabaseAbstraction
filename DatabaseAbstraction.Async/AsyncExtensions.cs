namespace DatabaseAbstraction
{
    using DatabaseAbstraction.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions for <see cref="IDatabaseService" /> supporting task-based async processing
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Get a set of data from the database asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <returns>
        /// A task whose result is a data reader with the data
        /// </returns>
        public static Task<DbDataReader> SelectAsync(this IDatabaseService service, string queryName)
        {
            return service.SelectAsync(queryName, new Dictionary<string, object>());
        }

        /// <summary>
        /// Get a set of data from the database asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="parameters">
        /// Parameters to use when constructing the query
        /// </param>
        /// <returns>
        /// A task whose result is a data reader with the data
        /// </returns>
        public static Task<DbDataReader> SelectAsync(this IDatabaseService service, string queryName,
            IDictionary<string, object> parameters)
        {
            using (var command = ((DatabaseService)service).GetCommandForSelect(queryName, parameters))
            {
                return command.ExecuteReaderAsync();
            }
        }

        /// <summary>
        /// Get a set of data from the database asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        /// <returns>
        /// A task whose result is a data reader with the data
        /// </returns>
        public static Task<DbDataReader> SelectAsync(this IDatabaseService service,
            string queryName, IParameterProvider model)
        {
            return service.SelectAsync(queryName, model.Parameters());
        }

        /// <summary>
        /// Get a single result from the database asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <returns>
        /// A task whose result is a data reader with the data
        /// </returns>
        public static Task<DbDataReader> SelectOneAsync(this IDatabaseService service, string queryName)
        {
            return service.SelectOneAsync(queryName, new Dictionary<string, object>());
        }

        /// <summary>
        /// Get a single result from the database asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="parameters">
        /// Parameters to use when constructing the query
        /// </param>
        /// <returns>
        /// A task whose result is a data reader with the data
        /// </returns>
        public static Task<DbDataReader> SelectOneAsync(this IDatabaseService service, string queryName,
            IDictionary<string, object> parameters)
        {
            using (var command = ((DatabaseService)service).GetCommandForSelect(queryName, parameters))
            {
                return command.ExecuteReaderAsync(CommandBehavior.SingleRow);
            }
        }

        /// <summary>
        /// Get a single result from the database asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        /// <returns>
        /// A task whose result is a data reader with the data
        /// </returns>
        public static Task<DbDataReader> SelectOneAsync(this IDatabaseService service, string queryName,
            IParameterProvider model)
        {
            return service.SelectOneAsync(queryName, model.Parameters());
        }

        /// <summary>
        /// Insert data asynchronously
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
        public static Task InsertAsync(this IDatabaseService service, string queryName,
            IDictionary<string, object> parameters)
        {
            var query = ((DatabaseService)service).GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("INSERT"))
            {
                throw new NotSupportedException(String.Format("Query {0} is not an insert statement", queryName));
            }

            using (var command = ((DatabaseService)service).MakeCommand(query, parameters))
            {
                return command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Insert data asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        public static Task InsertAsync(this IDatabaseService service, string queryName, IParameterProvider model)
        {
            return service.InsertAsync(queryName, model.Parameters());
        }

        /// <summary>
        /// Update data asynchronously
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
        public static Task UpdateAsync(this IDatabaseService service, string queryName,
            IDictionary<string, object> parameters)
        {
            var query = ((DatabaseService)service).GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("UPDATE"))
            {
                throw new NotSupportedException(String.Format("Query {0} is not an update statement", queryName));
            }

            using (var command = ((DatabaseService)service).MakeCommand(query, parameters))
            {
                return command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Update data asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        public static Task UpdateAsync(this IDatabaseService service, string queryName, IParameterProvider model)
        {
            return service.UpdateAsync(queryName, model.Parameters());
        }

        /// <summary>
        /// Delete data asynchronously
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
        public static Task DeleteAsync(this IDatabaseService service, string queryName,
            IDictionary<string, object> parameters)
        {
            var query = ((DatabaseService)service).GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("DELETE"))
            {
                throw new NotSupportedException(String.Format("Query {0} is not a delete statement", queryName));
            }

            using (var command = ((DatabaseService)service).MakeCommand(query, parameters))
            {
                return command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Delete data asynchronously
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to execute
        /// </param>
        /// <param name="model">
        /// The <see cref="IParameterProvider"/> model object to use for the query
        /// </param>
        public static Task DeleteAsync(this IDatabaseService service, string queryName, IParameterProvider model)
        {
            return service.DeleteAsync(queryName, model.Parameters());
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
        /// <remarks>
        /// Since this is overridden in vendor-specific implementations, we will simply wwrap this in a task
        /// </remarks>
        public static Task<int> SequenceAsync(this IDatabaseService service, string sequenceName)
        {
            return Task.FromResult<int>(service.Sequence(sequenceName));
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
        /// <remarks>
        /// Since this is overridden in vendor-specific implementations, we will simply wwrap this in a task
        /// </remarks>
        public static Task<long> LongSequenceAsync(this IDatabaseService service, string sequenceName)
        {
            return Task.FromResult<long>(service.LongSequence(sequenceName));
        }

        /// <summary>
        /// Get the last "identity" value (int - async)
        /// </summary>
        /// <returns>
        /// The identity value
        /// </returns>
        /// <remarks>
        /// Since this is overridden in vendor-specific implementations, we will simply wwrap this in a task
        /// </remarks>
        public static Task<int> LastIdentityAsync(this IDatabaseService service)
        {
            return Task.FromResult<int>(service.LastIdentity());
        }

        /// <summary>
        /// Get the last "identity" value (long - async)
        /// </summary>
        /// <returns>
        /// The identity value
        /// </returns>
        /// <remarks>
        /// Since this is overridden in vendor-specific implementations, we will simply wwrap this in a task
        /// </remarks>
        public static Task<long> LongLastIdentityAsync(this IDatabaseService service)
        {
            return Task.FromResult<long>(service.LongLastIdentity());
        }
    }
}