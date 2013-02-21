using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Specialized;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_attempting_to_retrieve_selected_button_from_radio_group_with_none_selected : PageReaderSpecification
    {
        private Action _selectedButtonInRadioGroupAction;
        private ExceptionAssertions<NoSuchElementException> _exceptionThrown;
        private readonly Expression<Func<TestViewModel, Object>> _radioButtonGroupSelector = viewModel => viewModel.Choice;

        public void Given_a_radio_group_has_no_selected_radio_button()
        {
            HtmlControl<RadioButtonGroup>(_radioButtonGroupSelector);

            ElementFinder
                .TryFindElement(Arg.Any<By.jQueryBy>(), Arg.Any<int>())
                .Returns(null as IWebElement);
        }

        public void When_getting_selected_radio_button()
        {
            _selectedButtonInRadioGroupAction = () => SUT.SelectedButtonInRadioGroup(_radioButtonGroupSelector);
        }

        public void Then_it_should_throw_NoSuchElementException()
        {
            _exceptionThrown =_selectedButtonInRadioGroupAction.ShouldThrow<NoSuchElementException>();
        }

        public void AndThen_exception_message_should_be_No_selected_radio_button_has_been_found()
        {
            _exceptionThrown.WithMessage("No selected element has been found");
        }
    }
}