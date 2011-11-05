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
    public class ProviderQueryLibrary : IQueryLibrary, IQueryFragmentProvider
    {
        private static string PREFIX = "provider.";

        #region Main

        public void GetQueries(Dictionary<string, DatabaseQuery> queries)
        {
            // Select
            queries.Add(PREFIX + "validate_user", ValidateUser());
            queries.Add(PREFIX + "get.user", GetUser());
            queries.Add(PREFIX + "get.user.by_application", GetUserByApplication());
            queries.Add(PREFIX + "get.user.by_id", GetUserByID());
            queries.Add(PREFIX + "get.username_by_email", GetUsernameByEmail());
            queries.Add(PREFIX + "find.user.by_username", FindUserByUsername());
            queries.Add(PREFIX + "find.user.by_email", FindUserByEmail());
            queries.Add(PREFIX + "count.user", CountUser());
            queries.Add(PREFIX + "count.user.online", CountUserOnline());
            queries.Add(PREFIX + "count.user.by_username", CountUserByUsername());
            queries.Add(PREFIX + "count.user.by_email", CountUserByEmail());
            queries.Add(PREFIX + "retrieve_password", RetrievePassword());
            queries.Add(PREFIX + "failure_counts", FailureCounts());
            queries.Add(PREFIX + "role_exists", RoleExists());
            queries.Add(PREFIX + "count.user_role", CountUserRole());
            queries.Add(PREFIX + "get.role.by_application", GetRoleByApplication());
            queries.Add(PREFIX + "get.user_role.by_role", GetUserRoleByRole());
            queries.Add(PREFIX + "get.user_role.by_user", GetUserRoleByUser());
            queries.Add(PREFIX + "find.user_role", FindUserRole());

            // Insert
            queries.Add(PREFIX + "insert.user", InsertUser());
            queries.Add(PREFIX + "insert.role", InsertRole());
            queries.Add(PREFIX + "insert.user_role", InsertUserRole());

            // Update
            queries.Add(PREFIX + "update.login_date", UpdateLoginDate());
            queries.Add(PREFIX + "change_password", ChangePassword());
            queries.Add(PREFIX + "update.password_question", UpdatePasswordQuestion());
            queries.Add(PREFIX + "update.activity_date", UpdateActivityDate());
            queries.Add(PREFIX + "update.failure.password", UpdateFailurePassword());
            queries.Add(PREFIX + "update.failure.answer", UpdateFailureAnswer());
            queries.Add(PREFIX + "update.set_locked", UpdateSetLocked());
            queries.Add(PREFIX + "update.user", UpdateUser());

            // Delete
            queries.Add(PREFIX + "delete.user", DeleteUser());
            queries.Add(PREFIX + "delete.role", DeleteRole());
            queries.Add(PREFIX + "delete.user_role", DeleteUserRole());
            queries.Add(PREFIX + "delete.user_role.by_role", DeleteUserRoleByRole());
        }

        #endregion

        #region Select

        /// <summary>
        /// provider.validate_user
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery ValidateUser()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT
                    password, is_approved
                FROM membership_user"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.AfterFragment.Add(QueryFragmentType.Where, "AND is_locked_out = false");

            return query;
        }

        /// <summary>
        /// provider.get.user
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetUser()
        {
            FragmentedQuery query = new FragmentedQuery();

            query.SQL = "SELECT";
            query.Fragments.Add(QueryFragmentType.Select, "user.select.column_list");
            query.AfterFragment.Add(QueryFragmentType.Select, "FROM membership_user");
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");

            return query;
        }

        /// <summary>
        /// provider.get.user.by_application
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetUserByApplication()
        {
            FragmentedQuery query = new FragmentedQuery();

            query.SQL = "SELECT";
            query.Fragments.Add(QueryFragmentType.Select, "user.select.column_list");
            query.AfterFragment.Add(QueryFragmentType.Select,
                @"FROM membership_user
                WHERE application_name = @application_name
                ORDER BY username ASC");
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.get.user.by_id
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetUserByID()
        {
            FragmentedQuery query = new FragmentedQuery();

            query.SQL = "SELECT";
            query.Fragments.Add(QueryFragmentType.Select, "user.select.column_list");
            query.AfterFragment.Add(QueryFragmentType.Select,
                @"FROM membership_user
                WHERE user_id = @user_id");
            query.Parameters.Add("user_id", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.get.username.by_email
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery GetUsernameByEmail()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT username
                FROM membership_user
                WHERE   email            = @email
                    AND application_name = @application_name"
            };
            query.Parameters.Add("email", DbType.String);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.find.user.by_username
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery FindUserByUsername()
        {
            FragmentedQuery query = new FragmentedQuery();

            query.SQL = "SELECT";
            query.Fragments.Add(QueryFragmentType.Select, "user.select.column_list");
            query.AfterFragment.Add(QueryFragmentType.Select,
                @"FROM membership_user
                WHERE   username         LIKE @username
                    AND application_name    = @application_name
                ORDER BY username ASC");
            query.Parameters.Add("username", DbType.String);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.find.user.by_email
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery FindUserByEmail()
        {
            FragmentedQuery query = new FragmentedQuery();

            query.SQL = "SELECT";
            query.Fragments.Add(QueryFragmentType.Select, "user.select.column_list");
            query.AfterFragment.Add(QueryFragmentType.Select,
                @"FROM membership_user
                WHERE   email            LIKE @email
                    AND application_name    = @application_name
                ORDER BY username ASC");
            query.Parameters.Add("email", DbType.String);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.count.user
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery CountUser()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT count(user_id) AS user_count
                FROM membership_user
                WHERE application_name = @application_name"
            };
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.count.user.online
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery CountUserOnline()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT count(user_id) AS user_count
                FROM membership_user
                WHERE   last_activity_date > @last_activity_date
                    AND application_name   = @application_name"
            };
            query.Parameters.Add("last_activity_date", DbType.DateTime);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.count.user.by_username
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery CountUserByUsername()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT count(user_id) AS user_count
                FROM membership_user
                WHERE   username         LIKE @username
                    AND application_name    = @application_name"
            };
            query.Parameters.Add("username", DbType.String);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.count.user.by_email
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery CountUserByEmail()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT count(user_id) AS user_count
                FROM membership_user
                WHERE   email            LIKE @email
                    AND application_name    = @application_name"
            };
            query.Parameters.Add("email", DbType.String);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.retrieve_password
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery RetrievePassword()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT
                    password, password_answer, is_locked_out
                FROM membership_user"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");

            return query;
        }

        /// <summary>
        /// provider.failure_counts
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery FailureCounts()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT
                    failed_password_attempt_count, failed_password_attempt_window_start,
                    failed_password_answer_attempt_count, failed_password_answer_attempt_window_start
                FROM membership_user"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");

            return query;
        }

        /// <summary>
        /// proivder.role_exists
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery RoleExists()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT count(rolename) AS role_count
                FROM membership_role WHERE"
            };
            query.Fragments.Add(QueryFragmentType.Where, "role.where.role_and_application");

            return query;
        }

        /// <summary>
        /// provider.count.user_role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery CountUserRole()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT count(rolename) AS role_count
                FROM membership_user_role
                WHERE username = @username AND"
            };
            query.Fragments.Add(QueryFragmentType.Where, "role.where.role_and_application");
            query.Parameters.Add("username", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.get.role.by_application
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery GetRoleByApplication()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"SELECT rolename
                FROM membership_role
                WHERE application_name = @application_name"
            };
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.get.user_role.by_role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetUserRoleByRole()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT username
                FROM membership_user_role WHERE"
            };
            query.Fragments.Add(QueryFragmentType.Where, "role.where.role_and_application");

            return query;
        }

        /// <summary>
        /// provider.get.user_role.by_user
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery GetUserRoleByUser()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT rolename
                FROM membership_user_role"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");

            return query;
        }

        /// <summary>
        /// provider.find.user_role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery FindUserRole()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"SELECT username
                FROM membership_user_role
                WHERE username LIKE @username AND"
            };
            query.Fragments.Add(QueryFragmentType.Where, "role.where.role_and_application");
            query.Parameters.Add("username", DbType.String);

            return query;
        }

        #endregion

        #region Insert

        /// <summary>
        /// provider.insert.user
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery InsertUser()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"INSERT INTO membership_user
                    (user_id, username, password, email, password_question, password_answer, is_approved, comment,
                    creation_date, last_password_changed_date, last_activity_date, application_name, is_locked_out,
                    last_locked_out_date, failed_password_attempt_count, failed_password_attempt_window_start,
                    failed_password_answer_attempt_count, failed_password_answer_attempt_window_start)
                VALUES
                    (@user_id, @username, @password, @email, @question, @answer, @is_approved, '', @creation_date,
                    @creation_date, @creation_date, @application_name, false, @creation_date, 0, @creation_date, 0,
                    @creation_date)"
            };
            query.Parameters.Add("user_id", DbType.String);
            query.Parameters.Add("username", DbType.String);
            query.Parameters.Add("email", DbType.String);
            query.Parameters.Add("question", DbType.String);
            query.Parameters.Add("answer", DbType.String);
            query.Parameters.Add("is_approved", DbType.Boolean);
            query.Parameters.Add("creation_date", DbType.DateTime);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.insert.role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery InsertRole()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"INSERT INTO membership_role
                    (rolename, application_name)
                VALUES
                    (@rolename, @application_name)"
            };
            query.Parameters.Add("rolename", DbType.String);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.insert.user_role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private DatabaseQuery InsertUserRole()
        {
            DatabaseQuery query = new DatabaseQuery
            {
                SQL = @"INSERT INTO membership_user_role
                    (username, rolename, application_name)
                VALUES
                    (@username, @rolename, @application_name)"
            };
            query.Parameters.Add("username", DbType.String);
            query.Parameters.Add("rolename", DbType.String);
            query.Parameters.Add("application_name", DbType.String);

            return query;
        }

        #endregion

        #region Update

        /// <summary>
        /// provider.update.login_date
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateLoginDate()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET last_login_date = @last_login_date"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("last_login_date", DbType.DateTime);

            return query;
        }

        /// <summary>
        /// provider.change_password
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery ChangePassword()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET password                   = @password,
                    last_password_changed_date = @last_password_changed_date"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("password", DbType.String);
            query.Parameters.Add("last_password_changed_date", DbType.DateTime);

            return query;
        }

        /// <summary>
        /// provider.update.password_question
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdatePasswordQuestion()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET password_question = @question,
                    password_answer   = @answer"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("question", DbType.String);
            query.Parameters.Add("answer", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.update.activity_date
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateActivityDate()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET last_activity_date = @last_activity_date"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("last_activity_date", DbType.DateTime);

            return query;
        }

        /// <summary>
        /// provider.update.failure.password
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateFailurePassword()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET failed_password_attempt_count        = @failure_count,
                    failed_password_attempt_window_start = COALESCE(@window_start, failed_password_attempt_window_start)"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("failure_count", DbType.Int32);
            query.Parameters.Add("window_start", DbType.DateTime);

            return query;
        }

        /// <summary>
        /// provider.update.failure.answer
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateFailureAnswer()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET failed_password_answer_attempt_count        = @failure_count,
                    failed_password_answer_attempt_window_start = COALESCE(@window_start, failed_password_answer_attempt_window_start)"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("failure_count", DbType.Int32);
            query.Parameters.Add("window_start", DbType.DateTime);

            return query;
        }

        /// <summary>
        /// provider.update.set_locked
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateSetLocked()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET is_locked_out        = @is_locked_out,
                    last_locked_out_date = @last_locked_out_date"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("is_locked_out", DbType.Boolean);
            query.Parameters.Add("last_locked_out_date", DbType.DateTime);

            return query;
        }

        /// <summary>
        /// provider.update.user
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery UpdateUser()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = @"UPDATE membership_user
                SET email       = @email,
                    comment     = @comment,
                    is_approved = @is_approved"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.Parameters.Add("email", DbType.String);
            query.Parameters.Add("comment", DbType.String);
            query.Parameters.Add("is_approved", DbType.Boolean);

            return query;
        }

        #endregion

        #region Delete

        /// <summary>
        /// provider.delete.user
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery DeleteUser()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM membership_user"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");

            return query;
        }

        /// <summary>
        /// provider.delete.role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery DeleteRole()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM membership_role WHERE"
            };
            query.Fragments.Add(QueryFragmentType.Where, "role.where.role_and_application");

            return query;
        }

        /// <summary>
        /// provider.delete.user_role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery DeleteUserRole()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM membership_user_role"
            };
            query.Fragments.Add(QueryFragmentType.Where, "user.where.user_and_application");
            query.AfterFragment.Add(QueryFragmentType.Where, "AND rolename = @rolename");
            query.Parameters.Add("rolename", DbType.String);

            return query;
        }

        /// <summary>
        /// provider.delete.user_role.by_role
        /// </summary>
        /// <returns>
        /// The populated query
        /// </returns>
        private FragmentedQuery DeleteUserRoleByRole()
        {
            FragmentedQuery query = new FragmentedQuery
            {
                SQL = "DELETE FROM membership_user_role WHERE"
            };
            query.Fragments.Add(QueryFragmentType.Where, "role.where.role_and_application");

            return query;
        }

        #endregion

        #region Fragments

        /// <summary>
        /// Query fragments for the queries above
        /// </summary>
        /// <returns>
        /// The fragments for the query
        /// </returns>
        public void Fragments(Dictionary<string, QueryFragment> fragments)
        {
            // Select
            fragments.Add("user.select.column_list", UserSelectColumnListFragment());

            // Where
            fragments.Add("user.where.user_and_application", UserWhereUserAndApplicationFragment());
            fragments.Add("role.where.role_and_application", RoleWhereRoleAndApplicationFragment());
        }

        /// <summary>
        /// user.select.column_list
        /// </summary>
        private QueryFragment UserSelectColumnListFragment()
        {
            return new QueryFragment
                {
                    SQL = @"user_id, username, email, password_question, comment, is_approved, is_locked_out, creation_date,
                    last_login_date, last_activity_date, last_password_changed_date, last_locked_out_date"
                };
        }

        /// <summary>
        /// user.where.user_and_application
        /// </summary>
        private QueryFragment UserWhereUserAndApplicationFragment()
        {
            QueryFragment fragment = new QueryFragment
            {
                SQL = "WHERE username = @username AND application_name = @application_name"
            };
            fragment.Parameters.Add("username", DbType.String);
            fragment.Parameters.Add("application_name", DbType.String);

            return fragment;
        }

        /// <summary>
        /// role.where.role_and_application
        /// </summary>
        private QueryFragment RoleWhereRoleAndApplicationFragment()
        {
            QueryFragment fragment = new QueryFragment
            {
                SQL = "rolename = @rolename AND application_name = @application_name"
            };
            fragment.Parameters.Add("rolename", DbType.String);
            fragment.Parameters.Add("application_name", DbType.String);

            return fragment;
        }
        #endregion
    }
}