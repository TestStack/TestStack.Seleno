using System.ComponentModel;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_replacing_input_value_with_another
    {
        private static IWebDriver _webDriver = Substitute.For<IWebDriver, IJavaScriptExecutor>();
        private static IJavaScriptExecutor _javaScriptExecutor = (IJavaScriptExecutor) _webDriver;

        public When_replacing_input_value_with_another()
        {
            _webDriver.ReplaceInputValueWith("#myInput", "something fresh");
        }

        [Test]
        public void Then_it_should_execute_jquery_to_set_the_input_value()
        {

            _javaScriptExecutor
                .Received()
                .ExecuteScript("$('#myInput').val('something fresh')");
        }
    }
}