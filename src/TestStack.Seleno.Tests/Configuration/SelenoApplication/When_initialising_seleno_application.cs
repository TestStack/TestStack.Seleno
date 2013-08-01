using NSubstitute;

namespace TestStack.Seleno.Tests.Configuration.SelenoApplication
{
    class When_initialising_seleno_application : SelenoApplicationSpecification
    {
        public void When_initialising_application()
        {
            SUT.Initialize();
        }

        public void Then_navigate_browser_to_site_home()
        {
            WebDriver.Navigate().Received().GoToUrl(SUT.WebServer.BaseUrl);
        }

        public void And_start_webserver()
        {
            WebServer.Received().Start();
        }
    }
}
