using System;

namespace Seleno
{
    public class SeleniumExtensionsException : Exception
    {
        public SeleniumExtensionsException()
        {
        }

        public SeleniumExtensionsException(string message) : base(message)
        {
        }

        public SeleniumExtensionsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}