namespace DatabaseAbstraction.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// This defines methods necessary to provide parameters to DatabaseAbstraction services.
    /// </summary>
    public interface IParameterProvider
    {
        /// <summary>
        /// The properties of the object as a string/object key/value pair
        /// </summary>
        /// <returns>
        /// The properties as a string/object dictionary.
        /// </returns>
        IDictionary<string, object> Parameters();
    }
}