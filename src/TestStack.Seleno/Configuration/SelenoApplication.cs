using System;
using Funq;
using TestStack.Seleno.Configuration.Contracts;

using OpenQA.Selenium;

namespace TestStack.Seleno.Configuration
{
    public class SelenoApplication : ISelenoApplication
    {
        public Container Container { get; set; }
        public IWebDriver Browser { get { return Container.Resolve<IWebDriver>(); } }
        public ICamera Camera { get { return Container.Resolve<ICamera>(); } }
        public IWebServer WebServer { get { return Container.Resolve<IWebServer>(); } }

        public SelenoApplication(Container container)
        {
            Container = container;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        public void Initialize()
        {
            WebServer.Start();
            Browser.Navigate().GoToUrl(WebServer.BaseUrl);
        }

        public void ShutDown()
        {
            Browser.Close();
            WebServer.Stop();
        }


        void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            ShutDown();
        }

    }
}