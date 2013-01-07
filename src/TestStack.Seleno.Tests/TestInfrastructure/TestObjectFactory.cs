using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using TestStack.Seleno.Infrastructure.Logging.Loggers;

using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.TestInfrastructure
{
    public static class TestObjectFactory
    {
        public static AppConfigurator TestableAppConfigurator()
        {
            var webApplication = new WebApplication(Substitute.For<IProjectLocation>(), 45123);
            var configurator = new AppConfigurator()
                .ProjectToTest(webApplication)
                .WithWebServer(Substitute.For<IWebServer>())
                .WithWebDriver(() => Substitute.For<IWebDriver>())
                .UsingCamera(new NullCamera())
                .UsingLogger(new NullLogFactory());

            return configurator;
        }
    }
}
