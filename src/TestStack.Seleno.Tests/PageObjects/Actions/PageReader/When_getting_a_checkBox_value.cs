using FluentAssertions;
using NSubstitute;
using OpenQA.Selenium;

namespace TestStack.Seleno.Tests.PageObjects.Actions.PageReader
{
    public class When_getting_a_checkBox_value : PageReaderSpecification
    {
        private bool _result;

        public void Given_there_is_a_Checkbox()
        {
            Fake<IWebDriver>().FindElement(Arg.Any<By>()).Returns(Fake<IWebElement>());
        }

        public void AndGiven_that_Checkbox_is_ticked()
        {
            Fake<IWebElement>().Selected.Returns(true);
        }

        public void When_getting_the_checkBox_value()
        {
            _result = SUT.CheckBoxValue(x => x.Exists);
        }

        public void Then_it_should_find_the_check_box_by_its_name()
        {
            Fake<IWebDriver>().Received().FindElement(By.Name("Exists"));
        }

        public void AndThen_it_should_return_true()
        {
            _result.Should().BeTrue();
        }
    }
}
