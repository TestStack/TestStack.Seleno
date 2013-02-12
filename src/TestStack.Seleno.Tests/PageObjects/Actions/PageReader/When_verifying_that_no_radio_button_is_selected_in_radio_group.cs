using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_verifying_that_no_radio_button_is_selected_in_radio_group : PageReaderSpecification
    {
        private bool _result;

        public void Given_a_radio_group_has_no_selected_radio_button()
        {
            SubstituteFor<IElementFinder>()
                .TryFindElement(Arg.Any<By.jQueryBy>(), Arg.Any<int>())
                .Returns(null as IWebElement);
        }
        
        public void When_getting_selected_radio_button()
        {
            _result = SUT.HasSelectedRadioButtonInRadioGroup(x => x.Choice);
        }

        public void Then_it_should_throw_NoSuchElementException()
        {
            _result.Should().BeFalse();
        }
    }
}