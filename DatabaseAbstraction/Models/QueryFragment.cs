namespace DatabaseAbstraction.Models
{
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// This is a query fragment, used to construct a query from reusable parts
    /// </summary>
    public class QueryFragment
    {
        /// <summary>
        /// The SQL for this query fragment
        /// </summary>
        public string SQL { get; set; }

        /// <summary>
        /// Parameters associated with this fragment
        /// </summary>
        public Dictionary<string, DbType> Parameters
        {
            get
            {
                if (null == _parameters) _parameters = new Dictionary<string, DbType>();
                return _parameters;
            }
        }
        private Dictionary<string, DbType> _parameters;
    }
}