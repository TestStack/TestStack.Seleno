using System;
using System.Runtime.Serialization;

namespace TestStack.Seleno.Configuration
{
    /// <summary>
    /// AppConfiguration Exception throw during the set up of webserver 
    /// </summary>
    [Serializable]
    public class AppConfigurationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// AppConfiguration Exception throw during the set up of webserver 
        /// </summary>
        public AppConfigurationException()
        {
        }
        /// <summary>
        /// AppConfiguration Exception throw during the set up of webserver 
        /// </summary>
        public AppConfigurationException(string message) : base(message)
        {
        }
        /// <summary>
        /// AppConfiguration Exception throw during the set up of webserver 
        /// </summary>
        public AppConfigurationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AppConfigurationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}