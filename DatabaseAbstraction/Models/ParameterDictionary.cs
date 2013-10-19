namespace DatabaseAbstraction.Models
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// This dictionary uses <see cref="Dictionary"/> as its underlying structure.  However, for values that are
    /// <see cref="Nullable"/>, it will return the value if it exists, instead of returning the nullable object. Also,
    /// attempts to get keys that do not exist will return null (or default) for the value, instead of throwing a
    /// <see cref="KeyNotFoundException"/>.
    /// </summary>
    public class ParameterDictionary : IDictionary<string, object>
    {
        /// <summary>
        /// The internal dictionary to use
        /// </summary>
        private IDictionary<string, object> _dictionary;

        /// <summary>
        /// Constructor
        /// </summary>
        public ParameterDictionary()
        {
            _dictionary = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constructor (single parameter)
        /// </summary>
        /// <param name="key">
        /// The key for the parameter
        /// </param>
        /// <param name="value">
        /// The value for the parameter
        /// </param>
        /// <remarks>
        /// This constructor effectively replaces the need for DbUtils.SingleParameter() in earlier versions; it still
        /// exists, and uses this constructor for its implementation
        /// </remarks>
        public ParameterDictionary(string key, object value)
            : this()
        {
            _dictionary[key] = value;
        }

        #region IDictionary Implementation (custom)

        /// <summary>
        /// Index accessor
        /// </summary>
        /// <param name="key">
        /// The key to retrieve
        /// </param>
        /// <returns>
        /// The value; if the value is a Nullable, it will return either null or the scalar value. If the value is not
        /// found, it will return null.
        /// </returns>
        public object this[string key]
        {
            get
            {
                object value = null;

                if (_dictionary.TryGetValue(key, out value))
                {
                    if (null != value && typeof(Nullable<>).IsAssignableFrom(value.GetType()))
                    {
                        if ((bool)typeof(Nullable<>).InvokeMember("HasValue",
                            BindingFlags.GetProperty, null, value, null))
                        {
                            value = typeof(Nullable<>).InvokeMember("Value",
                                BindingFlags.GetProperty, null, value, null);
                        }
                    }
                }

                return value;
            }
            set
            {
                _dictionary[key] = value;
            }
        }

        #endregion

        #region IDictionary Implementation (passthrough to Dictionary implementation)

        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public ICollection<object> Values
        {
            get { return _dictionary.Values; }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _dictionary.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion
    }
}