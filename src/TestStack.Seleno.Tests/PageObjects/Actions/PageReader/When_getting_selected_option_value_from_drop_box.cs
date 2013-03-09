using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_selected_option_value_from_drop_box : PageReaderSpecification
    {
        private IDropDown _dropDown;
        private IComponentFactory _componentFactory;
        private readonly Expression<Func<TestViewModel, int>> _dropDownPropertySelector = x => x.Item;

        public void Given_a_drop_down_has_a_selected_option()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _dropDown = SubstituteFor<IDropDown>();

            _componentFactory
                .HtmlControlFor<IDropDown>(_dropDownPropertySelector)
                .Returns(_dropDown);
        }

        public void When_getting_the_selected_option_value()
        {
            SUT.SelectedOptionValueInDropDown(_dropDownPropertySelector);
        }

        public void Then_the_component_factory_should_retrieve_the_drop_down_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<IDropDown>(_dropDownPropertySelector);
        }

        public void AndThen_the_radio_button_group_was_retrieved_the_value_of_its_selected_element()
        {
            _dropDown.Received().SelectedElementAs<int>();
        }
    }
}