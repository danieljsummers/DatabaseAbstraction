namespace DatabaseAbstraction.Providers
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Data;
    using System.Diagnostics;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Configuration;
    using System.Web.Hosting;
    using System.Web.Security;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Queries;
    using DatabaseAbstraction.Utils;

    #endregion

    /// <summary>
    /// This is a membership provider that utilizes a Database Abstraction implementation to perform membership
    /// functions.  It derives the concrete class from the connection string provided.
    /// </summary>
    public sealed class DatabaseAbstractionMembershipProvider : MembershipProvider
    {
        #region Class Fields

        /// <summary>
        /// The length of new generated passwords
        /// </summary>
        private int newPasswordLength = 8;

        /// <summary>
        /// Generic exception message to use after writing detail to the event log
        /// </summary>
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        /// <summary>
        /// The connection string for this provider
        /// </summary>
        private string ConnectionString { get; set; }

        /// <summary>
        /// The provider name for the connection
        /// </summary>
        private string ProviderName { get; set; }

        /// <summary>
        /// The machine key, used to determine encryption keys
        /// </summary>
        private MachineKeySection machineKey;

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
            if (null == config)
                throw new ArgumentNullException("config");

            name = ProviderUtils.ConfigValue(name, "DatabaseAbstractionMembershipProvider");

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "DatabaseAbstraction Membership provider");
            }

            // Initialize the abstract base class
            base.Initialize(name, config);

            ApplicationName = ProviderUtils.ConfigValue(
                config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(ProviderUtils.ConfigValue(
                config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(ProviderUtils.ConfigValue(
                config["passwordAttemptWindow"], "10"));
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(ProviderUtils.ConfigValue(
                config["minRequiredNonAlphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(ProviderUtils.ConfigValue(
                config["minRequiredPasswordLength"], "7"));
            _passwordStrengthRegularExpression = Convert.ToString(ProviderUtils.ConfigValue(
                config["passwordStrengthRegularExpression"], ""));
            _enablePasswordReset = Convert.ToBoolean(ProviderUtils.ConfigValue(
                config["enablePasswordReset"], "true"));
            _enablePasswordRetrieval = Convert.ToBoolean(ProviderUtils.ConfigValue(
                config["enablePasswordRetrieval"], "true"));
            _requiresQuestionAndAnswer = Convert.ToBoolean(ProviderUtils.ConfigValue(
                config["requiresQuestionAndAnswer"], "false"));
            _requiresUniqueEmail = Convert.ToBoolean(ProviderUtils.ConfigValue(
                config["requiresUniqueEmail"], "true"));
            WriteExceptionsToEventLog = Convert.ToBoolean(ProviderUtils.ConfigValue(
                config["writeExceptionsToEventLog"], "true"));

            string configFormat = ProviderUtils.ConfigValue(config["passwordFormat"], "Hashed");

            switch (configFormat)
            {
                case "Hashed":
                    _passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    _passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    _passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Initialize connection string
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if ((null == settings) || (String.IsNullOrEmpty(settings.ConnectionString)))
                throw new ProviderException("Connection string cannot be blank.");

            ConnectionString = settings.ConnectionString;

            // Get encryption and decryption key information from the configuration
            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");
        }

        #endregion

        #region MembershipProvider Properties

        public override string ApplicationName { get; set; }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }
        private bool _enablePasswordReset;

        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }
        private bool _enablePasswordRetrieval;


        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }
        private bool _requiresQuestionAndAnswer;


        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }
        private bool _requiresUniqueEmail;


        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }
        private int _maxInvalidPasswordAttempts;


        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }
        private int _passwordAttemptWindow;

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }
        private MembershipPasswordFormat _passwordFormat;

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }
        private int _minRequiredNonAlphanumericCharacters;


        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }
        private int _minRequiredPasswordLength;


        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }
        private string _passwordStrengthRegularExpression;

        #endregion

        #region MembershipProvider Methods

        /// <summary>
        /// Change the user's password
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="oldPwd">
        /// The old password
        /// </param>
        /// <param name="newPwd">
        /// The new password
        /// </param>
        /// <returns>
        /// True if the change was successful, false if not
        /// </returns>
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            using (IDatabaseService database = Database())
            {
                if (!ValidateUser(username, oldPwd, database))
                    return false;

                ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPwd, true);

                OnValidatingPassword(args);

                if (args.Cancel)
                {
                    if (args.FailureInformation != null)
                        throw args.FailureInformation;
                    else
                        throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
                }

                Dictionary<string, object> parameters = GetDefaultParameters(username);
                parameters.Add("password", EncodePassword(newPwd));
                parameters.Add("last_password_changed_date", DateTime.Now);

                try
                {
                    database.Update("provider.change_password", parameters);
                }
                catch (DataException exception)
                {
                    if (WriteExceptionsToEventLog)
                        throw ProviderUtils.WriteToEventLog(exception, "ChangePassword", this.Name, exceptionMessage);
                    throw exception;
                }
            }

            return true;
        }

        /// <summary>
        /// Change the user's password question and answer
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="password">
        /// The password of the user
        /// </param>
        /// <param name="newPwdQuestion">
        /// The new password retrieval question
        /// </param>
        /// <param name="newPwdAnswer">
        /// The new password retrieval answer
        /// </param>
        /// <returns>
        /// True if the change was successful, false if not
        /// </returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPwdQuestion,
            string newPwdAnswer)
        {
            using (IDatabaseService database = Database())
            {
                if (!ValidateUser(username, password, database))
                    return false;

                Dictionary<string, object> parameters = GetDefaultParameters(username);

                parameters.Add("question", newPwdQuestion);
                parameters.Add("answer", EncodePassword(newPwdAnswer));

                try
                {
                    database.Update("provider.update.password_question", parameters);
                }
                catch (DataException exception)
                {
                    if (WriteExceptionsToEventLog)
                        throw ProviderUtils.WriteToEventLog(exception, "ChangePasswordQuestionAndAnswer", this.Name, exceptionMessage);
                    throw exception;
                }
            }

            return true;
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="username">
        /// The username for the user
        /// </param>
        /// <param name="password">
        /// The plain-text password for the user
        /// </param>
        /// <param name="email">
        /// The user's e-mail address
        /// </param>
        /// <param name="passwordQuestion">
        /// The password retrieval question
        /// </param>
        /// <param name="passwordAnswer">
        /// The password retrieval answer
        /// </param>
        /// <param name="isApproved">
        /// Whether the user has been approved
        /// </param>
        /// <param name="providerUserKey">
        /// The key for the user (blank and GUID acceptable)
        /// </param>
        /// <param name="status">
        /// The status of the member create action
        /// </param>
        /// <returns>
        /// The user created (or null if unsuccessful)
        /// </returns>
        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey,
            out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if ((RequiresUniqueEmail) && (GetUserNameByEmail(email) != ""))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser user = GetUser(username, false);

            if (user != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            DateTime createDate = DateTime.Now;

            if (providerUserKey == null)
            {
                providerUserKey = Guid.NewGuid();
            }
            else
            {
                if (!(providerUserKey is Guid))
                {
                    status = MembershipCreateStatus.InvalidProviderUserKey;
                    return null;
                }
            }

            Dictionary<string, object> parameters = GetDefaultParameters(username);

            parameters.Add("user_id", providerUserKey.ToString());
            parameters.Add("password", EncodePassword(password));
            parameters.Add("email", email);
            parameters.Add("question", passwordQuestion);
            parameters.Add("answer", EncodePassword(passwordAnswer));
            parameters.Add("is_approved", isApproved);
            parameters.Add("creation_date", createDate);

            try
            {
                using (IDatabaseService database = Database())
                {
                    database.Insert("provider.insert.user", parameters);
                    user = GetUser(username, false, database);

                    status = (null == user) ? MembershipCreateStatus.UserRejected : MembershipCreateStatus.Success;
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    ProviderUtils.WriteToEventLog(exception, "CreateUser", this.Name);

                status = MembershipCreateStatus.ProviderError;
            }

            return user;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="deleteAllRelatedData">
        /// Whether to delete all related data or not
        /// </param>
        /// <returns>
        /// True if a user was deleted, false if not
        /// </returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            try
            {
                using (IDatabaseService database = Database())
                {
                    MembershipUser user = GetUser(username, false, database);

                    if (deleteAllRelatedData)
                    {
                        // PLACEHOLDER: Do we need to delete anything here?
                    }

                    database.Delete("provider.delete.user", GetDefaultParameters(username));

                    return (null != user);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "DeleteUser", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Get all users for an application
        /// </summary>
        /// <param name="pageIndex">
        /// The page number
        /// </param>
        /// <param name="pageSize">
        /// How many users are on each page
        /// </param>
        /// <param name="totalRecords">
        /// The total number of users found
        /// </param>
        /// <returns>
        /// A list of the users
        /// </returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            try
            {
                using (IDatabaseService database = Database())
                {
                    using (IDataReader reader = database.SelectOne("provider.count.users",
                        DbUtils.SingleParameter("application_name", ApplicationName)))
                    {
                        reader.Read();
                        totalRecords = reader.GetInt32(reader.GetOrdinal("user_count"));
                    }

                    if (totalRecords <= 0)
                        return users;

                    GetPageOfUsers("provider.get.user.by_application", pageIndex, pageSize, database,
                        DbUtils.SingleParameter("application_name", ApplicationName), users);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetAllUsers", this.Name, exceptionMessage);
                throw exception;
            }

            return users;
        }

        /// <summary>
        /// Get the number of users online
        /// </summary>
        /// <returns>
        /// The number of users online
        /// </returns>
        public override int GetNumberOfUsersOnline()
        {
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);

            int numOnline = 0;

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("last_activity_date", DateTime.Now.Subtract(onlineSpan));
            parameters.Add("application_name", ApplicationName);

            try
            {
                using (IDatabaseService database = Database())
                using (IDataReader reader = database.SelectOne("provider.count.user.online", parameters))
                    if (reader.Read())
                        numOnline = reader.GetInt32(reader.GetOrdinal("user_count"));
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetNumberOfUsersOnline", this.Name, exceptionMessage);
                throw exception;
            }

            return numOnline;
        }

        /// <summary>
        /// Get the user's password
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="answer">
        /// The answer to the password retrieval question
        /// </param>
        /// <returns>
        /// The password, if retrieval is possible and successful
        /// </returns>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
                throw new ProviderException("Password Retrieval Not Enabled.");

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
                throw new ProviderException("Cannot retrieve Hashed passwords.");

            string password = "";
            string passwordAnswer = "";

            try
            {
                using (IDatabaseService database = Database())
                {
                    using (IDataReader reader = database.SelectOne("provider.retrieve_password",
                        GetDefaultParameters(username)))
                    {
                        if (!reader.Read())
                            throw new MembershipPasswordException("The supplied user name is not found.");

                        if (reader.GetBoolean(reader.GetOrdinal("is_locked_out")))
                            throw new MembershipPasswordException("The supplied user is locked out.");

                        password = reader.GetString(reader.GetOrdinal("password"));
                        passwordAnswer = reader.GetString(reader.GetOrdinal("password_answer"));
                    }

                    if ((RequiresQuestionAndAnswer) && (!CheckPassword(answer, passwordAnswer)))
                    {
                        UpdateFailureCount(username, "answer", database);
                        throw new MembershipPasswordException("Incorrect password answer.");
                    }
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetPassword", this.Name, exceptionMessage);
                throw exception;
            }

            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
                password = UnEncodePassword(password);

            return password;
        }

        /// <summary>
        /// Get a user by username
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="userIsOnline">
        /// If true, the last activity date is updated
        /// </param>
        /// <returns>
        /// The user (null if not found)
        /// </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (IDatabaseService database = Database())
                return GetUser(username, userIsOnline, database);
        }

        /// <summary>
        /// Get a user by username
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="userIsOnline">
        /// If true, the last activity date is updated
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        /// <returns>
        /// The user (null if not found)
        /// </returns>
        private MembershipUser GetUser(string username, bool userIsOnline, IDatabaseService database)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                parameters.Add("username", username);
                parameters.Add("application_name", ApplicationName);

                using (IDataReader reader = database.SelectOne("provider.get.user", parameters))
                {
                    if (!reader.Read())
                        return null;

                    if (userIsOnline)
                        UpdateLastActivity(username, database);

                    return GetUserFromReader(reader);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetUser(String, Boolean)", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Get a user by GUID
        /// </summary>
        /// <param name="providerUserKey">
        /// The GUID (user key)
        /// </param>
        /// <param name="userIsOnline">
        /// If true, the last activity date is updated
        /// </param>
        /// <returns>
        /// The user (null if not found)
        /// </returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            try
            {
                using (IDatabaseService database = Database())
                {
                    using (IDataReader reader = database.SelectOne("provider.get.user.by_id",
                        DbUtils.SingleParameter("user_id", providerUserKey)))
                    {
                        if (!reader.Read())
                        {
                            return null;
                        }

                        MembershipUser user = GetUserFromReader(reader);

                        if ((null != user) && (userIsOnline))
                            UpdateLastActivity(user.UserName, database);

                        return user;
                    }
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetUser(Object, Boolean)", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Unlock a user
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <returns>
        /// True if updated, false if not
        /// </returns>
        public override bool UnlockUser(string username)
        {
            try
            {
                using (IDatabaseService database = Database())
                {
                    if (null == GetUser(username, false, database))
                        return false;

                    Dictionary<string, object> parameters = GetDefaultParameters(username);

                    parameters.Add("is_locked_out", false);
                    parameters.Add("last_locked_out_date", DateTime.Now);

                    database.Update("provider.update.set_locked", parameters);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "UnlockUser", this.Name, exceptionMessage);
                throw exception;
            }

            return true;
        }

        /// <summary>
        /// Get a username by e-mail address
        /// </summary>
        /// <param name="email">
        /// The e-mail address
        /// </param>
        /// <returns>
        /// The username (empty string if not found)
        /// </returns>
        public override string GetUserNameByEmail(string email)
        {
            try
            {
                using (IDatabaseService database = Database())
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    parameters.Add("email", email);
                    parameters.Add("application_name", ApplicationName);

                    using (IDataReader reader = database.SelectOne("provider.get.username_by_email", parameters))
                    {
                        if (!reader.Read()) return String.Empty;
                        return NullUtils.GetString(reader, "username");
                    }
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "GetUserNameByEmail", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Reset a user's password
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="answer">
        /// The password retrieval answer
        /// </param>
        /// <returns>
        /// The new password
        /// </returns>
        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
                throw new NotSupportedException("Password reset is not enabled.");

            if ((answer == null) && (RequiresQuestionAndAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");
                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword = Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);

            // Validate password
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
            }

            try
            {
                using (IDatabaseService database = Database())
                {
                    using (IDataReader reader = database.SelectOne("provider.retrieve_password",
                        GetDefaultParameters(username)))
                    {
                        if (!reader.Read())
                            throw new MembershipPasswordException("The supplied user name is not found.");

                        if (reader.GetBoolean(reader.GetOrdinal("is_locked_out")))
                            throw new MembershipPasswordException("The supplied user is locked out.");

                        if ((RequiresQuestionAndAnswer)
                            && (!CheckPassword(answer, reader.GetString(reader.GetOrdinal("password_answer")))))
                        {
                            UpdateFailureCount(username, "passwordAnswer", database);
                            throw new MembershipPasswordException("Incorrect password answer.");
                        }
                    }

                    // Set the password
                    Dictionary<string, object> parameters = GetDefaultParameters(username);

                    parameters.Add("password", EncodePassword(newPassword));
                    parameters.Add("last_password_changed_date", DateTime.Now);

                    database.Update("provider.change_password", parameters);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "ResetPassword", this.Name, exceptionMessage);
                throw exception;
            }

            return newPassword;
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">
        /// The username of the user
        /// </param>
        public override void UpdateUser(MembershipUser user)
        {
            Dictionary<string, object> parameters = GetDefaultParameters(user.UserName);

            parameters.Add("email", user.Email);
            parameters.Add("comment", user.Comment);
            parameters.Add("is_approved", user.IsApproved);

            try
            {
                using (IDatabaseService database = Database())
                    database.Update("provider.update.user", parameters);
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "UpdateUser", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Validate a user
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="password">
        /// The password of the user
        /// </param>
        /// <returns>
        /// True if the user is valid, false if not
        /// </returns>
        public override bool ValidateUser(string username, string password)
        {
            using (IDatabaseService database = Database())
                return ValidateUser(username, password, database);
        }

        /// <summary>
        /// Validate a user
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="password">
        /// The password of the user
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        /// <returns>
        /// True if the user is valid, false if not
        /// </returns>
        private bool ValidateUser(string username, string password, IDatabaseService database)
        {
            bool isValid = false;

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("username", username);
            parameters.Add("application_name", ApplicationName);

            // Is the user approved?
            bool isApproved = false;

            // The password.
            string actualPassword = "";

            try
            {
                using (IDataReader reader = database.SelectOne("provider.validate_user", parameters))
                {
                    if (reader.Read())
                    {
                        actualPassword = reader.GetString(reader.GetOrdinal("password"));
                        isApproved = reader.GetBoolean(reader.GetOrdinal("is_approved"));
                    }
                    else
                        return false;
                }

                if (CheckPassword(password, actualPassword))
                {
                    if (isApproved)
                    {
                        isValid = true;

                        parameters.Add("last_login_date", DateTime.Now);
                        database.Update("provider.update.login_date", parameters);
                    }
                }
                else
                    UpdateFailureCount(username, "password", database);
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "ValidateUser", this.Name, exceptionMessage);
                throw exception;
            }

            return isValid;
        }

        /// <summary>
        /// Find users by name
        /// </summary>
        /// <param name="usernameToMatch">
        /// The username portion to match
        /// </param>
        /// <param name="pageIndex">
        /// The page number
        /// </param>
        /// <param name="pageSize">
        /// How many users are on each page
        /// </param>
        /// <param name="totalRecords">
        /// The total number of matching users found
        /// </param>
        /// <returns>
        /// The matching users
        /// </returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            try
            {
                using (IDatabaseService database = Database())
                {
                    using (IDataReader reader = database.SelectOne("provider.count.user.by_username",
                        GetDefaultParameters(usernameToMatch)))
                    {
                        if (reader.Read())
                            totalRecords = reader.GetInt32(reader.GetOrdinal("user_count"));
                    }


                    if (0 >= totalRecords)
                        return users;

                    GetPageOfUsers("provider.find.user.by_username", pageIndex, pageSize, database,
                        GetDefaultParameters(usernameToMatch), users);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "FindUsersByName", this.Name, exceptionMessage);
                throw exception;
            }

            return users;
        }

        /// <summary>
        /// Find users by e-mail address
        /// </summary>
        /// <param name="emailToMatch">
        /// The e-mail address portion to match
        /// </param>
        /// <param name="pageIndex">
        /// The page number
        /// </param>
        /// <param name="pageSize">
        /// How many users are on each page
        /// </param>
        /// <param name="totalRecords">
        /// The total number of matching users
        /// </param>
        /// <returns>
        /// The matching users
        /// </returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            totalRecords = 0;
            MembershipUserCollection users = new MembershipUserCollection();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("email", emailToMatch);
            parameters.Add("application_name", ApplicationName);

            try
            {
                using (IDatabaseService database = Database())
                {
                    using (IDataReader reader = database.SelectOne("provider.count.user.by_email", parameters))
                    {
                        if (reader.Read())
                            totalRecords = reader.GetInt32(reader.GetOrdinal("user_count"));
                    }

                    if (0 >= totalRecords)
                        return users;

                    GetPageOfUsers("provider.find.user.by_email", pageIndex, pageSize, database, parameters, users);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "FindUsersByEmail", this.Name, exceptionMessage);
                throw exception;
            }

            return users;
        }


        #endregion

        #region Helpers

        /// <summary>
        /// Populate a <see cref="MembershipUser"/> from the current data reader
        /// </summary>
        /// <param name="reader">
        /// An open data reader, pointing to the row to use
        /// </param>
        /// <returns>
        /// A populated user
        /// </returns>
        private MembershipUser GetUserFromReader(IDataReader reader)
        {
            return new MembershipUser(
                this.Name,
                reader.GetString(reader.GetOrdinal("username")),
                reader.GetGuid(reader.GetOrdinal("user_id")),
                reader.GetString(reader.GetOrdinal("email")),
                NullUtils.GetString(reader, "password_question"),
                NullUtils.GetString(reader, "comment"),
                reader.GetBoolean(reader.GetOrdinal("is_approved")),
                reader.GetBoolean(reader.GetOrdinal("is_locked_out")),
                reader.GetDateTime(reader.GetOrdinal("creationd_date")),
                NullUtils.GetDateTime(reader, "last_login_date"),
                reader.GetDateTime(reader.GetOrdinal("last_activity_date")),
                reader.GetDateTime(reader.GetOrdinal("last_password_changed_date")),
                NullUtils.GetDateTime(reader, "last_locked_out_date"));
        }

        /// <summary>
        /// Get parameter dictionary with user name and application name
        /// </summary>
        /// <param name="username">
        /// The username being queried
        /// </param>
        /// <returns>
        /// A dictionary with "username" and "application_name" parameters filled
        /// </returns>
        private Dictionary<string, object> GetDefaultParameters(string username)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("username", username);
            parameters.Add("application_name", ApplicationName);

            return parameters;
        }

        /// <summary>
        /// Update the last activity date
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        private void UpdateLastActivity(string username, IDatabaseService database)
        {
            Dictionary<string, object> parameters = GetDefaultParameters(username);
            parameters.Add("last_activity_date", DateTime.Now);

            database.Update("provider.update.last_activity", parameters);
        }

        /// <summary>
        /// Update failure counts
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="failureType">
        /// The type of failure (one of "password" or "answer")
        /// </param>
        private void UpdateFailureCount(string username, string failureType)
        {
            using (IDatabaseService database = Database())
                UpdateFailureCount(username, failureType, database);
        }

        /// <summary>
        /// Update failure counts
        /// </summary>
        /// <param name="username">
        /// The username of the user
        /// </param>
        /// <param name="failureType">
        /// The type of failure (one of "password" or "answer")
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        private void UpdateFailureCount(string username, string failureType, IDatabaseService database)
        {
            DateTime windowStart = new DateTime();
            int failureCount = 0;

            try
            {
                using (IDataReader reader = database.SelectOne("provider.failure_counts", GetDefaultParameters(username)))
                {
                    if (reader.Read())
                    {

                        if ("password" == failureType)
                        {
                            failureCount = reader.GetInt32(reader.GetOrdinal("failed_password_attempt_count"));
                            windowStart = reader.GetDateTime(reader.GetOrdinal("failed_password_attempt_window_start"));
                        }
                        else if ("answer" == failureType)
                        {
                            failureCount = reader.GetInt32(reader.GetOrdinal("failed_password_answer_attempt_count"));
                            windowStart = reader.GetDateTime(reader.GetOrdinal("failed_password_answer_attempt_window_start"));
                        }
                    }
                }

                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);
                Dictionary<string, object> parameters = GetDefaultParameters(username);

                if ((failureCount == 0) || (DateTime.Now > windowEnd))
                {
                    // First failure; start new count and window
                    parameters.Add("failure_count", 1);
                    parameters.Add("window_start", DateTime.Now);

                    database.Update("provider.update.failure." + failureType, parameters);
                }
                else if (failureCount++ >= MaxInvalidPasswordAttempts)
                {
                    // Too many attempts; lock out the user
                    parameters.Add("is_locked_out", true);
                    parameters.Add("last_locked_out_date", DateTime.Now);

                    database.Update("provider.update.set_locked", parameters);
                }
                else
                {
                    // Update the counts, leaving the window the same
                    parameters.Add("failure_count", failureCount);
                    parameters.Add("window_start", null);

                    database.Update("provider.update.failure." + failureType, parameters);
                }
            }
            catch (DataException exception)
            {
                if (WriteExceptionsToEventLog)
                    throw ProviderUtils.WriteToEventLog(exception, "UpdateFailureCount", this.Name, exceptionMessage);
                throw exception;
            }
        }

        /// <summary>
        /// Check the user's password against the one provided
        /// </summary>
        /// <param name="password">
        /// The password provided by the user
        /// </param>
        /// <param name="dbpassword">
        /// The password obtained from the database
        /// </param>
        /// <returns>
        /// True if the passwords match, false if not
        /// </returns>
        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            return (pass1 == pass2);
        }

        /// <summary>
        /// Encrypt, Hash, or leave the password clear based on the PasswordFormat
        /// </summary>
        /// <param name="password">
        /// The clear-text password
        /// </param>
        /// <returns>
        /// The password in the proper format
        /// </returns>
        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        /// <summary>
        /// Decrypt or leave the password clear based on the PasswordFormat
        /// </summary>
        /// <param name="encodedPassword">
        /// The encoded password
        /// </param>
        /// <returns>
        /// The password in the proper format
        /// </returns>
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        /// <summary>
        /// Convert a hexadecimal string to a byte array
        /// </summary>
        /// <remarks>
        /// Used to convert encryption keys from the configuration file
        /// </remarks>
        /// <param name="hexString">
        /// The encryption key as a hexadecimal string
        /// </param>
        /// <returns>
        /// The encryption key as a byte array
        /// </returns>
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// Get a page of users
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to use to obtain the users
        /// </param>
        /// <param name="pageIndex">
        /// The page number for the results
        /// </param>
        /// <param name="pageSize">
        /// The number of users per page
        /// </param>
        /// <param name="database">
        /// The database connection to use
        /// </param>
        /// <param name="parameters">
        /// The parameters to use for the query
        /// </param>
        /// <param name="users">
        /// The user collection to populate
        /// </param>
        private void GetPageOfUsers(string queryName, int pageIndex, int pageSize, IDatabaseService database,
            Dictionary<string, object> parameters, MembershipUserCollection users)
        {
            int counter = 0;
            int startIndex = pageIndex * pageSize;
            int endIndex = startIndex + pageSize - 1;

            using (IDataReader reader = database.Select(queryName, parameters))
            {
                while (reader.Read())
                {
                    if (counter >= startIndex)
                        users.Add(GetUserFromReader(reader));

                    if (counter >= endIndex)
                        break;

                    counter++;
                }
            }
        }

        /// <summary>
        /// Get a database connection
        /// </summary>
        /// <returns>
        /// A database connection (type derived from provider name)
        /// </returns>
        private IDatabaseService Database()
        {
            // Determine if the "provider.validate_user" query exists in the static query library.  If it does, the
            // caller has already populated it, and we don't need to pass it.  If it does not, we'll be using an
            // instance query library instead.
            if (DatabaseService.StaticQueries.ContainsKey("provider.validate_user"))
                return DbUtils.CreateDatabaseService(ConnectionString, ProviderName);

            List<IQueryFragmentProvider> fragmentProviders = new List<IQueryFragmentProvider>();
            fragmentProviders.Add(new ProviderQueryLibrary());

            return DbUtils.CreateDatabaseService(ConnectionString, ProviderName, fragmentProviders, new ProviderQueryLibrary());
        }

        #endregion
    }
}