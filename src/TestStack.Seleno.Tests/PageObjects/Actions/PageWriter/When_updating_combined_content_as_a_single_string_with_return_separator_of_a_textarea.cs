using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Core;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_updating_combined_content_as_a_single_string_with_return_separator_of_a_textarea : PageWriterSpecification
    {
        private ITextArea _textArea;
        private IComponentFactory _componentFactory;
        private readonly Expression<Func<TestViewModel, string>> _propertySelector = x => x.MultiLineContent;
        private const string _content = "something on first line\nsomething else on another line";
        private readonly string[] _expectedMultiLineContent = new[] { "something on first line", "something else on another line" };

        public void Given_a_text_area_contains_multiple_lines_of_content()
        {
             _textArea = SubstituteFor<ITextArea>();

            _componentFactory = SubstituteFor<IComponentFactory>();
            _componentFactory
                .HtmlControlFor<ITextArea>(_propertySelector, Arg.Any<int>())
                .Returns(_textArea);
        }

        public void When_getting_the_textarea_content()
        {
            SUT.UpdateTextAreaContent(_propertySelector, _content);
        }

        public void Then_the_component_factory_should_retrieve_the_textarea_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<ITextArea>(_propertySelector, 0);
        }

        public void AndThen_the_textarea_multi_lines_content_was_retrieved()
        {
            _textArea.MultiLineContent.Should().ContainInOrder(_expectedMultiLineContent);

        }
    }
}