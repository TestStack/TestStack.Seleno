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
        protected WebApplication _webApplication;
        protected IWebServer _webServer;
        protected ICamera _camera = new NullCamera();
        protected Func<IWebDriver> _webDriver = BrowserFactory.FireFox;
        protected ILogFactory _logFactory = new ConsoleLogFactory();

        public ISelenoApplication CreateApplication()
        {
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

        public AppConfigurator ProjectToTest(WebApplication webApplication)
        {
            _webApplication = webApplication;
            return this;
        }

        public AppConfigurator WithWebServer(IWebServer webServer)
        {
            _webServer = webServer;
            return this;
        }

        public AppConfigurator WithWebDriver(Func<IWebDriver> webDriver)
        {
            _webDriver = webDriver;
            return this;
        }

        public AppConfigurator UsingCamera(ICamera camera)
        {
            _camera = camera;
            return this;
        }

        public AppConfigurator UsingLogger(ILogFactory logFactory)
        {
            _logFactory = logFactory;
            return this;
        }
    }
}