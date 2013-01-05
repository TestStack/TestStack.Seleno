using System;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_getting_a_strongly_typed_textBox_value
    {

        private static readonly IWebElement _webElement = Substitute.For<IWebElement>();

        public When_getting_a_strongly_typed_textBox_value()
        {
            _webElement.GetValueFromTextBox<DateTime>();
        }

        
        [Test]
        public void Then_it_should_get_the_value_of_the_textbox_value_attribute()
        {
            _webElement.Received().GetAttributeAsType<DateTime>("value");
        }
    }
}