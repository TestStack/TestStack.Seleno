using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OpenQA.Selenium;
using TestStack.Seleno.Extensions;

namespace TestStack.Seleno.Tests.Extensions.WebElementExtensions
{
    public class When_getting_strongly_typed_attribute_value
    {
        private static readonly IWebElement _webElement = Substitute.For<IWebElement>();
        private bool? _result;
        private const string AttributeName = "myAttributeName";


        public When_getting_strongly_typed_attribute_value()
        {
            _webElement.GetAttribute(AttributeName).Returns("false");
            _result = _webElement.GetAttributeAsType<bool?>(AttributeName);
        }

        [Test]
        public void Then_it_should_return_the_strongly_typed_value_of_the_attribute()
        {
            _result.Should().BeFalse();
        }

    }
}