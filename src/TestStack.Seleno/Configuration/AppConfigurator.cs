using System;
using Funq;
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
        private WebApplication _webApplication;
        private IWebServer _webServer;
        private ICamera _camera = new NullCamera();
        private Func<IWebDriver> _webDriver = BrowserFactory.FireFox;
        private ILogFactory _logFactory = new ConsoleLogFactory();

        private void Validate()
        {
            if (_webApplication == null)
                throw new AppConfigurationException("The web application must be set.");
        }

        public ISelenoApplication CreateApplication()
        {
            Validate();
            _logFactory
                .GetLogger(GetType())
                .InfoFormat("Seleno v{0}, .NET Framework v{1}", 
                    typeof(SelenoApplicationRunner).Assembly.GetName().Version, Environment.Version);

            var container = BuildContainer();
            var app = new SelenoApplication(container);

            return app;
        }

        private Container BuildContainer()
        {
            var container = new Container();
            container.Register(c => _webServer ?? new IisExpressWebServer(_webApplication));
            container.Register(c => _webDriver.Invoke());
            container.Register(c => _camera);
            return container;
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