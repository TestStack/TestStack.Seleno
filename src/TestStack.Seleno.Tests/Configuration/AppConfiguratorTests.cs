using FluentAssertions;
using NUnit.Framework;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Tests.Specify;
using TestStack.Seleno.Tests.TestInfrastructure;

namespace TestStack.Seleno.Tests.Configuration
{
    public abstract class WithAppConfigurator : SpecificationFor<AppConfigurator>
    {
        protected ISelenoApplication Host;

        public override void InitialiseSystemUnderTest()
        {
            SUT = TestObjectFactory.TestableAppConfigurator();
        }
    }

    public class when_creating_default_Application_with_no_overrides : WithAppConfigurator
    {
        public when_creating_default_Application_with_no_overrides()
        {
            Host = SUT.CreateApplication();
        }

        public void Then_it_should_create_the_application()
        {
            Host.Should().NotBeNull();
        }

        public void AndThen_it_should_be_a_SelenoApplication()
        {
            Host.Should().BeOfType<SelenoApplication>();
        }
    }
}
