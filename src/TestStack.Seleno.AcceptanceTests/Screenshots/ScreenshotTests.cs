using System;
using Castle.Core.Logging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.Screenshots
{
    class ScreenshotTests
    {
        [Explicit]
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void TakeScreenshot()
        {
            using (var host = new SelenoHost())
            {
                host.Run(x => x
                    .UsingCamera(@"c:\screenshots")
                    .WithWebServer(new InternetWebServer("http://www.google.com/"))
                    .WithMinimumWaitTimeoutOf(TimeSpan.FromMilliseconds(100))
                    .UsingLoggerFactory(new ConsoleFactory(LoggerLevel.Debug))
                );

                host.NavigateToInitialPage<Page>()
                    .Find.Element(By.Name("doesntexist"));
            }
        }
    }
}
