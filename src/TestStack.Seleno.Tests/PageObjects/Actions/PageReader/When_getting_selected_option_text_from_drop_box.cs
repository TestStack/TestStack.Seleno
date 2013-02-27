using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_selected_option_text_from_drop_box : PageReaderSpecification
    {
        private IDropDown _dropDown;
        private IComponentFactory _componentFactory;
        private readonly Expression<Func<TestViewModel, int>> _dropDownPropertySelector = x => x.Item;
        private string _result;

        public void Given_a_drop_down_has_a_selected_option()
        {
            _componentFactory = SubstituteFor<IComponentFactory>();
            _dropDown = SubstituteFor<IDropDown>();

            _componentFactory
                .HtmlControlFor<IDropDown>(_dropDownPropertySelector, Arg.Any<int>())
                .Returns(_dropDown);

            _dropDown.SelectedElementText.Returns("Selected option....");
        }

        public void When_getting_the_selected_option_text()
        {
            _result = SUT.SelectedOptionTextInDropDown(_dropDownPropertySelector);
        }

        public void Then_the_component_factory_should_retrieve_the_radio_button_group_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<IDropDown>(_dropDownPropertySelector, 0);
        }

        public void AndThen_the_radio_button_group_was_retrieved_the_text_of_its_selected_option()
        {
            _result.Should().Be("Selected option....");
        }
    }
}