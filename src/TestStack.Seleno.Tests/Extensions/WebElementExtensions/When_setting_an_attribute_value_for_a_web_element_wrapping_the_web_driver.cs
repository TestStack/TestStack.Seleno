using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    [TestFixture]
    public class When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver
    {
        private static readonly IWebDriver _webDriver = Substitute.For<IWebDriver, IJavaScriptExecutor>();
        private readonly IWebElement _webElement = Substitute.For<IWebElement, IWrapsDriver>();
        private readonly IJavaScriptExecutor _javaScriptExecutor = (IJavaScriptExecutor) _webDriver;
        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue";

        public When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver()
        {
            ((IWrapsDriver)_webElement).WrappedDriver.Returns(_webDriver);

            _webElement.SetAttribute(AttributeName, AttributeValue);
        }
        
        [Test]
        public void Then_it_should_execute_javascript_to_set_attribute()
        {
            _javaScriptExecutor
                .Received(1)
                .ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])",
                               _webElement,
                               AttributeName, AttributeValue);

        }
    }
}