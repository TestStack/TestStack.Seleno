                        using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver_with_no_javascript_executor 
        : SpecificationFor<IWebElement>.Implementing<IWrapsDriver>
    {                                         
        private IWebDriver _webDriver;                 
        private readonly Action _settingAnAttributeForWebElementNotWrappingAWebDriver;

        public void Given_the_Web_element_wraps_the_web_driver_which_has_no_javascript_executor()
        {
            _webDriver =  Fake<IWebDriver>();
            SUTAsFirstImplementation.WrappedDriver.Returns(_webDriver);
        }

        public When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver_with_no_javascript_executor()
        {
            _settingAnAttributeForWebElementNotWrappingAWebDriver =
                () => SUT.SetAttribute("data-myAttribute", "someValue");
        }

        
        public void Then_it_should_throw_an_ArgumentException_with_message_Element_must_wrap_a_web_driver_that_supports_javascript_execution()
        {
            _settingAnAttributeForWebElementNotWrappingAWebDriver
                .ShouldThrow<ArgumentException>()
                .WithMessage("element\r\nParameter name: Element must wrap a web driver that supports javascript execution");
        }
    }
}