using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;
using TestStack.Seleno.PageObjects.Controls;
using By = TestStack.Seleno.PageObjects.Locators.By;

namespace TestStack.Seleno.Tests.PageObjects.Actions.Controls
{
    class When_getting_selected_option_value_from_drop_box :  HtmlControlSpecificationFor<DropDown>
    {
        private int _result;
        private IWebElement _selectedOption;
        private By.jQueryBy _actualJqueryBy;

        public When_getting_selected_option_value_from_drop_box() : base(x => x.Item) { }

        public void Given_a_drop_down_has_a_selected_option()
        {
            _selectedOption = SubstituteFor<IWebElement>();

            ElementFinder
                .Element(Arg.Any<By.jQueryBy>(), Arg.Any<int>())
                .Returns(_selectedOption);

            ElementFinder
                .WhenForAnyArgs(x => x.Element(Arg.Any<By.jQueryBy>(), Arg.Any<int>()))
                .Do(c => _actualJqueryBy = (By.jQueryBy)c.Args()[0]);
        }

        public void AndGiven_the_selected_option_has_a_value()
        {
            _selectedOption
                .GetAttribute(Arg.Any<string>())
                .Returns("5");
        }

        public void When_getting_the_selected_option()
        {
            _result = SUT.SelectedElementAs<int>();
        }

        public void Then_it_should_retrieve_the_selected_option()
        {
            _actualJqueryBy.Selector.Should().Contain("#Item option:selected");
        }

        public void AndThen_it_should_get_the_value_of_the_selected_drop_down_list_option()
        {
            _selectedOption.Received().GetAttribute("value");
        }

        public void AndThen_the_selected_value_should_be_casted_to_the_drop_down_property_type()
        {
            _result.Should().Be(5);
        }
    }
}