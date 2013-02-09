using System;
using Castle.Core.Logging;
using Funq;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    internal class AppConfigurator : IAppConfigurator
    {
        protected WebApplication WebApplication;
        protected IWebServer WebServer;
        protected ICamera Camera = new NullCamera();
        protected Func<IWebDriver> WebDriver = BrowserFactory.FireFox;
        private ILoggerFactory _loggerFactory = new NullLogFactory();

        public ISelenoApplication CreateApplication()
        {
            _loggerFactory
                .Create(GetType())
                .InfoFormat("Seleno v{0}, .NET Framework v{1}",
                    typeof(SelenoApplicationRunner).Assembly.GetName().Version, Environment.Version);

            var container = BuildContainer();
            var app = new SelenoApplication(container);

            return app;
        }

        private Container BuildContainer()
        {
            var container = new Container();
            container.Register(c => WebServer ?? new IisExpressWebServer(WebApplication));
            container.Register(c => WebDriver.Invoke());
            container.Register(c => Camera);
            container.Register(c => _loggerFactory);
            return container;
        }

        public IAppConfigurator ProjectToTest(WebApplication webApplication)
        {
            WebApplication = webApplication;
            return this;
        }

        public IAppConfigurator WithWebServer(IWebServer webServer)
        {
            WebServer = webServer;
            return this;
        }

        public IAppConfigurator WithWebDriver(Func<IWebDriver> webDriver)
        {
            WebDriver = webDriver;
            return this;
        }

        public IAppConfigurator UsingCamera(ICamera camera)
        {
            Camera = camera;
            return this;
        }

        public IAppConfigurator UsingLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }
    }
}