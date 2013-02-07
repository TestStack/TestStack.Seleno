using System;
using FluentAssertions;
using NSubstitute;
using TestStack.Seleno.Configuration.Contracts;
using SUT = TestStack.Seleno.Configuration.SelenoHost;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    public class When_running_Seleno_Application_with_configure_action : SelenoHostSpecification
    {
        private static bool _configureActionHasBeenInvoked;
        private Action<IAppConfigurator> _configureAction = c => { _configureActionHasBeenInvoked = true; };

        public void Given_an_configure_action_on_the_configurator_is_set_up()
        {
            _configureAction = c => { _configureActionHasBeenInvoked = true; };
        }
        
        public void When_running_Seleno_Application()
        {
            SUT.Run(_configureAction,AppConfigurator);
        }

        public void Then_it_should_invoke_configure_action()
        {
            _configureActionHasBeenInvoked.Should().BeTrue();
        }

        public void AndThen_it_should_Create_a_Seleno_Application()
        {
            AppConfigurator.Received().CreateApplication();
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
