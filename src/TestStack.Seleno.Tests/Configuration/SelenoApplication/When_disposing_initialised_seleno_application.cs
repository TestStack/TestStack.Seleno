using NSubstitute;

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

        public void Then_dispose_container()
        {
            ContainerDisposal.Received().Dispose();
        }

        public void And_close_browser()
        {
            WebDriver.Received().Close();
        }

        public void And_stop_webserver()
        {
            WebServer.Received().Stop();
        }
    }
}
