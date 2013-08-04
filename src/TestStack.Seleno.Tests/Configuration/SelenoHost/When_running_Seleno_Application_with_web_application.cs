using System;
using FluentAssertions;
using NSubstitute;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Configuration.WebServers;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    class When_running_Seleno_Application_with_web_application : SelenoHostSpecification
    {
        private static bool _configureActionHasBeenInvoked;
        private Action<IAppConfigurator> _configureAction = c => { _configureActionHasBeenInvoked = true; };
        private readonly WebApplication _webApplication = new WebApplication(Substitute.For<IProjectLocation>(), 80);

        public void Given_an_configure_action_on_the_configurator_is_set_up()
        {
            _configureAction = c => { _configureActionHasBeenInvoked = true; };
        }

        public void When_running_Seleno_Application()
        {
            SUT.Run(_webApplication, _configureAction);
        }

        public void Then_it_should_configure_Seleno_Application_with_web_application()
        {
            AppConfigurator.Received().ProjectToTest(_webApplication);
        }

        public void AndThen_it_should_Create_a_Seleno_Application()
        {
            AppConfigurator.Received().CreateApplication();
        }

        public void AndThen_it_should_invoke_configure_action()
        {
            _configureActionHasBeenInvoked.Should().BeTrue();
        }

        public void AndThen_it_should_Initialise_the_Seleno_Application()
        {
            SelenoApplication.Received().Initialize();
        }

        public void AndThen_the_Seleno_Application_is_kept()
        {
            SUT.Host.Should().BeSameAs(SelenoApplication);
        }
    }
}