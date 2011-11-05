namespace DatabaseAbstraction.Utils
{
    using System;
    using System.Configuration.Provider;
    using System.Diagnostics;

    /// <summary>
    /// Utility methods for the membership and role provider classes
    /// </summary>
    public static class ProviderUtils
    {
        /// <summary>
        /// Get a value from the configuration, or a default value if not present
        /// </summary>
        /// <param name="configValue">
        /// The configuration value
        /// </param>
        /// <param name="defaultValue">
        /// The default value
        /// </param>
        /// <returns>
        /// The default value if the config value is null, the config value otherwise
        /// </returns>
        public static string ConfigValue(string configValue, string defaultValue)
        {
            return ((null == configValue) || (String.IsNullOrEmpty(configValue.Trim()))) ? defaultValue : configValue;
        }

        /// <summary>
        /// Write to the event log
        /// </summary>
        /// <remarks>
        /// This can be used as a security measure, so that details are written to the event log instead of being
        /// returned to the user.  It is up to the calling method to throw a different exception if a generic one
        /// should be thrown instead.
        /// </remarks>
        /// <param name="exception">
        /// The exception that was thrown
        /// </param>
        /// <param name="action">
        /// The action being performed when the exception occurred
        /// </param>
        public static void WriteToEventLog(Exception exception, string action, string source)
        {
            WriteToEventLog(exception, action, source, null);
        }

        /// <summary>
        /// Write to the event log, then return an exception (if exceptionText is provided)
        /// </summary>
        /// <remarks>
        /// ex. throw ProviderUtils.WriteToEventLog
        /// </remarks>
        /// <param name="exception">
        /// The exception that was thrown
        /// </param>
        /// <param name="action">
        /// The action being performed when the exception occurred
        /// </param>
        /// <exception cref="ProviderException">
        /// Thrown if the exceptionText specified is not null
        /// </exception>
        public static ProviderException WriteToEventLog(Exception exception, string action, string source,
            string exceptionText)
        {
            EventLog log = new EventLog();
            log.Source = source;
            log.Log = "Application";

            string message = "An exception occurred communicating with the data source.\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + exception.ToString();

            log.WriteEntry(message);

            if (null != exceptionText)
                return new ProviderException(exceptionText);

            return null;
        }
    }
}