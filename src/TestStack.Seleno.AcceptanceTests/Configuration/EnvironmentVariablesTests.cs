using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.AcceptanceTests.PageObjects;
using TestStack.Seleno.AcceptanceTests.Web.App_Start;
using TestStack.Seleno.Configuration;

namespace TestStack.Seleno.AcceptanceTests.Configuration
{
    class EnvironmentVariablesTests
    {
        private SelenoHost _host;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _host = new SelenoHost();
            _host.Run("TestStack.Seleno.AcceptanceTests.Web", 12324, x => 
                x.WithRouteConfig(RouteConfig.RegisterRoutes)
                 .WithEnvironmentVariable("FunctionalTest", "SomeVal"));
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            _host.Dispose();
        }

        [Test]
        public void CanInjectEnvironmentVariables()
        {
            var homepage = _host.NavigateToInitialPage<HomePage>();
            homepage = homepage.SelectCheckingEnvironmentVariables();

            homepage.Title.Should().Be("Home");
        }

    }
}
