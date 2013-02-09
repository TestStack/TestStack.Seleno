using System;
using Castle.Core.Logging;
using Funq;
using TestStack.Seleno.Configuration.Contracts;
using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    internal class SelenoApplication : ISelenoApplication
    {
        private readonly ILogger _logger;
        public IContainer Container { get; private set; }
        public IWebDriver Browser { get { return Container.Resolve<IWebDriver>(); } }
        public ICamera Camera { get { return Container.Resolve<ICamera>(); } }
        public IWebServer WebServer { get { return Container.Resolve<IWebServer>(); } }

        public SelenoApplication(Container container)
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

        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            _logger.Info("Starting domain unload");
            ShutDown();
            _logger.Debug("Domain unloaded");
        }
    }
}