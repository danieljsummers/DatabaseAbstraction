namespace DatabaseAbstraction.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;

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
        public static Dictionary<string, object> SingleParameter(string name, object parameter)
        {
            Dictionary<string, object> list = new Dictionary<string, object>();
            list.Add(name, parameter);
            return list;
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
        /// <param name="queries">
        /// The <see cref="IQueryLibrary"/> classes with instance-level queries
        /// </param>
        /// <returns>
        /// A database connection of the applicable type
        /// </returns>
        public static IDatabaseService CreateDatabaseService(string connectionString, string providerName,
            params IQueryLibrary[] queries)
        {
            return CreateDatabaseService(connectionString, providerName, null, queries);
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
        /// <param name="fragments">
        /// The <see cref="IQueryFragmentProvider"/> classes to use in building instance-level queries
        /// </param>
        /// <param name="queries">
        /// The <see cref="IQueryLibrary"/> classes with instance-level queries
        /// </param>
        /// <returns>
        /// A database connection of the applicable type
        /// </returns>
        public static IDatabaseService CreateDatabaseService(string connectionString, string providerName,
            List<IQueryFragmentProvider> fragments, params IQueryLibrary[] queries)
        {
            IDatabaseService service = null;

            switch (providerName)
            {
                case "Npgsql":
                    service = new PostgresDatabaseService(connectionString, fragments, queries);
                    break;

                case "MySql.Data.MySqlClient":
                    service = new MySqlDatabaseService(connectionString, fragments, queries);
                    break;

                case "System.Data.SqlClient":
                    service = new SqlDatabaseService(connectionString, fragments, queries);
                    break;

                case "System.Data.SQLite":
                    service = new SQLiteDatabaseService(connectionString, fragments, queries);
                    break;

                case "System.Data.Odbc":
                    service = new OdbcDatabaseService(connectionString, fragments, queries);
                    break;
            }

            return service;
        }
    }
}