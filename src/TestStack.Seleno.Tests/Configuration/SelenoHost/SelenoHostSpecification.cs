using NSubstitute;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    public abstract class SelenoHostSpecification : Specification
    {
        internal readonly IInternalAppConfigurator AppConfigurator = Substitute.For<IInternalAppConfigurator>();
        protected readonly ISelenoApplication SelenoApplication = Substitute.For<ISelenoApplication>();
        
        protected SelenoHostSpecification()
        {
            Seleno.Configuration.SelenoHost.AppConfigurator = () => AppConfigurator;
            AppConfigurator.CreateApplication().Returns(SelenoApplication);
        }
    }
}
