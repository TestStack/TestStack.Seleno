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
        private bool _initialised = false;
        private bool _disposed = false;

        private readonly ILogger _logger;
        private readonly IWebServer _webServer;
        private readonly IWebDriver _webDriver;
        private readonly ICamera _camera;
        private readonly IContainer _container;

        public IWebDriver Browser { get { return _webDriver; } }
        public ICamera Camera { get { return _camera; } }
        public ILogger Logger { get { return _logger; } }
        public IWebServer WebServer { get { return _webServer; } }

        /// <summary>
        /// Create a SelenoApplication
        /// </summary>
        /// <param name="container">An Autofac container that will be owned by the SelenoApplication and disposed by the SelenoApplication</param>
        public SelenoApplication(IContainer container)
        {
            _container = container;
            _webDriver = _container.Resolve<IWebDriver>();
            _camera = _container.Resolve<ICamera>();
            _logger = _container.Resolve<ILoggerFactory>().Create(GetType());
            _webServer = _container.Resolve<IWebServer>();
        }

        public void Initialize()
        {
            _initialised = true;
            _logger.Debug("Starting Webserver");
            WebServer.Start();
            _logger.Debug("Browsing to base URL");
            Browser.Navigate().GoToUrl(WebServer.BaseUrl);
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            
            _disposed = true;

            if (_initialised)
            {
                Browser.Close();
                _logger.Debug("Browser closed");
                WebServer.Stop();
                _logger.Debug("Webserver shutdown");
            }

            try
            {
                _container.Dispose();
            }
            catch (Exception ex)
            {
                // Safari throws 'System.InvalidOperationException : No process is associated with this object.'
                _logger.Warn(ex.Message);
            }
        }

        public TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            return _container.Resolve<IPageNavigator>().To<TController, TPage>(action);
        }

        public TPage NavigateToInitialPage<TPage>(string url = "") where TPage : UiComponent, new()
        {
            return _container.Resolve<IPageNavigator>().To<TPage>(url);
        }
    }
}