using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Actions;
using TestStack.Seleno.PageObjects.Controls;
using TestStack.Seleno.Tests.TestObjects;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    class When_getting_selected_option_text_from_drop_box : PageReaderSpecification
    {
        private string _result;
        private IWebElement _selectedOption;
        private By.jQueryBy _actualJqueryBy;
        private const string ExpectedOptionText = "Selected option";
        private readonly Expression<Func<TestViewModel, Object>> _dropDownSelector = viewModel => viewModel.Item;
        private DropDown _dropDown;

        public void Given_a_drop_down_has_a_selected_option()
        {
            _selectedOption = SubstituteFor<IWebElement>();

            _dropDown =HtmlControl<DropDown>(_dropDownSelector);
 
            ElementFinder
                .ElementWithWait(Arg.Any<By.jQueryBy>(), Arg.Any<int>())
                .Returns(_selectedOption);

            ElementFinder
                .WhenForAnyArgs(x => x.ElementWithWait(Arg.Any<By.jQueryBy>(), Arg.Any<int>()))
                .Do(c => _actualJqueryBy = (By.jQueryBy)c.Args()[0]);
        }

        public void AndGiven_the_selected_option_has_text()
        {
            _selectedOption
                .Text
                .Returns(ExpectedOptionText);
        }

        public void When_getting_the_selected_option_text()
        {
            _result = SUT.SelectedOptionTextInDropDown(x => x.Item);
        }

        public void Then_it_should_retrieve_the_selected_option()
        {
            _actualJqueryBy.Selector.Should().Contain("$('#Item option:selected')");
        }
        
        public void AndThen_the_selected_value_should_be_casted_to_the_drop_down_property_type()
        {
            _result.Should().Be(ExpectedOptionText);
        }
    }
}