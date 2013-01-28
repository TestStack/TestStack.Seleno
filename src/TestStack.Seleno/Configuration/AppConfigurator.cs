using System;
using Castle.Core.Logging;
using Funq;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Configuration
{
    internal class AppConfigurator : IAppConfigurator
    {
        protected WebApplication WebApplication;
        protected IWebServer WebServer;
        protected Func<Container, ICamera> Camera = c => new NullCamera();
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

            container.Register<IElementFinder>(c => new ElementFinder(c.Resolve<IWebDriver>()));
            container.Register<IScriptExecutor>(
                c => new ScriptExecutor(c.Resolve<IWebDriver>(), c.Resolve<IWebDriver>().GetJavaScriptExecutor(),
                                        c.Resolve<IElementFinder>(), c.Resolve<ICamera>()));
            container.Register<IPageNavigator>(
                c => new PageNavigator(c.Resolve<IWebDriver>(), c.Resolve<IScriptExecutor>(),
                                       c.Resolve<IWebServer>(), c.Resolve<IComponentFactory>()));
            container.Register<IComponentFactory>(
                c => new ComponentFactory(c));

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
            Camera = c => camera;
            return this;
        }

        public IAppConfigurator UsingCamera(string screenShotPath)
        {
            Camera = c => new FileCamera(c.Resolve<IWebDriver>(), screenShotPath);
            return this;
        }

        public IAppConfigurator UsingLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            return this;
        }
    }
}