using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_whether_a_checkbox_is_ticked : PageReaderSpecification
    {
        private CheckBox _checkBox;
        private IComponentFactory _componentFactory;
        private readonly Expression<Func<TestViewModel, bool>> _propertySelector = x => x.Exists;
        private bool _result;

        public void Given_a_drop_down_has_a_selected_option()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _checkBox = Substitute.For<CheckBox>();

            _componentFactory
                .HtmlControlFor<CheckBox>(_propertySelector)
                .Returns(_checkBox);
        }

        public void When_getting_the_selected_option_value()
        {
            _result = SUT.CheckBoxValue(_propertySelector);
        }

        public void Then_the_component_factory_should_retrieve_the_checkbox_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<CheckBox>(_propertySelector);
        }

        public void AndThen_the_radio_button_group_was_retrieved_the_value_of_its_selected_element()
        {
            _result.Should().BeFalse();
        }
    }
}