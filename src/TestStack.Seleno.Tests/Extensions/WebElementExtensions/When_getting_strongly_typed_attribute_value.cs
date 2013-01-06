using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;
using TestStack.Seleno.Tests.Specify;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_getting_strongly_typed_attribute_value : SpecificationFor<IWebElement>
    {
        private bool? _result;
        private const string AttributeName = "myAttributeName";

        public When_getting_strongly_typed_attribute_value()
        {
            SUT.GetAttribute(AttributeName).Returns("false");
            _result = SUT.GetAttributeAsType<bool?>(AttributeName);
        }

        public void Then_it_should_return_the_strongly_typed_value_of_the_attribute()
        {
            _result.Should().BeFalse();
        }

    }
}