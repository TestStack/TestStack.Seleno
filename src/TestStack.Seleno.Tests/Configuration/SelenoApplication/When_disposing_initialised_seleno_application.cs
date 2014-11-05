using NSubstitute;
using Received = NSubstitute.Received;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_disposing_initialised_seleno_application : SelenoApplicationSpecification
    {
        public void Given_initialised_seleno_application()
        {
            SUT.Initialize();
        }

        public void When_disposing_application()
        {
            SUT.Dispose();
        }

        public void Then_dispose_browser_and_webserver_followed_by_container()
        {
            Received.InOrder(() => {
                WebDriver.Received().Close();
                WebServer.Received().Stop();
                ContainerDisposal.Received().Dispose();
            });
        }
    }
}
