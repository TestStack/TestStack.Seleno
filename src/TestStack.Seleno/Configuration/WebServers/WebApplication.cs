using System;

namespace TestStack.Seleno.Configuration.WebServers
{
    //public class WebApplication
    //{
    //    public ProjectLocation Location { get; set; }
    //    public int PortNumber { get; set; }

    //    private WebApplication() { }

    //    public static WebApplication Create(Action<WebApplication> action)
    //    {
    //        var application = new WebApplication();
    //        action(application);

    //        if (application.Location == null)
    //            throw new ArgumentNullException("Location");
    //        if (application.PortNumber == 0)
    //            application.PortNumber = 23456;

    //        return application;
    //    }
    //}

    public class WebApplication
    {
        public ProjectLocation Location { get; private set; }
        public int PortNumber { get; private set; }

        public WebApplication(ProjectLocation location, int portNumber)
        {
            Location = location;
            PortNumber = portNumber;
        }
    }
}
