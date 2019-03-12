using System;
using System.IO;
using System.Linq;
using Castle.Core.Logging;
using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.AcceptanceTests.PageObjects;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Interceptors;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{

    class ScreenshotTest
    {
        private SelenoHost _host;
        private const string CameraFolderPath = @"c:\screenshots";

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _host = new SelenoHost();
            _host.Run(x => x
                .UsingCamera(CameraFolderPath)
                .WithWebServer(new InternetWebServer("http://www.google.com/"))
                .WithMinimumWaitTimeoutOf(TimeSpan.FromMilliseconds(100))
                .UsingLoggerFactory(new ConsoleFactory(LoggerLevel.Debug))
            );
        }

        [OneTimeTearDown]
        public void FixtureTeardown()
        {
            _host.Dispose();
        }

        [Test]
        public void TakeScreenshotFromElementFinder()
        {
            Assert.Throws<SelenoReceivedException>(() =>
            {
                _host.NavigateToInitialPage<NonExistentTestPage>()
                    .NavigateToNonExistentPageWithElementFinder();
            });
        }

        [Test]
        public void TakeScreenshotFromPageNavigator()
        {
            Assert.Throws<SelenoReceivedException>(() =>
            {
                _host.NavigateToInitialPage<NonExistentTestPage>()
                    .NavigateToNonExistentPageWithPageNavigator();
            });
        }

        [Test]
        public void TakeScreenshotFromSelenoApplicationThrowsAndSavesScreenshotToFile()
        {
            const string imageName = "screenshot";
            const string errorMessage = "there was an error";

            var dateTime = new DateTime(2014, 05, 11, 10, 29, 33);
            var fileName = string.Format(@"{0}\{1}{2}.png", CameraFolderPath, imageName, dateTime.ToString("yyyy-MM-dd_HH-mm-ss"));

            using (new TestableSystemTime(dateTime))
            {
                Assert.Throws<SelenoException>(() => { _host.Application.TakeScreenshotAndThrow(imageName, errorMessage); }, errorMessage);
                File.Exists(fileName).Should().BeTrue();
            }
        }

    }
}
