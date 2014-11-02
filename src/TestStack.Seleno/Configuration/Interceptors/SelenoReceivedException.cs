using System;
using System.Linq;

namespace TestStack.Seleno.Configuration.Interceptors
{
    /// <summary>
    /// Wraps an exception that has been received and processed by Seleno.
    /// </summary>
    public class SelenoReceivedException : Exception
    {
        /// <summary>
        /// Creates a <see cref="SelenoReceivedException"/>.
        /// </summary>
        /// <param name="innerException">The exception that has been caught and processed by Seleno</param>
        public SelenoReceivedException(Exception innerException)
            : base(innerException.Message, innerException)
        {}
    }
}