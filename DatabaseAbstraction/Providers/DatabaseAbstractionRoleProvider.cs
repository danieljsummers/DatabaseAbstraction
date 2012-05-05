namespace DatabaseAbstraction.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Data;
    using System.Web.Hosting;
    using System.Web.Security;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Queries;
    using DatabaseAbstraction.Utils;

    #endregion

    /// <summary>
    /// This is a role provider that utilizes a Database Abstraction implementation to perform role management
    /// functions.  It derives the concrete class from the connection string provided.
    /// </summary>
    public sealed class DatabaseAbstractionRoleProvider : RoleProvider
    {

        #region Class Fields

        /// <summary>
        /// The generic message thrown when details have been writen to the log
        /// </summary>
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        /// <summary>
        /// The connection string to use for this 
        /// </summary>
        private string ConnectionString { get; set; }

        /// <summary>
        /// The name of the provider of the database connection
        /// </summary>
        private string ProviderName { get; set; }

        /// <summary>
        /// Whether to write exceptions to the event log, or throw them to the caller
        /// </summary>
        public bool WriteExceptionsToEventLog { get; set; }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize the provider
        /// </summary>
        /// <param name="name">
        /// The name of the provider
        /// </param>
        /// <param name="config">
        /// The configuration of the provider
        /// </param>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config
            if (config == null)
                throw new ArgumentNullException("config");

            name = ProviderUtils.ConfigValue(name, "DatabaseAbstractionRoleProvider");

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Sample ODBC Role provider");
            }

            // Initialize the abstract base class
            base.Initialize(name, config);

            ApplicationName = ProviderUtils.ConfigValue(
                config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            WriteExceptionsToEventLog = Convert.ToBoolean(ProviderUtils.ConfigValue(
                config["writeExceptionsToEventLog"], "false"));

            // Initialize connection string
            var settings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if ((null == settings) || (String.IsNullOrEmpty(settings.ConnectionString)))
                throw new ProviderException("Connection string cannot be blank.");

            ConnectionString = settings.ConnectionString;
        }

        #endregion

        #region RoleProvider Properties

        /// <summary>
        /// The name of the application
        /// </summary>
        public override string ApplicationName { get; set; }

        #endregion

        #region RoleProvider Methods

        /// <summary>
        /// Add one or more users to one or more roles
        /// </summary>
        /// <param name="usernames">
        /// The user names
        /// </param>
        /// <param name="rolenames">
        /// The role names
        /// </param>
        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            try
            {
                using (var database = Database())
                {
                    // Validate the roles
                    foreach (var rolename in rolenames)
                        if (!RoleExists(rolename, database))
                            throw new ProviderException("Role name not found.");

                    // Validate the users in the roles
                    foreach (var username in usernames)
                    {
                        if (username.Contains(","))
                            throw new ArgumentException("User names cannot contain commas.");

                        foreach (var rolename in rolenames)
                            if (IsUserInRole(username, rolename, database))
                                throw new ProviderException("User is already in role.");
                    }

                    // Add the users to the roles
                    var parameters = GetDefaultParameters("");
                    parameters.Add("username", "");

                    foreach (var username in usernames)
                        foreach (var rolename in rolenames)
                        {
                            parameters["username"] = username;
                            parameters["rolename"] = rolename;

                            database.Insert("provider.insert.user_role", parameters);
                        }
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "AddUsersToRoles", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="rolename">
        /// The name of the role to create
        /// </param>
        public override void CreateRole(string rolename)
        {
            if (rolename.Contains(","))
                throw new ArgumentException("Role names cannot contain commas.");

            try
            {
                using (var database = Database())
                {
                    if (RoleExists(rolename, database))
                        throw new ProviderException("Role name already exists.");

                    database.Insert("provider.insert.role", GetDefaultParameters(rolename));
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "CreateRole", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="rolename">
        /// The name of the role being deleted
        /// </param>
        /// <param name="throwOnPopulatedRole">
        /// Whether to throw an exception if the role has any users assigned
        /// </param>
        /// <returns>
        /// True if deletion is successful, false if not
        /// </returns>
        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            try
            {
                using (var database = Database())
                {
                    if (!RoleExists(rolename, database))
                        throw new ProviderException("Role does not exist.");

                    if ((throwOnPopulatedRole) && (GetUsersInRole(rolename, database).Length > 0))
                        throw new ProviderException("Cannot delete a populated role.");

                    database.Delete("provider.delete.user_role.by_role", GetDefaultParameters(rolename));
                    database.Delete("provider.delete.role", GetDefaultParameters(rolename));
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                {
                    ProviderUtils.WriteToEventLog(exception, "DeleteRole", this.Name);
                    return false;
                }
                else
                    throw exception;
            }

            return true;
        }

        /// <summary>
        /// Get all roles for the application
        /// </summary>
        /// <returns>
        /// A list of roles for the application
        /// </returns>
        public override string[] GetAllRoles()
        {
            var roles = new List<string>();

            try
            {
                using (var database = Database())
                    using (var reader = database.Select("provider.get.role.by_application",
                            DbUtils.SingleParameter("application_name", ApplicationName)))
                        while (reader.Read())
                            roles.Add(reader.GetString(reader.GetOrdinal("rolename")));
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetAllRoles", this.Name, exceptionMessage);
                throw exception;
            }

            return roles.ToArray();
        }

        /// <summary>
        /// Get all roles for user
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <returns>
        /// A list of roles
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            var roles = new List<string>();

            var parameters = new Dictionary<string, object>();
            parameters.Add("username", username);
            parameters.Add("application_name", ApplicationName);

            try
            {
                using (var database = Database())
                    using (var reader = database.Select("provider.get.user_role.by_user", parameters))
                        while (reader.Read())
                            roles.Add(reader.GetString(reader.GetOrdinal("rolename")));
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetRolesForUser", this.Name, exceptionMessage);
                throw exception;
            }

            return roles.ToArray();
        }

        /// <summary>
        /// Get users in a role
        /// </summary>
        /// <param name="rolename">
        /// The name of the role
        /// </param>
        /// <returns>
        /// A list of users in that role
        /// </returns>
        public override string[] GetUsersInRole(string rolename)
        {
            using (var database = Database())
                return GetUsersInRole(rolename, database);
        }

        /// <summary>
        /// Get users in a role
        /// </summary>
        /// <param name="rolename">
        /// The name of the role
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        /// <returns>
        /// A list of users in that role
        /// </returns>
        private string[] GetUsersInRole(string rolename, IDatabaseService database)
        {
            var names = new List<string>();

            try
            {
                using (var reader = database.Select("provider.get.user_role.by_role", GetDefaultParameters(rolename)))
                    while (reader.Read())
                        names.Add(reader.GetString(reader.GetOrdinal("username")));
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetUsersInRole", this.Name, exceptionMessage);
                throw exception;
            }

            return names.ToArray();
        }

        /// <summary>
        /// Is the user in a specified role?
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="rolename">
        /// The rolename in question
        /// </param>
        /// <returns>
        /// True if they have the role, false if not
        /// </returns>
        public override bool IsUserInRole(string username, string rolename)
        {
            using (var database = Database())
                return IsUserInRole(username, rolename, database);
        }

        /// <summary>
        /// Is the user in a specified role?
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="rolename">
        /// The rolename in question
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        /// <returns>
        /// True if they have the role, false if not
        /// </returns>
        private bool IsUserInRole(string username, string rolename, IDatabaseService database)
        {
            var parameters = GetDefaultParameters(rolename);
            parameters.Add("username", username);

            try
            {
                using (var reader = database.SelectOne("provider.count.user_role", parameters))
                    return ((reader.Read()) && (0 < reader.GetInt32(reader.GetOrdinal("role_count"))));
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "IsUserInRole", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Remove the specified users from the specified roles
        /// </summary>
        /// <param name="usernames">
        /// The names of the users
        /// </param>
        /// <param name="rolenames">
        /// The names of the roles
        /// </param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            try
            {
                using (var database = Database())
                {
                    foreach (var rolename in rolenames)
                        if (!RoleExists(rolename, database))
                            throw new ProviderException("Role name not found.");

                    foreach (var username in usernames)
                        foreach (var rolename in rolenames)
                            if (!IsUserInRole(username, rolename, database))
                                throw new ProviderException("User is not in role.");

                    var parameters = GetDefaultParameters("");
                    parameters.Add("username", "");

                    foreach (var username in usernames)
                        foreach (var rolename in rolenames)
                        {
                            parameters["username"] = username;
                            parameters["rolename"] = rolename;
                            database.Delete("provider.delete.user_role", parameters);
                        }
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "RemoveUsersFromRoles", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Does the specified role exist?
        /// </summary>
        /// <param name="rolename">
        /// The name of the role
        /// </param>
        /// <returns>
        /// True if it exists, false if not
        /// </returns>
        public override bool RoleExists(string rolename)
        {
            using (var database = Database())
                return RoleExists(rolename, database);
        }

        /// <summary>
        /// Does the specified role exist?
        /// </summary>
        /// <param name="rolename">
        /// The name of the role
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        /// <returns>
        /// True if it exists, false if not
        /// </returns>
        private bool RoleExists(string rolename, IDatabaseService database)
        {
            try
            {
                using (var reader = database.SelectOne("provider.role_exists", GetDefaultParameters(rolename)))
                    return ((reader.Read()) && (0 < reader.GetInt32(reader.GetOrdinal("role_count"))));
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "RoleExists", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Find users in a role
        /// </summary>
        /// <param name="rolename">
        /// The name of the role
        /// </param>
        /// <param name="usernameToMatch">
        /// The text to match usernames
        /// </param>
        /// <returns>
        /// Matching users in the specified role
        /// </returns>
        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            var users = new List<string>();

            var parameters = GetDefaultParameters(rolename);
            parameters.Add("username", usernameToMatch);

            try
            {
                using (var database = Database())
                using (var reader = database.Select("provider.find.user_role", parameters))
                    while (reader.Read())
                        users.Add(reader.GetString(reader.GetOrdinal("username")));
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "FindUsersInRole", this.Name, exceptionMessage);
                throw exception;
            }

            return users.ToArray();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Get the default parameters used in most queries
        /// </summary>
        /// <param name="rolename">
        /// The name of the role in question
        /// </param>
        /// <returns>
        /// A parameter dictionary
        /// </returns>
        private Dictionary<string, object> GetDefaultParameters(string rolename)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("rolename", rolename);
            parameters.Add("application_name", ApplicationName);

            return parameters;
        }

        /// <summary>
        /// Get a database connection
        /// </summary>
        /// <returns>
        /// A database connection (type derived from connection string)
        /// </returns>
        private IDatabaseService Database()
        {
            // Determine if the "provider.validate_user" query exists in the static query library.  If it does, the
            // caller has already populated it, and we don't need to pass it.  If it does not, we'll be using an
            // instance query library instead.
            return (DatabaseService.StaticQueries.ContainsKey("provider.validate_user"))
                ? DbUtils.CreateDatabaseService(ConnectionString, ProviderName)
                : DbUtils.CreateDatabaseService(ConnectionString, ProviderName, typeof(ProviderQueryProvider));
        }

        #endregion

    }
}