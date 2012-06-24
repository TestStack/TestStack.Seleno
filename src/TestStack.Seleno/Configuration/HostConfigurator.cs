using System;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Infrastructure.Logging;
using TestStack.Seleno.Infrastructure.Logging.Loggers;

using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    public class HostConfigurator : IHostConfigurator
    {
        WebApplication _webApplication;
        private ICamera _camera = new NullCamera();
        private ILogFactory _logFactory = new ConsoleLogFactory();
        private Func<IWebDriver> _webDriver = BrowserFactory.FireFox;

        public void Validate()
        {
            //           throw new NotImplementedException();
        }

        public IHost CreateHost()
        {
            var host = new SelenoHost(_webApplication);
            host.Browser = _webDriver.Invoke();
            host.Camera = _camera;

            return host;
        }

        public void ProjectToTest(WebApplication webApplication)
        {
            _webApplication = webApplication;
        }

        public void WithWebDriver(Func<IWebDriver> webDriver)
        {
            _webDriver = webDriver;
        }

        public void UsingCamera(ICamera camera)
        {
            _camera = camera;
        }

        public void UsingLogger(ILogFactory logFactory)
        {
            _logFactory = logFactory;
        }
    }
}