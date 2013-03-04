using System;
using Autofac;
using Castle.Core.Logging;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Registration;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;

using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Configuration
{
    internal class AppConfigurator : IInternalAppConfigurator
    {
        protected ContainerBuilder ContainerBuilder = new ContainerBuilder();
        protected WebApplication WebApplication;

        public AppConfigurator()
        {
            UsingCamera(new NullCamera());
            UsingLoggerFactory(new NullLogFactory());
            WithRemoteWebDriver(BrowserFactory.FireFox);
            ContainerBuilder.Register(c => new IisExpressWebServer(WebApplication))
                .As<IWebServer>().SingleInstance();
        }

        public ISelenoApplication CreateApplication()
        {
            var container = BuildContainer();

            container.Resolve<ILoggerFactory>()
                .Create(GetType())
                .InfoFormat("Seleno v{0}, .NET Framework v{1}",
                    typeof(SelenoHost).Assembly.GetName().Version,
                    Environment.Version
                );

            return new SelenoApplication(container);
        }

        private IContainer BuildContainer()
        {
            ContainerBuilder.RegisterType<ElementFinder>()
                .AsImplementedInterfaces().SingleInstance();
            ContainerBuilder.RegisterType<ScriptExecutor>()
                .AsImplementedInterfaces().SingleInstance();
            ContainerBuilder.RegisterType<PageNavigator>()
                .AsImplementedInterfaces().SingleInstance();
            ContainerBuilder.RegisterType<ComponentFactory>()
                .AsImplementedInterfaces().SingleInstance();
            ContainerBuilder.RegisterSource(new PageObjectRegistrationSource());

            ContainerBuilder.RegisterAssemblyTypes(typeof (HTMLControl).Assembly)
                            .As<IHtmlControl>()
                            .AsImplementedInterfaces()
                            .OnActivatedInitialiseUiComponent();

            return ContainerBuilder.Build();
        }

       

        // todo: move to separate file

        public IAppConfigurator ProjectToTest(WebApplication webApplication)
        {
            WebApplication = webApplication;
            return this;
        }

        public IAppConfigurator WithWebServer(IWebServer webServer)
        {
            ContainerBuilder.Register(c => webServer)
                .As<IWebServer>().SingleInstance();
            return this;
        }

        public IAppConfigurator WithRemoteWebDriver(Func<RemoteWebDriver> webDriver)
        {
            var driver = new Lazy<RemoteWebDriver>(webDriver);
            WithWebDriver(() => driver.Value);
            WithJavaScriptExecutor(() => driver.Value);
            return this;
        }

        internal IAppConfigurator WithWebDriver(Func<IWebDriver> webDriver)
        {
            ContainerBuilder.Register(c => webDriver())
                .As<IWebDriver>().SingleInstance();
            return this;
        }

        internal IAppConfigurator WithJavaScriptExecutor(Func<IJavaScriptExecutor> javaScriptExecutor)
        {
            ContainerBuilder.Register(c => javaScriptExecutor())
                .As<IJavaScriptExecutor>().SingleInstance();
            return this;
        }

        public IAppConfigurator UsingCamera(ICamera camera)
        {
            ContainerBuilder.Register(c => camera)
                .As<ICamera>().SingleInstance();
            return this;
        }

        public IAppConfigurator UsingCamera(string screenShotPath)
        {
            ContainerBuilder.Register(c => new FileCamera(c.Resolve<IWebDriver>(), screenShotPath))
                .As<ICamera>().SingleInstance();
            return this;
        }

        public IAppConfigurator UsingLoggerFactory(ILoggerFactory loggerFactory)
        {
            ContainerBuilder.Register(c => loggerFactory)
                .As<ILoggerFactory>().SingleInstance();
            return this;
        }
    }
}