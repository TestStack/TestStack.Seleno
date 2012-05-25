using System;

namespace Seleno
{
    public class SelenoException : Exception
    {
        public SelenoException()
        {
        }

        public SelenoException(string message) : base(message)
        {
        }

        public SelenoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}