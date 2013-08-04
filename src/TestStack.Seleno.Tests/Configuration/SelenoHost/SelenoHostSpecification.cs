using NSubstitute;
using NUnit.Framework;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    abstract class SelenoHostSpecification : Specification
    {
        internal readonly IInternalAppConfigurator AppConfigurator = Substitute.For<IInternalAppConfigurator>();
        protected readonly ISelenoApplication SelenoApplication = Substitute.For<ISelenoApplication>();
        protected static Seleno.Configuration.SelenoHost SUT = new Seleno.Configuration.SelenoHost();

        protected SelenoHostSpecification()
        {
            SUT.AppConfiguratorFactory = () => AppConfigurator;
            AppConfigurator.CreateApplication().Returns(SelenoApplication);
        }

        [TearDown]
        public void Teardown()
        {
            SUT.Host = null;
        }
    }
}
