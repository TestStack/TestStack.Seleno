using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_retrieving_multiLine_content_of_TextArea : HtmlControlSpecificationFor<TextArea>
    {
        private string[] _result;
        private IWebElement _textAreaElement;
        public When_retrieving_multiLine_content_of_TextArea() : base(x => x.MultiLineContent) { }

        public void Given_a_TextArea_has_multi_Lines_of_content()
        {
            _textAreaElement = SubstituteFor<IWebElement>();

            _textAreaElement
                .Text
                .Returns("line 1\r\nline 2");

            ElementFinder
                .ElementWithWait(Arg.Any<By>())
                .Returns(_textAreaElement);
            
        }

        public void When_retrieving_the_TextArea_content()
        {
            _result = SUT.MultiLineContent;
        }

        public void AndThen_it_should_return_the_content_lines_in_the_correct_order()
        {
            _result.Should().ContainInOrder(new [] {"line 1", "line 2"});
        }
    }
}
