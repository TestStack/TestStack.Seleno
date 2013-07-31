using System;

namespace TestStack.Seleno.Configuration.WebServers
{
    /// <summary>
    /// Defines a web application that exists at a location and can be deployed to a certain port number.
    /// </summary>
    public class WebApplication
    {
        /// <summary>
        /// The location of the web application.
        /// </summary>
        public IProjectLocation Location { get; private set; }

        /// <summary>
        /// The port number the web application will be deployed to.
        /// </summary>
        public int PortNumber { get; private set; }

        /// <summary>
        /// Create a web application using the given location and port number.
        /// </summary>
        /// <param name="location">The location of the web application</param>
        /// <param name="portNumber">The port number the web application will be deployed to</param>
        public WebApplication(IProjectLocation location, int portNumber)
        {
            if (location == null)
                throw new ArgumentNullException("location", "You must specify a location");
            if (portNumber <= 0)
                throw new ArgumentOutOfRangeException("portNumber", portNumber, "portNumber must be greater than zero");

            Location = location;
            PortNumber = portNumber;
        }
    }
}
