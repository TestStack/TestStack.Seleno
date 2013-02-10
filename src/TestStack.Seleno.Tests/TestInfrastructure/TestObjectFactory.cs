using OpenQA.Selenium;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.Screenshots;
using TestStack.Seleno.Configuration.WebServers;
using NSubstitute;

namespace TestStack.Seleno.Tests.TestInfrastructure
{
    internal static class TestObjectFactory
    {
        public static AppConfigurator TestableAppConfigurator()
        {
            var webApplication = new WebApplication(Substitute.For<IProjectLocation>(), 45123);
            var configurator = new AppConfigurator();
            configurator
                .WithWebDriver(() => Substitute.For<IWebDriver>())
                .ProjectToTest(webApplication)
                .WithWebServer(Substitute.For<IWebServer>())
                .UsingCamera(new NullCamera());

            return configurator;
        }
    }
}
