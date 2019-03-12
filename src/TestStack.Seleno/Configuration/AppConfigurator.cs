using System;
using System.Web.Routing;
using Autofac;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.ControlIdGenerators;
using TestStack.Seleno.Configuration.DomCaptures;
using TestStack.Seleno.Configuration.Registration;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Assertions;

namespace TestStack.Seleno.Configuration
{
    internal class AppConfigurator : IInternalAppConfigurator
    {
        protected ContainerBuilder ContainerBuilder = new ContainerBuilder();
        protected WebApplication WebApplication;
        private TimeSpan _minimumWait = TimeSpan.FromSeconds(10);
        private readonly RouteCollection _routes = new RouteCollection();

        public AppConfigurator()
        {
            UsingCamera(new NullCamera());
            UsingLoggerFactory(new NullLogFactory());
            WithRemoteWebDriver(BrowserFactory.FireFox);
            ContainerBuilder.Register(c => new IisExpressWebServer(WebApplication))
                .As<IWebServer>().SingleInstance();
            UsingControlIdGenerator(new DefaultControlIdGenerator());
            UsingDomCapture(new NullDomCapture());
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
            ContainerBuilder.Register(c => new ProxyGenerator(disableSignedModule: true))
                .SingleInstance();
            ContainerBuilder.RegisterProxiedClass<ElementFinder, IElementFinder>();
            ContainerBuilder.RegisterProxiedClass<Executor, IExecutor>();
            ContainerBuilder.RegisterProxiedClass<ElementAssert, IElementAssert>();
            ContainerBuilder.RegisterProxiedClass<PageNavigator, IPageNavigator>();
            ContainerBuilder.RegisterProxiedClass<Wait, IWait>();
            ContainerBuilder.RegisterType<ComponentFactory>()
                .AsImplementedInterfaces().SingleInstance();
            ContainerBuilder.Register(c => _routes).SingleInstance();
            ContainerBuilder.RegisterSource(new UiComponentRegistrationSource());

            return ContainerBuilder.Build();
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
            WithScreenshotTaker(() => driver.Value as ITakesScreenshot);
            return this;
        }

        /// <summary>
        /// Adds an environment variable to be injected into the web application process
        /// </summary>
        /// <param name="name">The name of the environment variable</param>
        /// <param name="value">The optional value of the environment variable</param>
        public IAppConfigurator WithEnvironmentVariable(string name, string value = null)
        {
            WebApplication.AddEnvironmentVariable(name, value);
            return this;
        }

        internal IAppConfigurator WithWebDriver(Func<IWebDriver> webDriver)
        {
            ContainerBuilder.Register(c => webDriver())
                .As<IWebDriver>().SingleInstance()
                .OnActivated(a => a.Instance.Manage().Timeouts().ImplicitWait = _minimumWait);
            return this;
        }

        internal IAppConfigurator WithJavaScriptExecutor(Func<IJavaScriptExecutor> javaScriptExecutor)
        {
            ContainerBuilder.Register(c => javaScriptExecutor())
                .As<IJavaScriptExecutor>().SingleInstance();
            return this;
        }

        internal IAppConfigurator WithScreenshotTaker(Func<ITakesScreenshot> screenshotTaker)
        {
            ContainerBuilder.Register(c => screenshotTaker())
                .As<ITakesScreenshot>().SingleInstance();
            return this;
        }

        public IAppConfigurator WithMinimumWaitTimeoutOf(TimeSpan minimumWait)
        {
            _minimumWait = minimumWait;
            return this;
        }

        public IAppConfigurator UsingCamera(ICamera camera)
        {
            ContainerBuilder.Register(c => camera)
                .As<ICamera>().SingleInstance()
                .OnActivated(a => {
                    a.Instance.ScreenshotTaker = a.Context.Resolve<ITakesScreenshot>();
                    a.Instance.Browser = a.Context.Resolve<IWebDriver>();
                });
            return this;
        }

        public IAppConfigurator UsingCamera(string screenShotPath)
        {
            return UsingCamera(new FileCamera(screenShotPath));
        }

        public IAppConfigurator UsingLoggerFactory(ILoggerFactory loggerFactory)
        {
            ContainerBuilder.Register(c => loggerFactory)
                .As<ILoggerFactory>().SingleInstance();
            return this;
        }

        public IAppConfigurator WithRouteConfig(Action<RouteCollection> routeCollectionUpdater)
        {
            routeCollectionUpdater(_routes);
            return this;
        }

        public IAppConfigurator UsingControlIdGenerator(IControlIdGenerator controlIdGenerator)
        {
            ContainerBuilder.Register(c => controlIdGenerator)
                .As<IControlIdGenerator>().SingleInstance();
            return this;
        }

        public IAppConfigurator UsingDomCapture(string capturePath)
        {
            return UsingDomCapture(new FileDomCapture(capturePath));
        }

        public IAppConfigurator UsingDomCapture(IDomCapture domCapture)
        {
            ContainerBuilder.Register(c => domCapture)
                .As<IDomCapture>().SingleInstance()
                .OnActivated(x => {
                    x.Instance.Browser = x.Context.Resolve<IWebDriver>();
                });
            return this;
        }
    }
}