using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_retrieving_multiline_content_of_textarea : HtmlControlSpecificationFor<TextArea>
    {
        private string _result;
        private IWebElement _textAreaElement;
        public When_retrieving_multiline_content_of_textarea() : base(x => x.MultiLineContent) { }
        private const string MultilineContent = "line 1\r\nline 2";

        public void Given_a_TextArea_has_multi_Lines_of_content()
        {
            _textAreaElement = SubstituteFor<IWebElement>();

            _textAreaElement
                .GetAttribute("value")
                .Returns(MultilineContent);

            ElementFinder
                .Element(Arg.Any<By>())
                .Returns(_textAreaElement);
            
        }

        public void When_retrieving_the_textarea_content()
        {
            _result = SUT.Content;
        }

        public void AndThen_it_should_return_the_content_lines_in_the_correct_order()
        {
            _result.Should().Be(MultilineContent);
        }
    }
}
