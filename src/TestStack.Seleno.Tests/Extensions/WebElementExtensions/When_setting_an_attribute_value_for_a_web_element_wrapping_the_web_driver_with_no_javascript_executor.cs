using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    [TestFixture]
    public class When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver_with_no_javascript_executor
    {
        private readonly IWebElement _webElement = Substitute.For<IWebElement, IWrapsDriver>();
        private readonly IWebDriver _webDriver = Substitute.For<IWebDriver>();
        private readonly Action _settingAnAttributeForWebElementNotWrappingAWebDriver;

        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue";

        [SetUp]
        public void Given_the_Web_element_wraps_the_web_driver_which_has_no_javascript_executor()
        {
            ((IWrapsDriver)_webElement).WrappedDriver.Returns(_webDriver);
        }

        public When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver_with_no_javascript_executor()
        {
            _settingAnAttributeForWebElementNotWrappingAWebDriver =
                () => _webElement.SetAttribute(AttributeName, AttributeValue);
        }

        
        [Test]
        public void Then_it_should_throw_an_ArgumentException_with_message_Element_must_wrap_a_web_driver_that_supports_javascript_execution()
        {
            _settingAnAttributeForWebElementNotWrappingAWebDriver
                .ShouldThrow<ArgumentException>()
                .WithMessage("element\r\nParameter name: Element must wrap a web driver that supports javascript execution");
        }
    }
}