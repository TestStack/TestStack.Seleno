using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Web.Mvc;
using Autofac;
using Castle.Core.Logging;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;

namespace TestStack.Seleno.Configuration
{
    internal class SelenoApplication : ISelenoApplication
    {
        private bool _initialised;
        private bool _disposed;

        private readonly IContainer _container;

        public IWebDriver Browser { get; }
        public ICamera Camera { get; }
        public IDomCapture DomCapture { get; }
        public ILogger Logger { get; }
        public IWebServer WebServer { get; }

        /// <summary>
        /// Create a SelenoApplication
        /// </summary>
        /// <param name="container">An Autofac container that will be owned by the SelenoApplication and disposed by the SelenoApplication</param>
        public SelenoApplication(IContainer container)
        {
            _container = container;
            Browser = _container.Resolve<IWebDriver>();
            Camera = _container.Resolve<ICamera>();
            DomCapture = _container.Resolve<IDomCapture>();
            Logger = _container.Resolve<ILoggerFactory>().Create(GetType());
            WebServer = _container.Resolve<IWebServer>();
        }

        public void Initialize()
        {
            _initialised = true;
            Logger.Debug("Starting Webserver");
            WebServer.Start();
            Logger.Debug("Browsing to base URL");
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
                Logger.Debug("Browser closed");
                WebServer.Stop();
                Logger.Debug("Webserver shutdown");
            }

            try
            {
                _container.Dispose();
            }
            catch (Exception ex)
            {
                // Safari throws 'System.InvalidOperationException : No process is associated with this object.'
                Logger.Warn(ex.Message);
            }
        }

        public TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action, IDictionary<string, object> routeValues = null)
            where TController : Controller
            where TPage : UiComponent, new()
        {
            return _container.Resolve<IPageNavigator>().To<TController, TPage>(action, routeValues);
        }

        public TPage NavigateToInitialPage<TController, TPage>(Expression<Action<TController>> action, object routeValues) where TController : Controller where TPage : UiComponent, new()
        {
            return _container.Resolve<IPageNavigator>().To<TController, TPage>(action, routeValues);
        }

        public TPage NavigateToInitialPage<TPage>(string url = "") where TPage : UiComponent, new()
        {
            return _container.Resolve<IPageNavigator>().To<TPage>(url);
        }

        public void SetBrowserWindowSize(int width, int height)
        {
            Browser.Manage().Window.Size = new Size(width, height);
        }

        public void TakeScreenshotAndThrow(string imageName, string errorMessage)
        {
            Camera.TakeScreenshot(string.Format(imageName + SystemTime.Now().ToString("yyyy-MM-dd_HH-mm-ss") + ".png"));
            throw new SelenoException(errorMessage);
        }

        public void CaptureDomAndThrow(string captureName, string errorMessage)
        {
            DomCapture.CaptureDom(string.Format(captureName + SystemTime.Now().ToString("yyyy-MM-dd_HH-mm-ss") + ".html"));
            throw new SelenoException(errorMessage);
        }

    }
}