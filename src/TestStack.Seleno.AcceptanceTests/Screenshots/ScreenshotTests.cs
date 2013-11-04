using System;
using Castle.Core.Logging;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Interceptors;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.Screenshots
{
    class ScreenshotTest
    {
        private SelenoHost _host;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _host = new SelenoHost();
            _host.Run(x => x
                .UsingCamera(@"c:\screenshots")
                .WithWebServer(new InternetWebServer("http://www.google.com/"))
                .WithMinimumWaitTimeoutOf(TimeSpan.FromMilliseconds(100))
                .UsingLoggerFactory(new ConsoleFactory(LoggerLevel.Debug))
            );
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            _host.Dispose();
        }

        [Explicit]
        [Test]
        [ExpectedException(typeof(SelenoReceivedException))]
        public void TakeScreenshotFromElementFinder()
        {
            _host.NavigateToInitialPage<Page>()
                .Find.Element(By.Name("doesntexist"));
        }

        [Explicit]
        [Test]
        [ExpectedException(typeof(SelenoReceivedException))]
        public void TakeScreenshotFromPageNavigator()
        {
            _host.NavigateToInitialPage<Page>()
                .Navigate.To<Page>(By.Id("doesntexist"));
        }
    }
}
