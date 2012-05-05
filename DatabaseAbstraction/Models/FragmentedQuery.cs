namespace DatabaseAbstraction.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This defines a query that must be put together with one or more fragments
    /// </summary>
    public class FragmentedQuery : DatabaseQuery
    {
        /// <summary>
        /// The fragments for the query, if any
        /// </summary>
        public IDictionary<QueryFragmentType, string> Fragments
        {
            get
            {
                if (null == _fragments)
                    _fragments = new Dictionary<QueryFragmentType, string>();

                return _fragments;
            }
        }
        private IDictionary<QueryFragmentType, string> _fragments;

        /// <summary>
        /// Straight SQL to include after the specified fragment
        /// </summary>
        public IDictionary<QueryFragmentType, string> AfterFragment
        {
            get
            {
                if (null == _afterFragment)
                    _afterFragment = new Dictionary<QueryFragmentType, string>();

                return _afterFragment;
            }
        }
        private IDictionary<QueryFragmentType, string> _afterFragment;

        /// <summary>
        /// Put the fragmented query together
        /// </summary>
        /// <param name="query">
        /// The query being assembled
        /// </param>
        /// <param name="fragments">
        /// The fragments to use in the assembly
        /// </param>
        /// <returns>
        /// A database query
        /// </returns>
        public void Assemble(IDictionary<string, QueryFragment> fragments)
        {
            var sql = new StringBuilder(SQL);

            foreach (QueryFragmentType type in Enum.GetValues(typeof(QueryFragmentType)))
                AppendFragment(type, sql, fragments);

            SQL = sql.ToString().Trim();

            // Clear out the fragment definitions
            _fragments = null;
            _afterFragment = null;
        }

        /// <summary>
        /// Append a query fragment to the new query
        /// </summary>
        /// <param name="type">
        /// The <see cref="QueryFragmentType"/> being appended
        /// </param>
        /// <param name="sql">
        /// The SQL string being built
        /// </param>
        /// <param name="fragments">
        /// The fragments available for selection
        /// </param>
        public void AppendFragment(QueryFragmentType type, StringBuilder sql, IDictionary<string, QueryFragment> fragments)
        {
            // Does the query have a fragment of this type?
            var fragment = from frag in Fragments
                           where type == frag.Key
                           select frag;

            if (0 == fragment.Count())
                return;

            // Is there a fragment that matches the name of this fragment?
            if (!fragments.ContainsKey(fragment.ElementAt(0).Value))
                throw new KeyNotFoundException(String.Format("Unable to find {0} query fragment {1} defined in query {2}",
                    Enum.GetName(typeof(QueryFragmentType), type), fragment.ElementAt(0).Value, Name));

            // Append the SQL from this fragment
            sql.Append(" ").Append(fragments[fragment.ElementAt(0).Value].SQL);

            // Append the parameter for this fragment
            foreach (var parameter in fragments[fragment.ElementAt(0).Value].Parameters)
                Parameters.Add(parameter.Key, parameter.Value);

            // Append after-the-fragment SQL if it exists
            fragment = from frag in AfterFragment
                       where type == frag.Key
                       select frag;

            if (0 == fragment.Count())
                return;

            sql.Append(" ").Append(fragment.ElementAt(0).Value);
        }
    }
}