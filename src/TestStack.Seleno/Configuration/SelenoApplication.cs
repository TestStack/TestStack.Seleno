using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;

using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    public class SelenoApplication : ISelenoApplication
    {
        readonly WebApplication _webApplication;
        public IWebDriver Browser { get; internal set; }
        public ICamera Camera { get; internal set; }
        public IWebServer WebServer { get; internal set; }

        public SelenoApplication(WebApplication webApplication)
        {
            _webApplication = webApplication;
        }

        public void Initialize()
        {
            WebServer.Start();
            Browser.Navigate().GoToUrl(WebServer.BaseUrl);
        }

        public void ShutDown()
        {
            Browser.Close();
            WebServer.Stop();
        }
    }
}