using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Autofac;
using Castle.Core.Logging;
using TestStack.Seleno.Configuration.Contracts;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Configuration
{
    internal class SelenoApplication : ISelenoApplication
    {
        private readonly ILogger _logger;
        internal IContainer Container { get; private set; }

        public IWebDriver Browser { get { return Container.Resolve<IWebDriver>(); } }
        public ICamera Camera { get { return Container.Resolve<ICamera>(); } }

        public IWebServer WebServer { get { return Container.Resolve<IWebServer>(); } }

        public SelenoApplication(IContainer container)
        {
            Container = container;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            _logger = Container.Resolve<ILoggerFactory>()
                .Create(GetType());
        }

        public void Initialize()
        {
            _logger.Debug("Starting Webserver");
            WebServer.Start();
            _logger.Debug("Browsing to base URL");
            Browser.Navigate().GoToUrl(WebServer.BaseUrl);
        }

        public void ShutDown()
        {
            Browser.Close();
            _logger.Debug("Browser closed");
            WebServer.Stop();
            _logger.Debug("Webserver shutdown");
            Container.Dispose();
        }

        public TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            return Container.Resolve<IPageNavigator>().To<TController, TPage>(action);
        }

        public TPage NavigateToInitialPage<TPage>(string relativeUrl = "") where TPage : UiComponent, new()
        {
            return Container.Resolve<IPageNavigator>().To<TPage>(relativeUrl);
        }

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            _logger.Info("Starting domain unload");
            ShutDown();
            _logger.Debug("Domain unloaded");
        }
    }
}