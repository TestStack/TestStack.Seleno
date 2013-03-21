using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageWriter
{
    class When_updating_multiline_content_of_a_textarea : PageWriterSpecification
    {
        private TextArea _textArea;
        private IComponentFactory _componentFactory;
        private readonly Expression<Func<TestViewModel, string>> _propertySelector = x => x.MultiLineContent;
        private const string _newMultiLineContent = "something on first line\nsomething else on another line";

        public void Given_a_text_area_contains_multiple_lines_of_content()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _textArea = Substitute.For<TextArea>();

            _componentFactory
                .HtmlControlFor<TextArea>(_propertySelector)
                .Returns(_textArea);
        }

        public void When_getting_the_textarea_content()
        {
            SUT.UpdateTextAreaContent(_propertySelector, _newMultiLineContent);
        }

        public void Then_the_component_factory_should_retrieve_the_textarea_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<TextArea>(_propertySelector);
        }

        public void AndThen_the_textarea_multi_lines_content_was_retrieved()
        {
            _textArea.Received().Content = _newMultiLineContent;

        }
    }
}