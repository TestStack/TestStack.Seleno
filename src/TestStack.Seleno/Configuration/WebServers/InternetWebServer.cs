using TestStack.Seleno.Configuration.Contracts;

namespace TestStack.Seleno.Configuration.WebServers
{
    public class InternetWebServer : IWebServer
    {
        public InternetWebServer(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public void Start() { }

        public void Stop() { }

        public string BaseUrl { get; private set; }
    }
}