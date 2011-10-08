namespace DatabaseAbstraction.Utils.UnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// A stub implementation of a data reader
    /// </summary>
    /// <remarks>
    /// This code was adapted from the wonderful StubDataReader developed by Phil Haack of the Subtext project.
    /// The original blog item can be found at
    /// http://haacked.com/archive/2006/05/31/UnitTestingDataAccessCodeWithTheStubDataReader.aspx , and
    /// the source code from the Subtext project can be found at
    /// http://www.koders.com/csharp/fidF24F8770B7D4A02933B8620B474DB5178F7593F0.aspx .
    /// </remarks>
    public class StubDataReader : IDataReader
    {
        #region Properties

        /// <summary>
        /// The current result set
        /// </summary>
        public StubResultSet CurrentResultSet
        {
            get
            {
                /// If we haven't issued NextResult() yet, just advance to the first index.  IDataReader doesn't make
                /// you do that, but if you're calling <see cref="MockDatabaseService"/>.Select, it does.  This way,
                /// it can call NextResult() without skipping the first one, but that execution is not required.  Also,
                /// if we set the index to 0 here, the next call to NextResult() will advance to index 1.
                if (0 > CurrentResultSetIndex)
                    CurrentResultSetIndex = 0;

                return ResultSets[CurrentResultSetIndex];
            }
        }

        /// <summary>
        /// A count of the fields in the result set
        /// </summary>
        public int FieldCount
        {
            get
            {
                return CurrentResultSet.GetFieldNames().Length;
            }
        }

        /// <summary>
        /// Get a piece of data for the current row in the current set by field index
        /// </summary>
        /// <param name="index">
        /// The field index
        /// </param>
        /// <returns>
        /// The item of data
        /// </returns>
        public object this[int index]
        {
            get
            {
                return CurrentResultSet[index];
            }
        }

        /// <summary>
        /// Get a piece of data for the current row in the current set by name
        /// </summary>
        /// <param name="name">
        /// The field name
        /// </param>
        /// <returns>
        /// The item of data
        /// </returns>
        public object this[string name]
        {
            get
            {
                return CurrentResultSet[name];
            }
        }
        
        /// <summary>
        /// The result sets for this data reader
        /// </summary>
        private IList<StubResultSet> ResultSets { get; set; }

        /// <summary>
        /// The index of the current result set
        /// </summary>
        private int CurrentResultSetIndex { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of the <see cref="StubDataReader"/> class; each item in the list is a result set
        /// </summary>
        /// <param name="resultSets">
        /// The result sets
        /// </param>
        public StubDataReader(IList<StubResultSet> resultSets)
        {
            CurrentResultSetIndex = -1;
            ResultSets = resultSets;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="StubDataReader"/> class; each item of the array is a result set
        /// </summary>
        /// <param name="resultSets">
        /// The result sets
        /// </param>
        public StubDataReader(params StubResultSet[] resultSets)
        {
            CurrentResultSetIndex = -1;
            ResultSets = new List<StubResultSet>();

            foreach (StubResultSet result in resultSets)
                ResultSets.Add(result);
        }

        #endregion

        #region IDataReader Implementation

        /// <summary>
        /// Close this data reader
        /// </summary>
        /// <remarks>stub</remarks>
        public void Close() { }

        /// <summary>
        /// Dispose of the resources required by this data reader
        /// </summary>
        /// <remarks>stub</remarks>
        public void Dispose() { }

        /// <summary>
        /// Advance to the next result set
        /// </summary>
        /// <returns>
        /// True if another result set was available, false if not
        /// </returns>
        public bool NextResult()
        {
            if (CurrentResultSetIndex >= ResultSets.Count) return false;
            return (++CurrentResultSetIndex < ResultSets.Count);
        }

        /// <summary>
        /// Read the next result in the current result set
        /// </summary>
        /// <returns>
        /// True if there were more rows, false if not
        /// </returns>
        public bool Read()
        {
            return CurrentResultSet.Read();
        }

        /// <summary>
        /// Get the schema for the current result set
        /// </summary>
        /// <remarks>stub</remarks>
        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the nesting level for the current row
        /// </summary>
        /// <remarks>stub</remarks>
        public int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Is this data reader closed?
        /// </summary>
        /// <remarks>stub</remarks>
        public bool IsClosed
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// How many records were affected by the last command?
        /// </summary>
        /// <remarks>stub</remarks>
        public int RecordsAffected
        {
            get
            {
                return 1;
            }
        }

        public string GetName(int index)
        {
            return CurrentResultSet.GetFieldNames()[index];
        }

        public string GetDataTypeName(int index)
        {
            return GetName(index);
        }

        public Type GetFieldType(int index)
        {
            return ResultSets[0][index].GetType();
        }

        public object GetValue(int index)
        {
            return CurrentResultSet[index];
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            return CurrentResultSet.GetIndexFromFieldName(name);
        }

        public bool GetBoolean(int index)
        {
            return (bool)CurrentResultSet[index];
        }

        public byte GetByte(int index)
        {
            return (byte)CurrentResultSet[index];
        }

        public long GetBytes(int index, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            byte[] totalBytes = (byte[])CurrentResultSet[index];

            int bytesRead = 0;

            for (int byteIndex = 0; byteIndex < length; byteIndex++)
            {
                long readIndex = fieldOffset + byteIndex;
                long writeIndex = bufferOffset + byteIndex;

                if (totalBytes.Length <= readIndex)
                    throw new ArgumentOutOfRangeException("fieldOffset",
                        String.Format("Trying to read index {0} is out of range. (fieldOffset {1} + current position {2})",
                        readIndex, bufferOffset, byteIndex));

                if (buffer.Length <= writeIndex)
                    throw new ArgumentOutOfRangeException("bufferOffset",
                        String.Format("Trying to write to buffer index {0} is out of range. (bufferOffset {1} + current position {2})",
                        readIndex, bufferOffset, byteIndex));

                buffer[writeIndex] = totalBytes[readIndex];
                bytesRead++;
            }

            return bytesRead;
        }

        public char GetChar(int index)
        {
            return (char)CurrentResultSet[index];
        }

        public long GetChars(int index, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int index)
        {
            return (Guid)CurrentResultSet[index];
        }

        public short GetInt16(int index)
        {
            return (short)CurrentResultSet[index];
        }

        public int GetInt32(int index)
        {
            return (int)CurrentResultSet[index];
        }

        public long GetInt64(int index)
        {
            return (long)CurrentResultSet[index];
        }

        public float GetFloat(int index)
        {
            return (float)CurrentResultSet[index];
        }

        public double GetDouble(int index)
        {
            return (double)CurrentResultSet[index];
        }

        public string GetString(int index)
        {
            return (string)CurrentResultSet[index];
        }

        public decimal GetDecimal(int index)
        {
            return (decimal)CurrentResultSet[index];
        }

        public DateTime GetDateTime(int index)
        {
            return (DateTime)CurrentResultSet[index];
        }

        public IDataReader GetData(int index)
        {
            StubDataReader reader = new StubDataReader(ResultSets);

            int currentIndex = 0;
            while (currentIndex < CurrentResultSetIndex)
            {
                reader.NextResult();
                currentIndex++;
            }

            return reader;
        }

        public bool IsDBNull(int index)
        {
            return null == CurrentResultSet[index];
        }

        #endregion
    }
}