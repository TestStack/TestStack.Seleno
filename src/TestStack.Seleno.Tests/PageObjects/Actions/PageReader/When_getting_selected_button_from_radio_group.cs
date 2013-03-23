using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_selected_button_from_radio_group : PageReaderSpecification
    {
        private RadioButtonGroup _radioButtonGroup;
        private readonly Expression<Func<TestViewModel, ChoiceType>> _radioGroupPropertySelector = x => x.Choice;
        private IComponentFactory _componentFactory;

        public void Given_a_radio_group_has_a_selected_radio_button()
        {

            _componentFactory = SubstituteFor<IComponentFactory>();
            _radioButtonGroup = Substitute.For<RadioButtonGroup>();

            _componentFactory
                .HtmlControlFor<RadioButtonGroup>(_radioGroupPropertySelector)
                .Returns(_radioButtonGroup);

            
            _radioButtonGroup.SelectedElementAs<ChoiceType>().Returns(ChoiceType.Another);
        }

        public void When_getting_selected_radio_button()
        {
            SUT.SelectedButtonInRadioGroup(_radioGroupPropertySelector);
        }

        public void Then_the_component_factory_should_retrieve_the_radio_button_group_control()
        {
            _componentFactory
                .Received()
                .HtmlControlFor<RadioButtonGroup>(_radioGroupPropertySelector);
        }

        public void AndThen_the_radio_button_group_was_retrieved_the_value_of_its_selected_element()
        {
            _radioButtonGroup.Received().SelectedElementAs<ChoiceType>();
        }
    }
}