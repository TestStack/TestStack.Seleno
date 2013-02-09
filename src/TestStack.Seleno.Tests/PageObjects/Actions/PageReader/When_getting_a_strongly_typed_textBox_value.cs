using System;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    public class When_getting_a_strongly_typed_textBox_value : PageReaderSpecification
    {
        private Boolean _result;

        public void Given_a_web_element_has_an_attribute_data_value()
        {
            SubstituteFor<IElementFinder>().TryFindElement(Arg.Any<By>()).Returns(SubstituteFor<IWebElement>());

        }

        public void AndGiven_the_web_element_attribute_value_is_false()
        {
            SubstituteFor<IWebElement>().GetAttribute("value").Returns("true");
        }
        
        public void When_getting_the_textbox_value_for_matching_view_model_property()
        {
            _result = SUT.GetValueFromTextBox(viewModel => viewModel.Exists);
        }

        public void Then_it_should_get_the_value_of_the_textbox_value_attribute()
        {
            _result.Should().BeTrue();
        }
    }
}