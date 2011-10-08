namespace DatabaseAbstraction.Utils.UnitTest
{
    using System;

    /// <summary>
    /// A data row for the stub data reader
    /// </summary>
    /// <remarks>
    /// This code was adapted from the wonderful StubDataReader developed by Phil Haack of the Subtext project.
    /// The original blog item can be found at
    /// http://haacked.com/archive/2006/05/31/UnitTestingDataAccessCodeWithTheStubDataReader.aspx , and
    /// the source code from the Subtext project can be found at
    /// http://www.koders.com/csharp/fidF24F8770B7D4A02933B8620B474DB5178F7593F0.aspx .
    /// </remarks>
    public sealed class StubDataRow
    {
        /// <summary>
        /// The values for the row
        /// </summary>
        private object[] _values;

        /// <summary>
        /// Gets the <see cref="Object"/> with the specified index
        /// </summary>
        /// <param name="index">
        /// The index
        /// </param>
        /// <returns>
        /// The object with the specified index
        /// </returns>
        public object this[int index]
        {
            get
            {
                return _values[index];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubDataRow"/> class
        /// </summary>
        /// <param name="values">
        /// The values
        /// </param>
        public StubDataRow(params object[] values)
        {
            _values = values;
        }
    }
}