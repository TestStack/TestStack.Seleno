using System;

namespace TestStack.Seleno.Configuration
{
    /// <summary>
    /// Base class for exceptions that the Seleno library throws.
    /// </summary>
    public class SelenoException : Exception
    {
        /// <summary>
        /// Create Seleno exception without message.
        /// </summary>
        public SelenoException() {}

        /// <summary>
        /// Create Seleno exception with message.
        /// </summary>
        /// <param name="message">Exception message</param>
        public SelenoException(string message) : base(message) {}

        /// <summary>
        /// Create Seleno exception with message and inner exception.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public SelenoException(string message, Exception innerException) : base(message, innerException) {}
    }
}