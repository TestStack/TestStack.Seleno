using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_strongly_typed_attribute_value : PageReaderSpecification
    {
        private bool? _result;
        private const string AttributeName = "data-value";

        public void Given_a_web_element_has_an_attribute_data_value()
        {
            SubstituteFor<IElementFinder>().TryFindElement(Arg.Any<By>()).Returns(SubstituteFor<IWebElement>());

        }

        public void AndGiven_the_web_element_attribute_value_is_false()
        {
            SubstituteFor<IWebElement>().GetAttribute(AttributeName).Returns("false");
        }

        public void When_getting_attribute_value_as_boolean()
        {
            _result = SUT.GetAttributeAsType(x => x.Exists,AttributeName);
        }

        public void Then_it_should_return_the_strongly_typed_value_of_the_attribute()
        {
            _result.Should().BeFalse();
        }

    }
}