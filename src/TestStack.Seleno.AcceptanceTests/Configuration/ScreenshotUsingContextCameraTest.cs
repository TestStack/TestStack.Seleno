using System;
using System.IO;
using Castle.Core.Logging;
using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.AcceptanceTests.PageObjects;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Interceptors;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{

    class ScreenshotUsingContextCameraTest
    {
        private SelenoHost _host;
        private const string CameraFolderPath = @"c:\screenshots";

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _host = new SelenoHost();
            _host.Run(x => x
                .UsingCamera(new ContextFileCamera(CameraFolderPath))
                .ProjectToTest(new WebApplication(ProjectLocation.FromFolder("TestStack.Seleno.AcceptanceTests.Web"), 12324))
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
            _host.NavigateToInitialPage<NonExistentTestPage>()
                .NavigateToNonExistentPageWithElementFinder();
        }

        [Test]
        [ExpectedException(typeof(SelenoReceivedException))]
        public void TakeScreenshotFromPageNavigator()
        {
            _host.NavigateToInitialPage<NonExistentTestPage>()
                .NavigateToNonExistentPageWithPageNavigator();
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
                Action result = () => _host.Application.TakeScreenshotAndThrow(imageName, errorMessage);

                result.ShouldThrow<SelenoException>()
                    .WithMessage(errorMessage);
                File.Exists(fileName).Should().BeTrue();
            }
        }

    }
}
