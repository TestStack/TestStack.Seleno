using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_verifying_that_no_radio_button_is_selected_in_radio_group : HtmlControlSpecificationFor<RadioButtonGroup>
    {

        private bool _result;

        public When_verifying_that_no_radio_button_is_selected_in_radio_group() : base(x => x.Choice) { }

        public void Given_a_radio_group_has_no_selected_radio_button()
        {
            ElementFinder
                .TryFindElement(Arg.Any<By.jQueryBy>())
                .Returns(null as IWebElement);
        }

        public void When_getting_selected_radio_button()
        {
            _result = SUT.HasSelectedElement;
        }

        public void Then_it_should_throw_NoSuchElementException()
        {
            _result.Should().BeFalse();
        }
    }
}