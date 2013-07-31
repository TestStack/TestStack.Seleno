using System;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Configuration.Contracts;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    abstract class SelenoApplicationSpecification : SpecificationFor<Seleno.Configuration.SelenoApplication>
    {
        protected IWebServer WebServer;
        protected IWebDriver WebDriver;
        protected IDisposable ContainerDisposal;

        public override void InitialiseSystemUnderTest()
        {
            AutoSubstitute.Provide(AutoSubstitute.Container);
            base.InitialiseSystemUnderTest();
        }

        public override void EstablishContext()
        {
            WebServer = SubstituteFor<IWebServer>();
            WebDriver = SubstituteFor<IWebDriver>();
            ContainerDisposal = Substitute.For<IDisposable>();
            AutoSubstitute.Container.Disposer.AddInstanceForDisposal(ContainerDisposal);
        }

        protected void ClearReceivedCalls()
        {
            ContainerDisposal.ClearReceivedCalls();
            WebDriver.ClearReceivedCalls();
            WebServer.ClearReceivedCalls();
        }
    }
}
