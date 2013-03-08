using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_a_strongly_typed_textBox_value : PageReaderSpecification
    {
        private IComponentFactory _componentFactory;
        private IInputHtmlControl _textBox;
        private readonly Expression<Func<TestViewModel, bool>> _textBoxPropertySelector = x => x.Exists;

        public void Given_a_web_element_has_an_attribute_data_value()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _textBox = SubstituteFor<ITextBox>();

            _componentFactory
                .HtmlControlFor<ITextBox>(_textBoxPropertySelector)
                .Returns(_textBox);
        }
        
        public void When_getting_the_textbox_value_for_matching_view_model_property()
        {
            SUT.GetValueFromTextBox(_textBoxPropertySelector);
        }

        public void Then_the_component_factory_should_retrieve_the_textBox_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<ITextBox>(_textBoxPropertySelector);
        }

        public void Then_it_should_get_the_value_of_the_textbox_value_attribute()
        {
            _textBox.Received().ValueAs<Boolean>();
        }
    }
}