namespace DatabaseAbstraction.Models
{
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// A database query.
    /// </summary>
    public class DatabaseQuery
    {
        /// <summary>
        /// The name by which the query is known
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The SQL for the query
        /// </summary>
        public string SQL { get; set; }

        /// <summary>
        /// The parameters for the query
        /// </summary>
        public Dictionary<string, DbType> Parameters
        {
            get
            {
                if (null == _parameters) _parameters = new Dictionary<string, DbType>();
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }
        private Dictionary<string, DbType> _parameters;
    }
}