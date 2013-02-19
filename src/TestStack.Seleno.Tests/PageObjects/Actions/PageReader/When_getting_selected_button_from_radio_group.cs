using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Components;
using TestStack.Seleno.Tests.TestObjects;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_selected_button_from_radio_group : PageReaderSpecification
    {
        private ChoiceType _result;
        private By.jQueryBy _actualJqueryBy;
        private IWebElement _selectedRadioButton;
        private ISelectableHtmlControl _radioButtonGroup;

        public void Given_a_radio_group_has_a_selected_radio_button()
        {
            //_selectedRadioButton = SubstituteFor<IWebElement>();

            //SubstituteFor<IElementFinder>()
            //   .TryFindElement(Arg.Any<By.jQueryBy>(), Arg.Any<int>())
            //   .Returns(_selectedRadioButton);

            //SubstituteFor<IElementFinder>()
            //    .WhenForAnyArgs(x => x.TryFindElement(Arg.Any<By.jQueryBy>(), Arg.Any<int>()))
            //    .Do(c => _actualJqueryBy = (By.jQueryBy)c.Args()[0]);

            //_radioButtonGroup = SubstituteFor<ISelectableHtmlControl>();

            //_radioButtonGroup.SelectedElementAs<ChoiceType>().Returns(ChoiceType.Another);

            SubstituteFor<IComponentFactory>()
              .HtmlControlFor<ISelectableHtmlControl>(Arg.Any<LambdaExpression>(), Arg.Any<int>())
              .Returns(_radioButtonGroup);
        }

        public void AndGiven_the_selected_radio_button_has_a_value()
        {
            SubstituteFor<IWebElement>().GetAttribute(Arg.Any<string>()).Returns(ChoiceType.Another.ToString());
        }

        public void When_getting_selected_radio_button()
        {
            _result = SUT.SelectedButtonInRadioGroup(x => x.Choice);
        }

        //public void Then_it_should_retrieve_the_selected_button()
        //{
        //    //_actualJqueryBy.Selector.Should().Contain("$('input[type=radio][name=Choice]:checked')");
        //}

        public void AndThen_it_should_get_the_value_of_the_selected_button()
        {
            //SubstituteFor<IWebElement>().Received().GetAttribute("value");
            _radioButtonGroup.Received().SelectedElementAs<ChoiceType>();
        }

        //public void AndThen_the_selected_value_should_be_casted_to_the_drop_down_property_type()
        //{
        //    _result.Should().Be(ChoiceType.Another);
        //}
    }
}