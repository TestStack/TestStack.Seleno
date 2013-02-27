using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_multiline_content_of_a_textarea : PageReaderSpecification
    {
        private ITextArea _textArea;
        private IComponentFactory _componentFactory;
        private readonly Expression<Func<TestViewModel, string>> _propertySelector = x => x.MultiLineContent;
        private readonly string[] _expectedMultiLineContent = new[] { "something on first line", "something else on another line" };
        private string[] _result;

        public void Given_a_text_area_contains_multiple_lines_of_content()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _textArea = SubstituteFor<ITextArea>();
            _textArea.MultiLineContent.Returns(_expectedMultiLineContent);

            _componentFactory
                .HtmlControlFor<ITextArea>(_propertySelector, Arg.Any<int>())
                .Returns(_textArea);
        }

        public void When_getting_the_textarea_content()
        {
            _result = SUT.TextAreaContent(_propertySelector);
        }

        public void Then_the_component_factory_should_retrieve_the_textarea_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<ITextArea>(_propertySelector, 0);
        }

        public void AndThen_the_textarea_multi_lines_content_was_retrieved()
        {
            _result.Should().Equal(_expectedMultiLineContent);
        }
    }
}