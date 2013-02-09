using System;
using FluentAssertions;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestInfrastructure;

namespace TestStack.Seleno.Tests.Configuration
{
    internal abstract class WithAppConfigurator : SpecificationFor<AppConfigurator>
    {
        protected ISelenoApplication Application;

        public override void InitialiseSystemUnderTest()
        {
            SUT = TestObjectFactory.TestableAppConfigurator();
        }
    }

    class when_creating_default_Application_with_no_overrides : WithAppConfigurator
    {
        public when_creating_default_Application_with_no_overrides()
        {
            Application = SUT.CreateApplication();
        }

        public void Then_it_should_create_the_application()
        {
            Application.Should().NotBeNull();
        }

        public void AndThen_it_should_be_a_SelenoApplication()
        {
            Application.Should().BeOfType<SelenoApplication>();
        }
    }
}
