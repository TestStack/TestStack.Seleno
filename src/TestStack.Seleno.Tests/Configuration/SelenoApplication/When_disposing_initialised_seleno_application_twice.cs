using NSubstitute;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_disposing_initialised_seleno_application_twice : SelenoApplicationSpecification
    {
        public void Given_initialised_and_disposed_seleno_application()
        {
            SUT.Initialize();
            SUT.Dispose();
            ClearReceivedCalls();
        }

        public void When_disposing_application()
        {
            SUT.Dispose();
        }

        public void Then_dont_dispose_container_children()
        {
            ContainerDisposal.DidNotReceive().Dispose();
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
