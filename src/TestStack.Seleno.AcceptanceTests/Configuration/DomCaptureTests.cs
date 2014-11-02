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
    [Explicit]
    class DomCaptureTests
    {
        private SelenoHost _host;
        private const string DomCaptureFolderPath = @"c:\domcapture";

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _host = new SelenoHost();
            _host.Run(x => x
                .UsingDomCapture(DomCaptureFolderPath)
                .WithWebServer(new InternetWebServer("http://www.google.com/"))
                .WithMinimumWaitTimeoutOf(TimeSpan.FromMilliseconds(100))
                .UsingLoggerFactory(new ConsoleFactory(LoggerLevel.Debug))
            );
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _host.Dispose();
        }

        [Test]
        [ExpectedException(typeof(SelenoReceivedException))]
        public void CaptureDomFromElementFinder()
        {
            _host.NavigateToInitialPage<NonExistentTestPage>()
                .NavigateToNonExistentPageWithElementFinder();
        }

        [Test]
        [ExpectedException(typeof(SelenoReceivedException))]
        public void CaptureDomFromPageNavigator()
        {
            _host.NavigateToInitialPage<NonExistentTestPage>()
                .NavigateToNonExistentPageWithPageNavigator();
        }

        [Test]
        public void CaptureDomFromSelenoApplicationThrowsAndSavesDomToFile()
        {
            const string captureName = "page";
            const string errorMessage = "there was an error";

            var dateTime = new DateTime(2014, 05, 11, 10, 29, 33);
            var fileName = string.Format(@"{0}\{1}{2}.html", DomCaptureFolderPath, captureName, dateTime.ToString("yyyy-MM-dd_HH-mm-ss"));

            using (new TestableSystemTime(dateTime))
            {
                Action result = () => _host.Application.CaptureDomAndThrow(captureName, errorMessage);

                result.ShouldThrow<SelenoException>()
                    .WithMessage(errorMessage);
                File.Exists(fileName).Should().BeTrue();
            }
        }
    }
}
