using System;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_getting_a_strongly_typed_textBox_value : SpecificationFor<IWebElement>
    {
        public When_getting_a_strongly_typed_textBox_value()
        {
            SUT.GetValueFromTextBox<DateTime>();
        }

        public void Then_it_should_get_the_value_of_the_textbox_value_attribute()
        {
            SUT.Received().GetAttributeAsType<DateTime>("value");
        }
    }
}