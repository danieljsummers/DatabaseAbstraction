namespace DatabaseAbstraction.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// This defines a query that must be put together with one or more fragments
    /// </summary>
    public class FragmentedQuery : DatabaseQuery
    {
        /// <summary>
        /// The fragments for the query, if any
        /// </summary>
        public Dictionary<QueryFragmentType, string> Fragments
        {
            get
            {
                if (null == _fragments) _fragments = new Dictionary<QueryFragmentType, string>();
                return _fragments;
            }
        }
        private Dictionary<QueryFragmentType, string> _fragments;

        /// <summary>
        /// Straight SQL to include after the specified fragment
        /// </summary>
        public Dictionary<QueryFragmentType, string> AfterFragment
        {
            get
            {
                if (null == _afterFragment) _afterFragment = new Dictionary<QueryFragmentType, string>();
                return _afterFragment;
            }
        }
        private Dictionary<QueryFragmentType, string> _afterFragment;
    }
}