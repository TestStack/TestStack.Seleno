using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    [TestFixture]
    public class When_setting_an_attribute_value_for_a_web_element_not_wrapping_the_web_driver
    {
        private readonly IWebElement _webElement = Substitute.For<IWebElement>();
        private readonly Action _settingAnAttributeForWebElementNotWrappingWebDriver;
        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue";

        public When_setting_an_attribute_value_for_a_web_element_not_wrapping_the_web_driver()
        {
            _settingAnAttributeForWebElementNotWrappingWebDriver = 
                () => _webElement.SetAttribute(AttributeName, AttributeValue);
        }


        [Test]
        public void Then_it_should_throw_an_ArgumentException_with_message_execute_Element_must_wrap_a_web_driver()
        {
            _settingAnAttributeForWebElementNotWrappingWebDriver
                .ShouldThrow<ArgumentException>()
                .WithMessage("element\r\nParameter name: Element must wrap a web driver");
        }
    }
}