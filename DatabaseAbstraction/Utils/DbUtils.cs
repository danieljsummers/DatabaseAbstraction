namespace DatabaseAbstraction.Utils {

    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Utility methods for use by services and models that utilize Database Abstraction
    /// </summary>
    public static class DbUtils {

        /// <summary>
        /// Create a single parameter for a database abstraction query
        /// </summary>
        /// <param name="name">
        /// The name of the parameter
        /// </param>
        /// <param name="parameter">
        /// The object to use for the parameter's value
        /// </param>
        /// <returns>
        /// A parameter list suitable for use with the Database Abstraction methods
        /// </returns>
        public static Dictionary<string, object> SingleParameter(string name, object parameter) {
            Dictionary<string, object> list = new Dictionary<string, object>();
            list.Add(name, parameter);
            return list;
        }
    }
}