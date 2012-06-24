using System;

namespace TestStack.Seleno.Configuration
{
    public class AppConfigurationException : SelenoException
    {
        public AppConfigurationException()
        {
        }

        public AppConfigurationException(string message) : base(message)
        {
        }

        public AppConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}