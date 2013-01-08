using NSubstitute;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_setting_an_attribute_value_for_a_web_element_wrapping_the_web_driver
        : SpecificationFor<IWebElement>.Implementing<IWrapsDriver,IWebDriver,IJavaScriptExecutor>
    {
        private const string AttributeName = "data-myAttribute";
        private const string AttributeValue = "someValue";

        public void Given_a_web_element_wrapping_the_web_driver_that_implements_JavaScriptExecutor()
        {
            SUTAsFirstImplementation.WrappedDriver.Returns(SUTAsSecondImplementation);
        }

        public void When_setting_an_attribute_value_for_a_web_element()
        {
            SUT.SetAttribute(AttributeName, AttributeValue);
        }

        public void Then_it_should_execute_javascript_to_set_attribute()
        {
            SUTAsThirdImplementation
                .Received(1)
                .ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2])",
                               SUT,
                               AttributeName, AttributeValue);

        }
    }
}