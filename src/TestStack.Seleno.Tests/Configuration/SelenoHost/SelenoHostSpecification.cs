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
        
        protected SelenoHostSpecification()
        {
            Seleno.Configuration.SelenoHost.AppConfiguratorFactory = () => AppConfigurator;
            AppConfigurator.CreateApplication().Returns(SelenoApplication);
        }

        [TearDown]
        public void Teardown()
        {
            Seleno.Configuration.SelenoHost.Host = null;
        }
    }
}
