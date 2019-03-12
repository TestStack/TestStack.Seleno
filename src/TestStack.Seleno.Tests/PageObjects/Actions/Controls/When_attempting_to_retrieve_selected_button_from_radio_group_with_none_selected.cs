using System;
using FluentAssertions;
using FluentAssertions.Specialized;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_attempting_to_retrieve_selected_button_from_radio_group_with_none_selected : HtmlControlSpecificationFor<RadioButtonGroup, ChoiceType>
    {
        private Action _selectedButtonInRadioGroupAction;
        private ExceptionAssertions<NoSuchElementException> _exceptionThrown;

        public When_attempting_to_retrieve_selected_button_from_radio_group_with_none_selected() 
            : base(viewModel => viewModel.Choice)
        { }

        public void Given_a_radio_group_has_no_selected_radio_button()
        {
            ElementFinder
                .OptionalElement(Arg.Any<By.jQueryBy>())
                .Returns(null as IWebElement);
        }

        public void When_getting_selected_radio_button()
        {
            _selectedButtonInRadioGroupAction = () => SUT.SelectedElementAs<ChoiceType>();
        }

        public void Then_it_should_throw_NoSuchElementException()
        {
            _exceptionThrown = _selectedButtonInRadioGroupAction.Should().Throw<NoSuchElementException>();
        }

        public void AndThen_exception_message_should_be_No_selected_radio_button_has_been_found()
        {
            _exceptionThrown.WithMessage("No selected element has been found");
        }
    }
}