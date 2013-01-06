using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.Tests.Configuration
{
    [TestFixture]
    public class AppConfiguratorTests
    {
        [Test]
        public void should_be_able_to_create_IisExpressWebServer()
        {
            var configurator = new AppConfigurator();
            var app = configurator.CreateApplication();
            app.Should().NotBeNull();
        }
    }
}
