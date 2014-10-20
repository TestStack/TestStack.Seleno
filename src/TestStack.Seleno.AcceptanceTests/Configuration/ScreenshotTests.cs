using System;
using System.IO;
using System.Linq;
using Castle.Core.Logging;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Interceptors;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.PageObjects;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{
    [Explicit]
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

        [Test]
        [ExpectedException(typeof(SelenoReceivedException))]
        public void TakeScreenshotFromElementFinder()
        {
            _host.NavigateToInitialPage<ScreenshotTestPage>()
                .NavigateToNonExistentPageWithElementFinder();
        }

        [Test]
        [ExpectedException(typeof(SelenoReceivedException))]
        public void TakeScreenshotFromPageNavigator()
        {
            _host.NavigateToInitialPage<ScreenshotTestPage>()
                .NavigateToNonExistentPageWithPageNavigator();
        }

        [Test]
        public void TakeScreenshotFromSelenoApplicationThrows()
        {
            string imageName = "screenshot";
            string errorMessage = "there was an error";

            Action result = () => _host.Application.TakeScreenshotAndThrow(imageName, errorMessage);

            result.ShouldThrow<SelenoException>()
                .WithMessage(errorMessage);
        }

        [Test]
        public void TakeScreenshotFromSelenoApplicationSavesScreenshotToFile()
        {
            var directoryInfo = new DirectoryInfo(@"c:\screenshots");
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            string imageName = "screenshot";
            string errorMessage = "there was an error";

            Action result = () => _host.Application.TakeScreenshotAndThrow(imageName, errorMessage);

            result.ShouldThrow<SelenoException>()
                .WithMessage(errorMessage);
            directoryInfo.GetFiles("screenshot*.png").Count().Should().Be(1);
        }

        private class ScreenshotTestPage : Page
        {
            public ScreenshotTestPage NavigateToNonExistentPageWithElementFinder()
            {
                Find.Element(By.Name("doesntexist"));
                return this;
            }
             public ScreenshotTestPage NavigateToNonExistentPageWithPageNavigator()
             {
                 Navigate.To<Page>(By.Id("doesntexist"));
                 return this;
             }

        }
    }
}
