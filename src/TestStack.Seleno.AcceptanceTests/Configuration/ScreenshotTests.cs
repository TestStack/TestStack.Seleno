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
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{
    [Explicit]
    class ScreenshotTest
    {
        private SelenoHost _host;
        private string _cameraFolderPath = @"c:\screenshots";

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _host = new SelenoHost();
            _host.Run(x => x
                .UsingCamera(_cameraFolderPath)
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
        public void TakeScreenshotFromSelenoApplicationThrowsAndSavesScreenshotToFile()
        {
            string imageName = "screenshot";
            string errorMessage = "there was an error";
            var dateTime = new DateTime(2014, 05, 11,10,29,33);
            string fileName = string.Format(@"{0}\{1}{2}.png", _cameraFolderPath, imageName, dateTime.ToString("yyyy-MM-dd_HH-mm-ss"));

            using (new TestableSystemTime(dateTime))
            {
                Action result = () => _host.Application.TakeScreenshotAndThrow(imageName, errorMessage);

                result.ShouldThrow<SelenoException>()
                    .WithMessage(errorMessage);
                File.Exists(fileName).Should().BeTrue();
            }
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
