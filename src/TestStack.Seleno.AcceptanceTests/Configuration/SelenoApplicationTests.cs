using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{
    [Explicit]
    class SelenoApplicationTests
    {
        private SelenoHost _host;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _host = new SelenoHost();
            _host.Run(x => x
                .WithWebServer(new InternetWebServer("http://www.google.com/"))
            );
            _host.Application.Browser.Manage().Window.Size = new Size(750,750);
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            _host.Dispose();
        }

        [Test]
        public void CanChangeWindowSize()
        {
            var oldsize = _host.Application.Browser.Manage().Window.Size;
            var newSize = new Size(500, 500);

            _host.Application.SetBrowserWindowSize(newSize.Width, newSize.Height);

            _host.Application.Browser.Manage().Window.Size.Should().Be(newSize);
            _host.Application.Browser.Manage().Window.Size.Should().NotBe(oldsize);
        }
    }
}
