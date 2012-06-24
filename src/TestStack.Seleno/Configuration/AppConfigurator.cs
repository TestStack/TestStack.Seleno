using System;

using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Infrastructure.Logging;
using TestStack.Seleno.Infrastructure.Logging.Loggers;

using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    public class AppConfigurator : IAppConfigurator
    {
        static readonly ILog _log = LogManager.GetLogger("Seleno");

        private WebApplication _webApplication;
        private IWebServer _webServer;
        private ICamera _camera = new NullCamera();
        private ILogFactory _logFactory = new ConsoleLogFactory();
        private Func<IWebDriver> _webDriver = BrowserFactory.FireFox;

        private void Validate()
        {
            if (_webApplication == null)
                throw new AppConfigurationException("The web application must be set.");
        }

        public ISelenoApplication CreateApplication()
        {
            Validate();
            _log.InfoFormat("Seleno v{0}, .NET Framework v{1}", 
                typeof(SelenoApplicationRunner).Assembly.GetName().Version, Environment.Version);

            var app = new SelenoApplication(_webApplication);
            app.WebServer = _webServer ?? new IisExpressWebServer(_webApplication);
            app.Browser = _webDriver.Invoke();
            app.Camera = _camera;

            return app;
        }

        public void ProjectToTest(WebApplication webApplication)
        {
            _webApplication = webApplication;
        }

        public void WithWebServer(IWebServer webServer)
        {
            _webServer = webServer;
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