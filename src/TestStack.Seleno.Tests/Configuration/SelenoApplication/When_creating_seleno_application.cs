using Castle.Core.Logging;
using FluentAssertions;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using NSubstitute;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_creating_seleno_application : SelenoApplicationSpecification
    {
        private readonly ILogger _expectedLogger = new ConsoleLogger();

        public override void InitialiseSystemUnderTest()
        {
            SubstituteFor<ILoggerFactory>()
                .Create(typeof (Seleno.Configuration.SelenoApplication))
                .Returns(_expectedLogger);

            base.InitialiseSystemUnderTest();
        }

        public void When_creating_application() {}

        public void Then_camera_should_be_retrieved_from_container()
        {
            SUT.Camera.Should().Be(SubstituteFor<ICamera>());
        }

        public void And_webserver_should_be_retrieved_from_container()
        {
            SUT.WebServer.Should().Be(SubstituteFor<IWebServer>());
        }

        public void And_webdriver_should_be_retrieved_from_container()
        {
            SUT.Browser.Should().Be(SubstituteFor<IWebDriver>());
        }

        public void And_logger_should_be_retrieved_from_container()
        {
            SUT.Logger.Should().Be(_expectedLogger);
        }
    }
}
