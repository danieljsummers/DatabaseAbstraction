namespace DatabaseAbstraction.Queries
{
    using System.Collections.Generic;
    using System.Data;
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;

    /// <summary>
    /// This contains queries to support the membership provider.
    /// It uses the "provider" query namespace.
    /// </summary>
    public class ProviderQueryLibrary : IQueryLibrary
    {
        private static string PREFIX = "provider.";

        #region Main

        public void GetQueries(Dictionary<string, DatabaseQuery> queries)
        {
            // Select
            addValidateUser(queries);
            addGetUser(queries);
            addGetUserByApplication(queries);
            addGetUserByID(queries);
            addGetUsernameByEmail(queries);
            addFindUserByUsername(queries);
            addFindUserByEmail(queries);
            addCountUser(queries);
            addCountUserOnline(queries);
            addCountUserByUsername(queries);
            addCountUserByEmail(queries);
            addRetrievePassword(queries);
            addFailureCounts(queries);
            addRoleExists(queries);
            addCountUserRole(queries);
            addGetRoleByApplication(queries);
            addGetUserRoleByRole(queries);
            addGetUserRoleByUser(queries);
            addFindUserRole(queries);
            
            // Insert
            addInsertUser(queries);
            addInsertRole(queries);
            addInsertUserRole(queries);

            // Update
            addUpdateLoginDate(queries);
            addChangePassword(queries);
            addUpdatePasswordQuestion(queries);
            addUpdateActivityDate(queries);
            addUpdateFailurePassword(queries);
            addUpdateFailureAnswer(queries);
            addUpdateSetLocked(queries);
            addUpdateUser(queries);

            // Delete
            addDeleteUser(queries);
            addDeleteRole(queries);
            addDeleteUserRole(queries);
            addDeleteUserRoleByRole(queries);
        }

        #endregion

        #region Select

        /// <summary>
        /// provider.validate_user
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addValidateUser(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "validate_user";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    password, is_approved
                FROM membership_user
                WHERE   username         = @username
                    AND application_name = @application_name
                    AND is_locked_out    = false";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.get.user
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addGetUser(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.user";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    user_id, username, email, password_question, comment, is_approved, is_locked_out, creation_date,
                    last_login_date, last_activity_date, last_password_changed_date, last_locked_out_date
                FROM membership_user
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.get.user.by_application
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addGetUserByApplication(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.user.by_application";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    user_id, username, email, password_question, comment, is_approved, is_locked_out, creation_date,
                    last_login_date, last_activity_date, last_password_changed_date, last_locked_out_date
                FROM membership_user
                WHERE application_name = @application_name
                ORDER BY username ASC";

            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.get.user.by_id
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addGetUserByID(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.user.by_id";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    user_id, username, email, password_question, comment, is_approved, is_locked_out, creation_date,
                    last_login_date, last_activity_date, last_password_changed_date, last_locked_out_date
                FROM membership_user
                WHERE user_id = @user_id";

            queries[name].Parameters.Add("user_id", DbType.String);
        }

        /// <summary>
        /// provider.get.username.by_email
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addGetUsernameByEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.username_by_email";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT username
                FROM membership_user
                WHERE   email            = @email
                    AND application_name = @application_name";

            queries[name].Parameters.Add("email", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.find.user.by_username
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addFindUserByUsername(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "find.user.by_username";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    user_id, username, email, password_question, comment, is_approved, is_locked_out, creation_date,
                    last_login_date, last_activity_date, last_password_changed_date, last_locked_out_date
                FROM membership_user
                WHERE   username         LIKE @username
                    AND application_name    = @application_name
                ORDER BY username ASC";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.find.user.by_email
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addFindUserByEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "find.user.by_email";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    user_id, username, email, password_question, comment, is_approved, is_locked_out, creation_date,
                    last_login_date, last_activity_date, last_password_changed_date, last_locked_out_date
                FROM membership_user
                WHERE   email            LIKE @email
                    AND application_name    = @application_name
                ORDER BY username ASC";

            queries[name].Parameters.Add("email", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.count.user
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addCountUser(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "count.user";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    count(user_id) AS user_count
                FROM membership_user
                WHERE application_name = @application_name";

            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.count.user.online
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addCountUserOnline(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "count.user.online";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    count(*) AS user_count
                FROM membership_user
                WHERE   last_activity_date > @last_activity_date
                    AND application_name   = @application_name";

            queries[name].Parameters.Add("last_activity_date", DbType.DateTime);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.count.user.by_username
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addCountUserByUsername(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "count.user.by_username";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    count(user_id) AS user_count
                FROM membership_user
                WHERE   username         LIKE @username
                    AND application_name    = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.count.user.by_email
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addCountUserByEmail(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "count.user.by_email";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    count(user_id) AS user_count
                FROM membership_user
                WHERE   email            LIKE @email
                    AND application_name    = @application_name";

            queries[name].Parameters.Add("email", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.retrieve_password
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addRetrievePassword(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "retrieve_password";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    password, password_answer, is_locked_out
                FROM membership_user
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.failure_counts
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addFailureCounts(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "failure_counts";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    failed_password_attempt_count, failed_password_attempt_window_start,
                    failed_password_answer_attempt_count, failed_password_answer_attempt_window_start
                FROM membership_user
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// proivder.role_exists
        /// </summary>
        /// <param name="queries">
        /// The query library being build
        /// </param>
        private void addRoleExists(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "role_exists";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                count(rolename) AS role_count
                FROM membership_role
                WHERE   rolename         = @rolename
                    AND application_name = @application_name";

            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.count.user_role
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addCountUserRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "count.user_role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                count(rolename) AS role_count
                FROM membership_user_role
                WHERE   username         = @username
                    AND rolename         = @rolename
                    AND application_name = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.get.role.by_application
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addGetRoleByApplication(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.role.by_application";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    rolename
                FROM membership_role
                WHERE application_name = @application_name";

            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.get.user_role.by_role
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addGetUserRoleByRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.user_role.by_role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    username
                FROM membership_user_role
                WHERE   rolename         = @rolename
                    AND application_name = @application_name";

            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.get.user_role.by_user
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addGetUserRoleByUser(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "get.user_role.by_user";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT
                    rolename
                FROM membership_user_role
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.find.user_role
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addFindUserRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "find.user_role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"SELECT username
                FROM membership_user_role
                WHERE   username         LIKE @username
                    AND rolename            = @rolename
                    AND application_name    = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        #endregion

        #region Insert

        /// <summary>
        /// provider.insert.user
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addInsertUser(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "insert.user";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"INSERT INTO membership_user
                    (user_id, username, password, email, password_question, password_answer, is_approved, comment,
                    creation_date, last_password_changed_date, last_activity_date, application_name, is_locked_out,
                    last_locked_out_date, failed_password_attempt_count, failed_password_attempt_window_start,
                    failed_password_answer_attempt_count, failed_password_answer_attempt_window_start)
                VALUES
                    (@user_id, @username, @password, @email, @question, @answer, @is_approved, '', @creation_date,
                    @creation_date, @creation_date, @application_name, false, @creation_date, 0, @creation_date, 0,
                    @creation_date)";

            queries[name].Parameters.Add("user_id", DbType.String);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("email", DbType.String);
            queries[name].Parameters.Add("question", DbType.String);
            queries[name].Parameters.Add("answer", DbType.String);
            queries[name].Parameters.Add("is_approved", DbType.Boolean);
            queries[name].Parameters.Add("creation_date", DbType.DateTime);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.insert.role
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addInsertRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "insert.role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"INSERT INTO membership_role
                    (rolename, application_name)
                VALUES
                    (@rolename, @application_name)";

            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.insert.user_role
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addInsertUserRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "insert.user_role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"INSERT INTO membership_user_role
                    (username, rolename, application_name)
                VALUES
                    (@username, @rolename, @application_name)";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        #endregion

        #region Update

        /// <summary>
        /// provider.update.login_date
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addUpdateLoginDate(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.login_date";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET last_login_date = @last_login_date
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("last_login_date", DbType.DateTime);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.change_password
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addChangePassword(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "change_password";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET password                   = @password,
                    last_password_changed_date = @last_password_changed_date
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("password", DbType.String);
            queries[name].Parameters.Add("last_password_changed_date", DbType.DateTime);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.update.password_question
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addUpdatePasswordQuestion(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.password_question";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET password_question = @question,
                    password_answer   = @answer
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("question", DbType.String);
            queries[name].Parameters.Add("answer", DbType.String);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.update.activity_date
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addUpdateActivityDate(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.activity_date";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET last_activity_date = @last_activity_date
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("last_activity_date", DbType.DateTime);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.update.failure.password
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addUpdateFailurePassword(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.failure.password";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET failed_password_attempt_count        = @failure_count,
                    failed_password_attempt_window_start = COALESCE(@window_start, failed_password_attempt_window_start)
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("failure_count", DbType.Int32);
            queries[name].Parameters.Add("window_start", DbType.DateTime);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.update.failure.answer
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addUpdateFailureAnswer(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.failure.answer";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET failed_password_answer_attempt_count        = @failure_count,
                    failed_password_answer_attempt_window_start = COALESCE(@window_start, failed_password_answer_attempt_window_start)
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("failure_count", DbType.Int32);
            queries[name].Parameters.Add("window_start", DbType.DateTime);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.update.set_locked
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addUpdateSetLocked(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.set_locked";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET is_locked_out        = @is_locked_out,
                    last_locked_out_date = @last_locked_out_date
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("is_locked_out", DbType.Boolean);
            queries[name].Parameters.Add("last_locked_out_date", DbType.DateTime);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.update.user
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addUpdateUser(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "update.user";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"UPDATE membership_user
                SET email       = @email,
                    comment     = @comment,
                    is_approved = @is_approved
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("email", DbType.String);
            queries[name].Parameters.Add("comment", DbType.String);
            queries[name].Parameters.Add("is_approved", DbType.Boolean);
            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        #endregion

        #region Delete

        /// <summary>
        /// provider.delete.user
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addDeleteUser(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete.user";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"DELETE FROM membership_user
                WHERE   username         = @username
                    AND application_name = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.delete.role
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addDeleteRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete.role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"DELETE FROM membership_role
                WHERE   rolename         = @rolename
                    AND application_name = @application_name";

            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.delete.user_role
        /// </summary>
        /// <param name="queries"></param>
        private void addDeleteUserRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete.user_role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"DELETE FROM membership_user_role
                WHERE   username         = @username
                    AND rolename         = @rolename
                    AND application_name = @application_name";

            queries[name].Parameters.Add("username", DbType.String);
            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        /// <summary>
        /// provider.delete.user_role.by_role
        /// </summary>
        /// <param name="queries">
        /// The query library being built
        /// </param>
        private void addDeleteUserRoleByRole(Dictionary<string, DatabaseQuery> queries)
        {
            string name = PREFIX + "delete.user_role.by_role";

            queries.Add(name, new DatabaseQuery());
            queries[name].SQL = @"DELETE FROM membership_user_role
                WHERE   rolename         = @rolename
                    AND application_name = @application_name";

            queries[name].Parameters.Add("rolename", DbType.String);
            queries[name].Parameters.Add("application_name", DbType.String);
        }

        #endregion
    }
}