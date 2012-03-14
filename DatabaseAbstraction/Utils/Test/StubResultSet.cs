namespace DatabaseAbstraction.Utils.UnitTest
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the result set read by the <see cref="StubDataReader"/> - a one-way result set
    /// </summary>
    /// <remarks>
    /// This code was adapted from the wonderful StubDataReader developed by Phil Haack of the Subtext project.
    /// The original blog item can be found at
    /// http://haacked.com/archive/2006/05/31/UnitTestingDataAccessCodeWithTheStubDataReader.aspx , and
    /// the source code from the Subtext project can be found at
    /// http://www.koders.com/csharp/fidF24F8770B7D4A02933B8620B474DB5178F7593F0.aspx .
    /// </remarks>
    public sealed class StubResultSet
    {
        #region Fields

        /// <summary>
        /// The current index for the set
        /// </summary>
        private int _currentIndex = -1;

        /// <summary>
        /// The rows comprising the data set
        /// </summary>
        private List<StubDataRow> _rows = new List<StubDataRow>();

        /// <summary>
        /// The field names and indices for the data set
        /// </summary>
        private Dictionary<string, int> _fieldNames = new Dictionary<string, int>();

        #endregion

        #region Properties

        /// <summary>
        /// Get the current row
        /// </summary>
        public StubDataRow CurrentRow
        {
            get
            {
                return _rows[_currentIndex];
            }
        }

        /// <summary>
        /// Get a piece of data from the current row by name
        /// </summary>
        /// <param name="name">
        /// The field name
        /// </param>
        /// <returns>
        /// The value of that field from the current row
        /// </returns>
        public object this[string name]
        {
            get
            {
                return CurrentRow[GetIndexFromFieldName(name)];
            }
        }
        /// <summary>
        /// Get a piece of data from the current row by column index
        /// </summary>
        /// <param name="index">
        /// The column index
        /// </param>
        /// <returns>
        /// The value of that field from the current row
        /// </returns>
        public object this[int index]
        {
            get
            {
                return CurrentRow[index];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Force use of the parameter constructor
        /// </summary>
        private StubResultSet() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubResultSet"/> class with the column names
        /// </summary>
        /// <param name="fieldNames">
        /// The column names
        /// </param>
        public StubResultSet(params string[] fieldNames)
        {
            for (int index = 0; index < fieldNames.Length; index++)
                _fieldNames.Add(fieldNames[index], index);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Get the field names for this result set
        /// </summary>
        /// <returns>
        /// The field name for this result set
        /// </returns>
        public string[] GetFieldNames()
        {
            string[] keys = new string[_fieldNames.Keys.Count];
            _fieldNames.Keys.CopyTo(keys, 0);
            return keys;
        }

        /// <summary>
        /// Get the field name for the specific index
        /// </summary>
        /// <param name="index">
        /// The index to retrieve
        /// </param>
        /// <returns>
        /// The field name for the specific index
        /// </returns>
        public string GetFieldName(int index)
        {
            if ((0 > index) || (_fieldNames.Keys.Count <= index))
                throw new ArgumentOutOfRangeException("index",
                    String.Format("Index can only be between 0 and {0}", _fieldNames.Count - 1));

            return GetFieldNames()[index];
        }

        /// <summary>
        /// Add a row to the result set
        /// </summary>
        /// <param name="values">
        /// The values for the row
        /// </param>
        public void AddRow(params object[] values)
        {
            if (values.Length != _fieldNames.Count)
                throw new ArgumentOutOfRangeException("values",
                    String.Format("The row must contain {0} items", _fieldNames.Count));
            _rows.Add(new StubDataRow(values));
        }

        /// <summary>
        /// Get the index for a field name
        /// </summary>
        /// <param name="name">
        /// The field name
        /// </param>
        /// <returns>
        /// The index of the field name
        /// </returns>
        public int GetIndexFromFieldName(string name)
        {
            if (!_fieldNames.ContainsKey(name))
                throw new IndexOutOfRangeException(String.Format("The key '{0}' was not found in this data reader", name));
            return _fieldNames[name];
        }

        /// <summary>
        /// Advance the result set to the next row
        /// </summary>
        /// <returns>
        /// True if more rows exist, false if EOF
        /// </returns>
        public bool Read()
        {
            return ++_currentIndex < _rows.Count;
        }

        #endregion
    }
}