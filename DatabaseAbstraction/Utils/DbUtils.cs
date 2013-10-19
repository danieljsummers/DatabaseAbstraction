namespace DatabaseAbstraction.Utils
{
    using System;
    using System.Collections.Generic;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// Utility methods for use by services and models that utilize Database Abstraction
    /// </summary>
    public static class DbUtils
    {
        /// <summary>
        /// Create a single parameter for a database abstraction query
        /// </summary>
        /// <param name="name">
        /// The name of the parameter
        /// </param>
        /// <param name="parameter">
        /// The object to use for the parameter's value
        /// </param>
        /// <returns>
        /// A parameter list suitable for use with the Database Abstraction methods
        /// </returns>
        public static IDictionary<string, object> SingleParameter(string name, object parameter)
        {
            return new ParameterDictionary(name, parameter);
        }

        /// <summary>
        /// Get a database connection
        /// </summary>
        /// <param name="connectionString">
        /// The connection string to use when creating the connection
        /// </param>
        /// <param name="providerName">
        /// The provider name (used to derive concrete class)
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        /// <returns>
        /// A database service of the applicable type
        /// </returns>
        public static IDatabaseService CreateDatabaseService(string connectionString, string providerName,
            params Type[] providers)
        {
            IDatabaseService service = null;

            switch (providerName)
            {
                case "Npgsql":
                    service = new PostgresDatabaseService(connectionString, providers);
                    break;

                case "MySql.Data.MySqlClient":
                    service = new MySqlDatabaseService(connectionString, providers);
                    break;

                case "System.Data.SqlClient":
                    service = new SqlDatabaseService(connectionString, providers);
                    break;

                case "System.Data.SQLite":
                    service = new SQLiteDatabaseService(connectionString, providers);
                    break;

                case "System.Data.Odbc":
                    service = new OdbcDatabaseService(connectionString, providers);
                    break;
            }

            return service;
        }
    }
}