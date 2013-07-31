using NSubstitute;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_disposing_uninitialised_seleno_application : SelenoApplicationSpecification
    {
        public void Given_uninitialised_seleno_application() {}

        public void When_disposing_application()
        {
            SUT.Dispose();
        }

        public void Then_dispose_container()
        {
            ContainerDisposal.Received().Dispose();
        }

        public void And_dont_close_browser()
        {
            WebDriver.DidNotReceive().Close();
        }

        public void And_dont_stop_webserver()
        {
            WebServer.DidNotReceive().Stop();
        }
    }
}
