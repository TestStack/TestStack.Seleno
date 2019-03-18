using System;
using FluentAssertions;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Configuration
{
    internal abstract class BrowserFactorySpecification : Specification
    {
        public override void EstablishContext()
        {
            Title = "Browser Factory";
        }
    }

    internal class DriverResourceNotEmbeddedTest : BrowserFactorySpecification
    {
        private Exception _caughtException;

        public void Given_internet_explorer_web_driver_is_not_embedded_in_the_current_assembly()
        {
        }

        public void When_creating_internet_explorer_web_driver()
        {
            try
            {
                BrowserFactory.InternetExplorer();
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        public void Then_throw_web_driver_not_found_exception()
        {
            _caughtException.Should().BeOfType<WebDriverNotFoundException>();
        }

        public void And_exception_should_tell_user_what_to_do()
        {
            _caughtException.Message.Should()
                .Be("Could not find configured web driver; you need to add nuget package which adds the IEDriverServer.exe to your bin folder in your project.");
        }
    }
}