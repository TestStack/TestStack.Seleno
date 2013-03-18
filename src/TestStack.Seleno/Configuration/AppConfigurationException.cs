using System;

namespace TestStack.Seleno.Configuration
{
    /// <summary>
    /// AppConfiguration Exception throw during the set up of webserver.
    /// </summary>
    public class AppConfigurationException : SelenoException
    {
        /// <summary>
        /// AppConfiguration Exception throw during the set up of webserver.
        /// </summary>
        public AppConfigurationException()
        {
        }

        /// <summary>
        /// AppConfiguration Exception throw during the set up of webserver.
        /// </summary>
        public AppConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// AppConfiguration Exception throw during the set up of webserver.
        /// </summary>
        public AppConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}