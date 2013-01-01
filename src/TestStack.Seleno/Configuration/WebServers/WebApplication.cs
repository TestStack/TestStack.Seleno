using System;

namespace TestStack.Seleno.Configuration.WebServers
{
    public class WebApplication
    {
        public IProjectLocation Location { get; private set; }
        public int PortNumber { get; private set; }

        public WebApplication(IProjectLocation location, int portNumber)
        {
            Guard.Against(location == null, "You must specify a location");
            Guard.Against(portNumber <= 0, "portNumber must be greater than zero");
            Location = location;
            PortNumber = portNumber;
        }
    }
}
