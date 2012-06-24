using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;

using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    public class SelenoHost : IHost
    {
        readonly WebApplication _webApplication;
        public IWebDriver Browser { get; internal set; }
        public ICamera Camera { get; internal set; }

        public SelenoHost(WebApplication webApplication)
        {
            _webApplication = webApplication;
        }

        public void Initialize()
        {
            IISExpressRunner.Start(_webApplication.Location.FullPath, _webApplication.PortNumber);
            Browser.Navigate().GoToUrl(IISExpressRunner.HomePage);
        }

        public void ShutDown()
        {
            Browser.Close();
            IISExpressRunner.Stop();
        }
    }
}