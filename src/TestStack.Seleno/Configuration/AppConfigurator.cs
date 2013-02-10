using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Castle.Core.Logging;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

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

            return ContainerBuilder.Build();
        }

        // todo: move to separate file
        class PageObjectRegistrationSource : IRegistrationSource
        {
            public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
            {
                if (service == null)
                    throw new ArgumentNullException("service");

                var typedService = service as TypedService;
                if (typedService == null || !typeof(UiComponent).IsAssignableFrom(typedService.ServiceType))
                    return Enumerable.Empty<IComponentRegistration>();

                var rb = RegistrationBuilder.ForType(typedService.ServiceType)
                    .As(service)
                    .InstancePerDependency()
                    .OnActivated(a =>
                        {
                            var component = ((UiComponent)a.Instance);
                            component.Browser = a.Context.Resolve<IWebDriver>();
                            component.Camera = a.Context.Resolve<ICamera>();
                            component.ComponentFactory = a.Context.Resolve<IComponentFactory>();
                            component.ElementFinder = a.Context.Resolve<IElementFinder>();
                            component.PageNavigator = a.Context.Resolve<IPageNavigator>();
                            component.ScriptExecutor = a.Context.Resolve<IScriptExecutor>();
                        }
                    );

                return new[] { rb.CreateRegistration() };
            }

            public bool IsAdapterForIndividualComponents { get { return false; } }
        }

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