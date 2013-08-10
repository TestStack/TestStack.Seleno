using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using TestStack.Seleno.Configuration;
using TestStack.Seleno.Tests.Specify;
using FluentAssertions;

namespace TestStack.Seleno.Tests.Configuration
{
    abstract class BrowserFactorySpecification : Specification
    {
        public override void EstablishContext()
        {
            Title = "Browser Factory";
        }
    }

    class DriverResourceNotEmbeddedTest : BrowserFactorySpecification
    {
        private Exception _caughtException;
        public void Given_internet_explorer_web_driver_is_not_embedded_in_the_current_assembly() {}

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

        public void And_exception_should_tell_user_what_file_to_embed()
        {
            _caughtException.Message.Should().Be("Could not find configured web driver; you need to embed an executable with the filename IEDriverServer.exe.");
        }
    }

    class OutputEmbeddedResourceFirstTime : BrowserFactorySpecification
    {
        private Exception _caughtException;
        private const string DriverExe = "chromedriver.exe";

        public void Given_chrome_web_driver_is_embedded_in_the_current_assembly()
        {
            Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Single(n => n.EndsWith(DriverExe));
        }

        public void AndGiven_chrome_driver_isnt_on_disk()
        {
            File.Delete(DriverExe);
        }

        public void When_creating_chrome_web_driver()
        {
            try
            {
                BrowserFactory.Chrome();
            }
            catch (Exception e)
            {
                _caughtException = e;
            }
        }

        public void Then_the_chrome_driver_should_be_written_to_disk()
        {
            File.Exists(DriverExe).Should().BeTrue();
        }

        public void And_the_contents_of_the_file_should_be_correct()
        {
            File.ReadAllText(DriverExe).Should().Be("chrome");
        }

        public void And_the_driver_should_have_been_attempted_to_be_loaded()
        {
            _caughtException.Should().BeOfType<Win32Exception>();
            _caughtException.Message.Should().Be("The specified executable is not a valid application for this OS platform.");
        }
    }
}
