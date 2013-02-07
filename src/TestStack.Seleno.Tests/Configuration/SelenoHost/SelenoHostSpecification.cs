using Funq;
using NSubstitute;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    public abstract class SelenoHostSpecification : Specification
    {
        protected readonly Container Container = new Container();
        protected readonly IPageNavigator PageNavigator = Substitute.For<IPageNavigator>();
        protected readonly IAppConfigurator AppConfigurator = Substitute.For<IAppConfigurator>();
        protected readonly ISelenoApplication SelenoApplication = Substitute.For<ISelenoApplication>();

        protected SelenoHostSpecification()
        {
            AppConfigurator.CreateApplication().Returns(SelenoApplication);
            SelenoApplication.Container.Returns(x => Container);
            Container.Register(c => PageNavigator);
        }
        
    }
}
