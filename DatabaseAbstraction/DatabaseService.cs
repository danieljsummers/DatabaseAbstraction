namespace DatabaseAbstraction
{
    using DatabaseAbstraction.Interfaces;
    using DatabaseAbstraction.Models;
    using DatabaseAbstraction.Queries;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// This abstract class contains the majority of the implementation of the database abstraction.  The specific
    /// implementations only need to call the constructor with the list of classes, then fill the connection property
    /// with a concrete database connection.
    /// </summary>
    public abstract class DatabaseService : IDisposable
    {
        #region Properties

        /// <summary>
        /// Static queries
        /// </summary>
        /// <remarks>
        /// The database service opens a connection, so implementors may wish to implement this as an instance class
        /// and not a singleton.  However, to reduce the overhead associated with creating a query library, this
        /// query library can be created using the
        /// <see cref="DatabaseAbstraction.DatabaseService.FillStaticQueryLibrary"/> method (for example, in
        /// Application_Start() of a web project).  The service will search it first, then the instance-level library.
        /// This provides the flexibility to put at many queries in the static library as the implmentor desires,
        /// while allowing them to add more queries to each instance of the service.
        /// </remarks>
        public static IDictionary<string, DatabaseQuery> StaticQueries
        {
            get
            {
                if (null == _staticQueries)
                {
                    _staticQueries = new Dictionary<string, DatabaseQuery>();
                    (new DatabaseQueryProvider()).Queries(_staticQueries);
                }

                return _staticQueries;
            }
        }
        private static IDictionary<string, DatabaseQuery> _staticQueries;

        /// <summary>
        /// The instance query library (see notes for <see cref="StaticQueries"/>)
        /// </summary>
        protected IDictionary<string, DatabaseQuery> Queries { get; set; }

        /// <summary>
        /// The database connection for this service
        /// </summary>
        protected IDbConnection Connection { get; set; }

        /// <summary>
        /// The database query prefix (only change if the query library was changed also)
        /// </summary>
        public string DatabaseQueryPrefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }
        private string _prefix = "database.";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="providers">
        /// Query and fragment providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public DatabaseService(params Type[] providers) {

            Queries = new Dictionary<string, DatabaseQuery>();
            FillQueryLibrary(Queries, providers);

            // Make sure we've loaded the database queries
            if ((!StaticQueries.ContainsKey(DatabaseQueryPrefix + "sequence.generic"))
                && (!Queries.ContainsKey(DatabaseQueryPrefix + "sequence.generic")))
                FillQueryLibrary(Queries, typeof(DatabaseQueryProvider));
        }


        #endregion

        #region IDatabaseService Implementation (most methods)

        /// <summary>
        /// Get a set of data from the database
        /// </summary>
        /// <param name="queryName">
        /// The name of the query
        /// </param>
        /// <returns>
        /// A data reader with the data
        /// </returns>
        public virtual IDataReader Select(string queryName)
        {
            return Select(queryName, new Dictionary<string, object>());
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
        public virtual IDataReader Select(string queryName, IDictionary<string, object> parameters)
        {
            using (var command = GetCommandForSelect(queryName, parameters))
                return command.ExecuteReader();
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
        public virtual IDataReader Select(string queryName, IParameterProvider model)
        {
            return Select(queryName, model.Parameters());
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
        public virtual IDataReader SelectOne(string queryName)
        {
            return SelectOne(queryName, new Dictionary<string, object>());
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
        public virtual IDataReader SelectOne(string queryName, IDictionary<string, object> parameters)
        {
            using (var command = GetCommandForSelect(queryName, parameters))
                return command.ExecuteReader(CommandBehavior.SingleRow);
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
        public virtual IDataReader SelectOne(string queryName, IParameterProvider model)
        {
            return SelectOne(queryName, model.Parameters());
        }

        /// <summary>
        /// Create a command for a select query. 
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to retrieve
        /// </param>
        /// <param name="parameters">
        /// The parameters to use for the command
        /// </param>
        /// <returns>
        /// A command ready to execute
        /// </returns>
        /// <exception cref="System.NotSupportedException">
        /// If the query is not a SELECT statement
        /// </exception>
        protected IDbCommand GetCommandForSelect(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("SELECT"))
                throw new NotSupportedException(String.Format("Query {0} is not a select statement", queryName));

            return MakeCommand(query, parameters);
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
        public virtual void Insert(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("INSERT"))
                throw new NotSupportedException(String.Format("Query {0} is not an insert statement", queryName));

            using (var command = MakeCommand(query, parameters))
                command.ExecuteNonQuery();
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
        public virtual void Insert(string queryName, IParameterProvider model)
        {
            Insert(queryName, model.Parameters());
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
        public virtual void Update(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("UPDATE"))
                throw new NotSupportedException(String.Format("Query {0} is not an update statement", queryName));

            using (var command = MakeCommand(query, parameters))
                command.ExecuteNonQuery();
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
        public virtual void Update(string queryName, IParameterProvider model)
        {
            Update(queryName, model.Parameters());
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
        public virtual void Delete(string queryName, IDictionary<string, object> parameters)
        {
            var query = GetQuery(queryName);

            if (!query.SQL.ToUpper().StartsWith("DELETE"))
                throw new NotSupportedException(String.Format("Query {0} is not a delete statement", queryName));

            using (var command = MakeCommand(query, parameters))
                command.ExecuteNonQuery();
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
        public virtual void Delete(string queryName, IParameterProvider model)
        {
            Delete(queryName, model.Parameters());
        }

        /// <summary>
        /// Get a sequence (int)
        /// </summary>
        /// <remarks>
        /// This method is very dumb; it simply returns a value of MAX(PK) + 1.  However, for SQLite, and for ODBC
        /// connections (source unknown), this is the best we can do.
        /// </remarks>
        /// <param name="sequenceName">
        /// The primary key name and table name, separated by a pipe (ex. "table_id|table_name")
        /// </param>
        /// <returns>
        /// The next value in sequence for the given primary key
        /// </returns>
        public virtual int Sequence(string sequenceName)
        {
            var inputParameters = sequenceName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (2 != inputParameters.Length)
                throw new ArgumentException(String.Format(
                    "Invalid generic sequence \"{0}\" received (must be of the format \"table_id|table_name\"",
                    sequenceName));

            var parameters = new Dictionary<string, object>();
            parameters.Add("[]primary_key_name", inputParameters[0]);
            parameters.Add("[]table_name", inputParameters[1]);

            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.generic", parameters))
                return IntValue(reader, reader.GetOrdinal("max_pk")) + 1;
        }

        /// <summary>
        /// Get a sequence (long)
        /// </summary>
        /// <remarks>
        /// This method is very dumb; it simply returns a value of MAX(PK) + 1.  However, for SQLite, and for ODBC
        /// connections (source unknown), this is the best we can do.
        /// </remarks>
        /// <param name="sequenceName">
        /// The primary key name and table name, separated by a pipe (ex. "table_id|table_name")
        /// </param>
        /// <returns>
        /// The next value in sequence for the given primary key
        /// </returns>
        public virtual long LongSequence(string sequenceName)
        {
            var inputParameters = sequenceName.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (2 != inputParameters.Length)
                throw new ArgumentException(String.Format(
                    "Invalid generic sequence \"{0}\" received (must be of the format \"table_id|table_name\"",
                    sequenceName));

            var parameters = new Dictionary<string, object>();
            parameters.Add("[]primary_key_name", inputParameters[0]);
            parameters.Add("[]table_name", inputParameters[1]);

            using (var reader = SelectOne(DatabaseQueryPrefix + "sequence.generic", parameters))
                return LongValue(reader, reader.GetOrdinal("max_pk")) + 1;
        }

        /// <summary>
        /// Get the last "identity" value, whatever that may be (implementation specific) (int)
        /// </summary>
        /// <returns>
        /// The identity value
        /// </returns>
        public abstract int LastIdentity();

        /// <summary>
        /// Get the last "identity" value, whatever that may be (implementation specific) (long)
        /// </summary>
        /// <returns>
        /// The identity value
        /// </returns>
        public abstract long LongLastIdentity();

        /// <summary>
        /// Get a query from the library 
        /// </summary>
        /// <param name="queryName">
        /// The name of the query to retrieve
        /// </param>
        /// <returns>
        /// The database query
        /// </returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Thrown when the query name is not found in the query library
        /// </exception>
        protected DatabaseQuery GetQuery(string queryName)
        {
            if (StaticQueries.ContainsKey(queryName))
                return StaticQueries[queryName];

            if (Queries.ContainsKey(queryName))
                return Queries[queryName];

            throw new KeyNotFoundException(String.Format("Unable to find query {0}", queryName));
        }

        /// <summary>
        /// Create a command that is ready to be run 
        /// </summary>
        /// <param name="query">
        /// The database query to be executed
        /// </param>
        /// <param name="parameters">
        /// The parameters to use for this query
        /// </param>
        /// <returns>
        /// A command ready to execute
        /// </returns>
        protected IDbCommand MakeCommand(DatabaseQuery query, IDictionary<string, object> parameters)
        {
            var command = Connection.CreateCommand();
            command.CommandText = String.Copy(query.SQL);
            command.CommandType = CommandType.Text;

            if (null == parameters)
                return command;

            foreach (var queryParameter in query.Parameters)
            {
                if (parameters.ContainsKey(queryParameter.Key))
                {
                    if (queryParameter.Key.StartsWith("[]"))
                    {
                        // Do a straight string replacement on this parameter.
                        command.CommandText = command.CommandText.Replace(queryParameter.Key,
                                parameters[queryParameter.Key].ToString());
                    }
                    else
                    {
                        // Bind the parameter.
                        var param = command.CreateParameter();
                        param.ParameterName = queryParameter.Key;
                        param.DbType = queryParameter.Value;
                        param.Value = parameters[queryParameter.Key];
                        command.Parameters.Add(param);
                    }
                }
            }

            return command;
        }

        /// <summary>
        /// Get an int value from a data reader
        /// </summary>
        /// <param name="reader">
        /// The data reader (will have Read() executed on it)
        /// </param>
        /// <param name="columnIndex">
        /// The index of the column
        /// </param>
        /// <returns>
        /// The value of the column as an int, whether exposed as a long or an int
        /// </returns>
        protected int IntValue(IDataReader reader, int columnIndex)
        {
            if (!reader.Read())
                return 0;

            try
            {
                return reader.GetInt32(columnIndex);
            }
            catch (InvalidCastException)
            {
                return Convert.ToInt32(reader.GetInt64(columnIndex));
            }
        }

        /// <summary>
        /// Get a long value from a data reader
        /// </summary>
        /// <param name="reader">
        /// The data reader (will have Read() executed on it)
        /// </param>
        /// <param name="columnIndex">
        /// The index of the column
        /// </param>
        /// <returns>
        /// The value of the column as a long, whether exposed as an int or a long
        /// </returns>
        protected long LongValue(IDataReader reader, int columnIndex)
        {
            if (!reader.Read())
                return 0L;

            try
            {
                return reader.GetInt64(columnIndex);
            }
            catch (InvalidCastException)
            {
                return Convert.ToInt64(reader.GetInt32(columnIndex));
            }
        }

        #endregion

        #region Library Maintenance

        /// <summary>
        /// Fill the static query library
        /// </summary>
        /// <param name="providers">
        /// The <see cref="IDatabaseQueryProvider"/> classes to use to populate the library; if these classes are also
        /// <see cref="IQueryFragmentProvider"/>s, their fragments will be used as well
        /// </param>
        public static void FillStaticQueryLibrary(params Type[] providers)
        {
            FillQueryLibrary(StaticQueries, providers);
        }

        /// <summary>
        /// Fill a query library
        /// </summary>
        /// <param name="library">
        /// The library to fill
        /// </param>
        /// <param name="providers">
        /// Providers of type <see cref="IDatabaseQueryProvider"/> or <see cref="IQueryFragmentProvider"/>
        /// </param>
        public static void FillQueryLibrary(IDictionary<string, DatabaseQuery> library, params Type[] providers)
        {
            // Assemble providers from the input array of types.
            var fragmentProviders = new HashSet<Type>();
            var queryProviders = new HashSet<Type>();

            foreach (var type in providers)
            {
                if (typeof(IQueryFragmentProvider).IsAssignableFrom(type))
                    fragmentProviders.Add(type);

                if (typeof(IDatabaseQueryProvider).IsAssignableFrom(type))
                    queryProviders.Add(type);
            }

            // Create the fragment library.
            var fragments = new Dictionary<string, QueryFragment>();
            
            foreach (var fragmentProvider in fragmentProviders)
                ((IQueryFragmentProvider)Activator.CreateInstance(fragmentProvider)).Fragments(fragments);

            // Get the queries
            foreach (var queryProvider in queryProviders)
                ((IDatabaseQueryProvider)Activator.CreateInstance(queryProvider)).Queries(library);

            // Set the name property in every query
            foreach (var query in library)
                query.Value.Name = query.Key;

            // Assemble fragmented queries
            foreach (string name in library.Keys)
                if (typeof(FragmentedQuery) == library[name].GetType())
                    ((FragmentedQuery)library[name]).Assemble(fragments);
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Clean up resources for this service
        /// </summary>
        public void Dispose()
        {
            if (ConnectionState.Closed != Connection.State)
                Connection.Close();

            Connection.Dispose();
        }

        #endregion

    }
}