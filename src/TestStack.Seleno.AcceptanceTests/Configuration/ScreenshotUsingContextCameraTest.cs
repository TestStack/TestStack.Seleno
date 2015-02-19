using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Test.VisualVerification;
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

        [Test]
        public void TakeScreenshotFromSelenoApplicationThrowsAndSavesScreenshotToCompare()
        {
            const string imageName = "ContextFileCamera";
            const string errorMessage = "there was an error";

            var dateTime = new DateTime(2014, 05, 11, 10, 29, 33);
            var fileName = string.Format(@"{0}\{1}{2}.png", CameraFolderPath, imageName, dateTime.ToString("yyyy-MM-dd_HH-mm-ss"));
            var fileNameDifference = string.Format(@"{0}\{1}{2}_diff.png", CameraFolderPath, imageName, dateTime.ToString("yyyy-MM-dd_HH-mm-ss"));

            using (new TestableSystemTime(dateTime))
            {
                Host.Instance.Application.SetBrowserWindowSize(800, 600);

                Action result = () => _host.Application.TakeScreenshotAndThrow(imageName, errorMessage);
                result.ShouldThrow<SelenoException>()
                    .WithMessage(errorMessage);

                File.Exists(fileName).Should().BeTrue();
                var actual = Snapshot.FromFile(fileName);

                // Get expected image from resources
                var manifestResourceStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("TestStack.Seleno.AcceptanceTests.Resouces.ContextFileCamera2014-05-11_10-29-33.png");
                var bitmap = new Bitmap(manifestResourceStream);

                var expected = Snapshot.FromBitmap(bitmap);

                // This operation creates a difference image. Any regions which are identical in 
                // the actual and master images appear as black. Areas with significant 
                // differences are shown in other colors.
                var difference = actual.CompareTo(expected);

                // Verify
                var verifier = new SnapshotColorVerifier(Color.Black, new ColorDifference());
                if (verifier.Verify(difference) == VerificationResult.Fail)
                {
                    difference.ToFile(fileNameDifference, ImageFormat.Png);
                    Assert.Fail("Image was different");
                }
            }
        }

    }
}
